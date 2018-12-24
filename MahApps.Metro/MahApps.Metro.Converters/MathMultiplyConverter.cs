using System;
using System.Globalization;
using System.Windows.Markup;

namespace MahApps.Metro.Converters
{
	[MarkupExtensionReturnType(typeof(MathMultiplyConverter))]
	public sealed class MathMultiplyConverter : MarkupMultiConverter
	{
		private static MathMultiplyConverter _instance;

		private readonly MathConverter theMathConverter = new MathConverter
		{
			Operation = MathOperation.Multiply
		};

		static MathMultiplyConverter()
		{
		}

		public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			return theMathConverter.Convert(values, targetType, parameter, culture);
		}

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return theMathConverter.Convert(value, targetType, parameter, culture);
		}

		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return theMathConverter.ConvertBack(value, targetTypes, parameter, culture);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return theMathConverter.ConvertBack(value, targetType, parameter, culture);
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return _instance ?? (_instance = new MathMultiplyConverter());
		}
	}
}
