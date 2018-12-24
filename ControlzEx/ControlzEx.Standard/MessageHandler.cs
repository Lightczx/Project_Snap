using System;

namespace ControlzEx.Standard
{
	internal delegate IntPtr MessageHandler(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled);
}
