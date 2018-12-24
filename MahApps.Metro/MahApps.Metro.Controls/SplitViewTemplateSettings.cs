using System.Windows;

namespace MahApps.Metro.Controls
{
	public sealed class SplitViewTemplateSettings : DependencyObject
	{
		internal static readonly DependencyProperty CompactPaneGridLengthProperty = DependencyProperty.Register("CompactPaneGridLength", typeof(GridLength), typeof(SplitViewTemplateSettings), new PropertyMetadata(null));

		internal static readonly DependencyProperty NegativeOpenPaneLengthProperty = DependencyProperty.Register("NegativeOpenPaneLength", typeof(double), typeof(SplitViewTemplateSettings), new PropertyMetadata(0.0));

		internal static readonly DependencyProperty NegativeOpenPaneLengthMinusCompactLengthProperty = DependencyProperty.Register("NegativeOpenPaneLengthMinusCompactLength", typeof(double), typeof(SplitViewTemplateSettings), new PropertyMetadata(0.0));

		internal static readonly DependencyProperty OpenPaneGridLengthProperty = DependencyProperty.Register("OpenPaneGridLength", typeof(GridLength), typeof(SplitViewTemplateSettings), new PropertyMetadata(null));

		internal static readonly DependencyProperty OpenPaneLengthProperty = DependencyProperty.Register("OpenPaneLength", typeof(double), typeof(SplitViewTemplateSettings), new PropertyMetadata(0.0));

		internal static readonly DependencyProperty OpenPaneLengthMinusCompactLengthProperty = DependencyProperty.Register("OpenPaneLengthMinusCompactLength", typeof(double), typeof(SplitViewTemplateSettings), new PropertyMetadata(0.0));

		public GridLength CompactPaneGridLength
		{
			get
			{
				return (GridLength)GetValue(CompactPaneGridLengthProperty);
			}
			private set
			{
				SetValue(CompactPaneGridLengthProperty, value);
			}
		}

		public double NegativeOpenPaneLength
		{
			get
			{
				return (double)GetValue(NegativeOpenPaneLengthProperty);
			}
			private set
			{
				SetValue(NegativeOpenPaneLengthProperty, value);
			}
		}

		public double NegativeOpenPaneLengthMinusCompactLength
		{
			get
			{
				return (double)GetValue(NegativeOpenPaneLengthMinusCompactLengthProperty);
			}
			set
			{
				SetValue(NegativeOpenPaneLengthMinusCompactLengthProperty, value);
			}
		}

		public GridLength OpenPaneGridLength
		{
			get
			{
				return (GridLength)GetValue(OpenPaneGridLengthProperty);
			}
			private set
			{
				SetValue(OpenPaneGridLengthProperty, value);
			}
		}

		public double OpenPaneLength
		{
			get
			{
				return (double)GetValue(OpenPaneLengthProperty);
			}
			private set
			{
				SetValue(OpenPaneLengthProperty, value);
			}
		}

		public double OpenPaneLengthMinusCompactLength
		{
			get
			{
				return (double)GetValue(OpenPaneLengthMinusCompactLengthProperty);
			}
			private set
			{
				SetValue(OpenPaneLengthMinusCompactLengthProperty, value);
			}
		}

		internal SplitView Owner
		{
			get;
		}

		internal SplitViewTemplateSettings(SplitView owner)
		{
			Owner = owner;
			Update();
		}

		internal void Update()
		{
			CompactPaneGridLength = new GridLength(Owner.CompactPaneLength, GridUnitType.Pixel);
			OpenPaneGridLength = new GridLength(Owner.OpenPaneLength, GridUnitType.Pixel);
			OpenPaneLength = Owner.OpenPaneLength;
			OpenPaneLengthMinusCompactLength = Owner.OpenPaneLength - Owner.CompactPaneLength;
			NegativeOpenPaneLength = 0.0 - OpenPaneLength;
			NegativeOpenPaneLengthMinusCompactLength = 0.0 - OpenPaneLengthMinusCompactLength;
		}
	}
}
