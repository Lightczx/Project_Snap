using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
	internal interface IFileOpenDialog : IFileDialog, IModalWindow
	{
		[PreserveSig]
		new HRESULT Show(IntPtr parent);

		new void SetFileTypes(uint cFileTypes, [In] ref COMDLG_FILTERSPEC rgFilterSpec);

		new void SetFileTypeIndex(uint iFileType);

		new uint GetFileTypeIndex();

		new uint Advise(IFileDialogEvents pfde);

		new void Unadvise(uint dwCookie);

		new void SetOptions(FOS fos);

		new FOS GetOptions();

		new void SetDefaultFolder(IShellItem psi);

		new void SetFolder(IShellItem psi);

		new IShellItem GetFolder();

		new IShellItem GetCurrentSelection();

		new void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		[return: MarshalAs(UnmanagedType.LPWStr)]
		new string GetFileName();

		new void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

		new void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);

		new void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

		new IShellItem GetResult();

		new void AddPlace(IShellItem psi, FDAP fdcp);

		new void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

		new void Close([MarshalAs(UnmanagedType.Error)] int hr);

		new void SetClientGuid([In] ref Guid guid);

		new void ClearClientData();

		new void SetFilter([MarshalAs(UnmanagedType.Interface)] object pFilter);

		IShellItemArray GetResults();

		IShellItemArray GetSelectedItems();
	}
}
