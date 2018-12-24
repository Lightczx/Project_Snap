using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public class MetroThumbContentControl : ContentControlEx, IMetroThumb, IInputElement
	{
		private TouchDevice currentDevice;

		private Point startDragPoint;

		private Point startDragScreenPoint;

		private Point oldDragScreenPoint;

		public static readonly RoutedEvent DragStartedEvent;

		public static readonly RoutedEvent DragDeltaEvent;

		public static readonly RoutedEvent DragCompletedEvent;

		public static readonly DependencyPropertyKey IsDraggingPropertyKey;

		public static readonly DependencyProperty IsDraggingProperty;

		public bool IsDragging
		{
			get
			{
				return (bool)GetValue(IsDraggingProperty);
			}
			protected set
			{
				SetValue(IsDraggingPropertyKey, value);
			}
		}

		public event DragStartedEventHandler DragStarted
		{
			add
			{
				AddHandler(DragStartedEvent, value);
			}
			remove
			{
				RemoveHandler(DragStartedEvent, value);
			}
		}

		public event DragDeltaEventHandler DragDelta
		{
			add
			{
				AddHandler(DragDeltaEvent, value);
			}
			remove
			{
				RemoveHandler(DragDeltaEvent, value);
			}
		}

		public event DragCompletedEventHandler DragCompleted
		{
			add
			{
				AddHandler(DragCompletedEvent, value);
			}
			remove
			{
				RemoveHandler(DragCompletedEvent, value);
			}
		}

		static MetroThumbContentControl()
		{
			DragStartedEvent = EventManager.RegisterRoutedEvent("DragStarted", RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(MetroThumbContentControl));
			DragDeltaEvent = EventManager.RegisterRoutedEvent("DragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(MetroThumbContentControl));
			DragCompletedEvent = EventManager.RegisterRoutedEvent("DragCompleted", RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(MetroThumbContentControl));
			IsDraggingPropertyKey = DependencyProperty.RegisterReadOnly("IsDragging", typeof(bool), typeof(MetroThumbContentControl), new FrameworkPropertyMetadata(false));
			IsDraggingProperty = IsDraggingPropertyKey.DependencyProperty;
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroThumbContentControl), new FrameworkPropertyMetadata(typeof(MetroThumbContentControl)));
			UIElement.FocusableProperty.OverrideMetadata(typeof(MetroThumbContentControl), new FrameworkPropertyMetadata(false));
			EventManager.RegisterClassHandler(typeof(MetroThumbContentControl), Mouse.LostMouseCaptureEvent, new MouseEventHandler(OnLostMouseCapture));
		}

		public void CancelDragAction()
		{
			if (IsDragging)
			{
				if (base.IsMouseCaptured)
				{
					ReleaseMouseCapture();
				}
				ClearValue(IsDraggingPropertyKey);
				double horizontalOffset = oldDragScreenPoint.X - startDragScreenPoint.X;
				double verticalOffset = oldDragScreenPoint.Y - startDragScreenPoint.Y;
				RaiseEvent(new MetroThumbContentControlDragCompletedEventArgs(horizontalOffset, verticalOffset, canceled: true));
			}
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!IsDragging)
			{
				e.Handled = true;
				try
				{
					Focus();
					CaptureMouse();
					SetValue(IsDraggingPropertyKey, true);
					startDragPoint = e.GetPosition(this);
					oldDragScreenPoint = (startDragScreenPoint = PointToScreen(startDragPoint));
					RaiseEvent(new MetroThumbContentControlDragStartedEventArgs(startDragPoint.X, startDragPoint.Y));
				}
				catch (Exception)
				{
					CancelDragAction();
				}
			}
			base.OnMouseLeftButtonDown(e);
		}

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (base.IsMouseCaptured && IsDragging)
			{
				e.Handled = true;
				ClearValue(IsDraggingPropertyKey);
				ReleaseMouseCapture();
				Point point = PointToScreen(e.MouseDevice.GetPosition(this));
				double horizontalOffset = point.X - startDragScreenPoint.X;
				double verticalOffset = point.Y - startDragScreenPoint.Y;
				RaiseEvent(new MetroThumbContentControlDragCompletedEventArgs(horizontalOffset, verticalOffset, canceled: false));
			}
			base.OnMouseLeftButtonUp(e);
		}

		private static void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			MetroThumbContentControl metroThumbContentControl = (MetroThumbContentControl)sender;
			if (Mouse.Captured != metroThumbContentControl)
			{
				metroThumbContentControl.CancelDragAction();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (IsDragging)
			{
				if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
				{
					Point position = e.GetPosition(this);
					Point point = PointToScreen(position);
					if (point != oldDragScreenPoint)
					{
						oldDragScreenPoint = point;
						e.Handled = true;
						double horizontalChange = position.X - startDragPoint.X;
						double verticalChange = position.Y - startDragPoint.Y;
						RaiseEvent(new DragDeltaEventArgs(horizontalChange, verticalChange)
						{
							RoutedEvent = DragDeltaEvent
						});
					}
				}
				else
				{
					if (e.MouseDevice.Captured == this)
					{
						ReleaseMouseCapture();
					}
					ClearValue(IsDraggingPropertyKey);
					startDragPoint.X = 0.0;
					startDragPoint.Y = 0.0;
				}
			}
		}

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

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new MetroThumbContentControlAutomationPeer(this);
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
