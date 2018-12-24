using ControlzEx;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_Root", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_Header", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_Content", Type = typeof(FrameworkElement))]
	public class Flyout : HeaderedContentControl
	{
		public static readonly RoutedEvent IsOpenChangedEvent;

		public static readonly RoutedEvent ClosingFinishedEvent;

		public static readonly DependencyProperty PositionProperty;

		public static readonly DependencyProperty IsPinnedProperty;

		public static readonly DependencyProperty IsOpenProperty;

		public static readonly DependencyProperty AnimateOnPositionChangeProperty;

		public static readonly DependencyProperty AnimateOpacityProperty;

		public static readonly DependencyProperty IsModalProperty;

		public static readonly DependencyProperty CloseCommandProperty;

		public static readonly DependencyProperty CloseCommandParameterProperty;

		[Obsolete("This property will be deleted in the next release. Please use the new CloseFlyoutAction trigger.")]
		internal static readonly DependencyProperty InternalCloseCommandProperty;

		public static readonly DependencyProperty ThemeProperty;

		public static readonly DependencyProperty ExternalCloseButtonProperty;

		public static readonly DependencyProperty CloseButtonVisibilityProperty;

		public static readonly DependencyProperty CloseButtonIsCancelProperty;

		public static readonly DependencyProperty TitleVisibilityProperty;

		public static readonly DependencyProperty AreAnimationsEnabledProperty;

		public static readonly DependencyProperty FocusedElementProperty;

		public static readonly DependencyProperty AllowFocusElementProperty;

		public static readonly DependencyProperty IsAutoCloseEnabledProperty;

		public static readonly DependencyProperty AutoCloseIntervalProperty;

		private MetroWindow parentWindow;

		private DispatcherTimer autoCloseTimer;

		private FrameworkElement flyoutRoot;

		private Storyboard hideStoryboard;

		private SplineDoubleKeyFrame hideFrame;

		private SplineDoubleKeyFrame hideFrameY;

		private SplineDoubleKeyFrame showFrame;

		private SplineDoubleKeyFrame showFrameY;

		private SplineDoubleKeyFrame fadeOutFrame;

		private FrameworkElement flyoutHeader;

		private FrameworkElement flyoutContent;

		private Point? dragStartedMousePos;

		internal PropertyChangeNotifier IsOpenPropertyChangeNotifier
		{
			get;
			set;
		}

		internal PropertyChangeNotifier ThemePropertyChangeNotifier
		{
			get;
			set;
		}

		public bool AreAnimationsEnabled
		{
			get
			{
				return (bool)GetValue(AreAnimationsEnabledProperty);
			}
			set
			{
				SetValue(AreAnimationsEnabledProperty, value);
			}
		}

		public Visibility TitleVisibility
		{
			get
			{
				return (Visibility)GetValue(TitleVisibilityProperty);
			}
			set
			{
				SetValue(TitleVisibilityProperty, value);
			}
		}

		public Visibility CloseButtonVisibility
		{
			get
			{
				return (Visibility)GetValue(CloseButtonVisibilityProperty);
			}
			set
			{
				SetValue(CloseButtonVisibilityProperty, value);
			}
		}

		public bool CloseButtonIsCancel
		{
			get
			{
				return (bool)GetValue(CloseButtonIsCancelProperty);
			}
			set
			{
				SetValue(CloseButtonIsCancelProperty, value);
			}
		}

		public ICommand CloseCommand
		{
			get
			{
				return (ICommand)GetValue(CloseCommandProperty);
			}
			set
			{
				SetValue(CloseCommandProperty, value);
			}
		}

		public object CloseCommandParameter
		{
			get
			{
				return GetValue(CloseCommandParameterProperty);
			}
			set
			{
				SetValue(CloseCommandParameterProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. Please use the new CloseFlyoutAction trigger.")]
		internal ICommand InternalCloseCommand
		{
			get
			{
				return (ICommand)GetValue(InternalCloseCommandProperty);
			}
			set
			{
				SetValue(InternalCloseCommandProperty, value);
			}
		}

		public bool IsOpen
		{
			get
			{
				return (bool)GetValue(IsOpenProperty);
			}
			set
			{
				SetValue(IsOpenProperty, value);
			}
		}

		public bool AnimateOnPositionChange
		{
			get
			{
				return (bool)GetValue(AnimateOnPositionChangeProperty);
			}
			set
			{
				SetValue(AnimateOnPositionChangeProperty, value);
			}
		}

		public bool AnimateOpacity
		{
			get
			{
				return (bool)GetValue(AnimateOpacityProperty);
			}
			set
			{
				SetValue(AnimateOpacityProperty, value);
			}
		}

		public bool IsPinned
		{
			get
			{
				return (bool)GetValue(IsPinnedProperty);
			}
			set
			{
				SetValue(IsPinnedProperty, value);
			}
		}

		public MouseButton ExternalCloseButton
		{
			get
			{
				return (MouseButton)GetValue(ExternalCloseButtonProperty);
			}
			set
			{
				SetValue(ExternalCloseButtonProperty, value);
			}
		}

		public bool IsModal
		{
			get
			{
				return (bool)GetValue(IsModalProperty);
			}
			set
			{
				SetValue(IsModalProperty, value);
			}
		}

		public Position Position
		{
			get
			{
				return (Position)GetValue(PositionProperty);
			}
			set
			{
				SetValue(PositionProperty, value);
			}
		}

		public FlyoutTheme Theme
		{
			get
			{
				return (FlyoutTheme)GetValue(ThemeProperty);
			}
			set
			{
				SetValue(ThemeProperty, value);
			}
		}

		public FrameworkElement FocusedElement
		{
			get
			{
				return (FrameworkElement)GetValue(FocusedElementProperty);
			}
			set
			{
				SetValue(FocusedElementProperty, value);
			}
		}

		public bool IsAutoCloseEnabled
		{
			get
			{
				return (bool)GetValue(IsAutoCloseEnabledProperty);
			}
			set
			{
				SetValue(IsAutoCloseEnabledProperty, value);
			}
		}

		public long AutoCloseInterval
		{
			get
			{
				return (long)GetValue(AutoCloseIntervalProperty);
			}
			set
			{
				SetValue(AutoCloseIntervalProperty, value);
			}
		}

		public bool AllowFocusElement
		{
			get
			{
				return (bool)GetValue(AllowFocusElementProperty);
			}
			set
			{
				SetValue(AllowFocusElementProperty, value);
			}
		}

		private MetroWindow ParentWindow => parentWindow ?? (parentWindow = this.TryFindParent<MetroWindow>());

		public event RoutedEventHandler IsOpenChanged
		{
			add
			{
				AddHandler(IsOpenChangedEvent, value);
			}
			remove
			{
				RemoveHandler(IsOpenChangedEvent, value);
			}
		}

		public event RoutedEventHandler ClosingFinished
		{
			add
			{
				AddHandler(ClosingFinishedEvent, value);
			}
			remove
			{
				RemoveHandler(ClosingFinishedEvent, value);
			}
		}

		static Flyout()
		{
			IsOpenChangedEvent = EventManager.RegisterRoutedEvent("IsOpenChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Flyout));
			ClosingFinishedEvent = EventManager.RegisterRoutedEvent("ClosingFinished", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Flyout));
			PositionProperty = DependencyProperty.Register("Position", typeof(Position), typeof(Flyout), new PropertyMetadata(Position.Left, PositionChanged));
			IsPinnedProperty = DependencyProperty.Register("IsPinned", typeof(bool), typeof(Flyout), new PropertyMetadata(true));
			IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(Flyout), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsOpenedChanged));
			AnimateOnPositionChangeProperty = DependencyProperty.Register("AnimateOnPositionChange", typeof(bool), typeof(Flyout), new PropertyMetadata(true));
			AnimateOpacityProperty = DependencyProperty.Register("AnimateOpacity", typeof(bool), typeof(Flyout), new FrameworkPropertyMetadata(false, AnimateOpacityChanged));
			IsModalProperty = DependencyProperty.Register("IsModal", typeof(bool), typeof(Flyout));
			CloseCommandProperty = DependencyProperty.RegisterAttached("CloseCommand", typeof(ICommand), typeof(Flyout), new UIPropertyMetadata(null));
			CloseCommandParameterProperty = DependencyProperty.Register("CloseCommandParameter", typeof(object), typeof(Flyout), new PropertyMetadata(null));
			InternalCloseCommandProperty = DependencyProperty.Register("InternalCloseCommand", typeof(ICommand), typeof(Flyout));
			ThemeProperty = DependencyProperty.Register("Theme", typeof(FlyoutTheme), typeof(Flyout), new FrameworkPropertyMetadata(FlyoutTheme.Dark, ThemeChanged));
			ExternalCloseButtonProperty = DependencyProperty.Register("ExternalCloseButton", typeof(MouseButton), typeof(Flyout), new PropertyMetadata(MouseButton.Left));
			CloseButtonVisibilityProperty = DependencyProperty.Register("CloseButtonVisibility", typeof(Visibility), typeof(Flyout), new FrameworkPropertyMetadata(Visibility.Visible));
			CloseButtonIsCancelProperty = DependencyProperty.Register("CloseButtonIsCancel", typeof(bool), typeof(Flyout), new FrameworkPropertyMetadata(false));
			TitleVisibilityProperty = DependencyProperty.Register("TitleVisibility", typeof(Visibility), typeof(Flyout), new FrameworkPropertyMetadata(Visibility.Visible));
			AreAnimationsEnabledProperty = DependencyProperty.Register("AreAnimationsEnabled", typeof(bool), typeof(Flyout), new PropertyMetadata(true));
			FocusedElementProperty = DependencyProperty.Register("FocusedElement", typeof(FrameworkElement), typeof(Flyout), new UIPropertyMetadata(null));
			AllowFocusElementProperty = DependencyProperty.Register("AllowFocusElement", typeof(bool), typeof(Flyout), new PropertyMetadata(true));
			IsAutoCloseEnabledProperty = DependencyProperty.Register("IsAutoCloseEnabled", typeof(bool), typeof(Flyout), new FrameworkPropertyMetadata(false, IsAutoCloseEnabledChanged));
			AutoCloseIntervalProperty = DependencyProperty.Register("AutoCloseInterval", typeof(long), typeof(Flyout), new FrameworkPropertyMetadata(5000L, AutoCloseIntervalChanged));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Flyout), new FrameworkPropertyMetadata(typeof(Flyout)));
		}

		public Flyout()
		{
			InternalCloseCommand = new CloseCommand(InternalCloseCommandCanExecute, InternalCloseCommandExecuteAction);
			base.Loaded += delegate
			{
				UpdateFlyoutTheme();
			};
			InitializeAutoCloseTimer();
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new FlyoutAutomationPeer(this);
		}

		private void InternalCloseCommandExecuteAction(object o)
		{
			ICommand closeCommand = CloseCommand;
			if (closeCommand == null)
			{
				SetCurrentValue(IsOpenProperty, false);
			}
			else
			{
				object parameter = CloseCommandParameter ?? this;
				if (closeCommand.CanExecute(parameter))
				{
					closeCommand.Execute(parameter);
				}
			}
		}

		private bool InternalCloseCommandCanExecute(object o)
		{
			return CloseCommand?.CanExecute(CloseCommandParameter ?? this) ?? true;
		}

		private void InitializeAutoCloseTimer()
		{
			StopAutoCloseTimer();
			autoCloseTimer = new DispatcherTimer();
			autoCloseTimer.Tick += AutoCloseTimerCallback;
			autoCloseTimer.Interval = TimeSpan.FromMilliseconds((double)AutoCloseInterval);
		}

		private void UpdateFlyoutTheme()
		{
			FlyoutsControl flyoutsControl = this.TryFindParent<FlyoutsControl>();
			if (DesignerProperties.GetIsInDesignMode(this))
			{
				base.Visibility = ((flyoutsControl != null) ? Visibility.Collapsed : Visibility.Visible);
			}
			MetroWindow metroWindow = ParentWindow;
			if (metroWindow != null)
			{
				Tuple<AppTheme, Accent> tuple = DetectTheme(this);
				if (((tuple != null) ? tuple.Item2 : null) != null)
				{
					Accent item = tuple.Item2;
					ChangeFlyoutTheme(item, tuple.Item1);
				}
				if (flyoutsControl != null && IsOpen)
				{
					flyoutsControl.HandleFlyoutStatusChange(this, metroWindow);
				}
			}
		}

		internal void ChangeFlyoutTheme(Accent windowAccent, AppTheme windowTheme)
		{
			switch (Theme)
			{
			case FlyoutTheme.Accent:
				ThemeManager.ChangeAppStyle(base.Resources, windowAccent, windowTheme);
				OverrideFlyoutResources(base.Resources, accent: true);
				break;
			case FlyoutTheme.Adapt:
				ThemeManager.ChangeAppStyle(base.Resources, windowAccent, windowTheme);
				OverrideFlyoutResources(base.Resources);
				break;
			case FlyoutTheme.Inverse:
			{
				AppTheme inverseAppTheme = ThemeManager.GetInverseAppTheme(windowTheme);
				if (inverseAppTheme == null)
				{
					throw new InvalidOperationException("The inverse flyout theme only works if the window theme abides the naming convention. See ThemeManager.GetInverseAppTheme for more infos");
				}
				ThemeManager.ChangeAppStyle(base.Resources, windowAccent, inverseAppTheme);
				OverrideFlyoutResources(base.Resources);
				break;
			}
			case FlyoutTheme.Dark:
				ThemeManager.ChangeAppStyle(base.Resources, windowAccent, ThemeManager.GetAppTheme("BaseDark"));
				OverrideFlyoutResources(base.Resources);
				break;
			case FlyoutTheme.Light:
				ThemeManager.ChangeAppStyle(base.Resources, windowAccent, ThemeManager.GetAppTheme("BaseLight"));
				OverrideFlyoutResources(base.Resources);
				break;
			}
		}

		private void OverrideFlyoutResources(ResourceDictionary resources, bool accent = false)
		{
			string key = accent ? "HighlightColor" : "FlyoutColor";
			resources.BeginInit();
			Color color = (Color)resources[key];
			resources["WhiteColor"] = color;
			resources["FlyoutColor"] = color;
			SolidColorBrush solidColorBrush = new SolidColorBrush(color);
			solidColorBrush.Freeze();
			resources["FlyoutBackgroundBrush"] = solidColorBrush;
			resources["ControlBackgroundBrush"] = solidColorBrush;
			resources["WhiteBrush"] = solidColorBrush;
			resources["WhiteColorBrush"] = solidColorBrush;
			resources["DisabledWhiteBrush"] = solidColorBrush;
			resources["WindowBackgroundBrush"] = solidColorBrush;
			resources[SystemColors.WindowBrushKey] = solidColorBrush;
			if (accent)
			{
				color = (Color)resources["IdealForegroundColor"];
				solidColorBrush = new SolidColorBrush(color);
				solidColorBrush.Freeze();
				resources["FlyoutForegroundBrush"] = solidColorBrush;
				resources["TextBrush"] = solidColorBrush;
				resources["LabelTextBrush"] = solidColorBrush;
				if (resources.Contains("AccentBaseColor"))
				{
					color = (Color)resources["AccentBaseColor"];
				}
				else
				{
					Color color2 = (Color)resources["AccentColor"];
					color = Color.FromArgb(byte.MaxValue, color2.R, color2.G, color2.B);
				}
				solidColorBrush = new SolidColorBrush(color);
				solidColorBrush.Freeze();
				resources["HighlightColor"] = color;
				resources["HighlightBrush"] = solidColorBrush;
			}
			resources.EndInit();
		}

		private static Tuple<AppTheme, Accent> DetectTheme(Flyout flyout)
		{
			if (flyout == null)
			{
				return null;
			}
			MetroWindow metroWindow = flyout.ParentWindow;
			Tuple<AppTheme, Accent> tuple = (metroWindow != null) ? ThemeManager.DetectAppStyle(metroWindow) : null;
			if (((tuple != null) ? tuple.Item2 : null) != null)
			{
				return tuple;
			}
			if (Application.Current != null)
			{
				MetroWindow metroWindow2 = Application.Current.MainWindow as MetroWindow;
				tuple = ((metroWindow2 != null) ? ThemeManager.DetectAppStyle(metroWindow2) : null);
				if (((tuple != null) ? tuple.Item2 : null) != null)
				{
					return tuple;
				}
				tuple = ThemeManager.DetectAppStyle(Application.Current);
				if (((tuple != null) ? tuple.Item2 : null) != null)
				{
					return tuple;
				}
			}
			return null;
		}

		private void UpdateOpacityChange()
		{
			if (flyoutRoot != null && fadeOutFrame != null && !DesignerProperties.GetIsInDesignMode(this))
			{
				if (!AnimateOpacity)
				{
					fadeOutFrame.Value = 1.0;
					flyoutRoot.Opacity = 1.0;
				}
				else
				{
					fadeOutFrame.Value = 0.0;
					if (!IsOpen)
					{
						flyoutRoot.Opacity = 0.0;
					}
				}
			}
		}

		private static void IsOpenedChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Flyout flyout = (Flyout)dependencyObject;
			Action method = delegate
			{
				if (e.NewValue != e.OldValue)
				{
					if (flyout.AreAnimationsEnabled)
					{
						if ((bool)e.NewValue)
						{
							if (flyout.hideStoryboard != null)
							{
								flyout.hideStoryboard.Completed -= flyout.HideStoryboardCompleted;
							}
							flyout.Visibility = Visibility.Visible;
							flyout.ApplyAnimation(flyout.Position, flyout.AnimateOpacity);
							flyout.TryFocusElement();
							if (flyout.IsAutoCloseEnabled)
							{
								flyout.StartAutoCloseTimer();
							}
						}
						else
						{
							flyout.StopAutoCloseTimer();
							if (flyout.hideStoryboard != null)
							{
								flyout.hideStoryboard.Completed += flyout.HideStoryboardCompleted;
							}
							else
							{
								flyout.Hide();
							}
						}
						VisualStateManager.GoToState(flyout, (!(bool)e.NewValue) ? "Hide" : "Show", useTransitions: true);
					}
					else
					{
						if ((bool)e.NewValue)
						{
							flyout.Visibility = Visibility.Visible;
							flyout.TryFocusElement();
							if (flyout.IsAutoCloseEnabled)
							{
								flyout.StartAutoCloseTimer();
							}
						}
						else
						{
							flyout.StopAutoCloseTimer();
							flyout.Hide();
						}
						VisualStateManager.GoToState(flyout, (!(bool)e.NewValue) ? "HideDirect" : "ShowDirect", useTransitions: true);
					}
				}
				flyout.RaiseEvent(new RoutedEventArgs(IsOpenChangedEvent));
			};
			flyout.Dispatcher.BeginInvoke(DispatcherPriority.Background, method);
		}

		private static void IsAutoCloseEnabledChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Flyout flyout = (Flyout)dependencyObject;
			Action method = delegate
			{
				if (e.NewValue != e.OldValue)
				{
					if ((bool)e.NewValue)
					{
						if (flyout.IsOpen)
						{
							flyout.StartAutoCloseTimer();
						}
					}
					else
					{
						flyout.StopAutoCloseTimer();
					}
				}
			};
			flyout.Dispatcher.BeginInvoke(DispatcherPriority.Background, method);
		}

		private static void AutoCloseIntervalChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Flyout flyout = (Flyout)dependencyObject;
			Action method = delegate
			{
				if (e.NewValue != e.OldValue)
				{
					flyout.InitializeAutoCloseTimer();
					if (flyout.IsAutoCloseEnabled && flyout.IsOpen)
					{
						flyout.StartAutoCloseTimer();
					}
				}
			};
			flyout.Dispatcher.BeginInvoke(DispatcherPriority.Background, method);
		}

		private void StartAutoCloseTimer()
		{
			StopAutoCloseTimer();
			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				autoCloseTimer.Start();
			}
		}

		private void StopAutoCloseTimer()
		{
			if (autoCloseTimer != null && autoCloseTimer.IsEnabled)
			{
				autoCloseTimer.Stop();
			}
		}

		private void AutoCloseTimerCallback(object sender, EventArgs e)
		{
			StopAutoCloseTimer();
			if (IsOpen && IsAutoCloseEnabled)
			{
				IsOpen = false;
			}
		}

		private void HideStoryboardCompleted(object sender, EventArgs e)
		{
			hideStoryboard.Completed -= HideStoryboardCompleted;
			Hide();
		}

		private void Hide()
		{
			base.Visibility = Visibility.Hidden;
			RaiseEvent(new RoutedEventArgs(ClosingFinishedEvent));
		}

		private void TryFocusElement()
		{
			if (AllowFocusElement)
			{
				Focus();
				if (FocusedElement != null)
				{
					FocusedElement.Focus();
				}
				else if (flyoutContent == null || !flyoutContent.MoveFocus(new TraversalRequest(FocusNavigationDirection.First)))
				{
					flyoutHeader?.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
				}
			}
		}

		private static void ThemeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			((Flyout)dependencyObject).UpdateFlyoutTheme();
		}

		private static void AnimateOpacityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			((Flyout)dependencyObject).UpdateOpacityChange();
		}

		private static void PositionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			Flyout flyout = (Flyout)dependencyObject;
			bool isOpen = flyout.IsOpen;
			if (isOpen && flyout.AnimateOnPositionChange)
			{
				flyout.ApplyAnimation((Position)e.NewValue, flyout.AnimateOpacity);
				VisualStateManager.GoToState(flyout, "Hide", useTransitions: true);
			}
			else
			{
				flyout.ApplyAnimation((Position)e.NewValue, flyout.AnimateOpacity, resetShowFrame: false);
			}
			if (isOpen && flyout.AnimateOnPositionChange)
			{
				flyout.ApplyAnimation((Position)e.NewValue, flyout.AnimateOpacity);
				VisualStateManager.GoToState(flyout, "Show", useTransitions: true);
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			flyoutRoot = (GetTemplateChild("PART_Root") as FrameworkElement);
			if (flyoutRoot != null)
			{
				flyoutHeader = (GetTemplateChild("PART_Header") as FrameworkElement);
				flyoutHeader?.ApplyTemplate();
				flyoutContent = (GetTemplateChild("PART_Content") as FrameworkElement);
				IMetroThumb metroThumb = flyoutHeader as IMetroThumb;
				if (metroThumb != null)
				{
					metroThumb.DragStarted -= WindowTitleThumbOnDragStarted;
					metroThumb.DragCompleted -= WindowTitleThumbOnDragCompleted;
					metroThumb.PreviewMouseLeftButtonUp -= WindowTitleThumbOnPreviewMouseLeftButtonUp;
					metroThumb.DragDelta -= WindowTitleThumbMoveOnDragDelta;
					metroThumb.MouseDoubleClick -= WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
					metroThumb.MouseRightButtonUp -= WindowTitleThumbSystemMenuOnMouseRightButtonUp;
					if (this.TryFindParent<FlyoutsControl>() != null)
					{
						metroThumb.DragStarted += WindowTitleThumbOnDragStarted;
						metroThumb.DragCompleted += WindowTitleThumbOnDragCompleted;
						metroThumb.PreviewMouseLeftButtonUp += WindowTitleThumbOnPreviewMouseLeftButtonUp;
						metroThumb.DragDelta += WindowTitleThumbMoveOnDragDelta;
						metroThumb.MouseDoubleClick += WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
						metroThumb.MouseRightButtonUp += WindowTitleThumbSystemMenuOnMouseRightButtonUp;
					}
				}
				hideStoryboard = (GetTemplateChild("HideStoryboard") as Storyboard);
				hideFrame = (GetTemplateChild("hideFrame") as SplineDoubleKeyFrame);
				hideFrameY = (GetTemplateChild("hideFrameY") as SplineDoubleKeyFrame);
				showFrame = (GetTemplateChild("showFrame") as SplineDoubleKeyFrame);
				showFrameY = (GetTemplateChild("showFrameY") as SplineDoubleKeyFrame);
				fadeOutFrame = (GetTemplateChild("fadeOutFrame") as SplineDoubleKeyFrame);
				if (hideFrame != null && showFrame != null && hideFrameY != null && showFrameY != null && fadeOutFrame != null)
				{
					ApplyAnimation(Position, AnimateOpacity);
				}
			}
		}

		private void WindowTitleThumbOnDragCompleted(object sender, DragCompletedEventArgs e)
		{
			dragStartedMousePos = null;
		}

		private void WindowTitleThumbOnDragStarted(object sender, DragStartedEventArgs e)
		{
			if (ParentWindow != null && Position != Position.Bottom)
			{
				dragStartedMousePos = Mouse.GetPosition((IInputElement)sender);
			}
			else
			{
				dragStartedMousePos = null;
			}
		}

		protected internal void CleanUp(FlyoutsControl flyoutsControl)
		{
			IMetroThumb metroThumb = flyoutHeader as IMetroThumb;
			if (metroThumb != null)
			{
				metroThumb.DragStarted -= WindowTitleThumbOnDragStarted;
				metroThumb.DragCompleted -= WindowTitleThumbOnDragCompleted;
				metroThumb.PreviewMouseLeftButtonUp -= WindowTitleThumbOnPreviewMouseLeftButtonUp;
				metroThumb.DragDelta -= WindowTitleThumbMoveOnDragDelta;
				metroThumb.MouseDoubleClick -= WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
				metroThumb.MouseRightButtonUp -= WindowTitleThumbSystemMenuOnMouseRightButtonUp;
			}
			parentWindow = null;
		}

		private void WindowTitleThumbOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			MetroWindow metroWindow = ParentWindow;
			if (metroWindow != null && Position != Position.Bottom)
			{
				MetroWindow.DoWindowTitleThumbOnPreviewMouseLeftButtonUp(metroWindow, e);
			}
			dragStartedMousePos = null;
		}

		private void WindowTitleThumbMoveOnDragDelta(object sender, DragDeltaEventArgs dragDeltaEventArgs)
		{
			MetroWindow metroWindow = ParentWindow;
			if (metroWindow != null && Position != Position.Bottom)
			{
				MetroWindow.DoWindowTitleThumbMoveOnDragDelta(sender as IMetroThumb, metroWindow, dragDeltaEventArgs);
			}
		}

		private void WindowTitleThumbChangeWindowStateOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			MetroWindow metroWindow = ParentWindow;
			if (metroWindow != null && Position != Position.Bottom && Mouse.GetPosition((IInputElement)sender).Y <= (double)metroWindow.TitlebarHeight && metroWindow.TitlebarHeight > 0)
			{
				MetroWindow.DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick(metroWindow, mouseButtonEventArgs);
			}
		}

		private void WindowTitleThumbSystemMenuOnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			MetroWindow metroWindow = ParentWindow;
			if (metroWindow != null && Position != Position.Bottom && Mouse.GetPosition((IInputElement)sender).Y <= (double)metroWindow.TitlebarHeight && metroWindow.TitlebarHeight > 0)
			{
				MetroWindow.DoWindowTitleThumbSystemMenuOnMouseRightButtonUp(metroWindow, e);
			}
		}

		internal void ApplyAnimation(Position position, bool animateOpacity, bool resetShowFrame = true)
		{
			if (flyoutRoot != null && hideFrame != null && showFrame != null && hideFrameY != null && showFrameY != null && fadeOutFrame != null)
			{
				if (Position == Position.Left || Position == Position.Right)
				{
					showFrame.Value = 0.0;
				}
				if (Position == Position.Top || Position == Position.Bottom)
				{
					showFrameY.Value = 0.0;
				}
				if (!animateOpacity)
				{
					fadeOutFrame.Value = 1.0;
					flyoutRoot.Opacity = 1.0;
				}
				else
				{
					fadeOutFrame.Value = 0.0;
					if (!IsOpen)
					{
						flyoutRoot.Opacity = 0.0;
					}
				}
				switch (position)
				{
				default:
					base.HorizontalAlignment = ((!(base.Margin.Right <= 0.0)) ? HorizontalAlignment.Stretch : ((base.HorizontalContentAlignment == HorizontalAlignment.Stretch) ? base.HorizontalContentAlignment : HorizontalAlignment.Left));
					base.VerticalAlignment = VerticalAlignment.Stretch;
					hideFrame.Value = 0.0 - flyoutRoot.ActualWidth - base.Margin.Left;
					if (resetShowFrame)
					{
						flyoutRoot.RenderTransform = new TranslateTransform(0.0 - flyoutRoot.ActualWidth, 0.0);
					}
					break;
				case Position.Right:
					base.HorizontalAlignment = ((!(base.Margin.Left <= 0.0)) ? HorizontalAlignment.Stretch : ((base.HorizontalContentAlignment != HorizontalAlignment.Stretch) ? HorizontalAlignment.Right : base.HorizontalContentAlignment));
					base.VerticalAlignment = VerticalAlignment.Stretch;
					hideFrame.Value = flyoutRoot.ActualWidth + base.Margin.Right;
					if (resetShowFrame)
					{
						flyoutRoot.RenderTransform = new TranslateTransform(flyoutRoot.ActualWidth, 0.0);
					}
					break;
				case Position.Top:
					base.HorizontalAlignment = HorizontalAlignment.Stretch;
					base.VerticalAlignment = ((!(base.Margin.Bottom <= 0.0)) ? VerticalAlignment.Stretch : ((base.VerticalContentAlignment == VerticalAlignment.Stretch) ? base.VerticalContentAlignment : VerticalAlignment.Top));
					hideFrameY.Value = 0.0 - flyoutRoot.ActualHeight - 1.0 - base.Margin.Top;
					if (resetShowFrame)
					{
						flyoutRoot.RenderTransform = new TranslateTransform(0.0, 0.0 - flyoutRoot.ActualHeight - 1.0);
					}
					break;
				case Position.Bottom:
					base.HorizontalAlignment = HorizontalAlignment.Stretch;
					base.VerticalAlignment = ((!(base.Margin.Top <= 0.0)) ? VerticalAlignment.Stretch : ((base.VerticalContentAlignment != VerticalAlignment.Stretch) ? VerticalAlignment.Bottom : base.VerticalContentAlignment));
					hideFrameY.Value = flyoutRoot.ActualHeight + base.Margin.Bottom;
					if (resetShowFrame)
					{
						flyoutRoot.RenderTransform = new TranslateTransform(0.0, flyoutRoot.ActualHeight);
					}
					break;
				}
			}
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			if (IsOpen && (sizeInfo.WidthChanged || sizeInfo.HeightChanged) && flyoutRoot != null && hideFrame != null && showFrame != null && hideFrameY != null && showFrameY != null)
			{
				if (Position == Position.Left || Position == Position.Right)
				{
					showFrame.Value = 0.0;
				}
				if (Position == Position.Top || Position == Position.Bottom)
				{
					showFrameY.Value = 0.0;
				}
				switch (Position)
				{
				default:
					hideFrame.Value = 0.0 - flyoutRoot.ActualWidth - base.Margin.Left;
					break;
				case Position.Right:
					hideFrame.Value = flyoutRoot.ActualWidth + base.Margin.Right;
					break;
				case Position.Top:
					hideFrameY.Value = 0.0 - flyoutRoot.ActualHeight - 1.0 - base.Margin.Top;
					break;
				case Position.Bottom:
					hideFrameY.Value = flyoutRoot.ActualHeight + base.Margin.Bottom;
					break;
				}
			}
		}
	}
}
