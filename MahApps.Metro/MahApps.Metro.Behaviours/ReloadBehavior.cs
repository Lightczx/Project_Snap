using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Behaviours
{
	public static class ReloadBehavior
	{
		public static readonly DependencyProperty OnDataContextChangedProperty = DependencyProperty.RegisterAttached("OnDataContextChanged", typeof(bool), typeof(ReloadBehavior), new PropertyMetadata(OnDataContextChanged));

		public static readonly DependencyProperty OnSelectedTabChangedProperty = DependencyProperty.RegisterAttached("OnSelectedTabChanged", typeof(bool), typeof(ReloadBehavior), new PropertyMetadata(OnSelectedTabChanged));

		public static readonly DependencyProperty MetroContentControlProperty = DependencyProperty.RegisterAttached("MetroContentControl", typeof(ContentControl), typeof(ReloadBehavior), new PropertyMetadata((object)null));

		[Category("MahApps.Metro")]
		public static bool GetOnDataContextChanged(MetroContentControl element)
		{
			return (bool)element.GetValue(OnDataContextChangedProperty);
		}

		[Category("MahApps.Metro")]
		public static void SetOnDataContextChanged(MetroContentControl element, bool value)
		{
			element.SetValue(OnDataContextChangedProperty, value);
		}

		private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MetroContentControl)d).DataContextChanged -= ReloadDataContextChanged;
			((MetroContentControl)d).DataContextChanged += ReloadDataContextChanged;
		}

		private static void ReloadDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			((MetroContentControl)sender).Reload();
		}

		[Category("MahApps.Metro")]
		public static bool GetOnSelectedTabChanged(ContentControl element)
		{
			return (bool)element.GetValue(OnSelectedTabChangedProperty);
		}

		[Category("MahApps.Metro")]
		public static void SetOnSelectedTabChanged(ContentControl element, bool value)
		{
			element.SetValue(OnSelectedTabChangedProperty, value);
		}

		private static void OnSelectedTabChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ContentControl)d).Loaded -= ReloadLoaded;
			((ContentControl)d).Loaded += ReloadLoaded;
		}

		private static void ReloadLoaded(object sender, RoutedEventArgs e)
		{
			ContentControl contentControl = (ContentControl)sender;
			TabControl tabControl = contentControl.TryFindParent<TabControl>();
			if (tabControl != null)
			{
				SetMetroContentControl(tabControl, contentControl);
				tabControl.SelectionChanged -= ReloadSelectionChanged;
				tabControl.SelectionChanged += ReloadSelectionChanged;
			}
		}

		private static void ReloadSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.OriginalSource == sender)
			{
				ContentControl metroContentControl = GetMetroContentControl((TabControl)sender);
				(metroContentControl as MetroContentControl)?.Reload();
				(metroContentControl as TransitioningContentControl)?.ReloadTransition();
			}
		}

		[Category("MahApps.Metro")]
		public static void SetMetroContentControl(UIElement element, ContentControl value)
		{
			element.SetValue(MetroContentControlProperty, value);
		}

		[Category("MahApps.Metro")]
		public static ContentControl GetMetroContentControl(UIElement element)
		{
			return (ContentControl)element.GetValue(MetroContentControlProperty);
		}
	}
}
