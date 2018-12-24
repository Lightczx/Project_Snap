using System;
using System.Diagnostics;
using System.Threading;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public static class Assert
	{
		public delegate void EvaluateFunction();

		public delegate bool ImplicationFunction();

		private static void _Break()
		{
			Debugger.Break();
		}

		[Conditional("DEBUG")]
		public static void Evaluate(EvaluateFunction argument)
		{
			argument();
		}

		[Obsolete("Use Assert.AreEqual instead of Assert.Equals", false)]
		[Conditional("DEBUG")]
		public static void Equals<T>(T expected, T actual)
		{
		}

		[Conditional("DEBUG")]
		public static void AreEqual<T>(T expected, T actual)
		{
			if (expected == null)
			{
				if (actual != null && !actual.Equals(expected))
				{
					_Break();
				}
			}
			else if (!expected.Equals(actual))
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void LazyAreEqual<T>(Func<T> expectedResult, Func<T> actualResult)
		{
			T val = actualResult();
			T val2 = expectedResult();
			if (val2 == null)
			{
				if (val != null && !val.Equals(val2))
				{
					_Break();
				}
			}
			else if (!val2.Equals(val))
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void AreNotEqual<T>(T notExpected, T actual)
		{
			if (notExpected == null)
			{
				if (actual == null || actual.Equals(notExpected))
				{
					_Break();
				}
			}
			else if (notExpected.Equals(actual))
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void Implies(bool condition, bool result)
		{
			if (condition && !result)
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void Implies(bool condition, ImplicationFunction result)
		{
			if (condition && !result())
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsNeitherNullNorEmpty(string value)
		{
		}

		[Conditional("DEBUG")]
		public static void IsNeitherNullNorWhitespace(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				_Break();
			}
			if (value.Trim().Length == 0)
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsNotNull<T>(T value) where T : class
		{
			if (value == null)
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsDefault<T>(T value) where T : struct
		{
			value.Equals(default(T));
		}

		[Conditional("DEBUG")]
		public static void IsNotDefault<T>(T value) where T : struct
		{
			value.Equals(default(T));
		}

		[Conditional("DEBUG")]
		public static void IsFalse(bool condition)
		{
			if (condition)
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsFalse(bool condition, string message)
		{
			if (condition)
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsTrue(bool condition)
		{
			if (!condition)
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsTrue<T>(Predicate<T> predicate, T arg)
		{
			if (!predicate(arg))
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsTrue(bool condition, string message)
		{
			if (!condition)
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void Fail()
		{
			_Break();
		}

		[Conditional("DEBUG")]
		public static void Fail(string message)
		{
			_Break();
		}

		[Conditional("DEBUG")]
		public static void IsNull<T>(T item) where T : class
		{
			if (item != null)
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void BoundedDoubleInc(double lowerBoundInclusive, double value, double upperBoundInclusive)
		{
			if (value < lowerBoundInclusive || value > upperBoundInclusive)
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void BoundedInteger(int lowerBoundInclusive, int value, int upperBoundExclusive)
		{
			if (value < lowerBoundInclusive || value >= upperBoundExclusive)
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void IsApartmentState(ApartmentState expectedState)
		{
			if (Thread.CurrentThread.GetApartmentState() != expectedState)
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void NullableIsNotNull<T>(T? value) where T : struct
		{
			if (!value.HasValue)
			{
				_Break();
			}
		}

		[Conditional("DEBUG")]
		public static void NullableIsNull<T>(T? value) where T : struct
		{
			if (value.HasValue)
			{
				_Break();
			}
		}
	}
}
