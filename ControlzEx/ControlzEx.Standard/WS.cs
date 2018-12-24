using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	[Flags]
	public enum WS : uint
	{
		OVERLAPPED = 0x0,
		POPUP = 0x80000000,
		CHILD = 0x40000000,
		MINIMIZE = 0x20000000,
		VISIBLE = 0x10000000,
		DISABLED = 0x8000000,
		CLIPSIBLINGS = 0x4000000,
		CLIPCHILDREN = 0x2000000,
		MAXIMIZE = 0x1000000,
		BORDER = 0x800000,
		DLGFRAME = 0x400000,
		VSCROLL = 0x200000,
		HSCROLL = 0x100000,
		SYSMENU = 0x80000,
		THICKFRAME = 0x40000,
		GROUP = 0x20000,
		TABSTOP = 0x10000,
		MINIMIZEBOX = 0x20000,
		MAXIMIZEBOX = 0x10000,
		CAPTION = 0xC00000,
		TILED = 0x0,
		ICONIC = 0x20000000,
		SIZEBOX = 0x40000,
		TILEDWINDOW = 0xCF0000,
		OVERLAPPEDWINDOW = 0xCF0000,
		POPUPWINDOW = 0x80880000,
		CHILDWINDOW = 0x40000000
	}
}
