using ControlzEx.Native;
using ControlzEx.Standard;
using ControlzEx.Windows.Shell;
using System;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace ControlzEx.Behaviors
{
	public class WindowChromeBehavior : Behavior<Window>
	{
		private IntPtr handle;

		private HwndSource hwndSource;

		private WindowChrome windowChrome;

		private PropertyChangeNotifier topMostChangeNotifier;

		private PropertyChangeNotifier borderThicknessChangeNotifier;

		private PropertyChangeNotifier resizeBorderThicknessChangeNotifier;

		private Thickness? savedBorderThickness;

		private Thickness? savedResizeBorderThickness;

		private bool savedTopMost;

		private bool isWindwos10OrHigher;

		public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register("ResizeBorderThickness", typeof(Thickness), typeof(WindowChromeBehavior), new PropertyMetadata(GetDefaultResizeBorderThickness()));

		public static readonly DependencyProperty GlassFrameThicknessProperty = DependencyProperty.Register("GlassFrameThickness", typeof(Thickness), typeof(WindowChromeBehavior), new PropertyMetadata(default(Thickness), OnGlassFrameThicknessChanged));

		public static readonly DependencyProperty GlowBrushProperty = DependencyProperty.Register("GlowBrush", typeof(Brush), typeof(WindowChromeBehavior), new PropertyMetadata());

		public static readonly DependencyProperty IgnoreTaskbarOnMaximizeProperty = DependencyProperty.Register("IgnoreTaskbarOnMaximize", typeof(bool), typeof(WindowChromeBehavior), new PropertyMetadata(false, OnIgnoreTaskbarOnMaximizePropertyChanged));

		public static readonly DependencyProperty KeepBorderOnMaximizeProperty = DependencyProperty.Register("KeepBorderOnMaximize", typeof(bool), typeof(WindowChromeBehavior), new PropertyMetadata(true, OnKeepBorderOnMaximizeChanged));

		private bool isCleanedUp;

		private IntPtr taskbarHandle;

		public Thickness ResizeBorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(ResizeBorderThicknessProperty);
			}
			set
			{
				base.SetValue(ResizeBorderThicknessProperty, (object)value);
			}
		}

		public Thickness GlassFrameThickness
		{
			get
			{
				return (Thickness)base.GetValue(GlassFrameThicknessProperty);
			}
			set
			{
				base.SetValue(GlassFrameThicknessProperty, (object)value);
			}
		}

		public Brush GlowBrush
		{
			get
			{
				return (Brush)base.GetValue(GlowBrushProperty);
			}
			set
			{
				base.SetValue(GlowBrushProperty, (object)value);
			}
		}

		public bool IgnoreTaskbarOnMaximize
		{
			get
			{
				return (bool)base.GetValue(IgnoreTaskbarOnMaximizeProperty);
			}
			set
			{
				base.SetValue(IgnoreTaskbarOnMaximizeProperty, (object)value);
			}
		}

		public bool KeepBorderOnMaximize
		{
			get
			{
				return (bool)base.GetValue(KeepBorderOnMaximizeProperty);
			}
			set
			{
				base.SetValue(KeepBorderOnMaximizeProperty, (object)value);
			}
		}

		private static bool IsWindows10OrHigher()
		{
			Version version = NtDll.RtlGetVersion();
			if (null == version)
			{
				using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT Caption, Version FROM Win32_OperatingSystem"))
				{
					version = new Version((managementObjectSearcher.Get().OfType<ManagementObject>().FirstOrDefault()?.GetPropertyValue("Version") ?? "0.0.0.0").ToString());
				}
			}
			return version >= new Version(10, 0);
		}

		protected override void OnAttached()
		{
			isWindwos10OrHigher = IsWindows10OrHigher();
			InitializeWindowChrome();
			if (base.get_AssociatedObject().AllowsTransparency && !base.get_AssociatedObject().IsLoaded && new WindowInteropHelper(base.get_AssociatedObject()).Handle == IntPtr.Zero)
			{
				try
				{
					base.get_AssociatedObject().AllowsTransparency = false;
				}
				catch (Exception)
				{
				}
			}
			base.get_AssociatedObject().WindowStyle = WindowStyle.None;
			savedBorderThickness = base.get_AssociatedObject().BorderThickness;
			borderThicknessChangeNotifier = new PropertyChangeNotifier(base.get_AssociatedObject(), Control.BorderThicknessProperty);
			borderThicknessChangeNotifier.ValueChanged += BorderThicknessChangeNotifierOnValueChanged;
			savedResizeBorderThickness = ResizeBorderThickness;
			resizeBorderThicknessChangeNotifier = new PropertyChangeNotifier((DependencyObject)this, ResizeBorderThicknessProperty);
			resizeBorderThicknessChangeNotifier.ValueChanged += ResizeBorderThicknessChangeNotifierOnValueChanged;
			savedTopMost = base.get_AssociatedObject().Topmost;
			topMostChangeNotifier = new PropertyChangeNotifier(base.get_AssociatedObject(), Window.TopmostProperty);
			topMostChangeNotifier.ValueChanged += TopMostChangeNotifierOnValueChanged;
			base.get_AssociatedObject().SourceInitialized += AssociatedObject_SourceInitialized;
			base.get_AssociatedObject().Loaded += AssociatedObject_Loaded;
			base.get_AssociatedObject().Unloaded += AssociatedObject_Unloaded;
			base.get_AssociatedObject().Closed += AssociatedObject_Closed;
			base.get_AssociatedObject().StateChanged += AssociatedObject_StateChanged;
			base.get_AssociatedObject().LostFocus += AssociatedObject_LostFocus;
			base.get_AssociatedObject().Deactivated += AssociatedObject_Deactivated;
			this.OnAttached();
		}

		private void TopMostHack()
		{
			if (base.get_AssociatedObject().Topmost)
			{
				bool raiseValueChanged = topMostChangeNotifier.RaiseValueChanged;
				topMostChangeNotifier.RaiseValueChanged = false;
				base.get_AssociatedObject().Topmost = false;
				base.get_AssociatedObject().Topmost = true;
				topMostChangeNotifier.RaiseValueChanged = raiseValueChanged;
			}
		}

		private void InitializeWindowChrome()
		{
			windowChrome = new WindowChrome();
			BindingOperations.SetBinding(windowChrome, WindowChrome.ResizeBorderThicknessProperty, new Binding
			{
				Path = new PropertyPath(ResizeBorderThicknessProperty),
				Source = this
			});
			BindingOperations.SetBinding(windowChrome, WindowChrome.GlassFrameThicknessProperty, new Binding
			{
				Path = new PropertyPath(GlassFrameThicknessProperty),
				Source = this
			});
			windowChrome.CaptionHeight = 0.0;
			windowChrome.CornerRadius = default(CornerRadius);
			windowChrome.UseAeroCaptionButtons = false;
			base.get_AssociatedObject().SetValue(WindowChrome.WindowChromeProperty, windowChrome);
		}

		public static Thickness GetDefaultResizeBorderThickness()
		{
			return SystemParameters.WindowResizeBorderThickness;
		}

		private void BorderThicknessChangeNotifierOnValueChanged(object sender, EventArgs e)
		{
			Window associatedObject = base.get_AssociatedObject();
			if (associatedObject != null)
			{
				savedBorderThickness = associatedObject.BorderThickness;
			}
		}

		private void ResizeBorderThicknessChangeNotifierOnValueChanged(object sender, EventArgs e)
		{
			savedResizeBorderThickness = ResizeBorderThickness;
		}

		private void TopMostChangeNotifierOnValueChanged(object sender, EventArgs e)
		{
			Window associatedObject = base.get_AssociatedObject();
			if (associatedObject != null)
			{
				savedTopMost = associatedObject.Topmost;
			}
		}

		private static void OnGlassFrameThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			WindowChromeBehavior windowChromeBehavior = (WindowChromeBehavior)d;
			if (windowChromeBehavior.get_AssociatedObject() != null)
			{
				windowChromeBehavior.get_AssociatedObject().SetValue(WindowChrome.WindowChromeProperty, null);
				windowChromeBehavior.InitializeWindowChrome();
			}
		}

		private static void OnIgnoreTaskbarOnMaximizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			WindowChromeBehavior windowChromeBehavior = (WindowChromeBehavior)d;
			if (windowChromeBehavior.windowChrome != null && !object.Equals(windowChromeBehavior.windowChrome.IgnoreTaskbarOnMaximize, windowChromeBehavior.IgnoreTaskbarOnMaximize))
			{
				windowChromeBehavior.windowChrome.IgnoreTaskbarOnMaximize = windowChromeBehavior.IgnoreTaskbarOnMaximize;
				if (windowChromeBehavior.get_AssociatedObject().WindowState == WindowState.Maximized)
				{
					windowChromeBehavior.get_AssociatedObject().WindowState = WindowState.Normal;
					windowChromeBehavior.get_AssociatedObject().WindowState = WindowState.Maximized;
				}
			}
		}

		private static void OnKeepBorderOnMaximizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((WindowChromeBehavior)d).HandleMaximize();
		}

		private void Cleanup()
		{
			if (!isCleanedUp)
			{
				isCleanedUp = true;
				if (taskbarHandle != IntPtr.Zero && isWindwos10OrHigher)
				{
					DeactivateTaskbarFix(taskbarHandle);
				}
				base.get_AssociatedObject().SourceInitialized -= AssociatedObject_SourceInitialized;
				base.get_AssociatedObject().Loaded -= AssociatedObject_Loaded;
				base.get_AssociatedObject().Unloaded -= AssociatedObject_Unloaded;
				base.get_AssociatedObject().Closed -= AssociatedObject_Closed;
				base.get_AssociatedObject().StateChanged -= AssociatedObject_StateChanged;
				base.get_AssociatedObject().LostFocus -= AssociatedObject_LostFocus;
				base.get_AssociatedObject().Deactivated -= AssociatedObject_Deactivated;
				hwndSource?.RemoveHook(this.WindowProc);
				windowChrome = null;
			}
		}

		protected override void OnDetaching()
		{
			Cleanup();
			this.OnDetaching();
		}

		private void AssociatedObject_SourceInitialized(object sender, EventArgs e)
		{
			handle = new WindowInteropHelper(base.get_AssociatedObject()).Handle;
			if (IntPtr.Zero == handle)
			{
				throw new Exception("Uups, at this point we really need the Handle from the associated object!");
			}
			if (base.get_AssociatedObject().SizeToContent != 0 && base.get_AssociatedObject().WindowState == WindowState.Normal)
			{
				Invoke(base.get_AssociatedObject(), delegate
				{
					base.get_AssociatedObject().InvalidateMeasure();
					if (UnsafeNativeMethods.GetWindowRect(handle, out RECT lpRect))
					{
						SWP sWP = SWP.SHOWWINDOW;
						if (!base.get_AssociatedObject().ShowActivated)
						{
							sWP |= SWP.NOACTIVATE;
						}
						NativeMethods.SetWindowPos(handle, Constants.HWND_NOTOPMOST, lpRect.Left, lpRect.Top, lpRect.Width, lpRect.Height, sWP);
					}
				});
			}
			hwndSource = HwndSource.FromHwnd(handle);
			hwndSource?.AddHook(this.WindowProc);
			HandleMaximize();
		}

		protected virtual void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
		{
		}

		private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
		{
			Cleanup();
		}

		private void AssociatedObject_Closed(object sender, EventArgs e)
		{
			Cleanup();
		}

		private void AssociatedObject_StateChanged(object sender, EventArgs e)
		{
			HandleMaximize();
		}

		private void AssociatedObject_Deactivated(object sender, EventArgs e)
		{
			TopMostHack();
		}

		private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
		{
			TopMostHack();
		}

		private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			IntPtr zero = IntPtr.Zero;
			switch (msg)
			{
			case 133:
				handled = (GlassFrameThickness == default(Thickness) && GlowBrush == null);
				break;
			case 70:
			{
				WINDOWPOS wINDOWPOS = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
				if ((wINDOWPOS.flags & SWP.NOMOVE) != 0)
				{
					return IntPtr.Zero;
				}
				Window associatedObject = base.get_AssociatedObject();
				if (associatedObject == null || hwndSource?.CompositionTarget == null)
				{
					return IntPtr.Zero;
				}
				bool flag = false;
				Matrix transformToDevice = hwndSource.CompositionTarget.TransformToDevice;
				double num = associatedObject.MinWidth * transformToDevice.M11;
				double num2 = associatedObject.MinHeight * transformToDevice.M22;
				if ((double)wINDOWPOS.cx < num)
				{
					wINDOWPOS.cx = (int)num;
					flag = true;
				}
				if ((double)wINDOWPOS.cy < num2)
				{
					wINDOWPOS.cy = (int)num2;
					flag = true;
				}
				double num3 = associatedObject.MaxWidth * transformToDevice.M11;
				double num4 = associatedObject.MaxHeight * transformToDevice.M22;
				if ((double)wINDOWPOS.cx > num3 && num3 > 0.0)
				{
					wINDOWPOS.cx = (int)Math.Round(num3);
					flag = true;
				}
				if ((double)wINDOWPOS.cy > num4 && num4 > 0.0)
				{
					wINDOWPOS.cy = (int)Math.Round(num4);
					flag = true;
				}
				if (!flag)
				{
					return IntPtr.Zero;
				}
				Marshal.StructureToPtr((object)wINDOWPOS, lParam, fDeleteOld: true);
				handled = true;
				break;
			}
			}
			return zero;
		}

		private void HandleMaximize()
		{
			bool raiseValueChanged = topMostChangeNotifier.RaiseValueChanged;
			topMostChangeNotifier.RaiseValueChanged = false;
			HandleBorderAndResizeBorderThicknessDuringMaximize();
			if (base.get_AssociatedObject().WindowState == WindowState.Maximized)
			{
				if (handle != IntPtr.Zero)
				{
					IntPtr intPtr = UnsafeNativeMethods.MonitorFromWindow(handle, MonitorOptions.MONITOR_DEFAULTTONEAREST);
					if (intPtr != IntPtr.Zero)
					{
						MONITORINFO monitorInfo = NativeMethods.GetMonitorInfo(intPtr);
						RECT rECT = IgnoreTaskbarOnMaximize ? monitorInfo.rcMonitor : monitorInfo.rcWork;
						int left = rECT.Left;
						int top = rECT.Top;
						int width = rECT.Width;
						int height = rECT.Height;
						if (IgnoreTaskbarOnMaximize && isWindwos10OrHigher)
						{
							ActivateTaskbarFix(intPtr);
						}
						NativeMethods.SetWindowPos(handle, Constants.HWND_NOTOPMOST, left, top, width, height, SWP.SHOWWINDOW);
					}
				}
			}
			else if (taskbarHandle != IntPtr.Zero && isWindwos10OrHigher)
			{
				DeactivateTaskbarFix(taskbarHandle);
			}
			base.get_AssociatedObject().Topmost = false;
			base.get_AssociatedObject().Topmost = (base.get_AssociatedObject().WindowState == WindowState.Minimized || savedTopMost);
			topMostChangeNotifier.RaiseValueChanged = raiseValueChanged;
		}

		private void HandleBorderAndResizeBorderThicknessDuringMaximize()
		{
			borderThicknessChangeNotifier.RaiseValueChanged = false;
			resizeBorderThicknessChangeNotifier.RaiseValueChanged = false;
			if (base.get_AssociatedObject().WindowState == WindowState.Maximized)
			{
				IntPtr intPtr = IntPtr.Zero;
				if (handle != IntPtr.Zero)
				{
					intPtr = UnsafeNativeMethods.MonitorFromWindow(handle, MonitorOptions.MONITOR_DEFAULTTONEAREST);
				}
				if (intPtr != IntPtr.Zero)
				{
					MONITORINFO monitorInfo = NativeMethods.GetMonitorInfo(intPtr);
					RECT rECT = IgnoreTaskbarOnMaximize ? monitorInfo.rcMonitor : monitorInfo.rcWork;
					double right = 0.0;
					double bottom = 0.0;
					if (KeepBorderOnMaximize && savedBorderThickness.HasValue)
					{
						if (base.get_AssociatedObject().MaxWidth < (double)rECT.Width)
						{
							right = savedBorderThickness.Value.Right;
						}
						if (base.get_AssociatedObject().MaxHeight < (double)rECT.Height)
						{
							bottom = savedBorderThickness.Value.Bottom;
						}
					}
					base.get_AssociatedObject().BorderThickness = new Thickness(0.0, 0.0, right, bottom);
				}
				else
				{
					base.get_AssociatedObject().BorderThickness = new Thickness(0.0);
				}
				windowChrome.ResizeBorderThickness = new Thickness(0.0);
			}
			else
			{
				base.get_AssociatedObject().BorderThickness = savedBorderThickness.GetValueOrDefault(new Thickness(0.0));
				Thickness valueOrDefault = savedResizeBorderThickness.GetValueOrDefault(new Thickness(0.0));
				if (windowChrome.ResizeBorderThickness != valueOrDefault)
				{
					windowChrome.ResizeBorderThickness = valueOrDefault;
				}
			}
			borderThicknessChangeNotifier.RaiseValueChanged = true;
			resizeBorderThicknessChangeNotifier.RaiseValueChanged = true;
		}

		private void ActivateTaskbarFix(IntPtr monitor)
		{
			IntPtr taskBarHandleForMonitor = NativeMethods.GetTaskBarHandleForMonitor(monitor);
			if (taskBarHandleForMonitor != IntPtr.Zero)
			{
				taskbarHandle = taskBarHandleForMonitor;
				NativeMethods.SetWindowPos(taskBarHandleForMonitor, Constants.HWND_BOTTOM, 0, 0, 0, 0, SWP.TOPMOST);
				NativeMethods.SetWindowPos(taskBarHandleForMonitor, Constants.HWND_TOP, 0, 0, 0, 0, SWP.TOPMOST);
				NativeMethods.SetWindowPos(taskBarHandleForMonitor, Constants.HWND_NOTOPMOST, 0, 0, 0, 0, SWP.TOPMOST);
			}
		}

		private void DeactivateTaskbarFix(IntPtr trayWndHandle)
		{
			if (trayWndHandle != IntPtr.Zero)
			{
				taskbarHandle = IntPtr.Zero;
				NativeMethods.SetWindowPos(trayWndHandle, Constants.HWND_BOTTOM, 0, 0, 0, 0, SWP.TOPMOST);
				NativeMethods.SetWindowPos(trayWndHandle, Constants.HWND_TOP, 0, 0, 0, 0, SWP.TOPMOST);
				NativeMethods.SetWindowPos(trayWndHandle, Constants.HWND_TOPMOST, 0, 0, 0, 0, SWP.TOPMOST);
			}
		}

		private static void Invoke(DispatcherObject dispatcherObject, Action invokeAction)
		{
			if (dispatcherObject == null)
			{
				throw new ArgumentNullException("dispatcherObject");
			}
			if (invokeAction == null)
			{
				throw new ArgumentNullException("invokeAction");
			}
			if (dispatcherObject.Dispatcher.CheckAccess())
			{
				invokeAction();
			}
			else
			{
				dispatcherObject.Dispatcher.Invoke(invokeAction);
			}
		}
	}
}
