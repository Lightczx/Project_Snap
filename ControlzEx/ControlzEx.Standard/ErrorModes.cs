using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	[Flags]
	public enum ErrorModes
	{
		Default = 0x0,
		FailCriticalErrors = 0x1,
		NoGpFaultErrorBox = 0x2,
		NoAlignmentFaultExcept = 0x4,
		NoOpenFileErrorBox = 0x8000
	}
}
