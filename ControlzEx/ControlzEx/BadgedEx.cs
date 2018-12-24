using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControlzEx
{
	[TemplatePart(Name = "PART_BadgeContainer", Type = typeof(UIElement))]
	public class BadgedEx : ContentControl
	{
		public const string BadgeContainerPartName = "PART_BadgeContainer";

		protected FrameworkElement _badgeContainer;

		public static readonly DependencyProperty BadgeProperty = DependencyProperty.Register("Badge", typeof(object), typeof(BadgedEx), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, OnBadgeChanged));

		public static readonly DependencyProperty BadgeBackgroundProperty = DependencyProperty.Register("BadgeBackground", typeof(Brush), typeof(BadgedEx), new PropertyMetadata((object)null));

		public static readonly DependencyProperty BadgeForegroundProperty = DependencyProperty.Register("BadgeForeground", typeof(Brush), typeof(BadgedEx), new PropertyMetadata((object)null));

		public static readonly DependencyProperty BadgePlacementModeProperty = DependencyProperty.Register("BadgePlacementMode", typeof(BadgePlacementMode), typeof(BadgedEx), new PropertyMetadata(BadgePlacementMode.TopLeft));

		public static readonly RoutedEvent BadgeChangedEvent = EventManager.RegisterRoutedEvent("BadgeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(BadgedEx));

		private static readonly DependencyPropertyKey IsBadgeSetPropertyKey = DependencyProperty.RegisterReadOnly("IsBadgeSet", typeof(bool), typeof(BadgedEx), new PropertyMetadata(false));

		public static readonly DependencyProperty IsBadgeSetProperty = IsBadgeSetPropertyKey.DependencyProperty;

		public object Badge
		{
			get
			{
				return GetValue(BadgeProperty);
			}
			set
			{
				SetValue(BadgeProperty, value);
			}
		}

		public Brush BadgeBackground
		{
			get
			{
				return (Brush)GetValue(BadgeBackgroundProperty);
			}
			set
			{
				SetValue(BadgeBackgroundProperty, value);
			}
		}

		public Brush BadgeForeground
		{
			get
			{
				return (Brush)GetValue(BadgeForegroundProperty);
			}
			set
			{
				SetValue(BadgeForegroundProperty, value);
			}
		}

		public BadgePlacementMode BadgePlacementMode
		{
			get
			{
				return (BadgePlacementMode)GetValue(BadgePlacementModeProperty);
			}
			set
			{
				SetValue(BadgePlacementModeProperty, value);
			}
		}

		public bool IsBadgeSet
		{
			get
			{
				return (bool)GetValue(IsBadgeSetProperty);
			}
			private set
			{
				SetValue(IsBadgeSetPropertyKey, value);
			}
		}

		public event RoutedPropertyChangedEventHandler<object> BadgeChanged
		{
			add
			{
				AddHandler(BadgeChangedEvent, value);
			}
			remove
			{
				RemoveHandler(BadgeChangedEvent, value);
			}
		}

		private static void OnBadgeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			BadgedEx obj = (BadgedEx)d;
			obj.IsBadgeSet = (!string.IsNullOrWhiteSpace(e.NewValue as string) || (e.NewValue != null && !(e.NewValue is string)));
			RoutedPropertyChangedEventArgs<object> e2 = new RoutedPropertyChangedEventArgs<object>(e.OldValue, e.NewValue)
			{
				RoutedEvent = BadgeChangedEvent
			};
			obj.RaiseEvent(e2);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_badgeContainer = (GetTemplateChild("PART_BadgeContainer") as FrameworkElement);
		}

		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			Size result = base.ArrangeOverride(arrangeBounds);
			if (_badgeContainer == null)
			{
				return result;
			}
			Size size = _badgeContainer.DesiredSize;
			if ((size.Width <= 0.0 || size.Height <= 0.0) && !double.IsNaN(_badgeContainer.ActualWidth) && !double.IsInfinity(_badgeContainer.ActualWidth) && !double.IsNaN(_badgeContainer.ActualHeight) && !double.IsInfinity(_badgeContainer.ActualHeight))
			{
				size = new Size(_badgeContainer.ActualWidth, _badgeContainer.ActualHeight);
			}
			double num = 0.0 - size.Width / 2.0;
			double num2 = 0.0 - size.Height / 2.0;
			_badgeContainer.Margin = new Thickness(0.0);
			_badgeContainer.Margin = new Thickness(num, num2, num, num2);
			return result;
		}
	}
}
