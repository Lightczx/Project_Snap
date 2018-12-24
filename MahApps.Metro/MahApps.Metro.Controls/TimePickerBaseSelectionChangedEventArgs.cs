using System.Windows;

namespace MahApps.Metro.Controls
{
	public class TimePickerBaseSelectionChangedEventArgs<T> : RoutedEventArgs
	{
		public T OldValue
		{
			get;
		}

		public T NewValue
		{
			get;
		}

		public TimePickerBaseSelectionChangedEventArgs(RoutedEvent eventId, T oldValue, T newValue)
			: base(eventId)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
}
