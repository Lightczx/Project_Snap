using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[StructLayout(LayoutKind.Explicit)]
	internal class PROPVARIANT : IDisposable
	{
		private static class NativeMethods
		{
			[DllImport("ole32.dll")]
			internal static extern HRESULT PropVariantClear(PROPVARIANT pvar);
		}

		[FieldOffset(0)]
		private ushort vt;

		[FieldOffset(8)]
		private IntPtr pointerVal;

		[FieldOffset(8)]
		private byte byteVal;

		[FieldOffset(8)]
		private long longVal;

		[FieldOffset(8)]
		private short boolVal;

		public VarEnum VarType => (VarEnum)vt;

		public string GetValue()
		{
			if (vt == 31)
			{
				return Marshal.PtrToStringUni(pointerVal);
			}
			return null;
		}

		public void SetValue(bool f)
		{
			Clear();
			vt = 11;
			boolVal = (short)(f ? (-1) : 0);
		}

		public void SetValue(string val)
		{
			Clear();
			vt = 31;
			pointerVal = Marshal.StringToCoTaskMemUni(val);
		}

		public void Clear()
		{
			NativeMethods.PropVariantClear(this);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~PROPVARIANT()
		{
			Dispose(disposing: false);
		}

		private void Dispose(bool disposing)
		{
			Clear();
		}
	}
}
