using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
	internal interface ITaskbarList4 : ITaskbarList3, ITaskbarList2, ITaskbarList
	{
		new void HrInit();

		new void AddTab(IntPtr hwnd);

		new void DeleteTab(IntPtr hwnd);

		new void ActivateTab(IntPtr hwnd);

		new void SetActiveAlt(IntPtr hwnd);

		new void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

		[PreserveSig]
		new HRESULT SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

		[PreserveSig]
		new HRESULT SetProgressState(IntPtr hwnd, TBPF tbpFlags);

		[PreserveSig]
		new HRESULT RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);

		[PreserveSig]
		new HRESULT UnregisterTab(IntPtr hwndTab);

		[PreserveSig]
		new HRESULT SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);

		[PreserveSig]
		new HRESULT SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, uint dwReserved);

		[PreserveSig]
		new HRESULT ThumbBarAddButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		[PreserveSig]
		new HRESULT ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButtons);

		[PreserveSig]
		new HRESULT ThumbBarSetImageList(IntPtr hwnd, [MarshalAs(UnmanagedType.IUnknown)] object himl);

		[PreserveSig]
		new HRESULT SetOverlayIcon(IntPtr hwnd, IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);

		[PreserveSig]
		new HRESULT SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);

		[PreserveSig]
		new HRESULT SetThumbnailClip(IntPtr hwnd, RefRECT prcClip);

		void SetTabProperties(IntPtr hwndTab, STPF stpFlags);
	}
}
