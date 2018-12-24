using ControlzEx.Native;
using ControlzEx.Standard;
using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Threading;

namespace MahApps.Metro.Behaviours
{
	public class GlowWindowBehavior : Behavior<Window>
	{
		private static readonly TimeSpan GlowTimerDelay = TimeSpan.FromMilliseconds(200.0);

		private GlowWindow left;

		private GlowWindow right;

		private GlowWindow top;

		private GlowWindow bottom;

		private DispatcherTimer makeGlowVisibleTimer;

		private IntPtr handle;

		private HwndSource hwndSource;

		private WINDOWPOS prevWindowPos;

		private bool IsGlowDisabled
		{
			get
			{
				MetroWindow metroWindow = base.get_AssociatedObject() as MetroWindow;
				if (metroWindow != null)
				{
					return metroWindow.GlowBrush == null;
				}
				return false;
			}
		}

		private bool IsWindowTransitionsEnabled => (base.get_AssociatedObject() as MetroWindow)?.WindowTransitionsEnabled ?? false;

		protected override void OnAttached()
		{
			this.OnAttached();
			base.get_AssociatedObject().SourceInitialized += delegate
			{
				handle = new WindowInteropHelper(base.get_AssociatedObject()).Handle;
				hwndSource = HwndSource.FromHwnd(handle);
				hwndSource?.AddHook(this.AssociatedObjectWindowProc);
			};
			base.get_AssociatedObject().Loaded += AssociatedObjectOnLoaded;
			base.get_AssociatedObject().Unloaded += AssociatedObjectUnloaded;
		}

		private void AssociatedObjectStateChanged(object sender, EventArgs e)
		{
			makeGlowVisibleTimer?.Stop();
			if (base.get_AssociatedObject().WindowState == WindowState.Normal)
			{
				bool flag = (base.get_AssociatedObject() as MetroWindow)?.IgnoreTaskbarOnMaximize ?? false;
				if (makeGlowVisibleTimer != null && SystemParameters.MinimizeAnimation && !flag)
				{
					makeGlowVisibleTimer.Start();
				}
				else
				{
					RestoreGlow();
				}
			}
			else
			{
				HideGlow();
			}
		}

		private void AssociatedObjectUnloaded(object sender, RoutedEventArgs e)
		{
			if (makeGlowVisibleTimer != null)
			{
				makeGlowVisibleTimer.Stop();
				makeGlowVisibleTimer.Tick -= GlowVisibleTimerOnTick;
				makeGlowVisibleTimer = null;
			}
		}

		private void GlowVisibleTimerOnTick(object sender, EventArgs e)
		{
			makeGlowVisibleTimer?.Stop();
			RestoreGlow();
		}

		private void RestoreGlow()
		{
			if (left != null)
			{
				left.IsGlowing = true;
			}
			if (top != null)
			{
				top.IsGlowing = true;
			}
			if (right != null)
			{
				right.IsGlowing = true;
			}
			if (bottom != null)
			{
				bottom.IsGlowing = true;
			}
			Update();
		}

		private void HideGlow()
		{
			if (left != null)
			{
				left.IsGlowing = false;
			}
			if (top != null)
			{
				top.IsGlowing = false;
			}
			if (right != null)
			{
				right.IsGlowing = false;
			}
			if (bottom != null)
			{
				bottom.IsGlowing = false;
			}
			Update();
		}

		private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			if (!IsGlowDisabled)
			{
				base.get_AssociatedObject().StateChanged -= AssociatedObjectStateChanged;
				base.get_AssociatedObject().StateChanged += AssociatedObjectStateChanged;
				if (makeGlowVisibleTimer == null)
				{
					makeGlowVisibleTimer = new DispatcherTimer
					{
						Interval = GlowTimerDelay
					};
					makeGlowVisibleTimer.Tick += GlowVisibleTimerOnTick;
				}
				left = new GlowWindow(base.get_AssociatedObject(), GlowDirection.Left);
				right = new GlowWindow(base.get_AssociatedObject(), GlowDirection.Right);
				top = new GlowWindow(base.get_AssociatedObject(), GlowDirection.Top);
				bottom = new GlowWindow(base.get_AssociatedObject(), GlowDirection.Bottom);
				Show();
				Update();
				if (!IsWindowTransitionsEnabled)
				{
					base.get_AssociatedObject().Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Action)delegate
					{
						SetOpacityTo(1.0);
					});
				}
				else
				{
					StartOpacityStoryboard();
					base.get_AssociatedObject().IsVisibleChanged += AssociatedObjectIsVisibleChanged;
					base.get_AssociatedObject().Closing += delegate(object o, CancelEventArgs args)
					{
						if (!args.Cancel)
						{
							base.get_AssociatedObject().IsVisibleChanged -= AssociatedObjectIsVisibleChanged;
						}
					};
				}
			}
		}

		private IntPtr AssociatedObjectWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (hwndSource?.RootVisual == null)
			{
				return IntPtr.Zero;
			}
			if (msg == 5)
			{
				goto IL_0071;
			}
			if ((uint)(msg - 70) > 1u)
			{
				if (msg == 532)
				{
					goto IL_0071;
				}
			}
			else
			{
				WINDOWPOS wINDOWPOS = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
				if (!wINDOWPOS.Equals(prevWindowPos))
				{
					UpdateCore();
				}
				prevWindowPos = wINDOWPOS;
			}
			goto IL_0077;
			IL_0071:
			UpdateCore();
			goto IL_0077;
			IL_0077:
			return IntPtr.Zero;
		}

		private void AssociatedObjectIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!base.get_AssociatedObject().IsVisible)
			{
				SetOpacityTo(0.0);
			}
			else
			{
				StartOpacityStoryboard();
			}
		}

		private void Update()
		{
			if (left != null && right != null && top != null && bottom != null)
			{
				left.Update();
				right.Update();
				top.Update();
				bottom.Update();
			}
		}

		private void UpdateCore()
		{
			if (left != null && right != null && top != null && bottom != null && left.CanUpdateCore() && right.CanUpdateCore() && top.CanUpdateCore() && bottom.CanUpdateCore() && handle != IntPtr.Zero && UnsafeNativeMethods.GetWindowRect(handle, out RECT lpRect))
			{
				left.UpdateCore(lpRect);
				right.UpdateCore(lpRect);
				top.UpdateCore(lpRect);
				bottom.UpdateCore(lpRect);
			}
		}

		private void SetOpacityTo(double newOpacity)
		{
			if (left != null && right != null && top != null && bottom != null)
			{
				left.Opacity = newOpacity;
				right.Opacity = newOpacity;
				top.Opacity = newOpacity;
				bottom.Opacity = newOpacity;
			}
		}

		private void StartOpacityStoryboard()
		{
			if (left != null && right != null && top != null && bottom != null && left.OpacityStoryboard != null && right.OpacityStoryboard != null && top.OpacityStoryboard != null && bottom.OpacityStoryboard != null)
			{
				left.BeginStoryboard(left.OpacityStoryboard);
				right.BeginStoryboard(right.OpacityStoryboard);
				top.BeginStoryboard(top.OpacityStoryboard);
				bottom.BeginStoryboard(bottom.OpacityStoryboard);
			}
		}

		private void Show()
		{
			left?.Show();
			right?.Show();
			top?.Show();
			bottom?.Show();
		}
	}
}
