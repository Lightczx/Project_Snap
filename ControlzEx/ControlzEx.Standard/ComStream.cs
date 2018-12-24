using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ControlzEx.Standard
{
	internal sealed class ComStream : Stream
	{
		private const int STATFLAG_NONAME = 1;

		private IStream _source;

		public override bool CanRead => true;

		public override bool CanSeek => true;

		public override bool CanWrite => false;

		public override long Length
		{
			get
			{
				_Validate();
				_source.Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, 1);
				return pstatstg.cbSize;
			}
		}

		public override long Position
		{
			get
			{
				return Seek(0L, SeekOrigin.Current);
			}
			set
			{
				Seek(value, SeekOrigin.Begin);
			}
		}

		private void _Validate()
		{
			if (_source == null)
			{
				throw new ObjectDisposedException("this");
			}
		}

		public ComStream(ref IStream stream)
		{
			Verify.IsNotNull(stream, "stream");
			_source = stream;
			stream = null;
		}

		public override void Close()
		{
			if (_source != null)
			{
				Utility.SafeRelease(ref _source);
			}
		}

		public override void Flush()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			_Validate();
			IntPtr hglobal = IntPtr.Zero;
			try
			{
				hglobal = Marshal.AllocHGlobal(4);
				byte[] array = new byte[count];
				_source.Read(array, count, hglobal);
				Array.Copy(array, 0, buffer, offset, Marshal.ReadInt32(hglobal));
				return Marshal.ReadInt32(hglobal);
			}
			finally
			{
				Utility.SafeFreeHGlobal(ref hglobal);
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			_Validate();
			IntPtr hglobal = IntPtr.Zero;
			try
			{
				hglobal = Marshal.AllocHGlobal(8);
				_source.Seek(offset, (int)origin, hglobal);
				return Marshal.ReadInt64(hglobal);
			}
			finally
			{
				Utility.SafeFreeHGlobal(ref hglobal);
			}
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}
}
