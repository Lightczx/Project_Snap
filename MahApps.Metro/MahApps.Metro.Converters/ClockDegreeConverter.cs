using System;
using System.Globalization;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	[ValueConversion(typeof(double), typeof(double))]
	public class ClockDegreeConverter : IValueConverter
	{
		public double TotalParts
		{
			get;
			set;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return 0;
			}
			if (value is TimeSpan)
			{
				TimeSpan timeSpan = (TimeSpan)value;
				switch ((string)parameter)
				{
				case "h":
					return 30.0 * timeSpan.TotalHours;
				case "m":
					return 6.0 * timeSpan.TotalMinutes;
				case "s":
					return 6.0 * (double)timeSpan.Seconds;
				default:
					throw new ArgumentException("must be \"h\", \"m\", or \"s", "parameter");
				}
			}
			if (value is int)
			{
				return 360.0 / TotalParts * (double)(int)value;
			}
			return 360.0 / TotalParts * (double)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new InvalidOperationException();
		}
	}
}
