using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;

namespace ControlzEx
{
	public class PopupEx : Popup
	{
		[Flags]
		internal enum SWP
		{
			ASYNCWINDOWPOS = 0x4000,
			DEFERERASE = 0x2000,
			DRAWFRAME = 0x20,
			FRAMECHANGED = 0x20,
			HIDEWINDOW = 0x80,
			NOACTIVATE = 0x10,
			NOCOPYBITS = 0x100,
			NOMOVE = 0x2,
			NOOWNERZORDER = 0x200,
			NOREDRAW = 0x8,
			NOREPOSITION = 0x200,
			NOSENDCHANGING = 0x400,
			NOSIZE = 0x1,
			NOZORDER = 0x4,
			SHOWWINDOW = 0x40,
			TOPMOST = 0x61B
		}

		internal struct POINT
		{
			public int x;

			public int y;
		}

		internal struct SIZE
		{
			public int cx;

			public int cy;
		}

		internal struct RECT
		{
			private int _left;

			private int _top;

			private int _right;

			private int _bottom;

			public int Left
			{
				get
				{
					return _left;
				}
				set
				{
					_left = value;
				}
			}

			public int Right
			{
				get
				{
					return _right;
				}
				set
				{
					_right = value;
				}
			}

			public int Top
			{
				get
				{
					return _top;
				}
				set
				{
					_top = value;
				}
			}

			public int Bottom
			{
				get
				{
					return _bottom;
				}
				set
				{
					_bottom = value;
				}
			}

			public int Width => _right - _left;

			public int Height => _bottom - _top;

			public POINT Position
			{
				get
				{
					POINT result = default(POINT);
					result.x = _left;
					result.y = _top;
					return result;
				}
			}

			public SIZE Size
			{
				get
				{
					SIZE result = default(SIZE);
					result.cx = Width;
					result.cy = Height;
					return result;
				}
			}

			public void Offset(int dx, int dy)
			{
				_left += dx;
				_top += dy;
				_right += dx;
				_bottom += dy;
			}

			public static RECT Union(RECT rect1, RECT rect2)
			{
				RECT result = default(RECT);
				result.Left = Math.Min(rect1.Left, rect2.Left);
				result.Top = Math.Min(rect1.Top, rect2.Top);
				result.Right = Math.Max(rect1.Right, rect2.Right);
				result.Bottom = Math.Max(rect1.Bottom, rect2.Bottom);
				return result;
			}

			public override bool Equals(object obj)
			{
				try
				{
					RECT rECT = (RECT)obj;
					return rECT._bottom == _bottom && rECT._left == _left && rECT._right == _right && rECT._top == _top;
				}
				catch (InvalidCastException)
				{
					return false;
				}
			}

			public override int GetHashCode()
			{
				return ((_left << 16) | LOWORD(_right)) ^ ((_top << 16) | LOWORD(_bottom));
			}
		}

		public static readonly DependencyProperty CloseOnMouseLeftButtonDownProperty = DependencyProperty.Register("CloseOnMouseLeftButtonDown", typeof(bool), typeof(PopupEx), new PropertyMetadata(false));

		private Window hostWindow;

		private bool? appliedTopMost;

		private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

		private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

		private static readonly IntPtr HWND_TOP = new IntPtr(0);

		private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

		public bool CloseOnMouseLeftButtonDown
		{
			get
			{
				return (bool)GetValue(CloseOnMouseLeftButtonDownProperty);
			}
			set
			{
				SetValue(CloseOnMouseLeftButtonDownProperty, value);
			}
		}

		public PopupEx()
		{
			base.Loaded += PopupEx_Loaded;
			base.Opened += PopupEx_Opened;
		}

		public void RefreshPosition()
		{
			double horizontalOffset = base.HorizontalOffset;
			SetCurrentValue(Popup.HorizontalOffsetProperty, horizontalOffset + 1.0);
			SetCurrentValue(Popup.HorizontalOffsetProperty, horizontalOffset);
		}

