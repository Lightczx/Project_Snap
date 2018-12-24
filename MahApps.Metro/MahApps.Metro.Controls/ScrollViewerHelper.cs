using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public static class ScrollViewerHelper
	{
		public static readonly DependencyProperty VerticalScrollBarOnLeftSideProperty = DependencyProperty.RegisterAttached("VerticalScrollBarOnLeftSide", typeof(bool), typeof(ScrollViewerHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty IsHorizontalScrollWheelEnabledProperty = DependencyProperty.RegisterAttached("IsHorizontalScrollWheelEnabled", typeof(bool), typeof(ScrollViewerHelper), new PropertyMetadata(false, OnIsHorizontalScrollWheelEnabledPropertyChangedCallback));

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ScrollViewer))]
		public static bool GetVerticalScrollBarOnLeftSide(UIElement element)
		{
			return (bool)element.GetValue(VerticalScrollBarOnLeftSideProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(ScrollViewer))]
		public static void SetVerticalScrollBarOnLeftSide(UIElement element, bool value)
		{
			element.SetValue(VerticalScrollBarOnLeftSideProperty, value);
		}

		private static void OnIsHorizontalScrollWheelEnabledPropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewer scrollViewer = o as ScrollViewer;
			if (scrollViewer != null && e.NewValue != e.OldValue && e.NewValue is bool)
			{
				scrollViewer.PreviewMouseWheel -= ScrollViewerOnPreviewMouseWheel;
				if ((bool)e.NewValue)
				{
					scrollViewer.PreviewMouseWheel += ScrollViewerOnPreviewMouseWheel;
				}
			}
		}

		private static void ScrollViewerOnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			ScrollViewer scrollViewer = sender as ScrollViewer;
			if (scrollViewer != null && scrollViewer.HorizontalScrollBarVisibility != 0)
			{
				if (e.Delta > 0)
				{
					scrollViewer.LineLeft();
				}
				else
				{
					scrollViewer.LineRight();
				}
				e.Handled = true;
			}
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(UIElement))]
		public static bool GetIsHorizontalScrollWheelEnabled(UIElement element)
		{
			return (bool)element.GetValue(IsHorizontalScrollWheelEnabledProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(UIElement))]
		public static void SetIsHorizontalScrollWheelEnabled(UIElement element, bool value)
		{
			element.SetValue(IsHorizontalScrollWheelEnabledProperty, value);
		}
	}
}
