using System.Windows;

namespace MahApps.Metro.Controls
{
	public class ItemClickEventArgs : RoutedEventArgs
	{
		public object ClickedItem
		{
			get;
			internal set;
		}

		public ItemClickEventArgs(object clickedObject)
		{
			ClickedItem = clickedObject;
		}
	}
}
