using System.Windows;

namespace MahApps.Metro.Controls
{
	public class HamburgerMenuIconItem : HamburgerMenuItem
	{
		public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(HamburgerMenuIconItem), new PropertyMetadata(null));

		public object Icon
		{
			get
			{
				return GetValue(IconProperty);
			}
			set
			{
				SetValue(IconProperty, value);
			}
		}

		protected override Freezable CreateInstanceCore()
		{
			return new HamburgerMenuIconItem();
		}
	}
}
