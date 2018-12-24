using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_Button", Type = typeof(Button))]
	[TemplatePart(Name = "PART_HourHand", Type = typeof(UIElement))]
	[TemplatePart(Name = "PART_HourPicker", Type = typeof(Selector))]
	[TemplatePart(Name = "PART_MinuteHand", Type = typeof(UIElement))]
	[TemplatePart(Name = "PART_SecondHand", Type = typeof(UIElement))]
	[TemplatePart(Name = "PART_SecondPicker", Type = typeof(Selector))]
	[TemplatePart(Name = "PART_MinutePicker", Type = typeof(Selector))]
	[TemplatePart(Name = "PART_AmPmSwitcher", Type = typeof(Selector))]
	[TemplatePart(Name = "PART_TextBox", Type = typeof(DatePickerTextBox))]
	public abstract class TimePickerBase : Control
	{
		public static readonly DependencyProperty SourceHoursProperty;

		public static readonly DependencyProperty SourceMinutesProperty;

		public static readonly DependencyProperty SourceSecondsProperty;

		public static readonly DependencyProperty IsDropDownOpenProperty;

		public static readonly DependencyProperty IsClockVisibleProperty;

		public static readonly DependencyProperty IsReadOnlyProperty;

		public static readonly DependencyProperty HandVisibilityProperty;

		public static readonly DependencyProperty CultureProperty;

		public static readonly DependencyProperty PickerVisibilityProperty;

		public static readonly RoutedEvent SelectedTimeChangedEvent;

		public static readonly DependencyProperty SelectedTimeProperty;

		public static readonly DependencyProperty SelectedTimeFormatProperty;

		private const string ElementAmPmSwitcher = "PART_AmPmSwitcher";

		private const string ElementButton = "PART_Button";

		private const string ElementHourHand = "PART_HourHand";

		private const string ElementHourPicker = "PART_HourPicker";

		private const string ElementMinuteHand = "PART_MinuteHand";

		private const string ElementMinutePicker = "PART_MinutePicker";

		private const string ElementPopup = "PART_Popup";

		private const string ElementSecondHand = "PART_SecondHand";

		private const string ElementSecondPicker = "PART_SecondPicker";

		private const string ElementTextBox = "PART_TextBox";

		private static readonly DependencyPropertyKey IsDatePickerVisiblePropertyKey;

		public static readonly DependencyProperty IsDatePickerVisibleProperty;

		private static readonly TimeSpan MinTimeOfDay;

		private static readonly TimeSpan MaxTimeOfDay;

		public static readonly IEnumerable<int> IntervalOf5;

		public static readonly IEnumerable<int> IntervalOf10;

		public static readonly IEnumerable<int> IntervalOf15;

		private Selector _ampmSwitcher;

		private Button _button;

		private bool _deactivateRangeBaseEvent;

		private bool _deactivateTextChangedEvent;

		private bool _textInputChanged;

		private UIElement _hourHand;

		private Selector _hourInput;

		private UIElement _minuteHand;

		private Selector _minuteInput;

		private Popup _popup;

		private UIElement _secondHand;

		private Selector _secondInput;

		protected DatePickerTextBox _textBox;

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

		[Category("Appearance")]
		[DefaultValue(TimePartVisibility.All)]
		public TimePartVisibility HandVisibility
		{
			get
			{
				return (TimePartVisibility)GetValue(HandVisibilityProperty);
			}
			set
			{
				SetValue(HandVisibilityProperty, value);
			}
		}

		public bool IsDatePickerVisible
		{
			get
			{
				return (bool)GetValue(IsDatePickerVisibleProperty);
			}
			protected set
			{
				SetValue(IsDatePickerVisiblePropertyKey, value);
			}
		}

		[Category("Appearance")]
		public bool IsClockVisible
		{
			get
			{
				return (bool)GetValue(IsClockVisibleProperty);
			}
			set
			{
				SetValue(IsClockVisibleProperty, value);
			}
		}

		public bool IsDropDownOpen
		{
			get
			{
				return (bool)GetValue(IsDropDownOpenProperty);
			}
			set
			{
				SetValue(IsDropDownOpenProperty, value);
			}
		}

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

		[Category("Appearance")]
		[DefaultValue(TimePartVisibility.All)]
		public TimePartVisibility PickerVisibility
		{
			get
			{
				return (TimePartVisibility)GetValue(PickerVisibilityProperty);
			}
			set
			{
				SetValue(PickerVisibilityProperty, value);
			}
		}

		public TimeSpan? SelectedTime
		{
			get
			{
				return (TimeSpan?)GetValue(SelectedTimeProperty);
			}
			set
			{
				SetValue(SelectedTimeProperty, value);
			}
		}

		[Category("Appearance")]
		[DefaultValue(TimePickerFormat.Long)]
		public TimePickerFormat SelectedTimeFormat
		{
			get
			{
				return (TimePickerFormat)GetValue(SelectedTimeFormatProperty);
			}
			set
			{
				SetValue(SelectedTimeFormatProperty, value);
			}
		}

		[Category("Common")]
		public IEnumerable<int> SourceHours
		{
			get
			{
				return (IEnumerable<int>)GetValue(SourceHoursProperty);
			}
			set
			{
				SetValue(SourceHoursProperty, value);
			}
		}

		[Category("Common")]
		public IEnumerable<int> SourceMinutes
		{
			get
			{
				return (IEnumerable<int>)GetValue(SourceMinutesProperty);
			}
			set
			{
				SetValue(SourceMinutesProperty, value);
			}
		}

		[Category("Common")]
		public IEnumerable<int> SourceSeconds
		{
			get
			{
				return (IEnumerable<int>)GetValue(SourceSecondsProperty);
			}
			set
			{
				SetValue(SourceSecondsProperty, value);
			}
		}

		public bool IsMilitaryTime
		{
			get
			{
				DateTimeFormatInfo dateTimeFormat = SpecificCultureInfo.DateTimeFormat;
				if (!string.IsNullOrEmpty(dateTimeFormat.AMDesignator))
				{
					if (!dateTimeFormat.ShortTimePattern.Contains("h"))
					{
						return dateTimeFormat.LongTimePattern.Contains("h");
					}
					return true;
				}
				return false;
			}
		}

		protected internal Popup Popup => _popup;

		protected CultureInfo SpecificCultureInfo => Culture ?? base.Language.GetSpecificCulture();

		public event EventHandler<TimePickerBaseSelectionChangedEventArgs<TimeSpan?>> SelectedTimeChanged
		{
			add
			{
				AddHandler(SelectedTimeChangedEvent, value);
			}
			remove
			{
				RemoveHandler(SelectedTimeChangedEvent, value);
			}
		}

		static TimePickerBase()
		{
			SourceHoursProperty = DependencyProperty.Register("SourceHours", typeof(IEnumerable<int>), typeof(TimePickerBase), new FrameworkPropertyMetadata(Enumerable.Range(0, 24), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceSourceHours));
			SourceMinutesProperty = DependencyProperty.Register("SourceMinutes", typeof(IEnumerable<int>), typeof(TimePickerBase), new FrameworkPropertyMetadata(Enumerable.Range(0, 60), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceSource60));
			SourceSecondsProperty = DependencyProperty.Register("SourceSeconds", typeof(IEnumerable<int>), typeof(TimePickerBase), new FrameworkPropertyMetadata(Enumerable.Range(0, 60), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceSource60));
			IsDropDownOpenProperty = DatePicker.IsDropDownOpenProperty.AddOwner(typeof(TimePickerBase), new PropertyMetadata(false));
			IsClockVisibleProperty = DependencyProperty.Register("IsClockVisible", typeof(bool), typeof(TimePickerBase), new PropertyMetadata(true));
			IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(TimePickerBase), new PropertyMetadata(false));
			HandVisibilityProperty = DependencyProperty.Register("HandVisibility", typeof(TimePartVisibility), typeof(TimePickerBase), new PropertyMetadata(TimePartVisibility.All, OnHandVisibilityChanged));
			CultureProperty = DependencyProperty.Register("Culture", typeof(CultureInfo), typeof(TimePickerBase), new PropertyMetadata(null, OnCultureChanged));
			PickerVisibilityProperty = DependencyProperty.Register("PickerVisibility", typeof(TimePartVisibility), typeof(TimePickerBase), new PropertyMetadata(TimePartVisibility.All, OnPickerVisibilityChanged));
			SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedTimeChanged", RoutingStrategy.Direct, typeof(EventHandler<TimePickerBaseSelectionChangedEventArgs<TimeSpan?>>), typeof(TimePickerBase));
			SelectedTimeProperty = DependencyProperty.Register("SelectedTime", typeof(TimeSpan?), typeof(TimePickerBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTimeChanged, CoerceSelectedTime));
			SelectedTimeFormatProperty = DependencyProperty.Register("SelectedTimeFormat", typeof(TimePickerFormat), typeof(TimePickerBase), new PropertyMetadata(TimePickerFormat.Long, OnSelectedTimeFormatChanged));
			IsDatePickerVisiblePropertyKey = DependencyProperty.RegisterReadOnly("IsDatePickerVisible", typeof(bool), typeof(TimePickerBase), new PropertyMetadata(true));
			IsDatePickerVisibleProperty = IsDatePickerVisiblePropertyKey.DependencyProperty;
			MinTimeOfDay = TimeSpan.Zero;
			MaxTimeOfDay = TimeSpan.FromDays(1.0) - TimeSpan.FromTicks(1L);
			IntervalOf5 = CreateValueList(5);
			IntervalOf10 = CreateValueList(10);
			IntervalOf15 = CreateValueList(15);
			EventManager.RegisterClassHandler(typeof(TimePickerBase), UIElement.GotFocusEvent, new RoutedEventHandler(OnGotFocus));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePickerBase), new FrameworkPropertyMetadata(typeof(TimePickerBase)));
			Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(TimePickerBase), new FrameworkPropertyMetadata(VerticalAlignment.Center));
			FrameworkElement.LanguageProperty.OverrideMetadata(typeof(TimePickerBase), new FrameworkPropertyMetadata(OnCultureChanged));
		}

		protected TimePickerBase()
		{
			Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OutsideCapturedElementHandler);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			UnSubscribeEvents();
			_popup = (GetTemplateChild("PART_Popup") as Popup);
			_button = (GetTemplateChild("PART_Button") as Button);
			_hourInput = (GetTemplateChild("PART_HourPicker") as Selector);
			_minuteInput = (GetTemplateChild("PART_MinutePicker") as Selector);
			_secondInput = (GetTemplateChild("PART_SecondPicker") as Selector);
			_hourHand = (GetTemplateChild("PART_HourHand") as FrameworkElement);
			_ampmSwitcher = (GetTemplateChild("PART_AmPmSwitcher") as Selector);
			_minuteHand = (GetTemplateChild("PART_MinuteHand") as FrameworkElement);
			_secondHand = (GetTemplateChild("PART_SecondHand") as FrameworkElement);
			_textBox = (GetTemplateChild("PART_TextBox") as DatePickerTextBox);
			SetHandVisibility(HandVisibility);
			SetPickerVisibility(PickerVisibility);
			SetHourPartValues(SelectedTime.GetValueOrDefault());
			WriteValueToTextBox();
			SetDefaultTimeOfDayValues();
			SubscribeEvents();
			ApplyCulture();
			ApplyBindings();
		}

		protected virtual void ApplyBindings()
		{
			if (Popup != null)
			{
				Popup.SetBinding(Popup.IsOpenProperty, GetBinding(IsDropDownOpenProperty));
			}
		}

		protected virtual void ApplyCulture()
		{
			_deactivateRangeBaseEvent = true;
			if (_ampmSwitcher != null)
			{
				_ampmSwitcher.Items.Clear();
				if (!string.IsNullOrEmpty(SpecificCultureInfo.DateTimeFormat.AMDesignator))
				{
					_ampmSwitcher.Items.Add(SpecificCultureInfo.DateTimeFormat.AMDesignator);
				}
				if (!string.IsNullOrEmpty(SpecificCultureInfo.DateTimeFormat.PMDesignator))
				{
					_ampmSwitcher.Items.Add(SpecificCultureInfo.DateTimeFormat.PMDesignator);
				}
			}
			SetAmPmVisibility();
			CoerceValue(SourceHoursProperty);
			if (SelectedTime.HasValue)
			{
				SetHourPartValues(SelectedTime.Value);
			}
			SetDefaultTimeOfDayValues();
			_deactivateRangeBaseEvent = false;
			WriteValueToTextBox();
		}

		protected Binding GetBinding(DependencyProperty property)
		{
			return new Binding(property.Name)
			{
				Source = this
			};
		}

		protected virtual string GetValueForTextBox()
		{
			string str = (SelectedTimeFormat == TimePickerFormat.Long) ? string.Intern(SpecificCultureInfo.DateTimeFormat.LongTimePattern) : string.Intern(SpecificCultureInfo.DateTimeFormat.ShortTimePattern);
			DateTime minValue = DateTime.MinValue;
			TimeSpan? selectedTime = SelectedTime;
			return (minValue + selectedTime)?.ToString(string.Intern(str), SpecificCultureInfo);
		}

		protected virtual void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
		{
			if (DateTime.TryParse(string.Intern($"{DateTime.MinValue.ToString(SpecificCultureInfo.DateTimeFormat.ShortDatePattern)} {((DatePickerTextBox)sender).Text}"), SpecificCultureInfo, DateTimeStyles.None, out DateTime result))
			{
				SelectedTime = result.TimeOfDay;
			}
			else
			{
				if (!SelectedTime.HasValue)
				{
					WriteValueToTextBox();
				}
				SelectedTime = null;
			}
		}

		protected virtual void OnRangeBaseValueChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedTime = GetSelectedTimeFromGUI();
		}

		protected virtual void OnSelectedTimeChanged(TimePickerBaseSelectionChangedEventArgs<TimeSpan?> e)
		{
			RaiseEvent(e);
		}

		private static void OnSelectedTimeFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as TimePickerBase)?.WriteValueToTextBox();
		}

		protected void SetDefaultTimeOfDayValues()
		{
			SetDefaultTimeOfDayValue(_hourInput);
			SetDefaultTimeOfDayValue(_minuteInput);
			SetDefaultTimeOfDayValue(_secondInput);
			SetDefaultTimeOfDayValue(_ampmSwitcher);
		}

		protected virtual void SubscribeEvents()
		{
			SubscribeRangeBaseValueChanged(_hourInput, _minuteInput, _secondInput, _ampmSwitcher);
			if (_button != null)
			{
				_button.Click += OnButtonClicked;
			}
			if (_textBox != null)
			{
				_textBox.TextChanged += OnTextChanged;
				_textBox.LostFocus += InternalOnTextBoxLostFocus;
			}
		}

		protected virtual void UnSubscribeEvents()
		{
			UnsubscribeRangeBaseValueChanged(_hourInput, _minuteInput, _secondInput, _ampmSwitcher);
			if (_button != null)
			{
				_button.Click -= OnButtonClicked;
			}
			if (_textBox != null)
			{
				_textBox.TextChanged -= OnTextChanged;
				_textBox.LostFocus -= InternalOnTextBoxLostFocus;
			}
		}

		protected virtual void WriteValueToTextBox()
		{
			if (_textBox != null)
			{
				_deactivateTextChangedEvent = true;
				_textBox.Text = GetValueForTextBox();
				_deactivateTextChangedEvent = false;
			}
		}

		private static IList<int> CreateValueList(int interval)
		{
			return Enumerable.Repeat(interval, 60 / interval).Select((int value, int index) => value * index).ToList();
		}

		private static object CoerceSelectedTime(DependencyObject d, object basevalue)
		{
			TimeSpan? timeSpan = (TimeSpan?)basevalue;
			if (timeSpan < MinTimeOfDay)
			{
				return MinTimeOfDay;
			}
			if (timeSpan > MaxTimeOfDay)
			{
				return MaxTimeOfDay;
			}
			return timeSpan;
		}

		private static object CoerceSource60(DependencyObject d, object basevalue)
		{
			IEnumerable<int> enumerable = basevalue as IEnumerable<int>;
			if (enumerable != null)
			{
				return enumerable.Where(delegate(int i)
				{
					if (i >= 0)
					{
						return i < 60;
					}
					return false;
				});
			}
			return Enumerable.Empty<int>();
		}

		private static object CoerceSourceHours(DependencyObject d, object basevalue)
		{
			TimePickerBase timePickerBase = d as TimePickerBase;
			IEnumerable<int> enumerable = basevalue as IEnumerable<int>;
			if (timePickerBase != null && enumerable != null)
			{
				if (timePickerBase.IsMilitaryTime)
				{
					return enumerable.Where(delegate(int i)
					{
						if (i > 0)
						{
							return i <= 12;
						}
						return false;
					}).OrderBy((int i) => i, new AmPmComparer());
				}
				return enumerable.Where(delegate(int i)
				{
					if (i >= 0)
					{
						return i < 24;
					}
					return false;
				});
			}
			return Enumerable.Empty<int>();
		}

		private void InternalOnTextBoxLostFocus(object sender, RoutedEventArgs e)
		{
			if (_textInputChanged)
			{
				_textInputChanged = false;
				OnTextBoxLostFocus(sender, e);
			}
		}

		private void InternalOnRangeBaseValueChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!_deactivateRangeBaseEvent)
			{
				OnRangeBaseValueChanged(sender, e);
			}
		}

		private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TimePickerBase timePickerBase = (TimePickerBase)d;
			if (e.NewValue is XmlLanguage)
			{
				timePickerBase.Language = (XmlLanguage)e.NewValue;
			}
			else if (e.NewValue is CultureInfo)
			{
				timePickerBase.Language = XmlLanguage.GetLanguage(((CultureInfo)e.NewValue).IetfLanguageTag);
			}
			else
			{
				timePickerBase.Language = XmlLanguage.Empty;
			}
			timePickerBase.ApplyCulture();
		}

		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			if (!(bool)e.NewValue)
			{
				IsDropDownOpen = false;
			}
		}

		private void OutsideCapturedElementHandler(object sender, MouseButtonEventArgs e)
		{
			IsDropDownOpen = false;
		}

		private static void OnGotFocus(object sender, RoutedEventArgs e)
		{
			TimePickerBase timePickerBase = (TimePickerBase)sender;
			if (!e.Handled && timePickerBase.Focusable)
			{
				if (object.Equals(e.OriginalSource, timePickerBase))
				{
					TraversalRequest request = new TraversalRequest(((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next);
					UIElement uIElement = Keyboard.FocusedElement as UIElement;
					if (uIElement != null)
					{
						uIElement.MoveFocus(request);
					}
					else
					{
						timePickerBase.Focus();
					}
					e.Handled = true;
				}
				else if (timePickerBase._textBox != null && object.Equals(e.OriginalSource, timePickerBase._textBox))
				{
					timePickerBase._textBox.SelectAll();
					e.Handled = true;
				}
			}
		}

		private static void OnHandVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TimePickerBase)d).SetHandVisibility((TimePartVisibility)e.NewValue);
		}

		private static void OnPickerVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TimePickerBase)d).SetPickerVisibility((TimePartVisibility)e.NewValue);
		}

		private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TimePickerBase timePickerBase = (TimePickerBase)d;
			if (!timePickerBase._deactivateRangeBaseEvent)
			{
				timePickerBase.SetHourPartValues((e.NewValue as TimeSpan?).GetValueOrDefault(TimeSpan.Zero));
				timePickerBase.OnSelectedTimeChanged(new TimePickerBaseSelectionChangedEventArgs<TimeSpan?>(SelectedTimeChangedEvent, (TimeSpan?)e.OldValue, (TimeSpan?)e.NewValue));
				timePickerBase.WriteValueToTextBox();
			}
		}

		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			if (!_deactivateTextChangedEvent)
			{
				_textInputChanged = true;
			}
		}

		private static void SetVisibility(UIElement partHours, UIElement partMinutes, UIElement partSeconds, TimePartVisibility visibility)
		{
			if (partHours != null)
			{
				partHours.Visibility = ((!visibility.HasFlag(TimePartVisibility.Hour)) ? Visibility.Collapsed : Visibility.Visible);
			}
			if (partMinutes != null)
			{
				partMinutes.Visibility = ((!visibility.HasFlag(TimePartVisibility.Minute)) ? Visibility.Collapsed : Visibility.Visible);
			}
			if (partSeconds != null)
			{
				partSeconds.Visibility = ((!visibility.HasFlag(TimePartVisibility.Second)) ? Visibility.Collapsed : Visibility.Visible);
			}
		}

		private static bool IsValueSelected(Selector selector)
		{
			if (selector != null)
			{
				return selector.SelectedItem != null;
			}
			return false;
		}

		private static void SetDefaultTimeOfDayValue(Selector selector)
		{
			if (selector != null && selector.SelectedValue == null)
			{
				selector.SelectedIndex = 0;
			}
		}

		protected TimeSpan? GetSelectedTimeFromGUI()
		{
			if (IsValueSelected(_hourInput) && IsValueSelected(_minuteInput) && IsValueSelected(_secondInput))
			{
				int num = (int)_hourInput.SelectedItem;
				int minutes = (int)_minuteInput.SelectedItem;
				int seconds = (int)_secondInput.SelectedItem;
				num += GetAmPmOffset(num);
				return new TimeSpan(num, minutes, seconds);
			}
			return SelectedTime;
		}

		private int GetAmPmOffset(int currentHour)
		{
			if (IsMilitaryTime)
			{
				if (currentHour == 12)
				{
					if (object.Equals(_ampmSwitcher.SelectedItem, SpecificCultureInfo.DateTimeFormat.AMDesignator))
					{
						return -12;
					}
				}
				else if (object.Equals(_ampmSwitcher.SelectedItem, SpecificCultureInfo.DateTimeFormat.PMDesignator))
				{
					return 12;
				}
			}
			return 0;
		}

		private void OnButtonClicked(object sender, RoutedEventArgs e)
		{
			IsDropDownOpen = !IsDropDownOpen;
			if (Popup != null)
			{
				Popup.IsOpen = IsDropDownOpen;
			}
		}

		private void SetAmPmVisibility()
		{
			if (_ampmSwitcher != null)
			{
				if (!PickerVisibility.HasFlag(TimePartVisibility.Hour))
				{
					_ampmSwitcher.Visibility = Visibility.Collapsed;
				}
				else
				{
					_ampmSwitcher.Visibility = ((!IsMilitaryTime) ? Visibility.Collapsed : Visibility.Visible);
				}
			}
		}

		private void SetHandVisibility(TimePartVisibility visibility)
		{
			SetVisibility(_hourHand, _minuteHand, _secondHand, visibility);
		}

		private void SetHourPartValues(TimeSpan timeOfDay)
		{
			if (!_deactivateRangeBaseEvent)
			{
				_deactivateRangeBaseEvent = true;
				if (_hourInput != null)
				{
					if (IsMilitaryTime)
					{
						_ampmSwitcher.SelectedValue = ((timeOfDay.Hours < 12) ? SpecificCultureInfo.DateTimeFormat.AMDesignator : SpecificCultureInfo.DateTimeFormat.PMDesignator);
						if (timeOfDay.Hours == 0 || timeOfDay.Hours == 12)
						{
							_hourInput.SelectedValue = 12;
						}
						else
						{
							_hourInput.SelectedValue = timeOfDay.Hours % 12;
						}
					}
					else
					{
						_hourInput.SelectedValue = timeOfDay.Hours;
					}
				}
				if (_minuteInput != null)
				{
					_minuteInput.SelectedValue = timeOfDay.Minutes;
				}
				if (_secondInput != null)
				{
					_secondInput.SelectedValue = timeOfDay.Seconds;
				}
				_deactivateRangeBaseEvent = false;
			}
		}

		private void SetPickerVisibility(TimePartVisibility visibility)
		{
			SetVisibility(_hourInput, _minuteInput, _secondInput, visibility);
			SetAmPmVisibility();
		}

		private void SubscribeRangeBaseValueChanged(params Selector[] selectors)
		{
			foreach (Selector item in from i in selectors
			where i != null
			select i)
			{
				item.SelectionChanged += InternalOnRangeBaseValueChanged;
			}
		}

		private void UnsubscribeRangeBaseValueChanged(params Selector[] selectors)
		{
			foreach (Selector item in from i in selectors
			where i != null
			select i)
			{
				item.SelectionChanged -= InternalOnRangeBaseValueChanged;
			}
		}
	}
}
