using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("56FDF342-FD6D-11d0-958A-006097C9A090")]
	internal interface ITaskbarList
	{
		void HrInit();

		void AddTab(IntPtr hwnd);

		void DeleteTab(IntPtr hwnd);

		void ActivateTab(IntPtr hwnd);

		void SetActiveAlt(IntPtr hwnd);
	}
}
