using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ControlzEx
{
	[TemplatePart(Name = "PART_ItemsHolder", Type = typeof(Panel))]
	public class TabControlEx : TabControl
	{
		public static readonly DependencyProperty ChildContentVisibilityProperty = DependencyProperty.Register("ChildContentVisibility", typeof(Visibility), typeof(TabControlEx), new PropertyMetadata(Visibility.Collapsed));

		private Panel _itemsHolder;

		public Visibility ChildContentVisibility
		{
			get
			{
				return (Visibility)GetValue(ChildContentVisibilityProperty);
			}
			set
			{
				SetValue(ChildContentVisibilityProperty, value);
			}
		}

		public TabControlEx()
		{
			base.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
			base.Loaded += TabControlEx_Loaded;
		}

		private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
		{
			if (base.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				base.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
				UpdateSelectedItem();
			}
		}

		private void TabControlEx_Loaded(object sender, RoutedEventArgs e)
		{
			UpdateSelectedItem();
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_itemsHolder = (GetTemplateChild("PART_ItemsHolder") as Panel);
			UpdateSelectedItem();
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (_itemsHolder != null)
			{
				switch (e.Action)
				{
				case NotifyCollectionChangedAction.Move:
					break;
				case NotifyCollectionChangedAction.Reset:
					_itemsHolder.Children.Clear();
					break;
				case NotifyCollectionChangedAction.Add:
				case NotifyCollectionChangedAction.Remove:
					if (e.OldItems != null)
					{
						foreach (object oldItem in e.OldItems)
						{
							ContentPresenter contentPresenter = FindChildContentPresenter(oldItem);
							if (contentPresenter != null)
							{
								_itemsHolder.Children.Remove(contentPresenter);
							}
						}
					}
					UpdateSelectedItem();
					break;
				case NotifyCollectionChangedAction.Replace:
					throw new NotImplementedException("Replace not implemented yet");
				}
			}
		}

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);
			UpdateSelectedItem();
		}

		private void UpdateSelectedItem()
		{
			if (_itemsHolder != null)
			{
				TabItem selectedTabItem = GetSelectedTabItem();
				if (selectedTabItem != null)
				{
					CreateChildContentPresenter(selectedTabItem);
				}
				foreach (ContentPresenter child in _itemsHolder.Children)
				{
					child.Visibility = ((!(child.Tag as TabItem).IsSelected) ? ChildContentVisibility : Visibility.Visible);
				}
			}
		}

		private ContentPresenter CreateChildContentPresenter(object item)
		{
			if (item == null)
			{
				return null;
			}
			ContentPresenter contentPresenter = FindChildContentPresenter(item);
			if (contentPresenter != null)
			{
				return contentPresenter;
			}
			TabItem tabItem = item as TabItem;
			contentPresenter = new ContentPresenter();
			contentPresenter.Content = ((tabItem != null) ? tabItem.Content : item);
			contentPresenter.ContentTemplate = base.SelectedContentTemplate;
			contentPresenter.ContentTemplateSelector = base.SelectedContentTemplateSelector;
			contentPresenter.ContentStringFormat = base.SelectedContentStringFormat;
			contentPresenter.Visibility = ChildContentVisibility;
			contentPresenter.Tag = (tabItem ?? base.ItemContainerGenerator.ContainerFromItem(item));
			_itemsHolder.Children.Add(contentPresenter);
			return contentPresenter;
		}

		private ContentPresenter FindChildContentPresenter(object data)
		{
			if (data is TabItem)
			{
				data = ((TabItem)data).Content;
			}
			if (data == null)
			{
				return null;
			}
			if (_itemsHolder == null)
			{
				return null;
			}
			foreach (ContentPresenter child in _itemsHolder.Children)
			{
				if (child.Content == data)
				{
					return child;
				}
			}
			return null;
		}

		protected TabItem GetSelectedTabItem()
		{
			object selectedItem = base.SelectedItem;
			if (selectedItem == null)
			{
				return null;
			}
			TabItem tabItem = selectedItem as TabItem;
			if (tabItem == null)
			{
				tabItem = (base.ItemContainerGenerator.ContainerFromIndex(base.SelectedIndex) as TabItem);
			}
			return tabItem;
		}
	}
}
