using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	[Flags]
	public enum HCF
	{
		HIGHCONTRASTON = 0x1,
		AVAILABLE = 0x2,
		HOTKEYACTIVE = 0x4,
		CONFIRMHOTKEY = 0x8,
		HOTKEYSOUND = 0x10,
		INDICATOR = 0x20,
		HOTKEYAVAILABLE = 0x40
	}
}
