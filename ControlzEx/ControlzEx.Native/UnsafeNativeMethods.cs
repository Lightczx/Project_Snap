using ControlzEx.Standard;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace ControlzEx.Native
{
	[SuppressUnmanagedCodeSecurity]
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public static class UnsafeNativeMethods
	{
		[DllImport("user32")]
		internal static extern IntPtr DefWindowProc([In] IntPtr hwnd, [In] int msg, [In] IntPtr wParam, [In] IntPtr lParam);

		[DllImport("user32", CharSet = CharSet.Unicode, EntryPoint = "GetMonitorInfoW", ExactSpelling = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetMonitorInfo([In] IntPtr hMonitor, [Out] MONITORINFO lpmi);

		[DllImport("user32")]
		internal static extern IntPtr MonitorFromWindow([In] IntPtr handle, [In] MonitorOptions flags);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

		[DllImport("user32", CharSet = CharSet.Unicode, EntryPoint = "LoadStringW", ExactSpelling = true, SetLastError = true)]
		public static extern int LoadString([Optional] [In] SafeLibraryHandle hInstance, [In] uint uID, [Out] StringBuilder lpBuffer, [In] int nBufferMax);

		[DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = true)]
		internal static extern bool IsWindow([Optional] [In] IntPtr hWnd);

		[DllImport("user32")]
		internal static extern IntPtr GetSystemMenu([In] IntPtr hWnd, [In] bool bRevert);

		[DllImport("user32")]
		internal static extern uint TrackPopupMenuEx([In] IntPtr hmenu, [In] uint fuFlags, [In] int x, [In] int y, [In] IntPtr hwnd, [Optional] [In] IntPtr lptpm);

		[DllImport("user32", EntryPoint = "PostMessage", SetLastError = true)]
		private static extern bool _PostMessage([Optional] [In] IntPtr hWnd, [In] uint Msg, [In] IntPtr wParam, [In] IntPtr lParam);

		[DllImport("kernel32", CharSet = CharSet.Unicode, EntryPoint = "LoadLibraryW", ExactSpelling = true, SetLastError = true)]
		public static extern SafeLibraryHandle LoadLibrary([In] [MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

		[DllImport("kernel32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary([In] IntPtr hModule);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		internal static void PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam)
		{
			if (!_PostMessage(hWnd, Msg, wParam, lParam))
			{
				throw new Win32Exception();
			}
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate, IntPtr hrgnUpdate, Constants.RedrawWindowFlags flags);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, Constants.RedrawWindowFlags flags);

		[DllImport("user32.dll")]
		internal static extern int MapVirtualKey(uint uCode, uint uMapType);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetKeyNameText(int lParam, [Out] [MarshalAs(UnmanagedType.LPWStr)] StringBuilder str, int size);
	}
}
