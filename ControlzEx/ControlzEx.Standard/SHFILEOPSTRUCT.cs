using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public struct SHFILEOPSTRUCT
	{
		public IntPtr hwnd;

		[MarshalAs(UnmanagedType.U4)]
		public FO wFunc;

		public string pFrom;

		public string pTo;

		[MarshalAs(UnmanagedType.U2)]
		public FOF fFlags;

		[MarshalAs(UnmanagedType.Bool)]
		public int fAnyOperationsAborted;

		public IntPtr hNameMappings;

		public string lpszProgressTitle;
	}
}
