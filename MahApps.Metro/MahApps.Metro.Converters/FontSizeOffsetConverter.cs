using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	public class FontSizeOffsetConverter : IValueConverter
	{
		private static FontSizeOffsetConverter _instance;

		public static FontSizeOffsetConverter Instance => _instance ?? (_instance = new FontSizeOffsetConverter());

		static FontSizeOffsetConverter()
		{
		}

		private FontSizeOffsetConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double && parameter is double)
			{
				double num = (double)parameter;
				return Math.Round((double)value + num);
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
