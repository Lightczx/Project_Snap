using System.Windows;

namespace MahApps.Metro.Controls
{
	public class TimePicker : TimePickerBase
	{
		static TimePicker()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(typeof(TimePicker)));
		}

		public TimePicker()
		{
			base.IsDatePickerVisible = false;
		}
	}
}
