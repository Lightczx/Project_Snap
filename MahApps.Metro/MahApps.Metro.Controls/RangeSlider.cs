using ControlzEx;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MahApps.Metro.Controls
{
	[DefaultEvent("RangeSelectionChanged")]
	[TemplatePart(Name = "PART_Container", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_RangeSliderContainer", Type = typeof(StackPanel))]
	[TemplatePart(Name = "PART_LeftEdge", Type = typeof(RepeatButton))]
	[TemplatePart(Name = "PART_LeftThumb", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_MiddleThumb", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_RightThumb", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_RightEdge", Type = typeof(RepeatButton))]
	public class RangeSlider : RangeBase
	{
		private enum ButtonType
		{
			BottomLeft,
			TopRight,
			Both
		}

		private enum Direction
		{
			Increase,
			Decrease
		}

		public static RoutedUICommand MoveBack;

		public static RoutedUICommand MoveForward;

		public static RoutedUICommand MoveAllForward;

		public static RoutedUICommand MoveAllBack;

		public static readonly RoutedEvent RangeSelectionChangedEvent;

		public static readonly RoutedEvent LowerValueChangedEvent;

		public static readonly RoutedEvent UpperValueChangedEvent;

		public static readonly RoutedEvent LowerThumbDragStartedEvent;

		public static readonly RoutedEvent LowerThumbDragCompletedEvent;

		public static readonly RoutedEvent UpperThumbDragStartedEvent;

		public static readonly RoutedEvent UpperThumbDragCompletedEvent;

		public static readonly RoutedEvent CentralThumbDragStartedEvent;

		public static readonly RoutedEvent CentralThumbDragCompletedEvent;

		public static readonly RoutedEvent LowerThumbDragDeltaEvent;

		public static readonly RoutedEvent UpperThumbDragDeltaEvent;

		public static readonly RoutedEvent CentralThumbDragDeltaEvent;

		public static readonly DependencyProperty UpperValueProperty;

		public static readonly DependencyProperty LowerValueProperty;

		public static readonly DependencyProperty MinRangeProperty;

		public static readonly DependencyProperty MinRangeWidthProperty;

		public static readonly DependencyProperty MoveWholeRangeProperty;

		public static readonly DependencyProperty ExtendedModeProperty;

		public static readonly DependencyProperty IsSnapToTickEnabledProperty;

		public static readonly DependencyProperty OrientationProperty;

		public static readonly DependencyProperty TickPlacementProperty;

		public static readonly DependencyProperty TickFrequencyProperty;

		public static readonly DependencyProperty TicksProperty;

		public static readonly DependencyProperty IsMoveToPointEnabledProperty;

		public static readonly DependencyProperty AutoToolTipPlacementProperty;

		public static readonly DependencyProperty AutoToolTipPrecisionProperty;

		public static readonly DependencyProperty AutoToolTipTextConverterProperty;

		public static readonly DependencyProperty IntervalProperty;

		public static readonly DependencyProperty IsSelectionRangeEnabledProperty;

		public static readonly DependencyProperty SelectionStartProperty;

		public static readonly DependencyProperty SelectionEndProperty;

		private const double Epsilon = 1.53E-06;

		private bool _internalUpdate;

		private Thumb _centerThumb;

		private Thumb _leftThumb;

		private Thumb _rightThumb;

		private RepeatButton _leftButton;

		private RepeatButton _rightButton;

		private StackPanel _visualElementsContainer;

		private FrameworkElement _container;

		private double _movableWidth;

		private readonly DispatcherTimer _timer;

		private uint _tickCount;

		private double _currentpoint;

		private bool _isInsideRange;

		private bool _centerThumbBlocked;

		private Direction _direction;

		private ButtonType _bType;

		private Point _position;

		private Point _basePoint;

		private double _currenValue;

		private double _density;

		private ToolTip _autoToolTip;

		private double _oldLower;

		private double _oldUpper;

		private bool _isMoved;

		private bool _roundToPrecision;

		private int _precision;

		private readonly PropertyChangeNotifier actualWidthPropertyChangeNotifier;

		private readonly PropertyChangeNotifier actualHeightPropertyChangeNotifier;

		[Bindable(true)]
		[Category("Behavior")]
		public int Interval
		{
			get
			{
				return (int)GetValue(IntervalProperty);
			}
			set
			{
				SetValue(IntervalProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		public int AutoToolTipPrecision
		{
			get
			{
				return (int)GetValue(AutoToolTipPrecisionProperty);
			}
			set
			{
				SetValue(AutoToolTipPrecisionProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		public IValueConverter AutoToolTipTextConverter
		{
			get
			{
				return (IValueConverter)GetValue(AutoToolTipTextConverterProperty);
			}
			set
			{
				SetValue(AutoToolTipTextConverterProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		public AutoToolTipPlacement AutoToolTipPlacement
		{
			get
			{
				return (AutoToolTipPlacement)GetValue(AutoToolTipPlacementProperty);
			}
			set
			{
				SetValue(AutoToolTipPlacementProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		public TickPlacement TickPlacement
		{
			get
			{
				return (TickPlacement)GetValue(TickPlacementProperty);
			}
			set
			{
				SetValue(TickPlacementProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		public double TickFrequency
		{
			get
			{
				return (double)GetValue(TickFrequencyProperty);
			}
			set
			{
				SetValue(TickFrequencyProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		public DoubleCollection Ticks
		{
			get
			{
				return (DoubleCollection)GetValue(TicksProperty);
			}
			set
			{
				SetValue(TicksProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		public bool IsMoveToPointEnabled
		{
			get
			{
				return (bool)GetValue(IsMoveToPointEnabledProperty);
			}
			set
			{
				SetValue(IsMoveToPointEnabledProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
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

		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSnapToTickEnabled
		{
			get
			{
				return (bool)GetValue(IsSnapToTickEnabledProperty);
			}
			set
			{
				SetValue(IsSnapToTickEnabledProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		public bool ExtendedMode
		{
			get
			{
				return (bool)GetValue(ExtendedModeProperty);
			}
			set
			{
				SetValue(ExtendedModeProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Behavior")]
		public bool MoveWholeRange
		{
			get
			{
				return (bool)GetValue(MoveWholeRangeProperty);
			}
			set
			{
				SetValue(MoveWholeRangeProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		public double MinRangeWidth
		{
			get
			{
				return (double)GetValue(MinRangeWidthProperty);
			}
			set
			{
				SetValue(MinRangeWidthProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		public double LowerValue
		{
			get
			{
				return (double)GetValue(LowerValueProperty);
			}
			set
			{
				SetValue(LowerValueProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		public double UpperValue
		{
			get
			{
				return (double)GetValue(UpperValueProperty);
			}
			set
			{
				SetValue(UpperValueProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Common")]
		public double MinRange
		{
			get
			{
				return (double)GetValue(MinRangeProperty);
			}
			set
			{
				SetValue(MinRangeProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSelectionRangeEnabled
		{
			get
			{
				return (bool)GetValue(IsSelectionRangeEnabledProperty);
			}
			set
			{
				SetValue(IsSelectionRangeEnabledProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		public double SelectionStart
		{
			get
			{
				return (double)GetValue(SelectionStartProperty);
			}
			set
			{
				SetValue(SelectionStartProperty, value);
			}
		}

		[Bindable(true)]
		[Category("Appearance")]
		public double SelectionEnd
		{
			get
			{
				return (double)GetValue(SelectionEndProperty);
			}
			set
			{
				SetValue(SelectionEndProperty, value);
			}
		}

		public double MovableRange => base.Maximum - base.Minimum - MinRange;

		public event RangeSelectionChangedEventHandler RangeSelectionChanged
		{
			add
			{
				AddHandler(RangeSelectionChangedEvent, value);
			}
			remove
			{
				RemoveHandler(RangeSelectionChangedEvent, value);
			}
		}

		public event RangeParameterChangedEventHandler LowerValueChanged
		{
			add
			{
				AddHandler(LowerValueChangedEvent, value);
			}
			remove
			{
				RemoveHandler(LowerValueChangedEvent, value);
			}
		}

		public event RangeParameterChangedEventHandler UpperValueChanged
		{
			add
			{
				AddHandler(UpperValueChangedEvent, value);
			}
			remove
			{
				RemoveHandler(UpperValueChangedEvent, value);
			}
		}

		public event DragStartedEventHandler LowerThumbDragStarted
		{
			add
			{
				AddHandler(LowerThumbDragStartedEvent, value);
			}
			remove
			{
				RemoveHandler(LowerThumbDragStartedEvent, value);
			}
		}

		public event DragCompletedEventHandler LowerThumbDragCompleted
		{
			add
			{
				AddHandler(LowerThumbDragCompletedEvent, value);
			}
			remove
			{
				RemoveHandler(LowerThumbDragCompletedEvent, value);
			}
		}

		public event DragStartedEventHandler UpperThumbDragStarted
		{
			add
			{
				AddHandler(UpperThumbDragStartedEvent, value);
			}
			remove
			{
				RemoveHandler(UpperThumbDragStartedEvent, value);
			}
		}

		public event DragCompletedEventHandler UpperThumbDragCompleted
		{
			add
			{
				AddHandler(UpperThumbDragCompletedEvent, value);
			}
			remove
			{
				RemoveHandler(UpperThumbDragCompletedEvent, value);
			}
		}

		public event DragStartedEventHandler CentralThumbDragStarted
		{
			add
			{
				AddHandler(CentralThumbDragStartedEvent, value);
			}
			remove
			{
				RemoveHandler(CentralThumbDragStartedEvent, value);
			}
		}

		public event DragCompletedEventHandler CentralThumbDragCompleted
		{
			add
			{
				AddHandler(CentralThumbDragCompletedEvent, value);
			}
			remove
			{
				RemoveHandler(CentralThumbDragCompletedEvent, value);
			}
		}

		public event DragDeltaEventHandler LowerThumbDragDelta
		{
			add
			{
				AddHandler(LowerThumbDragDeltaEvent, value);
			}
			remove
			{
				RemoveHandler(LowerThumbDragDeltaEvent, value);
			}
		}

		public event DragDeltaEventHandler UpperThumbDragDelta
		{
			add
			{
				AddHandler(UpperThumbDragDeltaEvent, value);
			}
			remove
			{
				RemoveHandler(UpperThumbDragDeltaEvent, value);
			}
		}

		public event DragDeltaEventHandler CentralThumbDragDelta
		{
			add
			{
				AddHandler(CentralThumbDragDeltaEvent, value);
			}
			remove
			{
				RemoveHandler(CentralThumbDragDeltaEvent, value);
			}
		}

		private static void OnSelectionStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RangeSlider)d).CoerceValue(SelectionEndProperty);
		}

		private static object CoerceSelectionStart(DependencyObject d, object value)
		{
			RangeSlider obj = (RangeSlider)d;
			double num = (double)value;
			double minimum = obj.Minimum;
			double maximum = obj.Maximum;
			if (num < minimum)
			{
				return minimum;
			}
			if (num > maximum)
			{
				return maximum;
			}
			return value;
		}

		private static void OnSelectionEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		private static object CoerceSelectionEnd(DependencyObject d, object value)
		{
			RangeSlider obj = (RangeSlider)d;
			double num = (double)value;
			double selectionStart = obj.SelectionStart;
			double maximum = obj.Maximum;
			if (num < selectionStart)
			{
				return selectionStart;
			}
			if (num > maximum)
			{
				return maximum;
			}
			return value;
		}

		public RangeSlider()
		{
			base.CommandBindings.Add(new CommandBinding(MoveBack, MoveBackHandler));
			base.CommandBindings.Add(new CommandBinding(MoveForward, MoveForwardHandler));
			base.CommandBindings.Add(new CommandBinding(MoveAllForward, MoveAllForwardHandler));
			base.CommandBindings.Add(new CommandBinding(MoveAllBack, MoveAllBackHandler));
			actualWidthPropertyChangeNotifier = new PropertyChangeNotifier(this, FrameworkElement.ActualWidthProperty);
			actualWidthPropertyChangeNotifier.ValueChanged += delegate
			{
				ReCalculateSize();
			};
			actualHeightPropertyChangeNotifier = new PropertyChangeNotifier(this, FrameworkElement.ActualHeightProperty);
			actualHeightPropertyChangeNotifier.ValueChanged += delegate
			{
				ReCalculateSize();
			};
			_timer = new DispatcherTimer();
			_timer.Tick += MoveToNextValue;
			_timer.Interval = TimeSpan.FromMilliseconds((double)Interval);
		}

		static RangeSlider()
		{
			MoveBack = new RoutedUICommand("MoveBack", "MoveBack", typeof(RangeSlider), new InputGestureCollection(new InputGesture[1]
			{
				new KeyGesture(Key.B, ModifierKeys.Control)
			}));
			MoveForward = new RoutedUICommand("MoveForward", "MoveForward", typeof(RangeSlider), new InputGestureCollection(new InputGesture[1]
			{
				new KeyGesture(Key.F, ModifierKeys.Control)
			}));
			MoveAllForward = new RoutedUICommand("MoveAllForward", "MoveAllForward", typeof(RangeSlider), new InputGestureCollection(new InputGesture[1]
			{
				new KeyGesture(Key.F, ModifierKeys.Alt)
			}));
			MoveAllBack = new RoutedUICommand("MoveAllBack", "MoveAllBack", typeof(RangeSlider), new InputGestureCollection(new InputGesture[1]
			{
				new KeyGesture(Key.B, ModifierKeys.Alt)
			}));
			RangeSelectionChangedEvent = EventManager.RegisterRoutedEvent("RangeSelectionChanged", RoutingStrategy.Bubble, typeof(RangeSelectionChangedEventHandler), typeof(RangeSlider));
			LowerValueChangedEvent = EventManager.RegisterRoutedEvent("LowerValueChanged", RoutingStrategy.Bubble, typeof(RangeParameterChangedEventHandler), typeof(RangeSlider));
			UpperValueChangedEvent = EventManager.RegisterRoutedEvent("UpperValueChanged", RoutingStrategy.Bubble, typeof(RangeParameterChangedEventHandler), typeof(RangeSlider));
			LowerThumbDragStartedEvent = EventManager.RegisterRoutedEvent("LowerThumbDragStarted", RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(RangeSlider));
			LowerThumbDragCompletedEvent = EventManager.RegisterRoutedEvent("LowerThumbDragCompleted", RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(RangeSlider));
			UpperThumbDragStartedEvent = EventManager.RegisterRoutedEvent("UpperThumbDragStarted", RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(RangeSlider));
			UpperThumbDragCompletedEvent = EventManager.RegisterRoutedEvent("UpperThumbDragCompleted", RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(RangeSlider));
			CentralThumbDragStartedEvent = EventManager.RegisterRoutedEvent("CentralThumbDragStarted", RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(RangeSlider));
			CentralThumbDragCompletedEvent = EventManager.RegisterRoutedEvent("CentralThumbDragCompleted", RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(RangeSlider));
			LowerThumbDragDeltaEvent = EventManager.RegisterRoutedEvent("LowerThumbDragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(RangeSlider));
			UpperThumbDragDeltaEvent = EventManager.RegisterRoutedEvent("UpperThumbDragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(RangeSlider));
			CentralThumbDragDeltaEvent = EventManager.RegisterRoutedEvent("CentralThumbDragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(RangeSlider));
			UpperValueProperty = DependencyProperty.Register("UpperValue", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RangesChanged, CoerceUpperValue));
			LowerValueProperty = DependencyProperty.Register("LowerValue", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RangesChanged, CoerceLowerValue));
			MinRangeProperty = DependencyProperty.Register("MinRange", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata(0.0, MinRangeChanged, CoerceMinRange), IsValidMinRange);
			MinRangeWidthProperty = DependencyProperty.Register("MinRangeWidth", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata(30.0, MinRangeWidthChanged, CoerceMinRangeWidth), IsValidMinRange);
			MoveWholeRangeProperty = DependencyProperty.Register("MoveWholeRange", typeof(bool), typeof(RangeSlider), new PropertyMetadata(false));
			ExtendedModeProperty = DependencyProperty.Register("ExtendedMode", typeof(bool), typeof(RangeSlider), new PropertyMetadata(false));
			IsSnapToTickEnabledProperty = DependencyProperty.Register("IsSnapToTickEnabled", typeof(bool), typeof(RangeSlider), new PropertyMetadata(false));
			OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(RangeSlider), new FrameworkPropertyMetadata(Orientation.Horizontal));
			TickPlacementProperty = DependencyProperty.Register("TickPlacement", typeof(TickPlacement), typeof(RangeSlider), new FrameworkPropertyMetadata(TickPlacement.None));
			TickFrequencyProperty = DependencyProperty.Register("TickFrequency", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata(1.0), IsValidDoubleValue);
			TicksProperty = DependencyProperty.Register("Ticks", typeof(DoubleCollection), typeof(RangeSlider), new FrameworkPropertyMetadata((object)null));
			IsMoveToPointEnabledProperty = DependencyProperty.Register("IsMoveToPointEnabled", typeof(bool), typeof(RangeSlider), new PropertyMetadata(false));
			AutoToolTipPlacementProperty = DependencyProperty.Register("AutoToolTipPlacement", typeof(AutoToolTipPlacement), typeof(RangeSlider), new FrameworkPropertyMetadata(AutoToolTipPlacement.None));
			AutoToolTipPrecisionProperty = DependencyProperty.Register("AutoToolTipPrecision", typeof(int), typeof(RangeSlider), new FrameworkPropertyMetadata(0), IsValidPrecision);
			AutoToolTipTextConverterProperty = DependencyProperty.Register("AutoToolTipTextConverter", typeof(IValueConverter), typeof(RangeSlider), new FrameworkPropertyMetadata(null));
			IntervalProperty = DependencyProperty.Register("Interval", typeof(int), typeof(RangeSlider), new FrameworkPropertyMetadata(100, IntervalChangedCallback), IsValidPrecision);
			IsSelectionRangeEnabledProperty = DependencyProperty.Register("IsSelectionRangeEnabled", typeof(bool), typeof(RangeSlider), new FrameworkPropertyMetadata(false));
			SelectionStartProperty = DependencyProperty.Register("SelectionStart", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectionStartChanged, CoerceSelectionStart), IsValidDoubleValue);
			SelectionEndProperty = DependencyProperty.Register("SelectionEnd", typeof(double), typeof(RangeSlider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectionEndChanged, CoerceSelectionEnd), IsValidDoubleValue);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata(typeof(RangeSlider)));
			RangeBase.MinimumProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, MinPropertyChangedCallback, CoerceMinimum));
			RangeBase.MaximumProperty.OverrideMetadata(typeof(RangeSlider), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsMeasure, MaxPropertyChangedCallback, CoerceMaximum));
		}

		protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
			CoerceValue(SelectionStartProperty);
			ReCalculateSize();
		}

		protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
			CoerceValue(SelectionStartProperty);
			CoerceValue(SelectionEndProperty);
			ReCalculateSize();
		}

		private void MoveAllBackHandler(object sender, ExecutedRoutedEventArgs e)
		{
			ResetSelection(isStart: true);
		}

		private void MoveAllForwardHandler(object sender, ExecutedRoutedEventArgs e)
		{
			ResetSelection(isStart: false);
		}

		private void MoveBackHandler(object sender, ExecutedRoutedEventArgs e)
		{
			MoveSelection(isLeft: true);
		}

		private void MoveForwardHandler(object sender, ExecutedRoutedEventArgs e)
		{
			MoveSelection(isLeft: false);
		}

		private static void MoveThumb(FrameworkElement x, FrameworkElement y, double change, Orientation orientation)
		{
			Direction direction = Direction.Increase;
			MoveThumb(x, y, change, orientation, out direction);
		}

		private static void MoveThumb(FrameworkElement x, FrameworkElement y, double change, Orientation orientation, out Direction direction)
		{
			direction = Direction.Increase;
			switch (orientation)
			{
			case Orientation.Horizontal:
				direction = ((change < 0.0) ? Direction.Decrease : Direction.Increase);
				MoveThumbHorizontal(x, y, change);
				break;
			case Orientation.Vertical:
				direction = ((!(change < 0.0)) ? Direction.Decrease : Direction.Increase);
				MoveThumbVertical(x, y, change);
				break;
			}
		}

		private static void MoveThumbHorizontal(FrameworkElement x, FrameworkElement y, double horizonalChange)
		{
			if (!double.IsNaN(x.Width) && !double.IsNaN(y.Width))
			{
				if (horizonalChange < 0.0)
				{
					double changeKeepPositive = GetChangeKeepPositive(x.Width, horizonalChange);
					if (x.Name == "PART_MiddleThumb")
					{
						if (x.Width > x.MinWidth)
						{
							if (x.Width + changeKeepPositive < x.MinWidth)
							{
								double num = x.Width - x.MinWidth;
								x.Width = x.MinWidth;
								y.Width += num;
							}
							else
							{
								x.Width += changeKeepPositive;
								y.Width -= changeKeepPositive;
							}
						}
					}
					else
					{
						x.Width += changeKeepPositive;
						y.Width -= changeKeepPositive;
					}
				}
				else if (horizonalChange > 0.0)
				{
					double num2 = 0.0 - GetChangeKeepPositive(y.Width, 0.0 - horizonalChange);
					if (y.Name == "PART_MiddleThumb")
					{
						if (y.Width > y.MinWidth)
						{
							if (y.Width - num2 < y.MinWidth)
							{
								double num3 = y.Width - y.MinWidth;
								y.Width = y.MinWidth;
								x.Width += num3;
							}
							else
							{
								x.Width += num2;
								y.Width -= num2;
							}
						}
					}
					else
					{
						x.Width += num2;
						y.Width -= num2;
					}
				}
			}
		}

		private static void MoveThumbVertical(FrameworkElement x, FrameworkElement y, double verticalChange)
		{
			if (!double.IsNaN(x.Height) && !double.IsNaN(y.Height))
			{
				if (verticalChange < 0.0)
				{
					double num = 0.0 - GetChangeKeepPositive(y.Height, verticalChange);
					if (y.Name == "PART_MiddleThumb")
					{
						if (y.Height > y.MinHeight)
						{
							if (y.Height - num < y.MinHeight)
							{
								double num2 = y.Height - y.MinHeight;
								y.Height = y.MinHeight;
								x.Height += num2;
							}
							else
							{
								x.Height += num;
								y.Height -= num;
							}
						}
					}
					else
					{
						x.Height += num;
						y.Height -= num;
					}
				}
				else if (verticalChange > 0.0)
				{
					double changeKeepPositive = GetChangeKeepPositive(x.Height, 0.0 - verticalChange);
					if (x.Name == "PART_MiddleThumb")
					{
						if (x.Height > y.MinHeight)
						{
							if (x.Height + changeKeepPositive < x.MinHeight)
							{
								double num3 = x.Height - x.MinHeight;
								x.Height = x.MinHeight;
								y.Height += num3;
							}
							else
							{
								x.Height += changeKeepPositive;
								y.Height -= changeKeepPositive;
							}
						}
					}
					else
					{
						x.Height += changeKeepPositive;
						y.Height -= changeKeepPositive;
					}
				}
			}
		}

		private void ReCalculateSize()
		{
			if (_leftButton != null && _rightButton != null && _centerThumb != null)
			{
				if (Orientation == Orientation.Horizontal)
				{
					_movableWidth = Math.Max(base.ActualWidth - _rightThumb.ActualWidth - _leftThumb.ActualWidth - MinRangeWidth, 1.0);
					if (MovableRange <= 0.0)
					{
						_leftButton.Width = double.NaN;
						_rightButton.Width = double.NaN;
					}
					else
					{
						_leftButton.Width = Math.Max(_movableWidth * (LowerValue - base.Minimum) / MovableRange, 0.0);
						_rightButton.Width = Math.Max(_movableWidth * (base.Maximum - UpperValue) / MovableRange, 0.0);
					}
					if (IsValidDouble(_rightButton.Width) && IsValidDouble(_leftButton.Width))
					{
						_centerThumb.Width = Math.Max(base.ActualWidth - (_leftButton.Width + _rightButton.Width + _rightThumb.ActualWidth + _leftThumb.ActualWidth), 0.0);
					}
					else
					{
						_centerThumb.Width = Math.Max(base.ActualWidth - (_rightThumb.ActualWidth + _leftThumb.ActualWidth), 0.0);
					}
				}
				else if (Orientation == Orientation.Vertical)
				{
					_movableWidth = Math.Max(base.ActualHeight - _rightThumb.ActualHeight - _leftThumb.ActualHeight - MinRangeWidth, 1.0);
					if (MovableRange <= 0.0)
					{
						_leftButton.Height = double.NaN;
						_rightButton.Height = double.NaN;
					}
					else
					{
						_leftButton.Height = Math.Max(_movableWidth * (LowerValue - base.Minimum) / MovableRange, 0.0);
						_rightButton.Height = Math.Max(_movableWidth * (base.Maximum - UpperValue) / MovableRange, 0.0);
					}
					if (IsValidDouble(_rightButton.Height) && IsValidDouble(_leftButton.Height))
					{
						_centerThumb.Height = Math.Max(base.ActualHeight - (_leftButton.Height + _rightButton.Height + _rightThumb.ActualHeight + _leftThumb.ActualHeight), 0.0);
					}
					else
					{
						_centerThumb.Height = Math.Max(base.ActualHeight - (_rightThumb.ActualHeight + _leftThumb.ActualHeight), 0.0);
					}
				}
				_density = _movableWidth / MovableRange;
			}
		}

		private void ReCalculateRangeSelected(bool reCalculateLowerValue, bool reCalculateUpperValue, Direction direction)
		{
			_internalUpdate = true;
			if (direction == Direction.Increase)
			{
				if (reCalculateUpperValue)
				{
					_oldUpper = UpperValue;
					double num = (Orientation == Orientation.Horizontal) ? _rightButton.Width : _rightButton.Height;
					if (IsValidDouble(num))
					{
						double num2 = object.Equals(num, 0.0) ? base.Maximum : Math.Min(base.Maximum, base.Maximum - MovableRange * num / _movableWidth);
						UpperValue = (_isMoved ? num2 : (_roundToPrecision ? Math.Round(num2, _precision) : num2));
					}
				}
				if (reCalculateLowerValue)
				{
					_oldLower = LowerValue;
					double num3 = (Orientation == Orientation.Horizontal) ? _leftButton.Width : _leftButton.Height;
					if (IsValidDouble(num3))
					{
						double num4 = object.Equals(num3, 0.0) ? base.Minimum : Math.Max(base.Minimum, base.Minimum + MovableRange * num3 / _movableWidth);
						LowerValue = (_isMoved ? num4 : (_roundToPrecision ? Math.Round(num4, _precision) : num4));
					}
				}
			}
			else
			{
				if (reCalculateLowerValue)
				{
					_oldLower = LowerValue;
					double num5 = (Orientation == Orientation.Horizontal) ? _leftButton.Width : _leftButton.Height;
					if (IsValidDouble(num5))
					{
						double num6 = object.Equals(num5, 0.0) ? base.Minimum : Math.Max(base.Minimum, base.Minimum + MovableRange * num5 / _movableWidth);
						LowerValue = (_isMoved ? num6 : (_roundToPrecision ? Math.Round(num6, _precision) : num6));
					}
				}
				if (reCalculateUpperValue)
				{
					_oldUpper = UpperValue;
					double num7 = (Orientation == Orientation.Horizontal) ? _rightButton.Width : _rightButton.Height;
					if (IsValidDouble(num7))
					{
						double num8 = object.Equals(num7, 0.0) ? base.Maximum : Math.Min(base.Maximum, base.Maximum - MovableRange * num7 / _movableWidth);
						UpperValue = (_isMoved ? num8 : (_roundToPrecision ? Math.Round(num8, _precision) : num8));
					}
				}
			}
			_roundToPrecision = false;
			_internalUpdate = false;
			RaiseValueChangedEvents(this, reCalculateLowerValue, reCalculateUpperValue);
		}

		private void ReCalculateRangeSelected(bool reCalculateLowerValue, bool reCalculateUpperValue, double value, Direction direction)
		{
			_internalUpdate = true;
			string text = TickFrequency.ToString(CultureInfo.InvariantCulture);
			if (reCalculateLowerValue)
			{
				_oldLower = LowerValue;
				double num = 0.0;
				if (IsSnapToTickEnabled)
				{
					num = ((direction == Direction.Increase) ? Math.Min(UpperValue - MinRange, value) : Math.Max(base.Minimum, value));
				}
				if (!text.ToLower().Contains("e+") && text.Contains("."))
				{
					string[] array = text.Split('.');
					LowerValue = Math.Round(num, array[1].Length, MidpointRounding.AwayFromZero);
				}
				else
				{
					LowerValue = num;
				}
			}
			if (reCalculateUpperValue)
			{
				_oldUpper = UpperValue;
				double num2 = 0.0;
				if (IsSnapToTickEnabled)
				{
					num2 = ((direction == Direction.Increase) ? Math.Min(value, base.Maximum) : Math.Max(LowerValue + MinRange, value));
				}
				if (!text.ToLower().Contains("e+") && text.Contains("."))
				{
					string[] array2 = text.Split('.');
					UpperValue = Math.Round(num2, array2[1].Length, MidpointRounding.AwayFromZero);
				}
				else
				{
					UpperValue = num2;
				}
			}
			_internalUpdate = false;
			RaiseValueChangedEvents(this, reCalculateLowerValue, reCalculateUpperValue);
		}

		private void ReCalculateRangeSelected(double newLower, double newUpper, Direction direction)
		{
			double num = 0.0;
			double num2 = 0.0;
			_internalUpdate = true;
			_oldLower = LowerValue;
			_oldUpper = UpperValue;
			if (IsSnapToTickEnabled)
			{
				if (direction == Direction.Increase)
				{
					num = Math.Min(newLower, base.Maximum - (UpperValue - LowerValue));
					num2 = Math.Min(newUpper, base.Maximum);
				}
				else
				{
					num = Math.Max(newLower, base.Minimum);
					num2 = Math.Max(base.Minimum + (UpperValue - LowerValue), newUpper);
				}
				string text = TickFrequency.ToString(CultureInfo.InvariantCulture);
				if (!text.ToLower().Contains("e+") && text.Contains("."))
				{
					string[] array = text.Split('.');
					if (direction == Direction.Decrease)
					{
						LowerValue = Math.Round(num, array[1].Length, MidpointRounding.AwayFromZero);
						UpperValue = Math.Round(num2, array[1].Length, MidpointRounding.AwayFromZero);
					}
					else
					{
						UpperValue = Math.Round(num2, array[1].Length, MidpointRounding.AwayFromZero);
						LowerValue = Math.Round(num, array[1].Length, MidpointRounding.AwayFromZero);
					}
				}
				else if (direction == Direction.Decrease)
				{
					LowerValue = num;
					UpperValue = num2;
				}
				else
				{
					UpperValue = num2;
					LowerValue = num;
				}
			}
			_internalUpdate = false;
			RaiseValueChangedEvents(this);
		}

		private void OnRangeParameterChanged(RangeParameterChangedEventArgs e, RoutedEvent Event)
		{
			e.RoutedEvent = Event;
			RaiseEvent(e);
		}

		public void MoveSelection(bool isLeft)
		{
			double num = base.SmallChange * (UpperValue - LowerValue) * _movableWidth / MovableRange;
			num = (isLeft ? (0.0 - num) : num);
			MoveThumb(_leftButton, _rightButton, num, Orientation, out _direction);
			ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: true, _direction);
		}

		public void ResetSelection(bool isStart)
		{
			double num = base.Maximum - base.Minimum;
			num = (isStart ? (0.0 - num) : num);
			MoveThumb(_leftButton, _rightButton, num, Orientation, out _direction);
			ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: true, _direction);
		}

		private void OnRangeSelectionChanged(RangeSelectionChangedEventArgs e)
		{
			e.RoutedEvent = RangeSelectionChangedEvent;
			RaiseEvent(e);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_container = (GetTemplateChild("PART_Container") as FrameworkElement);
			_visualElementsContainer = (GetTemplateChild("PART_RangeSliderContainer") as StackPanel);
			_centerThumb = (GetTemplateChild("PART_MiddleThumb") as Thumb);
			_leftButton = (GetTemplateChild("PART_LeftEdge") as RepeatButton);
			_rightButton = (GetTemplateChild("PART_RightEdge") as RepeatButton);
			_leftThumb = (GetTemplateChild("PART_LeftThumb") as Thumb);
			_rightThumb = (GetTemplateChild("PART_RightThumb") as Thumb);
			InitializeVisualElementsContainer();
			ReCalculateSize();
		}

		private void InitializeVisualElementsContainer()
		{
			if (_visualElementsContainer != null && _leftThumb != null && _rightThumb != null && _centerThumb != null)
			{
				_leftThumb.DragCompleted -= LeftThumbDragComplete;
				_rightThumb.DragCompleted -= RightThumbDragComplete;
				_leftThumb.DragStarted -= LeftThumbDragStart;
				_rightThumb.DragStarted -= RightThumbDragStart;
				_centerThumb.DragStarted -= CenterThumbDragStarted;
				_centerThumb.DragCompleted -= CenterThumbDragCompleted;
				_centerThumb.DragDelta -= CenterThumbDragDelta;
				_leftThumb.DragDelta -= LeftThumbDragDelta;
				_rightThumb.DragDelta -= RightThumbDragDelta;
				_visualElementsContainer.PreviewMouseDown -= VisualElementsContainerPreviewMouseDown;
				_visualElementsContainer.PreviewMouseUp -= VisualElementsContainerPreviewMouseUp;
				_visualElementsContainer.MouseLeave -= VisualElementsContainerMouseLeave;
				_visualElementsContainer.MouseDown -= VisualElementsContainerMouseDown;
				_leftThumb.DragCompleted += LeftThumbDragComplete;
				_rightThumb.DragCompleted += RightThumbDragComplete;
				_leftThumb.DragStarted += LeftThumbDragStart;
				_rightThumb.DragStarted += RightThumbDragStart;
				_centerThumb.DragStarted += CenterThumbDragStarted;
				_centerThumb.DragCompleted += CenterThumbDragCompleted;
				_centerThumb.DragDelta += CenterThumbDragDelta;
				_leftThumb.DragDelta += LeftThumbDragDelta;
				_rightThumb.DragDelta += RightThumbDragDelta;
				_visualElementsContainer.PreviewMouseDown += VisualElementsContainerPreviewMouseDown;
				_visualElementsContainer.PreviewMouseUp += VisualElementsContainerPreviewMouseUp;
				_visualElementsContainer.MouseLeave += VisualElementsContainerMouseLeave;
				_visualElementsContainer.MouseDown += VisualElementsContainerMouseDown;
			}
		}

		private void VisualElementsContainerPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			Point position = Mouse.GetPosition(_visualElementsContainer);
			if (Orientation == Orientation.Horizontal)
			{
				if (position.X < _leftButton.ActualWidth)
				{
					LeftButtonMouseDown();
				}
				else if (position.X > base.ActualWidth - _rightButton.ActualWidth)
				{
					RightButtonMouseDown();
				}
				else if (position.X > _leftButton.ActualWidth + _leftThumb.ActualWidth && position.X < base.ActualWidth - (_rightButton.ActualWidth + _rightThumb.ActualWidth))
				{
					CentralThumbMouseDown();
				}
			}
			else if (position.Y > base.ActualHeight - _leftButton.ActualHeight)
			{
				LeftButtonMouseDown();
			}
			else if (position.Y < _rightButton.ActualHeight)
			{
				RightButtonMouseDown();
			}
			else if (position.Y > _rightButton.ActualHeight + _rightButton.ActualHeight && position.Y < base.ActualHeight - (_leftButton.ActualHeight + _leftThumb.ActualHeight))
			{
				CentralThumbMouseDown();
			}
		}

		private void VisualElementsContainerMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.MiddleButton == MouseButtonState.Pressed)
			{
				MoveWholeRange = !MoveWholeRange;
			}
		}

		private void VisualElementsContainerMouseLeave(object sender, MouseEventArgs e)
		{
			_tickCount = 0u;
			_timer.Stop();
		}

		private void VisualElementsContainerPreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			_tickCount = 0u;
			_timer.Stop();
			_centerThumbBlocked = false;
		}

		private void LeftButtonMouseDown()
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				Point position = Mouse.GetPosition(_visualElementsContainer);
				double num = (Orientation == Orientation.Horizontal) ? (_leftButton.ActualWidth - position.X + _leftThumb.ActualWidth / 2.0) : (0.0 - (_leftButton.ActualHeight - (base.ActualHeight - (position.Y + _leftThumb.ActualHeight / 2.0))));
				if (!IsSnapToTickEnabled)
				{
					if (IsMoveToPointEnabled && !MoveWholeRange)
					{
						MoveThumb(_leftButton, _centerThumb, 0.0 - num, Orientation, out _direction);
						ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: false, _direction);
					}
					else if (IsMoveToPointEnabled && MoveWholeRange)
					{
						MoveThumb(_leftButton, _rightButton, 0.0 - num, Orientation, out _direction);
						ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: true, _direction);
					}
				}
				else if (IsMoveToPointEnabled && !MoveWholeRange)
				{
					JumpToNextTick(Direction.Decrease, ButtonType.BottomLeft, 0.0 - num, LowerValue, jumpDirectlyToTick: true);
				}
				else if (IsMoveToPointEnabled && MoveWholeRange)
				{
					JumpToNextTick(Direction.Decrease, ButtonType.Both, 0.0 - num, LowerValue, jumpDirectlyToTick: true);
				}
				if (!IsMoveToPointEnabled)
				{
					_position = Mouse.GetPosition(_visualElementsContainer);
					_bType = (MoveWholeRange ? ButtonType.Both : ButtonType.BottomLeft);
					_currentpoint = ((Orientation == Orientation.Horizontal) ? _position.X : _position.Y);
					_currenValue = LowerValue;
					_isInsideRange = false;
					_direction = Direction.Decrease;
					_timer.Start();
				}
			}
		}

		private void RightButtonMouseDown()
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				Point position = Mouse.GetPosition(_visualElementsContainer);
				double num = (Orientation == Orientation.Horizontal) ? (_rightButton.ActualWidth - (base.ActualWidth - (position.X + _rightThumb.ActualWidth / 2.0))) : (0.0 - (_rightButton.ActualHeight - (position.Y - _rightThumb.ActualHeight / 2.0)));
				if (!IsSnapToTickEnabled)
				{
					if (IsMoveToPointEnabled && !MoveWholeRange)
					{
						MoveThumb(_centerThumb, _rightButton, num, Orientation, out _direction);
						ReCalculateRangeSelected(reCalculateLowerValue: false, reCalculateUpperValue: true, _direction);
					}
					else if (IsMoveToPointEnabled && MoveWholeRange)
					{
						MoveThumb(_leftButton, _rightButton, num, Orientation, out _direction);
						ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: true, _direction);
					}
				}
				else if (IsMoveToPointEnabled && !MoveWholeRange)
				{
					JumpToNextTick(Direction.Increase, ButtonType.TopRight, num, UpperValue, jumpDirectlyToTick: true);
				}
				else if (IsMoveToPointEnabled && MoveWholeRange)
				{
					JumpToNextTick(Direction.Increase, ButtonType.Both, num, UpperValue, jumpDirectlyToTick: true);
				}
				if (!IsMoveToPointEnabled)
				{
					_position = Mouse.GetPosition(_visualElementsContainer);
					_bType = ((!MoveWholeRange) ? ButtonType.TopRight : ButtonType.Both);
					_currentpoint = ((Orientation == Orientation.Horizontal) ? _position.X : _position.Y);
					_currenValue = UpperValue;
					_direction = Direction.Increase;
					_isInsideRange = false;
					_timer.Start();
				}
			}
		}

		private void CentralThumbMouseDown()
		{
			if (ExtendedMode)
			{
				if (Mouse.LeftButton == MouseButtonState.Pressed && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
				{
					_centerThumbBlocked = true;
					Point position = Mouse.GetPosition(_visualElementsContainer);
					double num = (Orientation == Orientation.Horizontal) ? (position.X + _leftThumb.ActualWidth / 2.0 - (_leftButton.ActualWidth + _leftThumb.ActualWidth)) : (0.0 - (base.ActualHeight - (position.Y + _leftThumb.ActualHeight / 2.0 + _leftButton.ActualHeight)));
					if (!IsSnapToTickEnabled)
					{
						if (IsMoveToPointEnabled && !MoveWholeRange)
						{
							MoveThumb(_leftButton, _centerThumb, num, Orientation, out _direction);
							ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: false, _direction);
						}
						else if (IsMoveToPointEnabled && MoveWholeRange)
						{
							MoveThumb(_leftButton, _rightButton, num, Orientation, out _direction);
							ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: true, _direction);
						}
					}
					else if (IsMoveToPointEnabled && !MoveWholeRange)
					{
						JumpToNextTick(Direction.Increase, ButtonType.BottomLeft, num, LowerValue, jumpDirectlyToTick: true);
					}
					else if (IsMoveToPointEnabled && MoveWholeRange)
					{
						JumpToNextTick(Direction.Increase, ButtonType.Both, num, LowerValue, jumpDirectlyToTick: true);
					}
					if (!IsMoveToPointEnabled)
					{
						_position = Mouse.GetPosition(_visualElementsContainer);
						_bType = (MoveWholeRange ? ButtonType.Both : ButtonType.BottomLeft);
						_currentpoint = ((Orientation == Orientation.Horizontal) ? _position.X : _position.Y);
						_currenValue = LowerValue;
						_direction = Direction.Increase;
						_isInsideRange = true;
						_timer.Start();
					}
				}
				else if (Mouse.RightButton == MouseButtonState.Pressed && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
				{
					_centerThumbBlocked = true;
					Point position2 = Mouse.GetPosition(_visualElementsContainer);
					double num2 = (Orientation == Orientation.Horizontal) ? (base.ActualWidth - (position2.X + _rightThumb.ActualWidth / 2.0 + _rightButton.ActualWidth)) : (0.0 - (position2.Y + _rightThumb.ActualHeight / 2.0 - (_rightButton.ActualHeight + _rightThumb.ActualHeight)));
					if (!IsSnapToTickEnabled)
					{
						if (IsMoveToPointEnabled && !MoveWholeRange)
						{
							MoveThumb(_centerThumb, _rightButton, 0.0 - num2, Orientation, out _direction);
							ReCalculateRangeSelected(reCalculateLowerValue: false, reCalculateUpperValue: true, _direction);
						}
						else if (IsMoveToPointEnabled && MoveWholeRange)
						{
							MoveThumb(_leftButton, _rightButton, 0.0 - num2, Orientation, out _direction);
							ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: true, _direction);
						}
					}
					else if (IsMoveToPointEnabled && !MoveWholeRange)
					{
						JumpToNextTick(Direction.Decrease, ButtonType.TopRight, 0.0 - num2, UpperValue, jumpDirectlyToTick: true);
					}
					else if (IsMoveToPointEnabled && MoveWholeRange)
					{
						JumpToNextTick(Direction.Decrease, ButtonType.Both, 0.0 - num2, UpperValue, jumpDirectlyToTick: true);
					}
					if (!IsMoveToPointEnabled)
					{
						_position = Mouse.GetPosition(_visualElementsContainer);
						_bType = ((!MoveWholeRange) ? ButtonType.TopRight : ButtonType.Both);
						_currentpoint = ((Orientation == Orientation.Horizontal) ? _position.X : _position.Y);
						_currenValue = UpperValue;
						_direction = Direction.Decrease;
						_isInsideRange = true;
						_timer.Start();
					}
				}
			}
		}

		private void LeftThumbDragStart(object sender, DragStartedEventArgs e)
		{
			_isMoved = true;
			if (AutoToolTipPlacement != 0)
			{
				if (_autoToolTip == null)
				{
					_autoToolTip = new ToolTip();
					_autoToolTip.Placement = PlacementMode.Custom;
					_autoToolTip.CustomPopupPlacementCallback = PopupPlacementCallback;
				}
				_autoToolTip.Content = GetLowerToolTipNumber();
				_autoToolTip.PlacementTarget = _leftThumb;
				_autoToolTip.IsOpen = true;
			}
			_basePoint = Mouse.GetPosition(_container);
			e.RoutedEvent = LowerThumbDragStartedEvent;
			RaiseEvent(e);
		}

		private void LeftThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			double num = (Orientation == Orientation.Horizontal) ? e.HorizontalChange : e.VerticalChange;
			if (!IsSnapToTickEnabled)
			{
				MoveThumb(_leftButton, _centerThumb, num, Orientation, out _direction);
				ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: false, _direction);
			}
			else
			{
				Point position = Mouse.GetPosition(_container);
				if (Orientation == Orientation.Horizontal)
				{
					if (position.X >= 0.0 && position.X < _container.ActualWidth - (_rightButton.ActualWidth + _rightThumb.ActualWidth + _centerThumb.MinWidth))
					{
						Direction direction = (!(position.X > _basePoint.X)) ? Direction.Decrease : Direction.Increase;
						JumpToNextTick(direction, ButtonType.BottomLeft, num, LowerValue, jumpDirectlyToTick: false);
					}
				}
				else if (position.Y <= _container.ActualHeight && position.Y > _rightButton.ActualHeight + _rightThumb.ActualHeight + _centerThumb.MinHeight)
				{
					Direction direction = (!(position.Y < _basePoint.Y)) ? Direction.Decrease : Direction.Increase;
					JumpToNextTick(direction, ButtonType.BottomLeft, 0.0 - num, LowerValue, jumpDirectlyToTick: false);
				}
			}
			_basePoint = Mouse.GetPosition(_container);
			if (AutoToolTipPlacement != 0)
			{
				_autoToolTip.Content = GetLowerToolTipNumber();
				RelocateAutoToolTip();
			}
			e.RoutedEvent = LowerThumbDragDeltaEvent;
			RaiseEvent(e);
		}

		private void LeftThumbDragComplete(object sender, DragCompletedEventArgs e)
		{
			if (_autoToolTip != null)
			{
				_autoToolTip.IsOpen = false;
				_autoToolTip = null;
			}
			e.RoutedEvent = LowerThumbDragCompletedEvent;
			RaiseEvent(e);
		}

		private void RightThumbDragStart(object sender, DragStartedEventArgs e)
		{
			_isMoved = true;
			if (AutoToolTipPlacement != 0)
			{
				if (_autoToolTip == null)
				{
					_autoToolTip = new ToolTip();
					_autoToolTip.Placement = PlacementMode.Custom;
					_autoToolTip.CustomPopupPlacementCallback = PopupPlacementCallback;
				}
				_autoToolTip.Content = GetUpperToolTipNumber();
				_autoToolTip.PlacementTarget = _rightThumb;
				_autoToolTip.IsOpen = true;
			}
			_basePoint = Mouse.GetPosition(_container);
			e.RoutedEvent = UpperThumbDragStartedEvent;
			RaiseEvent(e);
		}

		private void RightThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			double num = (Orientation == Orientation.Horizontal) ? e.HorizontalChange : e.VerticalChange;
			if (!IsSnapToTickEnabled)
			{
				MoveThumb(_centerThumb, _rightButton, num, Orientation, out _direction);
				ReCalculateRangeSelected(reCalculateLowerValue: false, reCalculateUpperValue: true, _direction);
			}
			else
			{
				Point position = Mouse.GetPosition(_container);
				if (Orientation == Orientation.Horizontal)
				{
					if (position.X < _container.ActualWidth && position.X > _leftButton.ActualWidth + _leftThumb.ActualWidth + _centerThumb.MinWidth)
					{
						Direction direction = (!(position.X > _basePoint.X)) ? Direction.Decrease : Direction.Increase;
						JumpToNextTick(direction, ButtonType.TopRight, num, UpperValue, jumpDirectlyToTick: false);
					}
				}
				else if (position.Y >= 0.0 && position.Y < _container.ActualHeight - (_leftButton.ActualHeight + _leftThumb.ActualHeight + _centerThumb.MinHeight))
				{
					Direction direction = (!(position.Y < _basePoint.Y)) ? Direction.Decrease : Direction.Increase;
					JumpToNextTick(direction, ButtonType.TopRight, 0.0 - num, UpperValue, jumpDirectlyToTick: false);
				}
				_basePoint = Mouse.GetPosition(_container);
			}
			if (AutoToolTipPlacement != 0)
			{
				_autoToolTip.Content = GetUpperToolTipNumber();
				RelocateAutoToolTip();
			}
			e.RoutedEvent = UpperThumbDragDeltaEvent;
			RaiseEvent(e);
		}

		private void RightThumbDragComplete(object sender, DragCompletedEventArgs e)
		{
			if (_autoToolTip != null)
			{
				_autoToolTip.IsOpen = false;
				_autoToolTip = null;
			}
			e.RoutedEvent = UpperThumbDragCompletedEvent;
			RaiseEvent(e);
		}

		private void CenterThumbDragStarted(object sender, DragStartedEventArgs e)
		{
			_isMoved = true;
			if (AutoToolTipPlacement != 0)
			{
				if (_autoToolTip == null)
				{
					_autoToolTip = new ToolTip
					{
						Placement = PlacementMode.Custom,
						CustomPopupPlacementCallback = PopupPlacementCallback
					};
				}
				_autoToolTip.Content = GetLowerToolTipNumber() + " ; " + GetUpperToolTipNumber();
				_autoToolTip.PlacementTarget = _centerThumb;
				_autoToolTip.IsOpen = true;
			}
			_basePoint = Mouse.GetPosition(_container);
			e.RoutedEvent = CentralThumbDragStartedEvent;
			RaiseEvent(e);
		}

		private void CenterThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			if (!_centerThumbBlocked)
			{
				double num = (Orientation == Orientation.Horizontal) ? e.HorizontalChange : e.VerticalChange;
				if (!IsSnapToTickEnabled)
				{
					MoveThumb(_leftButton, _rightButton, num, Orientation, out _direction);
					ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: true, _direction);
				}
				else
				{
					Point position = Mouse.GetPosition(_container);
					if (Orientation == Orientation.Horizontal)
					{
						if (position.X >= 0.0 && position.X < _container.ActualWidth)
						{
							Direction direction = (!(position.X > _basePoint.X)) ? Direction.Decrease : Direction.Increase;
							JumpToNextTick(direction, ButtonType.Both, num, (direction == Direction.Increase) ? UpperValue : LowerValue, jumpDirectlyToTick: false);
						}
					}
					else if (position.Y >= 0.0 && position.Y < _container.ActualHeight)
					{
						Direction direction = (!(position.Y < _basePoint.Y)) ? Direction.Decrease : Direction.Increase;
						JumpToNextTick(direction, ButtonType.Both, 0.0 - num, (direction == Direction.Increase) ? UpperValue : LowerValue, jumpDirectlyToTick: false);
					}
				}
				_basePoint = Mouse.GetPosition(_container);
				if (AutoToolTipPlacement != 0)
				{
					_autoToolTip.Content = GetLowerToolTipNumber() + " ; " + GetUpperToolTipNumber();
					RelocateAutoToolTip();
				}
			}
			e.RoutedEvent = CentralThumbDragDeltaEvent;
			RaiseEvent(e);
		}

		private void CenterThumbDragCompleted(object sender, DragCompletedEventArgs e)
		{
			if (_autoToolTip != null)
			{
				_autoToolTip.IsOpen = false;
				_autoToolTip = null;
			}
			e.RoutedEvent = CentralThumbDragCompletedEvent;
			RaiseEvent(e);
		}

		private static double GetChangeKeepPositive(double width, double increment)
		{
			return Math.Max(width + increment, 0.0) - width;
		}

		private double UpdateEndPoint(ButtonType type, Direction dir)
		{
			double result = 0.0;
			switch (dir)
			{
			case Direction.Increase:
				switch (type)
				{
				case ButtonType.Both:
					if (_isInsideRange)
					{
						goto case ButtonType.BottomLeft;
					}
					goto default;
				case ButtonType.BottomLeft:
					result = ((Orientation == Orientation.Horizontal) ? (_leftButton.ActualWidth + _leftThumb.ActualWidth) : (base.ActualHeight - (_leftButton.ActualHeight + _leftThumb.ActualHeight)));
					break;
				default:
					switch (type)
					{
					case ButtonType.Both:
						if (_isInsideRange)
						{
							break;
						}
						goto case ButtonType.TopRight;
					case ButtonType.TopRight:
						result = ((Orientation == Orientation.Horizontal) ? (base.ActualWidth - _rightButton.ActualWidth) : _rightButton.ActualHeight);
						break;
					}
					break;
				}
				break;
			case Direction.Decrease:
				switch (type)
				{
				case ButtonType.Both:
					if (!_isInsideRange)
					{
						goto case ButtonType.BottomLeft;
					}
					goto default;
				case ButtonType.BottomLeft:
					result = ((Orientation == Orientation.Horizontal) ? _leftButton.ActualWidth : (base.ActualHeight - _leftButton.ActualHeight));
					break;
				default:
					switch (type)
					{
					case ButtonType.Both:
						if (!_isInsideRange)
						{
							break;
						}
						goto case ButtonType.TopRight;
					case ButtonType.TopRight:
						result = ((Orientation == Orientation.Horizontal) ? (base.ActualWidth - _rightButton.ActualWidth - _rightThumb.ActualWidth) : (_rightButton.ActualHeight + _rightThumb.ActualHeight));
						break;
					}
					break;
				}
				break;
			}
			return result;
		}

		private bool GetResult(double currentPoint, double endPoint, Direction direction)
		{
			if (direction == Direction.Increase)
			{
				if (Orientation != 0 || !(currentPoint > endPoint))
				{
					if (Orientation == Orientation.Vertical)
					{
						return currentPoint < endPoint;
					}
					return false;
				}
				return true;
			}
			if (Orientation != 0 || !(currentPoint < endPoint))
			{
				if (Orientation == Orientation.Vertical)
				{
					return currentPoint > endPoint;
				}
				return false;
			}
			return true;
		}

		private void MoveToNextValue(object sender, EventArgs e)
		{
			_position = Mouse.GetPosition(_visualElementsContainer);
			_currentpoint = ((Orientation == Orientation.Horizontal) ? _position.X : _position.Y);
			double endPoint = UpdateEndPoint(_bType, _direction);
			bool result = GetResult(_currentpoint, endPoint, _direction);
			if (!IsSnapToTickEnabled)
			{
				double num = base.SmallChange;
				if (_tickCount > 5)
				{
					num = base.LargeChange;
				}
				_roundToPrecision = true;
				if (!num.ToString(CultureInfo.InvariantCulture).ToLower().Contains("e") && num.ToString(CultureInfo.InvariantCulture).Contains("."))
				{
					string[] array = num.ToString(CultureInfo.InvariantCulture).Split('.');
					_precision = array[1].Length;
				}
				else
				{
					_precision = 0;
				}
				num = ((Orientation == Orientation.Horizontal) ? num : (0.0 - num));
				num = ((_direction == Direction.Increase) ? num : (0.0 - num));
				if (result)
				{
					switch (_bType)
					{
					case ButtonType.BottomLeft:
						MoveThumb(_leftButton, _centerThumb, num * _density, Orientation, out _direction);
						ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: false, _direction);
						break;
					case ButtonType.TopRight:
						MoveThumb(_centerThumb, _rightButton, num * _density, Orientation, out _direction);
						ReCalculateRangeSelected(reCalculateLowerValue: false, reCalculateUpperValue: true, _direction);
						break;
					case ButtonType.Both:
						MoveThumb(_leftButton, _rightButton, num * _density, Orientation, out _direction);
						ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: true, _direction);
						break;
					}
				}
			}
			else
			{
				double num = CalculateNextTick(_direction, _currenValue, 0.0, moveDirectlyToNextTick: true);
				double num2 = num;
				num = ((Orientation == Orientation.Horizontal) ? num : (0.0 - num));
				if (_direction == Direction.Increase)
				{
					if (result)
					{
						switch (_bType)
						{
						case ButtonType.BottomLeft:
							MoveThumb(_leftButton, _centerThumb, num * _density, Orientation);
							ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: false, LowerValue + num2, _direction);
							break;
						case ButtonType.TopRight:
							MoveThumb(_centerThumb, _rightButton, num * _density, Orientation);
							ReCalculateRangeSelected(reCalculateLowerValue: false, reCalculateUpperValue: true, UpperValue + num2, _direction);
							break;
						case ButtonType.Both:
							MoveThumb(_leftButton, _rightButton, num * _density, Orientation);
							ReCalculateRangeSelected(LowerValue + num2, UpperValue + num2, _direction);
							break;
						}
					}
				}
				else if (_direction == Direction.Decrease && result)
				{
					switch (_bType)
					{
					case ButtonType.BottomLeft:
						MoveThumb(_leftButton, _centerThumb, (0.0 - num) * _density, Orientation);
						ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: false, LowerValue - num2, _direction);
						break;
					case ButtonType.TopRight:
						MoveThumb(_centerThumb, _rightButton, (0.0 - num) * _density, Orientation);
						ReCalculateRangeSelected(reCalculateLowerValue: false, reCalculateUpperValue: true, UpperValue - num2, _direction);
						break;
					case ButtonType.Both:
						MoveThumb(_leftButton, _rightButton, (0.0 - num) * _density, Orientation);
						ReCalculateRangeSelected(LowerValue - num2, UpperValue - num2, _direction);
						break;
					}
				}
			}
			_tickCount++;
		}

		private void SnapToTickHandle(ButtonType type, Direction direction, double difference)
		{
			double num = difference;
			difference = ((Orientation == Orientation.Horizontal) ? difference : (0.0 - difference));
			if (direction == Direction.Increase)
			{
				switch (type)
				{
				case ButtonType.TopRight:
					if (UpperValue < base.Maximum)
					{
						MoveThumb(_centerThumb, _rightButton, difference * _density, Orientation);
						ReCalculateRangeSelected(reCalculateLowerValue: false, reCalculateUpperValue: true, UpperValue + num, direction);
					}
					break;
				case ButtonType.BottomLeft:
					if (LowerValue < UpperValue - MinRange)
					{
						MoveThumb(_leftButton, _centerThumb, difference * _density, Orientation);
						ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: false, LowerValue + num, direction);
					}
					break;
				case ButtonType.Both:
					if (UpperValue < base.Maximum)
					{
						MoveThumb(_leftButton, _rightButton, difference * _density, Orientation);
						ReCalculateRangeSelected(LowerValue + num, UpperValue + num, direction);
					}
					break;
				}
			}
			else
			{
				switch (type)
				{
				case ButtonType.TopRight:
					if (UpperValue > LowerValue + MinRange)
					{
						MoveThumb(_centerThumb, _rightButton, (0.0 - difference) * _density, Orientation);
						ReCalculateRangeSelected(reCalculateLowerValue: false, reCalculateUpperValue: true, UpperValue - num, direction);
					}
					break;
				case ButtonType.BottomLeft:
					if (LowerValue > base.Minimum)
					{
						MoveThumb(_leftButton, _centerThumb, (0.0 - difference) * _density, Orientation);
						ReCalculateRangeSelected(reCalculateLowerValue: true, reCalculateUpperValue: false, LowerValue - num, direction);
					}
					break;
				case ButtonType.Both:
					if (LowerValue > base.Minimum)
					{
						MoveThumb(_leftButton, _rightButton, (0.0 - difference) * _density, Orientation);
						ReCalculateRangeSelected(LowerValue - num, UpperValue - num, direction);
					}
					break;
				}
			}
		}

		private double CalculateNextTick(Direction direction, double checkingValue, double distance, bool moveDirectlyToNextTick)
		{
			double num = checkingValue - base.Minimum;
			if (!IsMoveToPointEnabled)
			{
				double num2 = num / TickFrequency;
				if (!IsDoubleCloseToInt(num2))
				{
					distance = TickFrequency * (double)(int)num2;
					if (direction == Direction.Increase)
					{
						distance += TickFrequency;
					}
					distance -= Math.Abs(num);
					_currenValue = 0.0;
					return Math.Abs(distance);
				}
			}
			if (moveDirectlyToNextTick)
			{
				distance = TickFrequency;
			}
			else
			{
				double num3 = (num + distance / _density) / TickFrequency;
				if (direction == Direction.Increase)
				{
					distance = (num3.ToString(CultureInfo.InvariantCulture).ToLower().Contains("e+") ? (num3 * TickFrequency + TickFrequency) : ((double)(int)num3 * TickFrequency + TickFrequency)) - Math.Abs(num);
				}
				else
				{
					double num4 = num3.ToString(CultureInfo.InvariantCulture).ToLower().Contains("e+") ? (num3 * TickFrequency) : ((double)(int)num3 * TickFrequency);
					distance = Math.Abs(num) - num4;
				}
			}
			return Math.Abs(distance);
		}

		private void JumpToNextTick(Direction direction, ButtonType type, double distance, double checkingValue, bool jumpDirectlyToTick)
		{
			double num = CalculateNextTick(direction, checkingValue, distance, moveDirectlyToNextTick: false);
			Point position = Mouse.GetPosition(_visualElementsContainer);
			double num2 = (Orientation == Orientation.Horizontal) ? position.X : position.Y;
			double num3 = (Orientation == Orientation.Horizontal) ? base.ActualWidth : base.ActualHeight;
			double num4 = (direction == Direction.Increase) ? (TickFrequency * _density) : ((0.0 - TickFrequency) * _density);
			if (jumpDirectlyToTick)
			{
				SnapToTickHandle(type, direction, num);
			}
			else if (direction == Direction.Increase)
			{
				if (!IsDoubleCloseToInt(checkingValue / TickFrequency))
				{
					if (distance > num * _density / 2.0 || distance >= num3 - num2 || distance >= num2)
					{
						SnapToTickHandle(type, direction, num);
					}
				}
				else if (distance > num4 / 2.0 || distance >= num3 - num2 || distance >= num2)
				{
					SnapToTickHandle(type, direction, num);
				}
			}
			else if (!IsDoubleCloseToInt(checkingValue / TickFrequency))
			{
				if (distance <= (0.0 - num * _density) / 2.0 || UpperValue - LowerValue < num)
				{
					SnapToTickHandle(type, direction, num);
				}
			}
			else if (distance < num4 / 2.0 || UpperValue - LowerValue < num)
			{
				SnapToTickHandle(type, direction, num);
			}
		}

		private void RelocateAutoToolTip()
		{
			double horizontalOffset = _autoToolTip.HorizontalOffset;
			_autoToolTip.HorizontalOffset = horizontalOffset + 0.001;
			_autoToolTip.HorizontalOffset = horizontalOffset;
		}

		private bool ApproximatelyEquals(double value1, double value2)
		{
			return Math.Abs(value1 - value2) <= 1.53E-06;
		}

		private bool IsDoubleCloseToInt(double val)
		{
			return ApproximatelyEquals(Math.Abs(val - Math.Round(val)), 0.0);
		}

		private string GetLowerToolTipNumber()
		{
			double lowerValue = LowerValue;
			return GetToolTipNumber(lowerValue);
		}

		private string GetUpperToolTipNumber()
		{
			double upperValue = UpperValue;
			return GetToolTipNumber(upperValue);
		}

		private string GetToolTipNumber(double value)
		{
			IValueConverter autoToolTipTextConverter = AutoToolTipTextConverter;
			if (autoToolTipTextConverter != null)
			{
				object obj = autoToolTipTextConverter.Convert(value, typeof(string), null, CultureInfo.InvariantCulture);
				if (obj != null)
				{
					return obj.ToString();
				}
			}
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
			numberFormatInfo.NumberDecimalDigits = AutoToolTipPrecision;
			return value.ToString("N", numberFormatInfo);
		}

		private CustomPopupPlacement[] PopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
		{
			switch (AutoToolTipPlacement)
			{
			case AutoToolTipPlacement.TopLeft:
				if (Orientation != 0)
				{
					return new CustomPopupPlacement[1]
					{
						new CustomPopupPlacement(new Point(0.0 - popupSize.Width, (targetSize.Height - popupSize.Height) * 0.5), PopupPrimaryAxis.Vertical)
					};
				}
				return new CustomPopupPlacement[1]
				{
					new CustomPopupPlacement(new Point((targetSize.Width - popupSize.Width) * 0.5, 0.0 - popupSize.Height), PopupPrimaryAxis.Horizontal)
				};
			case AutoToolTipPlacement.BottomRight:
				if (Orientation != 0)
				{
					return new CustomPopupPlacement[1]
					{
						new CustomPopupPlacement(new Point(targetSize.Width, (targetSize.Height - popupSize.Height) * 0.5), PopupPrimaryAxis.Vertical)
					};
				}
				return new CustomPopupPlacement[1]
				{
					new CustomPopupPlacement(new Point((targetSize.Width - popupSize.Width) * 0.5, targetSize.Height), PopupPrimaryAxis.Horizontal)
				};
			default:
				return new CustomPopupPlacement[0];
			}
		}

		private static bool IsValidDoubleValue(object value)
		{
			if (value is double)
			{
				return IsValidDouble((double)value);
			}
			return false;
		}

		private static bool IsValidDouble(double d)
		{
			if (!double.IsNaN(d))
			{
				return !double.IsInfinity(d);
			}
			return false;
		}

		private static bool IsValidPrecision(object value)
		{
			return (int)value >= 0;
		}

		private static bool IsValidMinRange(object value)
		{
			if (value is double && IsValidDouble((double)value))
			{
				return (double)value >= 0.0;
			}
			return false;
		}

		private static object CoerceMinimum(DependencyObject d, object basevalue)
		{
			RangeSlider rangeSlider = (RangeSlider)d;
			if ((double)basevalue > rangeSlider.Maximum)
			{
				return rangeSlider.Maximum;
			}
			return basevalue;
		}

		private static object CoerceMaximum(DependencyObject d, object basevalue)
		{
			RangeSlider rangeSlider = (RangeSlider)d;
			if ((double)basevalue < rangeSlider.Minimum)
			{
				return rangeSlider.Minimum;
			}
			return basevalue;
		}

		private static object CoerceLowerValue(DependencyObject d, object basevalue)
		{
			RangeSlider rangeSlider = (RangeSlider)d;
			double num = (double)basevalue;
			if (num < rangeSlider.Minimum || rangeSlider.UpperValue - rangeSlider.MinRange < rangeSlider.Minimum)
			{
				return rangeSlider.Minimum;
			}
			if (num > rangeSlider.UpperValue - rangeSlider.MinRange)
			{
				return rangeSlider.UpperValue - rangeSlider.MinRange;
			}
			return basevalue;
		}

		private static object CoerceUpperValue(DependencyObject d, object basevalue)
		{
			RangeSlider rangeSlider = (RangeSlider)d;
			double num = (double)basevalue;
			if (num > rangeSlider.Maximum || rangeSlider.LowerValue + rangeSlider.MinRange > rangeSlider.Maximum)
			{
				return rangeSlider.Maximum;
			}
			if (num < rangeSlider.LowerValue + rangeSlider.MinRange)
			{
				return rangeSlider.LowerValue + rangeSlider.MinRange;
			}
			return basevalue;
		}

		private static object CoerceMinRange(DependencyObject d, object basevalue)
		{
			RangeSlider rangeSlider = (RangeSlider)d;
			double num = (double)basevalue;
			if (rangeSlider.LowerValue + num > rangeSlider.Maximum)
			{
				return rangeSlider.Maximum - rangeSlider.LowerValue;
			}
			return basevalue;
		}

		private static object CoerceMinRangeWidth(DependencyObject d, object basevalue)
		{
			RangeSlider rangeSlider = (RangeSlider)d;
			if (rangeSlider._leftThumb != null && rangeSlider._rightThumb != null)
			{
				double num = (rangeSlider.Orientation != 0) ? (rangeSlider.ActualHeight - rangeSlider._leftThumb.ActualHeight - rangeSlider._rightThumb.ActualHeight) : (rangeSlider.ActualWidth - rangeSlider._leftThumb.ActualWidth - rangeSlider._rightThumb.ActualWidth);
				return ((double)basevalue > num / 2.0) ? (num / 2.0) : ((double)basevalue);
			}
			return basevalue;
		}

		private static void MaxPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			dependencyObject.CoerceValue(RangeBase.MaximumProperty);
			dependencyObject.CoerceValue(RangeBase.MinimumProperty);
			dependencyObject.CoerceValue(UpperValueProperty);
		}

		private static void MinPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			dependencyObject.CoerceValue(RangeBase.MinimumProperty);
			dependencyObject.CoerceValue(RangeBase.MaximumProperty);
			dependencyObject.CoerceValue(LowerValueProperty);
		}

		private static void RangesChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			RangeSlider rangeSlider = (RangeSlider)dependencyObject;
			if (!rangeSlider._internalUpdate)
			{
				dependencyObject.CoerceValue(UpperValueProperty);
				dependencyObject.CoerceValue(LowerValueProperty);
				RaiseValueChangedEvents(dependencyObject);
				rangeSlider._oldLower = rangeSlider.LowerValue;
				rangeSlider._oldUpper = rangeSlider.UpperValue;
				rangeSlider.ReCalculateSize();
			}
		}

		private static void MinRangeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			double num = (double)e.NewValue;
			if (num < 0.0)
			{
				num = 0.0;
			}
			RangeSlider rangeSlider = (RangeSlider)dependencyObject;
			dependencyObject.CoerceValue(MinRangeProperty);
			rangeSlider._internalUpdate = true;
			rangeSlider.UpperValue = Math.Max(rangeSlider.UpperValue, rangeSlider.LowerValue + num);
			rangeSlider.UpperValue = Math.Min(rangeSlider.UpperValue, rangeSlider.Maximum);
			rangeSlider._internalUpdate = false;
			rangeSlider.CoerceValue(UpperValueProperty);
			RaiseValueChangedEvents(dependencyObject);
			rangeSlider._oldLower = rangeSlider.LowerValue;
			rangeSlider._oldUpper = rangeSlider.UpperValue;
			rangeSlider.ReCalculateSize();
		}

		private static void MinRangeWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			((RangeSlider)sender).ReCalculateSize();
		}

		private static void IntervalChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			((RangeSlider)dependencyObject)._timer.Interval = TimeSpan.FromMilliseconds((double)(int)e.NewValue);
		}

		private static void RaiseValueChangedEvents(DependencyObject dependencyObject, bool lowerValueReCalculated = true, bool upperValueReCalculated = true)
		{
			RangeSlider rangeSlider = (RangeSlider)dependencyObject;
			bool flag = object.Equals(rangeSlider._oldLower, rangeSlider.LowerValue);
			bool flag2 = object.Equals(rangeSlider._oldUpper, rangeSlider.UpperValue);
			if ((lowerValueReCalculated | upperValueReCalculated) && (!flag || !flag2))
			{
				rangeSlider.OnRangeSelectionChanged(new RangeSelectionChangedEventArgs(rangeSlider.LowerValue, rangeSlider.UpperValue, rangeSlider._oldLower, rangeSlider._oldUpper));
			}
			if (lowerValueReCalculated && !flag)
			{
				rangeSlider.OnRangeParameterChanged(new RangeParameterChangedEventArgs(RangeParameterChangeType.Lower, rangeSlider._oldLower, rangeSlider.LowerValue), LowerValueChangedEvent);
			}
			if (upperValueReCalculated && !flag2)
			{
				rangeSlider.OnRangeParameterChanged(new RangeParameterChangedEventArgs(RangeParameterChangeType.Upper, rangeSlider._oldUpper, rangeSlider.UpperValue), UpperValueChangedEvent);
			}
		}
	}
}
