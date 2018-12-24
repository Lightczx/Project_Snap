using ControlzEx.Standard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Media;

namespace ControlzEx.Windows.Shell
{
	internal class SystemParameters2 : INotifyPropertyChanged
	{
		private delegate void _SystemMetricUpdate(IntPtr wParam, IntPtr lParam);

		private enum CacheSlot
		{
			DpiX,
			NumSlots
		}

		[ThreadStatic]
		private static SystemParameters2 _threadLocalSingleton;

		private MessageWindow _messageHwnd;

		private bool _isGlassEnabled;

		private Color _glassColor;

		private SolidColorBrush _glassColorBrush;

		private Thickness _windowResizeBorderThickness;

		private Thickness _windowNonClientFrameThickness;

		private double _captionHeight;

		private Size _smallIconSize;

		private string _uxThemeName;

		private string _uxThemeColor;

		private bool _isHighContrast;

		private CornerRadius _windowCornerRadius;

		private Rect _captionButtonLocation;

		private readonly Dictionary<WM, List<_SystemMetricUpdate>> _UpdateTable;

		private static int _dpi;

		private static bool _dpiInitialized;

		private static readonly object _dpiLock = new object();

		private static bool _setDpiX = true;

		private static BitArray _cacheValid = new BitArray(1);

		private static int _dpiX;

		public static SystemParameters2 Current
		{
			get
			{
				if (_threadLocalSingleton == null)
				{
					_threadLocalSingleton = new SystemParameters2();
				}
				return _threadLocalSingleton;
			}
		}

		public bool IsGlassEnabled
		{
			get
			{
				return NativeMethods.DwmIsCompositionEnabled();
			}
			private set
			{
				if (value != _isGlassEnabled)
				{
					_isGlassEnabled = value;
					_NotifyPropertyChanged("IsGlassEnabled");
				}
			}
		}

		public Color WindowGlassColor
		{
			get
			{
				return _glassColor;
			}
			private set
			{
				if (value != _glassColor)
				{
					_glassColor = value;
					_NotifyPropertyChanged("WindowGlassColor");
				}
			}
		}

		public SolidColorBrush WindowGlassBrush
		{
			get
			{
				return _glassColorBrush;
			}
			private set
			{
				if (_glassColorBrush == null || value.Color != _glassColorBrush.Color)
				{
					_glassColorBrush = value;
					_NotifyPropertyChanged("WindowGlassBrush");
				}
			}
		}

		public Thickness WindowResizeBorderThickness
		{
			get
			{
				return _windowResizeBorderThickness;
			}
			private set
			{
				if (value != _windowResizeBorderThickness)
				{
					_windowResizeBorderThickness = value;
					_NotifyPropertyChanged("WindowResizeBorderThickness");
				}
			}
		}

		public Thickness WindowNonClientFrameThickness
		{
			get
			{
				return _windowNonClientFrameThickness;
			}
			private set
			{
				if (value != _windowNonClientFrameThickness)
				{
					_windowNonClientFrameThickness = value;
					_NotifyPropertyChanged("WindowNonClientFrameThickness");
				}
			}
		}

		public double WindowCaptionHeight
		{
			get
			{
				return _captionHeight;
			}
			private set
			{
				if (value != _captionHeight)
				{
					_captionHeight = value;
					_NotifyPropertyChanged("WindowCaptionHeight");
				}
			}
		}

		public Size SmallIconSize
		{
			get
			{
				return new Size(_smallIconSize.Width, _smallIconSize.Height);
			}
			private set
			{
				if (value != _smallIconSize)
				{
					_smallIconSize = value;
					_NotifyPropertyChanged("SmallIconSize");
				}
			}
		}

		public string UxThemeName
		{
			get
			{
				return _uxThemeName;
			}
			private set
			{
				if (value != _uxThemeName)
				{
					_uxThemeName = value;
					_NotifyPropertyChanged("UxThemeName");
				}
			}
		}

		public string UxThemeColor
		{
			get
			{
				return _uxThemeColor;
			}
			private set
			{
				if (value != _uxThemeColor)
				{
					_uxThemeColor = value;
					_NotifyPropertyChanged("UxThemeColor");
				}
			}
		}

		public bool HighContrast
		{
			get
			{
				return _isHighContrast;
			}
			private set
			{
				if (value != _isHighContrast)
				{
					_isHighContrast = value;
					_NotifyPropertyChanged("HighContrast");
				}
			}
		}

