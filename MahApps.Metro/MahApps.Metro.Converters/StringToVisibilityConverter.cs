using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MahApps.Metro.Converters
{
	[ValueConversion(typeof(string), typeof(Visibility))]
	[MarkupExtensionReturnType(typeof(StringToVisibilityConverter))]
	public class StringToVisibilityConverter : MarkupExtension, IValueConverter
	{
		public Visibility FalseEquivalent
		{
			get;
			set;
		}

		public bool OppositeStringValue
		{
			get;
			set;
		}

		public StringToVisibilityConverter()
		{
			FalseEquivalent = Visibility.Collapsed;
			OppositeStringValue = false;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new StringToVisibilityConverter
			{
				FalseEquivalent = FalseEquivalent,
				OppositeStringValue = OppositeStringValue
			};
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((value == null || value is string) && targetType == typeof(Visibility))
			{
				if (OppositeStringValue)
				{
					return (!string.IsNullOrEmpty((string)value)) ? FalseEquivalent : Visibility.Visible;
				}
				return string.IsNullOrEmpty((string)value) ? FalseEquivalent : Visibility.Visible;
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility)
			{
				if (OppositeStringValue)
				{
					if ((Visibility)value != 0)
					{
						return "visible";
					}
					return string.Empty;
				}
				if ((Visibility)value != 0)
				{
					return string.Empty;
				}
				return "visible";
			}
			return value;
		}
	}
}
