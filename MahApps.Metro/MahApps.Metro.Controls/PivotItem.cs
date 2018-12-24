using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class PivotItem : ContentControl
	{
		public static readonly DependencyProperty HeaderProperty;

		public string Header
		{
			get
			{
				return (string)GetValue(HeaderProperty);
			}
			set
			{
				SetValue(HeaderProperty, value);
			}
		}

		static PivotItem()
		{
			HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(PivotItem), new PropertyMetadata((object)null));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PivotItem), new FrameworkPropertyMetadata(typeof(PivotItem)));
		}

		public PivotItem()
		{
			base.RequestBringIntoView += delegate(object s, RequestBringIntoViewEventArgs e)
			{
				e.Handled = true;
			};
		}
	}
}
