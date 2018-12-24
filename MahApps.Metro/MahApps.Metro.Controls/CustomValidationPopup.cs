using ControlzEx.Native;
using ControlzEx.Standard;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;

namespace MahApps.Metro.Controls
{
	public class CustomValidationPopup : Popup
	{
		public static readonly DependencyProperty CloseOnMouseLeftButtonDownProperty = DependencyProperty.Register("CloseOnMouseLeftButtonDown", typeof(bool), typeof(CustomValidationPopup), new PropertyMetadata(true));

		private Window hostWindow;

		private bool? appliedTopMost;

		public bool CloseOnMouseLeftButtonDown
		{
			get
			{
				return (bool)GetValue(CloseOnMouseLeftButtonDownProperty);
			}
			set
			{
				SetValue(CloseOnMouseLeftButtonDownProperty, value);
			}
		}

		public CustomValidationPopup()
		{
			base.Loaded += CustomValidationPopup_Loaded;
			base.Opened += CustomValidationPopup_Opened;
		}

		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (CloseOnMouseLeftButtonDown)
			{
				SetCurrentValue(Popup.IsOpenProperty, false);
			}
		}

		private void CustomValidationPopup_Loaded(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = base.PlacementTarget as FrameworkElement;
			if (frameworkElement != null)
			{
				hostWindow = Window.GetWindow(frameworkElement);
				if (hostWindow != null)
				{
					hostWindow.LocationChanged -= hostWindow_SizeOrLocationChanged;
					hostWindow.LocationChanged += hostWindow_SizeOrLocationChanged;
					hostWindow.SizeChanged -= hostWindow_SizeOrLocationChanged;
					hostWindow.SizeChanged += hostWindow_SizeOrLocationChanged;
					frameworkElement.SizeChanged -= hostWindow_SizeOrLocationChanged;
					frameworkElement.SizeChanged += hostWindow_SizeOrLocationChanged;
					hostWindow.StateChanged -= hostWindow_StateChanged;
					hostWindow.StateChanged += hostWindow_StateChanged;
					hostWindow.Activated -= hostWindow_Activated;
					hostWindow.Activated += hostWindow_Activated;
					hostWindow.Deactivated -= hostWindow_Deactivated;
					hostWindow.Deactivated += hostWindow_Deactivated;
					base.Unloaded -= CustomValidationPopup_Unloaded;
					base.Unloaded += CustomValidationPopup_Unloaded;
				}
			}
		}

		private void CustomValidationPopup_Opened(object sender, EventArgs e)
		{
			SetTopmostState(isTop: true);
		}

		private void hostWindow_Activated(object sender, EventArgs e)
		{
			SetTopmostState(isTop: true);
		}

		private void hostWindow_Deactivated(object sender, EventArgs e)
		{
			SetTopmostState(isTop: false);
		}

		private void CustomValidationPopup_Unloaded(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = base.PlacementTarget as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.SizeChanged -= hostWindow_SizeOrLocationChanged;
			}
			if (hostWindow != null)
			{
				hostWindow.LocationChanged -= hostWindow_SizeOrLocationChanged;
				hostWindow.SizeChanged -= hostWindow_SizeOrLocationChanged;
				hostWindow.StateChanged -= hostWindow_StateChanged;
				hostWindow.Activated -= hostWindow_Activated;
				hostWindow.Deactivated -= hostWindow_Deactivated;
			}
			base.Unloaded -= CustomValidationPopup_Unloaded;
			base.Opened -= CustomValidationPopup_Opened;
			hostWindow = null;
		}

		private void hostWindow_StateChanged(object sender, EventArgs e)
		{
			if (hostWindow != null && hostWindow.WindowState != WindowState.Minimized)
			{
				FrameworkElement frameworkElement = base.PlacementTarget as FrameworkElement;
				AdornedElementPlaceholder adornedElementPlaceholder = (frameworkElement != null) ? (frameworkElement.DataContext as AdornedElementPlaceholder) : null;
				if (adornedElementPlaceholder != null && adornedElementPlaceholder.AdornedElement != null)
				{
					base.PopupAnimation = PopupAnimation.None;
					base.IsOpen = false;
					object value = adornedElementPlaceholder.AdornedElement.GetValue(Validation.ErrorTemplateProperty);
					adornedElementPlaceholder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, null);
					adornedElementPlaceholder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, value);
				}
			}
		}

		private void hostWindow_SizeOrLocationChanged(object sender, EventArgs e)
		{
			double horizontalOffset = base.HorizontalOffset;
			base.HorizontalOffset = horizontalOffset + 1.0;
			base.HorizontalOffset = horizontalOffset;
		}

		private void SetTopmostState(bool isTop)
		{
			if ((!appliedTopMost.HasValue || appliedTopMost != isTop) && base.Child != null)
			{
				HwndSource hwndSource = PresentationSource.FromVisual(base.Child) as HwndSource;
				if (hwndSource != null)
				{
					IntPtr handle = hwndSource.Handle;
					if (UnsafeNativeMethods.GetWindowRect(handle, out RECT lpRect))
					{
						int left = lpRect.Left;
						int top = lpRect.Top;
						int width = lpRect.Width;
						int height = lpRect.Height;
						if (isTop)
						{
							NativeMethods.SetWindowPos(handle, Constants.HWND_TOPMOST, left, top, width, height, SWP.TOPMOST);
						}
						else
						{
							NativeMethods.SetWindowPos(handle, Constants.HWND_BOTTOM, left, top, width, height, SWP.TOPMOST);
							NativeMethods.SetWindowPos(handle, Constants.HWND_TOP, left, top, width, height, SWP.TOPMOST);
							NativeMethods.SetWindowPos(handle, Constants.HWND_NOTOPMOST, left, top, width, height, SWP.TOPMOST);
						}
						appliedTopMost = isTop;
					}
				}
			}
		}
	}
}
