using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class Tile : Button
	{
		public static readonly DependencyProperty TitleProperty;

		public static readonly DependencyProperty HorizontalTitleAlignmentProperty;

		public static readonly DependencyProperty VerticalTitleAlignmentProperty;

		public static readonly DependencyProperty CountProperty;

		public static readonly DependencyProperty KeepDraggingProperty;

		public static readonly DependencyProperty TiltFactorProperty;

		public static readonly DependencyProperty TitleFontSizeProperty;

		public static readonly DependencyProperty CountFontSizeProperty;

		public string Title
		{
			get
			{
				return (string)GetValue(TitleProperty);
			}
			set
			{
				SetValue(TitleProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Layout")]
		public HorizontalAlignment HorizontalTitleAlignment
		{
			get
			{
				return (HorizontalAlignment)GetValue(HorizontalTitleAlignmentProperty);
			}
			set
			{
				SetValue(HorizontalTitleAlignmentProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Layout")]
		public VerticalAlignment VerticalTitleAlignment
		{
			get
			{
				return (VerticalAlignment)GetValue(VerticalTitleAlignmentProperty);
			}
			set
			{
				SetValue(VerticalTitleAlignmentProperty, value);
			}
		}

		public string Count
		{
			get
			{
				return (string)GetValue(CountProperty);
			}
			set
			{
				SetValue(CountProperty, value);
			}
		}

		public bool KeepDragging
		{
			get
			{
				return (bool)GetValue(KeepDraggingProperty);
			}
			set
			{
				SetValue(KeepDraggingProperty, value);
			}
		}

		public int TiltFactor
		{
			get
			{
				return (int)GetValue(TiltFactorProperty);
			}
			set
			{
				SetValue(TiltFactorProperty, value);
			}
		}

		public double TitleFontSize
		{
			get
			{
				return (double)GetValue(TitleFontSizeProperty);
			}
			set
			{
				SetValue(TitleFontSizeProperty, value);
			}
		}

		public double CountFontSize
		{
			get
			{
				return (double)GetValue(CountFontSizeProperty);
			}
			set
			{
				SetValue(CountFontSizeProperty, value);
			}
		}

		static Tile()
		{
			TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Tile), new PropertyMetadata((object)null));
			HorizontalTitleAlignmentProperty = DependencyProperty.Register("HorizontalTitleAlignment", typeof(HorizontalAlignment), typeof(Tile), new FrameworkPropertyMetadata(HorizontalAlignment.Left));
			VerticalTitleAlignmentProperty = DependencyProperty.Register("VerticalTitleAlignment", typeof(VerticalAlignment), typeof(Tile), new FrameworkPropertyMetadata(VerticalAlignment.Bottom));
			CountProperty = DependencyProperty.Register("Count", typeof(string), typeof(Tile), new PropertyMetadata((object)null));
			KeepDraggingProperty = DependencyProperty.Register("KeepDragging", typeof(bool), typeof(Tile), new PropertyMetadata(true));
			TiltFactorProperty = DependencyProperty.Register("TiltFactor", typeof(int), typeof(Tile), new PropertyMetadata(5));
			TitleFontSizeProperty = DependencyProperty.Register("TitleFontSize", typeof(double), typeof(Tile), new PropertyMetadata(16.0));
			CountFontSizeProperty = DependencyProperty.Register("CountFontSize", typeof(double), typeof(Tile), new PropertyMetadata(28.0));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Tile), new FrameworkPropertyMetadata(typeof(Tile)));
		}
	}
}
