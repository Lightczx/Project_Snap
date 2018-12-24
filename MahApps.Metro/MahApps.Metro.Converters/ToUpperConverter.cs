using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MahApps.Metro.Converters
{
	[MarkupExtensionReturnType(typeof(ToUpperConverter))]
	public class ToUpperConverter : MarkupConverter
	{
		private static ToUpperConverter _instance;

		static ToUpperConverter()
		{
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return _instance ?? (_instance = new ToUpperConverter());
		}

		protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (text == null)
			{
				return value;
			}
			return text.ToUpper();
		}

		protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}
