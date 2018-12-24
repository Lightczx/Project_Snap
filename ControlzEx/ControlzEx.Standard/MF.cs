using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	[Flags]
	public enum MF : uint
	{
		DOES_NOT_EXIST = uint.MaxValue,
		ENABLED = 0x0,
		BYCOMMAND = 0x0,
		GRAYED = 0x1,
		DISABLED = 0x2
	}
}
