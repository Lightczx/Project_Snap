using System;

namespace ControlzEx.Standard
{
	[Obsolete("Use this element with caution and only if you know what you are doing. It's only meant to be used internally by MahApps.Metro and Fluent.Ribbon. Breaking changes might occur anytime without prior warning.")]
	public struct BLENDFUNCTION
	{
		public AC BlendOp;

		public byte BlendFlags;

		public byte SourceConstantAlpha;

		public AC AlphaFormat;
	}
}
