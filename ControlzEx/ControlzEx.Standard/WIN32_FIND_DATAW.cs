using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ControlzEx.Standard
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	[BestFitMapping(false)]
	public class WIN32_FIND_DATAW
	{
		public FileAttributes dwFileAttributes;

		public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;

		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;

		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;

		public int nFileSizeHigh;

		public int nFileSizeLow;

		public int dwReserved0;

		public int dwReserved1;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string cFileName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
		public string cAlternateFileName;
	}
}
