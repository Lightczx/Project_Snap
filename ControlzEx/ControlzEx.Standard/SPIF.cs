using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	[Flags]
	public enum SPIF
	{
		None = 0x0,
		UPDATEINIFILE = 0x1,
		SENDWININICHANGE = 0x2
	}
}
