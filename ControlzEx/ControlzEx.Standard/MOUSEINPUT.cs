using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public struct MOUSEINPUT
	{
		public int dx;

		public int dy;

		public int mouseData;

		public int dwFlags;

		public int time;

		public IntPtr dwExtraInfo;
	}
}
