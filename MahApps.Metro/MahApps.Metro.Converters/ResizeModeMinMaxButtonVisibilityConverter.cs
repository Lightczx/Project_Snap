using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MahApps.Metro.Converters
{
	public sealed class ResizeModeMinMaxButtonVisibilityConverter : IMultiValueConverter
	{
		private static ResizeModeMinMaxButtonVisibilityConverter _instance;

		public static ResizeModeMinMaxButtonVisibilityConverter Instance => _instance ?? (_instance = new ResizeModeMinMaxButtonVisibilityConverter());

		static ResizeModeMinMaxButtonVisibilityConverter()
		{
		}

		private ResizeModeMinMaxButtonVisibilityConverter()
		{
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			string text = parameter as string;
			if (values != null && !string.IsNullOrEmpty(text))
			{
				bool flag = values.Length != 0 && (bool)values[0];
				bool flag2 = values.Length > 1 && (bool)values[1];
				ResizeMode resizeMode = (values.Length > 2) ? ((ResizeMode)values[2]) : ResizeMode.CanResize;
				if (!(text == "CLOSE"))
				{
					switch (resizeMode)
					{
					case ResizeMode.NoResize:
						return Visibility.Collapsed;
					case ResizeMode.CanMinimize:
						if (text == "MIN")
						{
							return (flag2 || !flag) ? Visibility.Collapsed : Visibility.Visible;
						}
						return Visibility.Collapsed;
					default:
						return (flag2 || !flag) ? Visibility.Collapsed : Visibility.Visible;
					}
				}
				return (flag2 || !flag) ? Visibility.Collapsed : Visibility.Visible;
			}
			return Visibility.Visible;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return (from t in targetTypes
			select DependencyProperty.UnsetValue).ToArray();
		}
	}
}
