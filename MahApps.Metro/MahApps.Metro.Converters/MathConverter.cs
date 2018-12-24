using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	public sealed class MathConverter : IValueConverter, IMultiValueConverter
	{
		public MathOperation Operation
		{
			get;
			set;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DoConvert(value, parameter, Operation);
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null || values.Length < 2)
			{
				return Binding.DoNothing;
			}
			return DoConvert(values[0], values[1], Operation);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return (from t in targetTypes
			select Binding.DoNothing).ToArray();
		}

		private static object DoConvert(object firstValue, object secondValue, MathOperation operation)
		{
			if (firstValue != null && secondValue != null && firstValue != DependencyProperty.UnsetValue && secondValue != DependencyProperty.UnsetValue && firstValue != DBNull.Value && secondValue != DBNull.Value)
			{
				try
				{
					double valueOrDefault = (firstValue as double?).GetValueOrDefault(System.Convert.ToDouble(firstValue, CultureInfo.InvariantCulture));
					double valueOrDefault2 = (secondValue as double?).GetValueOrDefault(System.Convert.ToDouble(secondValue, CultureInfo.InvariantCulture));
					switch (operation)
					{
					case MathOperation.Add:
						return valueOrDefault + valueOrDefault2;
					case MathOperation.Divide:
						return valueOrDefault / valueOrDefault2;
					case MathOperation.Multiply:
						return valueOrDefault * valueOrDefault2;
					case MathOperation.Subtract:
						return valueOrDefault - valueOrDefault2;
					default:
						return Binding.DoNothing;
					}
				}
				catch (Exception)
				{
					return Binding.DoNothing;
				}
			}
			return Binding.DoNothing;
		}
	}
}
