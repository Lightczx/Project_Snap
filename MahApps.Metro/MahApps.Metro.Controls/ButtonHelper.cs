using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MahApps.Metro.Controls
{
	public static class ButtonHelper
	{
		[Obsolete("This property will be deleted in the next release. You should use ContentCharacterCasing attached property located in ControlsHelper.")]
		public static readonly DependencyProperty PreserveTextCaseProperty = DependencyProperty.RegisterAttached("PreserveTextCase", typeof(bool), typeof(ButtonHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			ButtonBase buttonBase = o as ButtonBase;
			if (buttonBase != null && e.NewValue is bool)
			{
				ControlsHelper.SetContentCharacterCasing(buttonBase, (!(bool)e.NewValue) ? CharacterCasing.Upper : CharacterCasing.Normal);
			}
		}));

		[Obsolete("This property will be deleted in the next release. You should use CornerRadius attached property located in ControlsHelper.")]
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ButtonHelper), new FrameworkPropertyMetadata(new CornerRadius(-1.0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			UIElement uIElement = o as UIElement;
			if (uIElement != null && e.OldValue != e.NewValue && e.NewValue is CornerRadius)
			{
				ControlsHelper.SetCornerRadius(uIElement, (CornerRadius)e.NewValue);
			}
		}));

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ButtonBase))]
		[Obsolete("This property will be deleted in the next release. You should use ContentCharacterCasing attached property located in ControlsHelper.")]
		public static bool GetPreserveTextCase(UIElement element)
		{
			return (bool)element.GetValue(PreserveTextCaseProperty);
		}

		[Obsolete("This property will be deleted in the next release. You should use ContentCharacterCasing attached property located in ControlsHelper.")]
		public static void SetPreserveTextCase(UIElement element, bool value)
		{
			element.SetValue(PreserveTextCaseProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ButtonBase))]
		[AttachedPropertyBrowsableForType(typeof(ToggleButton))]
		[Obsolete("This property will be deleted in the next release. You should use CornerRadius attached property located in ControlsHelper.")]
		public static CornerRadius GetCornerRadius(UIElement element)
		{
			return (CornerRadius)element.GetValue(CornerRadiusProperty);
		}

		[Obsolete("This property will be deleted in the next release. You should use CornerRadius attached property located in ControlsHelper.")]
		public static void SetCornerRadius(UIElement element, CornerRadius value)
		{
			element.SetValue(CornerRadiusProperty, value);
		}
	}
}
