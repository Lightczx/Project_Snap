using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace MahApps.Metro.Behaviours
{
	public class TabControlSelectFirstVisibleTabBehavior : Behavior<TabControl>
	{
		protected override void OnAttached()
		{
			base.get_AssociatedObject().SelectionChanged += OnSelectionChanged;
		}

		private void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
		{
			List<TabItem> list = base.get_AssociatedObject().Items.Cast<TabItem>().ToList();
			TabItem tabItem = base.get_AssociatedObject().SelectedItem as TabItem;
			if (tabItem == null || tabItem.Visibility != 0)
			{
				TabItem tabItem2 = list.FirstOrDefault((TabItem t) => t.Visibility == Visibility.Visible);
				if (tabItem2 != null)
				{
					base.get_AssociatedObject().SelectedIndex = list.IndexOf(tabItem2);
				}
				else
				{
					base.get_AssociatedObject().SelectedItem = null;
				}
			}
		}

		protected override void OnDetaching()
		{
			base.get_AssociatedObject().SelectionChanged -= OnSelectionChanged;
		}
	}
}
