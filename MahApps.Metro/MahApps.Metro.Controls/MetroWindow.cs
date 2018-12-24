using ControlzEx.Behaviors;
using ControlzEx.Native;
using ControlzEx.Standard;
using ControlzEx.Windows.Shell;
using MahApps.Metro.Behaviours;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_Icon", Type = typeof(UIElement))]
	[TemplatePart(Name = "PART_TitleBar", Type = typeof(UIElement))]
	[TemplatePart(Name = "PART_WindowTitleBackground", Type = typeof(UIElement))]
	[TemplatePart(Name = "PART_WindowTitleThumb", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_FlyoutModalDragMoveThumb", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_LeftWindowCommands", Type = typeof(WindowCommands))]
	[TemplatePart(Name = "PART_RightWindowCommands", Type = typeof(WindowCommands))]
	[TemplatePart(Name = "PART_WindowButtonCommands", Type = typeof(WindowButtonCommands))]
	[TemplatePart(Name = "PART_OverlayBox", Type = typeof(Grid))]
	[TemplatePart(Name = "PART_MetroActiveDialogContainer", Type = typeof(Grid))]
	[TemplatePart(Name = "PART_MetroInactiveDialogsContainer", Type = typeof(Grid))]
	[TemplatePart(Name = "PART_FlyoutModal", Type = typeof(Rectangle))]
	public class MetroWindow : Window
	{
		public class FlyoutStatusChangedRoutedEventArgs : RoutedEventArgs
		{
			public Flyout ChangedFlyout
			{
				get;
				internal set;
			}

			internal FlyoutStatusChangedRoutedEventArgs(RoutedEvent rEvent, object source)
				: base(rEvent, source)
			{
			}
		}

		private const string PART_Icon = "PART_Icon";

		private const string PART_TitleBar = "PART_TitleBar";

		private const string PART_WindowTitleBackground = "PART_WindowTitleBackground";

		private const string PART_WindowTitleThumb = "PART_WindowTitleThumb";

		private const string PART_FlyoutModalDragMoveThumb = "PART_FlyoutModalDragMoveThumb";

		private const string PART_LeftWindowCommands = "PART_LeftWindowCommands";

		private const string PART_RightWindowCommands = "PART_RightWindowCommands";

		private const string PART_WindowButtonCommands = "PART_WindowButtonCommands";

		private const string PART_OverlayBox = "PART_OverlayBox";

		private const string PART_MetroActiveDialogContainer = "PART_MetroActiveDialogContainer";

		private const string PART_MetroInactiveDialogsContainer = "PART_MetroInactiveDialogsContainer";

		private const string PART_FlyoutModal = "PART_FlyoutModal";

		private const string PART_Content = "PART_Content";

		public static readonly DependencyProperty ShowIconOnTitleBarProperty;

		public static readonly DependencyProperty IconEdgeModeProperty;

		public static readonly DependencyProperty IconBitmapScalingModeProperty;

		public static readonly DependencyProperty IconScalingModeProperty;

		public static readonly DependencyProperty ShowTitleBarProperty;

		public static readonly DependencyProperty ShowDialogsOverTitleBarProperty;

		public static readonly DependencyPropertyKey IsAnyDialogOpenPropertyKey;

		public static readonly DependencyProperty IsAnyDialogOpenProperty;

		public static readonly DependencyProperty ShowMinButtonProperty;

		public static readonly DependencyProperty ShowMaxRestoreButtonProperty;

		public static readonly DependencyProperty ShowCloseButtonProperty;

		public static readonly DependencyProperty IsMinButtonEnabledProperty;

		public static readonly DependencyProperty IsMaxRestoreButtonEnabledProperty;

		public static readonly DependencyProperty IsCloseButtonEnabledProperty;

		public static readonly DependencyPropertyKey IsCloseButtonEnabledWithDialogPropertyKey;

		public static readonly DependencyProperty IsCloseButtonEnabledWithDialogProperty;

		public static readonly DependencyProperty ShowSystemMenuOnRightClickProperty;

		public static readonly DependencyProperty TitlebarHeightProperty;

		[Obsolete("This property will be deleted in the next release. You should use the new TitleCharacterCasing dependency property.")]
		public static readonly DependencyProperty TitleCapsProperty;

		public static readonly DependencyProperty TitleCharacterCasingProperty;

		public static readonly DependencyProperty TitleAlignmentProperty;

		public static readonly DependencyProperty SaveWindowPositionProperty;

		public static readonly DependencyProperty WindowPlacementSettingsProperty;

		public static readonly DependencyProperty TitleForegroundProperty;

		public static readonly DependencyProperty IgnoreTaskbarOnMaximizeProperty;

		public static readonly DependencyProperty FlyoutsProperty;

		public static readonly DependencyProperty WindowTransitionsEnabledProperty;

		public static readonly DependencyProperty MetroDialogOptionsProperty;

		public static readonly DependencyProperty WindowTitleBrushProperty;

		public static readonly DependencyProperty NonActiveWindowTitleBrushProperty;

		public static readonly DependencyProperty NonActiveBorderBrushProperty;

		public static readonly DependencyProperty GlowBrushProperty;

		public static readonly DependencyProperty NonActiveGlowBrushProperty;

		public static readonly DependencyProperty OverlayBrushProperty;

		public static readonly DependencyProperty OverlayOpacityProperty;

		public static readonly DependencyProperty OverlayFadeInProperty;

		public static readonly DependencyProperty OverlayFadeOutProperty;

		public static readonly DependencyProperty IconTemplateProperty;

		public static readonly DependencyProperty TitleTemplateProperty;

		public static readonly DependencyProperty LeftWindowCommandsProperty;

		public static readonly DependencyProperty RightWindowCommandsProperty;

		public static readonly DependencyProperty WindowButtonCommandsProperty;

		public static readonly DependencyProperty LeftWindowCommandsOverlayBehaviorProperty;

		public static readonly DependencyProperty RightWindowCommandsOverlayBehaviorProperty;

		public static readonly DependencyProperty WindowButtonCommandsOverlayBehaviorProperty;

		public static readonly DependencyProperty IconOverlayBehaviorProperty;

		[Obsolete("This property will be deleted in the next release. You should use LightMinButtonStyle or DarkMinButtonStyle in WindowButtonCommands to override the style.")]
		public static readonly DependencyProperty WindowMinButtonStyleProperty;

		[Obsolete("This property will be deleted in the next release. You should use LightMaxButtonStyle or DarkMaxButtonStyle in WindowButtonCommands to override the style.")]
		public static readonly DependencyProperty WindowMaxButtonStyleProperty;

		[Obsolete("This property will be deleted in the next release. You should use LightCloseButtonStyle or DarkCloseButtonStyle in WindowButtonCommands to override the style.")]
		public static readonly DependencyProperty WindowCloseButtonStyleProperty;

		public static readonly DependencyProperty UseNoneWindowStyleProperty;

		public static readonly DependencyProperty OverrideDefaultWindowCommandsBrushProperty;

		[Obsolete("This property will be deleted in the next release. You should use BorderThickness=\"0\" and a GlowBrush=\"Black\" to get a drop shadow around the Window.")]
		public static readonly DependencyProperty EnableDWMDropShadowProperty;

		public static readonly DependencyProperty IsWindowDraggableProperty;

		private FrameworkElement icon;

		private UIElement titleBar;

		private UIElement titleBarBackground;

		private Thumb windowTitleThumb;

		private Thumb flyoutModalDragMoveThumb;

		private IInputElement restoreFocus;

		internal ContentPresenter LeftWindowCommandsPresenter;

		internal ContentPresenter RightWindowCommandsPresenter;

		internal ContentPresenter WindowButtonCommandsPresenter;

		internal Grid overlayBox;

		internal Grid metroActiveDialogContainer;

		internal Grid metroInactiveDialogContainer;

		private Storyboard overlayStoryboard;

		private Rectangle flyoutModal;

		public static readonly RoutedEvent FlyoutsStatusChangedEvent;

		public static readonly RoutedEvent WindowTransitionCompletedEvent;

		public static readonly DependencyProperty ResizeBorderThicknessProperty;

		public SolidColorBrush OverrideDefaultWindowCommandsBrush
		{
			get
			{
				return (SolidColorBrush)GetValue(OverrideDefaultWindowCommandsBrushProperty);
			}
			set
			{
				SetValue(OverrideDefaultWindowCommandsBrushProperty, value);
			}
		}

		public MetroDialogSettings MetroDialogOptions
		{
			get
			{
				return (MetroDialogSettings)GetValue(MetroDialogOptionsProperty);
			}
			set
			{
				SetValue(MetroDialogOptionsProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use BorderThickness=\"0\" and a GlowBrush=\"Black\" to get a drop shadow around the Window.")]
		public bool EnableDWMDropShadow
		{
			get
			{
				return (bool)GetValue(EnableDWMDropShadowProperty);
			}
			set
			{
				SetValue(EnableDWMDropShadowProperty, value);
			}
		}

		public bool IsWindowDraggable
		{
			get
			{
				return (bool)GetValue(IsWindowDraggableProperty);
			}
			set
			{
				SetValue(IsWindowDraggableProperty, value);
			}
		}

		public WindowCommandsOverlayBehavior LeftWindowCommandsOverlayBehavior
		{
			get
			{
				return (WindowCommandsOverlayBehavior)GetValue(LeftWindowCommandsOverlayBehaviorProperty);
			}
			set
			{
				SetValue(LeftWindowCommandsOverlayBehaviorProperty, value);
			}
		}

		public WindowCommandsOverlayBehavior RightWindowCommandsOverlayBehavior
		{
			get
			{
				return (WindowCommandsOverlayBehavior)GetValue(RightWindowCommandsOverlayBehaviorProperty);
			}
			set
			{
				SetValue(RightWindowCommandsOverlayBehaviorProperty, value);
			}
		}

		public WindowCommandsOverlayBehavior WindowButtonCommandsOverlayBehavior
		{
			get
			{
				return (WindowCommandsOverlayBehavior)GetValue(WindowButtonCommandsOverlayBehaviorProperty);
			}
			set
			{
				SetValue(WindowButtonCommandsOverlayBehaviorProperty, value);
			}
		}

		public WindowCommandsOverlayBehavior IconOverlayBehavior
		{
			get
			{
				return (WindowCommandsOverlayBehavior)GetValue(IconOverlayBehaviorProperty);
			}
			set
			{
				SetValue(IconOverlayBehaviorProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use LightMinButtonStyle or DarkMinButtonStyle in WindowButtonCommands to override the style.")]
		public Style WindowMinButtonStyle
		{
			get
			{
				return (Style)GetValue(WindowMinButtonStyleProperty);
			}
			set
			{
				SetValue(WindowMinButtonStyleProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use LightMaxButtonStyle or DarkMaxButtonStyle in WindowButtonCommands to override the style.")]
		public Style WindowMaxButtonStyle
		{
			get
			{
				return (Style)GetValue(WindowMaxButtonStyleProperty);
			}
			set
			{
				SetValue(WindowMaxButtonStyleProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use LightCloseButtonStyle or DarkCloseButtonStyle in WindowButtonCommands to override the style.")]
		public Style WindowCloseButtonStyle
		{
			get
			{
				return (Style)GetValue(WindowCloseButtonStyleProperty);
			}
			set
			{
				SetValue(WindowCloseButtonStyleProperty, value);
			}
		}

		public bool WindowTransitionsEnabled
		{
			get
			{
				return (bool)GetValue(WindowTransitionsEnabledProperty);
			}
			set
			{
				SetValue(WindowTransitionsEnabledProperty, value);
			}
		}

		public FlyoutsControl Flyouts
		{
			get
			{
				return (FlyoutsControl)GetValue(FlyoutsProperty);
			}
			set
			{
				SetValue(FlyoutsProperty, value);
			}
		}

		public DataTemplate IconTemplate
		{
			get
			{
				return (DataTemplate)GetValue(IconTemplateProperty);
			}
			set
			{
				SetValue(IconTemplateProperty, value);
			}
		}

		public DataTemplate TitleTemplate
		{
			get
			{
				return (DataTemplate)GetValue(TitleTemplateProperty);
			}
			set
			{
				SetValue(TitleTemplateProperty, value);
			}
		}

		public WindowCommands LeftWindowCommands
		{
			get
			{
				return (WindowCommands)GetValue(LeftWindowCommandsProperty);
			}
			set
			{
				SetValue(LeftWindowCommandsProperty, value);
			}
		}

		public WindowCommands RightWindowCommands
		{
			get
			{
				return (WindowCommands)GetValue(RightWindowCommandsProperty);
			}
			set
			{
				SetValue(RightWindowCommandsProperty, value);
			}
		}

		public WindowButtonCommands WindowButtonCommands
		{
			get
			{
				return (WindowButtonCommands)GetValue(WindowButtonCommandsProperty);
			}
			set
			{
				SetValue(WindowButtonCommandsProperty, value);
			}
		}

		public bool IgnoreTaskbarOnMaximize
		{
			get
			{
				return (bool)GetValue(IgnoreTaskbarOnMaximizeProperty);
			}
			set
			{
				SetValue(IgnoreTaskbarOnMaximizeProperty, value);
			}
		}

		public Thickness ResizeBorderThickness
		{
			get
			{
				return (Thickness)GetValue(ResizeBorderThicknessProperty);
			}
			set
			{
				SetValue(ResizeBorderThicknessProperty, value);
			}
		}

		public Brush TitleForeground
		{
			get
			{
				return (Brush)GetValue(TitleForegroundProperty);
			}
			set
			{
				SetValue(TitleForegroundProperty, value);
			}
		}

		public bool SaveWindowPosition
		{
			get
			{
				return (bool)GetValue(SaveWindowPositionProperty);
			}
			set
			{
				SetValue(SaveWindowPositionProperty, value);
			}
		}

		public IWindowPlacementSettings WindowPlacementSettings
		{
			get
			{
				return (IWindowPlacementSettings)GetValue(WindowPlacementSettingsProperty);
			}
			set
			{
				SetValue(WindowPlacementSettingsProperty, value);
			}
		}

		public bool ShowIconOnTitleBar
		{
			get
			{
				return (bool)GetValue(ShowIconOnTitleBarProperty);
			}
			set
			{
				SetValue(ShowIconOnTitleBarProperty, value);
			}
		}

		public bool ShowDialogsOverTitleBar
		{
			get
			{
				return (bool)GetValue(ShowDialogsOverTitleBarProperty);
			}
			set
			{
				SetValue(ShowDialogsOverTitleBarProperty, value);
			}
		}

		public bool IsAnyDialogOpen
		{
			get
			{
				return (bool)GetValue(IsAnyDialogOpenProperty);
			}
			private set
			{
				SetValue(IsAnyDialogOpenPropertyKey, value);
			}
		}

		public EdgeMode IconEdgeMode
		{
			get
			{
				return (EdgeMode)GetValue(IconEdgeModeProperty);
			}
			set
			{
				SetValue(IconEdgeModeProperty, value);
			}
		}

		public BitmapScalingMode IconBitmapScalingMode
		{
			get
			{
				return (BitmapScalingMode)GetValue(IconBitmapScalingModeProperty);
			}
			set
			{
				SetValue(IconBitmapScalingModeProperty, value);
			}
		}

		public MultiFrameImageMode IconScalingMode
		{
			get
			{
				return (MultiFrameImageMode)GetValue(IconScalingModeProperty);
			}
			set
			{
				SetValue(IconScalingModeProperty, value);
			}
		}

		public bool ShowTitleBar
		{
			get
			{
				return (bool)GetValue(ShowTitleBarProperty);
			}
			set
			{
				SetValue(ShowTitleBarProperty, value);
			}
		}

		public bool UseNoneWindowStyle
		{
			get
			{
				return (bool)GetValue(UseNoneWindowStyleProperty);
			}
			set
			{
				SetValue(UseNoneWindowStyleProperty, value);
			}
		}

		public bool ShowMinButton
		{
			get
			{
				return (bool)GetValue(ShowMinButtonProperty);
			}
			set
			{
				SetValue(ShowMinButtonProperty, value);
			}
		}

		public bool ShowMaxRestoreButton
		{
			get
			{
				return (bool)GetValue(ShowMaxRestoreButtonProperty);
			}
			set
			{
				SetValue(ShowMaxRestoreButtonProperty, value);
			}
		}

		public bool ShowCloseButton
		{
			get
			{
				return (bool)GetValue(ShowCloseButtonProperty);
			}
			set
			{
				SetValue(ShowCloseButtonProperty, value);
			}
		}

		public bool IsMinButtonEnabled
		{
			get
			{
				return (bool)GetValue(IsMinButtonEnabledProperty);
			}
			set
			{
				SetValue(IsMinButtonEnabledProperty, value);
			}
		}

		public bool IsMaxRestoreButtonEnabled
		{
			get
			{
				return (bool)GetValue(IsMaxRestoreButtonEnabledProperty);
			}
			set
			{
				SetValue(IsMaxRestoreButtonEnabledProperty, value);
			}
		}

		public bool IsCloseButtonEnabled
		{
			get
			{
				return (bool)GetValue(IsCloseButtonEnabledProperty);
			}
			set
			{
				SetValue(IsCloseButtonEnabledProperty, value);
			}
		}

		public bool IsCloseButtonEnabledWithDialog
		{
			get
			{
				return (bool)GetValue(IsCloseButtonEnabledWithDialogProperty);
			}
			private set
			{
				SetValue(IsCloseButtonEnabledWithDialogPropertyKey, value);
			}
		}

		public bool ShowSystemMenuOnRightClick
		{
			get
			{
				return (bool)GetValue(ShowSystemMenuOnRightClickProperty);
			}
			set
			{
				SetValue(ShowSystemMenuOnRightClickProperty, value);
			}
		}

		public int TitlebarHeight
		{
			get
			{
				return (int)GetValue(TitlebarHeightProperty);
			}
			set
			{
				SetValue(TitlebarHeightProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use the new TitleCharacterCasing dependency property.")]
		public bool TitleCaps
		{
			get
			{
				return (bool)GetValue(TitleCapsProperty);
			}
			set
			{
				SetValue(TitleCapsProperty, value);
			}
		}

		public CharacterCasing TitleCharacterCasing
		{
			get
			{
				return (CharacterCasing)GetValue(TitleCharacterCasingProperty);
			}
			set
			{
				SetValue(TitleCharacterCasingProperty, value);
			}
		}

		public HorizontalAlignment TitleAlignment
		{
			get
			{
				return (HorizontalAlignment)GetValue(TitleAlignmentProperty);
			}
			set
			{
				SetValue(TitleAlignmentProperty, value);
			}
		}

		public Brush WindowTitleBrush
		{
			get
			{
				return (Brush)GetValue(WindowTitleBrushProperty);
			}
			set
			{
				SetValue(WindowTitleBrushProperty, value);
			}
		}

		public Brush GlowBrush
		{
			get
			{
				return (Brush)GetValue(GlowBrushProperty);
			}
			set
			{
				SetValue(GlowBrushProperty, value);
			}
		}

		public Brush NonActiveGlowBrush
		{
			get
			{
				return (Brush)GetValue(NonActiveGlowBrushProperty);
			}
			set
			{
				SetValue(NonActiveGlowBrushProperty, value);
			}
		}

		public Brush NonActiveBorderBrush
		{
			get
			{
				return (Brush)GetValue(NonActiveBorderBrushProperty);
			}
			set
			{
				SetValue(NonActiveBorderBrushProperty, value);
			}
		}

		public Brush NonActiveWindowTitleBrush
		{
			get
			{
				return (Brush)GetValue(NonActiveWindowTitleBrushProperty);
			}
			set
			{
				SetValue(NonActiveWindowTitleBrushProperty, value);
			}
		}

		public Brush OverlayBrush
		{
			get
			{
				return (Brush)GetValue(OverlayBrushProperty);
			}
			set
			{
				SetValue(OverlayBrushProperty, value);
			}
		}

		public double OverlayOpacity
		{
			get
			{
				return (double)GetValue(OverlayOpacityProperty);
			}
			set
			{
				SetValue(OverlayOpacityProperty, value);
			}
		}

		public Storyboard OverlayFadeIn
		{
			get
			{
				return (Storyboard)GetValue(OverlayFadeInProperty);
			}
			set
			{
				SetValue(OverlayFadeInProperty, value);
			}
		}

		public Storyboard OverlayFadeOut
		{
			get
			{
				return (Storyboard)GetValue(OverlayFadeOutProperty);
			}
			set
			{
				SetValue(OverlayFadeOutProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release.")]
		public string WindowTitle
		{
			get
			{
				if (!TitleCaps)
				{
					return base.Title;
				}
				return base.Title.ToUpper();
			}
		}

		protected internal override IEnumerator LogicalChildren
		{
			protected get
			{
				ArrayList arrayList = new ArrayList
				{
					base.Content
				};
				if (LeftWindowCommands != null)
				{
					arrayList.Add(LeftWindowCommands);
				}
				if (RightWindowCommands != null)
				{
					arrayList.Add(RightWindowCommands);
				}
				if (WindowButtonCommands != null)
				{
					arrayList.Add(WindowButtonCommands);
				}
				if (Flyouts != null)
				{
					arrayList.Add(Flyouts);
				}
				return arrayList.GetEnumerator();
			}
		}

		protected IntPtr CriticalHandle => (IntPtr)typeof(Window).GetProperty("CriticalHandle", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(this, new object[0]);

		public event RoutedEventHandler FlyoutsStatusChanged
		{
			add
			{
				AddHandler(FlyoutsStatusChangedEvent, value);
			}
			remove
			{
				RemoveHandler(FlyoutsStatusChangedEvent, value);
			}
		}

		public event RoutedEventHandler WindowTransitionCompleted
		{
			add
			{
				AddHandler(WindowTransitionCompletedEvent, value);
			}
			remove
			{
				RemoveHandler(WindowTransitionCompletedEvent, value);
			}
		}

		private static void OnEnableDWMDropShadowPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue && (bool)e.NewValue)
			{
				((MetroWindow)d).UseDropShadow();
			}
		}

		private void UseDropShadow()
		{
			SetCurrentValue(Control.BorderThicknessProperty, new Thickness(0.0));
			SetCurrentValue(Control.BorderBrushProperty, null);
			SetCurrentValue(GlowBrushProperty, Brushes.Black);
		}

		public static void OnWindowButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue)
			{
				MetroWindow metroWindow = (MetroWindow)d;
				if (metroWindow.WindowButtonCommands != null)
				{
					metroWindow.WindowButtonCommands.ApplyTheme();
				}
			}
		}

		public virtual IWindowPlacementSettings GetWindowPlacementSettings()
		{
			return WindowPlacementSettings ?? new WindowApplicationSettings(this);
		}

		private static void OnShowIconOnTitleBarPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MetroWindow metroWindow = (MetroWindow)d;
			if (e.NewValue != e.OldValue)
			{
				metroWindow.SetVisibiltyForIcon();
			}
		}

		private static void OnShowTitleBarPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MetroWindow metroWindow = (MetroWindow)d;
			if (e.NewValue != e.OldValue)
			{
				metroWindow.SetVisibiltyForAllTitleElements();
			}
		}

		private static object OnShowTitleBarCoerceValueCallback(DependencyObject d, object value)
		{
			if (((MetroWindow)d).UseNoneWindowStyle)
			{
				return false;
			}
			return value;
		}

		private static void OnUseNoneWindowStylePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue && (bool)e.NewValue)
			{
				((MetroWindow)d).SetCurrentValue(ShowTitleBarProperty, false);
			}
		}

		private static void TitlebarHeightPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			MetroWindow metroWindow = (MetroWindow)dependencyObject;
			if (e.NewValue != e.OldValue)
			{
				metroWindow.SetVisibiltyForAllTitleElements();
			}
		}

		private void SetVisibiltyForIcon()
		{
			if (icon != null)
			{
				Visibility visibility = ((!IconOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.HiddenTitleBar) || ShowTitleBar) && (!ShowIconOnTitleBar || !ShowTitleBar)) ? Visibility.Collapsed : Visibility.Visible;
				icon.Visibility = visibility;
			}
		}

		private void SetVisibiltyForAllTitleElements()
		{
			SetVisibiltyForIcon();
			Visibility visibility = (TitlebarHeight <= 0 || !ShowTitleBar || UseNoneWindowStyle) ? Visibility.Collapsed : Visibility.Visible;
			titleBar?.SetCurrentValue(UIElement.VisibilityProperty, visibility);
			titleBarBackground?.SetCurrentValue(UIElement.VisibilityProperty, visibility);
			Visibility visibility2 = (!LeftWindowCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.HiddenTitleBar) || UseNoneWindowStyle) ? visibility : Visibility.Visible;
			LeftWindowCommandsPresenter?.SetCurrentValue(UIElement.VisibilityProperty, visibility2);
			Visibility visibility3 = (!RightWindowCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.HiddenTitleBar) || UseNoneWindowStyle) ? visibility : Visibility.Visible;
			RightWindowCommandsPresenter?.SetCurrentValue(UIElement.VisibilityProperty, visibility3);
			Visibility visibility4 = (!WindowButtonCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.HiddenTitleBar)) ? visibility : Visibility.Visible;
			WindowButtonCommandsPresenter?.SetCurrentValue(UIElement.VisibilityProperty, visibility4);
			SetWindowEvents();
		}

		private static void OnTitleAlignmentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			MetroWindow metroWindow = dependencyObject as MetroWindow;
			if (metroWindow != null)
			{
				metroWindow.SizeChanged -= metroWindow.MetroWindow_SizeChanged;
				if (e.NewValue is HorizontalAlignment && (HorizontalAlignment)e.NewValue == HorizontalAlignment.Center)
				{
					metroWindow.SizeChanged += metroWindow.MetroWindow_SizeChanged;
				}
			}
		}

		private bool CanUseOverlayFadingStoryboard(Storyboard sb, out DoubleAnimation animation)
		{
			animation = null;
			if (sb == null)
			{
				return false;
			}
			sb.Dispatcher.VerifyAccess();
			animation = sb.Children.OfType<DoubleAnimation>().FirstOrDefault();
			if (animation == null)
			{
				return false;
			}
			if ((!sb.Duration.HasTimeSpan || sb.Duration.TimeSpan.Ticks <= 0) && !(sb.AccelerationRatio > 0.0) && !(sb.DecelerationRatio > 0.0) && (!animation.Duration.HasTimeSpan || animation.Duration.TimeSpan.Ticks <= 0) && !(animation.AccelerationRatio > 0.0))
			{
				return animation.DecelerationRatio > 0.0;
			}
			return true;
		}

		public Task ShowOverlayAsync()
		{
			if (overlayBox == null)
			{
				throw new InvalidOperationException("OverlayBox can not be founded in this MetroWindow's template. Are you calling this before the window has loaded?");
			}
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			if (IsOverlayVisible() && overlayStoryboard == null)
			{
				tcs.SetResult(null);
				return tcs.Task;
			}
			base.Dispatcher.VerifyAccess();
			Storyboard sb = OverlayFadeIn?.Clone();
			overlayStoryboard = sb;
			if (CanUseOverlayFadingStoryboard(sb, out DoubleAnimation animation))
			{
				overlayBox.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Visible);
				animation.To = OverlayOpacity;
				EventHandler completionHandler = null;
				completionHandler = delegate
				{
					sb.Completed -= completionHandler;
					if (overlayStoryboard == sb)
					{
						overlayStoryboard = null;
					}
					tcs.TrySetResult(null);
				};
				sb.Completed += completionHandler;
				overlayBox.BeginStoryboard(sb);
			}
			else
			{
				ShowOverlay();
				tcs.TrySetResult(null);
			}
			return tcs.Task;
		}

		public Task HideOverlayAsync()
		{
			if (overlayBox == null)
			{
				throw new InvalidOperationException("OverlayBox can not be founded in this MetroWindow's template. Are you calling this before the window has loaded?");
			}
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
			if (overlayBox.Visibility == Visibility.Visible && overlayBox.Opacity <= 0.0)
			{
				overlayBox.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Hidden);
				tcs.SetResult(null);
				return tcs.Task;
			}
			base.Dispatcher.VerifyAccess();
			Storyboard sb = OverlayFadeOut?.Clone();
			overlayStoryboard = sb;
			if (CanUseOverlayFadingStoryboard(sb, out DoubleAnimation animation))
			{
				animation.To = 0.0;
				EventHandler completionHandler = null;
				completionHandler = delegate
				{
					sb.Completed -= completionHandler;
					if (overlayStoryboard == sb)
					{
						overlayBox.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Hidden);
						overlayStoryboard = null;
					}
					tcs.TrySetResult(null);
				};
				sb.Completed += completionHandler;
				overlayBox.BeginStoryboard(sb);
			}
			else
			{
				HideOverlay();
				tcs.TrySetResult(null);
			}
			return tcs.Task;
		}

		public bool IsOverlayVisible()
		{
			if (overlayBox == null)
			{
				throw new InvalidOperationException("OverlayBox can not be founded in this MetroWindow's template. Are you calling this before the window has loaded?");
			}
			if (overlayBox.Visibility == Visibility.Visible)
			{
				return overlayBox.Opacity >= OverlayOpacity;
			}
			return false;
		}

		public void ShowOverlay()
		{
			overlayBox.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Visible);
			overlayBox.SetCurrentValue(UIElement.OpacityProperty, OverlayOpacity);
		}

		public void HideOverlay()
		{
			overlayBox.SetCurrentValue(UIElement.OpacityProperty, 0.0);
			overlayBox.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Hidden);
		}

		public void StoreFocus(IInputElement thisElement = null)
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				restoreFocus = (thisElement ?? restoreFocus ?? FocusManager.GetFocusedElement(this));
			});
		}

		internal void RestoreFocus()
		{
			if (restoreFocus != null)
			{
				base.Dispatcher.BeginInvoke((Action)delegate
				{
					Keyboard.Focus(restoreFocus);
					restoreFocus = null;
				});
			}
		}

		public void ResetStoredFocus()
		{
			restoreFocus = null;
		}

		static MetroWindow()
		{
			ShowIconOnTitleBarProperty = DependencyProperty.Register("ShowIconOnTitleBar", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true, OnShowIconOnTitleBarPropertyChangedCallback));
			IconEdgeModeProperty = DependencyProperty.Register("IconEdgeMode", typeof(EdgeMode), typeof(MetroWindow), new PropertyMetadata(EdgeMode.Aliased));
			IconBitmapScalingModeProperty = DependencyProperty.Register("IconBitmapScalingMode", typeof(BitmapScalingMode), typeof(MetroWindow), new PropertyMetadata(BitmapScalingMode.HighQuality));
			IconScalingModeProperty = DependencyProperty.Register("IconScalingMode", typeof(MultiFrameImageMode), typeof(MetroWindow), new FrameworkPropertyMetadata(MultiFrameImageMode.ScaleDownLargerFrame, FrameworkPropertyMetadataOptions.AffectsRender));
			ShowTitleBarProperty = DependencyProperty.Register("ShowTitleBar", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true, OnShowTitleBarPropertyChangedCallback, OnShowTitleBarCoerceValueCallback));
			ShowDialogsOverTitleBarProperty = DependencyProperty.Register("ShowDialogsOverTitleBar", typeof(bool), typeof(MetroWindow), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));
			IsAnyDialogOpenPropertyKey = DependencyProperty.RegisterReadOnly("IsAnyDialogOpen", typeof(bool), typeof(MetroWindow), new PropertyMetadata(false));
			IsAnyDialogOpenProperty = IsAnyDialogOpenPropertyKey.DependencyProperty;
			ShowMinButtonProperty = DependencyProperty.Register("ShowMinButton", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			ShowMaxRestoreButtonProperty = DependencyProperty.Register("ShowMaxRestoreButton", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			ShowCloseButtonProperty = DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			IsMinButtonEnabledProperty = DependencyProperty.Register("IsMinButtonEnabled", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			IsMaxRestoreButtonEnabledProperty = DependencyProperty.Register("IsMaxRestoreButtonEnabled", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			IsCloseButtonEnabledProperty = DependencyProperty.Register("IsCloseButtonEnabled", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			IsCloseButtonEnabledWithDialogPropertyKey = DependencyProperty.RegisterReadOnly("IsCloseButtonEnabledWithDialog", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			IsCloseButtonEnabledWithDialogProperty = IsCloseButtonEnabledWithDialogPropertyKey.DependencyProperty;
			ShowSystemMenuOnRightClickProperty = DependencyProperty.Register("ShowSystemMenuOnRightClick", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			TitlebarHeightProperty = DependencyProperty.Register("TitlebarHeight", typeof(int), typeof(MetroWindow), new PropertyMetadata(30, TitlebarHeightPropertyChangedCallback));
			TitleCapsProperty = DependencyProperty.Register("TitleCaps", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
			{
				((MetroWindow)o).SetCurrentValue(TitleCharacterCasingProperty, ((bool)e.NewValue) ? CharacterCasing.Upper : CharacterCasing.Normal);
			}));
			TitleCharacterCasingProperty = DependencyProperty.Register("TitleCharacterCasing", typeof(CharacterCasing), typeof(MetroWindow), new FrameworkPropertyMetadata(CharacterCasing.Upper, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits), delegate(object value)
			{
				if (CharacterCasing.Normal <= (CharacterCasing)value)
				{
					return (CharacterCasing)value <= CharacterCasing.Upper;
				}
				return false;
			});
			TitleAlignmentProperty = DependencyProperty.Register("TitleAlignment", typeof(HorizontalAlignment), typeof(MetroWindow), new PropertyMetadata(HorizontalAlignment.Stretch, OnTitleAlignmentChanged));
			SaveWindowPositionProperty = DependencyProperty.Register("SaveWindowPosition", typeof(bool), typeof(MetroWindow), new PropertyMetadata(false));
			WindowPlacementSettingsProperty = DependencyProperty.Register("WindowPlacementSettings", typeof(IWindowPlacementSettings), typeof(MetroWindow), new PropertyMetadata(null));
			TitleForegroundProperty = DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(MetroWindow));
			IgnoreTaskbarOnMaximizeProperty = DependencyProperty.Register("IgnoreTaskbarOnMaximize", typeof(bool), typeof(MetroWindow), new PropertyMetadata(false));
			FlyoutsProperty = DependencyProperty.Register("Flyouts", typeof(FlyoutsControl), typeof(MetroWindow), new PropertyMetadata(null, UpdateLogicalChilds));
			WindowTransitionsEnabledProperty = DependencyProperty.Register("WindowTransitionsEnabled", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			MetroDialogOptionsProperty = DependencyProperty.Register("MetroDialogOptions", typeof(MetroDialogSettings), typeof(MetroWindow), new PropertyMetadata((object)null));
			WindowTitleBrushProperty = DependencyProperty.Register("WindowTitleBrush", typeof(Brush), typeof(MetroWindow), new PropertyMetadata(Brushes.Transparent));
			NonActiveWindowTitleBrushProperty = DependencyProperty.Register("NonActiveWindowTitleBrush", typeof(Brush), typeof(MetroWindow), new PropertyMetadata(Brushes.Gray));
			NonActiveBorderBrushProperty = DependencyProperty.Register("NonActiveBorderBrush", typeof(Brush), typeof(MetroWindow), new PropertyMetadata(Brushes.Gray));
			GlowBrushProperty = DependencyProperty.Register("GlowBrush", typeof(Brush), typeof(MetroWindow), new PropertyMetadata(null));
			NonActiveGlowBrushProperty = DependencyProperty.Register("NonActiveGlowBrush", typeof(Brush), typeof(MetroWindow), new PropertyMetadata(null));
			OverlayBrushProperty = DependencyProperty.Register("OverlayBrush", typeof(Brush), typeof(MetroWindow), new PropertyMetadata(null));
			OverlayOpacityProperty = DependencyProperty.Register("OverlayOpacity", typeof(double), typeof(MetroWindow), new PropertyMetadata(0.7));
			OverlayFadeInProperty = DependencyProperty.Register("OverlayFadeIn", typeof(Storyboard), typeof(MetroWindow), new PropertyMetadata((object)null));
			OverlayFadeOutProperty = DependencyProperty.Register("OverlayFadeOut", typeof(Storyboard), typeof(MetroWindow), new PropertyMetadata((object)null));
			IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(MetroWindow), new PropertyMetadata(null));
			TitleTemplateProperty = DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(MetroWindow), new PropertyMetadata(null));
			LeftWindowCommandsProperty = DependencyProperty.Register("LeftWindowCommands", typeof(WindowCommands), typeof(MetroWindow), new PropertyMetadata(null, UpdateLogicalChilds));
			RightWindowCommandsProperty = DependencyProperty.Register("RightWindowCommands", typeof(WindowCommands), typeof(MetroWindow), new PropertyMetadata(null, UpdateLogicalChilds));
			WindowButtonCommandsProperty = DependencyProperty.Register("WindowButtonCommands", typeof(WindowButtonCommands), typeof(MetroWindow), new PropertyMetadata(null, UpdateLogicalChilds));
			LeftWindowCommandsOverlayBehaviorProperty = DependencyProperty.Register("LeftWindowCommandsOverlayBehavior", typeof(WindowCommandsOverlayBehavior), typeof(MetroWindow), new PropertyMetadata(WindowCommandsOverlayBehavior.Flyouts, OnShowTitleBarPropertyChangedCallback));
			RightWindowCommandsOverlayBehaviorProperty = DependencyProperty.Register("RightWindowCommandsOverlayBehavior", typeof(WindowCommandsOverlayBehavior), typeof(MetroWindow), new PropertyMetadata(WindowCommandsOverlayBehavior.Flyouts, OnShowTitleBarPropertyChangedCallback));
			WindowButtonCommandsOverlayBehaviorProperty = DependencyProperty.Register("WindowButtonCommandsOverlayBehavior", typeof(WindowCommandsOverlayBehavior), typeof(MetroWindow), new PropertyMetadata(WindowCommandsOverlayBehavior.Always, OnShowTitleBarPropertyChangedCallback));
			IconOverlayBehaviorProperty = DependencyProperty.Register("IconOverlayBehavior", typeof(WindowCommandsOverlayBehavior), typeof(MetroWindow), new PropertyMetadata(WindowCommandsOverlayBehavior.Never, OnShowTitleBarPropertyChangedCallback));
			WindowMinButtonStyleProperty = DependencyProperty.Register("WindowMinButtonStyle", typeof(Style), typeof(MetroWindow), new PropertyMetadata(null, OnWindowButtonStyleChanged));
			WindowMaxButtonStyleProperty = DependencyProperty.Register("WindowMaxButtonStyle", typeof(Style), typeof(MetroWindow), new PropertyMetadata(null, OnWindowButtonStyleChanged));
			WindowCloseButtonStyleProperty = DependencyProperty.Register("WindowCloseButtonStyle", typeof(Style), typeof(MetroWindow), new PropertyMetadata(null, OnWindowButtonStyleChanged));
			UseNoneWindowStyleProperty = DependencyProperty.Register("UseNoneWindowStyle", typeof(bool), typeof(MetroWindow), new PropertyMetadata(false, OnUseNoneWindowStylePropertyChangedCallback));
			OverrideDefaultWindowCommandsBrushProperty = DependencyProperty.Register("OverrideDefaultWindowCommandsBrush", typeof(SolidColorBrush), typeof(MetroWindow));
			EnableDWMDropShadowProperty = DependencyProperty.Register("EnableDWMDropShadow", typeof(bool), typeof(MetroWindow), new PropertyMetadata(false, OnEnableDWMDropShadowPropertyChangedCallback));
			IsWindowDraggableProperty = DependencyProperty.Register("IsWindowDraggable", typeof(bool), typeof(MetroWindow), new PropertyMetadata(true));
			FlyoutsStatusChangedEvent = EventManager.RegisterRoutedEvent("FlyoutsStatusChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MetroWindow));
			WindowTransitionCompletedEvent = EventManager.RegisterRoutedEvent("WindowTransitionCompleted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MetroWindow));
			ResizeBorderThicknessProperty = DependencyProperty.Register("ResizeBorderThickness", typeof(Thickness), typeof(MetroWindow), new PropertyMetadata(WindowChromeBehavior.GetDefaultResizeBorderThickness()));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(typeof(MetroWindow)));
		}

		public MetroWindow()
		{
			MetroDialogOptions = new MetroDialogSettings();
			base.DataContextChanged += MetroWindow_DataContextChanged;
			base.Loaded += MetroWindow_Loaded;
			InitializeStylizedBehaviors();
		}

		private void InitializeStylizedBehaviors()
		{
			StylizedBehaviorCollection value = new StylizedBehaviorCollection
			{
				new BorderlessWindowBehavior(),
				new WindowsSettingBehaviour(),
				new GlowWindowBehavior()
			};
			StylizedBehaviors.SetBehaviors(this, value);
		}

		protected override async void OnClosing(CancelEventArgs e)
		{
			if (!e.Cancel)
			{
				BaseMetroDialog baseMetroDialog = await this.GetCurrentDialogAsync<BaseMetroDialog>();
				e.Cancel = (baseMetroDialog != null && (ShowDialogsOverTitleBar || baseMetroDialog.DialogSettings == null || !baseMetroDialog.DialogSettings.OwnerCanCloseWithDialog));
			}
			base.OnClosing(e);
		}

		private void MetroWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (LeftWindowCommands != null)
			{
				LeftWindowCommands.DataContext = base.DataContext;
			}
			if (RightWindowCommands != null)
			{
				RightWindowCommands.DataContext = base.DataContext;
			}
			if (WindowButtonCommands != null)
			{
				WindowButtonCommands.DataContext = base.DataContext;
			}
			if (Flyouts != null)
			{
				Flyouts.DataContext = base.DataContext;
			}
		}

		private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if (EnableDWMDropShadow)
			{
				UseDropShadow();
			}
			if (WindowTransitionsEnabled)
			{
				VisualStateManager.GoToState(this, "AfterLoaded", useTransitions: true);
			}
			if (Flyouts == null)
			{
				Flyouts = new FlyoutsControl();
			}
			this.ResetAllWindowCommandsBrush();
			ThemeManager.IsThemeChanged += ThemeManagerOnIsThemeChanged;
			base.Unloaded += delegate
			{
				ThemeManager.IsThemeChanged -= ThemeManagerOnIsThemeChanged;
			};
		}

		private void MetroWindow_SizeChanged(object sender, RoutedEventArgs e)
		{
			if (TitleAlignment == HorizontalAlignment.Center)
			{
				double num = base.ActualWidth / 2.0;
				double num2 = titleBar.DesiredSize.Width / 2.0;
				double num3 = icon.ActualWidth + LeftWindowCommands.ActualWidth;
				double num4 = WindowButtonCommands.ActualWidth + RightWindowCommands.ActualWidth;
				double num5 = num3 + num2 + 5.0;
				double num6 = num4 + num2 + 5.0;
				if (num5 < num && num6 < num)
				{
					Grid.SetColumn(titleBar, 0);
					Grid.SetColumnSpan(titleBar, 7);
				}
				else
				{
					Grid.SetColumn(titleBar, 3);
					Grid.SetColumnSpan(titleBar, 1);
				}
			}
		}

		private void ThemeManagerOnIsThemeChanged(object sender, OnThemeChangedEventArgs e)
		{
			if (e.Accent != null)
			{
				List<Flyout> list = Flyouts.GetFlyouts().ToList();
				List<FlyoutsControl> source = (base.Content as DependencyObject).FindChildren<FlyoutsControl>(forceUsingTheVisualTreeHelper: true).ToList();
				if (source.Any())
				{
					list.AddRange(source.SelectMany((FlyoutsControl flyoutsControl) => flyoutsControl.GetFlyouts()));
				}
				if (!list.Any())
				{
					this.ResetAllWindowCommandsBrush();
				}
				else
				{
					foreach (Flyout item in list)
					{
						item.ChangeFlyoutTheme(e.Accent, e.AppTheme);
					}
					this.HandleWindowCommandsForFlyouts(list);
				}
			}
		}

		private void FlyoutsPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			DependencyObject dependencyObject = e.OriginalSource as DependencyObject;
			if (dependencyObject == null || (dependencyObject.TryFindParent<Flyout>() == null && !object.Equals(dependencyObject, overlayBox) && dependencyObject.TryFindParent<BaseMetroDialog>() == null && !object.Equals(dependencyObject.TryFindParent<ContentControl>(), icon) && dependencyObject.TryFindParent<WindowCommands>() == null && dependencyObject.TryFindParent<WindowButtonCommands>() == null))
			{
				if (!Flyouts.OverrideExternalCloseButton.HasValue)
				{
					foreach (Flyout item in Flyouts.GetFlyouts().Where(delegate(Flyout x)
					{
						if (x.IsOpen && x.ExternalCloseButton == e.ChangedButton)
						{
							if (x.IsPinned)
							{
								return Flyouts.OverrideIsPinned;
							}
							return true;
						}
						return false;
					}))
					{
						item.IsOpen = false;
					}
				}
				else if (Flyouts.OverrideExternalCloseButton == e.ChangedButton)
				{
					foreach (Flyout item2 in Flyouts.GetFlyouts().Where(delegate(Flyout x)
					{
						if (x.IsOpen)
						{
							if (x.IsPinned)
							{
								return Flyouts.OverrideIsPinned;
							}
							return true;
						}
						return false;
					}))
					{
						item2.IsOpen = false;
					}
				}
			}
		}

		private static void UpdateLogicalChilds(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			MetroWindow metroWindow = dependencyObject as MetroWindow;
			if (metroWindow != null)
			{
				FrameworkElement frameworkElement = e.OldValue as FrameworkElement;
				if (frameworkElement != null)
				{
					metroWindow.RemoveLogicalChild(frameworkElement);
				}
				FrameworkElement frameworkElement2 = e.NewValue as FrameworkElement;
				if (frameworkElement2 != null)
				{
					metroWindow.AddLogicalChild(frameworkElement2);
					frameworkElement2.DataContext = metroWindow.DataContext;
				}
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			LeftWindowCommandsPresenter = (GetTemplateChild("PART_LeftWindowCommands") as ContentPresenter);
			RightWindowCommandsPresenter = (GetTemplateChild("PART_RightWindowCommands") as ContentPresenter);
			WindowButtonCommandsPresenter = (GetTemplateChild("PART_WindowButtonCommands") as ContentPresenter);
			if (LeftWindowCommands == null)
			{
				LeftWindowCommands = new WindowCommands();
			}
			if (RightWindowCommands == null)
			{
				RightWindowCommands = new WindowCommands();
			}
			if (WindowButtonCommands == null)
			{
				WindowButtonCommands = new WindowButtonCommands();
			}
			LeftWindowCommands.ParentWindow = this;
			RightWindowCommands.ParentWindow = this;
			WindowButtonCommands.ParentWindow = this;
			overlayBox = (GetTemplateChild("PART_OverlayBox") as Grid);
			metroActiveDialogContainer = (GetTemplateChild("PART_MetroActiveDialogContainer") as Grid);
			metroInactiveDialogContainer = (GetTemplateChild("PART_MetroInactiveDialogsContainer") as Grid);
			flyoutModal = (Rectangle)GetTemplateChild("PART_FlyoutModal");
			flyoutModal.PreviewMouseDown += FlyoutsPreviewMouseDown;
			base.PreviewMouseDown += FlyoutsPreviewMouseDown;
			icon = (GetTemplateChild("PART_Icon") as FrameworkElement);
			titleBar = (GetTemplateChild("PART_TitleBar") as UIElement);
			titleBarBackground = (GetTemplateChild("PART_WindowTitleBackground") as UIElement);
			windowTitleThumb = (GetTemplateChild("PART_WindowTitleThumb") as Thumb);
			flyoutModalDragMoveThumb = (GetTemplateChild("PART_FlyoutModalDragMoveThumb") as Thumb);
			SetVisibiltyForAllTitleElements();
			MetroContentControl metroContentControl = GetTemplateChild("PART_Content") as MetroContentControl;
			if (metroContentControl != null)
			{
				metroContentControl.TransitionCompleted += delegate
				{
					RaiseEvent(new RoutedEventArgs(WindowTransitionCompletedEvent));
				};
			}
		}

		private void ClearWindowEvents()
		{
			if (windowTitleThumb != null)
			{
				windowTitleThumb.PreviewMouseLeftButtonUp -= WindowTitleThumbOnPreviewMouseLeftButtonUp;
				windowTitleThumb.DragDelta -= WindowTitleThumbMoveOnDragDelta;
				windowTitleThumb.MouseDoubleClick -= WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
				windowTitleThumb.MouseRightButtonUp -= WindowTitleThumbSystemMenuOnMouseRightButtonUp;
			}
			IMetroThumb metroThumb = titleBar as IMetroThumb;
			if (metroThumb != null)
			{
				metroThumb.PreviewMouseLeftButtonUp -= WindowTitleThumbOnPreviewMouseLeftButtonUp;
				metroThumb.DragDelta -= WindowTitleThumbMoveOnDragDelta;
				metroThumb.MouseDoubleClick -= WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
				metroThumb.MouseRightButtonUp -= WindowTitleThumbSystemMenuOnMouseRightButtonUp;
			}
			if (flyoutModalDragMoveThumb != null)
			{
				flyoutModalDragMoveThumb.PreviewMouseLeftButtonUp -= WindowTitleThumbOnPreviewMouseLeftButtonUp;
				flyoutModalDragMoveThumb.DragDelta -= WindowTitleThumbMoveOnDragDelta;
				flyoutModalDragMoveThumb.MouseDoubleClick -= WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
				flyoutModalDragMoveThumb.MouseRightButtonUp -= WindowTitleThumbSystemMenuOnMouseRightButtonUp;
			}
			if (icon != null)
			{
				icon.MouseDown -= IconMouseDown;
			}
			base.SizeChanged -= MetroWindow_SizeChanged;
		}

		private void SetWindowEvents()
		{
			ClearWindowEvents();
			if (icon != null && icon.Visibility == Visibility.Visible)
			{
				icon.MouseDown += IconMouseDown;
			}
			if (windowTitleThumb != null)
			{
				windowTitleThumb.PreviewMouseLeftButtonUp += WindowTitleThumbOnPreviewMouseLeftButtonUp;
				windowTitleThumb.DragDelta += WindowTitleThumbMoveOnDragDelta;
				windowTitleThumb.MouseDoubleClick += WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
				windowTitleThumb.MouseRightButtonUp += WindowTitleThumbSystemMenuOnMouseRightButtonUp;
			}
			IMetroThumb metroThumb = titleBar as IMetroThumb;
			if (metroThumb != null)
			{
				metroThumb.PreviewMouseLeftButtonUp += WindowTitleThumbOnPreviewMouseLeftButtonUp;
				metroThumb.DragDelta += WindowTitleThumbMoveOnDragDelta;
				metroThumb.MouseDoubleClick += WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
				metroThumb.MouseRightButtonUp += WindowTitleThumbSystemMenuOnMouseRightButtonUp;
			}
			if (flyoutModalDragMoveThumb != null)
			{
				flyoutModalDragMoveThumb.PreviewMouseLeftButtonUp += WindowTitleThumbOnPreviewMouseLeftButtonUp;
				flyoutModalDragMoveThumb.DragDelta += WindowTitleThumbMoveOnDragDelta;
				flyoutModalDragMoveThumb.MouseDoubleClick += WindowTitleThumbChangeWindowStateOnMouseDoubleClick;
				flyoutModalDragMoveThumb.MouseRightButtonUp += WindowTitleThumbSystemMenuOnMouseRightButtonUp;
			}
			if (titleBar != null && TitleAlignment == HorizontalAlignment.Center)
			{
				base.SizeChanged += MetroWindow_SizeChanged;
			}
		}

		private void IconMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				if (e.ClickCount == 2)
				{
					Close();
				}
				else
				{
					ShowSystemMenuPhysicalCoordinates(this, PointToScreen(new Point(0.0, (double)TitlebarHeight)));
				}
			}
		}

		private void WindowTitleThumbOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			DoWindowTitleThumbOnPreviewMouseLeftButtonUp(this, e);
		}

		private void WindowTitleThumbMoveOnDragDelta(object sender, DragDeltaEventArgs dragDeltaEventArgs)
		{
			DoWindowTitleThumbMoveOnDragDelta(sender as IMetroThumb, this, dragDeltaEventArgs);
		}

		private void WindowTitleThumbChangeWindowStateOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick(this, mouseButtonEventArgs);
		}

		private void WindowTitleThumbSystemMenuOnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			DoWindowTitleThumbSystemMenuOnMouseRightButtonUp(this, e);
		}

		internal static void DoWindowTitleThumbOnPreviewMouseLeftButtonUp(MetroWindow window, MouseButtonEventArgs mouseButtonEventArgs)
		{
			if (mouseButtonEventArgs.Source == mouseButtonEventArgs.OriginalSource)
			{
				Mouse.Capture(null);
			}
		}

		internal static void DoWindowTitleThumbMoveOnDragDelta(IMetroThumb thumb, MetroWindow window, DragDeltaEventArgs dragDeltaEventArgs)
		{
			if (thumb == null)
			{
				throw new ArgumentNullException("thumb");
			}
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			if (window.IsWindowDraggable && (Math.Abs(dragDeltaEventArgs.HorizontalChange) > 2.0 || Math.Abs(dragDeltaEventArgs.VerticalChange) > 2.0))
			{
				window.VerifyAccess();
				bool flag = window.WindowState == WindowState.Maximized;
				if ((Mouse.GetPosition(thumb).Y <= (double)window.TitlebarHeight && window.TitlebarHeight > 0) || !flag)
				{
					UnsafeNativeMethods.ReleaseCapture();
					if (flag)
					{
						EventHandler windowOnStateChanged = null;
						windowOnStateChanged = delegate
						{
							window.StateChanged -= windowOnStateChanged;
							if (window.WindowState == WindowState.Normal)
							{
								Mouse.Capture(thumb, CaptureMode.Element);
							}
						};
						window.StateChanged += windowOnStateChanged;
					}
					IntPtr criticalHandle = window.CriticalHandle;
					NativeMethods.SendMessage(criticalHandle, WM.SYSCOMMAND, (IntPtr)61458, IntPtr.Zero);
					NativeMethods.SendMessage(criticalHandle, WM.LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
				}
			}
		}

		internal static void DoWindowTitleThumbChangeWindowStateOnMouseDoubleClick(MetroWindow window, MouseButtonEventArgs mouseButtonEventArgs)
		{
			if (mouseButtonEventArgs.ChangedButton == MouseButton.Left)
			{
				bool num = window.ResizeMode == ResizeMode.CanResizeWithGrip || window.ResizeMode == ResizeMode.CanResize;
				bool flag = Mouse.GetPosition(window).Y <= (double)window.TitlebarHeight && window.TitlebarHeight > 0;
				if (num & flag)
				{
					if (window.WindowState == WindowState.Normal)
					{
						ControlzEx.Windows.Shell.SystemCommands.MaximizeWindow(window);
					}
					else
					{
						ControlzEx.Windows.Shell.SystemCommands.RestoreWindow(window);
					}
					mouseButtonEventArgs.Handled = true;
				}
			}
		}

		internal static void DoWindowTitleThumbSystemMenuOnMouseRightButtonUp(MetroWindow window, MouseButtonEventArgs e)
		{
			if (window.ShowSystemMenuOnRightClick)
			{
				Point position = e.GetPosition(window);
				if ((position.Y <= (double)window.TitlebarHeight && window.TitlebarHeight > 0) || (window.UseNoneWindowStyle && window.TitlebarHeight <= 0))
				{
					ShowSystemMenuPhysicalCoordinates(window, window.PointToScreen(position));
				}
			}
		}

		internal T GetPart<T>(string name) where T : class
		{
			return GetTemplateChild(name) as T;
		}

		internal DependencyObject GetPart(string name)
		{
			return GetTemplateChild(name);
		}

		private static void ShowSystemMenuPhysicalCoordinates(Window window, Point physicalScreenLocation)
		{
			if (window != null)
			{
				IntPtr handle = new WindowInteropHelper(window).Handle;
				if (!(handle == IntPtr.Zero) && NativeMethods.IsWindow(handle))
				{
					uint num = NativeMethods.TrackPopupMenuEx(NativeMethods.GetSystemMenu(handle, bRevert: false), 256u, (int)physicalScreenLocation.X, (int)physicalScreenLocation.Y, handle, IntPtr.Zero);
					if (num != 0)
					{
						NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr(num), IntPtr.Zero);
					}
				}
			}
		}

		internal void HandleFlyoutStatusChange(Flyout flyout, IList<Flyout> visibleFlyouts)
		{
			int num = flyout.IsOpen ? (Panel.GetZIndex(flyout) + 3) : (visibleFlyouts.Count() + 2);
			icon?.SetValue(Panel.ZIndexProperty, (!flyout.IsModal || !flyout.IsOpen) ? ((!IconOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.Flyouts)) ? 1 : num) : 0);
			LeftWindowCommandsPresenter?.SetValue(Panel.ZIndexProperty, (!flyout.IsModal || !flyout.IsOpen) ? ((!LeftWindowCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.Flyouts)) ? 1 : num) : 0);
			RightWindowCommandsPresenter?.SetValue(Panel.ZIndexProperty, (!flyout.IsModal || !flyout.IsOpen) ? ((!RightWindowCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.Flyouts)) ? 1 : num) : 0);
			WindowButtonCommandsPresenter?.SetValue(Panel.ZIndexProperty, (!flyout.IsModal || !flyout.IsOpen) ? ((!WindowButtonCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.Flyouts)) ? 1 : num) : 0);
			this.HandleWindowCommandsForFlyouts(visibleFlyouts);
			if (flyoutModal != null)
			{
				flyoutModal.Visibility = ((!visibleFlyouts.Any((Flyout x) => x.IsModal)) ? Visibility.Hidden : Visibility.Visible);
			}
			RaiseEvent(new FlyoutStatusChangedRoutedEventArgs(FlyoutsStatusChangedEvent, this)
			{
				ChangedFlyout = flyout
			});
		}
	}
}
