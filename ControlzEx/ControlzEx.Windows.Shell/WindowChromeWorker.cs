using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace ControlzEx.Windows.Shell
{
	internal class WindowChromeWorker : DependencyObject
	{
		private delegate void _Action();

		private const SWP _SwpFlags = SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER;

		private readonly List<KeyValuePair<WM, MessageHandler>> _messageTable;

		private Window _window;

		[SecurityCritical]
		private IntPtr _hwnd;

		[SecurityCritical]
		private HwndSource _hwndSource;

		private bool _isHooked;

		private bool _isFixedUp;

		private bool _isUserResizing;

		private bool _hasUserMovedWindow;

		private Point _windowPosAtStartOfUserMove;

		private WindowChrome _chromeInfo;

		private WindowState _lastRoundingState;

		private WindowState _lastMenuState;

		private bool _isGlassEnabled;

		private WINDOWPOS _previousWP;

		public static readonly DependencyProperty WindowChromeWorkerProperty = DependencyProperty.RegisterAttached("WindowChromeWorker", typeof(WindowChromeWorker), typeof(WindowChromeWorker), new PropertyMetadata(null, _OnChromeWorkerChanged));

		private static readonly HT[,] _HitTestBorders = new HT[3, 3]
		{
			{
				HT.TOPLEFT,
				HT.TOP,
				HT.TOPRIGHT
			},
			{
				HT.LEFT,
				HT.CLIENT,
				HT.RIGHT
			},
			{
				HT.BOTTOMLEFT,
				HT.BOTTOM,
				HT.BOTTOMRIGHT
			}
		};

		private bool _IsWindowDocked
		{
			[SecurityCritical]
			get
			{
				if (_window.WindowState != 0)
				{
					return false;
				}
				ControlzEx.Standard.DpiScale dpi = _window.GetDpi();
				RECT rECT = _GetAdjustedWindowRect(new RECT
				{
					Bottom = 100,
					Right = 100
				});
				Point point = new Point(_window.Left, _window.Top);
				point -= (Vector)DpiHelper.DevicePixelsToLogical(new Point((double)rECT.Left, (double)rECT.Top), dpi.DpiScaleX, dpi.DpiScaleY);
				return _window.RestoreBounds.Location != point;
			}
		}

		private bool _MinimizeAnimation
		{
			get
			{
				if (SystemParameters.MinimizeAnimation)
				{
					return !_chromeInfo.IgnoreTaskbarOnMaximize;
				}
				return false;
			}
		}

		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public WindowChromeWorker()
		{
			_messageTable = new List<KeyValuePair<WM, MessageHandler>>
			{
				new KeyValuePair<WM, MessageHandler>(WM.NCUAHDRAWCAPTION, this._HandleNCUAHDrawCaption),
				new KeyValuePair<WM, MessageHandler>(WM.SETTEXT, this._HandleSetTextOrIcon),
				new KeyValuePair<WM, MessageHandler>(WM.SETICON, this._HandleSetTextOrIcon),
				new KeyValuePair<WM, MessageHandler>(WM.SYSCOMMAND, this._HandleRestoreWindow),
				new KeyValuePair<WM, MessageHandler>(WM.NCACTIVATE, this._HandleNCActivate),
				new KeyValuePair<WM, MessageHandler>(WM.NCCALCSIZE, this._HandleNCCalcSize),
				new KeyValuePair<WM, MessageHandler>(WM.NCHITTEST, this._HandleNCHitTest),
				new KeyValuePair<WM, MessageHandler>(WM.NCRBUTTONUP, this._HandleNCRButtonUp),
				new KeyValuePair<WM, MessageHandler>(WM.SIZE, this._HandleSize),
				new KeyValuePair<WM, MessageHandler>(WM.WINDOWPOSCHANGING, this._HandleWindowPosChanging),
				new KeyValuePair<WM, MessageHandler>(WM.WINDOWPOSCHANGED, this._HandleWindowPosChanged),
				new KeyValuePair<WM, MessageHandler>(WM.GETMINMAXINFO, this._HandleGetMinMaxInfo),
				new KeyValuePair<WM, MessageHandler>(WM.DWMCOMPOSITIONCHANGED, this._HandleDwmCompositionChanged),
				new KeyValuePair<WM, MessageHandler>(WM.ENTERSIZEMOVE, this._HandleEnterSizeMoveForAnimation),
				new KeyValuePair<WM, MessageHandler>(WM.MOVE, this._HandleMoveForRealSize),
				new KeyValuePair<WM, MessageHandler>(WM.EXITSIZEMOVE, this._HandleExitSizeMoveForAnimation)
			};
			if (Utility.IsPresentationFrameworkVersionLessThan4)
			{
				_messageTable.AddRange(new KeyValuePair<WM, MessageHandler>[4]
				{
					new KeyValuePair<WM, MessageHandler>(WM.WININICHANGE, this._HandleSettingChange),
					new KeyValuePair<WM, MessageHandler>(WM.ENTERSIZEMOVE, this._HandleEnterSizeMove),
					new KeyValuePair<WM, MessageHandler>(WM.EXITSIZEMOVE, this._HandleExitSizeMove),
					new KeyValuePair<WM, MessageHandler>(WM.MOVE, this._HandleMove)
				});
			}
		}

		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public void SetWindowChrome(WindowChrome newChrome)
		{
			VerifyAccess();
			if (newChrome != _chromeInfo)
			{
				if (_chromeInfo != null)
				{
					_chromeInfo.PropertyChangedThatRequiresRepaint -= _OnChromePropertyChangedThatRequiresRepaint;
				}
				_chromeInfo = newChrome;
				if (_chromeInfo != null)
				{
					_chromeInfo.PropertyChangedThatRequiresRepaint += _OnChromePropertyChangedThatRequiresRepaint;
				}
				_ApplyNewCustomChrome();
			}
		}

		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void _OnChromePropertyChangedThatRequiresRepaint(object sender, EventArgs e)
		{
			_UpdateFrameState(force: true);
		}

		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private static void _OnChromeWorkerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			((WindowChromeWorker)e.NewValue)._SetWindow(window);
		}

		[SecurityCritical]
		private void _SetWindow(Window window)
		{
			UnsubscribeWindowEvents();
			_window = window;
			_hwnd = new WindowInteropHelper(_window).Handle;
			Utility.AddDependencyPropertyChangeListener(_window, Control.TemplateProperty, _OnWindowPropertyChangedThatRequiresTemplateFixup);
			Utility.AddDependencyPropertyChangeListener(_window, FrameworkElement.FlowDirectionProperty, _OnWindowPropertyChangedThatRequiresTemplateFixup);
			_window.Closed += _UnsetWindow;
			if (IntPtr.Zero != _hwnd)
			{
				_hwndSource = HwndSource.FromHwnd(_hwnd);
				_window.ApplyTemplate();
				if (_chromeInfo != null)
				{
					_ApplyNewCustomChrome();
				}
			}
			else
			{
				_window.SourceInitialized += _WindowSourceInitialized;
			}
		}

		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void _WindowSourceInitialized(object sender, EventArgs e)
		{
			_hwnd = new WindowInteropHelper(_window).Handle;
			_hwndSource = HwndSource.FromHwnd(_hwnd);
			if (_chromeInfo != null)
			{
				_ApplyNewCustomChrome();
			}
		}

		[SecurityCritical]
		private void UnsubscribeWindowEvents()
		{
			if (_window != null)
			{
				Utility.RemoveDependencyPropertyChangeListener(_window, Control.TemplateProperty, _OnWindowPropertyChangedThatRequiresTemplateFixup);
				Utility.RemoveDependencyPropertyChangeListener(_window, FrameworkElement.FlowDirectionProperty, _OnWindowPropertyChangedThatRequiresTemplateFixup);
				_window.SourceInitialized -= _WindowSourceInitialized;
				_window.StateChanged -= _FixupRestoreBounds;
				_window.Closed -= _UnsetWindow;
			}
		}

		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void _UnsetWindow(object sender, EventArgs e)
		{
			UnsubscribeWindowEvents();
			if (_chromeInfo != null)
			{
				_chromeInfo.PropertyChangedThatRequiresRepaint -= _OnChromePropertyChangedThatRequiresRepaint;
			}
			_RestoreStandardChromeState(isClosing: true);
		}

		public static WindowChromeWorker GetWindowChromeWorker(Window window)
		{
			Verify.IsNotNull(window, "window");
			return (WindowChromeWorker)window.GetValue(WindowChromeWorkerProperty);
		}

		public static void SetWindowChromeWorker(Window window, WindowChromeWorker chrome)
		{
			Verify.IsNotNull(window, "window");
			window.SetValue(WindowChromeWorkerProperty, chrome);
		}

		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void _OnWindowPropertyChangedThatRequiresTemplateFixup(object sender, EventArgs e)
		{
			if (_chromeInfo != null && _hwnd != IntPtr.Zero)
			{
				_window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new _Action(_FixupTemplateIssues));
			}
		}

		[SecurityCritical]
		private void _ApplyNewCustomChrome()
		{
			if (!(_hwnd == IntPtr.Zero) && !_hwndSource.IsDisposed)
			{
				if (_chromeInfo == null)
				{
					_RestoreStandardChromeState(isClosing: false);
				}
				else
				{
					if (!_isHooked)
					{
						_hwndSource.AddHook(this._WndProc);
						_isHooked = true;
					}
					if (_MinimizeAnimation)
					{
						_ModifyStyle(WS.OVERLAPPED, WS.CAPTION);
					}
					_FixupTemplateIssues();
					_UpdateSystemMenu(_window.WindowState);
					_UpdateFrameState(force: true);
					if (_hwndSource.IsDisposed)
					{
						_UnsetWindow(_window, EventArgs.Empty);
					}
					else
					{
						NativeMethods.SetWindowPos(_hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
					}
				}
			}
		}

		[SecurityCritical]
		private void _FixupTemplateIssues()
		{
			if (_window.Template != null)
			{
				if (VisualTreeHelper.GetChildrenCount(_window) == 0)
				{
					_window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new _Action(_FixupTemplateIssues));
				}
				else
				{
					Thickness margin = default(Thickness);
					Transform transform = null;
					FrameworkElement frameworkElement = (FrameworkElement)VisualTreeHelper.GetChild(_window, 0);
					if (_chromeInfo.SacrificialEdge != 0)
					{
						if (Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 2))
						{
							margin.Top -= SystemParameters.WindowResizeBorderThickness.Top;
						}
						if (Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 1))
						{
							margin.Left -= SystemParameters.WindowResizeBorderThickness.Left;
						}
						if (Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 8))
						{
							margin.Bottom -= SystemParameters.WindowResizeBorderThickness.Bottom;
						}
						if (Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 4))
						{
							margin.Right -= SystemParameters.WindowResizeBorderThickness.Right;
						}
					}
					if (Utility.IsPresentationFrameworkVersionLessThan4)
					{
						ControlzEx.Standard.DpiScale dpi = _window.GetDpi();
						RECT windowRect = NativeMethods.GetWindowRect(_hwnd);
						RECT rECT = _GetAdjustedWindowRect(windowRect);
						Rect rect = DpiHelper.DeviceRectToLogical(new Rect((double)windowRect.Left, (double)windowRect.Top, (double)windowRect.Width, (double)windowRect.Height), dpi.DpiScaleX, dpi.DpiScaleY);
						Rect rect2 = DpiHelper.DeviceRectToLogical(new Rect((double)rECT.Left, (double)rECT.Top, (double)rECT.Width, (double)rECT.Height), dpi.DpiScaleX, dpi.DpiScaleY);
						if (!Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 1))
						{
							margin.Right -= SystemParameters.WindowResizeBorderThickness.Left;
						}
						if (!Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 4))
						{
							margin.Right -= SystemParameters.WindowResizeBorderThickness.Right;
						}
						if (!Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 2))
						{
							margin.Bottom -= SystemParameters.WindowResizeBorderThickness.Top;
						}
						if (!Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 8))
						{
							margin.Bottom -= SystemParameters.WindowResizeBorderThickness.Bottom;
						}
						margin.Bottom -= SystemParameters.WindowCaptionHeight;
						if (_window.FlowDirection == FlowDirection.RightToLeft)
						{
							Thickness thickness = new Thickness(rect.Left - rect2.Left, rect.Top - rect2.Top, rect2.Right - rect.Right, rect2.Bottom - rect.Bottom);
							transform = new MatrixTransform(1.0, 0.0, 0.0, 1.0, 0.0 - (thickness.Left + thickness.Right), 0.0);
						}
						else
						{
							transform = null;
						}
						frameworkElement.RenderTransform = transform;
					}
					frameworkElement.Margin = margin;
					if (Utility.IsPresentationFrameworkVersionLessThan4 && !_isFixedUp)
					{
						_hasUserMovedWindow = false;
						_window.StateChanged += _FixupRestoreBounds;
						_isFixedUp = true;
					}
				}
			}
		}

		[SecuritySafeCritical]
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		private void _FixupRestoreBounds(object sender, EventArgs e)
		{
			if ((_window.WindowState == WindowState.Maximized || _window.WindowState == WindowState.Minimized) && _hasUserMovedWindow)
			{
				ControlzEx.Standard.DpiScale dpi = _window.GetDpi();
				_hasUserMovedWindow = false;
				WINDOWPLACEMENT windowPlacement = NativeMethods.GetWindowPlacement(_hwnd);
				RECT rECT = _GetAdjustedWindowRect(new RECT
				{
					Bottom = 100,
					Right = 100
				});
				Point point = DpiHelper.DevicePixelsToLogical(new Point((double)(windowPlacement.normalPosition.Left - rECT.Left), (double)(windowPlacement.normalPosition.Top - rECT.Top)), dpi.DpiScaleX, dpi.DpiScaleY);
				_window.Top = point.Y;
				_window.Left = point.X;
			}
		}

		[SecurityCritical]
		private RECT _GetAdjustedWindowRect(RECT rcWindow)
		{
			WS dwStyle = (WS)(int)NativeMethods.GetWindowLongPtr(_hwnd, GWL.STYLE);
			WS_EX dwExStyle = (WS_EX)(int)NativeMethods.GetWindowLongPtr(_hwnd, GWL.EXSTYLE);
			return NativeMethods.AdjustWindowRectEx(rcWindow, dwStyle, bMenu: false, dwExStyle);
		}

		[SecurityCritical]
		private IntPtr _WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (_hwndSource?.RootVisual == null)
			{
				return IntPtr.Zero;
			}
			foreach (KeyValuePair<WM, MessageHandler> item in _messageTable)
			{
				if (item.Key == (WM)msg)
				{
					return item.Value((WM)msg, wParam, lParam, out handled);
				}
			}
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleNCUAHDrawCaption(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (!_window.ShowInTaskbar && _GetHwndState() == WindowState.Minimized)
			{
				bool num = _ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
				IntPtr result = NativeMethods.DefWindowProc(_hwnd, uMsg, wParam, lParam);
				if (num)
				{
					_ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
				}
				handled = true;
				return result;
			}
			handled = false;
			return IntPtr.Zero;
		}

		private IntPtr _HandleSetTextOrIcon(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			bool num = _ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
			IntPtr result = NativeMethods.DefWindowProc(_hwnd, uMsg, wParam, lParam);
			if (num)
			{
				_ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
			}
			handled = true;
			return result;
		}

		[SecurityCritical]
		private IntPtr _HandleRestoreWindow(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			WINDOWPLACEMENT windowPlacement = NativeMethods.GetWindowPlacement(_hwnd);
			SC sC = (SC)(Environment.Is64BitProcess ? wParam.ToInt64() : wParam.ToInt32());
			if (SC.RESTORE == sC && windowPlacement.showCmd == SW.SHOWMAXIMIZED && _MinimizeAnimation)
			{
				bool num = _ModifyStyle(WS.SYSMENU, WS.OVERLAPPED);
				IntPtr result = NativeMethods.DefWindowProc(_hwnd, uMsg, wParam, lParam);
				if (num)
				{
					_ModifyStyle(WS.OVERLAPPED, WS.SYSMENU);
				}
				handled = true;
				return result;
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleNCActivate(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			IntPtr result = NativeMethods.DefWindowProc(_hwnd, WM.NCACTIVATE, wParam, new IntPtr(-1));
			handled = true;
			return result;
		}

		[SecurityCritical]
		private static RECT AdjustWorkingAreaForAutoHide(IntPtr monitorContainingApplication, RECT area)
		{
			IntPtr taskBarHandleForMonitor = NativeMethods.GetTaskBarHandleForMonitor(monitorContainingApplication);
			if (taskBarHandleForMonitor == IntPtr.Zero)
			{
				return area;
			}
			APPBARDATA pData = default(APPBARDATA);
			pData.cbSize = Marshal.SizeOf((object)pData);
			pData.hWnd = taskBarHandleForMonitor;
			NativeMethods.SHAppBarMessage(5, ref pData);
			if (!Convert.ToBoolean(NativeMethods.SHAppBarMessage(4, ref pData)))
			{
				return area;
			}
			switch (pData.uEdge)
			{
			case 0:
				area.Left += 2;
				break;
			case 2:
				area.Right -= 2;
				break;
			case 1:
				area.Top += 2;
				break;
			case 3:
				area.Bottom -= 2;
				break;
			default:
				return area;
			}
			return area;
		}

		[SecurityCritical]
		private IntPtr _HandleNCCalcSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (NativeMethods.GetWindowPlacement(_hwnd).showCmd == SW.SHOWMAXIMIZED && _MinimizeAnimation)
			{
				IntPtr intPtr = NativeMethods.MonitorFromWindow(_hwnd, MonitorOptions.MONITOR_DEFAULTTONEAREST);
				MONITORINFO monitorInfo = NativeMethods.GetMonitorInfo(intPtr);
				RECT rECT = _chromeInfo.IgnoreTaskbarOnMaximize ? monitorInfo.rcMonitor : monitorInfo.rcWork;
				RECT rECT2 = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
				rECT2.Left = rECT.Left;
				rECT2.Top = rECT.Top;
				rECT2.Right = rECT.Right;
				rECT2.Bottom = rECT.Bottom;
				if (monitorInfo.rcMonitor.Height == monitorInfo.rcWork.Height && monitorInfo.rcMonitor.Width == monitorInfo.rcWork.Width)
				{
					rECT2 = AdjustWorkingAreaForAutoHide(intPtr, rECT2);
				}
				Marshal.StructureToPtr((object)rECT2, lParam, fDeleteOld: true);
			}
			if (_chromeInfo.SacrificialEdge != 0)
			{
				ControlzEx.Standard.DpiScale dpi = _window.GetDpi();
				Thickness thickness = DpiHelper.LogicalThicknessToDevice(SystemParameters.WindowResizeBorderThickness, dpi.DpiScaleX, dpi.DpiScaleY);
				RECT rECT3 = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
				if (Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 2))
				{
					rECT3.Top += (int)thickness.Top;
				}
				if (Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 1))
				{
					rECT3.Left += (int)thickness.Left;
				}
				if (Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 8))
				{
					rECT3.Bottom -= (int)thickness.Bottom;
				}
				if (Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 4))
				{
					rECT3.Right -= (int)thickness.Right;
				}
				Marshal.StructureToPtr((object)rECT3, lParam, fDeleteOld: false);
			}
			handled = true;
			IntPtr result = IntPtr.Zero;
			if (wParam.ToInt32() != 0)
			{
				result = new IntPtr(1792);
			}
			return result;
		}

		private HT _GetHTFromResizeGripDirection(ResizeGripDirection direction)
		{
			bool flag = _window.FlowDirection == FlowDirection.RightToLeft;
			switch (direction)
			{
			case ResizeGripDirection.Bottom:
				return HT.BOTTOM;
			case ResizeGripDirection.BottomLeft:
				if (!flag)
				{
					return HT.BOTTOMLEFT;
				}
				return HT.BOTTOMRIGHT;
			case ResizeGripDirection.BottomRight:
				if (!flag)
				{
					return HT.BOTTOMRIGHT;
				}
				return HT.BOTTOMLEFT;
			case ResizeGripDirection.Left:
				if (!flag)
				{
					return HT.LEFT;
				}
				return HT.RIGHT;
			case ResizeGripDirection.Right:
				if (!flag)
				{
					return HT.RIGHT;
				}
				return HT.LEFT;
			case ResizeGripDirection.Top:
				return HT.TOP;
			case ResizeGripDirection.TopLeft:
				if (!flag)
				{
					return HT.TOPLEFT;
				}
				return HT.TOPRIGHT;
			case ResizeGripDirection.TopRight:
				if (!flag)
				{
					return HT.TOPRIGHT;
				}
				return HT.TOPLEFT;
			case ResizeGripDirection.Caption:
				return HT.CAPTION;
			default:
				return HT.NOWHERE;
			}
		}

		[SecurityCritical]
		private IntPtr _HandleNCHitTest(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			ControlzEx.Standard.DpiScale dpi = _window.GetDpi();
			Point point = Utility.GetPoint(lParam);
			Rect deviceRectangle = _GetWindowRect();
			Point devicePoint = point;
			devicePoint.Offset(0.0 - deviceRectangle.X, 0.0 - deviceRectangle.Y);
			devicePoint = DpiHelper.DevicePixelsToLogical(devicePoint, dpi.DpiScaleX, dpi.DpiScaleY);
			IInputElement inputElement = _window.InputHitTest(devicePoint);
			if (inputElement != null)
			{
				if (WindowChrome.GetIsHitTestVisibleInChrome(inputElement))
				{
					handled = true;
					return new IntPtr(1);
				}
				ResizeGripDirection resizeGripDirection = WindowChrome.GetResizeGripDirection(inputElement);
				if (resizeGripDirection != 0)
				{
					handled = true;
					return new IntPtr((int)_GetHTFromResizeGripDirection(resizeGripDirection));
				}
			}
			if (_chromeInfo.UseAeroCaptionButtons && Utility.IsOSVistaOrNewer && _chromeInfo.GlassFrameThickness != default(Thickness) && _isGlassEnabled)
			{
				handled = NativeMethods.DwmDefWindowProc(_hwnd, uMsg, wParam, lParam, out IntPtr plResult);
				if (IntPtr.Zero != plResult)
				{
					return plResult;
				}
			}
			HT value = _HitTestNca(DpiHelper.DeviceRectToLogical(deviceRectangle, dpi.DpiScaleX, dpi.DpiScaleY), DpiHelper.DevicePixelsToLogical(point, dpi.DpiScaleX, dpi.DpiScaleY));
			handled = true;
			return new IntPtr((int)value);
		}

		[SecurityCritical]
		private IntPtr _HandleNCRButtonUp(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (2 == (int)(Environment.Is64BitProcess ? wParam.ToInt64() : wParam.ToInt32()))
			{
				SystemCommands.ShowSystemMenuPhysicalCoordinates(_window, Utility.GetPoint(lParam));
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			WindowState? assumeState = null;
			if ((Environment.Is64BitProcess ? wParam.ToInt64() : wParam.ToInt32()) == 2)
			{
				assumeState = WindowState.Maximized;
			}
			_UpdateSystemMenu(assumeState);
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleWindowPosChanging(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (!_isGlassEnabled)
			{
				WINDOWPOS wINDOWPOS = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
				if (_chromeInfo.IgnoreTaskbarOnMaximize && _GetHwndState() == WindowState.Maximized && wINDOWPOS.flags == SWP.DRAWFRAME)
				{
					wINDOWPOS.flags = (SWP)0;
					Marshal.StructureToPtr((object)wINDOWPOS, lParam, fDeleteOld: true);
				}
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleWindowPosChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			_UpdateSystemMenu(null);
			if (!_isGlassEnabled)
			{
				WINDOWPOS wINDOWPOS = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
				if (!wINDOWPOS.Equals(_previousWP))
				{
					_previousWP = wINDOWPOS;
					_SetRoundingRegion(wINDOWPOS);
				}
				_previousWP = wINDOWPOS;
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleGetMinMaxInfo(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (_chromeInfo.IgnoreTaskbarOnMaximize && NativeMethods.IsZoomed(_hwnd))
			{
				MINMAXINFO mINMAXINFO = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
				IntPtr intPtr = NativeMethods.MonitorFromWindow(_hwnd, MonitorOptions.MONITOR_DEFAULTTONEAREST);
				if (intPtr != IntPtr.Zero)
				{
					MONITORINFO monitorInfoW = NativeMethods.GetMonitorInfoW(intPtr);
					RECT rcWork = monitorInfoW.rcWork;
					RECT rcMonitor = monitorInfoW.rcMonitor;
					mINMAXINFO.ptMaxPosition.X = Math.Abs(rcWork.Left - rcMonitor.Left);
					mINMAXINFO.ptMaxPosition.Y = Math.Abs(rcWork.Top - rcMonitor.Top);
					mINMAXINFO.ptMaxSize.X = Math.Abs(monitorInfoW.rcMonitor.Width);
					mINMAXINFO.ptMaxSize.Y = Math.Abs(monitorInfoW.rcMonitor.Height);
					mINMAXINFO.ptMaxTrackSize.X = mINMAXINFO.ptMaxSize.X;
					mINMAXINFO.ptMaxTrackSize.Y = mINMAXINFO.ptMaxSize.Y;
				}
				Marshal.StructureToPtr((object)mINMAXINFO, lParam, fDeleteOld: true);
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleDwmCompositionChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			_UpdateFrameState(force: false);
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleSettingChange(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			_FixupTemplateIssues();
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleEnterSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			_isUserResizing = true;
			if (_window.WindowState != WindowState.Maximized && !_IsWindowDocked)
			{
				_windowPosAtStartOfUserMove = new Point(_window.Left, _window.Top);
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleEnterSizeMoveForAnimation(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (_MinimizeAnimation)
			{
				_ModifyStyle(WS.CAPTION, WS.OVERLAPPED);
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleMoveForRealSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (_GetHwndState() == WindowState.Maximized)
			{
				IntPtr intPtr = NativeMethods.MonitorFromWindow(_hwnd, MonitorOptions.MONITOR_DEFAULTTONEAREST);
				if (intPtr != IntPtr.Zero)
				{
					bool ignoreTaskbarOnMaximize = _chromeInfo.IgnoreTaskbarOnMaximize;
					MONITORINFO monitorInfoW = NativeMethods.GetMonitorInfoW(intPtr);
					RECT rECT = ignoreTaskbarOnMaximize ? monitorInfoW.rcMonitor : monitorInfoW.rcWork;
					NativeMethods.SetWindowPos(_hwnd, IntPtr.Zero, rECT.Left, rECT.Top, rECT.Width, rECT.Height, SWP.ASYNCWINDOWPOS | SWP.DRAWFRAME | SWP.NOCOPYBITS);
				}
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private IntPtr _HandleExitSizeMoveForAnimation(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (_MinimizeAnimation && _ModifyStyle(WS.OVERLAPPED, WS.CAPTION))
			{
				NativeMethods.SetWindowPos(_hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
			}
			handled = false;
			return IntPtr.Zero;
		}

		private IntPtr _HandleExitSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			_isUserResizing = false;
			if (_window.WindowState == WindowState.Maximized)
			{
				_window.Top = _windowPosAtStartOfUserMove.Y;
				_window.Left = _windowPosAtStartOfUserMove.X;
			}
			handled = false;
			return IntPtr.Zero;
		}

		private IntPtr _HandleMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (_isUserResizing)
			{
				_hasUserMovedWindow = true;
			}
			handled = false;
			return IntPtr.Zero;
		}

		[SecurityCritical]
		private bool _ModifyStyle(WS removeStyle, WS addStyle)
		{
			IntPtr windowLongPtr = NativeMethods.GetWindowLongPtr(_hwnd, GWL.STYLE);
			int num = (int)(Environment.Is64BitProcess ? windowLongPtr.ToInt64() : windowLongPtr.ToInt32());
			WS wS = (WS)((num & (int)(~removeStyle)) | (int)addStyle);
			if (num == (int)wS)
			{
				return false;
			}
			NativeMethods.SetWindowLongPtr(_hwnd, GWL.STYLE, new IntPtr((int)wS));
			return true;
		}

		[SecurityCritical]
		private WindowState _GetHwndState()
		{
			switch (NativeMethods.GetWindowPlacement(_hwnd).showCmd)
			{
			case SW.SHOWMINIMIZED:
				return WindowState.Minimized;
			case SW.SHOWMAXIMIZED:
				return WindowState.Maximized;
			default:
				return WindowState.Normal;
			}
		}

		[SecurityCritical]
		private Rect _GetWindowRect()
		{
			RECT windowRect = NativeMethods.GetWindowRect(_hwnd);
			return new Rect((double)windowRect.Left, (double)windowRect.Top, (double)windowRect.Width, (double)windowRect.Height);
		}

		[SecurityCritical]
		private void _UpdateSystemMenu(WindowState? assumeState)
		{
			WindowState windowState = assumeState ?? _GetHwndState();
			if (assumeState.HasValue || _lastMenuState != windowState)
			{
				_lastMenuState = windowState;
				IntPtr systemMenu = NativeMethods.GetSystemMenu(_hwnd, bRevert: false);
				if (IntPtr.Zero != systemMenu)
				{
					IntPtr windowLongPtr = NativeMethods.GetWindowLongPtr(_hwnd, GWL.STYLE);
					int value = (int)(Environment.Is64BitProcess ? windowLongPtr.ToInt64() : windowLongPtr.ToInt32());
					bool flag = Utility.IsFlagSet(value, 131072);
					bool flag2 = Utility.IsFlagSet(value, 65536);
					bool flag3 = Utility.IsFlagSet(value, 262144);
					switch (windowState)
					{
					case WindowState.Maximized:
						NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, (!flag) ? (MF.GRAYED | MF.DISABLED) : MF.ENABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, MF.GRAYED | MF.DISABLED);
						break;
					case WindowState.Minimized:
						NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, (!flag2) ? (MF.GRAYED | MF.DISABLED) : MF.ENABLED);
						break;
					default:
						NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.ENABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, (!flag3) ? (MF.GRAYED | MF.DISABLED) : MF.ENABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, (!flag) ? (MF.GRAYED | MF.DISABLED) : MF.ENABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, (!flag2) ? (MF.GRAYED | MF.DISABLED) : MF.ENABLED);
						break;
					}
				}
			}
		}

		[SecurityCritical]
		private void _UpdateFrameState(bool force)
		{
			if (!(IntPtr.Zero == _hwnd) && !_hwndSource.IsDisposed)
			{
				bool flag = NativeMethods.DwmIsCompositionEnabled();
				if (force || flag != _isGlassEnabled)
				{
					_isGlassEnabled = (flag && _chromeInfo.GlassFrameThickness != default(Thickness));
					if (!_isGlassEnabled)
					{
						_SetRoundingRegion(null);
					}
					else
					{
						_ClearRoundingRegion();
						_ExtendGlassFrame();
					}
					if (!_hwndSource.IsDisposed)
					{
						if (_MinimizeAnimation)
						{
							_ModifyStyle(WS.OVERLAPPED, WS.CAPTION);
						}
						else
						{
							_ModifyStyle(WS.CAPTION, WS.OVERLAPPED);
						}
						NativeMethods.SetWindowPos(_hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
					}
				}
			}
		}

		[SecurityCritical]
		private void _ClearRoundingRegion()
		{
			NativeMethods.SetWindowRgn(_hwnd, IntPtr.Zero, NativeMethods.IsWindowVisible(_hwnd));
		}

		[SecurityCritical]
		private RECT _GetClientRectRelativeToWindowRect(IntPtr hWnd)
		{
			RECT windowRect = NativeMethods.GetWindowRect(hWnd);
			RECT clientRect = NativeMethods.GetClientRect(hWnd);
			POINT pOINT = default(POINT);
			pOINT.X = 0;
			pOINT.Y = 0;
			POINT point = pOINT;
			NativeMethods.ClientToScreen(hWnd, ref point);
			if (_window.FlowDirection == FlowDirection.RightToLeft)
			{
				clientRect.Offset(windowRect.Right - point.X, point.Y - windowRect.Top);
			}
			else
			{
				clientRect.Offset(point.X - windowRect.Left, point.Y - windowRect.Top);
			}
			return clientRect;
		}

		[SecurityCritical]
		private void _SetRoundingRegion(WINDOWPOS? wp)
		{
			if (NativeMethods.GetWindowPlacement(_hwnd).showCmd == SW.SHOWMAXIMIZED)
			{
				RECT lprc;
				if (_MinimizeAnimation)
				{
					lprc = _GetClientRectRelativeToWindowRect(_hwnd);
				}
				else
				{
					int num;
					int num2;
					if (wp.HasValue)
					{
						num = wp.Value.x;
						num2 = wp.Value.y;
					}
					else
					{
						Rect rect = _GetWindowRect();
						num = (int)rect.Left;
						num2 = (int)rect.Top;
					}
					MONITORINFO monitorInfo = NativeMethods.GetMonitorInfo(NativeMethods.MonitorFromWindow(_hwnd, MonitorOptions.MONITOR_DEFAULTTONEAREST));
					lprc = (_chromeInfo.IgnoreTaskbarOnMaximize ? monitorInfo.rcMonitor : monitorInfo.rcWork);
					lprc.Offset(-num, -num2);
				}
				IntPtr gdiObject = IntPtr.Zero;
				try
				{
					gdiObject = NativeMethods.CreateRectRgnIndirect(lprc);
					NativeMethods.SetWindowRgn(_hwnd, gdiObject, NativeMethods.IsWindowVisible(_hwnd));
					gdiObject = IntPtr.Zero;
				}
				finally
				{
					Utility.SafeDeleteObject(ref gdiObject);
				}
			}
			else
			{
				Size size;
				if (wp.HasValue && !Utility.IsFlagSet((int)wp.Value.flags, 1))
				{
					size = new Size((double)wp.Value.cx, (double)wp.Value.cy);
				}
				else
				{
					if (wp.HasValue && _lastRoundingState == _window.WindowState)
					{
						return;
					}
					size = _GetWindowRect().Size;
				}
				_lastRoundingState = _window.WindowState;
				IntPtr gdiObject2 = IntPtr.Zero;
				try
				{
					ControlzEx.Standard.DpiScale dpi = _window.GetDpi();
					double num3 = Math.Min(size.Width, size.Height);
					double x = DpiHelper.LogicalPixelsToDevice(new Point(_chromeInfo.CornerRadius.TopLeft, 0.0), dpi.DpiScaleX, dpi.DpiScaleY).X;
					x = Math.Min(x, num3 / 2.0);
					if (_IsUniform(_chromeInfo.CornerRadius))
					{
						gdiObject2 = _CreateRoundRectRgn(new Rect(size), x);
					}
					else
					{
						gdiObject2 = _CreateRoundRectRgn(new Rect(0.0, 0.0, size.Width / 2.0 + x, size.Height / 2.0 + x), x);
						double x2 = DpiHelper.LogicalPixelsToDevice(new Point(_chromeInfo.CornerRadius.TopRight, 0.0), dpi.DpiScaleX, dpi.DpiScaleY).X;
						x2 = Math.Min(x2, num3 / 2.0);
						Rect region = new Rect(0.0, 0.0, size.Width / 2.0 + x2, size.Height / 2.0 + x2);
						region.Offset(size.Width / 2.0 - x2, 0.0);
						_CreateAndCombineRoundRectRgn(gdiObject2, region, x2);
						double x3 = DpiHelper.LogicalPixelsToDevice(new Point(_chromeInfo.CornerRadius.BottomLeft, 0.0), dpi.DpiScaleX, dpi.DpiScaleY).X;
						x3 = Math.Min(x3, num3 / 2.0);
						Rect region2 = new Rect(0.0, 0.0, size.Width / 2.0 + x3, size.Height / 2.0 + x3);
						region2.Offset(0.0, size.Height / 2.0 - x3);
						_CreateAndCombineRoundRectRgn(gdiObject2, region2, x3);
						double x4 = DpiHelper.LogicalPixelsToDevice(new Point(_chromeInfo.CornerRadius.BottomRight, 0.0), dpi.DpiScaleX, dpi.DpiScaleY).X;
						x4 = Math.Min(x4, num3 / 2.0);
						Rect region3 = new Rect(0.0, 0.0, size.Width / 2.0 + x4, size.Height / 2.0 + x4);
						region3.Offset(size.Width / 2.0 - x4, size.Height / 2.0 - x4);
						_CreateAndCombineRoundRectRgn(gdiObject2, region3, x4);
					}
					NativeMethods.SetWindowRgn(_hwnd, gdiObject2, NativeMethods.IsWindowVisible(_hwnd));
					gdiObject2 = IntPtr.Zero;
				}
				finally
				{
					Utility.SafeDeleteObject(ref gdiObject2);
				}
			}
		}

		[SecurityCritical]
		private static IntPtr _CreateRoundRectRgn(Rect region, double radius)
		{
			if (DoubleUtilities.AreClose(0.0, radius))
			{
				return NativeMethods.CreateRectRgn((int)Math.Floor(region.Left), (int)Math.Floor(region.Top), (int)Math.Ceiling(region.Right), (int)Math.Ceiling(region.Bottom));
			}
			return NativeMethods.CreateRoundRectRgn((int)Math.Floor(region.Left), (int)Math.Floor(region.Top), (int)Math.Ceiling(region.Right) + 1, (int)Math.Ceiling(region.Bottom) + 1, (int)Math.Ceiling(radius), (int)Math.Ceiling(radius));
		}

		[SecurityCritical]
		private static void _CreateAndCombineRoundRectRgn(IntPtr hrgnSource, Rect region, double radius)
		{
			IntPtr gdiObject = IntPtr.Zero;
			try
			{
				gdiObject = _CreateRoundRectRgn(region, radius);
				if (NativeMethods.CombineRgn(hrgnSource, hrgnSource, gdiObject, RGN.OR) == CombineRgnResult.ERROR)
				{
					throw new InvalidOperationException("Unable to combine two HRGNs.");
				}
			}
			catch
			{
				Utility.SafeDeleteObject(ref gdiObject);
				throw;
			}
		}

		private static bool _IsUniform(CornerRadius cornerRadius)
		{
			if (!DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.BottomRight))
			{
				return false;
			}
			if (!DoubleUtilities.AreClose(cornerRadius.TopLeft, cornerRadius.TopRight))
			{
				return false;
			}
			if (!DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.TopRight))
			{
				return false;
			}
			return true;
		}

		[SecurityCritical]
		private void _ExtendGlassFrame()
		{
			if (Utility.IsOSVistaOrNewer && !(IntPtr.Zero == _hwnd) && !_hwndSource.IsDisposed)
			{
				if (!NativeMethods.DwmIsCompositionEnabled())
				{
					if (_window.AllowsTransparency)
					{
						_hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
					}
					else
					{
						_hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
					}
				}
				else
				{
					ControlzEx.Standard.DpiScale dpi = _window.GetDpi();
					if (_window.AllowsTransparency)
					{
						_hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
					}
					Thickness thickness = DpiHelper.LogicalThicknessToDevice(_chromeInfo.GlassFrameThickness, dpi.DpiScaleX, dpi.DpiScaleY);
					if (_chromeInfo.SacrificialEdge != 0)
					{
						Thickness thickness2 = DpiHelper.LogicalThicknessToDevice(SystemParameters.WindowResizeBorderThickness, dpi.DpiScaleX, dpi.DpiScaleY);
						if (Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 2))
						{
							thickness.Top -= thickness2.Top;
							thickness.Top = Math.Max(0.0, thickness.Top);
						}
						if (Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 1))
						{
							thickness.Left -= thickness2.Left;
							thickness.Left = Math.Max(0.0, thickness.Left);
						}
						if (Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 8))
						{
							thickness.Bottom -= thickness2.Bottom;
							thickness.Bottom = Math.Max(0.0, thickness.Bottom);
						}
						if (Utility.IsFlagSet((int)_chromeInfo.SacrificialEdge, 4))
						{
							thickness.Right -= thickness2.Right;
							thickness.Right = Math.Max(0.0, thickness.Right);
						}
					}
					MARGINS mARGINS = default(MARGINS);
					mARGINS.cxLeftWidth = (int)Math.Ceiling(thickness.Left);
					mARGINS.cxRightWidth = (int)Math.Ceiling(thickness.Right);
					mARGINS.cyTopHeight = (int)Math.Ceiling(thickness.Top);
					mARGINS.cyBottomHeight = (int)Math.Ceiling(thickness.Bottom);
					MARGINS pMarInset = mARGINS;
					NativeMethods.DwmExtendFrameIntoClientArea(_hwnd, ref pMarInset);
				}
			}
		}

		private HT _HitTestNca(Rect windowPosition, Point mousePosition)
		{
			int num = 1;
			int num2 = 1;
			bool flag = false;
			if (mousePosition.Y >= windowPosition.Top && mousePosition.Y < windowPosition.Top + _chromeInfo.ResizeBorderThickness.Top + _chromeInfo.CaptionHeight)
			{
				flag = (mousePosition.Y < windowPosition.Top + _chromeInfo.ResizeBorderThickness.Top);
				num = 0;
			}
			else if (mousePosition.Y < windowPosition.Bottom && mousePosition.Y >= windowPosition.Bottom - (double)(int)_chromeInfo.ResizeBorderThickness.Bottom)
			{
				num = 2;
			}
			if (mousePosition.X >= windowPosition.Left && mousePosition.X < windowPosition.Left + (double)(int)_chromeInfo.ResizeBorderThickness.Left)
			{
				num2 = 0;
			}
			else if (mousePosition.X < windowPosition.Right && mousePosition.X >= windowPosition.Right - _chromeInfo.ResizeBorderThickness.Right)
			{
				num2 = 2;
			}
			if (num == 0 && num2 != 1 && !flag)
			{
				num = 1;
			}
			HT hT = _HitTestBorders[num, num2];
			if (hT == HT.TOP && !flag)
			{
				hT = HT.CAPTION;
			}
			return hT;
		}

		[SecurityCritical]
		private void _RestoreStandardChromeState(bool isClosing)
		{
			VerifyAccess();
			_UnhookCustomChrome();
			if (!isClosing && !_hwndSource.IsDisposed)
			{
				_RestoreFrameworkIssueFixups();
				_RestoreGlassFrame();
				_RestoreHrgn();
				_window.InvalidateMeasure();
			}
		}

		[SecurityCritical]
		private void _UnhookCustomChrome()
		{
			if (_isHooked)
			{
				_hwndSource.RemoveHook(this._WndProc);
				_isHooked = false;
			}
		}

		[SecurityCritical]
		private void _RestoreFrameworkIssueFixups()
		{
			((FrameworkElement)VisualTreeHelper.GetChild(_window, 0)).Margin = default(Thickness);
			if (Utility.IsPresentationFrameworkVersionLessThan4)
			{
				_window.StateChanged -= _FixupRestoreBounds;
				_isFixedUp = false;
			}
		}

		[SecurityCritical]
		private void _RestoreGlassFrame()
		{
			if (Utility.IsOSVistaOrNewer && !(_hwnd == IntPtr.Zero))
			{
				_hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
				if (NativeMethods.DwmIsCompositionEnabled())
				{
					MARGINS pMarInset = default(MARGINS);
					NativeMethods.DwmExtendFrameIntoClientArea(_hwnd, ref pMarInset);
				}
			}
		}

		[SecurityCritical]
		private void _RestoreHrgn()
		{
			_ClearRoundingRegion();
			NativeMethods.SetWindowPos(_hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
		}
	}
}
