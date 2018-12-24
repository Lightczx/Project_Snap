using System;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_UnderlineBorder", Type = typeof(Border))]
	public class Underline : ContentControl
	{
		public const string UnderlineBorderPartName = "PART_UnderlineBorder";

		private Border _underlineBorder;

		public static readonly DependencyProperty PlacementProperty;

		public static readonly DependencyProperty LineThicknessProperty;

		public static readonly DependencyProperty LineExtentProperty;

		public Dock Placement
		{
			get
			{
				return (Dock)GetValue(PlacementProperty);
			}
			set
			{
				SetValue(PlacementProperty, value);
			}
		}

		public double LineThickness
		{
			get
			{
				return (double)GetValue(LineThicknessProperty);
			}
			set
			{
				SetValue(LineThicknessProperty, value);
			}
		}

		public double LineExtent
		{
			get
			{
				return (double)GetValue(LineExtentProperty);
			}
			set
			{
				SetValue(LineExtentProperty, value);
			}
		}

		static Underline()
		{
			PlacementProperty = DependencyProperty.Register("Placement", typeof(Dock), typeof(Underline), new PropertyMetadata(Dock.Left, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
			{
				(o as Underline)?.ApplyBorderProperties();
			}));
			LineThicknessProperty = DependencyProperty.Register("LineThickness", typeof(double), typeof(Underline), new PropertyMetadata(1.0, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
			{
				(o as Underline)?.ApplyBorderProperties();
			}));
			LineExtentProperty = DependencyProperty.Register("LineExtent", typeof(double), typeof(Underline), new PropertyMetadata(double.NaN, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
			{
				(o as Underline)?.ApplyBorderProperties();
			}));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Underline), new FrameworkPropertyMetadata(typeof(Underline)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_underlineBorder = (GetTemplateChild("PART_UnderlineBorder") as Border);
			ApplyBorderProperties();
		}

		private void ApplyBorderProperties()
		{
			if (_underlineBorder != null)
			{
				_underlineBorder.Height = double.NaN;
				_underlineBorder.Width = double.NaN;
				_underlineBorder.BorderThickness = default(Thickness);
				switch (Placement)
				{
				case Dock.Left:
					_underlineBorder.Width = LineExtent;
					_underlineBorder.BorderThickness = new Thickness(LineThickness, 0.0, 0.0, 0.0);
					break;
				case Dock.Top:
					_underlineBorder.Height = LineExtent;
					_underlineBorder.BorderThickness = new Thickness(0.0, LineThickness, 0.0, 0.0);
					break;
				case Dock.Right:
					_underlineBorder.Width = LineExtent;
					_underlineBorder.BorderThickness = new Thickness(0.0, 0.0, LineThickness, 0.0);
					break;
				case Dock.Bottom:
					_underlineBorder.Height = LineExtent;
					_underlineBorder.BorderThickness = new Thickness(0.0, 0.0, 0.0, LineThickness);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				InvalidateVisual();
			}
		}
	}
}
