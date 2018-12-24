using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("973510DB-7D7F-452B-8975-74A85828D354")]
	internal interface IFileDialogEvents
	{
		[PreserveSig]
		HRESULT OnFileOk(IFileDialog pfd);

		[PreserveSig]
		HRESULT OnFolderChanging(IFileDialog pfd, IShellItem psiFolder);

		[PreserveSig]
		HRESULT OnFolderChange(IFileDialog pfd);

		[PreserveSig]
		HRESULT OnSelectionChange(IFileDialog pfd);

		[PreserveSig]
		HRESULT OnShareViolation(IFileDialog pfd, IShellItem psi, out FDESVR pResponse);

		[PreserveSig]
		HRESULT OnTypeChange(IFileDialog pfd);

		[PreserveSig]
		HRESULT OnOverwrite(IFileDialog pfd, IShellItem psi, out FDEOR pResponse);
	}
}
