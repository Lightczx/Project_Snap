using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	public class MetroTabItem : TabItem
	{
		public static readonly DependencyProperty CloseButtonEnabledProperty = DependencyProperty.Register("CloseButtonEnabled", typeof(bool), typeof(MetroTabItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty CloseTabCommandProperty = DependencyProperty.Register("CloseTabCommand", typeof(ICommand), typeof(MetroTabItem));

		public static readonly DependencyProperty CloseTabCommandParameterProperty = DependencyProperty.Register("CloseTabCommandParameter", typeof(object), typeof(MetroTabItem), new PropertyMetadata(null));

		public static readonly DependencyProperty CloseButtonMarginProperty = DependencyProperty.Register("CloseButtonMargin", typeof(Thickness), typeof(MetroTabItem), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));

		public bool CloseButtonEnabled
		{
			get
			{
				return (bool)GetValue(CloseButtonEnabledProperty);
			}
			set
			{
				SetValue(CloseButtonEnabledProperty, value);
			}
		}

		public ICommand CloseTabCommand
		{
			get
			{
				return (ICommand)GetValue(CloseTabCommandProperty);
			}
			set
			{
				SetValue(CloseTabCommandProperty, value);
			}
		}

		public object CloseTabCommandParameter
		{
			get
			{
				return GetValue(CloseTabCommandParameterProperty);
			}
			set
			{
				SetValue(CloseTabCommandParameterProperty, value);
			}
		}

		public Thickness CloseButtonMargin
		{
			get
			{
				return (Thickness)GetValue(CloseButtonMarginProperty);
			}
			set
			{
				SetValue(CloseButtonMarginProperty, value);
			}
		}

		public MetroTabItem()
		{
			base.DefaultStyleKey = typeof(MetroTabItem);
		}
	}
}
