using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[StructLayout(LayoutKind.Sequential)]
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public class NOTIFYICONDATA
	{
		public int cbSize;

		public IntPtr hWnd;

		public int uID;

		public NIF uFlags;

		public int uCallbackMessage;

		public IntPtr hIcon;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] szTip = new char[128];

		public uint dwState;

		public uint dwStateMask;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public char[] szInfo = new char[256];

		public uint uVersion;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public char[] szInfoTitle = new char[64];

		public uint dwInfoFlags;

		public Guid guidItem;

		private IntPtr hBalloonIcon;
	}
}