		private void PopupEx_Loaded(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = base.PlacementTarget as FrameworkElement;
			if (frameworkElement != null)
			{
				hostWindow = Window.GetWindow(frameworkElement);
				if (hostWindow != null)
				{
					hostWindow.LocationChanged -= hostWindow_SizeOrLocationChanged;
					hostWindow.LocationChanged += hostWindow_SizeOrLocationChanged;
					hostWindow.SizeChanged -= hostWindow_SizeOrLocationChanged;
					hostWindow.SizeChanged += hostWindow_SizeOrLocationChanged;
					frameworkElement.SizeChanged -= hostWindow_SizeOrLocationChanged;
					frameworkElement.SizeChanged += hostWindow_SizeOrLocationChanged;
					hostWindow.StateChanged -= hostWindow_StateChanged;
					hostWindow.StateChanged += hostWindow_StateChanged;
					hostWindow.Activated -= hostWindow_Activated;
					hostWindow.Activated += hostWindow_Activated;
					hostWindow.Deactivated -= hostWindow_Deactivated;
					hostWindow.Deactivated += hostWindow_Deactivated;
					base.Unloaded -= PopupEx_Unloaded;
					base.Unloaded += PopupEx_Unloaded;
				}
			}
		}

		private void PopupEx_Opened(object sender, EventArgs e)
		{
			SetTopmostState(isTop: true);
		}

		private void hostWindow_Activated(object sender, EventArgs e)
		{
			SetTopmostState(isTop: true);
		}

		private void hostWindow_Deactivated(object sender, EventArgs e)
		{
			SetTopmostState(isTop: false);
		}

		private void PopupEx_Unloaded(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = base.PlacementTarget as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.SizeChanged -= hostWindow_SizeOrLocationChanged;
			}
			if (hostWindow != null)
			{
				hostWindow.LocationChanged -= hostWindow_SizeOrLocationChanged;
				hostWindow.SizeChanged -= hostWindow_SizeOrLocationChanged;
				hostWindow.StateChanged -= hostWindow_StateChanged;
				hostWindow.Activated -= hostWindow_Activated;
				hostWindow.Deactivated -= hostWindow_Deactivated;
			}
			base.Unloaded -= PopupEx_Unloaded;
			base.Opened -= PopupEx_Opened;
			hostWindow = null;
		}

		private void hostWindow_StateChanged(object sender, EventArgs e)
		{
			if (hostWindow != null && hostWindow.WindowState != WindowState.Minimized)
			{
				FrameworkElement frameworkElement = base.PlacementTarget as FrameworkElement;
				AdornedElementPlaceholder adornedElementPlaceholder = (frameworkElement != null) ? (frameworkElement.DataContext as AdornedElementPlaceholder) : null;
				if (adornedElementPlaceholder != null && adornedElementPlaceholder.AdornedElement != null)
				{
					base.PopupAnimation = PopupAnimation.None;
					base.IsOpen = false;
					object value = adornedElementPlaceholder.AdornedElement.GetValue(Validation.ErrorTemplateProperty);
					adornedElementPlaceholder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, null);
					adornedElementPlaceholder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, value);
				}
			}
		}

		private void hostWindow_SizeOrLocationChanged(object sender, EventArgs e)
		{
			RefreshPosition();
		}

		private void SetTopmostState(bool isTop)
		{
			if ((!appliedTopMost.HasValue || appliedTopMost != isTop) && base.Child != null)
			{
				HwndSource hwndSource = PresentationSource.FromVisual(base.Child) as HwndSource;
				if (hwndSource != null)
				{
					IntPtr handle = hwndSource.Handle;
					if (GetWindowRect(handle, out RECT lpRect))
					{
						int left = lpRect.Left;
						int top = lpRect.Top;
						int width = lpRect.Width;
						int height = lpRect.Height;
						if (isTop)
						{
							SetWindowPos(handle, HWND_TOPMOST, left, top, width, height, SWP.TOPMOST);
						}
						else
						{
							SetWindowPos(handle, HWND_BOTTOM, left, top, width, height, SWP.TOPMOST);
							SetWindowPos(handle, HWND_TOP, left, top, width, height, SWP.TOPMOST);
							SetWindowPos(handle, HWND_NOTOPMOST, left, top, width, height, SWP.TOPMOST);
						}
						appliedTopMost = isTop;
					}
				}
			}
		}

		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (CloseOnMouseLeftButtonDown)
			{
				base.IsOpen = false;
			}
		}

		internal static int LOWORD(int i)
		{
			return (short)(i & 0xFFFF);
		}

		[DllImport("user32.dll", SetLastError = true)]
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags);

		[SecurityCritical]
		private static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags)
		{
			if (!_SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags))
			{
				return false;
			}
			return true;
		}
	}
}
