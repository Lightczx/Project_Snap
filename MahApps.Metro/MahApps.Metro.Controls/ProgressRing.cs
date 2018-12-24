using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	[TemplateVisualState(Name = "Large", GroupName = "SizeStates")]
	[TemplateVisualState(Name = "Small", GroupName = "SizeStates")]
	[TemplateVisualState(Name = "Inactive", GroupName = "ActiveStates")]
	[TemplateVisualState(Name = "Active", GroupName = "ActiveStates")]
	public class ProgressRing : Control
	{
		public static readonly DependencyProperty BindableWidthProperty;

		public static readonly DependencyProperty IsActiveProperty;

		public static readonly DependencyProperty IsLargeProperty;

		public static readonly DependencyProperty MaxSideLengthProperty;

		public static readonly DependencyProperty EllipseDiameterProperty;

		public static readonly DependencyProperty EllipseOffsetProperty;

		public static readonly DependencyProperty EllipseDiameterScaleProperty;

		private List<Action> _deferredActions = new List<Action>();

		public double MaxSideLength
		{
			get
			{
				return (double)GetValue(MaxSideLengthProperty);
			}
			private set
			{
				SetValue(MaxSideLengthProperty, value);
			}
		}

		public double EllipseDiameter
		{
			get
			{
				return (double)GetValue(EllipseDiameterProperty);
			}
			private set
			{
				SetValue(EllipseDiameterProperty, value);
			}
		}

		public double EllipseDiameterScale
		{
			get
			{
				return (double)GetValue(EllipseDiameterScaleProperty);
			}
			set
			{
				SetValue(EllipseDiameterScaleProperty, value);
			}
		}

		public Thickness EllipseOffset
		{
			get
			{
				return (Thickness)GetValue(EllipseOffsetProperty);
			}
			private set
			{
				SetValue(EllipseOffsetProperty, value);
			}
		}

		public double BindableWidth
		{
			get
			{
				return (double)GetValue(BindableWidthProperty);
			}
			private set
			{
				SetValue(BindableWidthProperty, value);
			}
		}

		public bool IsActive
		{
			get
			{
				return (bool)GetValue(IsActiveProperty);
			}
			set
			{
				SetValue(IsActiveProperty, value);
			}
		}

		public bool IsLarge
		{
			get
			{
				return (bool)GetValue(IsLargeProperty);
			}
			set
			{
				SetValue(IsLargeProperty, value);
			}
		}

		static ProgressRing()
		{
			BindableWidthProperty = DependencyProperty.Register("BindableWidth", typeof(double), typeof(ProgressRing), new PropertyMetadata(0.0, BindableWidthCallback));
			IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true, IsActiveChanged));
			IsLargeProperty = DependencyProperty.Register("IsLarge", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true, IsLargeChangedCallback));
			MaxSideLengthProperty = DependencyProperty.Register("MaxSideLength", typeof(double), typeof(ProgressRing), new PropertyMetadata(0.0));
			EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(ProgressRing), new PropertyMetadata(0.0));
			EllipseOffsetProperty = DependencyProperty.Register("EllipseOffset", typeof(Thickness), typeof(ProgressRing), new PropertyMetadata(default(Thickness)));
			EllipseDiameterScaleProperty = DependencyProperty.Register("EllipseDiameterScale", typeof(double), typeof(ProgressRing), new PropertyMetadata(1.0));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(typeof(ProgressRing)));
			UIElement.VisibilityProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(delegate(DependencyObject ringObject, DependencyPropertyChangedEventArgs e)
			{
				if (e.NewValue != e.OldValue)
				{
					ProgressRing progressRing = (ProgressRing)ringObject;
					if ((Visibility)e.NewValue != 0)
					{
						progressRing.SetCurrentValue(IsActiveProperty, false);
					}
					else
					{
						progressRing.SetCurrentValue(IsActiveProperty, true);
					}
				}
			}));
		}

		public ProgressRing()
		{
			base.SizeChanged += OnSizeChanged;
		}

		private static void BindableWidthCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			ProgressRing ring = dependencyObject as ProgressRing;
			if (ring != null)
			{
				Action action = delegate
				{
					ring.SetEllipseDiameter((double)dependencyPropertyChangedEventArgs.NewValue);
					ring.SetEllipseOffset((double)dependencyPropertyChangedEventArgs.NewValue);
					ring.SetMaxSideLength((double)dependencyPropertyChangedEventArgs.NewValue);
				};
				if (ring._deferredActions != null)
				{
					ring._deferredActions.Add(action);
				}
				else
				{
					action();
				}
			}
		}

		private void SetMaxSideLength(double width)
		{
			SetCurrentValue(MaxSideLengthProperty, (width <= 20.0) ? 20.0 : width);
		}

		private void SetEllipseDiameter(double width)
		{
			SetCurrentValue(EllipseDiameterProperty, width / 8.0 * EllipseDiameterScale);
		}

		private void SetEllipseOffset(double width)
		{
			SetCurrentValue(EllipseOffsetProperty, new Thickness(0.0, width / 2.0, 0.0, 0.0));
		}

		private static void IsLargeChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			(dependencyObject as ProgressRing)?.UpdateLargeState();
		}

		private void UpdateLargeState()
		{
			Action action = (!IsLarge) ? ((Action)delegate
			{
				VisualStateManager.GoToState(this, "Small", useTransitions: true);
			}) : ((Action)delegate
			{
				VisualStateManager.GoToState(this, "Large", useTransitions: true);
			});
			if (_deferredActions != null)
			{
				_deferredActions.Add(action);
			}
			else
			{
				action();
			}
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			SetCurrentValue(BindableWidthProperty, base.ActualWidth);
		}

		private static void IsActiveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			(dependencyObject as ProgressRing)?.UpdateActiveState();
		}

		private void UpdateActiveState()
		{
			Action action = (!IsActive) ? ((Action)delegate
			{
				VisualStateManager.GoToState(this, "Inactive", useTransitions: true);
			}) : ((Action)delegate
			{
				VisualStateManager.GoToState(this, "Active", useTransitions: true);
			});
			if (_deferredActions != null)
			{
				_deferredActions.Add(action);
			}
			else
			{
				action();
			}
		}

		public override void OnApplyTemplate()
		{
			UpdateLargeState();
			UpdateActiveState();
			base.OnApplyTemplate();
			if (_deferredActions != null)
			{
				foreach (Action deferredAction in _deferredActions)
				{
					deferredAction();
				}
			}
			_deferredActions = null;
		}
	}
}
