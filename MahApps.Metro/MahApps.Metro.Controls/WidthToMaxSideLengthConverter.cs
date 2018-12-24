using System;
using System.Globalization;
using System.Windows.Data;

namespace MahApps.Metro.Controls
{
	[Obsolete("This class will be deleted in the next release.")]
	internal class WidthToMaxSideLengthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double)
			{
				double num = (double)value;
				return (num <= 20.0) ? 20.0 : num;
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
