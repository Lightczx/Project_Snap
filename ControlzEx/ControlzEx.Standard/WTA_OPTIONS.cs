using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[StructLayout(LayoutKind.Explicit)]
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public struct WTA_OPTIONS
	{
		public const uint Size = 8u;

		[FieldOffset(0)]
		public WTNCA dwFlags;

		[FieldOffset(4)]
		public WTNCA dwMask;
	}
}
