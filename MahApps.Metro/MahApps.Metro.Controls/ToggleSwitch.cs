using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace MahApps.Metro.Controls
{
	[TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
	[TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
	[TemplatePart(Name = "Switch", Type = typeof(ToggleButton))]
	public class ToggleSwitch : HeaderedContentControl
	{
		private const string CommonStates = "CommonStates";

		private const string NormalState = "Normal";

		private const string DisabledState = "Disabled";

		private const string SwitchPart = "Switch";

		private ToggleButton _toggleButton;

		public static readonly DependencyProperty HeaderFontFamilyProperty;

		public static readonly DependencyProperty OnLabelProperty;

		public static readonly DependencyProperty OffLabelProperty;

		[Obsolete("This property will be deleted in the next release. You should use OnSwitchBrush and OffSwitchBrush to change the switch's brushes.")]
		public static readonly DependencyProperty SwitchForegroundProperty;

		public static readonly DependencyProperty OnSwitchBrushProperty;

		public static readonly DependencyProperty OffSwitchBrushProperty;

		public static readonly DependencyProperty ThumbIndicatorBrushProperty;

		public static readonly DependencyProperty ThumbIndicatorDisabledBrushProperty;

		public static readonly DependencyProperty ThumbIndicatorWidthProperty;

		public static readonly DependencyProperty IsCheckedProperty;

		public static readonly DependencyProperty CheckChangedCommandProperty;

		public static readonly DependencyProperty CheckedCommandProperty;

		public static readonly DependencyProperty UnCheckedCommandProperty;

		public static readonly DependencyProperty CheckChangedCommandParameterProperty;

		public static readonly DependencyProperty CheckedCommandParameterProperty;

		public static readonly DependencyProperty UnCheckedCommandParameterProperty;

		public static readonly DependencyProperty ContentDirectionProperty;

		public static readonly DependencyProperty ContentPaddingProperty;

		public static readonly DependencyProperty ToggleSwitchButtonStyleProperty;

		[Bindable(true)]
		[Localizability(LocalizationCategory.Font)]
		public FontFamily HeaderFontFamily
		{
			get
			{
				return (FontFamily)GetValue(HeaderFontFamilyProperty);
			}
			set
			{
				SetValue(HeaderFontFamilyProperty, value);
			}
		}

		public string OnLabel
		{
			get
			{
				return (string)GetValue(OnLabelProperty);
			}
			set
			{
				SetValue(OnLabelProperty, value);
			}
		}

		public string OffLabel
		{
			get
			{
				return (string)GetValue(OffLabelProperty);
			}
			set
			{
				SetValue(OffLabelProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use OnSwitchBrush and OffSwitchBrush to change the switch's brushes.")]
		public Brush SwitchForeground
		{
			get
			{
				return (Brush)GetValue(SwitchForegroundProperty);
			}
			set
			{
				SetValue(SwitchForegroundProperty, value);
			}
		}

		public Brush OnSwitchBrush
		{
			get
			{
				return (Brush)GetValue(OnSwitchBrushProperty);
			}
			set
			{
				SetValue(OnSwitchBrushProperty, value);
			}
		}

		public Brush OffSwitchBrush
		{
			get
			{
				return (Brush)GetValue(OffSwitchBrushProperty);
			}
			set
			{
				SetValue(OffSwitchBrushProperty, value);
			}
		}

		public Brush ThumbIndicatorBrush
		{
			get
			{
				return (Brush)GetValue(ThumbIndicatorBrushProperty);
			}
			set
			{
				SetValue(ThumbIndicatorBrushProperty, value);
			}
		}

		public Brush ThumbIndicatorDisabledBrush
		{
			get
			{
				return (Brush)GetValue(ThumbIndicatorDisabledBrushProperty);
			}
			set
			{
				SetValue(ThumbIndicatorDisabledBrushProperty, value);
			}
		}

		public double ThumbIndicatorWidth
		{
			get
			{
				return (double)GetValue(ThumbIndicatorWidthProperty);
			}
			set
			{
				SetValue(ThumbIndicatorWidthProperty, value);
			}
		}

		public FlowDirection ContentDirection
		{
			get
			{
				return (FlowDirection)GetValue(ContentDirectionProperty);
			}
			set
			{
				SetValue(ContentDirectionProperty, value);
			}
		}

		[Bindable(true)]
		[Category("MahApps.Metro")]
		public Thickness ContentPadding
		{
			get
			{
				return (Thickness)GetValue(ContentPaddingProperty);
			}
			set
			{
				SetValue(ContentPaddingProperty, value);
			}
		}

		public Style ToggleSwitchButtonStyle
		{
			get
			{
				return (Style)GetValue(ToggleSwitchButtonStyleProperty);
			}
			set
			{
				SetValue(ToggleSwitchButtonStyleProperty, value);
			}
		}

		[TypeConverter(typeof(NullableBoolConverter))]
		public bool? IsChecked
		{
			get
			{
				return (bool?)GetValue(IsCheckedProperty);
			}
			set
			{
				SetValue(IsCheckedProperty, value);
			}
		}

		public ICommand CheckChangedCommand
		{
			get
			{
				return (ICommand)GetValue(CheckChangedCommandProperty);
			}
			set
			{
				SetValue(CheckChangedCommandProperty, value);
			}
		}

		public ICommand CheckedCommand
		{
			get
			{
				return (ICommand)GetValue(CheckedCommandProperty);
			}
			set
			{
				SetValue(CheckedCommandProperty, value);
			}
		}

		public ICommand UnCheckedCommand
		{
			get
			{
				return (ICommand)GetValue(UnCheckedCommandProperty);
			}
			set
			{
				SetValue(UnCheckedCommandProperty, value);
			}
		}

		public object CheckChangedCommandParameter
		{
			get
			{
				return GetValue(CheckChangedCommandParameterProperty);
			}
			set
			{
				SetValue(CheckChangedCommandParameterProperty, value);
			}
		}

		public object CheckedCommandParameter
		{
			get
			{
				return GetValue(CheckedCommandParameterProperty);
			}
			set
			{
				SetValue(CheckedCommandParameterProperty, value);
			}
		}

		public object UnCheckedCommandParameter
		{
			get
			{
				return GetValue(UnCheckedCommandParameterProperty);
			}
			set
			{
				SetValue(UnCheckedCommandParameterProperty, value);
			}
		}

		public event EventHandler<RoutedEventArgs> Checked;

		public event EventHandler<RoutedEventArgs> Unchecked;

		public event EventHandler<RoutedEventArgs> Indeterminate;

		public event EventHandler<RoutedEventArgs> Click;

		public event EventHandler IsCheckedChanged;

		private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ToggleSwitch toggleSwitch = (ToggleSwitch)d;
			if (toggleSwitch._toggleButton != null && (bool?)e.OldValue != (bool?)e.NewValue)
			{
				ICommand checkChangedCommand = toggleSwitch.CheckChangedCommand;
				object parameter = toggleSwitch.CheckChangedCommandParameter ?? toggleSwitch;
				if (checkChangedCommand != null && checkChangedCommand.CanExecute(parameter))
				{
					checkChangedCommand.Execute(parameter);
				}
				toggleSwitch.IsCheckedChanged?.Invoke(toggleSwitch, EventArgs.Empty);
			}
		}

		static ToggleSwitch()
		{
			HeaderFontFamilyProperty = DependencyProperty.Register("HeaderFontFamily", typeof(FontFamily), typeof(ToggleSwitch), new PropertyMetadata(SystemFonts.MessageFontFamily));
			OnLabelProperty = DependencyProperty.Register("OnLabel", typeof(string), typeof(ToggleSwitch), new PropertyMetadata("On"));
			OffLabelProperty = DependencyProperty.Register("OffLabel", typeof(string), typeof(ToggleSwitch), new PropertyMetadata("Off"));
			SwitchForegroundProperty = DependencyProperty.Register("SwitchForeground", typeof(Brush), typeof(ToggleSwitch), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
			{
				((ToggleSwitch)o).OnSwitchBrush = (e.NewValue as Brush);
			}));
			OnSwitchBrushProperty = DependencyProperty.Register("OnSwitchBrush", typeof(Brush), typeof(ToggleSwitch), null);
			OffSwitchBrushProperty = DependencyProperty.Register("OffSwitchBrush", typeof(Brush), typeof(ToggleSwitch), null);
			ThumbIndicatorBrushProperty = DependencyProperty.Register("ThumbIndicatorBrush", typeof(Brush), typeof(ToggleSwitch), null);
			ThumbIndicatorDisabledBrushProperty = DependencyProperty.Register("ThumbIndicatorDisabledBrush", typeof(Brush), typeof(ToggleSwitch), null);
			ThumbIndicatorWidthProperty = DependencyProperty.Register("ThumbIndicatorWidth", typeof(double), typeof(ToggleSwitch), new PropertyMetadata(13.0));
			IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool?), typeof(ToggleSwitch), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, OnIsCheckedChanged));
			CheckChangedCommandProperty = DependencyProperty.Register("CheckChangedCommand", typeof(ICommand), typeof(ToggleSwitch), new PropertyMetadata(null));
			CheckedCommandProperty = DependencyProperty.Register("CheckedCommand", typeof(ICommand), typeof(ToggleSwitch), new PropertyMetadata(null));
			UnCheckedCommandProperty = DependencyProperty.Register("UnCheckedCommand", typeof(ICommand), typeof(ToggleSwitch), new PropertyMetadata(null));
			CheckChangedCommandParameterProperty = DependencyProperty.Register("CheckChangedCommandParameter", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(null));
			CheckedCommandParameterProperty = DependencyProperty.Register("CheckedCommandParameter", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(null));
			UnCheckedCommandParameterProperty = DependencyProperty.Register("UnCheckedCommandParameter", typeof(object), typeof(ToggleSwitch), new PropertyMetadata(null));
			ContentDirectionProperty = DependencyProperty.Register("ContentDirection", typeof(FlowDirection), typeof(ToggleSwitch), new PropertyMetadata(FlowDirection.LeftToRight));
			ContentPaddingProperty = DependencyProperty.Register("ContentPadding", typeof(Thickness), typeof(ToggleSwitch), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsParentMeasure));
			ToggleSwitchButtonStyleProperty = DependencyProperty.Register("ToggleSwitchButtonStyle", typeof(Style), typeof(ToggleSwitch), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitch), new FrameworkPropertyMetadata(typeof(ToggleSwitch)));
		}

		public ToggleSwitch()
		{
			base.PreviewKeyUp += ToggleSwitch_PreviewKeyUp;
			base.MouseUp += delegate
			{
				Keyboard.Focus(this);
			};
		}

		private void ToggleSwitch_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space && e.OriginalSource == sender)
			{
				SetCurrentValue(IsCheckedProperty, !IsChecked);
			}
		}

		private void ChangeVisualState(bool useTransitions)
		{
			VisualStateManager.GoToState(this, base.IsEnabled ? "Normal" : "Disabled", useTransitions);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (_toggleButton != null)
			{
				_toggleButton.Checked -= CheckedHandler;
				_toggleButton.Unchecked -= UncheckedHandler;
				_toggleButton.Indeterminate -= IndeterminateHandler;
				_toggleButton.Click -= ClickHandler;
				BindingOperations.ClearBinding(_toggleButton, ToggleButton.IsCheckedProperty);
				_toggleButton.IsEnabledChanged -= IsEnabledHandler;
				_toggleButton.PreviewMouseUp -= ToggleButtonPreviewMouseUp;
			}
			_toggleButton = (GetTemplateChild("Switch") as ToggleButton);
			if (_toggleButton != null)
			{
				_toggleButton.Checked += CheckedHandler;
				_toggleButton.Unchecked += UncheckedHandler;
				_toggleButton.Indeterminate += IndeterminateHandler;
				_toggleButton.Click += ClickHandler;
				Binding binding = new Binding("IsChecked")
				{
					Source = this
				};
				_toggleButton.SetBinding(ToggleButton.IsCheckedProperty, binding);
				_toggleButton.IsEnabledChanged += IsEnabledHandler;
				_toggleButton.PreviewMouseUp += ToggleButtonPreviewMouseUp;
			}
			ChangeVisualState(useTransitions: false);
		}

		private void ToggleButtonPreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			Keyboard.Focus(this);
		}

		private void IsEnabledHandler(object sender, DependencyPropertyChangedEventArgs e)
		{
			ChangeVisualState(useTransitions: false);
		}

		private void CheckedHandler(object sender, RoutedEventArgs e)
		{
			ICommand checkedCommand = CheckedCommand;
			object parameter = CheckedCommandParameter ?? this;
			if (checkedCommand != null && checkedCommand.CanExecute(parameter))
			{
				checkedCommand.Execute(parameter);
			}
			SafeRaise.Raise(this.Checked, this, e);
		}

		private void UncheckedHandler(object sender, RoutedEventArgs e)
		{
			ICommand unCheckedCommand = UnCheckedCommand;
			object parameter = UnCheckedCommandParameter ?? this;
			if (unCheckedCommand != null && unCheckedCommand.CanExecute(parameter))
			{
				unCheckedCommand.Execute(parameter);
			}
			SafeRaise.Raise(this.Unchecked, this, e);
		}

		private void IndeterminateHandler(object sender, RoutedEventArgs e)
		{
			SafeRaise.Raise(this.Indeterminate, this, e);
		}

		private void ClickHandler(object sender, RoutedEventArgs e)
		{
			SafeRaise.Raise(this.Click, this, e);
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{{ToggleSwitch IsChecked={0}, Content={1}}}", IsChecked, base.Content);
		}
	}
}
