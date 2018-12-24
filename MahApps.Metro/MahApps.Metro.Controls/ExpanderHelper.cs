using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public static class ExpanderHelper
	{
		public static readonly DependencyProperty HeaderUpStyleProperty = DependencyProperty.RegisterAttached("HeaderUpStyle", typeof(Style), typeof(ExpanderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty HeaderDownStyleProperty = DependencyProperty.RegisterAttached("HeaderDownStyle", typeof(Style), typeof(ExpanderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty HeaderLeftStyleProperty = DependencyProperty.RegisterAttached("HeaderLeftStyle", typeof(Style), typeof(ExpanderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty HeaderRightStyleProperty = DependencyProperty.RegisterAttached("HeaderRightStyle", typeof(Style), typeof(ExpanderHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Expander))]
		public static Style GetHeaderUpStyle(UIElement element)
		{
			return (Style)element.GetValue(HeaderUpStyleProperty);
		}

		public static void SetHeaderUpStyle(UIElement element, Style value)
		{
			element.SetValue(HeaderUpStyleProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Expander))]
		public static Style GetHeaderDownStyle(UIElement element)
		{
			return (Style)element.GetValue(HeaderDownStyleProperty);
		}

		public static void SetHeaderDownStyle(UIElement element, Style value)
		{
			element.SetValue(HeaderDownStyleProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Expander))]
		public static Style GetHeaderLeftStyle(UIElement element)
		{
			return (Style)element.GetValue(HeaderLeftStyleProperty);
		}

		public static void SetHeaderLeftStyle(UIElement element, Style value)
		{
			element.SetValue(HeaderLeftStyleProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(Expander))]
		public static Style GetHeaderRightStyle(UIElement element)
		{
			return (Style)element.GetValue(HeaderRightStyleProperty);
		}

		public static void SetHeaderRightStyle(UIElement element, Style value)
		{
			element.SetValue(HeaderRightStyleProperty, value);
		}
	}
}
