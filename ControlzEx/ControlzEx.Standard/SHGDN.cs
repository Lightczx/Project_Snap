using System;

namespace ControlzEx.Standard
{
	[Flags]
	internal enum SHGDN
	{
		SHGDN_NORMAL = 0x0,
		SHGDN_INFOLDER = 0x1,
		SHGDN_FOREDITING = 0x1000,
		SHGDN_FORADDRESSBAR = 0x4000,
		SHGDN_FORPARSING = 0x8000
	}
}
