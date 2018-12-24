using System;

namespace ControlzEx.Standard
{
	internal static class MonitorHelper
	{
		public static MONITORINFO GetMonitorInfoFromPoint()
		{
			IntPtr intPtr = NativeMethods.MonitorFromPoint(NativeMethods.GetCursorPos(), MonitorOptions.MONITOR_DEFAULTTONEAREST);
			if (intPtr != IntPtr.Zero)
			{
				return NativeMethods.GetMonitorInfo(intPtr);
			}
			return new MONITORINFO();
		}
	}
}
