using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public class SliderHelper
	{
		public static readonly DependencyProperty ThumbFillBrushProperty = DependencyProperty.RegisterAttached("ThumbFillBrush", typeof(Brush), typeof(SliderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty ThumbFillHoverBrushProperty = DependencyProperty.RegisterAttached("ThumbFillHoverBrush", typeof(Brush), typeof(SliderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty ThumbFillPressedBrushProperty = DependencyProperty.RegisterAttached("ThumbFillPressedBrush", typeof(Brush), typeof(SliderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty ThumbFillDisabledBrushProperty = DependencyProperty.RegisterAttached("ThumbFillDisabledBrush", typeof(Brush), typeof(SliderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty TrackFillBrushProperty = DependencyProperty.RegisterAttached("TrackFillBrush", typeof(Brush), typeof(SliderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty TrackFillHoverBrushProperty = DependencyProperty.RegisterAttached("TrackFillHoverBrush", typeof(Brush), typeof(SliderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty TrackFillPressedBrushProperty = DependencyProperty.RegisterAttached("TrackFillPressedBrush", typeof(Brush), typeof(SliderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty TrackFillDisabledBrushProperty = DependencyProperty.RegisterAttached("TrackFillDisabledBrush", typeof(Brush), typeof(SliderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty TrackValueFillBrushProperty = DependencyProperty.RegisterAttached("TrackValueFillBrush", typeof(Brush), typeof(SliderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty TrackValueFillHoverBrushProperty = DependencyProperty.RegisterAttached("TrackValueFillHoverBrush", typeof(Brush), typeof(SliderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty TrackValueFillPressedBrushProperty = DependencyProperty.RegisterAttached("TrackValueFillPressedBrush", typeof(Brush), typeof(SliderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty TrackValueFillDisabledBrushProperty = DependencyProperty.RegisterAttached("TrackValueFillDisabledBrush", typeof(Brush), typeof(SliderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty ChangeValueByProperty = DependencyProperty.RegisterAttached("ChangeValueBy", typeof(MouseWheelChange), typeof(SliderHelper), new PropertyMetadata(MouseWheelChange.SmallChange));

		public static readonly DependencyProperty EnableMouseWheelProperty = DependencyProperty.RegisterAttached("EnableMouseWheel", typeof(MouseWheelState), typeof(SliderHelper), new PropertyMetadata(MouseWheelState.None, OnEnableMouseWheelChanged));

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static Brush GetThumbFillBrush(UIElement element)
		{
			return (Brush)element.GetValue(ThumbFillBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetThumbFillBrush(UIElement element, Brush value)
		{
			element.SetValue(ThumbFillBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static Brush GetThumbFillHoverBrush(UIElement element)
		{
			return (Brush)element.GetValue(ThumbFillHoverBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetThumbFillHoverBrush(UIElement element, Brush value)
		{
			element.SetValue(ThumbFillHoverBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static Brush GetThumbFillPressedBrush(UIElement element)
		{
			return (Brush)element.GetValue(ThumbFillPressedBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetThumbFillPressedBrush(UIElement element, Brush value)
		{
			element.SetValue(ThumbFillPressedBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static Brush GetThumbFillDisabledBrush(UIElement element)
		{
			return (Brush)element.GetValue(ThumbFillDisabledBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetThumbFillDisabledBrush(UIElement element, Brush value)
		{
			element.SetValue(ThumbFillDisabledBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static Brush GetTrackFillBrush(UIElement element)
		{
			return (Brush)element.GetValue(TrackFillBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetTrackFillBrush(UIElement element, Brush value)
		{
			element.SetValue(TrackFillBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static Brush GetTrackFillHoverBrush(UIElement element)
		{
			return (Brush)element.GetValue(TrackFillHoverBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetTrackFillHoverBrush(UIElement element, Brush value)
		{
			element.SetValue(TrackFillHoverBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static Brush GetTrackFillPressedBrush(UIElement element)
		{
			return (Brush)element.GetValue(TrackFillPressedBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetTrackFillPressedBrush(UIElement element, Brush value)
		{
			element.SetValue(TrackFillPressedBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static Brush GetTrackFillDisabledBrush(UIElement element)
		{
			return (Brush)element.GetValue(TrackFillDisabledBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetTrackFillDisabledBrush(UIElement element, Brush value)
		{
			element.SetValue(TrackFillDisabledBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static Brush GetTrackValueFillBrush(UIElement element)
		{
			return (Brush)element.GetValue(TrackValueFillBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetTrackValueFillBrush(UIElement element, Brush value)
		{
			element.SetValue(TrackValueFillBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static Brush GetTrackValueFillHoverBrush(UIElement element)
		{
			return (Brush)element.GetValue(TrackValueFillHoverBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetTrackValueFillHoverBrush(UIElement element, Brush value)
		{
			element.SetValue(TrackValueFillHoverBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static Brush GetTrackValueFillPressedBrush(UIElement element)
		{
			return (Brush)element.GetValue(TrackValueFillPressedBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetTrackValueFillPressedBrush(UIElement element, Brush value)
		{
			element.SetValue(TrackValueFillPressedBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static Brush GetTrackValueFillDisabledBrush(UIElement element)
		{
			return (Brush)element.GetValue(TrackValueFillDisabledBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetTrackValueFillDisabledBrush(UIElement element, Brush value)
		{
			element.SetValue(TrackValueFillDisabledBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static MouseWheelChange GetChangeValueBy(UIElement element)
		{
			return (MouseWheelChange)element.GetValue(ChangeValueByProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetChangeValueBy(UIElement element, MouseWheelChange value)
		{
			element.SetValue(ChangeValueByProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static MouseWheelState GetEnableMouseWheel(UIElement element)
		{
			return (MouseWheelState)element.GetValue(EnableMouseWheelProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Slider))]
		[AttachedPropertyBrowsableForType(typeof(RangeSlider))]
		public static void SetEnableMouseWheel(UIElement element, MouseWheelState value)
		{
			element.SetValue(EnableMouseWheelProperty, value);
		}

		private static void OnEnableMouseWheelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
			{
				if (d is Slider)
				{
					Slider slider = (Slider)d;
					slider.PreviewMouseWheel -= OnSliderPreviewMouseWheel;
					if ((MouseWheelState)e.NewValue != 0)
					{
						slider.PreviewMouseWheel += OnSliderPreviewMouseWheel;
					}
				}
				else if (d is RangeSlider)
				{
					RangeSlider rangeSlider = (RangeSlider)d;
					rangeSlider.PreviewMouseWheel -= OnRangeSliderPreviewMouseWheel;
					if ((MouseWheelState)e.NewValue != 0)
					{
						rangeSlider.PreviewMouseWheel += OnRangeSliderPreviewMouseWheel;
					}
				}
			}
		}

		private static void OnSliderPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null && (slider.IsFocused || 2.Equals(slider.GetValue(EnableMouseWheelProperty))))
			{
				double num = ((MouseWheelChange)slider.GetValue(ChangeValueByProperty) == MouseWheelChange.LargeChange) ? slider.LargeChange : slider.SmallChange;
				if (e.Delta > 0)
				{
					slider.Value += num;
				}
				else
				{
					slider.Value -= num;
				}
				e.Handled = true;
			}
		}

		private static void OnRangeSliderPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			RangeSlider rangeSlider = sender as RangeSlider;
			if (rangeSlider != null && (rangeSlider.IsFocused || 2.Equals(rangeSlider.GetValue(EnableMouseWheelProperty))))
			{
				double num = ((MouseWheelChange)rangeSlider.GetValue(ChangeValueByProperty) == MouseWheelChange.LargeChange) ? rangeSlider.LargeChange : rangeSlider.SmallChange;
				if (e.Delta > 0)
				{
					rangeSlider.LowerValue += num;
					rangeSlider.UpperValue += num;
				}
				else
				{
					rangeSlider.LowerValue -= num;
					rangeSlider.UpperValue -= num;
				}
				e.Handled = true;
			}
		}
	}
}
