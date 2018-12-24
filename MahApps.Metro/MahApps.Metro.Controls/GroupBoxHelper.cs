using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	public static class GroupBoxHelper
	{
		public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.RegisterAttached("HeaderForeground", typeof(Brush), typeof(GroupBoxHelper), new UIPropertyMetadata(Brushes.White));

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(GroupBox))]
		[AttachedPropertyBrowsableForType(typeof(Expander))]
		public static Brush GetHeaderForeground(UIElement element)
		{
			return (Brush)element.GetValue(HeaderForegroundProperty);
		}

		public static void SetHeaderForeground(UIElement element, Brush value)
		{
			element.SetValue(HeaderForegroundProperty, value);
		}
	}
}
