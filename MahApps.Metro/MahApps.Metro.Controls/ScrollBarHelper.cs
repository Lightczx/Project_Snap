using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	[Obsolete("This helper class will be deleted in the next release. Instead use the ScrollViewerHelper.")]
	public static class ScrollBarHelper
	{
		[Obsolete("This attached property will be deleted in the next release. Instead use ScrollViewerHelper.VerticalScrollBarOnLeftSide attached property.")]
		public static readonly DependencyProperty VerticalScrollBarOnLeftSideProperty = DependencyProperty.RegisterAttached("VerticalScrollBarOnLeftSide", typeof(bool), typeof(ScrollBarHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewer scrollViewer = o as ScrollViewer;
			if (scrollViewer != null && e.OldValue != e.NewValue && e.NewValue is bool)
			{
				ScrollViewerHelper.SetVerticalScrollBarOnLeftSide(scrollViewer, (bool)e.NewValue);
			}
		}));

		[Obsolete("This attached property will be deleted in the next release. Instead use ScrollViewerHelper.VerticalScrollBarOnLeftSide attached property.")]
		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ScrollViewer))]
		public static bool GetVerticalScrollBarOnLeftSide(ScrollViewer obj)
		{
			return (bool)obj.GetValue(VerticalScrollBarOnLeftSideProperty);
		}

		[Obsolete("This attached property will be deleted in the next release. Instead use ScrollViewerHelper.VerticalScrollBarOnLeftSide attached property.")]
		public static void SetVerticalScrollBarOnLeftSide(ScrollViewer obj, bool value)
		{
			obj.SetValue(VerticalScrollBarOnLeftSideProperty, value);
		}
	}
}
