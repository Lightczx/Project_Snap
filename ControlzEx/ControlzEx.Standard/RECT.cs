using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public struct RECT
	{
		private int _left;

		private int _top;

		private int _right;

		private int _bottom;

		public static readonly RECT Empty;

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

		public int Width => _right - _left;

		public int Height => _bottom - _top;

		public POINT Position
		{
			get
			{
				POINT result = default(POINT);
				result.X = _left;
				result.Y = _top;
				return result;
			}
		}

		public SIZE Size
		{
			get
			{
				SIZE result = default(SIZE);
				result.cx = Width;
				result.cy = Height;
				return result;
			}
		}

		public bool IsEmpty
		{
			get
			{
				if (Left < Right)
				{
					return Top >= Bottom;
				}
				return true;
			}
		}

		public RECT(int left, int top, int right, int bottom)
		{
			_left = left;
			_top = top;
			_right = right;
			_bottom = bottom;
		}

		public RECT(RECT rcSrc)
		{
			_left = rcSrc.Left;
			_top = rcSrc.Top;
			_right = rcSrc.Right;
			_bottom = rcSrc.Bottom;
		}

		public void Offset(int dx, int dy)
		{
			_left += dx;
			_top += dy;
			_right += dx;
			_bottom += dy;
		}

		public static RECT Union(RECT rect1, RECT rect2)
		{
			RECT result = default(RECT);
			result.Left = Math.Min(rect1.Left, rect2.Left);
			result.Top = Math.Min(rect1.Top, rect2.Top);
			result.Right = Math.Max(rect1.Right, rect2.Right);
			result.Bottom = Math.Max(rect1.Bottom, rect2.Bottom);
			return result;
		}

		public override bool Equals(object obj)
		{
			try
			{
				RECT rECT = (RECT)obj;
				return rECT._bottom == _bottom && rECT._left == _left && rECT._right == _right && rECT._top == _top;
			}
			catch (InvalidCastException)
			{
				return false;
			}
		}

		public override string ToString()
		{
			if (this == Empty)
			{
				return "RECT {Empty}";
			}
			return "RECT { left : " + Left + " / top : " + Top + " / right : " + Right + " / bottom : " + Bottom + " }";
		}

		public override int GetHashCode()
		{
			return ((_left << 16) | Utility.LOWORD(_right)) ^ ((_top << 16) | Utility.LOWORD(_bottom));
		}

		public static bool operator ==(RECT rect1, RECT rect2)
		{
			if (rect1.Left == rect2.Left && rect1.Top == rect2.Top && rect1.Right == rect2.Right)
			{
				return rect1.Bottom == rect2.Bottom;
			}
			return false;
		}

		public static bool operator !=(RECT rect1, RECT rect2)
		{
			return !(rect1 == rect2);
		}
	}
}
