using ControlzEx.Standard;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ControlzEx
{
	public static class ToolTipAssist
	{
		public static readonly DependencyProperty AutoMoveProperty = DependencyProperty.RegisterAttached("AutoMove", typeof(bool), typeof(ToolTipAssist), new FrameworkPropertyMetadata(false, AutoMovePropertyChangedCallback));

		public static readonly DependencyProperty AutoMoveHorizontalOffsetProperty = DependencyProperty.RegisterAttached("AutoMoveHorizontalOffset", typeof(double), typeof(ToolTipAssist), new FrameworkPropertyMetadata(16.0));

		public static readonly DependencyProperty AutoMoveVerticalOffsetProperty = DependencyProperty.RegisterAttached("AutoMoveVerticalOffset", typeof(double), typeof(ToolTipAssist), new FrameworkPropertyMetadata(16.0));

		[AttachedPropertyBrowsableForType(typeof(ToolTip))]
		public static bool GetAutoMove(ToolTip element)
		{
			return (bool)element.GetValue(AutoMoveProperty);
		}

		public static void SetAutoMove(ToolTip element, bool value)
		{
			element.SetValue(AutoMoveProperty, value);
		}

		[AttachedPropertyBrowsableForType(typeof(ToolTip))]
		public static double GetAutoMoveHorizontalOffset(ToolTip element)
		{
			return (double)element.GetValue(AutoMoveHorizontalOffsetProperty);
		}

		public static void SetAutoMoveHorizontalOffset(ToolTip element, double value)
		{
			element.SetValue(AutoMoveHorizontalOffsetProperty, value);
		}

		[AttachedPropertyBrowsableForType(typeof(ToolTip))]
		public static double GetAutoMoveVerticalOffset(ToolTip element)
		{
			return (double)element.GetValue(AutoMoveVerticalOffsetProperty);
		}

		public static void SetAutoMoveVerticalOffset(ToolTip element, double value)
		{
			element.SetValue(AutoMoveVerticalOffsetProperty, value);
		}

		private static void AutoMovePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
		{
			ToolTip toolTip = (ToolTip)dependencyObject;
			if (eventArgs.OldValue != eventArgs.NewValue && eventArgs.NewValue != null)
			{
				if ((bool)eventArgs.NewValue)
				{
					toolTip.Opened += ToolTip_Opened;
					toolTip.Closed += ToolTip_Closed;
				}
				else
				{
					toolTip.Opened -= ToolTip_Opened;
					toolTip.Closed -= ToolTip_Closed;
				}
			}
		}

		private static void ToolTip_Opened(object sender, RoutedEventArgs e)
		{
			ToolTip toolTip = (ToolTip)sender;
			FrameworkElement frameworkElement = toolTip.PlacementTarget as FrameworkElement;
			if (frameworkElement != null)
			{
				MoveToolTip(frameworkElement, toolTip);
				frameworkElement.MouseMove += ToolTipTargetPreviewMouseMove;
			}
		}

		private static void ToolTip_Closed(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = ((ToolTip)sender).PlacementTarget as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.MouseMove -= ToolTipTargetPreviewMouseMove;
			}
		}

		private static void ToolTipTargetPreviewMouseMove(object sender, MouseEventArgs e)
		{
			ToolTip toolTip = (sender as FrameworkElement)?.ToolTip as ToolTip;
			MoveToolTip(sender as IInputElement, toolTip);
		}

		private static void MoveToolTip(IInputElement target, ToolTip toolTip)
		{
			if (toolTip != null && target != null && toolTip.PlacementTarget != null)
			{
				toolTip.Placement = PlacementMode.Relative;
				double autoMoveHorizontalOffset = GetAutoMoveHorizontalOffset(toolTip);
				double autoMoveVerticalOffset = GetAutoMoveVerticalOffset(toolTip);
				ControlzEx.Standard.DpiScale dpi = DpiHelper.GetDpi(toolTip);
				double num = DpiHelper.TransformToDeviceX(toolTip.PlacementTarget, autoMoveHorizontalOffset, dpi.DpiScaleX);
				double num2 = DpiHelper.TransformToDeviceY(toolTip.PlacementTarget, autoMoveVerticalOffset, dpi.DpiScaleY);
				Point position = Mouse.GetPosition(toolTip.PlacementTarget);
				double num3 = position.X + num;
				double num4 = position.Y + num2;
				Point point = toolTip.PlacementTarget.PointToScreen(new Point(0.0, 0.0));
				MONITORINFO mONITORINFO = null;
				try
				{
					mONITORINFO = MonitorHelper.GetMonitorInfoFromPoint();
				}
				catch (UnauthorizedAccessException)
				{
				}
				if (mONITORINFO != null)
				{
					int num5 = Math.Abs(mONITORINFO.rcWork.Width);
					int num6 = Math.Abs(mONITORINFO.rcWork.Height);
					if (point.X < 0.0)
					{
						point.X = (double)(-mONITORINFO.rcWork.Left) + point.X;
					}
					if (point.Y < 0.0)
					{
						point.Y = (double)(-mONITORINFO.rcWork.Top) + point.Y;
					}
					int num7 = (int)point.X % num5;
					int num8 = (int)point.Y % num6;
					double num9 = DpiHelper.TransformToDeviceX(toolTip.RenderSize.Width, dpi.DpiScaleX);
					if ((double)num7 + num3 + num9 > (double)num5)
					{
						num3 = position.X - toolTip.RenderSize.Width - 0.5 * num;
					}
					double num10 = DpiHelper.TransformToDeviceY(toolTip.RenderSize.Height, dpi.DpiScaleY);
					if ((double)num8 + num4 + num10 > (double)num6)
					{
						num4 = position.Y - toolTip.RenderSize.Height - 0.5 * num2;
					}
					toolTip.HorizontalOffset = num3;
					toolTip.VerticalOffset = num4;
				}
			}
		}
	}
}
