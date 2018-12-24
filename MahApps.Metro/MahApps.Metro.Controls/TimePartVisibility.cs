using System;

namespace MahApps.Metro.Controls
{
	[Flags]
	public enum TimePartVisibility
	{
		Hour = 0x2,
		Minute = 0x4,
		Second = 0x8,
		HourMinute = 0x6,
		All = 0xE
	}
}
