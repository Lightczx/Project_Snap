using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public static class Utils
	{
		[StructLayout(LayoutKind.Explicit)]
		private struct NanUnion
		{
			[FieldOffset(0)]
			internal double DoubleValue;

			[FieldOffset(0)]
			internal ulong UintValue;
		}

		internal const double DBL_EPSILON = 2.2204460492503131E-16;

		internal const float FLT_MIN = 1.17549435E-38f;

		public static bool IsCloseTo(this double value1, double value2)
		{
			if (value1 == value2)
			{
				return true;
			}
			double num = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 2.2204460492503131E-16;
			double num2 = value1 - value2;
			if (0.0 - num < num2)
			{
				return num > num2;
			}
			return false;
		}

		public static bool IsLessThan(double value1, double value2)
		{
			if (value1 < value2)
			{
				return !value1.IsCloseTo(value2);
			}
			return false;
		}

		public static bool IsGreaterThan(this double value1, double value2)
		{
			if (value1 > value2)
			{
				return !value1.IsCloseTo(value2);
			}
			return false;
		}

		public static bool IsOne(this double value)
		{
			return Math.Abs(value - 1.0) < 2.2204460492503131E-15;
		}

		public static bool IsZero(this double value)
		{
			return Math.Abs(value) < 2.2204460492503131E-15;
		}

		public static bool IsCloseTo(this Point point1, Point point2)
		{
			if (point1.X.IsCloseTo(point2.X))
			{
				return point1.Y.IsCloseTo(point2.Y);
			}
			return false;
		}

		public static bool IsCloseTo(this Size size1, Size size2)
		{
			if (size1.Width.IsCloseTo(size2.Width))
			{
				return size1.Height.IsCloseTo(size2.Height);
			}
			return false;
		}

		public static bool IsCloseTo(this Vector vector1, Vector vector2)
		{
			if (vector1.X.IsCloseTo(vector2.X))
			{
				return vector1.Y.IsCloseTo(vector2.Y);
			}
			return false;
		}

		public static bool IsCloseTo(this Rect rect1, Rect rect2)
		{
			if (rect1.IsEmpty)
			{
				return rect2.IsEmpty;
			}
			if (!rect2.IsEmpty && rect1.X.IsCloseTo(rect2.X) && rect1.Y.IsCloseTo(rect2.Y) && rect1.Height.IsCloseTo(rect2.Height))
			{
				return rect1.Width.IsCloseTo(rect2.Width);
			}
			return false;
		}

		public static bool IsNaN(double value)
		{
			NanUnion nanUnion = default(NanUnion);
			nanUnion.DoubleValue = value;
			NanUnion nanUnion2 = nanUnion;
			ulong num = (ulong)((long)nanUnion2.UintValue & -4503599627370496L);
			ulong num2 = nanUnion2.UintValue & 0xFFFFFFFFFFFFF;
			if (num == 9218868437227405312L || num == 18442240474082181120uL)
			{
				return num2 != 0;
			}
			return false;
		}

		public static double RoundLayoutValue(double value, double dpiScale)
		{
			double num;
			if (!dpiScale.IsCloseTo(1.0))
			{
				num = Math.Round(value * dpiScale) / dpiScale;
				if (IsNaN(num) || double.IsInfinity(num) || num.IsCloseTo(1.7976931348623157E+308))
				{
					num = value;
				}
			}
			else
			{
				num = Math.Round(value);
			}
			return num;
		}

		public static bool IsValid(this Thickness thick, bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
		{
			if (!allowNegative && (thick.Left < 0.0 || thick.Right < 0.0 || thick.Top < 0.0 || thick.Bottom < 0.0))
			{
				return false;
			}
			if (!allowNaN && (IsNaN(thick.Left) || IsNaN(thick.Right) || IsNaN(thick.Top) || IsNaN(thick.Bottom)))
			{
				return false;
			}
			if (!allowPositiveInfinity && (double.IsPositiveInfinity(thick.Left) || double.IsPositiveInfinity(thick.Right) || double.IsPositiveInfinity(thick.Top) || double.IsPositiveInfinity(thick.Bottom)))
			{
				return false;
			}
			if (!allowNegativeInfinity && (double.IsNegativeInfinity(thick.Left) || double.IsNegativeInfinity(thick.Right) || double.IsNegativeInfinity(thick.Top) || double.IsNegativeInfinity(thick.Bottom)))
			{
				return false;
			}
			return true;
		}

		public static Size CollapseThickness(this Thickness thick)
		{
			return new Size(thick.Left + thick.Right, thick.Top + thick.Bottom);
		}

		public static bool IsZero(this Thickness thick)
		{
			if (thick.Left.IsZero() && thick.Top.IsZero() && thick.Right.IsZero())
			{
				return thick.Bottom.IsZero();
			}
			return false;
		}

		public static bool IsUniform(this Thickness thick)
		{
			if (thick.Left.IsCloseTo(thick.Top) && thick.Left.IsCloseTo(thick.Right))
			{
				return thick.Left.IsCloseTo(thick.Bottom);
			}
			return false;
		}

		public static bool IsValid(this CornerRadius corner, bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
		{
			if (!allowNegative && (corner.TopLeft < 0.0 || corner.TopRight < 0.0 || corner.BottomLeft < 0.0 || corner.BottomRight < 0.0))
			{
				return false;
			}
			if (!allowNaN && (IsNaN(corner.TopLeft) || IsNaN(corner.TopRight) || IsNaN(corner.BottomLeft) || IsNaN(corner.BottomRight)))
			{
				return false;
			}
			if (!allowPositiveInfinity && (double.IsPositiveInfinity(corner.TopLeft) || double.IsPositiveInfinity(corner.TopRight) || double.IsPositiveInfinity(corner.BottomLeft) || double.IsPositiveInfinity(corner.BottomRight)))
			{
				return false;
			}
			if (!allowNegativeInfinity && (double.IsNegativeInfinity(corner.TopLeft) || double.IsNegativeInfinity(corner.TopRight) || double.IsNegativeInfinity(corner.BottomLeft) || double.IsNegativeInfinity(corner.BottomRight)))
			{
				return false;
			}
			return true;
		}

		public static bool IsZero(this CornerRadius corner)
		{
			if (corner.TopLeft.IsZero() && corner.TopRight.IsZero() && corner.BottomRight.IsZero())
			{
				return corner.BottomLeft.IsZero();
			}
			return false;
		}

		public static bool IsUniform(this CornerRadius corner)
		{
			double topLeft = corner.TopLeft;
			if (topLeft.IsCloseTo(corner.TopRight) && topLeft.IsCloseTo(corner.BottomLeft))
			{
				return topLeft.IsCloseTo(corner.BottomRight);
			}
			return false;
		}

		public static Rect Deflate(this Rect rect, Thickness thick)
		{
			return new Rect(rect.Left + thick.Left, rect.Top + thick.Top, Math.Max(0.0, rect.Width - thick.Left - thick.Right), Math.Max(0.0, rect.Height - thick.Top - thick.Bottom));
		}

		public static Rect Inflate(this Rect rect, Thickness thick)
		{
			return new Rect(rect.Left - thick.Left, rect.Top - thick.Top, Math.Max(0.0, rect.Width + thick.Left + thick.Right), Math.Max(0.0, rect.Height + thick.Top + thick.Bottom));
		}

		public static bool IsOpaqueSolidColorBrush(this Brush brush)
		{
			return (brush as SolidColorBrush)?.Color.A == 255;
		}

		public static bool IsEqualTo(this Brush brush, Brush otherBrush)
		{
			if (brush.GetType() != otherBrush.GetType())
			{
				return false;
			}
			if (brush == otherBrush)
			{
				return true;
			}
			SolidColorBrush solidColorBrush = brush as SolidColorBrush;
			SolidColorBrush solidColorBrush2 = otherBrush as SolidColorBrush;
			if (solidColorBrush != null && solidColorBrush2 != null)
			{
				if (solidColorBrush.Color == solidColorBrush2.Color)
				{
					return solidColorBrush.Opacity.IsCloseTo(solidColorBrush2.Opacity);
				}
				return false;
			}
			LinearGradientBrush linearGradientBrush = brush as LinearGradientBrush;
			LinearGradientBrush linearGradientBrush2 = otherBrush as LinearGradientBrush;
			if (linearGradientBrush != null && linearGradientBrush2 != null)
			{
				bool flag = linearGradientBrush.ColorInterpolationMode == linearGradientBrush2.ColorInterpolationMode && linearGradientBrush.EndPoint == linearGradientBrush2.EndPoint && linearGradientBrush.MappingMode == linearGradientBrush2.MappingMode && linearGradientBrush.Opacity.IsCloseTo(linearGradientBrush2.Opacity) && linearGradientBrush.StartPoint == linearGradientBrush2.StartPoint && linearGradientBrush.SpreadMethod == linearGradientBrush2.SpreadMethod && linearGradientBrush.GradientStops.Count == linearGradientBrush2.GradientStops.Count;
				if (!flag)
				{
					return false;
				}
				for (int i = 0; i < linearGradientBrush.GradientStops.Count; i++)
				{
					flag = (linearGradientBrush.GradientStops[i].Color == linearGradientBrush2.GradientStops[i].Color && linearGradientBrush.GradientStops[i].Offset.IsCloseTo(linearGradientBrush2.GradientStops[i].Offset));
					if (!flag)
					{
						break;
					}
				}
				return flag;
			}
			RadialGradientBrush radialGradientBrush = brush as RadialGradientBrush;
			RadialGradientBrush radialGradientBrush2 = otherBrush as RadialGradientBrush;
			if (radialGradientBrush != null && radialGradientBrush2 != null)
			{
				bool flag2 = radialGradientBrush.ColorInterpolationMode == radialGradientBrush2.ColorInterpolationMode && radialGradientBrush.GradientOrigin == radialGradientBrush2.GradientOrigin && radialGradientBrush.MappingMode == radialGradientBrush2.MappingMode && radialGradientBrush.Opacity.IsCloseTo(radialGradientBrush2.Opacity) && radialGradientBrush.RadiusX.IsCloseTo(radialGradientBrush2.RadiusX) && radialGradientBrush.RadiusY.IsCloseTo(radialGradientBrush2.RadiusY) && radialGradientBrush.SpreadMethod == radialGradientBrush2.SpreadMethod && radialGradientBrush.GradientStops.Count == radialGradientBrush2.GradientStops.Count;
				if (!flag2)
				{
					return false;
				}
				for (int j = 0; j < radialGradientBrush.GradientStops.Count; j++)
				{
					flag2 = (radialGradientBrush.GradientStops[j].Color == radialGradientBrush2.GradientStops[j].Color && radialGradientBrush.GradientStops[j].Offset.IsCloseTo(radialGradientBrush2.GradientStops[j].Offset));
					if (!flag2)
					{
						break;
					}
				}
				return flag2;
			}
			ImageBrush imageBrush = brush as ImageBrush;
			ImageBrush imageBrush2 = otherBrush as ImageBrush;
			if (imageBrush != null && imageBrush2 != null)
			{
				if (imageBrush.AlignmentX == imageBrush2.AlignmentX && imageBrush.AlignmentY == imageBrush2.AlignmentY && imageBrush.Opacity.IsCloseTo(imageBrush2.Opacity) && imageBrush.Stretch == imageBrush2.Stretch && imageBrush.TileMode == imageBrush2.TileMode && imageBrush.Viewbox == imageBrush2.Viewbox && imageBrush.ViewboxUnits == imageBrush2.ViewboxUnits && imageBrush.Viewport == imageBrush2.Viewport && imageBrush.ViewportUnits == imageBrush2.ViewportUnits)
				{
					return imageBrush.ImageSource == imageBrush2.ImageSource;
				}
				return false;
			}
			return false;
		}
	}
}
