using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class ContentControlEx : ContentControl
	{
		public static readonly DependencyProperty ContentCharacterCasingProperty;

		public static readonly DependencyProperty RecognizesAccessKeyProperty;

		public CharacterCasing ContentCharacterCasing
		{
			get
			{
				return (CharacterCasing)GetValue(ContentCharacterCasingProperty);
			}
			set
			{
				SetValue(ContentCharacterCasingProperty, value);
			}
		}

		public bool RecognizesAccessKey
		{
			get
			{
				return (bool)GetValue(RecognizesAccessKeyProperty);
			}
			set
			{
				SetValue(RecognizesAccessKeyProperty, value);
			}
		}

		static ContentControlEx()
		{
			ContentCharacterCasingProperty = DependencyProperty.Register("ContentCharacterCasing", typeof(CharacterCasing), typeof(ContentControlEx), new FrameworkPropertyMetadata(CharacterCasing.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits), delegate(object value)
			{
				if (CharacterCasing.Normal <= (CharacterCasing)value)
				{
					return (CharacterCasing)value <= CharacterCasing.Upper;
				}
				return false;
			});
			RecognizesAccessKeyProperty = DependencyProperty.Register("RecognizesAccessKey", typeof(bool), typeof(ContentControlEx), new FrameworkPropertyMetadata(false));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentControlEx), new FrameworkPropertyMetadata(typeof(ContentControlEx)));
		}
	}
}
