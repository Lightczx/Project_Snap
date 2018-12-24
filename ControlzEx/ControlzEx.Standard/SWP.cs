using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	[Flags]
	public enum SWP
	{
		ASYNCWINDOWPOS = 0x4000,
		DEFERERASE = 0x2000,
		DRAWFRAME = 0x20,
		FRAMECHANGED = 0x20,
		HIDEWINDOW = 0x80,
		NOACTIVATE = 0x10,
		NOCOPYBITS = 0x100,
		NOMOVE = 0x2,
		NOOWNERZORDER = 0x200,
		NOREDRAW = 0x8,
		NOREPOSITION = 0x200,
		NOSENDCHANGING = 0x400,
		NOSIZE = 0x1,
		NOZORDER = 0x4,
		SHOWWINDOW = 0x40,
		TOPMOST = 0x61B
	}
}
