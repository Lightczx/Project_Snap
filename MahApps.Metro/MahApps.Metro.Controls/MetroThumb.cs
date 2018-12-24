using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public class MetroThumb : Thumb, IMetroThumb, IInputElement
	{
		private TouchDevice currentDevice;

		protected override void OnPreviewTouchDown(TouchEventArgs e)
		{
			ReleaseCurrentDevice();
			CaptureCurrentDevice(e);
		}

		protected override void OnPreviewTouchUp(TouchEventArgs e)
		{
			ReleaseCurrentDevice();
		}

		protected override void OnLostTouchCapture(TouchEventArgs e)
		{
			if (currentDevice != null)
			{
				CaptureCurrentDevice(e);
			}
		}

		private void ReleaseCurrentDevice()
		{
			if (currentDevice != null)
			{
				TouchDevice touchDevice = currentDevice;
				currentDevice = null;
				ReleaseTouchCapture(touchDevice);
			}
		}

		private void CaptureCurrentDevice(TouchEventArgs e)
		{
			if (CaptureTouch(e.TouchDevice))
			{
				currentDevice = e.TouchDevice;
			}
		}

		void IMetroThumb.add_DragStarted(DragStartedEventHandler value)
		{
			base.DragStarted += value;
		}

		void IMetroThumb.remove_DragStarted(DragStartedEventHandler value)
		{
			base.DragStarted -= value;
		}

		void IMetroThumb.add_DragDelta(DragDeltaEventHandler value)
		{
			base.DragDelta += value;
		}

		void IMetroThumb.remove_DragDelta(DragDeltaEventHandler value)
		{
			base.DragDelta -= value;
		}

		void IMetroThumb.add_DragCompleted(DragCompletedEventHandler value)
		{
			base.DragCompleted += value;
		}

		void IMetroThumb.remove_DragCompleted(DragCompletedEventHandler value)
		{
			base.DragCompleted -= value;
		}

		void IMetroThumb.add_MouseDoubleClick(MouseButtonEventHandler value)
		{
			base.MouseDoubleClick += value;
		}

		void IMetroThumb.remove_MouseDoubleClick(MouseButtonEventHandler value)
		{
			base.MouseDoubleClick -= value;
		}
	}
}
