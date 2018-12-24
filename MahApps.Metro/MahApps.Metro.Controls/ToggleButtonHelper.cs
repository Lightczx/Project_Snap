using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MahApps.Metro.Controls
{
	public static class ToggleButtonHelper
	{
		public static readonly DependencyProperty ContentDirectionProperty = DependencyProperty.RegisterAttached("ContentDirection", typeof(FlowDirection), typeof(ToggleButtonHelper), new FrameworkPropertyMetadata(FlowDirection.LeftToRight, ContentDirectionPropertyChanged));

		[AttachedPropertyBrowsableForType(typeof(ToggleButton))]
		[AttachedPropertyBrowsableForType(typeof(RadioButton))]
		[Category("MahApps.Metro")]
		public static FlowDirection GetContentDirection(UIElement element)
		{
			return (FlowDirection)element.GetValue(ContentDirectionProperty);
		}

		public static void SetContentDirection(UIElement element, FlowDirection value)
		{
			element.SetValue(ContentDirectionProperty, value);
		}

		private static void ContentDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ToggleButton toggleButton = d as ToggleButton;
			if (toggleButton == null)
			{
				throw new InvalidOperationException("The property 'ContentDirection' may only be set on ToggleButton elements.");
			}
		}
	}
}
