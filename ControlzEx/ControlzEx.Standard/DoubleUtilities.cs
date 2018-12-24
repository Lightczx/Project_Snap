namespace ControlzEx.Standard
{
	internal static class DoubleUtilities
	{
		private const double Epsilon = 1.53E-06;

		public static bool AreClose(double value1, double value2)
		{
			if (value1 == value2)
			{
				return true;
			}
			double num = value1 - value2;
			if (num < 1.53E-06)
			{
				return num > -1.53E-06;
			}
			return false;
		}

		public static bool IsCloseTo(this double value1, double value2)
		{
			return AreClose(value1, value2);
		}

		public static bool IsStrictlyLessThan(this double value1, double value2)
		{
			if (value1 < value2)
			{
				return !AreClose(value1, value2);
			}
			return false;
		}

		public static bool IsStrictlyGreaterThan(this double value1, double value2)
		{
			if (value1 > value2)
			{
				return !AreClose(value1, value2);
			}
			return false;
		}

		public static bool IsLessThanOrCloseTo(this double value1, double value2)
		{
			if (!(value1 < value2))
			{
				return AreClose(value1, value2);
			}
			return true;
		}

		public static bool IsGreaterThanOrCloseTo(this double value1, double value2)
		{
			if (!(value1 > value2))
			{
				return AreClose(value1, value2);
			}
			return true;
		}

		public static bool IsFinite(this double value)
		{
			if (!double.IsNaN(value))
			{
				return !double.IsInfinity(value);
			}
			return false;
		}

		public static bool IsValidSize(this double value)
		{
			if (value.IsFinite())
			{
				return value.IsGreaterThanOrCloseTo(0.0);
			}
			return false;
		}

		public static bool IsFiniteAndNonNegative(this double d)
		{
			if (double.IsNaN(d) || double.IsInfinity(d) || d < 0.0)
			{
				return false;
			}
			return true;
		}
	}
}
