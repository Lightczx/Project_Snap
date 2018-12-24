using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace ControlzEx.Standard
{
	internal sealed class MessageWindow : DispatcherObject, IDisposable
	{
		private static readonly WndProc s_WndProc = _WndProc;

		private static readonly Dictionary<IntPtr, MessageWindow> s_windowLookup = new Dictionary<IntPtr, MessageWindow>();

		private WndProc _wndProcCallback;

		private string _className;

		private bool _isDisposed;

		public IntPtr Handle
		{
			get;
			private set;
		}

		public MessageWindow(CS classStyle, WS style, WS_EX exStyle, Rect location, string name, WndProc callback)
		{
			_wndProcCallback = callback;
			_className = "MessageWindowClass+" + Guid.NewGuid().ToString();
			WNDCLASSEX lpwcx = new WNDCLASSEX
			{
				cbSize = Marshal.SizeOf(typeof(WNDCLASSEX)),
				style = classStyle,
				lpfnWndProc = s_WndProc,
				hInstance = NativeMethods.GetModuleHandle(null),
				hbrBackground = NativeMethods.GetStockObject(StockObject.NULL_BRUSH),
				lpszMenuName = "",
				lpszClassName = _className
			};
			NativeMethods.RegisterClassEx(ref lpwcx);
			GCHandle value = default(GCHandle);
			try
			{
				value = GCHandle.Alloc(this);
				IntPtr lpParam = (IntPtr)value;
				Handle = NativeMethods.CreateWindowEx(exStyle, _className, name, style, (int)location.X, (int)location.Y, (int)location.Width, (int)location.Height, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, lpParam);
			}
			finally
			{
				value.Free();
			}
		}

		~MessageWindow()
		{
			_Dispose(disposing: false, isHwndBeingDestroyed: false);
		}

		public void Dispose()
		{
			_Dispose(disposing: true, isHwndBeingDestroyed: false);
			GC.SuppressFinalize(this);
		}

		private void _Dispose(bool disposing, bool isHwndBeingDestroyed)
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				IntPtr hwnd = Handle;
				string className = _className;
				if (isHwndBeingDestroyed)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (DispatcherOperationCallback)((object arg) => _DestroyWindow(IntPtr.Zero, className)));
				}
				else if (Handle != IntPtr.Zero)
				{
					if (CheckAccess())
					{
						_DestroyWindow(hwnd, className);
					}
					else
					{
						base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (DispatcherOperationCallback)((object arg) => _DestroyWindow(hwnd, className)));
					}
				}
				s_windowLookup.Remove(hwnd);
				_className = null;
				Handle = IntPtr.Zero;
			}
		}

		private static IntPtr _WndProc(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam)
		{
			IntPtr zero = IntPtr.Zero;
			MessageWindow value = null;
			if (msg == WM.CREATE)
			{
				value = (MessageWindow)GCHandle.FromIntPtr(((CREATESTRUCT)Marshal.PtrToStructure(lParam, typeof(CREATESTRUCT))).lpCreateParams).Target;
				s_windowLookup.Add(hwnd, value);
			}
			else if (!s_windowLookup.TryGetValue(hwnd, out value))
			{
				return NativeMethods.DefWindowProc(hwnd, msg, wParam, lParam);
			}
			zero = (value._wndProcCallback?.Invoke(hwnd, msg, wParam, lParam) ?? NativeMethods.DefWindowProc(hwnd, msg, wParam, lParam));
			if (msg == WM.NCDESTROY)
			{
				value._Dispose(disposing: true, isHwndBeingDestroyed: true);
				GC.SuppressFinalize(value);
			}
			return zero;
		}

		private static object _DestroyWindow(IntPtr hwnd, string className)
		{
			Utility.SafeDestroyWindow(ref hwnd);
			NativeMethods.UnregisterClass(className, NativeMethods.GetModuleHandle(null));
			return null;
		}
	}
}
