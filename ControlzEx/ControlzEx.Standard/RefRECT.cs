using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[StructLayout(LayoutKind.Sequential)]
	internal class RefRECT
	{
		private int _left;

		private int _top;

		private int _right;

		private int _bottom;

		public int Width => _right - _left;

		public int Height => _bottom - _top;

		public int Left
		{
			get
			{
				return _left;
			}
			set
			{
				_left = value;
			}
		}

		public int Right
		{
			get
			{
				return _right;
			}
			set
			{
				_right = value;
			}
		}

		public int Top
		{
			get
			{
				return _top;
			}
			set
			{
				_top = value;
			}
		}

		public int Bottom
		{
			get
			{
				return _bottom;
			}
			set
			{
				_bottom = value;
			}
		}

		public RefRECT(int left, int top, int right, int bottom)
		{
			_left = left;
			_top = top;
			_right = right;
			_bottom = bottom;
		}

		public void Offset(int dx, int dy)
		{
			_left += dx;
			_top += dy;
			_right += dx;
			_bottom += dy;
		}
	}
}
