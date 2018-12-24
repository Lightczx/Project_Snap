using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public struct WINDOWINFO
	{
		public int cbSize;

		public RECT rcWindow;

		public RECT rcClient;

		public int dwStyle;

		public int dwExStyle;

		public uint dwWindowStatus;

		public uint cxWindowBorders;

		public uint cyWindowBorders;

		public ushort atomWindowType;

		public ushort wCreatorVersion;
	}
}
