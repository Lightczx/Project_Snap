using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ControlzEx.Standard
{
	internal sealed class ManagedIStream : IStream, IDisposable
	{
		private const int STGTY_STREAM = 2;

		private const int STGM_READWRITE = 2;

		private const int LOCK_EXCLUSIVE = 2;

		private Stream _source;

		public ManagedIStream(Stream source)
		{
			Verify.IsNotNull(source, "source");
			_source = source;
		}

		private void _Validate()
		{
			if (_source == null)
			{
				throw new ObjectDisposedException("this");
			}
		}

		[Obsolete("The method is not implemented", true)]
		public void Clone(out IStream ppstm)
		{
			ppstm = null;
			HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
		}

		public void Commit(int grfCommitFlags)
		{
			_Validate();
			_source.Flush();
		}

		public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
		{
			Verify.IsNotNull(pstm, "pstm");
			_Validate();
			byte[] array = new byte[4096];
			long num;
			int num2;
			for (num = 0L; num < cb; num += num2)
			{
				num2 = _source.Read(array, 0, array.Length);
				if (num2 == 0)
				{
					break;
				}
				pstm.Write(array, num2, IntPtr.Zero);
			}
			if (IntPtr.Zero != pcbRead)
			{
				Marshal.WriteInt64(pcbRead, num);
			}
			if (IntPtr.Zero != pcbWritten)
			{
				Marshal.WriteInt64(pcbWritten, num);
			}
		}

		[Obsolete("The method is not implemented", true)]
		public void LockRegion(long libOffset, long cb, int dwLockType)
		{
			HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
		}

		public void Read(byte[] pv, int cb, IntPtr pcbRead)
		{
			_Validate();
			int val = _source.Read(pv, 0, cb);
			if (IntPtr.Zero != pcbRead)
			{
				Marshal.WriteInt32(pcbRead, val);
			}
		}

		[Obsolete("The method is not implemented", true)]
		public void Revert()
		{
			HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
		}

		public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
		{
			_Validate();
			long val = _source.Seek(dlibMove, (SeekOrigin)dwOrigin);
			if (IntPtr.Zero != plibNewPosition)
			{
				Marshal.WriteInt64(plibNewPosition, val);
			}
		}

		public void SetSize(long libNewSize)
		{
			_Validate();
			_source.SetLength(libNewSize);
		}

		public void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
		{
			pstatstg = default(System.Runtime.InteropServices.ComTypes.STATSTG);
			_Validate();
			pstatstg.type = 2;
			pstatstg.cbSize = _source.Length;
			pstatstg.grfMode = 2;
			pstatstg.grfLocksSupported = 2;
		}

		[Obsolete("The method is not implemented", true)]
		public void UnlockRegion(long libOffset, long cb, int dwLockType)
		{
			HRESULT.STG_E_INVALIDFUNCTION.ThrowIfFailed("The method is not implemented.");
		}

		public void Write(byte[] pv, int cb, IntPtr pcbWritten)
		{
			_Validate();
			_source.Write(pv, 0, cb);
			if (IntPtr.Zero != pcbWritten)
			{
				Marshal.WriteInt32(pcbWritten, cb);
			}
		}

		public void Dispose()
		{
			_source = null;
		}
	}
}
