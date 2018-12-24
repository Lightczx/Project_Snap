using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	[Flags]
	public enum NIF : uint
	{
		MESSAGE = 0x1,
		ICON = 0x2,
		TIP = 0x4,
		STATE = 0x8,
		INFO = 0x10,
		GUID = 0x20,
		REALTIME = 0x40,
		SHOWTIP = 0x80,
		XP_MASK = 0x3B,
		VISTA_MASK = 0xFB
	}
}
