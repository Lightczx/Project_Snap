using ControlzEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(Flyout))]
	public class FlyoutsControl : ItemsControl
	{
		public static readonly DependencyProperty OverrideExternalCloseButtonProperty;

		public static readonly DependencyProperty OverrideIsPinnedProperty;

		public MouseButton? OverrideExternalCloseButton
		{
			get
			{
				return (MouseButton?)GetValue(OverrideExternalCloseButtonProperty);
			}
			set
			{
				SetValue(OverrideExternalCloseButtonProperty, value);
			}
		}

		public bool OverrideIsPinned
		{
			get
			{
				return (bool)GetValue(OverrideIsPinnedProperty);
			}
			set
			{
				SetValue(OverrideIsPinnedProperty, value);
			}
		}

		static FlyoutsControl()
		{
			OverrideExternalCloseButtonProperty = DependencyProperty.Register("OverrideExternalCloseButton", typeof(MouseButton?), typeof(FlyoutsControl), new PropertyMetadata(null));
			OverrideIsPinnedProperty = DependencyProperty.Register("OverrideIsPinned", typeof(bool), typeof(FlyoutsControl), new PropertyMetadata(false));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlyoutsControl), new FrameworkPropertyMetadata(typeof(FlyoutsControl)));
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new Flyout();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is Flyout;
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			Flyout flyout = element as Flyout;
			DataTemplate dataTemplate = flyout?.HeaderTemplate;
			DataTemplateSelector dataTemplateSelector = flyout?.HeaderTemplateSelector;
			string text = flyout?.HeaderStringFormat;
			base.PrepareContainerForItemOverride(element, item);
			if (flyout != null)
			{
				if (dataTemplate != null)
				{
					flyout.SetValue(HeaderedContentControl.HeaderTemplateProperty, dataTemplate);
				}
				if (dataTemplateSelector != null)
				{
					flyout.SetValue(HeaderedContentControl.HeaderTemplateSelectorProperty, dataTemplateSelector);
				}
				if (text != null)
				{
					flyout.SetValue(HeaderedContentControl.HeaderStringFormatProperty, text);
				}
				if (base.ItemTemplate != null && flyout.ContentTemplate == null)
				{
					flyout.SetValue(ContentControl.ContentTemplateProperty, base.ItemTemplate);
				}
				if (base.ItemTemplateSelector != null && flyout.ContentTemplateSelector == null)
				{
					flyout.SetValue(ContentControl.ContentTemplateSelectorProperty, base.ItemTemplateSelector);
				}
				if (base.ItemStringFormat != null && flyout.ContentStringFormat == null)
				{
					flyout.SetValue(ContentControl.ContentStringFormatProperty, base.ItemStringFormat);
				}
			}
			AttachHandlers((Flyout)element);
		}

		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			((Flyout)element).CleanUp(this);
			base.ClearContainerForItemOverride(element, item);
		}

		private void AttachHandlers(Flyout flyout)
		{
			PropertyChangeNotifier propertyChangeNotifier = new PropertyChangeNotifier(flyout, Flyout.IsOpenProperty);
			propertyChangeNotifier.ValueChanged += FlyoutStatusChanged;
			flyout.IsOpenPropertyChangeNotifier = propertyChangeNotifier;
			PropertyChangeNotifier propertyChangeNotifier2 = new PropertyChangeNotifier(flyout, Flyout.ThemeProperty);
			propertyChangeNotifier2.ValueChanged += FlyoutStatusChanged;
			flyout.ThemePropertyChangeNotifier = propertyChangeNotifier2;
		}

		private void FlyoutStatusChanged(object sender, EventArgs e)
		{
			Flyout flyout = GetFlyout(sender);
			HandleFlyoutStatusChange(flyout, this.TryFindParent<MetroWindow>());
		}

		internal void HandleFlyoutStatusChange(Flyout flyout, MetroWindow parentWindow)
		{
			if (flyout != null && parentWindow != null)
			{
				ReorderZIndices(flyout);
				List<Flyout> visibleFlyouts = (from i in GetFlyouts(base.Items)
				where i.IsOpen
				select i).OrderBy(Panel.GetZIndex).ToList();
				parentWindow.HandleFlyoutStatusChange(flyout, visibleFlyouts);
			}
		}

		private Flyout GetFlyout(object item)
		{
			Flyout flyout = item as Flyout;
			if (flyout != null)
			{
				return flyout;
			}
			return (Flyout)base.ItemContainerGenerator.ContainerFromItem(item);
		}

		internal IEnumerable<Flyout> GetFlyouts()
		{
			return GetFlyouts(base.Items);
		}

		private IEnumerable<Flyout> GetFlyouts(IEnumerable items)
		{
			return from object item in items
			select GetFlyout(item);
		}

		private void ReorderZIndices(Flyout lastChanged)
		{
			IOrderedEnumerable<Flyout> orderedEnumerable = GetFlyouts(base.Items).Where(delegate(Flyout i)
			{
				if (i.IsOpen)
				{
					return i != lastChanged;
				}
				return false;
			}).OrderBy(Panel.GetZIndex);
			int num = 0;
			foreach (Flyout item in orderedEnumerable)
			{
				Panel.SetZIndex(item, num);
				num++;
			}
			if (lastChanged.IsOpen)
			{
				Panel.SetZIndex(lastChanged, num);
			}
		}
	}
}
