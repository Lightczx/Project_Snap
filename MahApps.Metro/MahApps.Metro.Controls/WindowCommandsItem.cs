using ControlzEx;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_ContentPresenter", Type = typeof(UIElement))]
	[TemplatePart(Name = "PART_Separator", Type = typeof(UIElement))]
	public class WindowCommandsItem : ContentControl
	{
		private const string PART_ContentPresenter = "PART_ContentPresenter";

		private const string PART_Separator = "PART_Separator";

		public static readonly DependencyProperty IsSeparatorVisibleProperty;

		internal PropertyChangeNotifier VisibilityPropertyChangeNotifier
		{
			get;
			set;
		}

		public bool IsSeparatorVisible
		{
			get
			{
				return (bool)GetValue(IsSeparatorVisibleProperty);
			}
			set
			{
				SetValue(IsSeparatorVisibleProperty, value);
			}
		}

		static WindowCommandsItem()
		{
			IsSeparatorVisibleProperty = DependencyProperty.Register("IsSeparatorVisible", typeof(bool), typeof(WindowCommandsItem), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCommandsItem), new FrameworkPropertyMetadata(typeof(WindowCommandsItem)));
		}
	}
}
