using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;

namespace ControlzEx.Standard
{
	internal sealed class SafeGdiplusStartupToken : SafeHandleZeroOrMinusOneIsInvalid
	{
		private SafeGdiplusStartupToken(IntPtr ptr)
			: base(ownsHandle: true)
		{
			handle = ptr;
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return NativeMethods.GdiplusShutdown(handle) == Status.Ok;
		}

		public static SafeGdiplusStartupToken Startup()
		{
			if (NativeMethods.GdiplusStartup(out IntPtr token, new StartupInput(), out StartupOutput _) == Status.Ok)
			{
				return new SafeGdiplusStartupToken(token);
			}
			throw new Exception("Unable to initialize GDI+");
		}
	}
}
