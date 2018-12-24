using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MahApps.Metro.Controls
{
	public class MultiFrameImage : Image
	{
		public static readonly DependencyProperty MultiFrameImageModeProperty;

		private readonly List<BitmapSource> _frames = new List<BitmapSource>();

		public MultiFrameImageMode MultiFrameImageMode
		{
			get
			{
				return (MultiFrameImageMode)GetValue(MultiFrameImageModeProperty);
			}
			set
			{
				SetValue(MultiFrameImageModeProperty, value);
			}
		}

		static MultiFrameImage()
		{
			MultiFrameImageModeProperty = DependencyProperty.Register("MultiFrameImageMode", typeof(MultiFrameImageMode), typeof(MultiFrameImage), new FrameworkPropertyMetadata(MultiFrameImageMode.ScaleDownLargerFrame, FrameworkPropertyMetadataOptions.AffectsRender));
			Image.SourceProperty.OverrideMetadata(typeof(MultiFrameImage), new FrameworkPropertyMetadata(OnSourceChanged));
		}

		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MultiFrameImage)d).UpdateFrameList();
		}

		private void UpdateFrameList()
		{
			_frames.Clear();
			BitmapFrame bitmapFrame = base.Source as BitmapFrame;
			if (bitmapFrame != null)
			{
				BitmapDecoder decoder = bitmapFrame.Decoder;
				if (decoder != null && decoder.Frames.Count != 0)
				{
					_frames.AddRange(from f in decoder.Frames
					group f by f.PixelWidth * f.PixelHeight into g
					orderby g.Key
					select (from f in g
					orderby f.Format.BitsPerPixel descending
					select f).First());
				}
			}
		}

		protected override void OnRender(DrawingContext dc)
		{
			if (_frames.Count == 0)
			{
				base.OnRender(dc);
			}
			else
			{
				switch (MultiFrameImageMode)
				{
				case MultiFrameImageMode.ScaleDownLargerFrame:
				{
					double minSize = Math.Max(base.RenderSize.Width, base.RenderSize.Height);
					BitmapSource imageSource = _frames.FirstOrDefault(delegate(BitmapSource f)
					{
						if (f.Width >= minSize)
						{
							return f.Height >= minSize;
						}
						return false;
					}) ?? _frames.Last();
					dc.DrawImage(imageSource, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
					break;
				}
				case MultiFrameImageMode.NoScaleSmallerFrame:
				{
					double maxSize = Math.Min(base.RenderSize.Width, base.RenderSize.Height);
					BitmapSource bitmapSource = _frames.LastOrDefault(delegate(BitmapSource f)
					{
						if (f.Width <= maxSize)
						{
							return f.Height <= maxSize;
						}
						return false;
					}) ?? _frames.First();
					dc.DrawImage(bitmapSource, new Rect((base.RenderSize.Width - bitmapSource.Width) / 2.0, (base.RenderSize.Height - bitmapSource.Height) / 2.0, bitmapSource.Width, bitmapSource.Height));
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}
