using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
	internal interface IFileDialog : IModalWindow
	{
		[PreserveSig]
		new HRESULT Show(IntPtr parent);

		void SetFileTypes(uint cFileTypes, [In] ref COMDLG_FILTERSPEC rgFilterSpec);

		void SetFileTypeIndex(uint iFileType);

		uint GetFileTypeIndex();

		uint Advise(IFileDialogEvents pfde);

		void Unadvise(uint dwCookie);

		void SetOptions(FOS fos);

		FOS GetOptions();

		void SetDefaultFolder(IShellItem psi);

		void SetFolder(IShellItem psi);

		IShellItem GetFolder();

		IShellItem GetCurrentSelection();

		void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetFileName();

		void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

		void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);

		void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

		IShellItem GetResult();

		void AddPlace(IShellItem psi, FDAP alignment);

		void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

		void Close([MarshalAs(UnmanagedType.Error)] int hr);

		void SetClientGuid([In] ref Guid guid);

		void ClearClientData();

		void SetFilter([MarshalAs(UnmanagedType.Interface)] object pFilter);
	}
}
