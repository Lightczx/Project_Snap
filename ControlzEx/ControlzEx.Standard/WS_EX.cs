using System;

namespace ControlzEx.Standard
{
	[Flags]
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public enum WS_EX : uint
	{
		None = 0x0,
		DLGMODALFRAME = 0x1,
		NOPARENTNOTIFY = 0x4,
		TOPMOST = 0x8,
		ACCEPTFILES = 0x10,
		TRANSPARENT = 0x20,
		MDICHILD = 0x40,
		TOOLWINDOW = 0x80,
		WINDOWEDGE = 0x100,
		CLIENTEDGE = 0x200,
		CONTEXTHELP = 0x400,
		RIGHT = 0x1000,
		LEFT = 0x0,
		RTLREADING = 0x2000,
		LTRREADING = 0x0,
		LEFTSCROLLBAR = 0x4000,
		RIGHTSCROLLBAR = 0x0,
		CONTROLPARENT = 0x10000,
		STATICEDGE = 0x20000,
		APPWINDOW = 0x40000,
		LAYERED = 0x80000,
		NOINHERITLAYOUT = 0x100000,
		LAYOUTRTL = 0x400000,
		COMPOSITED = 0x2000000,
		NOACTIVATE = 0x8000000,
		OVERLAPPEDWINDOW = 0x300,
		PALETTEWINDOW = 0x188
	}
}
