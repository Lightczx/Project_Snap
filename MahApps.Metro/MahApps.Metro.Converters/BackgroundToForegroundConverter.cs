using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MahApps.Metro.Converters
{
	public class BackgroundToForegroundConverter : IValueConverter, IMultiValueConverter
	{
		private static BackgroundToForegroundConverter _instance;

		public static BackgroundToForegroundConverter Instance => _instance ?? (_instance = new BackgroundToForegroundConverter());

		static BackgroundToForegroundConverter()
		{
		}

		private BackgroundToForegroundConverter()
		{
		}

		private Color IdealTextColor(Color bg)
		{
			int num = System.Convert.ToInt32((double)(int)bg.R * 0.299 + (double)(int)bg.G * 0.587 + (double)(int)bg.B * 0.114);
			if (255 - num >= 86)
			{
				return Colors.White;
			}
			return Colors.Black;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is SolidColorBrush)
			{
				SolidColorBrush solidColorBrush = new SolidColorBrush(IdealTextColor(((SolidColorBrush)value).Color));
				solidColorBrush.Freeze();
				return solidColorBrush;
			}
			return Brushes.White;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Brush value = (values.Length != 0) ? (values[0] as Brush) : null;
			Brush brush = (values.Length > 1) ? (values[1] as Brush) : null;
			if (brush != null)
			{
				return brush;
			}
			return Convert(value, targetType, parameter, culture);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return (from t in targetTypes
			select DependencyProperty.UnsetValue).ToArray();
		}
	}
}
