using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public class SHARDAPPIDINFOIDLIST
	{
		private IntPtr pidl;

		[MarshalAs(UnmanagedType.LPWStr)]
		private string pszAppID;
	}
}
