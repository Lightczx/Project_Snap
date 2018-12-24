using System;

namespace MahApps.Metro.Controls
{
	[Flags]
	public enum WindowCommandsOverlayBehavior
	{
		Never = 0x0,
		Flyouts = 0x1,
		HiddenTitleBar = 0x2,
		Always = 0x3
	}
}
