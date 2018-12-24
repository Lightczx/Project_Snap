using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public static class ControlsHelper
	{
		public static readonly DependencyProperty DisabledVisualElementVisibilityProperty = DependencyProperty.RegisterAttached("DisabledVisualElementVisibility", typeof(Visibility), typeof(ControlsHelper), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty ContentCharacterCasingProperty = DependencyProperty.RegisterAttached("ContentCharacterCasing", typeof(CharacterCasing), typeof(ControlsHelper), new FrameworkPropertyMetadata(CharacterCasing.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure), delegate(object value)
		{
			if (CharacterCasing.Normal <= (CharacterCasing)value)
			{
				return (CharacterCasing)value <= CharacterCasing.Upper;
			}
			return false;
		});

		public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.RegisterAttached("HeaderFontSize", typeof(double), typeof(ControlsHelper), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize)
		{
			Inherits = true
		});

		public static readonly DependencyProperty HeaderFontStretchProperty = DependencyProperty.RegisterAttached("HeaderFontStretch", typeof(FontStretch), typeof(ControlsHelper), new UIPropertyMetadata(FontStretches.Normal));

		public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.RegisterAttached("HeaderFontWeight", typeof(FontWeight), typeof(ControlsHelper), new UIPropertyMetadata(FontWeights.Normal));

		public static readonly DependencyProperty HeaderMarginProperty = DependencyProperty.RegisterAttached("HeaderMargin", typeof(Thickness), typeof(ControlsHelper), new UIPropertyMetadata(default(Thickness)));

		[Obsolete("This property will be deleted in the next release. You should use TextBoxHelper.ButtonWidth instead.")]
		public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.RegisterAttached("ButtonWidth", typeof(double), typeof(ControlsHelper), new FrameworkPropertyMetadata(22.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			UIElement uIElement = o as UIElement;
			if (uIElement != null && e.OldValue != e.NewValue && e.NewValue is double)
			{
				TextBoxHelper.SetButtonWidth(uIElement, (double)e.NewValue);
			}
		}));

		public static readonly DependencyProperty FocusBorderBrushProperty = DependencyProperty.RegisterAttached("FocusBorderBrush", typeof(Brush), typeof(ControlsHelper), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.RegisterAttached("MouseOverBorderBrush", typeof(Brush), typeof(ControlsHelper), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ControlsHelper), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.RegisterAttached("IsReadOnly", typeof(bool), typeof(ControlsHelper), new FrameworkPropertyMetadata(false));

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
		[AttachedPropertyBrowsableForType(typeof(PasswordBox))]
		[AttachedPropertyBrowsableForType(typeof(NumericUpDown))]
		public static Visibility GetDisabledVisualElementVisibility(UIElement element)
		{
			return (Visibility)element.GetValue(DisabledVisualElementVisibilityProperty);
		}

		public static void SetDisabledVisualElementVisibility(UIElement element, Visibility value)
		{
			element.SetValue(DisabledVisualElementVisibilityProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ContentControl))]
		[AttachedPropertyBrowsableForType(typeof(DropDownButton))]
		[AttachedPropertyBrowsableForType(typeof(WindowCommands))]
		public static CharacterCasing GetContentCharacterCasing(UIElement element)
		{
			return (CharacterCasing)element.GetValue(ContentCharacterCasingProperty);
		}

		public static void SetContentCharacterCasing(UIElement element, CharacterCasing value)
		{
			element.SetValue(ContentCharacterCasingProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
		[AttachedPropertyBrowsableForType(typeof(Flyout))]
		public static double GetHeaderFontSize(UIElement element)
		{
			return (double)element.GetValue(HeaderFontSizeProperty);
		}

		public static void SetHeaderFontSize(UIElement element, double value)
		{
			element.SetValue(HeaderFontSizeProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
		[AttachedPropertyBrowsableForType(typeof(Flyout))]
		public static FontStretch GetHeaderFontStretch(UIElement element)
		{
			return (FontStretch)element.GetValue(HeaderFontStretchProperty);
		}

		public static void SetHeaderFontStretch(UIElement element, FontStretch value)
		{
			element.SetValue(HeaderFontStretchProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
		[AttachedPropertyBrowsableForType(typeof(Flyout))]
		public static FontWeight GetHeaderFontWeight(UIElement element)
		{
			return (FontWeight)element.GetValue(HeaderFontWeightProperty);
		}

		public static void SetHeaderFontWeight(UIElement element, FontWeight value)
		{
			element.SetValue(HeaderFontWeightProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
		[AttachedPropertyBrowsableForType(typeof(Flyout))]
		public static Thickness GetHeaderMargin(UIElement element)
		{
			return (Thickness)element.GetValue(HeaderMarginProperty);
		}

		public static void SetHeaderMargin(UIElement element, Thickness value)
		{
			element.SetValue(HeaderMarginProperty, value);
		}

		[Category("MahApps.Metro")]
		[Obsolete("This property will be deleted in the next release. You should use TextBoxHelper.ButtonWidth instead.")]
		public static double GetButtonWidth(DependencyObject obj)
		{
			return (double)obj.GetValue(ButtonWidthProperty);
		}

		[Obsolete("This property will be deleted in the next release. You should use TextBoxHelper.ButtonWidth instead.")]
		public static void SetButtonWidth(DependencyObject obj, double value)
		{
			obj.SetValue(ButtonWidthProperty, value);
		}

		public static void SetFocusBorderBrush(DependencyObject obj, Brush value)
		{
			obj.SetValue(FocusBorderBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBox))]
		[AttachedPropertyBrowsableForType(typeof(CheckBox))]
		[AttachedPropertyBrowsableForType(typeof(RadioButton))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		public static Brush GetFocusBorderBrush(DependencyObject obj)
		{
			return (Brush)obj.GetValue(FocusBorderBrushProperty);
		}

		public static void SetMouseOverBorderBrush(DependencyObject obj, Brush value)
		{
			obj.SetValue(MouseOverBorderBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(TextBox))]
		[AttachedPropertyBrowsableForType(typeof(CheckBox))]
		[AttachedPropertyBrowsableForType(typeof(RadioButton))]
		[AttachedPropertyBrowsableForType(typeof(DatePicker))]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		[AttachedPropertyBrowsableForType(typeof(Tile))]
		public static Brush GetMouseOverBorderBrush(DependencyObject obj)
		{
			return (Brush)obj.GetValue(MouseOverBorderBrushProperty);
		}

		[Category("MahApps.Metro")]
		public static CornerRadius GetCornerRadius(UIElement element)
		{
			return (CornerRadius)element.GetValue(CornerRadiusProperty);
		}

		public static void SetCornerRadius(UIElement element, CornerRadius value)
		{
			element.SetValue(CornerRadiusProperty, value);
		}

		public static bool GetIsReadOnly(UIElement element)
		{
			return (bool)element.GetValue(IsReadOnlyProperty);
		}

		public static void SetIsReadOnly(UIElement element, bool value)
		{
			element.SetValue(IsReadOnlyProperty, value);
		}
	}
}
