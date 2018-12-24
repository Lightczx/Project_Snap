using System;

namespace ControlzEx.Standard
{
	[Flags]
	internal enum THB : uint
	{
		BITMAP = 0x1,
		ICON = 0x2,
		TOOLTIP = 0x4,
		FLAGS = 0x8
	}
}
