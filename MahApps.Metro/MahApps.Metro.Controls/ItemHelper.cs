using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public static class ItemHelper
	{
		public static readonly DependencyProperty ActiveSelectionBackgroundBrushProperty = DependencyProperty.RegisterAttached("ActiveSelectionBackgroundBrush", typeof(Brush), typeof(ItemHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty ActiveSelectionForegroundBrushProperty = DependencyProperty.RegisterAttached("ActiveSelectionForegroundBrush", typeof(Brush), typeof(ItemHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty SelectedBackgroundBrushProperty = DependencyProperty.RegisterAttached("SelectedBackgroundBrush", typeof(Brush), typeof(ItemHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty SelectedForegroundBrushProperty = DependencyProperty.RegisterAttached("SelectedForegroundBrush", typeof(Brush), typeof(ItemHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty HoverBackgroundBrushProperty = DependencyProperty.RegisterAttached("HoverBackgroundBrush", typeof(Brush), typeof(ItemHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty HoverSelectedBackgroundBrushProperty = DependencyProperty.RegisterAttached("HoverSelectedBackgroundBrush", typeof(Brush), typeof(ItemHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty DisabledSelectedBackgroundBrushProperty = DependencyProperty.RegisterAttached("DisabledSelectedBackgroundBrush", typeof(Brush), typeof(ItemHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty DisabledSelectedForegroundBrushProperty = DependencyProperty.RegisterAttached("DisabledSelectedForegroundBrush", typeof(Brush), typeof(ItemHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty DisabledBackgroundBrushProperty = DependencyProperty.RegisterAttached("DisabledBackgroundBrush", typeof(Brush), typeof(ItemHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty DisabledForegroundBrushProperty = DependencyProperty.RegisterAttached("DisabledForegroundBrush", typeof(Brush), typeof(ItemHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static Brush GetActiveSelectionBackgroundBrush(UIElement element)
		{
			return (Brush)element.GetValue(ActiveSelectionBackgroundBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static void SetActiveSelectionBackgroundBrush(UIElement element, Brush value)
		{
			element.SetValue(ActiveSelectionBackgroundBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static Brush GetActiveSelectionForegroundBrush(UIElement element)
		{
			return (Brush)element.GetValue(ActiveSelectionForegroundBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static void SetActiveSelectionForegroundBrush(UIElement element, Brush value)
		{
			element.SetValue(ActiveSelectionForegroundBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static Brush GetSelectedBackgroundBrush(UIElement element)
		{
			return (Brush)element.GetValue(SelectedBackgroundBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static void SetSelectedBackgroundBrush(UIElement element, Brush value)
		{
			element.SetValue(SelectedBackgroundBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static Brush GetSelectedForegroundBrush(UIElement element)
		{
			return (Brush)element.GetValue(SelectedForegroundBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static void SetSelectedForegroundBrush(UIElement element, Brush value)
		{
			element.SetValue(SelectedForegroundBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static Brush GetHoverBackgroundBrush(UIElement element)
		{
			return (Brush)element.GetValue(HoverBackgroundBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static void SetHoverBackgroundBrush(UIElement element, Brush value)
		{
			element.SetValue(HoverBackgroundBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static Brush GetHoverSelectedBackgroundBrush(UIElement element)
		{
			return (Brush)element.GetValue(HoverSelectedBackgroundBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static void SetHoverSelectedBackgroundBrush(UIElement element, Brush value)
		{
			element.SetValue(HoverSelectedBackgroundBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static Brush GetDisabledSelectedBackgroundBrush(UIElement element)
		{
			return (Brush)element.GetValue(DisabledSelectedBackgroundBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static void SetDisabledSelectedBackgroundBrush(UIElement element, Brush value)
		{
			element.SetValue(DisabledSelectedBackgroundBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static Brush GetDisabledSelectedForegroundBrush(UIElement element)
		{
			return (Brush)element.GetValue(DisabledSelectedForegroundBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static void SetDisabledSelectedForegroundBrush(UIElement element, Brush value)
		{
			element.SetValue(DisabledSelectedForegroundBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static Brush GetDisabledBackgroundBrush(UIElement element)
		{
			return (Brush)element.GetValue(DisabledBackgroundBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static void SetDisabledBackgroundBrush(UIElement element, Brush value)
		{
			element.SetValue(DisabledBackgroundBrushProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static Brush GetDisabledForegroundBrush(UIElement element)
		{
			return (Brush)element.GetValue(DisabledForegroundBrushProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ListBoxItem))]
		[AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
		public static void SetDisabledForegroundBrush(UIElement element, Brush value)
		{
			element.SetValue(DisabledForegroundBrushProperty, value);
		}
	}
}