		public CornerRadius WindowCornerRadius
		{
			get
			{
				return _windowCornerRadius;
			}
			private set
			{
				if (value != _windowCornerRadius)
				{
					_windowCornerRadius = value;
					_NotifyPropertyChanged("WindowCornerRadius");
				}
			}
		}

		public Rect WindowCaptionButtonsLocation
		{
			get
			{
				return _captionButtonLocation;
			}
			private set
			{
				if (value != _captionButtonLocation)
				{
					_captionButtonLocation = value;
					_NotifyPropertyChanged("WindowCaptionButtonsLocation");
				}
			}
		}

		internal static int Dpi
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				if (!_dpiInitialized)
				{
					lock (_dpiLock)
					{
						if (!_dpiInitialized)
						{
							using (SafeDC safeDC = SafeDC.GetDesktop())
							{
								if (safeDC.DangerousGetHandle() == IntPtr.Zero)
								{
									throw new Win32Exception();
								}
								_dpi = NativeMethods.GetDeviceCaps(safeDC, DeviceCap.LOGPIXELSY);
								_dpiInitialized = true;
							}
						}
					}
				}
				return _dpi;
			}
		}

		internal static int DpiX
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				if (_setDpiX)
				{
					lock (_cacheValid)
					{
						if (_setDpiX)
						{
							_setDpiX = false;
							using (SafeDC safeDC = SafeDC.GetDesktop())
							{
								if (safeDC.DangerousGetHandle() == IntPtr.Zero)
								{
									throw new Win32Exception();
								}
								_dpiX = NativeMethods.GetDeviceCaps(safeDC, DeviceCap.LOGPIXELSX);
								_cacheValid[0] = true;
							}
						}
					}
				}
				return _dpiX;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void _InitializeIsGlassEnabled()
		{
			IsGlassEnabled = NativeMethods.DwmIsCompositionEnabled();
		}

		private void _UpdateIsGlassEnabled(IntPtr wParam, IntPtr lParam)
		{
			_InitializeIsGlassEnabled();
		}

		private void _InitializeGlassColor()
		{
			NativeMethods.DwmGetColorizationColor(out uint pcrColorization, out bool pfOpaqueBlend);
			pcrColorization = (uint)((int)pcrColorization | (pfOpaqueBlend ? (-16777216) : 0));
			WindowGlassColor = Utility.ColorFromArgbDword(pcrColorization);
			SolidColorBrush solidColorBrush = new SolidColorBrush(WindowGlassColor);
			solidColorBrush.Freeze();
			WindowGlassBrush = solidColorBrush;
		}

		private void _UpdateGlassColor(IntPtr wParam, IntPtr lParam)
		{
			bool flag = lParam != IntPtr.Zero;
			uint num = (uint)wParam.ToInt64();
			num = (uint)((int)num | (flag ? (-16777216) : 0));
			WindowGlassColor = Utility.ColorFromArgbDword(num);
			SolidColorBrush solidColorBrush = new SolidColorBrush(WindowGlassColor);
			solidColorBrush.Freeze();
			WindowGlassBrush = solidColorBrush;
		}

		private void _InitializeCaptionHeight()
		{
			Point devicePoint = new Point(0.0, (double)NativeMethods.GetSystemMetrics(SM.CYCAPTION));
			WindowCaptionHeight = DpiHelper.DevicePixelsToLogical(devicePoint, (double)DpiX / 96.0, (double)Dpi / 96.0).Y;
		}

		private void _UpdateCaptionHeight(IntPtr wParam, IntPtr lParam)
		{
			_InitializeCaptionHeight();
		}

		private void _InitializeWindowResizeBorderThickness()
		{
			Size size = DpiHelper.DeviceSizeToLogical(new Size((double)NativeMethods.GetSystemMetrics(SM.CXFRAME), (double)NativeMethods.GetSystemMetrics(SM.CYFRAME)), (double)DpiX / 96.0, (double)Dpi / 96.0);
			WindowResizeBorderThickness = new Thickness(size.Width, size.Height, size.Width, size.Height);
		}

		private void _UpdateWindowResizeBorderThickness(IntPtr wParam, IntPtr lParam)
		{
			_InitializeWindowResizeBorderThickness();
		}

		private void _InitializeWindowNonClientFrameThickness()
		{
			Size size = DpiHelper.DeviceSizeToLogical(new Size((double)NativeMethods.GetSystemMetrics(SM.CXFRAME), (double)NativeMethods.GetSystemMetrics(SM.CYFRAME)), (double)DpiX / 96.0, (double)Dpi / 96.0);
			int systemMetrics = NativeMethods.GetSystemMetrics(SM.CYCAPTION);
			double y = DpiHelper.DevicePixelsToLogical(new Point(0.0, (double)systemMetrics), (double)DpiX / 96.0, (double)Dpi / 96.0).Y;
			WindowNonClientFrameThickness = new Thickness(size.Width, size.Height + y, size.Width, size.Height);
		}

		private void _UpdateWindowNonClientFrameThickness(IntPtr wParam, IntPtr lParam)
		{
			_InitializeWindowNonClientFrameThickness();
		}

		private void _InitializeSmallIconSize()
		{
			SmallIconSize = new Size((double)NativeMethods.GetSystemMetrics(SM.CXSMICON), (double)NativeMethods.GetSystemMetrics(SM.CYSMICON));
		}

		private void _UpdateSmallIconSize(IntPtr wParam, IntPtr lParam)
		{
			_InitializeSmallIconSize();
		}

		private void _LegacyInitializeCaptionButtonLocation()
		{
			int systemMetrics = NativeMethods.GetSystemMetrics(SM.CXSIZE);
			int systemMetrics2 = NativeMethods.GetSystemMetrics(SM.CYSIZE);
			int num = NativeMethods.GetSystemMetrics(SM.CXFRAME) + NativeMethods.GetSystemMetrics(SM.CXEDGE);
			int num2 = NativeMethods.GetSystemMetrics(SM.CYFRAME) + NativeMethods.GetSystemMetrics(SM.CYEDGE);
			Rect windowCaptionButtonsLocation = new Rect(0.0, 0.0, (double)(systemMetrics * 3), (double)systemMetrics2);
			windowCaptionButtonsLocation.Offset((double)(-num) - windowCaptionButtonsLocation.Width, (double)num2);
			WindowCaptionButtonsLocation = windowCaptionButtonsLocation;
		}

		private void _InitializeCaptionButtonLocation()
		{
			if (!Utility.IsOSVistaOrNewer || !NativeMethods.IsThemeActive())
			{
				_LegacyInitializeCaptionButtonLocation();
			}
			else
			{
				TITLEBARINFOEX tITLEBARINFOEX = default(TITLEBARINFOEX);
				tITLEBARINFOEX.cbSize = Marshal.SizeOf(typeof(TITLEBARINFOEX));
				TITLEBARINFOEX tITLEBARINFOEX2 = tITLEBARINFOEX;
				IntPtr hglobal = Marshal.AllocHGlobal(tITLEBARINFOEX2.cbSize);
				try
				{
					Marshal.StructureToPtr((object)tITLEBARINFOEX2, hglobal, fDeleteOld: false);
					NativeMethods.ShowWindow(_messageHwnd.Handle, SW.SHOWNA);
					NativeMethods.SendMessage(_messageHwnd.Handle, WM.GETTITLEBARINFOEX, IntPtr.Zero, hglobal);
					tITLEBARINFOEX2 = (TITLEBARINFOEX)Marshal.PtrToStructure(hglobal, typeof(TITLEBARINFOEX));
				}
				finally
				{
					NativeMethods.ShowWindow(_messageHwnd.Handle, SW.HIDE);
					Utility.SafeFreeHGlobal(ref hglobal);
				}
				RECT rECT = RECT.Union(tITLEBARINFOEX2.rgrect_CloseButton, tITLEBARINFOEX2.rgrect_MinimizeButton);
				RECT windowRect = NativeMethods.GetWindowRect(_messageHwnd.Handle);
				Rect rect2 = WindowCaptionButtonsLocation = DpiHelper.DeviceRectToLogical(new Rect((double)(rECT.Left - windowRect.Width - windowRect.Left), (double)(rECT.Top - windowRect.Top), (double)rECT.Width, (double)rECT.Height), (double)DpiX / 96.0, (double)Dpi / 96.0);
			}
		}

		private void _UpdateCaptionButtonLocation(IntPtr wParam, IntPtr lParam)
		{
			_InitializeCaptionButtonLocation();
		}

		private void _InitializeHighContrast()
		{
			HIGHCONTRAST hIGHCONTRAST = NativeMethods.SystemParameterInfo_GetHIGHCONTRAST();
			HighContrast = ((hIGHCONTRAST.dwFlags & HCF.HIGHCONTRASTON) != (HCF)0);
		}

		private void _UpdateHighContrast(IntPtr wParam, IntPtr lParam)
		{
			_InitializeHighContrast();
		}

		private void _InitializeThemeInfo()
		{
			if (!NativeMethods.IsThemeActive())
			{
				UxThemeName = "Classic";
				UxThemeColor = "";
			}
			else
			{
				try
				{
					NativeMethods.GetCurrentThemeName(out string themeFileName, out string color, out string _);
					UxThemeName = Path.GetFileNameWithoutExtension(themeFileName);
					UxThemeColor = color;
				}
				catch (Exception)
				{
					UxThemeName = "Classic";
					UxThemeColor = "";
				}
			}
		}

		private void _UpdateThemeInfo(IntPtr wParam, IntPtr lParam)
		{
			_InitializeThemeInfo();
		}

		private void _InitializeWindowCornerRadius()
		{
			CornerRadius cornerRadius = default(CornerRadius);
			switch (UxThemeName.ToUpperInvariant())
			{
			case "LUNA":
				cornerRadius = new CornerRadius(6.0, 6.0, 0.0, 0.0);
				break;
			case "AERO":
				cornerRadius = ((!NativeMethods.DwmIsCompositionEnabled()) ? new CornerRadius(6.0, 6.0, 0.0, 0.0) : new CornerRadius(8.0));
				break;
			default:
				cornerRadius = new CornerRadius(0.0);
				break;
			}
			WindowCornerRadius = cornerRadius;
		}

		private void _UpdateWindowCornerRadius(IntPtr wParam, IntPtr lParam)
		{
			_InitializeWindowCornerRadius();
		}

		private SystemParameters2()
		{
			_messageHwnd = new MessageWindow((CS)0u, WS.DISABLED | WS.BORDER | WS.DLGFRAME | WS.SYSMENU | WS.THICKFRAME | WS.GROUP | WS.TABSTOP, WS_EX.None, new Rect(-16000.0, -16000.0, 100.0, 100.0), "", _WndProc);
			_messageHwnd.Dispatcher.ShutdownStarted += delegate
			{
				Utility.SafeDispose(ref _messageHwnd);
			};
			_InitializeIsGlassEnabled();
			_InitializeGlassColor();
			_InitializeCaptionHeight();
			_InitializeWindowNonClientFrameThickness();
			_InitializeWindowResizeBorderThickness();
			_InitializeCaptionButtonLocation();
			_InitializeSmallIconSize();
			_InitializeHighContrast();
			_InitializeThemeInfo();
			_InitializeWindowCornerRadius();
			_UpdateTable = new Dictionary<WM, List<_SystemMetricUpdate>>
			{
				{
					WM.THEMECHANGED,
					new List<_SystemMetricUpdate>
					{
						_UpdateThemeInfo,
						_UpdateHighContrast,
						_UpdateWindowCornerRadius,
						_UpdateCaptionButtonLocation
					}
				},
				{
					WM.WININICHANGE,
					new List<_SystemMetricUpdate>
					{
						_UpdateCaptionHeight,
						_UpdateWindowResizeBorderThickness,
						_UpdateSmallIconSize,
						_UpdateHighContrast,
						_UpdateWindowNonClientFrameThickness,
						_UpdateCaptionButtonLocation
					}
				},
				{
					WM.DWMNCRENDERINGCHANGED,
					new List<_SystemMetricUpdate>
					{
						_UpdateIsGlassEnabled
					}
				},
				{
					WM.DWMCOMPOSITIONCHANGED,
					new List<_SystemMetricUpdate>
					{
						_UpdateIsGlassEnabled
					}
				},
				{
					WM.DWMCOLORIZATIONCOLORCHANGED,
					new List<_SystemMetricUpdate>
					{
						_UpdateGlassColor
					}
				}
			};
		}

		private IntPtr _WndProc(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam)
		{
			if (_UpdateTable != null && _UpdateTable.TryGetValue(msg, out List<_SystemMetricUpdate> value))
			{
				foreach (_SystemMetricUpdate item in value)
				{
					item(wParam, lParam);
				}
			}
			return NativeMethods.DefWindowProc(hwnd, msg, wParam, lParam);
		}

		private void _NotifyPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
