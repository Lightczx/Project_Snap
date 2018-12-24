using System;
using System.Globalization;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	[ValueConversion(typeof(TimeSpan?), typeof(string))]
	internal class TimeSpanToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			TimeSpan? timeSpan = value as TimeSpan?;
			if (!timeSpan.HasValue)
			{
				return null;
			}
			return DateTime.MinValue.Add(timeSpan.GetValueOrDefault()).ToString(culture.DateTimeFormat.LongTimePattern, culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (DateTime.TryParseExact(value.ToString(), culture.DateTimeFormat.LongTimePattern, culture, DateTimeStyles.None, out DateTime result))
			{
				return result.TimeOfDay;
			}
			return null;
		}
	}
}
