using System;
using System.Globalization;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	public sealed class IsNullConverter : IValueConverter
	{
		private static IsNullConverter _instance;

		public static IsNullConverter Instance => _instance ?? (_instance = new IsNullConverter());

		static IsNullConverter()
		{
		}

		private IsNullConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}
