using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public abstract class BaseMetroTabControl : TabControl
	{
		public delegate void TabItemClosingEventHandler(object sender, TabItemClosingEventArgs e);

		public class TabItemClosingEventArgs : CancelEventArgs
		{
			public MetroTabItem ClosingTabItem
			{
				get;
				private set;
			}

			internal TabItemClosingEventArgs(MetroTabItem item)
			{
				ClosingTabItem = item;
			}
		}

		public static readonly DependencyProperty TabStripMarginProperty = DependencyProperty.Register("TabStripMargin", typeof(Thickness), typeof(BaseMetroTabControl), new PropertyMetadata(new Thickness(0.0)));

		public static readonly DependencyProperty CloseTabCommandProperty = DependencyProperty.Register("CloseTabCommand", typeof(ICommand), typeof(BaseMetroTabControl), new PropertyMetadata(null));

		public Thickness TabStripMargin
		{
			get
			{
				return (Thickness)GetValue(TabStripMarginProperty);
			}
			set
			{
				SetValue(TabStripMarginProperty, value);
			}
		}

		public ICommand CloseTabCommand
		{
			get
			{
				return (ICommand)GetValue(CloseTabCommandProperty);
			}
			set
			{
				SetValue(CloseTabCommandProperty, value);
			}
		}

		public event TabItemClosingEventHandler TabItemClosingEvent;

		public BaseMetroTabControl()
		{
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TabItem;
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new MetroTabItem();
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			if (element != item)
			{
				element.SetValue(FrameworkElement.DataContextProperty, item);
			}
			base.PrepareContainerForItemOverride(element, item);
		}

		internal bool RaiseTabItemClosingEvent(MetroTabItem closingItem)
		{
			TabItemClosingEventHandler tabItemClosingEvent = this.TabItemClosingEvent;
			if (tabItemClosingEvent != null)
			{
				foreach (TabItemClosingEventHandler item in tabItemClosingEvent.GetInvocationList().OfType<TabItemClosingEventHandler>())
				{
					TabItemClosingEventArgs tabItemClosingEventArgs = new TabItemClosingEventArgs(closingItem);
					item(this, tabItemClosingEventArgs);
					if (tabItemClosingEventArgs.Cancel)
					{
						return true;
					}
				}
			}
			return false;
		}

		internal void CloseThisTabItem(MetroTabItem tabItem)
		{
			if (tabItem == null)
			{
				throw new ArgumentNullException("tabItem");
			}
			if (CloseTabCommand != null)
			{
				object parameter = tabItem.CloseTabCommandParameter ?? tabItem;
				if (CloseTabCommand.CanExecute(parameter))
				{
					CloseTabCommand.Execute(parameter);
				}
			}
			else if (!RaiseTabItemClosingEvent(tabItem))
			{
				if (base.ItemsSource == null)
				{
					tabItem.ClearStyle();
					base.Items.Remove(tabItem);
				}
				else
				{
					IList list = base.ItemsSource as IList;
					if (list != null)
					{
						object obj = list.OfType<object>().FirstOrDefault(delegate(object item)
						{
							if (tabItem != item)
							{
								return tabItem.DataContext == item;
							}
							return true;
						});
						if (obj != null)
						{
							tabItem.ClearStyle();
							list.Remove(obj);
						}
					}
				}
			}
		}
	}
}
