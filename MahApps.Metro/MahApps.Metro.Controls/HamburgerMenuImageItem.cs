using System.Windows;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public class HamburgerMenuImageItem : HamburgerMenuItem
	{
		public static readonly DependencyProperty ThumbnailProperty = DependencyProperty.Register("Thumbnail", typeof(ImageSource), typeof(HamburgerMenuImageItem), new PropertyMetadata(null));

		public ImageSource Thumbnail
		{
			get
			{
				return (ImageSource)GetValue(ThumbnailProperty);
			}
			set
			{
				SetValue(ThumbnailProperty, value);
			}
		}

		protected override Freezable CreateInstanceCore()
		{
			return new HamburgerMenuImageItem();
		}
	}
}
