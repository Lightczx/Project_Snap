using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4e530b0a-e611-4c77-a3ac-9031d022281b")]
	internal interface IApplicationAssociationRegistration
	{
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string QueryCurrentDefault([MarshalAs(UnmanagedType.LPWStr)] string pszQuery, AT atQueryType, AL alQueryLevel);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryAppIsDefault([MarshalAs(UnmanagedType.LPWStr)] string pszQuery, AT atQueryType, AL alQueryLevel, [MarshalAs(UnmanagedType.LPWStr)] string pszAppRegistryName);

		[return: MarshalAs(UnmanagedType.Bool)]
		bool QueryAppIsDefaultAll(AL alQueryLevel, [MarshalAs(UnmanagedType.LPWStr)] string pszAppRegistryName);

		void SetAppAsDefault([MarshalAs(UnmanagedType.LPWStr)] string pszAppRegistryName, [MarshalAs(UnmanagedType.LPWStr)] string pszSet, AT atSetType);

		void SetAppAsDefaultAll([MarshalAs(UnmanagedType.LPWStr)] string pszAppRegistryName);

		void ClearUserAssociations();
	}
}
