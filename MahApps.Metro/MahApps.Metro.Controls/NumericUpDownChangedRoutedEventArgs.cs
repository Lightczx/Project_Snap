using System.Windows;

namespace MahApps.Metro.Controls
{
	public class NumericUpDownChangedRoutedEventArgs : RoutedEventArgs
	{
		public double Interval
		{
			get;
			set;
		}

		public NumericUpDownChangedRoutedEventArgs(RoutedEvent routedEvent, double interval)
			: base(routedEvent)
		{
			Interval = interval;
		}
	}
}
