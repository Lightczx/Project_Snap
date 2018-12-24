using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PaneClipRectangle", Type = typeof(RectangleGeometry))]
	[TemplatePart(Name = "LightDismissLayer", Type = typeof(Rectangle))]
	[TemplateVisualState(Name = "Closed                 ", GroupName = "DisplayModeStates")]
	[TemplateVisualState(Name = "ClosedCompactLeft      ", GroupName = "DisplayModeStates")]
	[TemplateVisualState(Name = "ClosedCompactRight     ", GroupName = "DisplayModeStates")]
	[TemplateVisualState(Name = "OpenOverlayLeft        ", GroupName = "DisplayModeStates")]
	[TemplateVisualState(Name = "OpenOverlayRight       ", GroupName = "DisplayModeStates")]
	[TemplateVisualState(Name = "OpenInlineLeft         ", GroupName = "DisplayModeStates")]
	[TemplateVisualState(Name = "OpenInlineRight        ", GroupName = "DisplayModeStates")]
	[TemplateVisualState(Name = "OpenCompactOverlayLeft ", GroupName = "DisplayModeStates")]
	[TemplateVisualState(Name = "OpenCompactOverlayRight", GroupName = "DisplayModeStates")]
	[ContentProperty("Content")]
	public class SplitView : Control
	{
		public static readonly DependencyProperty CompactPaneLengthProperty = DependencyProperty.Register("CompactPaneLength", typeof(double), typeof(SplitView), new PropertyMetadata(0.0, OnMetricsChanged));

		public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(UIElement), typeof(SplitView), new PropertyMetadata(null));

		public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(SplitViewDisplayMode), typeof(SplitView), new PropertyMetadata(SplitViewDisplayMode.Overlay, OnStateChanged));

		public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(SplitView), new PropertyMetadata(false, OnIsPaneOpenChanged));

		public static readonly DependencyProperty OpenPaneLengthProperty = DependencyProperty.Register("OpenPaneLength", typeof(double), typeof(SplitView), new PropertyMetadata(0.0, OnMetricsChanged));

		public static readonly DependencyProperty PaneProperty = DependencyProperty.Register("Pane", typeof(UIElement), typeof(SplitView), new PropertyMetadata(null));

		public static readonly DependencyProperty PaneBackgroundProperty = DependencyProperty.Register("PaneBackground", typeof(Brush), typeof(SplitView), new PropertyMetadata(null));

		public static readonly DependencyProperty PaneForegroundProperty = DependencyProperty.Register("PaneForeground", typeof(Brush), typeof(SplitView), new PropertyMetadata(null));

		public static readonly DependencyProperty PanePlacementProperty = DependencyProperty.Register("PanePlacement", typeof(SplitViewPanePlacement), typeof(SplitView), new PropertyMetadata(SplitViewPanePlacement.Left, OnStateChanged));

		public static readonly DependencyProperty TemplateSettingsProperty = DependencyProperty.Register("TemplateSettings", typeof(SplitViewTemplateSettings), typeof(SplitView), new PropertyMetadata(null));

		private Rectangle lightDismissLayer;

		private RectangleGeometry paneClipRectangle;

		public double CompactPaneLength
		{
			get
			{
				return (double)GetValue(CompactPaneLengthProperty);
			}
			set
			{
				SetValue(CompactPaneLengthProperty, value);
			}
		}

		public UIElement Content
		{
			get
			{
				return (UIElement)GetValue(ContentProperty);
			}
			set
			{
				SetValue(ContentProperty, value);
			}
		}

		public SplitViewDisplayMode DisplayMode
		{
			get
			{
				return (SplitViewDisplayMode)GetValue(DisplayModeProperty);
			}
			set
			{
				SetValue(DisplayModeProperty, value);
			}
		}

		public bool IsPaneOpen
		{
			get
			{
				return (bool)GetValue(IsPaneOpenProperty);
			}
			set
			{
				SetValue(IsPaneOpenProperty, value);
			}
		}

		public double OpenPaneLength
		{
			get
			{
				return (double)GetValue(OpenPaneLengthProperty);
			}
			set
			{
				SetValue(OpenPaneLengthProperty, value);
			}
		}

		public UIElement Pane
		{
			get
			{
				return (UIElement)GetValue(PaneProperty);
			}
			set
			{
				SetValue(PaneProperty, value);
			}
		}

		public Brush PaneBackground
		{
			get
			{
				return (Brush)GetValue(PaneBackgroundProperty);
			}
			set
			{
				SetValue(PaneBackgroundProperty, value);
			}
		}

		public Brush PaneForeground
		{
			get
			{
				return (Brush)GetValue(PaneForegroundProperty);
			}
			set
			{
				SetValue(PaneForegroundProperty, value);
			}
		}

		public SplitViewPanePlacement PanePlacement
		{
			get
			{
				return (SplitViewPanePlacement)GetValue(PanePlacementProperty);
			}
			set
			{
				SetValue(PanePlacementProperty, value);
			}
		}

		public SplitViewTemplateSettings TemplateSettings
		{
			get
			{
				return (SplitViewTemplateSettings)GetValue(TemplateSettingsProperty);
			}
			private set
			{
				SetValue(TemplateSettingsProperty, value);
			}
		}

		public event EventHandler PaneClosed;

		public event EventHandler<SplitViewPaneClosingEventArgs> PaneClosing;

		private static void OnMetricsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SplitView obj = d as SplitView;
			obj?.TemplateSettings?.Update();
			obj?.ChangeVisualState();
		}

		private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as SplitView)?.ChangeVisualState();
		}

		private static void OnIsPaneOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SplitView splitView = d as SplitView;
			bool flag = (bool)e.NewValue;
			bool flag2 = (bool)e.OldValue;
			if (splitView != null && flag != flag2)
			{
				if (flag)
				{
					splitView.ChangeVisualState();
				}
				else
				{
					splitView.OnIsPaneOpenChanged();
				}
			}
		}

		public SplitView()
		{
			base.DefaultStyleKey = typeof(SplitView);
			TemplateSettings = new SplitViewTemplateSettings(this);
			base.Loaded += delegate
			{
				TemplateSettings.Update();
				ChangeVisualState(animated: false);
			};
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			paneClipRectangle = (GetTemplateChild("PaneClipRectangle") as RectangleGeometry);
			lightDismissLayer = (GetTemplateChild("LightDismissLayer") as Rectangle);
			if (lightDismissLayer != null)
			{
				lightDismissLayer.MouseDown += OnLightDismiss;
			}
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo info)
		{
			base.OnRenderSizeChanged(info);
			if (paneClipRectangle != null)
			{
				paneClipRectangle.Rect = new Rect(0.0, 0.0, OpenPaneLength, base.ActualHeight);
			}
		}

		protected virtual void ChangeVisualState(bool animated = true)
		{
			if (paneClipRectangle != null)
			{
				paneClipRectangle.Rect = new Rect(0.0, 0.0, OpenPaneLength, base.ActualHeight);
			}
			string empty = string.Empty;
			if (IsPaneOpen)
			{
				empty += "Open";
				SplitViewDisplayMode displayMode = DisplayMode;
				empty = ((displayMode != SplitViewDisplayMode.CompactInline) ? (empty + DisplayMode.ToString()) : (empty + "Inline"));
				empty += PanePlacement.ToString();
			}
			else
			{
				empty += "Closed";
				if (DisplayMode == SplitViewDisplayMode.CompactInline || DisplayMode == SplitViewDisplayMode.CompactOverlay)
				{
					empty += "Compact";
					empty += PanePlacement.ToString();
				}
			}
			VisualStateManager.GoToState(this, "None", animated);
			VisualStateManager.GoToState(this, empty, animated);
		}

		protected virtual void OnIsPaneOpenChanged()
		{
			bool flag = false;
			if (this.PaneClosing != null)
			{
				SplitViewPaneClosingEventArgs splitViewPaneClosingEventArgs = new SplitViewPaneClosingEventArgs();
				Delegate[] invocationList = this.PaneClosing.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					EventHandler<SplitViewPaneClosingEventArgs> eventHandler = invocationList[i] as EventHandler<SplitViewPaneClosingEventArgs>;
					if (eventHandler != null)
					{
						eventHandler(this, splitViewPaneClosingEventArgs);
						if (splitViewPaneClosingEventArgs.Cancel)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag)
			{
				ChangeVisualState();
				this.PaneClosed?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				SetCurrentValue(IsPaneOpenProperty, false);
			}
		}

		private void OnLightDismiss(object sender, MouseButtonEventArgs e)
		{
			SetCurrentValue(IsPaneOpenProperty, false);
		}
	}
}
