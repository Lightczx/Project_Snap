using ControlzEx;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	[TemplatePart(Name = "PART_BackgroundTranslate", Type = typeof(TranslateTransform))]
	[TemplatePart(Name = "PART_DraggingThumb", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_SwitchTrack", Type = typeof(Grid))]
	[TemplatePart(Name = "PART_ThumbIndicator", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_ThumbTranslate", Type = typeof(TranslateTransform))]
	public class ToggleSwitchButton : ToggleButton
	{
		private const string PART_BackgroundTranslate = "PART_BackgroundTranslate";

		private const string PART_DraggingThumb = "PART_DraggingThumb";

		private const string PART_SwitchTrack = "PART_SwitchTrack";

		private const string PART_ThumbIndicator = "PART_ThumbIndicator";

		private const string PART_ThumbTranslate = "PART_ThumbTranslate";

		private TranslateTransform _BackgroundTranslate;

		private Thumb _DraggingThumb;

		private Grid _SwitchTrack;

		private FrameworkElement _ThumbIndicator;

		private TranslateTransform _ThumbTranslate;

		private readonly PropertyChangeNotifier isCheckedPropertyChangeNotifier;

		[Obsolete("This property will be deleted in the next release. You should use OnSwitchBrush and OffSwitchBrush to change the switch's brushes.")]
		public static readonly DependencyProperty SwitchForegroundProperty;

		public static readonly DependencyProperty OnSwitchBrushProperty;

		public static readonly DependencyProperty OffSwitchBrushProperty;

		public static readonly DependencyProperty ThumbIndicatorBrushProperty;

		public static readonly DependencyProperty ThumbIndicatorDisabledBrushProperty;

		public static readonly DependencyProperty ThumbIndicatorWidthProperty;

		private DoubleAnimation _thumbAnimation;

		private double? _lastDragPosition;

		private bool _isDragging;

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

		static ToggleSwitchButton()
		{
			SwitchForegroundProperty = DependencyProperty.Register("SwitchForeground", typeof(Brush), typeof(ToggleSwitchButton), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs e)
			{
				((ToggleSwitchButton)o).SetCurrentValue(OnSwitchBrushProperty, e.NewValue as Brush);
			}));
			OnSwitchBrushProperty = DependencyProperty.Register("OnSwitchBrush", typeof(Brush), typeof(ToggleSwitchButton), null);
			OffSwitchBrushProperty = DependencyProperty.Register("OffSwitchBrush", typeof(Brush), typeof(ToggleSwitchButton), null);
			ThumbIndicatorBrushProperty = DependencyProperty.Register("ThumbIndicatorBrush", typeof(Brush), typeof(ToggleSwitchButton), null);
			ThumbIndicatorDisabledBrushProperty = DependencyProperty.Register("ThumbIndicatorDisabledBrush", typeof(Brush), typeof(ToggleSwitchButton), null);
			ThumbIndicatorWidthProperty = DependencyProperty.Register("ThumbIndicatorWidth", typeof(double), typeof(ToggleSwitchButton), new PropertyMetadata(13.0));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitchButton), new FrameworkPropertyMetadata(typeof(ToggleSwitchButton)));
		}

		public ToggleSwitchButton()
		{
			isCheckedPropertyChangeNotifier = new PropertyChangeNotifier(this, ToggleButton.IsCheckedProperty);
			isCheckedPropertyChangeNotifier.ValueChanged += IsCheckedPropertyChangeNotifierValueChanged;
		}

		private void IsCheckedPropertyChangeNotifierValueChanged(object sender, EventArgs e)
		{
			UpdateThumb();
		}

		private void UpdateThumb()
		{
			if (_ThumbTranslate != null && _SwitchTrack != null && _ThumbIndicator != null)
			{
				double destination = base.IsChecked.GetValueOrDefault() ? (base.ActualWidth - (_SwitchTrack.Margin.Left + _SwitchTrack.Margin.Right + _ThumbIndicator.ActualWidth + _ThumbIndicator.Margin.Left + _ThumbIndicator.Margin.Right)) : 0.0;
				_thumbAnimation = new DoubleAnimation();
				_thumbAnimation.To = destination;
				_thumbAnimation.Duration = TimeSpan.FromMilliseconds(500.0);
				_thumbAnimation.EasingFunction = new ExponentialEase
				{
					Exponent = 9.0
				};
				_thumbAnimation.FillBehavior = FillBehavior.Stop;
				AnimationTimeline currentAnimation = _thumbAnimation;
				_thumbAnimation.Completed += delegate
				{
					if (_thumbAnimation != null && currentAnimation == _thumbAnimation)
					{
						_ThumbTranslate.X = destination;
						_thumbAnimation = null;
					}
				};
				_ThumbTranslate.BeginAnimation(TranslateTransform.XProperty, _thumbAnimation);
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_BackgroundTranslate = (GetTemplateChild("PART_BackgroundTranslate") as TranslateTransform);
			_DraggingThumb = (GetTemplateChild("PART_DraggingThumb") as Thumb);
			_SwitchTrack = (GetTemplateChild("PART_SwitchTrack") as Grid);
			_ThumbIndicator = (GetTemplateChild("PART_ThumbIndicator") as FrameworkElement);
			_ThumbTranslate = (GetTemplateChild("PART_ThumbTranslate") as TranslateTransform);
			if (_ThumbIndicator != null && _ThumbTranslate != null && _BackgroundTranslate != null)
			{
				Binding binding = new Binding("X");
				binding.Source = _ThumbTranslate;
				BindingOperations.SetBinding(_BackgroundTranslate, TranslateTransform.XProperty, binding);
			}
			if (_DraggingThumb != null && _ThumbIndicator != null && _ThumbTranslate != null)
			{
				_DraggingThumb.DragStarted -= _DraggingThumb_DragStarted;
				_DraggingThumb.DragDelta -= _DraggingThumb_DragDelta;
				_DraggingThumb.DragCompleted -= _DraggingThumb_DragCompleted;
				_DraggingThumb.DragStarted += _DraggingThumb_DragStarted;
				_DraggingThumb.DragDelta += _DraggingThumb_DragDelta;
				_DraggingThumb.DragCompleted += _DraggingThumb_DragCompleted;
				if (_SwitchTrack != null)
				{
					_SwitchTrack.SizeChanged -= _SwitchTrack_SizeChanged;
					_SwitchTrack.SizeChanged += _SwitchTrack_SizeChanged;
				}
			}
		}

		private void SetIsPressed(bool pressed)
		{
			typeof(ToggleButton).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this, new object[1]
			{
				pressed
			});
		}

		private void _DraggingThumb_DragStarted(object sender, DragStartedEventArgs e)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed && !base.IsPressed)
			{
				SetIsPressed(pressed: true);
			}
			if (_ThumbTranslate != null)
			{
				_ThumbTranslate.BeginAnimation(TranslateTransform.XProperty, null);
				double x = base.IsChecked.GetValueOrDefault() ? (base.ActualWidth - (_SwitchTrack.Margin.Left + _SwitchTrack.Margin.Right + _ThumbIndicator.ActualWidth + _ThumbIndicator.Margin.Left + _ThumbIndicator.Margin.Right)) : 0.0;
				_ThumbTranslate.X = x;
				_thumbAnimation = null;
			}
			_lastDragPosition = _ThumbTranslate.X;
			_isDragging = false;
		}

		private void _DraggingThumb_DragDelta(object sender, DragDeltaEventArgs e)
		{
			if (_lastDragPosition.HasValue)
			{
				if (Math.Abs(e.HorizontalChange) > 3.0)
				{
					_isDragging = true;
				}
				if (_SwitchTrack != null && _ThumbIndicator != null)
				{
					double value = _lastDragPosition.Value;
					_ThumbTranslate.X = Math.Min(base.ActualWidth - (_SwitchTrack.Margin.Left + _SwitchTrack.Margin.Right + _ThumbIndicator.ActualWidth + _ThumbIndicator.Margin.Left + _ThumbIndicator.Margin.Right), Math.Max(0.0, value + e.HorizontalChange));
				}
			}
		}

		private void _DraggingThumb_DragCompleted(object sender, DragCompletedEventArgs e)
		{
			SetIsPressed(pressed: false);
			_lastDragPosition = null;
			if (!_isDragging)
			{
				OnClick();
			}
			else if (_ThumbTranslate != null && _SwitchTrack != null)
			{
				if (!base.IsChecked.GetValueOrDefault() && _ThumbTranslate.X + 6.5 >= _SwitchTrack.ActualWidth / 2.0)
				{
					OnClick();
				}
				else if (base.IsChecked.GetValueOrDefault() && _ThumbTranslate.X + 6.5 <= _SwitchTrack.ActualWidth / 2.0)
				{
					OnClick();
				}
				else
				{
					UpdateThumb();
				}
			}
		}

		private void _SwitchTrack_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (_ThumbTranslate != null && _SwitchTrack != null && _ThumbIndicator != null)
			{
				double x = base.IsChecked.GetValueOrDefault() ? (base.ActualWidth - (_SwitchTrack.Margin.Left + _SwitchTrack.Margin.Right + _ThumbIndicator.ActualWidth + _ThumbIndicator.Margin.Left + _ThumbIndicator.Margin.Right)) : 0.0;
				_ThumbTranslate.X = x;
			}
		}
	}
}
