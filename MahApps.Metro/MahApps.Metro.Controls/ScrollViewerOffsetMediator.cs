using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class ScrollViewerOffsetMediator : FrameworkElement
	{
		public static readonly DependencyProperty ScrollViewerProperty = DependencyProperty.Register("ScrollViewer", typeof(ScrollViewer), typeof(ScrollViewerOffsetMediator), new PropertyMetadata(null, OnScrollViewerChanged));

		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(ScrollViewerOffsetMediator), new PropertyMetadata(0.0, OnHorizontalOffsetChanged));

		public ScrollViewer ScrollViewer
		{
			get
			{
				return (ScrollViewer)GetValue(ScrollViewerProperty);
			}
			set
			{
				SetValue(ScrollViewerProperty, value);
			}
		}

		public double HorizontalOffset
		{
			get
			{
				return (double)GetValue(HorizontalOffsetProperty);
			}
			set
			{
				SetValue(HorizontalOffsetProperty, value);
			}
		}

		private static void OnScrollViewerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewerOffsetMediator scrollViewerOffsetMediator = (ScrollViewerOffsetMediator)o;
			((ScrollViewer)e.NewValue)?.ScrollToHorizontalOffset(scrollViewerOffsetMediator.HorizontalOffset);
		}

		private static void OnHorizontalOffsetChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			ScrollViewerOffsetMediator scrollViewerOffsetMediator = (ScrollViewerOffsetMediator)o;
			if (scrollViewerOffsetMediator.ScrollViewer != null)
			{
				scrollViewerOffsetMediator.ScrollViewer.ScrollToHorizontalOffset((double)e.NewValue);
			}
		}
	}
}
