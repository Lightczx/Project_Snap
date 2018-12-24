using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_Scroll", Type = typeof(ScrollViewer))]
	[TemplatePart(Name = "PART_Headers", Type = typeof(ListView))]
	[TemplatePart(Name = "PART_Mediator", Type = typeof(ScrollViewerOffsetMediator))]
	public class Pivot : ItemsControl
	{
		private ScrollViewer scroller;

		private ListView headers;

		private PivotItem selectedItem;

		private ScrollViewerOffsetMediator mediator;

		internal int internalIndex;

		public static readonly RoutedEvent SelectionChangedEvent;

		public static readonly DependencyProperty HeaderProperty;

		public static readonly DependencyProperty HeaderTemplateProperty;

		public static readonly DependencyProperty SelectedIndexProperty;

		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)GetValue(HeaderTemplateProperty);
			}
			set
			{
				SetValue(HeaderTemplateProperty, value);
			}
		}

		public string Header
		{
			get
			{
				return (string)GetValue(HeaderProperty);
			}
			set
			{
				SetValue(HeaderProperty, value);
			}
		}

		public int SelectedIndex
		{
			get
			{
				return (int)GetValue(SelectedIndexProperty);
			}
			set
			{
				SetValue(SelectedIndexProperty, value);
			}
		}

		public event RoutedEventHandler SelectionChanged
		{
			add
			{
				AddHandler(SelectionChangedEvent, value);
			}
			remove
			{
				RemoveHandler(SelectionChangedEvent, value);
			}
		}

		public void GoToItem(PivotItem item)
		{
			if (item != null && item != selectedItem)
			{
				double num = 0.0;
				for (int i = 0; i < base.Items.Count; i++)
				{
					if (base.Items[i] == item)
					{
						internalIndex = i;
						break;
					}
					num += ((PivotItem)base.Items[i]).ActualWidth;
				}
				mediator.HorizontalOffset = scroller.HorizontalOffset;
				Storyboard obj = mediator.Resources["Storyboard1"] as Storyboard;
				((EasingDoubleKeyFrame)mediator.FindName("edkf")).Value = num;
				obj.Completed -= sb_Completed;
				obj.Completed += sb_Completed;
				obj.Begin();
				RaiseEvent(new RoutedEventArgs(SelectionChangedEvent));
			}
		}

		private void sb_Completed(object sender, EventArgs e)
		{
			SelectedIndex = internalIndex;
		}

		static Pivot()
		{
			SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Pivot));
			HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(Pivot), new PropertyMetadata((object)null));
			HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Pivot));
			SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Pivot), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedItemChanged));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Pivot), new FrameworkPropertyMetadata(typeof(Pivot)));
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			scroller = (ScrollViewer)GetTemplateChild("PART_Scroll");
			headers = (ListView)GetTemplateChild("PART_Headers");
			mediator = (GetTemplateChild("PART_Mediator") as ScrollViewerOffsetMediator);
			if (scroller != null)
			{
				scroller.ScrollChanged += scroller_ScrollChanged;
				scroller.PreviewMouseWheel += scroller_MouseWheel;
			}
			if (headers != null)
			{
				headers.SelectionChanged += headers_SelectionChanged;
			}
		}

		private void scroller_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			scroller.ScrollToHorizontalOffset(scroller.HorizontalOffset + (double)(-e.Delta));
		}

		private void headers_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			GoToItem((PivotItem)headers.SelectedItem);
		}

		private void scroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			double num = 0.0;
			int num2 = 0;
			PivotItem pivotItem;
			while (true)
			{
				if (num2 >= base.Items.Count)
				{
					return;
				}
				pivotItem = (PivotItem)base.Items[num2];
				double actualWidth = pivotItem.ActualWidth;
				if (e.HorizontalOffset <= num + actualWidth - 1.0)
				{
					break;
				}
				num += actualWidth;
				num2++;
			}
			selectedItem = pivotItem;
			if (headers.SelectedItem != selectedItem)
			{
				headers.SelectedItem = selectedItem;
				internalIndex = num2;
				SelectedIndex = num2;
				RaiseEvent(new RoutedEventArgs(SelectionChangedEvent));
			}
		}

		private static void SelectedItemChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
			{
				Pivot pivot = (Pivot)dependencyObject;
				int num = (int)e.NewValue;
				if (pivot.internalIndex != pivot.SelectedIndex && num >= 0 && num < pivot.Items.Count)
				{
					PivotItem item = (PivotItem)pivot.Items[num];
					pivot.headers.SelectedItem = item;
					pivot.GoToItem(item);
				}
			}
		}
	}
}
