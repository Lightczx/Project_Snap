using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public enum GWL
	{
		WNDPROC = -4,
		HINSTANCE = -6,
		HWNDPARENT = -8,
		STYLE = -16,
		EXSTYLE = -20,
		USERDATA = -21,
		ID = -12
	}
}
