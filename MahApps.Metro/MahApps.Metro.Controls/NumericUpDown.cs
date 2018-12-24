using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_NumericUp", Type = typeof(RepeatButton))]
	[TemplatePart(Name = "PART_NumericDown", Type = typeof(RepeatButton))]
	[TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
	public class NumericUpDown : Control
	{
		public static readonly RoutedEvent ValueIncrementedEvent;

		public static readonly RoutedEvent ValueDecrementedEvent;

		public static readonly RoutedEvent DelayChangedEvent;

		public static readonly RoutedEvent MaximumReachedEvent;

		public static readonly RoutedEvent MinimumReachedEvent;

		public static readonly RoutedEvent ValueChangedEvent;

		public static readonly DependencyProperty DelayProperty;

		public static readonly DependencyProperty TextAlignmentProperty;

		public static readonly DependencyProperty SpeedupProperty;

		public static readonly DependencyProperty IsReadOnlyProperty;

		public static readonly DependencyProperty StringFormatProperty;

		public static readonly DependencyProperty InterceptArrowKeysProperty;

		public static readonly DependencyProperty ValueProperty;

		public static readonly DependencyProperty ButtonsAlignmentProperty;

		public static readonly DependencyProperty MinimumProperty;

		public static readonly DependencyProperty MaximumProperty;

		public static readonly DependencyProperty IntervalProperty;

		public static readonly DependencyProperty InterceptMouseWheelProperty;

		public static readonly DependencyProperty TrackMouseWheelWhenMouseOverProperty;

		public static readonly DependencyProperty HideUpDownButtonsProperty;

		public static readonly DependencyProperty UpDownButtonsWidthProperty;

		public static readonly DependencyProperty InterceptManualEnterProperty;

		public static readonly DependencyProperty CultureProperty;

		[Obsolete("This property will be deleted in the next release. You should use TextBoxHelper.SelectAllOnFocus instead.")]
		public static readonly DependencyProperty SelectAllOnFocusProperty;

		[Obsolete("This property will be deleted in the next release. Please use the new NumericInputMode property instead.")]
		public static readonly DependencyProperty HasDecimalsProperty;

		public static readonly DependencyProperty NumericInputModeProperty;

		public static readonly DependencyProperty SnapToMultipleOfIntervalProperty;

		private static readonly Regex RegexStringFormatHexadecimal;

		private static readonly Regex RegexStringFormatNumber;

		private const double DefaultInterval = 1.0;

		private const int DefaultDelay = 500;

		private const string ElementNumericDown = "PART_NumericDown";

		private const string ElementNumericUp = "PART_NumericUp";

		private const string ElementTextBox = "PART_TextBox";

		private const string ScientificNotationChar = "E";

		private const StringComparison StrComp = StringComparison.InvariantCultureIgnoreCase;

		private Lazy<PropertyInfo> _handlesMouseWheelScrolling = new Lazy<PropertyInfo>();

		private double _internalIntervalMultiplierForCalculation = 1.0;

		private double _internalLargeChange = 100.0;

		private double _intervalValueSinceReset;

		private bool _manualChange;

		private RepeatButton _repeatDown;

		private RepeatButton _repeatUp;

		private TextBox _valueTextBox;

		private ScrollViewer _scrollViewer;

		[Bindable(true)]
		[DefaultValue(500)]
		[Category("Behavior")]
		public int Delay
		{
			get
			{
				return (int)GetValue(DelayProperty);
			}
			set
			{
				SetValue(DelayProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		[DefaultValue(true)]
		public bool InterceptArrowKeys
		{
			get
			{
				return (bool)GetValue(InterceptArrowKeysProperty);
			}
			set
			{
				SetValue(InterceptArrowKeysProperty, value);
			}
		}

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool InterceptMouseWheel
		{
			get
			{
				return (bool)GetValue(InterceptMouseWheelProperty);
			}
			set
			{
				SetValue(InterceptMouseWheelProperty, value);
			}
		}

		[Category("Behavior")]
		[DefaultValue(false)]
		public bool TrackMouseWheelWhenMouseOver
		{
			get
			{
				return (bool)GetValue(TrackMouseWheelWhenMouseOverProperty);
			}
			set
			{
				SetValue(TrackMouseWheelWhenMouseOverProperty, value);
			}
		}

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool InterceptManualEnter
		{
			get
			{
				return (bool)GetValue(InterceptManualEnterProperty);
			}
			set
			{
				SetValue(InterceptManualEnterProperty, value);
			}
		}

		[Category("Behavior")]
		[DefaultValue(null)]
		public CultureInfo Culture
		{
			get
			{
				return (CultureInfo)GetValue(CultureProperty);
			}
			set
			{
				SetValue(CultureProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		public bool HideUpDownButtons
		{
			get
			{
				return (bool)GetValue(HideUpDownButtonsProperty);
			}
			set
			{
				SetValue(HideUpDownButtonsProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(20.0)]
		public double UpDownButtonsWidth
		{
			get
			{
				return (double)GetValue(UpDownButtonsWidthProperty);
			}
			set
			{
				SetValue(UpDownButtonsWidthProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(ButtonsAlignment.Right)]
		public ButtonsAlignment ButtonsAlignment
		{
			get
			{
				return (ButtonsAlignment)GetValue(ButtonsAlignmentProperty);
			}
			set
			{
				SetValue(ButtonsAlignmentProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		[DefaultValue(1.0)]
		public double Interval
		{
			get
			{
				return (double)GetValue(IntervalProperty);
			}
			set
			{
				SetValue(IntervalProperty, value);
			}
		}

		[Obsolete("This property will be deleted in the next release. You should use Controls:TextBoxHelper.SelectAllOnFocus instead.")]
		[Bindable(true)]
		[Category("Behavior")]
		[DefaultValue(true)]
		public bool SelectAllOnFocus
		{
			get
			{
				return (bool)GetValue(SelectAllOnFocusProperty);
			}
			set
			{
				SetValue(SelectAllOnFocusProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(false)]
		public bool IsReadOnly
		{
			get
			{
				return (bool)GetValue(IsReadOnlyProperty);
			}
			set
			{
				SetValue(IsReadOnlyProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		[DefaultValue(double.MaxValue)]
		public double Maximum
		{
			get
			{
				return (double)GetValue(MaximumProperty);
			}
			set
			{
				SetValue(MaximumProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		[DefaultValue(double.MinValue)]
		public double Minimum
		{
			get
			{
				return (double)GetValue(MinimumProperty);
			}
			set
			{
				SetValue(MinimumProperty, value);
			}
		}

		[Category("Common")]
		[DefaultValue(true)]
		public bool Speedup
		{
			get
			{
				return (bool)GetValue(SpeedupProperty);
			}
			set
			{
				SetValue(SpeedupProperty, value);
			}
		}

		[Category("Common")]
		public string StringFormat
		{
			get
			{
				return (string)GetValue(StringFormatProperty);
			}
			set
			{
				SetValue(StringFormatProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		[DefaultValue(TextAlignment.Right)]
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)GetValue(TextAlignmentProperty);
			}
			set
			{
				SetValue(TextAlignmentProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		[DefaultValue(null)]
		public double? Value
		{
			get
			{
				return (double?)GetValue(ValueProperty);
			}
			set
			{
				SetValue(ValueProperty, value);
			}
		}

		private CultureInfo SpecificCultureInfo => Culture ?? base.Language.GetSpecificCulture();

		[Obsolete("This property will be deleted in the next release. Please use the new NumericInputMode property instead.")]
		[Bindable(true)]
		[Category("Common")]
		[DefaultValue(true)]
		public bool HasDecimals
		{
			get
			{
				return (bool)GetValue(HasDecimalsProperty);
			}
			set
			{
				SetValue(HasDecimalsProperty, value);
			}
		}

		[Category("Common")]
		[DefaultValue(NumericInput.All)]
		public NumericInput NumericInputMode
		{
			get
			{
				return (NumericInput)GetValue(NumericInputModeProperty);
			}
			set
			{
				SetValue(NumericInputModeProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		[DefaultValue(false)]
		public bool SnapToMultipleOfInterval
		{
			get
			{
				return (bool)GetValue(SnapToMultipleOfIntervalProperty);
			}
			set
			{
				SetValue(SnapToMultipleOfIntervalProperty, value);
			}
		}

		public event RoutedPropertyChangedEventHandler<double?> ValueChanged
		{
			add
			{
				AddHandler(ValueChangedEvent, value);
			}
			remove
			{
				RemoveHandler(ValueChangedEvent, value);
			}
		}

		public event RoutedEventHandler MaximumReached
		{
			add
			{
				AddHandler(MaximumReachedEvent, value);
			}
			remove
			{
				RemoveHandler(MaximumReachedEvent, value);
			}
		}

		public event RoutedEventHandler MinimumReached
		{
			add
			{
				AddHandler(MinimumReachedEvent, value);
			}
			remove
			{
				RemoveHandler(MinimumReachedEvent, value);
			}
		}

		public event NumericUpDownChangedRoutedEventHandler ValueIncremented
		{
			add
			{
				AddHandler(ValueIncrementedEvent, value);
			}
			remove
			{
				RemoveHandler(ValueIncrementedEvent, value);
			}
		}

		public event NumericUpDownChangedRoutedEventHandler ValueDecremented
		{
			add
			{
				AddHandler(ValueDecrementedEvent, value);
			}
			remove
			{
				RemoveHandler(ValueDecrementedEvent, value);
			}
		}

		public event RoutedEventHandler DelayChanged
		{
			add
			{
				AddHandler(DelayChangedEvent, value);
			}
			remove
			{
				RemoveHandler(DelayChangedEvent, value);
			}
		}

		private static void IsReadOnlyPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue && e.NewValue != null)
			{
				NumericUpDown numericUpDown = (NumericUpDown)dependencyObject;
				bool flag = (bool)e.NewValue;
				numericUpDown.ToggleReadOnlyMode(flag || !numericUpDown.InterceptManualEnter);
			}
		}

		private static void InterceptManualEnterChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue && e.NewValue != null)
			{
				NumericUpDown numericUpDown = (NumericUpDown)dependencyObject;
				bool flag = (bool)e.NewValue;
				numericUpDown.ToggleReadOnlyMode(!flag || numericUpDown.IsReadOnly);
			}
		}

		static NumericUpDown()
		{
			ValueIncrementedEvent = EventManager.RegisterRoutedEvent("ValueIncremented", RoutingStrategy.Bubble, typeof(NumericUpDownChangedRoutedEventHandler), typeof(NumericUpDown));
			ValueDecrementedEvent = EventManager.RegisterRoutedEvent("ValueDecremented", RoutingStrategy.Bubble, typeof(NumericUpDownChangedRoutedEventHandler), typeof(NumericUpDown));
			DelayChangedEvent = EventManager.RegisterRoutedEvent("DelayChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericUpDown));
			MaximumReachedEvent = EventManager.RegisterRoutedEvent("MaximumReached", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericUpDown));
			MinimumReachedEvent = EventManager.RegisterRoutedEvent("MinimumReached", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericUpDown));
			ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double?>), typeof(NumericUpDown));
			DelayProperty = DependencyProperty.Register("Delay", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(500, OnDelayChanged), ValidateDelay);
			TextAlignmentProperty = TextBox.TextAlignmentProperty.AddOwner(typeof(NumericUpDown));
			SpeedupProperty = DependencyProperty.Register("Speedup", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(true, OnSpeedupChanged));
			IsReadOnlyProperty = TextBoxBase.IsReadOnlyProperty.AddOwner(typeof(NumericUpDown), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, IsReadOnlyPropertyChangedCallback));
			StringFormatProperty = DependencyProperty.Register("StringFormat", typeof(string), typeof(NumericUpDown), new FrameworkPropertyMetadata(string.Empty, OnStringFormatChanged, CoerceStringFormat));
			InterceptArrowKeysProperty = DependencyProperty.Register("InterceptArrowKeys", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(true));
			ValueProperty = DependencyProperty.Register("Value", typeof(double?), typeof(NumericUpDown), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, CoerceValue));
			ButtonsAlignmentProperty = DependencyProperty.Register("ButtonsAlignment", typeof(ButtonsAlignment), typeof(NumericUpDown), new FrameworkPropertyMetadata(ButtonsAlignment.Right, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
			MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(-1.7976931348623157E+308, OnMinimumChanged));
			MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(1.7976931348623157E+308, OnMaximumChanged, CoerceMaximum));
			IntervalProperty = DependencyProperty.Register("Interval", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(1.0, IntervalChanged));
			InterceptMouseWheelProperty = DependencyProperty.Register("InterceptMouseWheel", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(true));
			TrackMouseWheelWhenMouseOverProperty = DependencyProperty.Register("TrackMouseWheelWhenMouseOver", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(false));
			HideUpDownButtonsProperty = DependencyProperty.Register("HideUpDownButtons", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false));
			UpDownButtonsWidthProperty = DependencyProperty.Register("UpDownButtonsWidth", typeof(double), typeof(NumericUpDown), new PropertyMetadata(20.0));
			InterceptManualEnterProperty = DependencyProperty.Register("InterceptManualEnter", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(true, InterceptManualEnterChangedCallback));
			CultureProperty = DependencyProperty.Register("Culture", typeof(CultureInfo), typeof(NumericUpDown), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
			{
				if (e.NewValue != e.OldValue)
				{
					NumericUpDown numericUpDown = (NumericUpDown)o;
					numericUpDown.OnValueChanged(numericUpDown.Value, numericUpDown.Value);
				}
			}));
			SelectAllOnFocusProperty = DependencyProperty.Register("SelectAllOnFocus", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(true, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
			{
				TextBoxHelper.SetSelectAllOnFocus(o, (bool)e.NewValue);
			}));
			HasDecimalsProperty = DependencyProperty.Register("HasDecimals", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(true, OnHasDecimalsChanged));
			NumericInputModeProperty = DependencyProperty.Register("NumericInputMode", typeof(NumericInput), typeof(NumericUpDown), new FrameworkPropertyMetadata(NumericInput.All, OnNumericInputModeChanged));
			SnapToMultipleOfIntervalProperty = DependencyProperty.Register("SnapToMultipleOfInterval", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false, OnSnapToMultipleOfIntervalChanged));
			RegexStringFormatHexadecimal = new Regex("^(?<complexHEX>.*{\\d:X\\d+}.*)?(?<simpleHEX>X\\d+)?$", RegexOptions.Compiled);
			RegexStringFormatNumber = new Regex("[-+]?(?<![0-9][.,])\\b[0-9]+(?:[.,\\s][0-9]+)*[.,]?[0-9]?(?:[eE][-+]?[0-9]+)?\\b(?!\\.[0-9])", RegexOptions.Compiled);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
			Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(VerticalAlignment.Center));
			Control.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(HorizontalAlignment.Right));
			EventManager.RegisterClassHandler(typeof(NumericUpDown), UIElement.GotFocusEvent, new RoutedEventHandler(OnGotFocus));
		}

		private static void OnGotFocus(object sender, RoutedEventArgs e)
		{
			if (!e.Handled)
			{
				NumericUpDown numericUpDown = (NumericUpDown)sender;
				if ((numericUpDown.InterceptManualEnter || numericUpDown.IsReadOnly) && numericUpDown.Focusable && e.OriginalSource == numericUpDown)
				{
					TraversalRequest request = new TraversalRequest(((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next);
					UIElement uIElement = Keyboard.FocusedElement as UIElement;
					if (uIElement != null)
					{
						uIElement.MoveFocus(request);
					}
					else
					{
						numericUpDown.Focus();
					}
					e.Handled = true;
				}
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_repeatUp = (GetTemplateChild("PART_NumericUp") as RepeatButton);
			_repeatDown = (GetTemplateChild("PART_NumericDown") as RepeatButton);
			_valueTextBox = (GetTemplateChild("PART_TextBox") as TextBox);
			if (_repeatUp == null || _repeatDown == null || _valueTextBox == null)
			{
				throw new InvalidOperationException(string.Format("You have missed to specify {0}, {1} or {2} in your template", "PART_NumericUp", "PART_NumericDown", "PART_TextBox"));
			}
			ToggleReadOnlyMode(IsReadOnly | !InterceptManualEnter);
			_repeatUp.Click += delegate
			{
				ChangeValueWithSpeedUp(toPositive: true);
			};
			_repeatDown.Click += delegate
			{
				ChangeValueWithSpeedUp(toPositive: false);
			};
			_repeatUp.PreviewMouseUp += delegate
			{
				ResetInternal();
			};
			_repeatDown.PreviewMouseUp += delegate
			{
				ResetInternal();
			};
			OnValueChanged(Value, Value);
			_scrollViewer = TryFindScrollViewer();
		}

		private void ToggleReadOnlyMode(bool isReadOnly)
		{
			if (_repeatUp != null && _repeatDown != null && _valueTextBox != null)
			{
				if (isReadOnly)
				{
					_valueTextBox.LostFocus -= OnTextBoxLostFocus;
					_valueTextBox.PreviewTextInput -= OnPreviewTextInput;
					_valueTextBox.PreviewKeyDown -= OnTextBoxKeyDown;
					_valueTextBox.TextChanged -= OnTextChanged;
					DataObject.RemovePastingHandler(_valueTextBox, OnValueTextBoxPaste);
				}
				else
				{
					_valueTextBox.LostFocus += OnTextBoxLostFocus;
					_valueTextBox.PreviewTextInput += OnPreviewTextInput;
					_valueTextBox.PreviewKeyDown += OnTextBoxKeyDown;
					_valueTextBox.TextChanged += OnTextChanged;
					DataObject.AddPastingHandler(_valueTextBox, OnValueTextBoxPaste);
				}
			}
		}

		public void SelectAll()
		{
			if (_valueTextBox != null)
			{
				_valueTextBox.SelectAll();
			}
		}

		protected virtual void OnDelayChanged(int oldDelay, int newDelay)
		{
			if (oldDelay != newDelay)
			{
				if (_repeatDown != null)
				{
					_repeatDown.Delay = newDelay;
				}
				if (_repeatUp != null)
				{
					_repeatUp.Delay = newDelay;
				}
			}
		}

		protected virtual void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
		}

		protected virtual void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			if (InterceptArrowKeys)
			{
				switch (e.Key)
				{
				case Key.Up:
					ChangeValueWithSpeedUp(toPositive: true);
					e.Handled = true;
					break;
				case Key.Down:
					ChangeValueWithSpeedUp(toPositive: false);
					e.Handled = true;
					break;
				}
				if (e.Handled)
				{
					_manualChange = false;
					InternalSetText(Value);
				}
			}
		}

		protected override void OnPreviewKeyUp(KeyEventArgs e)
		{
			base.OnPreviewKeyUp(e);
			if (e.Key == Key.Down || e.Key == Key.Up)
			{
				ResetInternal();
			}
		}

		protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
		{
			base.OnPreviewMouseWheel(e);
			if (InterceptMouseWheel && (base.IsFocused || _valueTextBox.IsFocused || TrackMouseWheelWhenMouseOver))
			{
				bool addInterval = e.Delta > 0;
				_manualChange = false;
				ChangeValueInternal(addInterval);
			}
			if (_scrollViewer != null && _handlesMouseWheelScrolling.Value != null)
			{
				if (TrackMouseWheelWhenMouseOver)
				{
					_handlesMouseWheelScrolling.Value.SetValue(_scrollViewer, true, null);
				}
				else if (InterceptMouseWheel)
				{
					_handlesMouseWheelScrolling.Value.SetValue(_scrollViewer, _valueTextBox.IsFocused, null);
				}
				else
				{
					_handlesMouseWheelScrolling.Value.SetValue(_scrollViewer, true, null);
				}
			}
		}

		protected void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = true;
			if (!string.IsNullOrWhiteSpace(e.Text) && e.Text.Length == 1)
			{
				TextBox textBox = (TextBox)sender;
				CultureInfo equivalentCulture = SpecificCultureInfo;
				NumberFormatInfo numberFormatInfo = equivalentCulture.NumberFormat;
				string text = e.Text;
				if (char.IsDigit(text[0]))
				{
					if (textBox.Text.IndexOf(numberFormatInfo.NegativeSign, textBox.SelectionStart + textBox.SelectionLength, StringComparison.InvariantCultureIgnoreCase) < 0 && textBox.Text.IndexOf(numberFormatInfo.PositiveSign, textBox.SelectionStart + textBox.SelectionLength, StringComparison.InvariantCultureIgnoreCase) < 0)
					{
						e.Handled = false;
					}
				}
				else
				{
					bool flag = textBox.SelectedText == textBox.Text;
					if (numberFormatInfo.NumberDecimalSeparator == text)
					{
						if ((textBox.Text.All((char i) => i.ToString(equivalentCulture) != numberFormatInfo.NumberDecimalSeparator) | flag) && NumericInputMode.HasFlag(NumericInput.Decimal))
						{
							e.Handled = false;
						}
					}
					else if (numberFormatInfo.NegativeSign == text || text == numberFormatInfo.PositiveSign)
					{
						if (textBox.SelectionStart == 0)
						{
							if (textBox.Text.Length > 1)
							{
								if (flag || (!textBox.Text.StartsWith(numberFormatInfo.NegativeSign, StringComparison.InvariantCultureIgnoreCase) && !textBox.Text.StartsWith(numberFormatInfo.PositiveSign, StringComparison.InvariantCultureIgnoreCase)))
								{
									e.Handled = false;
								}
							}
							else
							{
								e.Handled = false;
							}
						}
						else if (textBox.SelectionStart > 0 && textBox.Text.ElementAt(textBox.SelectionStart - 1).ToString(equivalentCulture).Equals("E", StringComparison.InvariantCultureIgnoreCase) && NumericInputMode.HasFlag(NumericInput.Decimal))
						{
							e.Handled = false;
						}
					}
					else if (text.Equals("E", StringComparison.InvariantCultureIgnoreCase) && NumericInputMode.HasFlag(NumericInput.Decimal) && textBox.SelectionStart > 0 && !textBox.Text.Any((char i) => i.ToString(equivalentCulture).Equals("E", StringComparison.InvariantCultureIgnoreCase)))
					{
						e.Handled = false;
					}
				}
				_manualChange = (_manualChange || !e.Handled);
			}
		}

		protected virtual void OnSpeedupChanged(bool oldSpeedup, bool newSpeedup)
		{
		}

		protected virtual void OnValueChanged(double? oldValue, double? newValue)
		{
			if (!_manualChange)
			{
				if (!newValue.HasValue)
				{
					if (_valueTextBox != null)
					{
						_valueTextBox.Text = null;
					}
					if (oldValue != newValue)
					{
						RaiseEvent(new RoutedPropertyChangedEventArgs<double?>(oldValue, newValue, ValueChangedEvent));
					}
					return;
				}
				if (_repeatUp != null && !_repeatUp.IsEnabled)
				{
					_repeatUp.IsEnabled = true;
				}
				if (_repeatDown != null && !_repeatDown.IsEnabled)
				{
					_repeatDown.IsEnabled = true;
				}
				if (newValue <= Minimum)
				{
					if (_repeatDown != null)
					{
						_repeatDown.IsEnabled = false;
					}
					ResetInternal();
					if (base.IsLoaded)
					{
						RaiseEvent(new RoutedEventArgs(MinimumReachedEvent));
					}
				}
				if (newValue >= Maximum)
				{
					if (_repeatUp != null)
					{
						_repeatUp.IsEnabled = false;
					}
					ResetInternal();
					if (base.IsLoaded)
					{
						RaiseEvent(new RoutedEventArgs(MaximumReachedEvent));
					}
				}
				if (_valueTextBox != null)
				{
					InternalSetText(newValue);
				}
			}
			if (oldValue != newValue)
			{
				RaiseEvent(new RoutedPropertyChangedEventArgs<double?>(oldValue, newValue, ValueChangedEvent));
			}
		}

		private static object CoerceMaximum(DependencyObject d, object value)
		{
			double minimum = ((NumericUpDown)d).Minimum;
			double num = (double)value;
			return (num < minimum) ? minimum : num;
		}

		private static object CoerceStringFormat(DependencyObject d, object basevalue)
		{
			return basevalue ?? string.Empty;
		}

		private static object CoerceValue(DependencyObject d, object value)
		{
			if (value == null)
			{
				return null;
			}
			NumericUpDown numericUpDown = (NumericUpDown)d;
			double num = ((double?)value).Value;
			if (!numericUpDown.NumericInputMode.HasFlag(NumericInput.Decimal))
			{
				num = Math.Truncate(num);
			}
			if (num < numericUpDown.Minimum)
			{
				return numericUpDown.Minimum;
			}
			if (num > numericUpDown.Maximum)
			{
				return numericUpDown.Maximum;
			}
			return num;
		}

		private static void IntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((NumericUpDown)d).ResetInternal();
		}

		private static void OnDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown obj = (NumericUpDown)d;
			obj.RaiseChangeDelay();
			obj.OnDelayChanged((int)e.OldValue, (int)e.NewValue);
		}

		private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown obj = (NumericUpDown)d;
			obj.CoerceValue(ValueProperty);
			obj.OnMaximumChanged((double)e.OldValue, (double)e.NewValue);
			obj.EnableDisableUpDown();
		}

		private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown obj = (NumericUpDown)d;
			obj.CoerceValue(MaximumProperty);
			obj.CoerceValue(ValueProperty);
			obj.OnMinimumChanged((double)e.OldValue, (double)e.NewValue);
			obj.EnableDisableUpDown();
		}

		private static void OnSpeedupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((NumericUpDown)d).OnSpeedupChanged((bool)e.OldValue, (bool)e.NewValue);
		}

		private static void OnStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)d;
			if (numericUpDown._valueTextBox != null && numericUpDown.Value.HasValue)
			{
				numericUpDown.InternalSetText(numericUpDown.Value);
			}
			string text = (string)e.NewValue;
			if (!numericUpDown.NumericInputMode.HasFlag(NumericInput.Decimal) && !string.IsNullOrEmpty(text) && RegexStringFormatHexadecimal.IsMatch(text))
			{
				numericUpDown.SetCurrentValue(NumericInputModeProperty, numericUpDown.NumericInputMode | NumericInput.Decimal);
			}
		}

		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((NumericUpDown)d).OnValueChanged((double?)e.OldValue, (double?)e.NewValue);
		}

		private static void OnHasDecimalsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)d;
			if (e.NewValue != e.OldValue && e.NewValue is bool && numericUpDown.Value.HasValue)
			{
				bool num = (bool)e.NewValue;
				NumericInput numericInputMode = numericUpDown.NumericInputMode;
				if (!num)
				{
					numericUpDown.Value = Math.Truncate(numericUpDown.Value.GetValueOrDefault());
					numericInputMode &= ~NumericInput.Decimal;
				}
				else
				{
					numericInputMode |= NumericInput.Decimal;
				}
				numericUpDown.SetCurrentValue(NumericInputModeProperty, numericInputMode);
			}
		}

		private static void OnNumericInputModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)d;
			if (e.NewValue != e.OldValue && e.NewValue is NumericInput && numericUpDown.Value.HasValue)
			{
				NumericInput numericInput = (NumericInput)e.NewValue;
				if (!numericInput.HasFlag(NumericInput.Decimal))
				{
					numericUpDown.Value = Math.Truncate(numericUpDown.Value.GetValueOrDefault());
				}
			}
		}

		private static void OnSnapToMultipleOfIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NumericUpDown numericUpDown = (NumericUpDown)d;
			double valueOrDefault = numericUpDown.Value.GetValueOrDefault();
			if ((bool)e.NewValue && Math.Abs(numericUpDown.Interval) > 0.0)
			{
				numericUpDown.Value = Math.Round(valueOrDefault / numericUpDown.Interval) * numericUpDown.Interval;
			}
		}

		private static bool ValidateDelay(object value)
		{
			return Convert.ToInt32(value) >= 0;
		}

		private void InternalSetText(double? newValue)
		{
			if (!newValue.HasValue)
			{
				_valueTextBox.Text = null;
			}
			else
			{
				_valueTextBox.Text = FormattedValue(newValue, StringFormat, SpecificCultureInfo);
				if ((bool)GetValue(TextBoxHelper.IsMonitoringProperty))
				{
					SetValue(TextBoxHelper.TextLengthProperty, _valueTextBox.Text.Length);
				}
			}
		}

		private string FormattedValue(double? newValue, string format, CultureInfo culture)
		{
			format = format.Replace("{}", string.Empty);
			if (!string.IsNullOrWhiteSpace(format))
			{
				Match match = RegexStringFormatHexadecimal.Match(format);
				if (!match.Success)
				{
					if (!format.Contains("{"))
					{
						return newValue.Value.ToString(format, culture);
					}
					return string.Format(culture, format, newValue.Value);
				}
				if (match.Groups["simpleHEX"].Success)
				{
					return ((int)newValue.Value).ToString(match.Groups["simpleHEX"].Value, culture);
				}
				if (match.Groups["complexHEX"].Success)
				{
					return string.Format(culture, match.Groups["complexHEX"].Value, (int)newValue.Value);
				}
			}
			return newValue.Value.ToString(culture);
		}

		private ScrollViewer TryFindScrollViewer()
		{
			_valueTextBox.ApplyTemplate();
			ScrollViewer obj = _valueTextBox.Template.FindName("PART_ContentHost", _valueTextBox) as ScrollViewer;
			if (obj != null)
			{
				_handlesMouseWheelScrolling = new Lazy<PropertyInfo>(() => _scrollViewer.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault((PropertyInfo i) => i.Name == "HandlesMouseWheelScrolling"));
			}
			return obj;
		}

		private void ChangeValueWithSpeedUp(bool toPositive)
		{
			if (!IsReadOnly)
			{
				double num = (double)(toPositive ? 1 : (-1));
				if (Speedup)
				{
					double num2 = Interval * _internalLargeChange;
					if ((_intervalValueSinceReset += Interval * _internalIntervalMultiplierForCalculation) > num2)
					{
						_internalLargeChange *= 10.0;
						_internalIntervalMultiplierForCalculation *= 10.0;
					}
					ChangeValueInternal(num * _internalIntervalMultiplierForCalculation);
				}
				else
				{
					ChangeValueInternal(num * Interval);
				}
			}
		}

		private void ChangeValueInternal(bool addInterval)
		{
			ChangeValueInternal(addInterval ? Interval : (0.0 - Interval));
		}

		private void ChangeValueInternal(double interval)
		{
			if (!IsReadOnly)
			{
				NumericUpDownChangedRoutedEventArgs numericUpDownChangedRoutedEventArgs = (interval > 0.0) ? new NumericUpDownChangedRoutedEventArgs(ValueIncrementedEvent, interval) : new NumericUpDownChangedRoutedEventArgs(ValueDecrementedEvent, interval);
				RaiseEvent(numericUpDownChangedRoutedEventArgs);
				if (!numericUpDownChangedRoutedEventArgs.Handled)
				{
					ChangeValueBy(numericUpDownChangedRoutedEventArgs.Interval);
					_valueTextBox.CaretIndex = _valueTextBox.Text.Length;
				}
			}
		}

		private void ChangeValueBy(double difference)
		{
			double num = Value.GetValueOrDefault() + difference;
			SetCurrentValue(ValueProperty, CoerceValue(this, num));
		}

		private void EnableDisableDown()
		{
			if (_repeatDown != null)
			{
				_repeatDown.IsEnabled = (Value > Minimum);
			}
		}

		private void EnableDisableUp()
		{
			if (_repeatUp != null)
			{
				_repeatUp.IsEnabled = (Value < Maximum);
			}
		}

		private void EnableDisableUpDown()
		{
			EnableDisableUp();
			EnableDisableDown();
		}

		private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			_manualChange = (_manualChange || e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Decimal || e.Key == Key.OemComma || e.Key == Key.OemPeriod);
			if (NumericInputMode.HasFlag(NumericInput.Decimal) && (e.Key == Key.Decimal || e.Key == Key.OemPeriod))
			{
				TextBox textBox = sender as TextBox;
				if (!textBox.Text.Contains(SpecificCultureInfo.NumberFormat.NumberDecimalSeparator))
				{
					int caretIndex = textBox.CaretIndex;
					textBox.Text = textBox.Text.Insert(caretIndex, SpecificCultureInfo.NumberFormat.CurrencyDecimalSeparator);
					textBox.CaretIndex = caretIndex + 1;
				}
				e.Handled = true;
			}
		}

		private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
		{
			if (InterceptManualEnter)
			{
				if (_manualChange)
				{
					TextBox textBox = (TextBox)sender;
					_manualChange = false;
					if (ValidateText(textBox.Text, out double convertedValue))
					{
						if (SnapToMultipleOfInterval && Math.Abs(Interval) > 0.0)
						{
							convertedValue = Math.Round(convertedValue / Interval) * Interval;
						}
						if (convertedValue > Maximum)
						{
							convertedValue = Maximum;
						}
						else if (convertedValue < Minimum)
						{
							convertedValue = Minimum;
						}
						SetCurrentValue(ValueProperty, convertedValue);
					}
				}
				OnValueChanged(Value, Value);
			}
		}

		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			double convertedValue;
			if (string.IsNullOrEmpty(((TextBox)sender).Text))
			{
				Value = null;
			}
			else if (_manualChange && ValidateText(((TextBox)sender).Text, out convertedValue))
			{
				SetCurrentValue(ValueProperty, convertedValue);
			}
		}

		private void OnValueTextBoxPaste(object sender, DataObjectPastingEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			string text = textBox.Text;
			if (!e.SourceDataObject.GetDataPresent(DataFormats.Text, autoConvert: true))
			{
				e.CancelCommand();
			}
			else
			{
				string str = e.SourceDataObject.GetData(DataFormats.Text) as string;
				string text2 = text.Substring(0, textBox.SelectionStart) + str + text.Substring(textBox.SelectionStart + textBox.SelectionLength);
				if (!ValidateText(text2, out double _))
				{
					e.CancelCommand();
				}
				else
				{
					_manualChange = true;
				}
			}
		}

		private void RaiseChangeDelay()
		{
			RaiseEvent(new RoutedEventArgs(DelayChangedEvent));
		}

		private void ResetInternal()
		{
			if (!IsReadOnly)
			{
				_internalLargeChange = 100.0 * Interval;
				_internalIntervalMultiplierForCalculation = Interval;
				_intervalValueSinceReset = 0.0;
			}
		}

		private bool ValidateText(string text, out double convertedValue)
		{
			text = GetAnyNumberFromText(text);
			return double.TryParse(text, NumberStyles.Any, SpecificCultureInfo, out convertedValue);
		}

		private string GetAnyNumberFromText(string text)
		{
			MatchCollection matchCollection = RegexStringFormatNumber.Matches(text);
			if (matchCollection.Count > 0)
			{
				return matchCollection[0].Value;
			}
			return text;
		}
	}
}
