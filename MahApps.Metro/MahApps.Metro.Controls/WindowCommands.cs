using ControlzEx;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(WindowCommands))]
	public class WindowCommands : ItemsControl, INotifyPropertyChanged
	{
		public static readonly DependencyProperty ThemeProperty;

		public static readonly DependencyProperty LightTemplateProperty;

		public static readonly DependencyProperty DarkTemplateProperty;

		public static readonly DependencyProperty ShowSeparatorsProperty;

		public static readonly DependencyProperty ShowLastSeparatorProperty;

		public static readonly DependencyProperty SeparatorHeightProperty;

		private Window _parentWindow;

		public Theme Theme
		{
			get
			{
				return (Theme)GetValue(ThemeProperty);
			}
			set
			{
				SetValue(ThemeProperty, value);
			}
		}

		public ControlTemplate LightTemplate
		{
			get
			{
				return (ControlTemplate)GetValue(LightTemplateProperty);
			}
			set
			{
				SetValue(LightTemplateProperty, value);
			}
		}

		public ControlTemplate DarkTemplate
		{
			get
			{
				return (ControlTemplate)GetValue(DarkTemplateProperty);
			}
			set
			{
				SetValue(DarkTemplateProperty, value);
			}
		}

		public bool ShowSeparators
		{
			get
			{
				return (bool)GetValue(ShowSeparatorsProperty);
			}
			set
			{
				SetValue(ShowSeparatorsProperty, value);
			}
		}

		public bool ShowLastSeparator
		{
			get
			{
				return (bool)GetValue(ShowLastSeparatorProperty);
			}
			set
			{
				SetValue(ShowLastSeparatorProperty, value);
			}
		}

		public int SeparatorHeight
		{
			get
			{
				return (int)GetValue(SeparatorHeightProperty);
			}
			set
			{
				SetValue(SeparatorHeightProperty, value);
			}
		}

		public Window ParentWindow
		{
			get
			{
				return _parentWindow;
			}
			set
			{
				if (!object.Equals(_parentWindow, value))
				{
					_parentWindow = value;
					RaisePropertyChanged("ParentWindow");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
			{
				WindowCommands windowCommands = (WindowCommands)d;
				if ((Theme)e.NewValue == Theme.Light)
				{
					if (windowCommands.LightTemplate != null)
					{
						windowCommands.SetValue(Control.TemplateProperty, windowCommands.LightTemplate);
					}
				}
				else if ((Theme)e.NewValue == Theme.Dark && windowCommands.DarkTemplate != null)
				{
					windowCommands.SetValue(Control.TemplateProperty, windowCommands.DarkTemplate);
				}
			}
		}

		private static void OnShowSeparatorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
			{
				((WindowCommands)d).ResetSeparators();
			}
		}

		private static void OnShowLastSeparatorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
			{
				((WindowCommands)d).ResetSeparators(reset: false);
			}
		}

		static WindowCommands()
		{
			ThemeProperty = DependencyProperty.Register("Theme", typeof(Theme), typeof(WindowCommands), new PropertyMetadata(Theme.Light, OnThemeChanged));
			LightTemplateProperty = DependencyProperty.Register("LightTemplate", typeof(ControlTemplate), typeof(WindowCommands), new PropertyMetadata(null));
			DarkTemplateProperty = DependencyProperty.Register("DarkTemplate", typeof(ControlTemplate), typeof(WindowCommands), new PropertyMetadata(null));
			ShowSeparatorsProperty = DependencyProperty.Register("ShowSeparators", typeof(bool), typeof(WindowCommands), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, OnShowSeparatorsChanged));
			ShowLastSeparatorProperty = DependencyProperty.Register("ShowLastSeparator", typeof(bool), typeof(WindowCommands), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender, OnShowLastSeparatorChanged));
			SeparatorHeightProperty = DependencyProperty.Register("SeparatorHeight", typeof(int), typeof(WindowCommands), new FrameworkPropertyMetadata(15, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCommands), new FrameworkPropertyMetadata(typeof(WindowCommands)));
		}

		public WindowCommands()
		{
			base.Loaded += WindowCommands_Loaded;
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new WindowCommandsItem();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is WindowCommandsItem;
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			AttachVisibilityHandler(element as WindowCommandsItem, item as UIElement);
			ResetSeparators();
		}

		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			DetachVisibilityHandler(element as WindowCommandsItem);
			ResetSeparators(reset: false);
		}

		private void AttachVisibilityHandler(WindowCommandsItem container, UIElement item)
		{
			if (container != null)
			{
				if (item == null)
				{
					container.ApplyTemplate();
					UIElement uIElement = container.ContentTemplate?.LoadContent() as UIElement;
					if (uIElement == null)
					{
						container.Visibility = Visibility.Collapsed;
					}
				}
				else
				{
					container.Visibility = item.Visibility;
					PropertyChangeNotifier propertyChangeNotifier = new PropertyChangeNotifier(item, UIElement.VisibilityProperty);
					propertyChangeNotifier.ValueChanged += VisibilityPropertyChanged;
					container.VisibilityPropertyChangeNotifier = propertyChangeNotifier;
				}
			}
		}

		private void DetachVisibilityHandler(WindowCommandsItem container)
		{
			if (container != null)
			{
				container.VisibilityPropertyChangeNotifier = null;
			}
		}

		private void VisibilityPropertyChanged(object sender, EventArgs e)
		{
			UIElement uIElement = sender as UIElement;
			if (uIElement != null)
			{
				WindowCommandsItem windowCommandsItem = GetWindowCommandsItem(uIElement);
				if (windowCommandsItem != null)
				{
					windowCommandsItem.Visibility = uIElement.Visibility;
					ResetSeparators();
				}
			}
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			ResetSeparators();
		}

		private void ResetSeparators(bool reset = true)
		{
			if (base.Items.Count != 0)
			{
				List<WindowCommandsItem> list = GetWindowCommandsItems().ToList();
				if (reset)
				{
					foreach (WindowCommandsItem item in list)
					{
						item.IsSeparatorVisible = ShowSeparators;
					}
				}
				WindowCommandsItem windowCommandsItem = list.LastOrDefault((WindowCommandsItem i) => i.IsVisible);
				if (windowCommandsItem != null)
				{
					windowCommandsItem.IsSeparatorVisible = (ShowSeparators && ShowLastSeparator);
				}
			}
		}

		private WindowCommandsItem GetWindowCommandsItem(object item)
		{
			WindowCommandsItem windowCommandsItem = item as WindowCommandsItem;
			if (windowCommandsItem != null)
			{
				return windowCommandsItem;
			}
			return (WindowCommandsItem)base.ItemContainerGenerator.ContainerFromItem(item);
		}

		private IEnumerable<WindowCommandsItem> GetWindowCommandsItems()
		{
			return from object item in base.Items
			select GetWindowCommandsItem(item) into i
			where i != null
			select i;
		}

		private void WindowCommands_Loaded(object sender, RoutedEventArgs e)
		{
			base.Loaded -= WindowCommands_Loaded;
			Window parentWindow = ParentWindow;
			if (parentWindow == null)
			{
				ParentWindow = this.TryFindParent<Window>();
			}
		}

		protected virtual void RaisePropertyChanged(string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
