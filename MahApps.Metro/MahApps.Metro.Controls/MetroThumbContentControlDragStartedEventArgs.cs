using System.Windows.Controls.Primitives;

namespace MahApps.Metro.Controls
{
	public class MetroThumbContentControlDragStartedEventArgs : DragStartedEventArgs
	{
		public MetroThumbContentControlDragStartedEventArgs(double horizontalOffset, double verticalOffset)
			: base(horizontalOffset, verticalOffset)
		{
			base.RoutedEvent = MetroThumbContentControl.DragStartedEvent;
		}
	}
}
