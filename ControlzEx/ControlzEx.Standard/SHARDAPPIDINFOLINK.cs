using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal class SHARDAPPIDINFOLINK
	{
		private IntPtr psl;

		[MarshalAs(UnmanagedType.LPWStr)]
		private string pszAppID;
	}
}
