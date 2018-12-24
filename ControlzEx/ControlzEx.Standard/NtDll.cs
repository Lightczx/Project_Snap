using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	public static class NtDll
	{
		public struct OSVERSIONINFOEX
		{
			public uint dwOSVersionInfoSize;

			public uint dwMajorVersion;

			public uint dwMinorVersion;

			public uint dwBuildNumber;

			public uint dwPlatformId;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string szCSDVersion;

			public ushort wServicePackMajor;

			public ushort wServicePackMinor;

			public ushort wSuiteMask;

			public byte wProductType;

			public byte wReserved;
		}

		public static class NativeMethods
		{
			[DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
			public static extern int RtlGetVersion([In] [Out] ref OSVERSIONINFOEX version);
		}

		public static Version RtlGetVersion()
		{
			OSVERSIONINFOEX version = default(OSVERSIONINFOEX);
			version.dwOSVersionInfoSize = (uint)Marshal.SizeOf(typeof(OSVERSIONINFOEX));
			if (NativeMethods.RtlGetVersion(ref version) == 0)
			{
				return new Version((int)version.dwMajorVersion, (int)version.dwMinorVersion, (int)version.dwBuildNumber, 0);
			}
			return null;
		}
	}
}
