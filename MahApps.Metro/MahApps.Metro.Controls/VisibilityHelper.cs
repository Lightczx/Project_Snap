using System.ComponentModel;
using System.Windows;

namespace MahApps.Metro.Controls
{
	public static class VisibilityHelper
	{
		public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached("IsVisible", typeof(bool?), typeof(VisibilityHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, IsVisibleChangedCallback));

		public static readonly DependencyProperty IsCollapsedProperty = DependencyProperty.RegisterAttached("IsCollapsed", typeof(bool?), typeof(VisibilityHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, IsCollapsedChangedCallback));

		public static readonly DependencyProperty IsHiddenProperty = DependencyProperty.RegisterAttached("IsHidden", typeof(bool?), typeof(VisibilityHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, IsHiddenChangedCallback));

		private static void IsVisibleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.Visibility = (((bool?)e.NewValue != true) ? Visibility.Collapsed : Visibility.Visible);
			}
		}

		public static void SetIsVisible(DependencyObject element, bool? value)
		{
			element.SetValue(IsVisibleProperty, value);
		}

		[Category("MahApps.Metro")]
		public static bool? GetIsVisible(DependencyObject element)
		{
			return (bool?)element.GetValue(IsVisibleProperty);
		}

		private static void IsCollapsedChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.Visibility = (((bool?)e.NewValue == true) ? Visibility.Collapsed : Visibility.Visible);
			}
		}

		public static void SetIsCollapsed(DependencyObject element, bool? value)
		{
			element.SetValue(IsCollapsedProperty, value);
		}

		[Category("MahApps.Metro")]
		public static bool? GetIsCollapsed(DependencyObject element)
		{
			return (bool?)element.GetValue(IsCollapsedProperty);
		}

		private static void IsHiddenChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.Visibility = (((bool?)e.NewValue == true) ? Visibility.Hidden : Visibility.Visible);
			}
		}

		public static void SetIsHidden(DependencyObject element, bool? value)
		{
			element.SetValue(IsHiddenProperty, value);
		}

		[Category("MahApps.Metro")]
		public static bool? GetIsHidden(DependencyObject element)
		{
			return (bool?)element.GetValue(IsHiddenProperty);
		}
	}
}
