using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public class WINDOWPLACEMENT
	{
		public int length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));

		public int flags;

		public SW showCmd;

		public POINT minPosition;

		public POINT maxPosition;

		public RECT normalPosition;
	}
}
