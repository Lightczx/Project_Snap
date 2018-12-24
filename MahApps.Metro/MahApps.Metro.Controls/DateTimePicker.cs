using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_Calendar", Type = typeof(System.Windows.Controls.Calendar))]
	[DefaultEvent("SelectedDateChanged")]
	public class DateTimePicker : TimePickerBase
	{
		public static readonly DependencyProperty DisplayDateEndProperty;

		public static readonly DependencyProperty DisplayDateProperty;

		public static readonly DependencyProperty DisplayDateStartProperty;

		public static readonly DependencyProperty FirstDayOfWeekProperty;

		public static readonly DependencyProperty IsTodayHighlightedProperty;

		public static readonly DependencyProperty SelectedDateFormatProperty;

		public static readonly DependencyProperty OrientationProperty;

		public static readonly RoutedEvent SelectedDateChangedEvent;

		public static readonly DependencyProperty SelectedDateProperty;

		private const string ElementCalendar = "PART_Calendar";

		private System.Windows.Controls.Calendar _calendar;

		private bool _deactivateWriteValueToTextBox;

		public DateTime? DisplayDate
		{
			get
			{
				return (DateTime?)GetValue(DisplayDateProperty);
			}
			set
			{
				SetValue(DisplayDateProperty, value);
			}
		}

		public DateTime? DisplayDateEnd
		{
			get
			{
				return (DateTime?)GetValue(DisplayDateEndProperty);
			}
			set
			{
				SetValue(DisplayDateEndProperty, value);
			}
		}

		public DateTime? DisplayDateStart
		{
			get
			{
				return (DateTime?)GetValue(DisplayDateStartProperty);
			}
			set
			{
				SetValue(DisplayDateStartProperty, value);
			}
		}

		public DayOfWeek FirstDayOfWeek
		{
			get
			{
				return (DayOfWeek)GetValue(FirstDayOfWeekProperty);
			}
			set
			{
				SetValue(FirstDayOfWeekProperty, value);
			}
		}

		[Category("Appearance")]
		[DefaultValue(DatePickerFormat.Short)]
		public DatePickerFormat SelectedDateFormat
		{
			get
			{
				return (DatePickerFormat)GetValue(SelectedDateFormatProperty);
			}
			set
			{
				SetValue(SelectedDateFormatProperty, value);
			}
		}

		public bool IsTodayHighlighted
		{
			get
			{
				return (bool)GetValue(IsTodayHighlightedProperty);
			}
			set
			{
				SetValue(IsTodayHighlightedProperty, value);
			}
		}

		[Category("Layout")]
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

		public DateTime? SelectedDate
		{
			get
			{
				return (DateTime?)GetValue(SelectedDateProperty);
			}
			set
			{
				SetValue(SelectedDateProperty, value);
			}
		}

		public event EventHandler<TimePickerBaseSelectionChangedEventArgs<DateTime?>> SelectedDateChanged
		{
			add
			{
				AddHandler(SelectedDateChangedEvent, value);
			}
			remove
			{
				RemoveHandler(SelectedDateChangedEvent, value);
			}
		}

		static DateTimePicker()
		{
			DisplayDateEndProperty = DatePicker.DisplayDateEndProperty.AddOwner(typeof(DateTimePicker));
			DisplayDateProperty = DatePicker.DisplayDateProperty.AddOwner(typeof(DateTimePicker));
			DisplayDateStartProperty = DatePicker.DisplayDateStartProperty.AddOwner(typeof(DateTimePicker));
			FirstDayOfWeekProperty = DatePicker.FirstDayOfWeekProperty.AddOwner(typeof(DateTimePicker));
			IsTodayHighlightedProperty = DatePicker.IsTodayHighlightedProperty.AddOwner(typeof(DateTimePicker));
			SelectedDateFormatProperty = DatePicker.SelectedDateFormatProperty.AddOwner(typeof(DateTimePicker), new FrameworkPropertyMetadata(DatePickerFormat.Short, OnSelectedDateFormatChanged));
			OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(DateTimePicker), new PropertyMetadata(Orientation.Horizontal, null, CoerceOrientation));
			SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged", RoutingStrategy.Direct, typeof(EventHandler<TimePickerBaseSelectionChangedEventArgs<DateTime?>>), typeof(DateTimePicker));
			SelectedDateProperty = DatePicker.SelectedDateProperty.AddOwner(typeof(DateTimePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateChanged));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(typeof(DateTimePicker)));
			TimePickerBase.IsClockVisibleProperty.OverrideMetadata(typeof(DateTimePicker), new PropertyMetadata(OnClockVisibilityChanged));
		}

		public override void OnApplyTemplate()
		{
			_calendar = (GetTemplateChild("PART_Calendar") as System.Windows.Controls.Calendar);
			base.OnApplyTemplate();
			SetDatePartValues();
		}

		protected virtual void OnSelectedDateChanged(TimePickerBaseSelectionChangedEventArgs<DateTime?> e)
		{
			RaiseEvent(e);
		}

		private static void OnSelectedDateFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as DateTimePicker)?.WriteValueToTextBox();
		}

		protected override void ApplyBindings()
		{
			base.ApplyBindings();
			if (_calendar != null)
			{
				_calendar.SetBinding(System.Windows.Controls.Calendar.DisplayDateProperty, GetBinding(DisplayDateProperty));
				_calendar.SetBinding(System.Windows.Controls.Calendar.DisplayDateStartProperty, GetBinding(DisplayDateStartProperty));
				_calendar.SetBinding(System.Windows.Controls.Calendar.DisplayDateEndProperty, GetBinding(DisplayDateEndProperty));
				_calendar.SetBinding(System.Windows.Controls.Calendar.FirstDayOfWeekProperty, GetBinding(FirstDayOfWeekProperty));
				_calendar.SetBinding(System.Windows.Controls.Calendar.IsTodayHighlightedProperty, GetBinding(IsTodayHighlightedProperty));
				_calendar.SetBinding(FrameworkElement.FlowDirectionProperty, GetBinding(FrameworkElement.FlowDirectionProperty));
				_calendar.SelectedDatesChanged += OnCalendarSelectedDateChanged;
			}
		}

		protected sealed override void ApplyCulture()
		{
			base.ApplyCulture();
			SetCurrentValue(FirstDayOfWeekProperty, base.SpecificCultureInfo.DateTimeFormat.FirstDayOfWeek);
		}

		protected override string GetValueForTextBox()
		{
			DateTimeFormatInfo dateTimeFormat = base.SpecificCultureInfo.DateTimeFormat;
			string arg = (base.SelectedTimeFormat == TimePickerFormat.Long) ? dateTimeFormat.LongTimePattern : dateTimeFormat.ShortTimePattern;
			string arg2 = (SelectedDateFormat == DatePickerFormat.Long) ? dateTimeFormat.LongDatePattern : dateTimeFormat.ShortDatePattern;
			string format = string.Intern($"{arg2} {arg}");
			return GetSelectedDateTimeFromGUI()?.ToString(format, base.SpecificCultureInfo);
		}

		protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseUp(e);
			if (Mouse.Captured is CalendarItem)
			{
				Mouse.Capture(null);
			}
		}

		protected override void OnRangeBaseValueChanged(object sender, SelectionChangedEventArgs e)
		{
			base.OnRangeBaseValueChanged(sender, e);
			SetDatePartValues();
		}

		protected override void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
		{
			if (DateTime.TryParse(((DatePickerTextBox)sender).Text, base.SpecificCultureInfo, DateTimeStyles.None, out DateTime result))
			{
				SelectedDate = result;
			}
			else
			{
				if (!SelectedDate.HasValue)
				{
					WriteValueToTextBox();
				}
				SelectedDate = null;
			}
		}

		protected override void WriteValueToTextBox()
		{
			if (!_deactivateWriteValueToTextBox)
			{
				base.WriteValueToTextBox();
			}
		}

		private void OnCalendarSelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				DateTime d = (DateTime)e.AddedItems[0];
				TimeSpan timeOfDay = SelectedDate.GetValueOrDefault().TimeOfDay;
				d += timeOfDay;
				SelectedDate = d;
			}
		}

		private static object CoerceOrientation(DependencyObject d, object basevalue)
		{
			if (((DateTimePicker)d).IsClockVisible)
			{
				return basevalue;
			}
			return Orientation.Vertical;
		}

		private static void OnClockVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(OrientationProperty);
		}

		private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DateTimePicker dateTimePicker = (DateTimePicker)d;
			dateTimePicker._deactivateWriteValueToTextBox = true;
			DateTime? dateTime = (DateTime?)e.NewValue;
			if (dateTime.HasValue)
			{
				dateTimePicker.SelectedTime = dateTime.Value.TimeOfDay;
				dateTimePicker.OnSelectedDateChanged(new TimePickerBaseSelectionChangedEventArgs<DateTime?>(SelectedDateChangedEvent, (DateTime?)e.OldValue, (DateTime?)e.NewValue));
			}
			else
			{
				dateTimePicker.SetDefaultTimeOfDayValues();
			}
			dateTimePicker._deactivateWriteValueToTextBox = false;
			dateTimePicker.WriteValueToTextBox();
		}

		private DateTime? GetSelectedDateTimeFromGUI()
		{
			DateTime? selectedDate = SelectedDate;
			if (selectedDate.HasValue)
			{
				return selectedDate.Value.Date + GetSelectedTimeFromGUI().GetValueOrDefault();
			}
			return null;
		}

		private void SetDatePartValues()
		{
			DateTime? selectedDateTimeFromGUI = GetSelectedDateTimeFromGUI();
			if (selectedDateTimeFromGUI.HasValue)
			{
				DisplayDate = ((selectedDateTimeFromGUI != DateTime.MinValue) ? selectedDateTimeFromGUI : new DateTime?(DateTime.Today));
				if ((SelectedDate != DisplayDate && SelectedDate != DateTime.MinValue) || (base.Popup != null && base.Popup.IsOpen))
				{
					SelectedDate = DisplayDate;
				}
			}
		}
	}
}
