using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class ComboBoxHelper
	{
		public static readonly DependencyProperty EnableVirtualizationWithGroupingProperty = DependencyProperty.RegisterAttached("EnableVirtualizationWithGrouping", typeof(bool), typeof(ComboBoxHelper), new FrameworkPropertyMetadata(false, OnEnableVirtualizationWithGroupingChanged));

		public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.RegisterAttached("MaxLength", typeof(int), typeof(ComboBoxHelper), new FrameworkPropertyMetadata(0), ValidateMaxLength);

		public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.RegisterAttached("CharacterCasing", typeof(CharacterCasing), typeof(ComboBoxHelper), new FrameworkPropertyMetadata(CharacterCasing.Normal), ValidateCharacterCasing);

		private static void OnEnableVirtualizationWithGroupingChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			ComboBox comboBox = dependencyObject as ComboBox;
			if (comboBox != null && e.NewValue != e.OldValue)
			{
				comboBox.SetCurrentValue(VirtualizingStackPanel.IsVirtualizingProperty, e.NewValue);
				comboBox.SetCurrentValue(VirtualizingPanel.IsVirtualizingWhenGroupingProperty, e.NewValue);
				comboBox.SetCurrentValue(ScrollViewer.CanContentScrollProperty, e.NewValue);
			}
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		public static void SetEnableVirtualizationWithGrouping(DependencyObject obj, bool value)
		{
			obj.SetValue(EnableVirtualizationWithGroupingProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		public static bool GetEnableVirtualizationWithGrouping(DependencyObject obj)
		{
			return (bool)obj.GetValue(EnableVirtualizationWithGroupingProperty);
		}

		private static bool ValidateMaxLength(object value)
		{
			return (int)value >= 0;
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		public static int GetMaxLength(UIElement obj)
		{
			return (int)obj.GetValue(MaxLengthProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		public static void SetMaxLength(UIElement obj, int value)
		{
			obj.SetValue(MaxLengthProperty, value);
		}

		private static bool ValidateCharacterCasing(object value)
		{
			if (CharacterCasing.Normal <= (CharacterCasing)value)
			{
				return (CharacterCasing)value <= CharacterCasing.Upper;
			}
			return false;
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		public static CharacterCasing GetCharacterCasing(UIElement obj)
		{
			return (CharacterCasing)obj.GetValue(CharacterCasingProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		public static void SetCharacterCasing(UIElement obj, CharacterCasing value)
		{
			obj.SetValue(CharacterCasingProperty, value);
		}
	}
}
