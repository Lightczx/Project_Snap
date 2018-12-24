using System.Windows;

namespace MahApps.Metro.Controls
{
	public class HamburgerMenuGlyphItem : HamburgerMenuItem
	{
		public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", typeof(string), typeof(HamburgerMenuGlyphItem), new PropertyMetadata(null));

		public string Glyph
		{
			get
			{
				return (string)GetValue(GlyphProperty);
			}
			set
			{
				SetValue(GlyphProperty, value);
			}
		}

		protected override Freezable CreateInstanceCore()
		{
			return new HamburgerMenuGlyphItem();
		}
	}
}
