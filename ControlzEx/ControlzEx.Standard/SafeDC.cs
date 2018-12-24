using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public sealed class SafeDC : SafeHandleZeroOrMinusOneIsInvalid
	{
		private static class NativeMethods
		{
			[DllImport("user32.dll")]
			public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

			[DllImport("user32.dll")]
			public static extern SafeDC GetDC(IntPtr hwnd);

			[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
			public static extern SafeDC CreateDC([MarshalAs(UnmanagedType.LPWStr)] string lpszDriver, [MarshalAs(UnmanagedType.LPWStr)] string lpszDevice, IntPtr lpszOutput, IntPtr lpInitData);

			[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern SafeDC CreateCompatibleDC(IntPtr hdc);

			[DllImport("gdi32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool DeleteDC(IntPtr hdc);
		}

		private IntPtr? _hwnd;

		private bool _created;

		public IntPtr Hwnd
		{
			set
			{
				_hwnd = value;
			}
		}

		private SafeDC()
			: base(ownsHandle: true)
		{
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			if (_created)
			{
				return NativeMethods.DeleteDC(handle);
			}
			if (!_hwnd.HasValue || _hwnd.Value == IntPtr.Zero)
			{
				return true;
			}
			return NativeMethods.ReleaseDC(_hwnd.Value, handle) == 1;
		}

		public static SafeDC CreateDC(string deviceName)
		{
			SafeDC safeDC = null;
			try
			{
				safeDC = NativeMethods.CreateDC(deviceName, null, IntPtr.Zero, IntPtr.Zero);
			}
			finally
			{
				if (safeDC != null)
				{
					safeDC._created = true;
				}
			}
			if (safeDC.IsInvalid)
			{
				safeDC.Dispose();
				throw new SystemException("Unable to create a device context from the specified device information.");
			}
			return safeDC;
		}

		public static SafeDC CreateCompatibleDC(SafeDC hdc)
		{
			SafeDC safeDC = null;
			try
			{
				IntPtr hdc2 = IntPtr.Zero;
				if (hdc != null)
				{
					hdc2 = hdc.handle;
				}
				safeDC = NativeMethods.CreateCompatibleDC(hdc2);
				if (safeDC == null)
				{
					HRESULT.ThrowLastError();
				}
			}
			finally
			{
				if (safeDC != null)
				{
					safeDC._created = true;
				}
			}
			if (safeDC.IsInvalid)
			{
				safeDC.Dispose();
				throw new SystemException("Unable to create a device context from the specified device information.");
			}
			return safeDC;
		}

		public static SafeDC GetDC(IntPtr hwnd)
		{
			SafeDC safeDC = null;
			try
			{
				safeDC = NativeMethods.GetDC(hwnd);
			}
			finally
			{
				if (safeDC != null)
				{
					safeDC.Hwnd = hwnd;
				}
			}
			if (safeDC.IsInvalid)
			{
				HRESULT.E_FAIL.ThrowIfFailed();
			}
			return safeDC;
		}

		public static SafeDC GetDesktop()
		{
			return GetDC(IntPtr.Zero);
		}

		public static SafeDC WrapDC(IntPtr hdc)
		{
			return new SafeDC
			{
				handle = hdc,
				_created = false,
				_hwnd = IntPtr.Zero
			};
		}
	}
}
