using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct COMDLG_FILTERSPEC
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszName;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszSpec;
	}
}
