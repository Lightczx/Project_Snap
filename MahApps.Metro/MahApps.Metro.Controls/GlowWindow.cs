using ControlzEx;
using ControlzEx.Native;
using ControlzEx.Standard;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	public class GlowWindow : Window, IComponentConnector
	{
		private readonly Func<Point, RECT, Cursor> getCursor;

		private readonly Func<Point, RECT, HT> getHitTestValue;

		private readonly Func<RECT, double> getLeft;

		private readonly Func<RECT, double> getTop;

		private readonly Func<RECT, double> getWidth;

		private readonly Func<RECT, double> getHeight;

		private const double edgeSize = 20.0;

		private const double glowSize = 6.0;

		private IntPtr handle;

		private IntPtr ownerHandle;

		private bool closing;

		private HwndSource hwndSource;

		private PropertyChangeNotifier resizeModeChangeNotifier;

		private Window _owner;

		internal GlowWindow glowWindow;

		private Glow glow;

		private bool _contentLoaded;

		public Storyboard OpacityStoryboard
		{
			get;
			set;
		}

		public bool IsGlowing
		{
			get;
			set;
		}

		public GlowWindow(Window owner, GlowDirection direction)
		{
			InitializeComponent();
			base.Owner = owner;
			_owner = owner;
			IsGlowing = true;
			base.AllowsTransparency = true;
			base.Closing += delegate(object sender, CancelEventArgs e)
			{
				e.Cancel = !closing;
			};
			base.ShowInTaskbar = false;
			glow.Visibility = Visibility.Collapsed;
			Binding binding = new Binding("GlowBrush")
			{
				Source = owner
			};
			glow.SetBinding(Glow.GlowBrushProperty, binding);
			binding = new Binding("NonActiveGlowBrush")
			{
				Source = owner
			};
			glow.SetBinding(Glow.NonActiveGlowBrushProperty, binding);
			binding = new Binding("BorderThickness")
			{
				Source = owner
			};
			glow.SetBinding(Control.BorderThicknessProperty, binding);
			glow.Direction = direction;
			switch (direction)
			{
			case GlowDirection.Left:
				glow.Orientation = Orientation.Vertical;
				glow.HorizontalAlignment = HorizontalAlignment.Right;
				getLeft = ((RECT rect) => (double)rect.Left - 6.0 + 1.0);
				getTop = ((RECT rect) => (double)(rect.Top - 2));
				getWidth = ((RECT rect) => 6.0);
				getHeight = ((RECT rect) => (double)(rect.Height + 4));
				getHitTestValue = delegate(Point p, RECT rect)
				{
					if (!new Rect(0.0, 0.0, (double)rect.Width, 20.0).Contains(p))
					{
						if (!new Rect(0.0, (double)(rect.Height + 4) - 20.0, (double)rect.Width, 20.0).Contains(p))
						{
							return HT.LEFT;
						}
						return HT.BOTTOMLEFT;
					}
					return HT.TOPLEFT;
				};
				getCursor = delegate(Point p, RECT rect)
				{
					if (!new Rect(0.0, 0.0, (double)rect.Width, 20.0).Contains(p))
					{
						if (!new Rect(0.0, (double)(rect.Height + 4) - 20.0, (double)rect.Width, 20.0).Contains(p))
						{
							return Cursors.SizeWE;
						}
						return Cursors.SizeNESW;
					}
					return Cursors.SizeNWSE;
				};
				break;
			case GlowDirection.Right:
				glow.Orientation = Orientation.Vertical;
				glow.HorizontalAlignment = HorizontalAlignment.Left;
				getLeft = ((RECT rect) => (double)(rect.Right - 1));
				getTop = ((RECT rect) => (double)(rect.Top - 2));
				getWidth = ((RECT rect) => 6.0);
				getHeight = ((RECT rect) => (double)(rect.Height + 4));
				getHitTestValue = delegate(Point p, RECT rect)
				{
					if (!new Rect(0.0, 0.0, (double)rect.Width, 20.0).Contains(p))
					{
						if (!new Rect(0.0, (double)(rect.Height + 4) - 20.0, (double)rect.Width, 20.0).Contains(p))
						{
							return HT.RIGHT;
						}
						return HT.BOTTOMRIGHT;
					}
					return HT.TOPRIGHT;
				};
				getCursor = delegate(Point p, RECT rect)
				{
					if (!new Rect(0.0, 0.0, (double)rect.Width, 20.0).Contains(p))
					{
						if (!new Rect(0.0, (double)(rect.Height + 4) - 20.0, (double)rect.Width, 20.0).Contains(p))
						{
							return Cursors.SizeWE;
						}
						return Cursors.SizeNWSE;
					}
					return Cursors.SizeNESW;
				};
				break;
			case GlowDirection.Top:
				base.PreviewMouseDoubleClick += delegate
				{
					if (ownerHandle != IntPtr.Zero)
					{
						NativeMethods.SendMessage(ownerHandle, WM.NCLBUTTONDBLCLK, (IntPtr)12, IntPtr.Zero);
					}
				};
				glow.Orientation = Orientation.Horizontal;
				glow.VerticalAlignment = VerticalAlignment.Bottom;
				getLeft = ((RECT rect) => (double)(rect.Left - 2));
				getTop = ((RECT rect) => (double)rect.Top - 6.0 + 1.0);
				getWidth = ((RECT rect) => (double)(rect.Width + 4));
				getHeight = ((RECT rect) => 6.0);
				getHitTestValue = delegate(Point p, RECT rect)
				{
					if (!new Rect(0.0, 0.0, 14.0, (double)rect.Height).Contains(p))
					{
						if (!new Rect((double)(rect.Width + 4) - 20.0 + 6.0, 0.0, 14.0, (double)rect.Height).Contains(p))
						{
							return HT.TOP;
						}
						return HT.TOPRIGHT;
					}
					return HT.TOPLEFT;
				};
				getCursor = delegate(Point p, RECT rect)
				{
					if (!new Rect(0.0, 0.0, 14.0, (double)rect.Height).Contains(p))
					{
						if (!new Rect((double)(rect.Width + 4) - 20.0 + 6.0, 0.0, 14.0, (double)rect.Height).Contains(p))
						{
							return Cursors.SizeNS;
						}
						return Cursors.SizeNESW;
					}
					return Cursors.SizeNWSE;
				};
				break;
			case GlowDirection.Bottom:
				base.PreviewMouseDoubleClick += delegate
				{
					if (ownerHandle != IntPtr.Zero)
					{
						NativeMethods.SendMessage(ownerHandle, WM.NCLBUTTONDBLCLK, (IntPtr)15, IntPtr.Zero);
					}
				};
				glow.Orientation = Orientation.Horizontal;
				glow.VerticalAlignment = VerticalAlignment.Top;
				getLeft = ((RECT rect) => (double)(rect.Left - 2));
				getTop = ((RECT rect) => (double)(rect.Bottom - 1));
				getWidth = ((RECT rect) => (double)(rect.Width + 4));
				getHeight = ((RECT rect) => 6.0);
				getHitTestValue = delegate(Point p, RECT rect)
				{
					if (!new Rect(0.0, 0.0, 14.0, (double)rect.Height).Contains(p))
					{
						if (!new Rect((double)(rect.Width + 4) - 20.0 + 6.0, 0.0, 14.0, (double)rect.Height).Contains(p))
						{
							return HT.BOTTOM;
						}
						return HT.BOTTOMRIGHT;
					}
					return HT.BOTTOMLEFT;
				};
				getCursor = delegate(Point p, RECT rect)
				{
					if (!new Rect(0.0, 0.0, 14.0, (double)rect.Height).Contains(p))
					{
						if (!new Rect((double)(rect.Width + 4) - 20.0 + 6.0, 0.0, 14.0, (double)rect.Height).Contains(p))
						{
							return Cursors.SizeNS;
						}
						return Cursors.SizeNWSE;
					}
					return Cursors.SizeNESW;
				};
				break;
			}
			owner.ContentRendered += delegate
			{
				glow.Visibility = Visibility.Visible;
			};
			owner.Activated += delegate
			{
				Update();
				glow.IsGlow = true;
			};
			owner.Deactivated += delegate
			{
				glow.IsGlow = false;
			};
			owner.StateChanged += delegate
			{
				Update();
			};
			owner.IsVisibleChanged += delegate
			{
				Update();
			};
			owner.Closed += delegate
			{
				closing = true;
				Close();
			};
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			OpacityStoryboard = (TryFindResource("OpacityStoryboard") as Storyboard);
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			hwndSource = (HwndSource)PresentationSource.FromVisual(this);
			if (hwndSource != null)
			{
				WS windowStyle = NativeMethods.GetWindowStyle(hwndSource.Handle);
				WS_EX windowStyleEx = NativeMethods.GetWindowStyleEx(hwndSource.Handle);
				windowStyle = (WS)((int)windowStyle | -2147483648);
				windowStyleEx = (WS_EX)((int)windowStyleEx & -262145);
				windowStyleEx |= WS_EX.TOOLWINDOW;
				if (_owner.ResizeMode == ResizeMode.NoResize || _owner.ResizeMode == ResizeMode.CanMinimize)
				{
					windowStyleEx |= WS_EX.TRANSPARENT;
				}
				NativeMethods.SetWindowStyle(hwndSource.Handle, windowStyle);
				NativeMethods.SetWindowStyleEx(hwndSource.Handle, windowStyleEx);
				hwndSource.AddHook(this.WndProc);
				handle = hwndSource.Handle;
				ownerHandle = new WindowInteropHelper(_owner).Handle;
				resizeModeChangeNotifier = new PropertyChangeNotifier(_owner, Window.ResizeModeProperty);
				resizeModeChangeNotifier.ValueChanged += ResizeModeChanged;
			}
		}

		private void ResizeModeChanged(object sender, EventArgs e)
		{
			WS_EX windowStyleEx = NativeMethods.GetWindowStyleEx(hwndSource.Handle);
			NativeMethods.SetWindowStyleEx(dwNewLong: (_owner.ResizeMode != 0 && _owner.ResizeMode != ResizeMode.CanMinimize) ? (windowStyleEx ^ WS_EX.TRANSPARENT) : (windowStyleEx | WS_EX.TRANSPARENT), hWnd: hwndSource.Handle);
		}

		public void Update()
		{
			if (!closing)
			{
				RECT lpRect;
				if (_owner.Visibility == Visibility.Hidden)
				{
					this.Invoke(() => glow.Visibility = Visibility.Collapsed);
					this.Invoke(() => base.Visibility = Visibility.Collapsed);
					if (IsGlowing && ownerHandle != IntPtr.Zero && UnsafeNativeMethods.GetWindowRect(ownerHandle, out lpRect))
					{
						UpdateCore(lpRect);
					}
				}
				else if (_owner.WindowState == WindowState.Normal)
				{
					this.Invoke(() => glow.Visibility = ((!IsGlowing) ? Visibility.Collapsed : Visibility.Visible));
					this.Invoke(() => base.Visibility = ((!IsGlowing) ? Visibility.Collapsed : Visibility.Visible));
					if (IsGlowing && ownerHandle != IntPtr.Zero && UnsafeNativeMethods.GetWindowRect(ownerHandle, out lpRect))
					{
						UpdateCore(lpRect);
					}
				}
				else
				{
					this.Invoke(() => glow.Visibility = Visibility.Collapsed);
					this.Invoke(() => base.Visibility = Visibility.Collapsed);
				}
			}
		}

		internal bool CanUpdateCore()
		{
			if (ownerHandle != IntPtr.Zero)
			{
				return handle != IntPtr.Zero;
			}
			return false;
		}

		internal void UpdateCore(RECT rect)
		{
			NativeMethods.SetWindowPos(handle, ownerHandle, (int)getLeft(rect), (int)getTop(rect), (int)getWidth(rect), (int)getHeight(rect), SWP.NOACTIVATE | SWP.NOZORDER);
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			RECT lpRect;
			switch (msg)
			{
			case 24:
				if ((int)lParam == 3 && base.Visibility != 0)
				{
					handled = true;
				}
				break;
			case 33:
				handled = true;
				if (ownerHandle != IntPtr.Zero)
				{
					NativeMethods.SendMessage(ownerHandle, WM.ACTIVATE, wParam, lParam);
				}
				return new IntPtr(3);
			case 513:
				if (ownerHandle != IntPtr.Zero && UnsafeNativeMethods.GetWindowRect(ownerHandle, out lpRect) && WinApiHelper.TryGetRelativeMousePosition(handle, out Point point2))
				{
					NativeMethods.PostMessage(ownerHandle, WM.NCLBUTTONDOWN, (IntPtr)(int)getHitTestValue(point2, lpRect), IntPtr.Zero);
				}
				break;
			case 132:
			{
				Cursor cursor = null;
				Point point;
				if (_owner.ResizeMode == ResizeMode.NoResize || _owner.ResizeMode == ResizeMode.CanMinimize)
				{
					cursor = _owner.Cursor;
				}
				else if (ownerHandle != IntPtr.Zero && UnsafeNativeMethods.GetWindowRect(ownerHandle, out lpRect) && WinApiHelper.TryGetRelativeMousePosition(handle, out point))
				{
					cursor = getCursor(point, lpRect);
				}
				if (cursor != null && cursor != base.Cursor)
				{
					base.Cursor = cursor;
				}
				break;
			}
			}
			return IntPtr.Zero;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/MahApps.Metro;component/controls/glowwindow.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				glowWindow = (GlowWindow)target;
				break;
			case 2:
				glow = (Glow)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
