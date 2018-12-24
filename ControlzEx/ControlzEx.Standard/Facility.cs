using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public enum Facility
	{
		Null = 0,
		Rpc = 1,
		Dispatch = 2,
		Storage = 3,
		Itf = 4,
		Win32 = 7,
		Windows = 8,
		Control = 10,
		Ese = 3678,
		WinCodec = 2200
	}
}
