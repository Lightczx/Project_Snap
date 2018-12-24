using ControlzEx.Native;
using ControlzEx.Standard;
using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;

namespace MahApps.Metro.Behaviours
{
	public class WindowsSettingBehaviour : Behavior<MetroWindow>
	{
		protected override void OnAttached()
		{
			this.OnAttached();
			base.get_AssociatedObject().SourceInitialized += AssociatedObject_SourceInitialized;
		}

		protected override void OnDetaching()
		{
			CleanUp("from OnDetaching");
			this.OnDetaching();
		}

		private void AssociatedObject_Closed(object sender, EventArgs e)
		{
			CleanUp("from AssociatedObject closed event");
		}

		private void AssociatedObject_Closing(object sender, CancelEventArgs e)
		{
			SaveWindowState();
		}

		private void AssociatedObject_SourceInitialized(object sender, EventArgs e)
		{
			LoadWindowState();
			MetroWindow associatedObject = base.get_AssociatedObject();
			if (associatedObject != null)
			{
				associatedObject.StateChanged += AssociatedObject_StateChanged;
				associatedObject.Closing += AssociatedObject_Closing;
				associatedObject.Closed += AssociatedObject_Closed;
			}
		}

		private void AssociatedObject_StateChanged(object sender, EventArgs e)
		{
			MetroWindow associatedObject = base.get_AssociatedObject();
			if (associatedObject != null && associatedObject.WindowState == WindowState.Minimized)
			{
				SaveWindowState();
			}
		}

		private void CleanUp(string fromWhere)
		{
			MetroWindow associatedObject = base.get_AssociatedObject();
			if (associatedObject != null)
			{
				associatedObject.StateChanged -= AssociatedObject_StateChanged;
				associatedObject.Closing -= AssociatedObject_Closing;
				associatedObject.Closed -= AssociatedObject_Closed;
				associatedObject.SourceInitialized -= AssociatedObject_SourceInitialized;
			}
		}

		private void LoadWindowState()
		{
			MetroWindow associatedObject = base.get_AssociatedObject();
			if (associatedObject != null)
			{
				IWindowPlacementSettings windowPlacementSettings = associatedObject.GetWindowPlacementSettings();
				if (windowPlacementSettings != null && associatedObject.SaveWindowPosition)
				{
					try
					{
						windowPlacementSettings.Reload();
					}
					catch (Exception)
					{
						return;
					}
					if (windowPlacementSettings.Placement != null && !windowPlacementSettings.Placement.normalPosition.IsEmpty)
					{
						try
						{
							WINDOWPLACEMENT placement = windowPlacementSettings.Placement;
							placement.flags = 0;
							placement.showCmd = ((placement.showCmd == SW.SHOWMINIMIZED) ? SW.SHOWNORMAL : placement.showCmd);
							associatedObject.Left = (double)placement.normalPosition.Left;
							associatedObject.Top = (double)placement.normalPosition.Top;
							NativeMethods.SetWindowPlacement(new WindowInteropHelper(associatedObject).Handle, placement);
						}
						catch (Exception innerException)
						{
							throw new MahAppsException("Failed to set the window state from the settings file", innerException);
						}
					}
				}
			}
		}

		private void SaveWindowState()
		{
			MetroWindow associatedObject = base.get_AssociatedObject();
			if (associatedObject != null)
			{
				IWindowPlacementSettings windowPlacementSettings = associatedObject.GetWindowPlacementSettings();
				if (windowPlacementSettings != null && associatedObject.SaveWindowPosition)
				{
					IntPtr handle = new WindowInteropHelper(associatedObject).Handle;
					WINDOWPLACEMENT windowPlacement = NativeMethods.GetWindowPlacement(handle);
					if (windowPlacement.showCmd != 0 && windowPlacement.length > 0)
					{
						if (windowPlacement.showCmd == SW.SHOWNORMAL && UnsafeNativeMethods.GetWindowRect(handle, out RECT lpRect))
						{
							windowPlacement.normalPosition = lpRect;
						}
						if (!windowPlacement.normalPosition.IsEmpty)
						{
							windowPlacementSettings.Placement = windowPlacement;
						}
					}
					try
					{
						windowPlacementSettings.Save();
					}
					catch (Exception)
					{
					}
				}
			}
		}
	}
}
