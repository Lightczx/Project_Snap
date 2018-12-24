using ControlzEx.Standard;
using System;
using System.Windows;

namespace MahApps.Metro.Controls
{
	public static class WinApiHelper
	{
		public static Point GetRelativeMousePosition(IntPtr hWnd)
		{
			if (hWnd == IntPtr.Zero)
			{
				return default(Point);
			}
			POINT point = GetPhysicalCursorPos();
			NativeMethods.ScreenToClient(hWnd, ref point);
			return new Point((double)point.X, (double)point.Y);
		}

		public static bool TryGetRelativeMousePosition(IntPtr hWnd, out Point point)
		{
			POINT pt = default(POINT);
			bool num = hWnd != IntPtr.Zero && NativeMethods.TryGetPhysicalCursorPos(out pt);
			if (!num)
			{
				point = default(Point);
				return num;
			}
			NativeMethods.ScreenToClient(hWnd, ref pt);
			point = new Point((double)pt.X, (double)pt.Y);
			return num;
		}

		internal static POINT GetPhysicalCursorPos()
		{
			try
			{
				return NativeMethods.GetPhysicalCursorPos();
			}
			catch (Exception innerException)
			{
				throw new MahAppsException("Uups, this should not happen! Sorry for this exception! Is this maybe happend on a virtual machine or via remote desktop? Please let us know, thx.", innerException);
			}
		}
	}
}
