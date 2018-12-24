using System;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public static class NativeMethods
	{
		public enum MapType : uint
		{
			MAPVK_VK_TO_VSC,
			MAPVK_VSC_TO_VK,
			MAPVK_VK_TO_CHAR,
			MAPVK_VSC_TO_VK_EX
		}

		[StructLayout(LayoutKind.Sequential)]
		public class CHOOSECOLOR
		{
			public int lStructSize = Marshal.SizeOf(typeof(CHOOSECOLOR));

			public IntPtr hwndOwner;

			public IntPtr hInstance = IntPtr.Zero;

			public int rgbResult;

			public IntPtr lpCustColors = IntPtr.Zero;

			public int Flags;

			public IntPtr lCustData = IntPtr.Zero;

			public IntPtr lpfnHook = IntPtr.Zero;

			public IntPtr lpTemplateName = IntPtr.Zero;
		}

		[DllImport("user32.dll", EntryPoint = "AdjustWindowRectEx", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _AdjustWindowRectEx(ref RECT lpRect, WS dwStyle, [MarshalAs(UnmanagedType.Bool)] bool bMenu, WS_EX dwExStyle);

		public static RECT AdjustWindowRectEx(RECT lpRect, WS dwStyle, bool bMenu, WS_EX dwExStyle)
		{
			if (!_AdjustWindowRectEx(ref lpRect, dwStyle, bMenu, dwExStyle))
			{
				HRESULT.ThrowLastError();
			}
			return lpRect;
		}

		[DllImport("user32.dll", EntryPoint = "AllowSetForegroundWindow", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _AllowSetForegroundWindow(int dwProcessId);

		public static void AllowSetForegroundWindow()
		{
			AllowSetForegroundWindow(-1);
		}

		public static void AllowSetForegroundWindow(int dwProcessId)
		{
			if (!_AllowSetForegroundWindow(dwProcessId))
			{
				HRESULT.ThrowLastError();
			}
		}

		[DllImport("user32.dll", EntryPoint = "ChangeWindowMessageFilter", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _ChangeWindowMessageFilter(WM message, MSGFLT dwFlag);

		[DllImport("user32.dll", EntryPoint = "ChangeWindowMessageFilterEx", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _ChangeWindowMessageFilterEx(IntPtr hwnd, WM message, MSGFLT action, [Optional] [In] [Out] ref CHANGEFILTERSTRUCT pChangeFilterStruct);

		public static HRESULT ChangeWindowMessageFilterEx(IntPtr hwnd, WM message, MSGFLT action, out MSGFLTINFO filterInfo)
		{
			filterInfo = MSGFLTINFO.NONE;
			if (!Utility.IsOSVistaOrNewer)
			{
				return HRESULT.S_FALSE;
			}
			if (!Utility.IsOSWindows7OrNewer)
			{
				if (!_ChangeWindowMessageFilter(message, action))
				{
					return (HRESULT)Win32Error.GetLastError();
				}
				return HRESULT.S_OK;
			}
			CHANGEFILTERSTRUCT cHANGEFILTERSTRUCT = default(CHANGEFILTERSTRUCT);
			cHANGEFILTERSTRUCT.cbSize = (uint)Marshal.SizeOf(typeof(CHANGEFILTERSTRUCT));
			CHANGEFILTERSTRUCT pChangeFilterStruct = cHANGEFILTERSTRUCT;
			if (!_ChangeWindowMessageFilterEx(hwnd, message, action, ref pChangeFilterStruct))
			{
				return (HRESULT)Win32Error.GetLastError();
			}
			filterInfo = pChangeFilterStruct.ExtStatus;
			return HRESULT.S_OK;
		}

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool ScreenToClient(IntPtr hWnd, ref POINT point);

		[DllImport("gdi32.dll")]
		public static extern CombineRgnResult CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, RGN fnCombineMode);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, EntryPoint = "CommandLineToArgvW")]
		private static extern IntPtr _CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string cmdLine, out int numArgs);

		public static string[] CommandLineToArgvW(string cmdLine)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				int numArgs = 0;
				intPtr = _CommandLineToArgvW(cmdLine, out numArgs);
				if (intPtr == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				string[] array = new string[numArgs];
				for (int i = 0; i < numArgs; i++)
				{
					IntPtr ptr = Marshal.ReadIntPtr(intPtr, i * Marshal.SizeOf(typeof(IntPtr)));
					array[i] = Marshal.PtrToStringUni(ptr);
				}
				return array;
			}
			finally
			{
				_LocalFree(intPtr);
			}
		}

		[DllImport("gdi32.dll", EntryPoint = "CreateDIBSection", SetLastError = true)]
		private static extern SafeHBITMAP _CreateDIBSection(SafeDC hdc, [In] ref BITMAPINFO bitmapInfo, int iUsage, out IntPtr ppvBits, IntPtr hSection, int dwOffset);

		[DllImport("gdi32.dll", EntryPoint = "CreateDIBSection", SetLastError = true)]
		private static extern SafeHBITMAP _CreateDIBSectionIntPtr(IntPtr hdc, [In] ref BITMAPINFO bitmapInfo, int iUsage, out IntPtr ppvBits, IntPtr hSection, int dwOffset);

		public static SafeHBITMAP CreateDIBSection(SafeDC hdc, ref BITMAPINFO bitmapInfo, out IntPtr ppvBits, IntPtr hSection, int dwOffset)
		{
			SafeHBITMAP safeHBITMAP = null;
			safeHBITMAP = ((hdc != null) ? _CreateDIBSection(hdc, ref bitmapInfo, 0, out ppvBits, hSection, dwOffset) : _CreateDIBSectionIntPtr(IntPtr.Zero, ref bitmapInfo, 0, out ppvBits, hSection, dwOffset));
			if (safeHBITMAP.IsInvalid)
			{
				HRESULT.ThrowLastError();
			}
			return safeHBITMAP;
		}

		[DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn", SetLastError = true)]
		private static extern IntPtr _CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

		public static IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse)
		{
			IntPtr intPtr = _CreateRoundRectRgn(nLeftRect, nTopRect, nRightRect, nBottomRect, nWidthEllipse, nHeightEllipse);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		[DllImport("gdi32.dll", EntryPoint = "CreateRectRgn", SetLastError = true)]
		private static extern IntPtr _CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

		public static IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
		{
			IntPtr intPtr = _CreateRectRgn(nLeftRect, nTopRect, nRightRect, nBottomRect);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		[DllImport("gdi32.dll", EntryPoint = "CreateRectRgnIndirect", SetLastError = true)]
		private static extern IntPtr _CreateRectRgnIndirect([In] ref RECT lprc);

		public static IntPtr CreateRectRgnIndirect(RECT lprc)
		{
			IntPtr intPtr = _CreateRectRgnIndirect(ref lprc);
			if (IntPtr.Zero == intPtr)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateSolidBrush(int crColor);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "CreateWindowExW", SetLastError = true)]
		private static extern IntPtr _CreateWindowEx(WS_EX dwExStyle, [MarshalAs(UnmanagedType.LPWStr)] string lpClassName, [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName, WS dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		public static IntPtr CreateWindowEx(WS_EX dwExStyle, string lpClassName, string lpWindowName, WS dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam)
		{
			IntPtr intPtr = _CreateWindowEx(dwExStyle, lpClassName, lpWindowName, dwStyle, x, y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
			if (IntPtr.Zero == intPtr)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "DefWindowProcW")]
		public static extern IntPtr DefWindowProc(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyIcon(IntPtr handle);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyWindow(IntPtr hwnd);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindow(IntPtr hwnd);

		[DllImport("dwmapi.dll", PreserveSig = true)]
		public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

		[DllImport("dwmapi.dll", EntryPoint = "DwmGetColorizationColor")]
		private static extern HRESULT _DwmGetColorizationColor(out uint pcrColorization, [MarshalAs(UnmanagedType.Bool)] out bool pfOpaqueBlend);

		public static bool DwmGetColorizationColor(out uint pcrColorization, out bool pfOpaqueBlend)
		{
			if (Utility.IsOSVistaOrNewer && IsThemeActive() && _DwmGetColorizationColor(out pcrColorization, out pfOpaqueBlend).Succeeded)
			{
				return true;
			}
			pcrColorization = 4278190080u;
			pfOpaqueBlend = true;
			return false;
		}

		[DllImport("dwmapi.dll", EntryPoint = "DwmGetCompositionTimingInfo")]
		private static extern HRESULT _DwmGetCompositionTimingInfo(IntPtr hwnd, ref DWM_TIMING_INFO pTimingInfo);

		public static DWM_TIMING_INFO? DwmGetCompositionTimingInfo(IntPtr hwnd)
		{
			if (!Utility.IsOSVistaOrNewer)
			{
				return null;
			}
			DWM_TIMING_INFO dWM_TIMING_INFO = default(DWM_TIMING_INFO);
			dWM_TIMING_INFO.cbSize = Marshal.SizeOf(typeof(DWM_TIMING_INFO));
			DWM_TIMING_INFO pTimingInfo = dWM_TIMING_INFO;
			HRESULT hrLeft = _DwmGetCompositionTimingInfo(hwnd, ref pTimingInfo);
			if (hrLeft == HRESULT.E_PENDING)
			{
				return null;
			}
			hrLeft.ThrowIfFailed();
			return pTimingInfo;
		}

		[DllImport("dwmapi.dll", EntryPoint = "DwmIsCompositionEnabled", PreserveSig = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _DwmIsCompositionEnabled();

		public static bool DwmIsCompositionEnabled()
		{
			if (!Utility.IsOSVistaOrNewer)
			{
				return false;
			}
			return _DwmIsCompositionEnabled();
		}

		[DllImport("dwmapi.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DwmDefWindowProc(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam, out IntPtr plResult);

		[DllImport("dwmapi.dll", EntryPoint = "DwmSetWindowAttribute")]
		private static extern void _DwmSetWindowAttribute(IntPtr hwnd, DWMWA dwAttribute, ref int pvAttribute, int cbAttribute);

		public static void DwmSetWindowAttributeFlip3DPolicy(IntPtr hwnd, DWMFLIP3D flip3dPolicy)
		{
			int pvAttribute = (int)flip3dPolicy;
			_DwmSetWindowAttribute(hwnd, DWMWA.FLIP3D_POLICY, ref pvAttribute, 4);
		}

		public static void DwmSetWindowAttributeDisallowPeek(IntPtr hwnd, bool disallowPeek)
		{
			int pvAttribute = disallowPeek ? 1 : 0;
			_DwmSetWindowAttribute(hwnd, DWMWA.DISALLOW_PEEK, ref pvAttribute, 4);
		}

		[DllImport("user32.dll", EntryPoint = "EnableMenuItem")]
		private static extern int _EnableMenuItem(IntPtr hMenu, SC uIDEnableItem, MF uEnable);

		public static MF EnableMenuItem(IntPtr hMenu, SC uIDEnableItem, MF uEnable)
		{
			return (MF)_EnableMenuItem(hMenu, uIDEnableItem, uEnable);
		}

		[DllImport("user32.dll", EntryPoint = "RemoveMenu", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

		public static void RemoveMenu(IntPtr hMenu, SC uPosition, MF uFlags)
		{
			if (!_RemoveMenu(hMenu, (uint)uPosition, (uint)uFlags))
			{
				throw new Win32Exception();
			}
		}

		[DllImport("user32.dll", EntryPoint = "DrawMenuBar", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _DrawMenuBar(IntPtr hWnd);

		public static void DrawMenuBar(IntPtr hWnd)
		{
			if (!_DrawMenuBar(hWnd))
			{
				throw new Win32Exception();
			}
		}

		[DllImport("kernel32.dll")]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FindClose(IntPtr handle);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern SafeFindHandle FindFirstFileW(string lpFileName, [In] [Out] [MarshalAs(UnmanagedType.LPStruct)] WIN32_FIND_DATAW lpFindFileData);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FindNextFileW(SafeFindHandle hndFindFile, [In] [Out] [MarshalAs(UnmanagedType.LPStruct)] WIN32_FIND_DATAW lpFindFileData);

		[DllImport("user32.dll", EntryPoint = "GetClientRect", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _GetClientRect(IntPtr hwnd, out RECT lpRect);

		public static RECT GetClientRect(IntPtr hwnd)
		{
			if (!_GetClientRect(hwnd, out RECT lpRect))
			{
				HRESULT.ThrowLastError();
			}
			return lpRect;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetCursorPos", ExactSpelling = true, SetLastError = true)]
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _GetCursorPos(out POINT lpPoint);

		[SecurityCritical]
		public static POINT GetCursorPos()
		{
			if (!_GetCursorPos(out POINT lpPoint))
			{
				HRESULT.ThrowLastError();
			}
			return lpPoint;
		}

		[SecurityCritical]
		public static bool TryGetCursorPos(out POINT pt)
		{
			bool num = _GetCursorPos(out pt);
			if (!num)
			{
				pt.X = 0;
				pt.Y = 0;
			}
			return num;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetPhysicalCursorPos", ExactSpelling = true, SetLastError = true)]
		[SecurityCritical]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _GetPhysicalCursorPos(out POINT lpPoint);

		[SecurityCritical]
		public static POINT GetPhysicalCursorPos()
		{
			if (!_GetPhysicalCursorPos(out POINT lpPoint))
			{
				HRESULT.ThrowLastError();
			}
			return lpPoint;
		}

		[SecurityCritical]
		public static bool TryGetPhysicalCursorPos(out POINT pt)
		{
			bool num = _GetPhysicalCursorPos(out pt);
			if (!num)
			{
				pt.X = 0;
				pt.Y = 0;
			}
			return num;
		}

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, EntryPoint = "GetCurrentThemeName")]
		private static extern HRESULT _GetCurrentThemeName(StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int cchMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars);

		public static void GetCurrentThemeName(out string themeFileName, out string color, out string size)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			StringBuilder stringBuilder2 = new StringBuilder(260);
			StringBuilder stringBuilder3 = new StringBuilder(260);
			_GetCurrentThemeName(stringBuilder, stringBuilder.Capacity, stringBuilder2, stringBuilder2.Capacity, stringBuilder3, stringBuilder3.Capacity).ThrowIfFailed();
			themeFileName = stringBuilder.ToString();
			color = stringBuilder2.ToString();
			size = stringBuilder3.ToString();
		}

		[DllImport("uxtheme.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsThemeActive();

		[Obsolete("Use SafeDC.GetDC instead.", true)]
		public static void GetDC()
		{
		}

		[DllImport("gdi32.dll")]
		public static extern int GetDeviceCaps(SafeDC hdc, DeviceCap nIndex);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetModuleFileName", SetLastError = true)]
		private static extern int _GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

		public static string GetModuleFileName(IntPtr hModule)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			while (true)
			{
				int num = _GetModuleFileName(hModule, stringBuilder, stringBuilder.Capacity);
				if (num == 0)
				{
					HRESULT.ThrowLastError();
				}
				if (num != stringBuilder.Capacity)
				{
					break;
				}
				stringBuilder.EnsureCapacity(stringBuilder.Capacity * 2);
			}
			return stringBuilder.ToString();
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetModuleHandleW", SetLastError = true)]
		private static extern IntPtr _GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);

		public static IntPtr GetModuleHandle(string lpModuleName)
		{
			IntPtr intPtr = _GetModuleHandle(lpModuleName);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		[DllImport("user32.dll", EntryPoint = "GetMonitorInfo", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _GetMonitorInfo(IntPtr hMonitor, [In] [Out] MONITORINFO lpmi);

		public static MONITORINFO GetMonitorInfo(IntPtr hMonitor)
		{
			MONITORINFO mONITORINFO = new MONITORINFO();
			if (!_GetMonitorInfo(hMonitor, mONITORINFO))
			{
				throw new Win32Exception();
			}
			return mONITORINFO;
		}

		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetMonitorInfoW", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _GetMonitorInfoW([In] IntPtr hMonitor, [Out] MONITORINFO lpmi);

		public static MONITORINFO GetMonitorInfoW(IntPtr hMonitor)
		{
			MONITORINFO mONITORINFO = new MONITORINFO();
			if (!_GetMonitorInfoW(hMonitor, mONITORINFO))
			{
				throw new Win32Exception();
			}
			return mONITORINFO;
		}

		public static IntPtr GetTaskBarHandleForMonitor(IntPtr monitor)
		{
			IntPtr intPtr = FindWindow("Shell_TrayWnd", null);
			IntPtr intPtr2 = MonitorFromWindow(intPtr, MonitorOptions.MONITOR_DEFAULTTONEAREST);
			if (!object.Equals(monitor, intPtr2))
			{
				intPtr = FindWindow("Shell_SecondaryTrayWnd", null);
				intPtr2 = MonitorFromWindow(intPtr, MonitorOptions.MONITOR_DEFAULTTONEAREST);
				if (!object.Equals(monitor, intPtr2))
				{
					return IntPtr.Zero;
				}
			}
			return intPtr;
		}

		[DllImport("gdi32.dll", EntryPoint = "GetStockObject", SetLastError = true)]
		private static extern IntPtr _GetStockObject(StockObject fnObject);

		public static IntPtr GetStockObject(StockObject fnObject)
		{
			return _GetStockObject(fnObject);
		}

		[DllImport("user32.dll")]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bRevert);

		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics(SM nIndex);

		[DllImport("user32.dll", EntryPoint = "GetWindowInfo", SetLastError = true)]
		private static extern bool _GetWindowInfo(IntPtr hWnd, ref WINDOWINFO pwi);

		public static WINDOWINFO GetWindowInfo(IntPtr hWnd)
		{
			WINDOWINFO wINDOWINFO = default(WINDOWINFO);
			wINDOWINFO.cbSize = Marshal.SizeOf(typeof(WINDOWINFO));
			WINDOWINFO pwi = wINDOWINFO;
			if (!_GetWindowInfo(hWnd, ref pwi))
			{
				HRESULT.ThrowLastError();
			}
			return pwi;
		}

		public static WS GetWindowStyle(IntPtr hWnd)
		{
			return (WS)(int)GetWindowLongPtr(hWnd, GWL.STYLE);
		}

		public static WS_EX GetWindowStyleEx(IntPtr hWnd)
		{
			return (WS_EX)(int)GetWindowLongPtr(hWnd, GWL.EXSTYLE);
		}

		public static WS SetWindowStyle(IntPtr hWnd, WS dwNewLong)
		{
			return (WS)(int)SetWindowLongPtr(hWnd, GWL.STYLE, (IntPtr)(int)dwNewLong);
		}

		public static WS_EX SetWindowStyleEx(IntPtr hWnd, WS_EX dwNewLong)
		{
			return (WS_EX)(int)SetWindowLongPtr(hWnd, GWL.EXSTYLE, (IntPtr)(int)dwNewLong);
		}

		public static IntPtr GetWindowLongPtr(IntPtr hwnd, GWL nIndex)
		{
			IntPtr zero = IntPtr.Zero;
			zero = ((8 != IntPtr.Size) ? GetWindowLongPtr32(hwnd, nIndex) : GetWindowLongPtr64(hwnd, nIndex));
			if (IntPtr.Zero == zero)
			{
				throw new Win32Exception();
			}
			return zero;
		}

		public static IntPtr GetClassLong(IntPtr hWnd, int nIndex)
		{
			if (IntPtr.Size == 4)
			{
				return new IntPtr(GetClassLong32(hWnd, nIndex));
			}
			return GetClassLong64(hWnd, nIndex);
		}

		[DllImport("user32.dll", EntryPoint = "GetClassLong")]
		private static extern uint GetClassLong32(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
		private static extern IntPtr GetClassLong64(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetProp", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SetProp(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string lpString, IntPtr hData);

		public static void SetProp(IntPtr hwnd, string lpString, IntPtr hData)
		{
			if (!_SetProp(hwnd, lpString, hData))
			{
				HRESULT.ThrowLastError();
			}
		}

		[DllImport("uxtheme.dll", PreserveSig = true)]
		public static extern void SetWindowThemeAttribute([In] IntPtr hwnd, [In] WINDOWTHEMEATTRIBUTETYPE eAttribute, [In] ref WTA_OPTIONS pvAttribute, [In] uint cbAttribute);

		[DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
		private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, GWL nIndex);

		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, GWL nIndex);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowPlacement(IntPtr hwnd, [In] [Out] WINDOWPLACEMENT lpwndpl);

		public static WINDOWPLACEMENT GetWindowPlacement(IntPtr hwnd)
		{
			WINDOWPLACEMENT wINDOWPLACEMENT = new WINDOWPLACEMENT();
			if (GetWindowPlacement(hwnd, wINDOWPLACEMENT))
			{
				return wINDOWPLACEMENT;
			}
			throw new Win32Exception();
		}

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPlacement(IntPtr hWnd, [In] WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll", EntryPoint = "GetWindowRect", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _GetWindowRect(IntPtr hWnd, out RECT lpRect);

		public static RECT GetWindowRect(IntPtr hwnd)
		{
			if (!_GetWindowRect(hwnd, out RECT lpRect))
			{
				HRESULT.ThrowLastError();
			}
			return lpRect;
		}

		[DllImport("gdiplus.dll")]
		public static extern Status GdipCreateBitmapFromStream(IStream stream, out IntPtr bitmap);

		[DllImport("gdiplus.dll")]
		public static extern Status GdipCreateHBITMAPFromBitmap(IntPtr bitmap, out IntPtr hbmReturn, int background);

		[DllImport("gdiplus.dll")]
		public static extern Status GdipCreateHICONFromBitmap(IntPtr bitmap, out IntPtr hbmReturn);

		[DllImport("gdiplus.dll")]
		public static extern Status GdipDisposeImage(IntPtr image);

		[DllImport("gdiplus.dll")]
		public static extern Status GdipImageForceValidation(IntPtr image);

		[DllImport("gdiplus.dll")]
		public static extern Status GdiplusStartup(out IntPtr token, StartupInput input, out StartupOutput output);

		[DllImport("gdiplus.dll")]
		public static extern Status GdiplusShutdown(IntPtr token);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsZoomed(IntPtr hwnd);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindowVisible(IntPtr hwnd);

		[DllImport("kernel32.dll", EntryPoint = "LocalFree", SetLastError = true)]
		private static extern IntPtr _LocalFree(IntPtr hMem);

		[DllImport("user32.dll")]
		public static extern IntPtr MonitorFromWindow(IntPtr hwnd, MonitorOptions dwFlags);

		[DllImport("user32.dll")]
		public static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

		[DllImport("user32.dll")]
		public static extern IntPtr MonitorFromRect([In] ref RECT lprc, MonitorOptions dwFlags);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr LoadImage(IntPtr hinst, IntPtr lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

		[DllImport("user32.dll")]
		public static extern IntPtr GetFocus();

		[DllImport("user32.dll")]
		public static extern IntPtr SetFocus(IntPtr hWnd);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern int ToUnicode(uint virtualKey, uint scanCode, byte[] keyStates, [Out] [MarshalAs(UnmanagedType.LPArray)] char[] chars, int charMaxCount, uint flags);

		[DllImport("user32.dll")]
		public static extern bool GetKeyboardState(byte[] lpKeyState);

		[DllImport("user32.dll")]
		public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

		[DllImport("comdlg32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ChooseColor(CHOOSECOLOR lpcc);

		[DllImport("user32.dll", EntryPoint = "PostMessage", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		public static void PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam)
		{
			if (!_PostMessage(hWnd, Msg, wParam, lParam))
			{
				throw new Win32Exception();
			}
		}

		[DllImport("user32.dll", EntryPoint = "RegisterClassExW", SetLastError = true)]
		private static extern short _RegisterClassEx([In] ref WNDCLASSEX lpwcx);

		public static short RegisterClassEx(ref WNDCLASSEX lpwcx)
		{
			short num = _RegisterClassEx(ref lpwcx);
			if (num == 0)
			{
				HRESULT.ThrowLastError();
			}
			return num;
		}

		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegisterWindowMessage", SetLastError = true)]
		private static extern uint _RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

		public static WM RegisterWindowMessage(string lpString)
		{
			uint num = _RegisterWindowMessage(lpString);
			if (num == 0)
			{
				HRESULT.ThrowLastError();
			}
			return (WM)num;
		}

		[DllImport("user32.dll", EntryPoint = "SetActiveWindow", SetLastError = true)]
		private static extern IntPtr _SetActiveWindow(IntPtr hWnd);

		public static IntPtr SetActiveWindow(IntPtr hwnd)
		{
			Verify.IsNotDefault(hwnd, "hwnd");
			IntPtr intPtr = _SetActiveWindow(hwnd);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		public static IntPtr SetClassLongPtr(IntPtr hwnd, GCLP nIndex, IntPtr dwNewLong)
		{
			if (8 == IntPtr.Size)
			{
				return SetClassLongPtr64(hwnd, nIndex, dwNewLong);
			}
			return new IntPtr(SetClassLongPtr32(hwnd, nIndex, dwNewLong.ToInt32()));
		}

		[DllImport("user32.dll", EntryPoint = "SetClassLong", SetLastError = true)]
		private static extern int SetClassLongPtr32(IntPtr hWnd, GCLP nIndex, int dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetClassLongPtr", SetLastError = true)]
		private static extern IntPtr SetClassLongPtr64(IntPtr hWnd, GCLP nIndex, IntPtr dwNewLong);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern ErrorModes SetErrorMode(ErrorModes newMode);

		[DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SetProcessWorkingSetSize(IntPtr hProcess, IntPtr dwMinimiumWorkingSetSize, IntPtr dwMaximumWorkingSetSize);

		public static void SetProcessWorkingSetSize(IntPtr hProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize)
		{
			if (!_SetProcessWorkingSetSize(hProcess, new IntPtr(dwMinimumWorkingSetSize), new IntPtr(dwMaximumWorkingSetSize)))
			{
				throw new Win32Exception();
			}
		}

		public static IntPtr SetWindowLongPtr(IntPtr hwnd, GWL nIndex, IntPtr dwNewLong)
		{
			if (8 == IntPtr.Size)
			{
				return SetWindowLongPtr64(hwnd, nIndex, dwNewLong);
			}
			return new IntPtr(SetWindowLongPtr32(hwnd, nIndex, dwNewLong.ToInt32()));
		}

		[DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
		private static extern int SetWindowLongPtr32(IntPtr hWnd, GWL nIndex, int dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
		private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowRgn", SetLastError = true)]
		private static extern int _SetWindowRgn(IntPtr hWnd, IntPtr hRgn, [MarshalAs(UnmanagedType.Bool)] bool bRedraw);

		public static void SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw)
		{
			if (_SetWindowRgn(hWnd, hRgn, bRedraw) == 0)
			{
				throw new Win32Exception();
			}
		}

		[DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags);

		public static void SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags)
		{
			if (!_SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags))
			{
				throw new Win32Exception();
			}
		}

		[DllImport("shell32.dll")]
		public static extern Win32Error SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

		[DllImport("user32.dll", EntryPoint = "SystemParametersInfoW", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SystemParametersInfo_String(SPI uiAction, int uiParam, [MarshalAs(UnmanagedType.LPWStr)] string pvParam, SPIF fWinIni);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SystemParametersInfoW", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SystemParametersInfo_NONCLIENTMETRICS(SPI uiAction, int uiParam, [In] [Out] ref NONCLIENTMETRICS pvParam, SPIF fWinIni);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SystemParametersInfoW", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _SystemParametersInfo_HIGHCONTRAST(SPI uiAction, int uiParam, [In] [Out] ref HIGHCONTRAST pvParam, SPIF fWinIni);

		public static void SystemParametersInfo(SPI uiAction, int uiParam, string pvParam, SPIF fWinIni)
		{
			if (!_SystemParametersInfo_String(uiAction, uiParam, pvParam, fWinIni))
			{
				HRESULT.ThrowLastError();
			}
		}

		public static NONCLIENTMETRICS SystemParameterInfo_GetNONCLIENTMETRICS()
		{
			NONCLIENTMETRICS pvParam = Utility.IsOSVistaOrNewer ? NONCLIENTMETRICS.VistaMetricsStruct : NONCLIENTMETRICS.XPMetricsStruct;
			if (!_SystemParametersInfo_NONCLIENTMETRICS(SPI.GETNONCLIENTMETRICS, pvParam.cbSize, ref pvParam, SPIF.None))
			{
				HRESULT.ThrowLastError();
			}
			return pvParam;
		}

		public static HIGHCONTRAST SystemParameterInfo_GetHIGHCONTRAST()
		{
			HIGHCONTRAST hIGHCONTRAST = default(HIGHCONTRAST);
			hIGHCONTRAST.cbSize = Marshal.SizeOf(typeof(HIGHCONTRAST));
			HIGHCONTRAST pvParam = hIGHCONTRAST;
			if (!_SystemParametersInfo_HIGHCONTRAST(SPI.GETHIGHCONTRAST, pvParam.cbSize, ref pvParam, SPIF.None))
			{
				HRESULT.ThrowLastError();
			}
			return pvParam;
		}

		[DllImport("user32.dll")]
		public static extern uint TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

		[DllImport("gdi32.dll", EntryPoint = "SelectObject", SetLastError = true)]
		private static extern IntPtr _SelectObject(SafeDC hdc, IntPtr hgdiobj);

		public static IntPtr SelectObject(SafeDC hdc, IntPtr hgdiobj)
		{
			IntPtr intPtr = _SelectObject(hdc, hgdiobj);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		[DllImport("gdi32.dll", EntryPoint = "SelectObject", SetLastError = true)]
		private static extern IntPtr _SelectObjectSafeHBITMAP(SafeDC hdc, SafeHBITMAP hgdiobj);

		public static IntPtr SelectObject(SafeDC hdc, SafeHBITMAP hgdiobj)
		{
			IntPtr intPtr = _SelectObjectSafeHBITMAP(hdc, hgdiobj);
			if (intPtr == IntPtr.Zero)
			{
				HRESULT.ThrowLastError();
			}
			return intPtr;
		}

		[DllImport("user32.dll", SetLastError = true)]
		public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbSize);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ShowWindow(IntPtr hwnd, SW nCmdShow);

		[DllImport("user32.dll", EntryPoint = "UnregisterClass", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _UnregisterClassAtom(IntPtr lpClassName, IntPtr hInstance);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "UnregisterClass", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _UnregisterClassName(string lpClassName, IntPtr hInstance);

		public static void UnregisterClass(short atom, IntPtr hinstance)
		{
			if (!_UnregisterClassAtom(new IntPtr(atom), hinstance))
			{
				HRESULT.ThrowLastError();
			}
		}

		public static void UnregisterClass(string lpClassName, IntPtr hInstance)
		{
			if (!_UnregisterClassName(lpClassName, hInstance))
			{
				HRESULT.ThrowLastError();
			}
		}

		[DllImport("user32.dll", EntryPoint = "UpdateLayeredWindow", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _UpdateLayeredWindow(IntPtr hwnd, SafeDC hdcDst, [In] ref POINT pptDst, [In] ref SIZE psize, SafeDC hdcSrc, [In] ref POINT pptSrc, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags);

		[DllImport("user32.dll", EntryPoint = "UpdateLayeredWindow", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool _UpdateLayeredWindowIntPtr(IntPtr hwnd, IntPtr hdcDst, IntPtr pptDst, IntPtr psize, IntPtr hdcSrc, IntPtr pptSrc, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags);

		public static void UpdateLayeredWindow(IntPtr hwnd, SafeDC hdcDst, ref POINT pptDst, ref SIZE psize, SafeDC hdcSrc, ref POINT pptSrc, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags)
		{
			if (!_UpdateLayeredWindow(hwnd, hdcDst, ref pptDst, ref psize, hdcSrc, ref pptSrc, crKey, ref pblend, dwFlags))
			{
				HRESULT.ThrowLastError();
			}
		}

		public static void UpdateLayeredWindow(IntPtr hwnd, int crKey, ref BLENDFUNCTION pblend, ULW dwFlags)
		{
			if (!_UpdateLayeredWindowIntPtr(hwnd, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, crKey, ref pblend, dwFlags))
			{
				HRESULT.ThrowLastError();
			}
		}

		[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegisterClipboardFormatW", SetLastError = true)]
		private static extern uint _RegisterClipboardFormat(string lpszFormatName);

		public static uint RegisterClipboardFormat(string formatName)
		{
			uint num = _RegisterClipboardFormat(formatName);
			if (num == 0)
			{
				HRESULT.ThrowLastError();
			}
			return num;
		}

		[DllImport("ole32.dll")]
		public static extern void ReleaseStgMedium(ref STGMEDIUM pmedium);

		[DllImport("ole32.dll")]
		public static extern HRESULT CreateStreamOnHGlobal(IntPtr hGlobal, [MarshalAs(UnmanagedType.Bool)] bool fDeleteOnRelease, out IStream ppstm);

		[DllImport("urlmon.dll")]
		public static extern HRESULT CopyStgMedium(ref STGMEDIUM pcstgmedSrc, ref STGMEDIUM pstgmedDest);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("shell32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

		[DllImport("dwmapi.dll", PreserveSig = true)]
		public static extern void DwmInvalidateIconicBitmaps(IntPtr hwnd);

		[DllImport("dwmapi.dll", PreserveSig = true)]
		public static extern void DwmSetIconicThumbnail(IntPtr hwnd, IntPtr hbmp, DWM_SIT dwSITFlags);

		[DllImport("dwmapi.dll", PreserveSig = true)]
		public static extern void DwmSetIconicLivePreviewBitmap(IntPtr hwnd, IntPtr hbmp, RefPOINT pptClient, DWM_SIT dwSITFlags);

		[DllImport("shell32.dll", PreserveSig = true)]
		public static extern void SHGetItemFromDataObject(IDataObject pdtobj, DOGIF dwFlags, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs", PreserveSig = true)]
		private static extern void _SHAddToRecentDocsObj(SHARD uFlags, object pv);

		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs")]
		private static extern void _SHAddToRecentDocs_String(SHARD uFlags, [MarshalAs(UnmanagedType.LPWStr)] string pv);

		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs")]
		private static extern void _SHAddToRecentDocs_ShellLink(SHARD uFlags, IShellLinkW pv);

		public static void SHAddToRecentDocs(string path)
		{
			_SHAddToRecentDocs_String(SHARD.PATHW, path);
		}

		public static void SHAddToRecentDocs(IShellLinkW shellLink)
		{
			_SHAddToRecentDocs_ShellLink(SHARD.LINK, shellLink);
		}

		public static void SHAddToRecentDocs(SHARDAPPIDINFO info)
		{
			_SHAddToRecentDocsObj(SHARD.APPIDINFO, info);
		}

		public static void SHAddToRecentDocs(SHARDAPPIDINFOIDLIST infodIdList)
		{
			_SHAddToRecentDocsObj(SHARD.APPIDINFOIDLIST, infodIdList);
		}

		[DllImport("shell32.dll", PreserveSig = true)]
		public static extern HRESULT SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IBindCtx pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		[DllImport("shell32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool Shell_NotifyIcon(NIM dwMessage, [In] NOTIFYICONDATA lpdata);

		[DllImport("shell32.dll", PreserveSig = true)]
		public static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

		[DllImport("shell32.dll")]
		public static extern HRESULT GetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] out string AppID);
	}
}
