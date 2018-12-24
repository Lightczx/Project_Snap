using System;

namespace MahApps.Metro
{
	public class OnThemeChangedEventArgs : EventArgs
	{
		public AppTheme AppTheme
		{
			get;
			set;
		}

		public Accent Accent
		{
			get;
			set;
		}

		public OnThemeChangedEventArgs(AppTheme appTheme, Accent accent)
		{
			AppTheme = appTheme;
			Accent = accent;
		}
	}
}
