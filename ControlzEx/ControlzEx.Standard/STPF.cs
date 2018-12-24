using System;

namespace ControlzEx.Standard
{
	[Flags]
	internal enum STPF
	{
		NONE = 0x0,
		USEAPPTHUMBNAILALWAYS = 0x1,
		USEAPPTHUMBNAILWHENACTIVE = 0x2,
		USEAPPPEEKALWAYS = 0x4,
		USEAPPPEEKWHENACTIVE = 0x8
	}
}
