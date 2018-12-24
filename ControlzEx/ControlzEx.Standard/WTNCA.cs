using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	[Flags]
	public enum WTNCA : uint
	{
		NODRAWCAPTION = 0x1,
		NODRAWICON = 0x2,
		NOSYSMENU = 0x4,
		NOMIRRORHELP = 0x8,
		VALIDBITS = 0xF
	}
}
