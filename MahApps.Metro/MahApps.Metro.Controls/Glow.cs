using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public class Glow : Control
	{
		public static readonly DependencyProperty GlowBrushProperty;

		public static readonly DependencyProperty NonActiveGlowBrushProperty;

		public static readonly DependencyProperty IsGlowProperty;

		public static readonly DependencyProperty OrientationProperty;

		public static readonly DependencyProperty DirectionProperty;

		public Brush GlowBrush
		{
			get
			{
				return (Brush)GetValue(GlowBrushProperty);
			}
			set
			{
				SetValue(GlowBrushProperty, value);
			}
		}

		public Brush NonActiveGlowBrush
		{
			get
			{
				return (Brush)GetValue(NonActiveGlowBrushProperty);
			}
			set
			{
				SetValue(NonActiveGlowBrushProperty, value);
			}
		}

		public bool IsGlow
		{
			get
			{
				return (bool)GetValue(IsGlowProperty);
			}
			set
			{
				SetValue(IsGlowProperty, value);
			}
		}

		public Orientation Orientation
		{
			get
			{
				return (Orientation)GetValue(OrientationProperty);
			}
			set
			{
				SetValue(OrientationProperty, value);
			}
		}

		public GlowDirection Direction
		{
			get
			{
				return (GlowDirection)GetValue(DirectionProperty);
			}
			set
			{
				SetValue(DirectionProperty, value);
			}
		}

		static Glow()
		{
			GlowBrushProperty = DependencyProperty.Register("GlowBrush", typeof(Brush), typeof(Glow), new UIPropertyMetadata(Brushes.Transparent));
			NonActiveGlowBrushProperty = DependencyProperty.Register("NonActiveGlowBrush", typeof(Brush), typeof(Glow), new UIPropertyMetadata(Brushes.Transparent));
			IsGlowProperty = DependencyProperty.Register("IsGlow", typeof(bool), typeof(Glow), new UIPropertyMetadata(true));
			OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Glow), new UIPropertyMetadata(Orientation.Vertical));
			DirectionProperty = DependencyProperty.Register("Direction", typeof(GlowDirection), typeof(Glow), new UIPropertyMetadata(GlowDirection.Top));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Glow), new FrameworkPropertyMetadata(typeof(Glow)));
		}
	}
}
