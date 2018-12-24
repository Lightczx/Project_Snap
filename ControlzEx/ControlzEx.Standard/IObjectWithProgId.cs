using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("71e806fb-8dee-46fc-bf8c-7748a8a1ae13")]
	internal interface IObjectWithProgId
	{
		void SetProgID([MarshalAs(UnmanagedType.LPWStr)] string pszProgID);

		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetProgID();
	}
}
