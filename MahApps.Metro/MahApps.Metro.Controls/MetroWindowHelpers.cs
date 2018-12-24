using ControlzEx.Windows.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	internal static class MetroWindowHelpers
	{
		public static void SetIsHitTestVisibleInChromeProperty<T>(this MetroWindow window, string name, bool hitTestVisible = true) where T : class
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			IInputElement inputElement = window.GetPart<T>(name) as IInputElement;
			if (WindowChrome.GetIsHitTestVisibleInChrome(inputElement) != hitTestVisible)
			{
				WindowChrome.SetIsHitTestVisibleInChrome(inputElement, hitTestVisible);
			}
		}

		public static void SetWindowChromeResizeGripDirection(this MetroWindow window, string name, ResizeGripDirection direction)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			IInputElement inputElement = window.GetPart(name) as IInputElement;
			if (WindowChrome.GetResizeGripDirection(inputElement) != direction)
			{
				WindowChrome.SetResizeGripDirection(inputElement, direction);
			}
		}

		public static void HandleWindowCommandsForFlyouts(this MetroWindow window, IEnumerable<Flyout> flyouts, Brush resetBrush = null)
		{
			IEnumerable<Flyout> source = from x in flyouts
			where x.IsOpen
			select x;
			if (!source.Any((Flyout x) => x.Position != Position.Bottom))
			{
				if (resetBrush == null)
				{
					window.ResetAllWindowCommandsBrush();
				}
				else
				{
					window.ChangeAllWindowCommandsBrush(resetBrush);
				}
			}
			Flyout flyout = (from x in source
			where x.Position == Position.Top
			select x).OrderByDescending(Panel.GetZIndex).FirstOrDefault();
			if (flyout != null)
			{
				window.UpdateWindowCommandsForFlyout(flyout);
			}
			else
			{
				Flyout flyout2 = (from x in source
				where x.Position == Position.Left
				select x).OrderByDescending(Panel.GetZIndex).FirstOrDefault();
				if (flyout2 != null)
				{
					window.UpdateWindowCommandsForFlyout(flyout2);
				}
				Flyout flyout3 = (from x in source
				where x.Position == Position.Right
				select x).OrderByDescending(Panel.GetZIndex).FirstOrDefault();
				if (flyout3 != null)
				{
					window.UpdateWindowCommandsForFlyout(flyout3);
				}
			}
		}

		public static void ResetAllWindowCommandsBrush(this MetroWindow window)
		{
			window.ChangeAllWindowCommandsBrush(window.OverrideDefaultWindowCommandsBrush);
		}

		public static void UpdateWindowCommandsForFlyout(this MetroWindow window, Flyout flyout)
		{
			window.ChangeAllWindowCommandsBrush(flyout.Foreground, flyout.Position);
		}

		private static void InvokeActionOnWindowCommands(this MetroWindow window, Action<Control> action1, Action<Control> action2 = null, Position position = Position.Top)
		{
			if (window.LeftWindowCommandsPresenter != null && window.RightWindowCommandsPresenter != null && window.WindowButtonCommands != null)
			{
				if (position == Position.Left || position == Position.Top)
				{
					action1(window.LeftWindowCommands);
				}
				if (position == Position.Right || position == Position.Top)
				{
					action1(window.RightWindowCommands);
					if (action2 == null)
					{
						action1(window.WindowButtonCommands);
					}
					else
					{
						action2(window.WindowButtonCommands);
					}
				}
			}
		}

		private static void ChangeAllWindowCommandsBrush(this MetroWindow window, Brush brush, Position position = Position.Top)
		{
			if (brush == null)
			{
				window.InvokeActionOnWindowCommands(delegate(Control x)
				{
					x.SetValue(WindowCommands.ThemeProperty, Theme.Light);
				}, delegate(Control x)
				{
					x.SetValue(WindowButtonCommands.ThemeProperty, Theme.Light);
				}, position);
				window.InvokeActionOnWindowCommands(delegate(Control x)
				{
					x.ClearValue(Control.ForegroundProperty);
				}, null, position);
			}
			else
			{
				Color color = ((SolidColorBrush)brush).Color;
				float num = (float)(int)color.R / 255f;
				float num2 = (float)(int)color.G / 255f;
				float num3 = (float)(int)color.B / 255f;
				float num4 = num;
				float num5 = num;
				if (num2 > num4)
				{
					num4 = num2;
				}
				if (num3 > num4)
				{
					num4 = num3;
				}
				if (num2 < num5)
				{
					num5 = num2;
				}
				if (num3 < num5)
				{
					num5 = num3;
				}
				if ((double)((num4 + num5) / 2f) > 0.1)
				{
					window.InvokeActionOnWindowCommands(delegate(Control x)
					{
						x.SetValue(WindowCommands.ThemeProperty, Theme.Light);
					}, delegate(Control x)
					{
						x.SetValue(WindowButtonCommands.ThemeProperty, Theme.Light);
					}, position);
				}
				else
				{
					window.InvokeActionOnWindowCommands(delegate(Control x)
					{
						x.SetValue(WindowCommands.ThemeProperty, Theme.Dark);
					}, delegate(Control x)
					{
						x.SetValue(WindowButtonCommands.ThemeProperty, Theme.Dark);
					}, position);
				}
				window.InvokeActionOnWindowCommands(delegate(Control x)
				{
					x.SetValue(Control.ForegroundProperty, brush);
				}, null, position);
			}
		}
	}
}
