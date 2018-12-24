using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	public class ThicknessBindingConverter : IValueConverter
	{
		public ThicknessSideType IgnoreThicknessSide
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
					IgnoreThicknessSide = (ThicknessSideType)parameter;
				}
				Thickness thickness = (Thickness)value;
				switch (IgnoreThicknessSide)
				{
				case ThicknessSideType.Left:
					return new Thickness(0.0, thickness.Top, thickness.Right, thickness.Bottom);
				case ThicknessSideType.Top:
					return new Thickness(thickness.Left, 0.0, thickness.Right, thickness.Bottom);
				case ThicknessSideType.Right:
					return new Thickness(thickness.Left, thickness.Top, 0.0, thickness.Bottom);
				case ThicknessSideType.Bottom:
					return new Thickness(thickness.Left, thickness.Top, thickness.Right, 0.0);
				default:
					return thickness;
				}
			}
			return default(Thickness);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
