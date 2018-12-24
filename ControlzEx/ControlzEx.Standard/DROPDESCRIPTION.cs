using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Size = 1044)]
	internal struct DROPDESCRIPTION
	{
		public DROPIMAGETYPE type;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szMessage;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szInsert;
	}
}
