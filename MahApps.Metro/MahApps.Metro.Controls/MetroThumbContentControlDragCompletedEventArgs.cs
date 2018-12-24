using System.Windows.Controls.Primitives;

namespace MahApps.Metro.Controls
{
	public class MetroThumbContentControlDragCompletedEventArgs : DragCompletedEventArgs
	{
		public MetroThumbContentControlDragCompletedEventArgs(double horizontalOffset, double verticalOffset, bool canceled)
			: base(horizontalOffset, verticalOffset, canceled)
		{
			base.RoutedEvent = MetroThumbContentControl.DragCompletedEvent;
		}
	}
}
