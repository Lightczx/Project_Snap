using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("36db0196-9665-46d1-9ba7-d3709eecf9ed")]
	internal interface IObjectWithAppUserModelId
	{
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetAppID();
	}
}
