using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_Presenter", Type = typeof(TransitioningContentControl))]
	[TemplatePart(Name = "PART_BackButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_ForwardButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_UpButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_DownButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_BannerGrid", Type = typeof(Grid))]
	[TemplatePart(Name = "PART_BannerLabel", Type = typeof(Label))]
	public class FlipView : Selector
	{
		private const string PART_Presenter = "PART_Presenter";

		private const string PART_BackButton = "PART_BackButton";

		private const string PART_ForwardButton = "PART_ForwardButton";

		private const string PART_UpButton = "PART_UpButton";

		private const string PART_DownButton = "PART_DownButton";

		private const string PART_BannerGrid = "PART_BannerGrid";

		private const string PART_BannerLabel = "PART_BannerLabel";

		private TransitioningContentControl presenter;

		private Button backButton;

		private Button forwardButton;

		private Button upButton;

		private Button downButton;

		private Grid bannerGrid;

		private Label bannerLabel;

		private Storyboard showBannerStoryboard;

		private Storyboard hideBannerStoryboard;

		private Storyboard hideControlStoryboard;

		private Storyboard showControlStoryboard;

		private EventHandler hideControlStoryboardCompletedHandler;

		private bool loaded;

		private bool allowSelectedIndexChangedCallback = true;

		public static readonly DependencyProperty UpTransitionProperty;

		public static readonly DependencyProperty DownTransitionProperty;

		public static readonly DependencyProperty LeftTransitionProperty;

		public static readonly DependencyProperty RightTransitionProperty;

		[Obsolete("This property will be deleted in the next release. You should use now MouseHoverBorderEnabled instead.")]
		public static readonly DependencyProperty MouseOverGlowEnabledProperty;

		public static readonly DependencyProperty MouseHoverBorderEnabledProperty;

		public static readonly DependencyProperty MouseHoverBorderBrushProperty;

		public static readonly DependencyProperty MouseHoverBorderThicknessProperty;

		public static readonly DependencyProperty OrientationProperty;

		public static readonly DependencyProperty IsBannerEnabledProperty;

		public static readonly DependencyProperty BannerTextProperty;

		public static readonly DependencyProperty CircularNavigationProperty;

		public static readonly DependencyProperty IsNavigationEnabledProperty;

		public TransitionType UpTransition
		{
			get
			{
				return (TransitionType)GetValue(UpTransitionProperty);
			}
			set
			{
				SetValue(UpTransitionProperty, value);
			}
		}

		public TransitionType DownTransition
		{
			get
			{
				return (TransitionType)GetValue(DownTransitionProperty);
			}
			set
			{
				SetValue(DownTransitionProperty, value);
			}
		}

		public TransitionType LeftTransition
		{
			get
			{
				return (TransitionType)GetValue(LeftTransitionProperty);
			}
			set
			{
				SetValue(LeftTransitionProperty, value);
			}
		}

		public TransitionType RightTransition
		{
			get
			{
				return (TransitionType)GetValue(RightTransitionProperty);
			}
			set
			{
				SetValue(RightTransitionProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use now MouseHoverBorderEnabled instead.")]
		public bool MouseOverGlowEnabled
		{
			get
			{
				return (bool)GetValue(MouseOverGlowEnabledProperty);
			}
			set
			{
				SetValue(MouseOverGlowEnabledProperty, value);
			}
		}

		public bool MouseHoverBorderEnabled
		{
			get
			{
				return (bool)GetValue(MouseHoverBorderEnabledProperty);
			}
			set
			{
				SetValue(MouseHoverBorderEnabledProperty, value);
			}
		}

		public Brush MouseHoverBorderBrush
		{
			get
			{
				return (Brush)GetValue(MouseHoverBorderBrushProperty);
			}
			set
			{
				SetValue(MouseHoverBorderBrushProperty, value);
			}
		}

		public Thickness MouseHoverBorderThickness
		{
			get
			{
				return (Thickness)GetValue(MouseHoverBorderThicknessProperty);
			}
			set
			{
				SetValue(MouseHoverBorderThicknessProperty, value);
			}
		}

		public Orientation Orientation
		{
			get
			{
				return (Orientation)GetValue(OrientationProperty);
			}
			set
			{
				SetValue(OrientationProperty, value);
			}
		}

		public string BannerText
		{
			get
			{
				return (string)GetValue(BannerTextProperty);
			}
			set
			{
				SetValue(BannerTextProperty, value);
			}
		}

		public bool IsBannerEnabled
		{
			get
			{
				return (bool)GetValue(IsBannerEnabledProperty);
			}
			set
			{
				SetValue(IsBannerEnabledProperty, value);
			}
		}

		public bool CircularNavigation
		{
			get
			{
				return (bool)GetValue(CircularNavigationProperty);
			}
			set
			{
				SetValue(CircularNavigationProperty, value);
			}
		}

		public bool IsNavigationEnabled
		{
			get
			{
				return (bool)GetValue(IsNavigationEnabledProperty);
			}
			set
			{
				SetValue(IsNavigationEnabledProperty, value);
			}
		}

		static FlipView()
		{
			UpTransitionProperty = DependencyProperty.Register("UpTransition", typeof(TransitionType), typeof(FlipView), new PropertyMetadata(TransitionType.Up));
			DownTransitionProperty = DependencyProperty.Register("DownTransition", typeof(TransitionType), typeof(FlipView), new PropertyMetadata(TransitionType.Down));
			LeftTransitionProperty = DependencyProperty.Register("LeftTransition", typeof(TransitionType), typeof(FlipView), new PropertyMetadata(TransitionType.LeftReplace));
			RightTransitionProperty = DependencyProperty.Register("RightTransition", typeof(TransitionType), typeof(FlipView), new PropertyMetadata(TransitionType.RightReplace));
			MouseOverGlowEnabledProperty = DependencyProperty.Register("MouseOverGlowEnabled", typeof(bool), typeof(FlipView), new PropertyMetadata(true, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
			{
				((FlipView)o).SetCurrentValue(MouseHoverBorderEnabledProperty, (bool)e.NewValue);
			}));
			MouseHoverBorderEnabledProperty = DependencyProperty.Register("MouseHoverBorderEnabled", typeof(bool), typeof(FlipView), new PropertyMetadata(true));
			MouseHoverBorderBrushProperty = DependencyProperty.Register("MouseHoverBorderBrush", typeof(Brush), typeof(FlipView), new PropertyMetadata(Brushes.LightGray));
			MouseHoverBorderThicknessProperty = DependencyProperty.Register("MouseHoverBorderThickness", typeof(Thickness), typeof(FlipView), new PropertyMetadata(new Thickness(4.0)));
			OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(FlipView), new PropertyMetadata(Orientation.Horizontal, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
			{
				((FlipView)d).DetectControlButtonsStatus();
			}));
			IsBannerEnabledProperty = DependencyProperty.Register("IsBannerEnabled", typeof(bool), typeof(FlipView), new UIPropertyMetadata(true, OnIsBannerEnabledPropertyChangedCallback));
			BannerTextProperty = DependencyProperty.Register("BannerText", typeof(string), typeof(FlipView), new FrameworkPropertyMetadata("Banner", FrameworkPropertyMetadataOptions.AffectsRender, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
			{
				ExecuteWhenLoaded((FlipView)d, delegate
				{
					((FlipView)d).ChangeBannerText((string)e.NewValue);
				});
			}));
			CircularNavigationProperty = DependencyProperty.Register("CircularNavigation", typeof(bool), typeof(FlipView), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
			{
				((FlipView)d).DetectControlButtonsStatus();
			}));
			IsNavigationEnabledProperty = DependencyProperty.Register("IsNavigationEnabled", typeof(bool), typeof(FlipView), new PropertyMetadata(true, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
			{
				((FlipView)d).DetectControlButtonsStatus();
			}));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata(typeof(FlipView)));
			PropertyMetadata previousSelectedIndexPropertyMetadata = Selector.SelectedIndexProperty.GetMetadata(typeof(FlipView));
			Selector.SelectedIndexProperty.OverrideMetadata(typeof(FlipView), new FrameworkPropertyMetadata
			{
				CoerceValueCallback = delegate(DependencyObject d, object value)
				{
					if (previousSelectedIndexPropertyMetadata.CoerceValueCallback != null)
					{
						Delegate[] invocationList = previousSelectedIndexPropertyMetadata.CoerceValueCallback.GetInvocationList();
						for (int i = 0; i < invocationList.Length; i++)
						{
							value = ((CoerceValueCallback)invocationList[i])(d, value);
						}
					}
					return CoerceSelectedIndexProperty(d, value);
				}
			});
		}

		public FlipView()
		{
			base.Loaded += FlipViewLoaded;
		}

		private static object CoerceSelectedIndexProperty(DependencyObject d, object value)
		{
			FlipView flipView = d as FlipView;
			if (flipView != null && flipView.allowSelectedIndexChangedCallback)
			{
				flipView.ComputeTransition(flipView.SelectedIndex, (value as int?) ?? flipView.SelectedIndex);
			}
			return value;
		}

		private void ComputeTransition(int fromIndex, int toIndex)
		{
			if (presenter != null)
			{
				if (fromIndex < toIndex)
				{
					presenter.Transition = ((Orientation == Orientation.Horizontal) ? LeftTransition : DownTransition);
				}
				else if (fromIndex > toIndex)
				{
					presenter.Transition = ((Orientation == Orientation.Horizontal) ? RightTransition : UpTransition);
				}
				else
				{
					presenter.Transition = TransitionType.Default;
				}
			}
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is FlipViewItem;
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new FlipViewItem
			{
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			if (element != item)
			{
				element.SetValue(FrameworkElement.DataContextProperty, item);
			}
			base.PrepareContainerForItemOverride(element, item);
		}

		private void GetNavigationButtons(out Button prevButton, out Button nextButton, out IEnumerable<Button> inactiveButtons)
		{
			if (Orientation == Orientation.Horizontal)
			{
				prevButton = backButton;
				nextButton = forwardButton;
				inactiveButtons = new Button[2]
				{
					upButton,
					downButton
				};
			}
			else
			{
				prevButton = upButton;
				nextButton = downButton;
				inactiveButtons = new Button[2]
				{
					backButton,
					forwardButton
				};
			}
		}

		private void ApplyToNavigationButtons(Action<Button> prevButtonApply, Action<Button> nextButtonApply, Action<Button> inactiveButtonsApply)
		{
			if (prevButtonApply == null)
			{
				throw new ArgumentNullException("prevButtonApply");
			}
			if (nextButtonApply == null)
			{
				throw new ArgumentNullException("nextButtonApply");
			}
			if (inactiveButtonsApply == null)
			{
				throw new ArgumentNullException("inactiveButtonsApply");
			}
			Button prevButton = null;
			Button nextButton = null;
			IEnumerable<Button> inactiveButtons = null;
			GetNavigationButtons(out prevButton, out nextButton, out inactiveButtons);
			foreach (Button item in inactiveButtons)
			{
				if (item != null)
				{
					inactiveButtonsApply(item);
				}
			}
			if (prevButton != null)
			{
				prevButtonApply(prevButton);
			}
			if (nextButton != null)
			{
				nextButtonApply(nextButton);
			}
		}

		private void DetectControlButtonsStatus(Visibility activeButtonsVisibility = Visibility.Visible)
		{
			if (!IsNavigationEnabled)
			{
				activeButtonsVisibility = Visibility.Hidden;
			}
			ApplyToNavigationButtons(delegate(Button prev)
			{
				prev.Visibility = ((!CircularNavigation && (base.Items.Count <= 0 || base.SelectedIndex <= 0)) ? Visibility.Hidden : activeButtonsVisibility);
			}, delegate(Button next)
			{
				next.Visibility = ((!CircularNavigation && (base.Items.Count <= 0 || base.SelectedIndex >= base.Items.Count - 1)) ? Visibility.Hidden : activeButtonsVisibility);
			}, delegate(Button inactive)
			{
				inactive.Visibility = Visibility.Hidden;
			});
		}

		private void FlipViewLoaded(object sender, RoutedEventArgs e)
		{
			if (backButton == null || forwardButton == null || upButton == null || downButton == null)
			{
				ApplyTemplate();
			}
			if (!loaded)
			{
				base.Unloaded += FlipViewUnloaded;
				if (base.SelectedIndex < 0)
				{
					base.SelectedIndex = 0;
				}
				DetectControlButtonsStatus();
				ShowBanner();
				loaded = true;
			}
		}

		private void FlipViewUnloaded(object sender, RoutedEventArgs e)
		{
			base.Unloaded -= FlipViewUnloaded;
			if (hideControlStoryboard != null && hideControlStoryboardCompletedHandler != null)
			{
				hideControlStoryboard.Completed -= hideControlStoryboardCompletedHandler;
			}
			loaded = false;
		}

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);
			DetectControlButtonsStatus();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			bool flag = Orientation == Orientation.Horizontal;
			bool flag2 = Orientation == Orientation.Vertical;
			bool num = (e.Key == Key.Left && flag && backButton != null && backButton.Visibility == Visibility.Visible && backButton.IsEnabled) || (e.Key == Key.Up && flag2 && upButton != null && upButton.Visibility == Visibility.Visible && upButton.IsEnabled);
			bool flag3 = (e.Key == Key.Right && flag && forwardButton != null && forwardButton.Visibility == Visibility.Visible && forwardButton.IsEnabled) || (e.Key == Key.Down && flag2 && downButton != null && downButton.Visibility == Visibility.Visible && downButton.IsEnabled);
			if (num)
			{
				GoBack();
				e.Handled = true;
				Focus();
			}
			else if (flag3)
			{
				GoForward();
				e.Handled = true;
				Focus();
			}
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			Focus();
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			showBannerStoryboard = ((Storyboard)base.Template.Resources["ShowBannerStoryboard"]).Clone();
			hideBannerStoryboard = ((Storyboard)base.Template.Resources["HideBannerStoryboard"]).Clone();
			showControlStoryboard = ((Storyboard)base.Template.Resources["ShowControlStoryboard"]).Clone();
			hideControlStoryboard = ((Storyboard)base.Template.Resources["HideControlStoryboard"]).Clone();
			presenter = (GetTemplateChild("PART_Presenter") as TransitioningContentControl);
			if (forwardButton != null)
			{
				forwardButton.Click -= NextButtonClick;
			}
			if (backButton != null)
			{
				backButton.Click -= PrevButtonClick;
			}
			if (upButton != null)
			{
				upButton.Click -= PrevButtonClick;
			}
			if (downButton != null)
			{
				downButton.Click -= NextButtonClick;
			}
			forwardButton = (GetTemplateChild("PART_ForwardButton") as Button);
			backButton = (GetTemplateChild("PART_BackButton") as Button);
			upButton = (GetTemplateChild("PART_UpButton") as Button);
			downButton = (GetTemplateChild("PART_DownButton") as Button);
			bannerGrid = (GetTemplateChild("PART_BannerGrid") as Grid);
			bannerLabel = (GetTemplateChild("PART_BannerLabel") as Label);
			if (forwardButton != null)
			{
				forwardButton.Click += NextButtonClick;
			}
			if (backButton != null)
			{
				backButton.Click += PrevButtonClick;
			}
			if (upButton != null)
			{
				upButton.Click += PrevButtonClick;
			}
			if (downButton != null)
			{
				downButton.Click += NextButtonClick;
			}
			if (bannerLabel != null)
			{
				bannerLabel.Opacity = (IsBannerEnabled ? 1.0 : 0.0);
			}
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			DetectControlButtonsStatus();
		}

		private void NextButtonClick(object sender, RoutedEventArgs e)
		{
			GoForward();
		}

		private void PrevButtonClick(object sender, RoutedEventArgs e)
		{
			GoBack();
		}

		public void GoBack()
		{
			allowSelectedIndexChangedCallback = false;
			presenter.Transition = ((Orientation == Orientation.Horizontal) ? RightTransition : UpTransition);
			if (base.SelectedIndex > 0)
			{
				base.SelectedIndex--;
			}
			else if (CircularNavigation)
			{
				base.SelectedIndex = base.Items.Count - 1;
			}
			allowSelectedIndexChangedCallback = true;
		}

		public void GoForward()
		{
			allowSelectedIndexChangedCallback = false;
			presenter.Transition = ((Orientation == Orientation.Horizontal) ? LeftTransition : DownTransition);
			if (base.SelectedIndex < base.Items.Count - 1)
			{
				base.SelectedIndex++;
			}
			else if (CircularNavigation)
			{
				base.SelectedIndex = 0;
			}
			allowSelectedIndexChangedCallback = true;
		}

		public void ShowControlButtons()
		{
			ExecuteWhenLoaded(this, delegate
			{
				DetectControlButtonsStatus();
			});
		}

		public void HideControlButtons()
		{
			ExecuteWhenLoaded(this, delegate
			{
				DetectControlButtonsStatus(Visibility.Hidden);
			});
		}

		private void ShowBanner()
		{
			if (IsBannerEnabled)
			{
				bannerGrid?.BeginStoryboard(showBannerStoryboard);
			}
		}

		private void HideBanner()
		{
			if (base.ActualHeight > 0.0)
			{
				bannerLabel?.BeginStoryboard(hideControlStoryboard);
				bannerGrid?.BeginStoryboard(hideBannerStoryboard);
			}
		}

		private static void ExecuteWhenLoaded(FlipView flipview, Action body)
		{
			if (flipview.IsLoaded)
			{
				Dispatcher.CurrentDispatcher.Invoke(body);
			}
			else
			{
				RoutedEventHandler handler = null;
				handler = delegate
				{
					flipview.Loaded -= handler;
					Dispatcher.CurrentDispatcher.Invoke(body);
				};
				flipview.Loaded += handler;
			}
		}

		private void ChangeBannerText(string value = null)
		{
			if (IsBannerEnabled)
			{
				string newValue = value ?? BannerText;
				if (newValue != null && hideControlStoryboard != null)
				{
					if (hideControlStoryboardCompletedHandler != null)
					{
						hideControlStoryboard.Completed -= hideControlStoryboardCompletedHandler;
					}
					hideControlStoryboardCompletedHandler = delegate
					{
						try
						{
							hideControlStoryboard.Completed -= hideControlStoryboardCompletedHandler;
							bannerLabel.Content = newValue;
							bannerLabel.BeginStoryboard(showControlStoryboard, HandoffBehavior.SnapshotAndReplace);
						}
						catch (Exception)
						{
						}
					};
					hideControlStoryboard.Completed += hideControlStoryboardCompletedHandler;
					bannerLabel.BeginStoryboard(hideControlStoryboard, HandoffBehavior.SnapshotAndReplace);
				}
			}
			else
			{
				ExecuteWhenLoaded(this, delegate
				{
					bannerLabel.Content = (value ?? BannerText);
				});
			}
		}

		private static void OnIsBannerEnabledPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FlipView flipview = (FlipView)d;
			if (!flipview.IsLoaded)
			{
				ExecuteWhenLoaded(flipview, delegate
				{
					flipview.ApplyTemplate();
					if ((bool)e.NewValue)
					{
						flipview.ChangeBannerText(flipview.BannerText);
						flipview.ShowBanner();
					}
					else
					{
						flipview.HideBanner();
					}
				});
			}
			else if ((bool)e.NewValue)
			{
				flipview.ChangeBannerText(flipview.BannerText);
				flipview.ShowBanner();
			}
			else
			{
				flipview.HideBanner();
			}
		}
	}
}
