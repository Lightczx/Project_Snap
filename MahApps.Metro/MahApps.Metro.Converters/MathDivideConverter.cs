using System;
using System.Globalization;
using System.Windows.Markup;

namespace MahApps.Metro.Converters
{
	[MarkupExtensionReturnType(typeof(MathDivideConverter))]
	public sealed class MathDivideConverter : MarkupMultiConverter
	{
		private static MathDivideConverter _instance;

		private readonly MathConverter theMathConverter = new MathConverter
		{
			Operation = MathOperation.Divide
		};

		static MathDivideConverter()
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
			return _instance ?? (_instance = new MathDivideConverter());
		}
	}
}
