using ControlzEx.Standard;
using System.Configuration;
using System.Windows;

namespace MahApps.Metro.Controls
{
	internal class WindowApplicationSettings : ApplicationSettingsBase, IWindowPlacementSettings
	{
		[UserScopedSetting]
		public WINDOWPLACEMENT Placement
		{
			get
			{
				if (this["Placement"] != null)
				{
					return (WINDOWPLACEMENT)this["Placement"];
				}
				return null;
			}
			set
			{
				this["Placement"] = value;
			}
		}

		[UserScopedSetting]
		public bool UpgradeSettings
		{
			get
			{
				try
				{
					if (this["UpgradeSettings"] != null)
					{
						return (bool)this["UpgradeSettings"];
					}
				}
				catch (ConfigurationErrorsException ex)
				{
					ConfigurationErrorsException ex2 = ex;
					string text = null;
					while (ex2 != null && (text = ex2.Filename) == null)
					{
						ex2 = (ex2.InnerException as ConfigurationErrorsException);
					}
					throw new MahAppsException(string.Format("The settings file '{0}' seems to be corrupted", text ?? "<unknown>"), ex2);
				}
				return true;
			}
			set
			{
				this["UpgradeSettings"] = value;
			}
		}

		public WindowApplicationSettings(Window window)
			: base(window.GetType().FullName)
		{
		}

		void IWindowPlacementSettings.Reload()
		{
			Reload();
		}
	}
}
