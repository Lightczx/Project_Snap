using System.Windows;

namespace MahApps.Metro.Controls
{
	public class RangeParameterChangedEventArgs : RoutedEventArgs
	{
		public RangeParameterChangeType ParameterType
		{
			get;
			private set;
		}

		public double OldValue
		{
			get;
			private set;
		}

		public double NewValue
		{
			get;
			private set;
		}

		internal RangeParameterChangedEventArgs(RangeParameterChangeType type, double _old, double _new)
		{
			ParameterType = type;
			OldValue = _old;
			NewValue = _new;
		}
	}
}
