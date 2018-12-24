using ControlzEx.Native;
using ControlzEx.Standard;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;

namespace MahApps.Metro.Controls
{
	[Obsolete("This class is obsolete and will be deleted at the next major release.")]
	public class WindowSettings
	{
		public static readonly DependencyProperty WindowPlacementSettingsProperty = DependencyProperty.RegisterAttached("WindowPlacementSettings", typeof(IWindowPlacementSettings), typeof(WindowSettings), new FrameworkPropertyMetadata(OnWindowPlacementSettingsInvalidated));

		private Window _window;

		private IWindowPlacementSettings _settings;

		public static void SetSave(DependencyObject dependencyObject, IWindowPlacementSettings windowPlacementSettings)
		{
			SetWindowPlacementSettings(dependencyObject, windowPlacementSettings);
		}

		public static void SetWindowPlacementSettings(DependencyObject dependencyObject, IWindowPlacementSettings windowPlacementSettings)
		{
			dependencyObject.SetValue(WindowPlacementSettingsProperty, windowPlacementSettings);
		}

		private static void OnWindowPlacementSettingsInvalidated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Window window = dependencyObject as Window;
			if (window != null && e.NewValue is IWindowPlacementSettings)
			{
				new WindowSettings(window, (IWindowPlacementSettings)e.NewValue).Attach();
			}
		}

		public WindowSettings(Window window, IWindowPlacementSettings windowPlacementSettings)
		{
			_window = window;
			_settings = windowPlacementSettings;
		}

		protected virtual void LoadWindowState()
		{
			if (_settings != null)
			{
				_settings.Reload();
				if (_settings.Placement != null && !_settings.Placement.normalPosition.IsEmpty)
				{
					try
					{
						WINDOWPLACEMENT placement = _settings.Placement;
						placement.flags = 0;
						placement.showCmd = ((placement.showCmd == SW.SHOWMINIMIZED) ? SW.SHOWNORMAL : placement.showCmd);
						NativeMethods.SetWindowPlacement(new WindowInteropHelper(_window).Handle, placement);
					}
					catch (Exception innerException)
					{
						throw new MahAppsException("Failed to set the window state from the settings file", innerException);
					}
				}
			}
		}

		protected virtual void SaveWindowState()
		{
			if (_settings != null)
			{
				IntPtr handle = new WindowInteropHelper(_window).Handle;
				WINDOWPLACEMENT windowPlacement = NativeMethods.GetWindowPlacement(handle);
				if (windowPlacement.showCmd != 0 && windowPlacement.length > 0)
				{
					if (windowPlacement.showCmd == SW.SHOWNORMAL && UnsafeNativeMethods.GetWindowRect(handle, out RECT lpRect))
					{
						windowPlacement.normalPosition = lpRect;
					}
					if (!windowPlacement.normalPosition.IsEmpty)
					{
						_settings.Placement = windowPlacement;
					}
				}
				_settings.Save();
			}
		}

		private void Attach()
		{
			if (_window != null)
			{
				_window.SourceInitialized += WindowSourceInitialized;
				_window.Closed += WindowClosed;
			}
		}

		private void WindowSourceInitialized(object sender, EventArgs e)
		{
			LoadWindowState();
			_window.StateChanged += WindowStateChanged;
			_window.Closing += WindowClosing;
		}

		private void WindowStateChanged(object sender, EventArgs e)
		{
			if (_window.WindowState == WindowState.Minimized)
			{
				SaveWindowState();
			}
		}

		private void WindowClosing(object sender, CancelEventArgs e)
		{
			SaveWindowState();
		}

		private void WindowClosed(object sender, EventArgs e)
		{
			SaveWindowState();
			_window.StateChanged -= WindowStateChanged;
			_window.Closing -= WindowClosing;
			_window.Closed -= WindowClosed;
			_window.SourceInitialized -= WindowSourceInitialized;
			_window = null;
			_settings = null;
		}
	}
}
