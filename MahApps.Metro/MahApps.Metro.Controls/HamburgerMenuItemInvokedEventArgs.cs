using System;

namespace MahApps.Metro.Controls
{
	public class HamburgerMenuItemInvokedEventArgs : EventArgs
	{
		public object InvokedItem
		{
			get;
			internal set;
		}

		public bool IsItemOptions
		{
			get;
			internal set;
		}
	}
}
