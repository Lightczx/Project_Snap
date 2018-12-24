using System;
using System.Runtime.InteropServices;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public struct NONCLIENTMETRICS
	{
		public int cbSize;

		public int iBorderWidth;

		public int iScrollWidth;

		public int iScrollHeight;

		public int iCaptionWidth;

		public int iCaptionHeight;

		public LOGFONT lfCaptionFont;

		public int iSmCaptionWidth;

		public int iSmCaptionHeight;

		public LOGFONT lfSmCaptionFont;

		public int iMenuWidth;

		public int iMenuHeight;

		public LOGFONT lfMenuFont;

		public LOGFONT lfStatusFont;

		public LOGFONT lfMessageFont;

		public int iPaddedBorderWidth;

		public static NONCLIENTMETRICS VistaMetricsStruct
		{
			get
			{
				NONCLIENTMETRICS result = default(NONCLIENTMETRICS);
				result.cbSize = Marshal.SizeOf(typeof(NONCLIENTMETRICS));
				return result;
			}
		}

		public static NONCLIENTMETRICS XPMetricsStruct
		{
			get
			{
				NONCLIENTMETRICS result = default(NONCLIENTMETRICS);
				result.cbSize = Marshal.SizeOf(typeof(NONCLIENTMETRICS)) - 4;
				return result;
			}
		}
	}
}
