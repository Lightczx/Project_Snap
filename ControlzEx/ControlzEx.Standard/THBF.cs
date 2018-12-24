using System;

namespace ControlzEx.Standard
{
	[Flags]
	internal enum THBF : uint
	{
		ENABLED = 0x0,
		DISABLED = 0x1,
		DISMISSONCLICK = 0x2,
		NOBACKGROUND = 0x4,
		HIDDEN = 0x8,
		NONINTERACTIVE = 0x10
	}
}
