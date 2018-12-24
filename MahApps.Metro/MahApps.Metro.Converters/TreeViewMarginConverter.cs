using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	public class TreeViewMarginConverter : IValueConverter
	{
		public double Length
		{
			get;
			set;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			TreeViewItem treeViewItem = value as TreeViewItem;
			if (treeViewItem == null)
			{
				return new Thickness(0.0);
			}
			return new Thickness(Length * (double)treeViewItem.GetDepth(), 0.0, 0.0, 0.0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
