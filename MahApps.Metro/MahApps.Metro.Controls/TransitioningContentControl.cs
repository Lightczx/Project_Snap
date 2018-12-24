using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	public class TransitioningContentControl : ContentControl
	{
		internal const string PresentationGroup = "PresentationStates";

		internal const string NormalState = "Normal";

		internal const string PreviousContentPresentationSitePartName = "PreviousContentPresentationSite";

		internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";

		private ContentPresenter currentContentPresentationSite;

		private ContentPresenter previousContentPresentationSite;

		private bool allowIsTransitioningPropertyWrite;

		private Storyboard currentTransition;

		public const TransitionType DefaultTransitionState = TransitionType.Default;

		public static readonly DependencyProperty IsTransitioningProperty = DependencyProperty.Register("IsTransitioning", typeof(bool), typeof(TransitioningContentControl), new PropertyMetadata(OnIsTransitioningPropertyChanged));

		public static readonly DependencyProperty TransitionProperty = DependencyProperty.Register("Transition", typeof(TransitionType), typeof(TransitioningContentControl), new FrameworkPropertyMetadata(TransitionType.Default, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits, OnTransitionPropertyChanged));

		public static readonly DependencyProperty RestartTransitionOnContentChangeProperty = DependencyProperty.Register("RestartTransitionOnContentChange", typeof(bool), typeof(TransitioningContentControl), new PropertyMetadata(false, OnRestartTransitionOnContentChangePropertyChanged));

		public static readonly DependencyProperty CustomVisualStatesProperty = DependencyProperty.Register("CustomVisualStates", typeof(ObservableCollection<VisualState>), typeof(TransitioningContentControl), new PropertyMetadata(null));

		public static readonly DependencyProperty CustomVisualStatesNameProperty = DependencyProperty.Register("CustomVisualStatesName", typeof(string), typeof(TransitioningContentControl), new PropertyMetadata("CustomTransition"));

		public ObservableCollection<VisualState> CustomVisualStates
		{
			get
			{
				return (ObservableCollection<VisualState>)GetValue(CustomVisualStatesProperty);
			}
			set
			{
				SetValue(CustomVisualStatesProperty, value);
			}
		}

		public string CustomVisualStatesName
		{
			get
			{
				return (string)GetValue(CustomVisualStatesNameProperty);
			}
			set
			{
				SetValue(CustomVisualStatesNameProperty, value);
			}
		}

		public bool IsTransitioning
		{
			get
			{
				return (bool)GetValue(IsTransitioningProperty);
			}
			private set
			{
				allowIsTransitioningPropertyWrite = true;
				SetValue(IsTransitioningProperty, value);
				allowIsTransitioningPropertyWrite = false;
			}
		}

		public TransitionType Transition
		{
			get
			{
				return (TransitionType)GetValue(TransitionProperty);
			}
			set
			{
				SetValue(TransitionProperty, value);
			}
		}

		public bool RestartTransitionOnContentChange
		{
			get
			{
				return (bool)GetValue(RestartTransitionOnContentChangeProperty);
			}
			set
			{
				SetValue(RestartTransitionOnContentChangeProperty, value);
			}
		}

		private Storyboard CurrentTransition
		{
			get
			{
				return currentTransition;
			}
			set
			{
				if (currentTransition != null)
				{
					currentTransition.Completed -= OnTransitionCompleted;
				}
				currentTransition = value;
				if (currentTransition != null)
				{
					currentTransition.Completed += OnTransitionCompleted;
				}
			}
		}

		public event RoutedEventHandler TransitionCompleted;

		private static void OnIsTransitioningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TransitioningContentControl transitioningContentControl = (TransitioningContentControl)d;
			if (!transitioningContentControl.allowIsTransitioningPropertyWrite)
			{
				transitioningContentControl.IsTransitioning = (bool)e.OldValue;
				throw new InvalidOperationException();
			}
		}

		private static void OnTransitionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TransitioningContentControl transitioningContentControl = (TransitioningContentControl)d;
			TransitionType transitionType = (TransitionType)e.OldValue;
			TransitionType transitionType2 = (TransitionType)e.NewValue;
			if (transitioningContentControl.IsTransitioning)
			{
				transitioningContentControl.AbortTransition();
			}
			Storyboard storyboard = transitioningContentControl.GetStoryboard(transitionType2);
			if (storyboard == null)
			{
				if (VisualStates.TryGetVisualStateGroup(transitioningContentControl, "PresentationStates") != null)
				{
					transitioningContentControl.SetValue(TransitionProperty, transitionType);
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Temporary removed exception message", transitionType2));
				}
				transitioningContentControl.CurrentTransition = null;
			}
			else
			{
				transitioningContentControl.CurrentTransition = storyboard;
			}
		}

		private static void OnRestartTransitionOnContentChangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TransitioningContentControl)d).OnRestartTransitionOnContentChangeChanged((bool)e.OldValue, (bool)e.NewValue);
		}

		protected virtual void OnRestartTransitionOnContentChangeChanged(bool oldValue, bool newValue)
		{
		}

		public TransitioningContentControl()
		{
			CustomVisualStates = new ObservableCollection<VisualState>();
			base.DefaultStyleKey = typeof(TransitioningContentControl);
		}

		public override void OnApplyTemplate()
		{
			if (IsTransitioning)
			{
				AbortTransition();
			}
			if (CustomVisualStates != null && CustomVisualStates.Any())
			{
				VisualStateGroup visualStateGroup = VisualStates.TryGetVisualStateGroup(this, "PresentationStates");
				if (visualStateGroup != null)
				{
					foreach (VisualState customVisualState in CustomVisualStates)
					{
						visualStateGroup.States.Add(customVisualState);
					}
				}
			}
			base.OnApplyTemplate();
			previousContentPresentationSite = (GetTemplateChild("PreviousContentPresentationSite") as ContentPresenter);
			currentContentPresentationSite = (GetTemplateChild("CurrentContentPresentationSite") as ContentPresenter);
			if (currentContentPresentationSite != null)
			{
				if (base.ContentTemplateSelector != null)
				{
					currentContentPresentationSite.ContentTemplate = base.ContentTemplateSelector.SelectTemplate(base.Content, this);
				}
				else
				{
					currentContentPresentationSite.ContentTemplate = base.ContentTemplate;
				}
				currentContentPresentationSite.Content = base.Content;
			}
			Storyboard storyboard2 = CurrentTransition = GetStoryboard(Transition);
			if (storyboard2 == null)
			{
				TransitionType transition = Transition;
				Transition = TransitionType.Default;
				throw new ArgumentException($"'{transition}' Transition could not be found!", "Transition");
			}
			VisualStateManager.GoToState(this, "Normal", useTransitions: false);
		}

		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			StartTransition(oldContent, newContent);
		}

		private void StartTransition(object oldContent, object newContent)
		{
			if (currentContentPresentationSite != null && previousContentPresentationSite != null)
			{
				if (RestartTransitionOnContentChange)
				{
					CurrentTransition.Completed -= OnTransitionCompleted;
				}
				if (base.ContentTemplateSelector != null)
				{
					previousContentPresentationSite.ContentTemplate = base.ContentTemplateSelector.SelectTemplate(oldContent, this);
					currentContentPresentationSite.ContentTemplate = base.ContentTemplateSelector.SelectTemplate(newContent, this);
				}
				else
				{
					previousContentPresentationSite.ContentTemplate = base.ContentTemplate;
					currentContentPresentationSite.ContentTemplate = base.ContentTemplate;
				}
				currentContentPresentationSite.Content = newContent;
				previousContentPresentationSite.Content = oldContent;
				if (!IsTransitioning || RestartTransitionOnContentChange)
				{
					if (RestartTransitionOnContentChange)
					{
						CurrentTransition.Completed += OnTransitionCompleted;
					}
					IsTransitioning = true;
					VisualStateManager.GoToState(this, "Normal", useTransitions: false);
					VisualStateManager.GoToState(this, GetTransitionName(Transition), useTransitions: true);
				}
			}
		}

		public void ReloadTransition()
		{
			if (currentContentPresentationSite != null && previousContentPresentationSite != null)
			{
				if (RestartTransitionOnContentChange)
				{
					CurrentTransition.Completed -= OnTransitionCompleted;
				}
				if (!IsTransitioning || RestartTransitionOnContentChange)
				{
					if (RestartTransitionOnContentChange)
					{
						CurrentTransition.Completed += OnTransitionCompleted;
					}
					IsTransitioning = true;
					VisualStateManager.GoToState(this, "Normal", useTransitions: false);
					VisualStateManager.GoToState(this, GetTransitionName(Transition), useTransitions: true);
				}
			}
		}

		private void OnTransitionCompleted(object sender, EventArgs e)
		{
			ClockGroup clockGroup = sender as ClockGroup;
			AbortTransition();
			if (clockGroup == null || clockGroup.CurrentState == ClockState.Stopped)
			{
				this.TransitionCompleted?.Invoke(this, new RoutedEventArgs());
			}
		}

		public void AbortTransition()
		{
			VisualStateManager.GoToState(this, "Normal", useTransitions: false);
			IsTransitioning = false;
			if (previousContentPresentationSite != null)
			{
				previousContentPresentationSite.ContentTemplate = null;
				previousContentPresentationSite.Content = null;
			}
		}

		private Storyboard GetStoryboard(TransitionType newTransition)
		{
			VisualStateGroup visualStateGroup = VisualStates.TryGetVisualStateGroup(this, "PresentationStates");
			Storyboard result = null;
			if (visualStateGroup != null)
			{
				string transitionName = GetTransitionName(newTransition);
				result = (from state in visualStateGroup.States.OfType<VisualState>()
				where state.Name == transitionName
				select state.Storyboard).FirstOrDefault();
			}
			return result;
		}

		private string GetTransitionName(TransitionType transition)
		{
			switch (transition)
			{
			default:
				return "DefaultTransition";
			case TransitionType.Normal:
				return "Normal";
			case TransitionType.Up:
				return "UpTransition";
			case TransitionType.Down:
				return "DownTransition";
			case TransitionType.Right:
				return "RightTransition";
			case TransitionType.RightReplace:
				return "RightReplaceTransition";
			case TransitionType.Left:
				return "LeftTransition";
			case TransitionType.LeftReplace:
				return "LeftReplaceTransition";
			case TransitionType.Custom:
				return CustomVisualStatesName;
			}
		}
	}
}
