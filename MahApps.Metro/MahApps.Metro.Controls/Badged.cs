using ControlzEx;
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_BadgeContainer", Type = typeof(UIElement))]
	public class Badged : BadgedEx
	{
		public static readonly DependencyProperty BadgeChangedStoryboardProperty;

		public Storyboard BadgeChangedStoryboard
		{
			get
			{
				return (Storyboard)GetValue(BadgeChangedStoryboardProperty);
			}
			set
			{
				SetValue(BadgeChangedStoryboardProperty, value);
			}
		}

		static Badged()
		{
			BadgeChangedStoryboardProperty = DependencyProperty.Register("BadgeChangedStoryboard", typeof(Storyboard), typeof(Badged), new PropertyMetadata((object)null));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Badged), new FrameworkPropertyMetadata(typeof(Badged)));
		}

		public override void OnApplyTemplate()
		{
			base.BadgeChanged -= OnBadgeChanged;
			base.OnApplyTemplate();
			base.BadgeChanged += OnBadgeChanged;
		}

		private void OnBadgeChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			Storyboard badgeChangedStoryboard = BadgeChangedStoryboard;
			if (_badgeContainer != null && badgeChangedStoryboard != null)
			{
				try
				{
					_badgeContainer.BeginStoryboard(badgeChangedStoryboard);
				}
				catch (Exception innerException)
				{
					throw new MahAppsException("Uups, it seems like there is something wrong with the given Storyboard.", innerException);
				}
			}
		}
	}
}
