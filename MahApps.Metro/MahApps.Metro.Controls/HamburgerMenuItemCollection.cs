using System.Windows;

namespace MahApps.Metro.Controls
{
	public class HamburgerMenuItemCollection : FreezableCollection<HamburgerMenuItem>
	{
		protected override Freezable CreateInstanceCore()
		{
			return new HamburgerMenuItemCollection();
		}
	}
}
