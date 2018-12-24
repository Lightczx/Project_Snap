using System;

namespace ControlzEx.Native
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public static class Constants
	{
		[Flags]
		public enum RedrawWindowFlags : uint
		{
			Invalidate = 0x1,
			InternalPaint = 0x2,
			Erase = 0x4,
			Validate = 0x8,
			NoInternalPaint = 0x10,
			NoErase = 0x20,
			NoChildren = 0x40,
			AllChildren = 0x80,
			UpdateNow = 0x100,
			EraseNow = 0x200,
			Frame = 0x400,
			NoFrame = 0x800
		}

		public const int GCLP_HBRBACKGROUND = -10;

		public const uint TPM_RETURNCMD = 256u;

		public const uint TPM_LEFTBUTTON = 0u;

		public const uint SYSCOMMAND = 274u;

		public const int MF_GRAYED = 1;

		public const int MF_BYCOMMAND = 0;

		public const int MF_ENABLED = 0;

		public const int VK_SHIFT = 16;

		public const int VK_CONTROL = 17;

		public const int VK_MENU = 18;

		public const uint MAPVK_VK_TO_VSC = 0u;

		public const uint MAPVK_VSC_TO_VK = 1u;

		public const uint MAPVK_VK_TO_CHAR = 2u;

		public const uint MAPVK_VSC_TO_VK_EX = 3u;

		public const uint MAPVK_VK_TO_VSC_EX = 4u;

		public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

		public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

		public static readonly IntPtr HWND_TOP = new IntPtr(0);

		public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

		public const int CC_ANYCOLOR = 256;
	}
}
