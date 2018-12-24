using System;

namespace ControlzEx.Standard
{
	[Serializable]
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public struct POINT
	{
		private int _x;

		private int _y;

		public int X
		{
			get
			{
				return _x;
			}
			set
			{
				_x = value;
			}
		}

		public int Y
		{
			get
			{
				return _y;
			}
			set
			{
				_y = value;
			}
		}

		public POINT(int x, int y)
		{
			_x = x;
			_y = y;
		}

		public override bool Equals(object obj)
		{
			if (obj is POINT)
			{
				POINT pOINT = (POINT)obj;
				if (pOINT._x == _x)
				{
					return pOINT._y == _y;
				}
				return false;
			}
			return ((ValueType)this).Equals(obj);
		}

		public override int GetHashCode()
		{
			return _x.GetHashCode() ^ _y.GetHashCode();
		}

		public static bool operator ==(POINT a, POINT b)
		{
			if (a._x == b._x)
			{
				return a._y == b._y;
			}
			return false;
		}

		public static bool operator !=(POINT a, POINT b)
		{
			return !(a == b);
		}
	}
}
