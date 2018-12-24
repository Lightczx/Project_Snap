using MahApps.Metro.Controls;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Converters
{
	public static class TreeViewItemExtensions
	{
		public static int GetDepth(this TreeViewItem item)
		{
			return item.GetAncestors().TakeWhile((DependencyObject e) => !(e is TreeView)).OfType<TreeViewItem>()
				.Count();
		}
	}
}
