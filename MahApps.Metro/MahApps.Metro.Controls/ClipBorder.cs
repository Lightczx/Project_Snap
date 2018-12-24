using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public sealed class ClipBorder : Decorator
	{
		private struct BorderInfo
		{
			internal readonly double LeftTop;

			internal readonly double TopLeft;

			internal readonly double TopRight;

			internal readonly double RightTop;

			internal readonly double RightBottom;

			internal readonly double BottomRight;

			internal readonly double BottomLeft;

			internal readonly double LeftBottom;

			internal BorderInfo(CornerRadius corners, Thickness borders, Thickness padding, bool isOuterBorder)
			{
				double num = 0.5 * borders.Left + padding.Left;
				double num2 = 0.5 * borders.Top + padding.Top;
				double num3 = 0.5 * borders.Right + padding.Right;
				double num4 = 0.5 * borders.Bottom + padding.Bottom;
				if (isOuterBorder)
				{
					if (corners.TopLeft.IsZero())
					{
						LeftTop = (TopLeft = 0.0);
					}
					else
					{
						LeftTop = corners.TopLeft + num;
						TopLeft = corners.TopLeft + num2;
					}
					if (corners.TopRight.IsZero())
					{
						TopRight = (RightTop = 0.0);
					}
					else
					{
						TopRight = corners.TopRight + num2;
						RightTop = corners.TopRight + num3;
					}
					if (corners.BottomRight.IsZero())
					{
						RightBottom = (BottomRight = 0.0);
					}
					else
					{
						RightBottom = corners.BottomRight + num3;
						BottomRight = corners.BottomRight + num4;
					}
					if (corners.BottomLeft.IsZero())
					{
						BottomLeft = (LeftBottom = 0.0);
					}
					else
					{
						BottomLeft = corners.BottomLeft + num4;
						LeftBottom = corners.BottomLeft + num;
					}
				}
				else
				{
					LeftTop = Math.Max(0.0, corners.TopLeft - num);
					TopLeft = Math.Max(0.0, corners.TopLeft - num2);
					TopRight = Math.Max(0.0, corners.TopRight - num2);
					RightTop = Math.Max(0.0, corners.TopRight - num3);
					RightBottom = Math.Max(0.0, corners.BottomRight - num3);
					BottomRight = Math.Max(0.0, corners.BottomRight - num4);
					BottomLeft = Math.Max(0.0, corners.BottomLeft - num4);
					LeftBottom = Math.Max(0.0, corners.BottomLeft - num);
				}
			}
		}

		private StreamGeometry _backgroundGeometryCache;

		private StreamGeometry _borderGeometryCache;

		public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(ClipBorder), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), OnValidateThickness);

		public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(ClipBorder), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), OnValidateThickness);

		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ClipBorder), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), OnValidateCornerRadius);

		public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(ClipBorder), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

		public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(ClipBorder), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

		public static readonly DependencyProperty OptimizeClipRenderingProperty = DependencyProperty.Register("OptimizeClipRendering", typeof(bool), typeof(ClipBorder), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)GetValue(BorderThicknessProperty);
			}
			set
			{
				SetValue(BorderThicknessProperty, value);
			}
		}

		public Thickness Padding
		{
			get
			{
				return (Thickness)GetValue(PaddingProperty);
			}
			set
			{
				SetValue(PaddingProperty, value);
			}
		}

		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)GetValue(CornerRadiusProperty);
			}
			set
			{
				SetValue(CornerRadiusProperty, value);
			}
		}

		public Brush BorderBrush
		{
			get
			{
				return (Brush)GetValue(BorderBrushProperty);
			}
			set
			{
				SetValue(BorderBrushProperty, value);
			}
		}

		public Brush Background
		{
			get
			{
				return (Brush)GetValue(BackgroundProperty);
			}
			set
			{
				SetValue(BackgroundProperty, value);
			}
		}

		public bool OptimizeClipRendering
		{
			get
			{
				return (bool)GetValue(OptimizeClipRenderingProperty);
			}
			set
			{
				SetValue(OptimizeClipRenderingProperty, value);
			}
		}

		private static bool OnValidateThickness(object value)
		{
			return ((Thickness)value).IsValid(allowNegative: false, allowNaN: false, allowPositiveInfinity: false, allowNegativeInfinity: false);
		}

		private static bool OnValidateCornerRadius(object value)
		{
			return ((CornerRadius)value).IsValid(allowNegative: false, allowNaN: false, allowPositiveInfinity: false, allowNegativeInfinity: false);
		}

		protected override Size MeasureOverride(Size constraint)
		{
			UIElement child = Child;
			Size result = default(Size);
			Size size = BorderThickness.CollapseThickness();
			Size size2 = Padding.CollapseThickness();
			if (child == null)
			{
				result = new Size(size.Width + size2.Width, size.Height + size2.Height);
			}
			else
			{
				Size size3 = new Size(size.Width + size2.Width, size.Height + size2.Height);
				Size availableSize = new Size(Math.Max(0.0, constraint.Width - size3.Width), Math.Max(0.0, constraint.Height - size3.Height));
				child.Measure(availableSize);
				Size desiredSize = child.DesiredSize;
				result.Width = desiredSize.Width + size3.Width;
				result.Height = desiredSize.Height + size3.Height;
			}
			return result;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			Thickness borderThickness = BorderThickness;
			Rect rect = new Rect(finalSize);
			Rect rect2 = rect.Deflate(borderThickness);
			CornerRadius cornerRadius = CornerRadius;
			Thickness padding = Padding;
			Rect finalRect = rect2.Deflate(padding);
			if (!rect.Width.IsZero() && !rect.Height.IsZero())
			{
				BorderInfo borderInfo = new BorderInfo(cornerRadius, borderThickness, default(Thickness), isOuterBorder: true);
				StreamGeometry streamGeometry = new StreamGeometry();
				using (StreamGeometryContext ctx = streamGeometry.Open())
				{
					GenerateGeometry(ctx, rect, borderInfo);
				}
				streamGeometry.Freeze();
				_borderGeometryCache = streamGeometry;
			}
			else
			{
				_borderGeometryCache = null;
			}
			if (!rect2.Width.IsZero() && !rect2.Height.IsZero())
			{
				BorderInfo borderInfo2 = new BorderInfo(cornerRadius, borderThickness, default(Thickness), isOuterBorder: false);
				StreamGeometry streamGeometry2 = new StreamGeometry();
				using (StreamGeometryContext ctx2 = streamGeometry2.Open())
				{
					GenerateGeometry(ctx2, rect2, borderInfo2);
				}
				streamGeometry2.Freeze();
				_backgroundGeometryCache = streamGeometry2;
			}
			else
			{
				_backgroundGeometryCache = null;
			}
			UIElement child = Child;
			if (child != null)
			{
				child.Arrange(finalRect);
				StreamGeometry streamGeometry3 = new StreamGeometry();
				BorderInfo borderInfo3 = new BorderInfo(cornerRadius, borderThickness, padding, isOuterBorder: false);
				using (StreamGeometryContext ctx3 = streamGeometry3.Open())
				{
					GenerateGeometry(ctx3, new Rect(0.0, 0.0, finalRect.Width, finalRect.Height), borderInfo3);
				}
				streamGeometry3.Freeze();
				child.Clip = streamGeometry3;
			}
			return finalSize;
		}

		protected override void OnRender(DrawingContext dc)
		{
			Thickness borderThickness = BorderThickness;
			Brush borderBrush = BorderBrush;
			Brush background = Background;
			StreamGeometry borderGeometryCache = _borderGeometryCache;
			StreamGeometry backgroundGeometryCache = _backgroundGeometryCache;
			if (OptimizeClipRendering)
			{
				dc.DrawGeometry(borderBrush, null, borderGeometryCache);
			}
			else if (borderBrush != null && !borderThickness.IsZero() && background != null)
			{
				if (borderBrush.IsEqualTo(background))
				{
					dc.DrawGeometry(borderBrush, null, borderGeometryCache);
				}
				else if (borderBrush.IsOpaqueSolidColorBrush() && background.IsOpaqueSolidColorBrush())
				{
					dc.DrawGeometry(borderBrush, null, borderGeometryCache);
					dc.DrawGeometry(background, null, backgroundGeometryCache);
				}
				else if (borderBrush.IsOpaqueSolidColorBrush())
				{
					if (borderGeometryCache != null && backgroundGeometryCache != null)
					{
						PathGeometry outlinedPathGeometry = borderGeometryCache.GetOutlinedPathGeometry();
						PathGeometry outlinedPathGeometry2 = backgroundGeometryCache.GetOutlinedPathGeometry();
						PathGeometry geometry = Geometry.Combine(outlinedPathGeometry, outlinedPathGeometry2, GeometryCombineMode.Exclude, null);
						dc.DrawGeometry(background, null, borderGeometryCache);
						dc.DrawGeometry(borderBrush, null, geometry);
					}
				}
				else if (borderGeometryCache != null && backgroundGeometryCache != null)
				{
					PathGeometry outlinedPathGeometry3 = borderGeometryCache.GetOutlinedPathGeometry();
					PathGeometry outlinedPathGeometry4 = backgroundGeometryCache.GetOutlinedPathGeometry();
					PathGeometry geometry2 = Geometry.Combine(outlinedPathGeometry3, outlinedPathGeometry4, GeometryCombineMode.Exclude, null);
					dc.DrawGeometry(borderBrush, null, geometry2);
					dc.DrawGeometry(background, null, backgroundGeometryCache);
				}
			}
			else
			{
				if (borderBrush != null && !borderThickness.IsZero())
				{
					if (borderGeometryCache != null && backgroundGeometryCache != null)
					{
						PathGeometry outlinedPathGeometry5 = borderGeometryCache.GetOutlinedPathGeometry();
						PathGeometry outlinedPathGeometry6 = backgroundGeometryCache.GetOutlinedPathGeometry();
						PathGeometry geometry3 = Geometry.Combine(outlinedPathGeometry5, outlinedPathGeometry6, GeometryCombineMode.Exclude, null);
						dc.DrawGeometry(borderBrush, null, geometry3);
					}
					else
					{
						dc.DrawGeometry(borderBrush, null, borderGeometryCache);
					}
				}
				if (background != null)
				{
					dc.DrawGeometry(background, null, backgroundGeometryCache);
				}
			}
		}

		private static void GenerateGeometry(StreamGeometryContext ctx, Rect rect, BorderInfo borderInfo)
		{
			Point point = new Point(borderInfo.LeftTop, 0.0);
			Point point2 = new Point(rect.Width - borderInfo.RightTop, 0.0);
			Point point3 = new Point(rect.Width, borderInfo.TopRight);
			Point point4 = new Point(rect.Width, rect.Height - borderInfo.BottomRight);
			Point point5 = new Point(rect.Width - borderInfo.RightBottom, rect.Height);
			Point point6 = new Point(borderInfo.LeftBottom, rect.Height);
			Point point7 = new Point(0.0, rect.Height - borderInfo.BottomLeft);
			Point point8 = new Point(0.0, borderInfo.TopLeft);
			if (point.X > point2.X)
			{
				double num3 = point2.X = (point.X = borderInfo.LeftTop / (borderInfo.LeftTop + borderInfo.RightTop) * rect.Width);
			}
			if (point3.Y > point4.Y)
			{
				double num6 = point4.Y = (point3.Y = borderInfo.TopRight / (borderInfo.TopRight + borderInfo.BottomRight) * rect.Height);
			}
			if (point6.X > point5.X)
			{
				double num9 = point6.X = (point5.X = borderInfo.LeftBottom / (borderInfo.LeftBottom + borderInfo.RightBottom) * rect.Width);
			}
			if (point8.Y > point7.Y)
			{
				double num12 = point8.Y = (point7.Y = borderInfo.TopLeft / (borderInfo.TopLeft + borderInfo.BottomLeft) * rect.Height);
			}
			double x = rect.TopLeft.X;
			double y = rect.TopLeft.Y;
			Vector vector = new Vector(x, y);
			point += vector;
			point2 += vector;
			point3 += vector;
			point4 += vector;
			point5 += vector;
			point6 += vector;
			point7 += vector;
			point8 += vector;
			ctx.BeginFigure(point, isFilled: true, isClosed: true);
			ctx.LineTo(point2, isStroked: true, isSmoothJoin: false);
			double num13 = rect.TopRight.X - point2.X;
			double num14 = point3.Y - rect.TopRight.Y;
			if (!num13.IsZero() || !num14.IsZero())
			{
				ctx.ArcTo(point3, new Size(num13, num14), 0.0, isLargeArc: false, SweepDirection.Clockwise, isStroked: true, isSmoothJoin: false);
			}
			ctx.LineTo(point4, isStroked: true, isSmoothJoin: false);
			num13 = rect.BottomRight.X - point5.X;
			num14 = rect.BottomRight.Y - point4.Y;
			if (!num13.IsZero() || !num14.IsZero())
			{
				ctx.ArcTo(point5, new Size(num13, num14), 0.0, isLargeArc: false, SweepDirection.Clockwise, isStroked: true, isSmoothJoin: false);
			}
			ctx.LineTo(point6, isStroked: true, isSmoothJoin: false);
			num13 = point6.X - rect.BottomLeft.X;
			num14 = rect.BottomLeft.Y - point7.Y;
			if (!num13.IsZero() || !num14.IsZero())
			{
				ctx.ArcTo(point7, new Size(num13, num14), 0.0, isLargeArc: false, SweepDirection.Clockwise, isStroked: true, isSmoothJoin: false);
			}
			ctx.LineTo(point8, isStroked: true, isSmoothJoin: false);
			num13 = point.X - rect.TopLeft.X;
			num14 = point8.Y - rect.TopLeft.Y;
			if (!num13.IsZero() || !num14.IsZero())
			{
				ctx.ArcTo(point, new Size(num13, num14), 0.0, isLargeArc: false, SweepDirection.Clockwise, isStroked: true, isSmoothJoin: false);
			}
		}
	}
}
