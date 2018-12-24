using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	public class MetroContentControl : ContentControl
	{
		public static readonly DependencyProperty ReverseTransitionProperty = DependencyProperty.Register("ReverseTransition", typeof(bool), typeof(MetroContentControl), new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty TransitionsEnabledProperty = DependencyProperty.Register("TransitionsEnabled", typeof(bool), typeof(MetroContentControl), new FrameworkPropertyMetadata(true));

		public static readonly DependencyProperty OnlyLoadTransitionProperty = DependencyProperty.Register("OnlyLoadTransition", typeof(bool), typeof(MetroContentControl), new FrameworkPropertyMetadata(false));

		public static readonly RoutedEvent TransitionCompletedEvent = EventManager.RegisterRoutedEvent("TransitionCompleted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MetroContentControl));

		private Storyboard afterLoadedStoryboard;

		private Storyboard afterLoadedReverseStoryboard;

		private bool transitionLoaded;

		public bool ReverseTransition
		{
			get
			{
				return (bool)GetValue(ReverseTransitionProperty);
			}
			set
			{
				SetValue(ReverseTransitionProperty, value);
			}
		}

		public bool TransitionsEnabled
		{
			get
			{
				return (bool)GetValue(TransitionsEnabledProperty);
			}
			set
			{
				SetValue(TransitionsEnabledProperty, value);
			}
		}

		public bool OnlyLoadTransition
		{
			get
			{
				return (bool)GetValue(OnlyLoadTransitionProperty);
			}
			set
			{
				SetValue(OnlyLoadTransitionProperty, value);
			}
		}

		public event RoutedEventHandler TransitionCompleted
		{
			add
			{
				AddHandler(TransitionCompletedEvent, value);
			}
			remove
			{
				RemoveHandler(TransitionCompletedEvent, value);
			}
		}

		public MetroContentControl()
		{
			base.DefaultStyleKey = typeof(MetroContentControl);
			base.Loaded += MetroContentControlLoaded;
			base.Unloaded += MetroContentControlUnloaded;
		}

		private void MetroContentControlIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (TransitionsEnabled && !transitionLoaded)
			{
				if (!base.IsVisible)
				{
					VisualStateManager.GoToState(this, ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded", useTransitions: false);
				}
				else
				{
					VisualStateManager.GoToState(this, ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded", useTransitions: true);
				}
			}
		}

		private void MetroContentControlUnloaded(object sender, RoutedEventArgs e)
		{
			if (TransitionsEnabled)
			{
				UnsetStoryboardEvents();
				if (transitionLoaded)
				{
					VisualStateManager.GoToState(this, ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded", useTransitions: false);
				}
				base.IsVisibleChanged -= MetroContentControlIsVisibleChanged;
			}
		}

		private void MetroContentControlLoaded(object sender, RoutedEventArgs e)
		{
			if (TransitionsEnabled)
			{
				if (!transitionLoaded)
				{
					SetStoryboardEvents();
					transitionLoaded = OnlyLoadTransition;
					VisualStateManager.GoToState(this, ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded", useTransitions: true);
				}
				base.IsVisibleChanged -= MetroContentControlIsVisibleChanged;
				base.IsVisibleChanged += MetroContentControlIsVisibleChanged;
			}
			else
			{
				Grid grid = (Grid)GetTemplateChild("root");
				if (grid != null)
				{
					grid.Opacity = 1.0;
					TranslateTransform translateTransform = (TranslateTransform)grid.RenderTransform;
					if (translateTransform.IsFrozen)
					{
						TranslateTransform translateTransform2 = translateTransform.Clone();
						translateTransform2.X = 0.0;
						grid.RenderTransform = translateTransform2;
					}
					else
					{
						translateTransform.X = 0.0;
					}
				}
			}
		}

		public void Reload()
		{
			if (TransitionsEnabled && !transitionLoaded)
			{
				if (ReverseTransition)
				{
					VisualStateManager.GoToState(this, "BeforeLoaded", useTransitions: true);
					VisualStateManager.GoToState(this, "AfterUnLoadedReverse", useTransitions: true);
				}
				else
				{
					VisualStateManager.GoToState(this, "BeforeLoaded", useTransitions: true);
					VisualStateManager.GoToState(this, "AfterLoaded", useTransitions: true);
				}
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			afterLoadedStoryboard = (GetTemplateChild("AfterLoadedStoryboard") as Storyboard);
			afterLoadedReverseStoryboard = (GetTemplateChild("AfterLoadedReverseStoryboard") as Storyboard);
		}

		private void AfterLoadedStoryboardCompleted(object sender, EventArgs e)
		{
			if (transitionLoaded)
			{
				UnsetStoryboardEvents();
			}
			InvalidateVisual();
			RaiseEvent(new RoutedEventArgs(TransitionCompletedEvent));
		}

		private void SetStoryboardEvents()
		{
			if (afterLoadedStoryboard != null)
			{
				afterLoadedStoryboard.Completed += AfterLoadedStoryboardCompleted;
			}
			if (afterLoadedReverseStoryboard != null)
			{
				afterLoadedReverseStoryboard.Completed += AfterLoadedStoryboardCompleted;
			}
		}

		private void UnsetStoryboardEvents()
		{
			if (afterLoadedStoryboard != null)
			{
				afterLoadedStoryboard.Completed -= AfterLoadedStoryboardCompleted;
			}
			if (afterLoadedReverseStoryboard != null)
			{
				afterLoadedReverseStoryboard.Completed -= AfterLoadedStoryboardCompleted;
			}
		}
	}
}
