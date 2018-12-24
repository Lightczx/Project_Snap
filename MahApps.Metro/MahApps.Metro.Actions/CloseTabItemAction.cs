using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MahApps.Metro.Actions
{
	public class CloseTabItemAction : CommandTriggerAction
	{
		private TabItem associatedTabItem;

		[Obsolete("This property will be deleted in the next release.")]
		public static readonly DependencyProperty TabControlProperty = DependencyProperty.Register("TabControl", typeof(TabControl), typeof(CloseTabItemAction), new PropertyMetadata((object)null));

		[Obsolete("This property will be deleted in the next release.")]
		public static readonly DependencyProperty TabItemProperty = DependencyProperty.Register("TabItem", typeof(TabItem), typeof(CloseTabItemAction), new PropertyMetadata((object)null));

		private TabItem AssociatedTabItem => associatedTabItem ?? (associatedTabItem = base.get_AssociatedObject().TryFindParent<TabItem>());

		[Obsolete("This property will be deleted in the next release.")]
		public TabControl TabControl
		{
			get
			{
				return (TabControl)base.GetValue(TabControlProperty);
			}
			set
			{
				base.SetValue(TabControlProperty, (object)value);
			}
		}

		[Obsolete("This property will be deleted in the next release.")]
		public TabItem TabItem
		{
			get
			{
				return (TabItem)base.GetValue(TabItemProperty);
			}
			set
			{
				base.SetValue(TabItemProperty, (object)value);
			}
		}

		protected override void Invoke(object parameter)
		{
			if (base.get_AssociatedObject() != null && (base.get_AssociatedObject() == null || base.get_AssociatedObject().IsEnabled))
			{
				TabControl tabControl = base.get_AssociatedObject().TryFindParent<TabControl>();
				TabItem tabItem = AssociatedTabItem;
				if (tabControl != null && tabItem != null)
				{
					ICommand command = base.Command;
					if (command != null)
					{
						object commandParameter = GetCommandParameter();
						if (command.CanExecute(commandParameter))
						{
							command.Execute(commandParameter);
						}
					}
					if (tabControl is MetroTabControl && tabItem is MetroTabItem)
					{
						tabControl.BeginInvoke(delegate
						{
							((MetroTabControl)tabControl).CloseThisTabItem((MetroTabItem)tabItem);
						});
					}
					else
					{
						Action invokeAction = delegate
						{
							if (tabControl.ItemsSource == null)
							{
								tabItem.ClearStyle();
								tabControl.Items.Remove(tabItem);
							}
							else
							{
								IList list = tabControl.ItemsSource as IList;
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
						};
						((DispatcherObject)this).BeginInvoke(invokeAction);
					}
				}
			}
		}

		protected override object GetCommandParameter()
		{
			return base.CommandParameter ?? AssociatedTabItem;
		}
	}
}
