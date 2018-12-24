using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	[ValueConversion(typeof(Thickness), typeof(double), ParameterType = typeof(ThicknessSideType))]
	public class ThicknessToDoubleConverter : IValueConverter
	{
		public ThicknessSideType TakeThicknessSide
		{
			get;
			set;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Thickness)
			{
				if (parameter is ThicknessSideType)
				{
					TakeThicknessSide = (ThicknessSideType)parameter;
				}
				Thickness thickness = (Thickness)value;
				switch (TakeThicknessSide)
				{
				case ThicknessSideType.Left:
					return thickness.Left;
				case ThicknessSideType.Top:
					return thickness.Top;
				case ThicknessSideType.Right:
					return thickness.Right;
				case ThicknessSideType.Bottom:
					return thickness.Bottom;
				default:
					return 0.0;
				}
			}
			return 0.0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
