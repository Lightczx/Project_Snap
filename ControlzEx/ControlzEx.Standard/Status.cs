using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public enum Status
	{
		Ok,
		GenericError,
		InvalidParameter,
		OutOfMemory,
		ObjectBusy,
		InsufficientBuffer,
		NotImplemented,
		Win32Error,
		WrongState,
		Aborted,
		FileNotFound,
		ValueOverflow,
		AccessDenied,
		UnknownImageFormat,
		FontFamilyNotFound,
		FontStyleNotFound,
		NotTrueTypeFont,
		UnsupportedGdiplusVersion,
		GdiplusNotInitialized,
		PropertyNotFound,
		PropertyNotSupported,
		ProfileNotFound
	}
}
