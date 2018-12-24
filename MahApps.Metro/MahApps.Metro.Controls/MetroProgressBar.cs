using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	public class MetroProgressBar : ProgressBar
	{
		public static readonly DependencyProperty EllipseDiameterProperty;

		public static readonly DependencyProperty EllipseOffsetProperty;

		private readonly object lockme = new object();

		private Storyboard indeterminateStoryboard;

		public double EllipseDiameter
		{
			get
			{
				return (double)GetValue(EllipseDiameterProperty);
			}
			set
			{
				SetValue(EllipseDiameterProperty, value);
			}
		}

		public double EllipseOffset
		{
			get
			{
				return (double)GetValue(EllipseOffsetProperty);
			}
			set
			{
				SetValue(EllipseOffsetProperty, value);
			}
		}

		static MetroProgressBar()
		{
			EllipseDiameterProperty = DependencyProperty.Register("EllipseDiameter", typeof(double), typeof(MetroProgressBar), new PropertyMetadata(0.0));
			EllipseOffsetProperty = DependencyProperty.Register("EllipseOffset", typeof(double), typeof(MetroProgressBar), new PropertyMetadata(0.0));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroProgressBar), new FrameworkPropertyMetadata(typeof(MetroProgressBar)));
			ProgressBar.IsIndeterminateProperty.OverrideMetadata(typeof(MetroProgressBar), new FrameworkPropertyMetadata(OnIsIndeterminateChanged));
		}

		public MetroProgressBar()
		{
			base.IsVisibleChanged += VisibleChangedHandler;
		}

		private void VisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (base.IsIndeterminate)
			{
				ToggleIndeterminate(this, (bool)e.OldValue, (bool)e.NewValue);
			}
		}

		private static void OnIsIndeterminateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			MetroProgressBar metroProgressBar = (MetroProgressBar)dependencyObject;
			if (metroProgressBar.IsLoaded && metroProgressBar.IsVisible)
			{
				ToggleIndeterminate(metroProgressBar, (bool)e.OldValue, (bool)e.NewValue);
			}
		}

		private static void ToggleIndeterminate(MetroProgressBar bar, bool oldValue, bool newValue)
		{
			if (newValue != oldValue)
			{
				VisualState indeterminateState = bar.GetIndeterminate();
				FrameworkElement containingObject = bar.GetTemplateChild("ContainingGrid") as FrameworkElement;
				if (indeterminateState != null && containingObject != null)
				{
					Action invokeAction = delegate
					{
						if (oldValue && indeterminateState.Storyboard != null)
						{
							indeterminateState.Storyboard.Stop(containingObject);
							indeterminateState.Storyboard.Remove(containingObject);
						}
						if (newValue)
						{
							bar.ResetStoryboard(bar.ActualSize(invalidateMeasureArrange: true), removeOldStoryboard: false);
						}
					};
					bar.Invoke(invokeAction);
				}
			}
		}

		private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
		{
			double width = ActualSize(invalidateMeasureArrange: false);
			if (base.Visibility == Visibility.Visible && base.IsIndeterminate)
			{
				ResetStoryboard(width, removeOldStoryboard: true);
			}
		}

		private double ActualSize(bool invalidateMeasureArrange)
		{
			if (invalidateMeasureArrange)
			{
				UpdateLayout();
				Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
				InvalidateArrange();
			}
			if (base.Orientation != 0)
			{
				return base.ActualHeight;
			}
			return base.ActualWidth;
		}

		private void ResetStoryboard(double width, bool removeOldStoryboard)
		{
			if (base.IsIndeterminate)
			{
				lock (lockme)
				{
					double num = CalcContainerAnimStart(width);
					double num2 = CalcContainerAnimEnd(width);
					double value = CalcEllipseAnimWell(width);
					double value2 = CalcEllipseAnimEnd(width);
					try
					{
						VisualState indeterminate = GetIndeterminate();
						if (indeterminate != null && indeterminateStoryboard != null)
						{
							Storyboard storyboard = indeterminateStoryboard.Clone();
							Timeline timeline = storyboard.Children.First((Timeline t) => t.Name == "MainDoubleAnim");
							timeline.SetValue(DoubleAnimation.FromProperty, num);
							timeline.SetValue(DoubleAnimation.ToProperty, num2);
							string[] array = new string[5]
							{
								"E1",
								"E2",
								"E3",
								"E4",
								"E5"
							};
							foreach (string elemName in array)
							{
								DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = (DoubleAnimationUsingKeyFrames)storyboard.Children.First((Timeline t) => t.Name == elemName + "Anim");
								DoubleKeyFrame doubleKeyFrame;
								DoubleKeyFrame doubleKeyFrame2;
								DoubleKeyFrame doubleKeyFrame3;
								if (elemName == "E1")
								{
									doubleKeyFrame = doubleAnimationUsingKeyFrames.KeyFrames[1];
									doubleKeyFrame2 = doubleAnimationUsingKeyFrames.KeyFrames[2];
									doubleKeyFrame3 = doubleAnimationUsingKeyFrames.KeyFrames[3];
								}
								else
								{
									doubleKeyFrame = doubleAnimationUsingKeyFrames.KeyFrames[2];
									doubleKeyFrame2 = doubleAnimationUsingKeyFrames.KeyFrames[3];
									doubleKeyFrame3 = doubleAnimationUsingKeyFrames.KeyFrames[4];
								}
								doubleKeyFrame.Value = value;
								doubleKeyFrame2.Value = value;
								doubleKeyFrame3.Value = value2;
								doubleKeyFrame.InvalidateProperty(DoubleKeyFrame.ValueProperty);
								doubleKeyFrame2.InvalidateProperty(DoubleKeyFrame.ValueProperty);
								doubleKeyFrame3.InvalidateProperty(DoubleKeyFrame.ValueProperty);
								doubleAnimationUsingKeyFrames.InvalidateProperty(Storyboard.TargetPropertyProperty);
								doubleAnimationUsingKeyFrames.InvalidateProperty(Storyboard.TargetNameProperty);
							}
							FrameworkElement containingObject = (FrameworkElement)GetTemplateChild("ContainingGrid");
							if (removeOldStoryboard && indeterminate.Storyboard != null)
							{
								indeterminate.Storyboard.Stop(containingObject);
								indeterminate.Storyboard.Remove(containingObject);
							}
							indeterminate.Storyboard = storyboard;
							indeterminate.Storyboard?.Begin(containingObject, isControllable: true);
						}
					}
					catch (Exception)
					{
					}
				}
			}
		}

		private VisualState GetIndeterminate()
		{
			FrameworkElement frameworkElement = GetTemplateChild("ContainingGrid") as FrameworkElement;
			if (frameworkElement == null)
			{
				ApplyTemplate();
				frameworkElement = (GetTemplateChild("ContainingGrid") as FrameworkElement);
				if (frameworkElement == null)
				{
					return null;
				}
			}
			return VisualStateManager.GetVisualStateGroups(frameworkElement)?.OfType<VisualStateGroup>().SelectMany((VisualStateGroup group) => group.States.OfType<VisualState>()).FirstOrDefault((VisualState state) => state.Name == "Indeterminate");
		}

		private void SetEllipseDiameter(double width)
		{
			SetCurrentValue(EllipseDiameterProperty, (width <= 180.0) ? 4.0 : ((width <= 280.0) ? 5.0 : 6.0));
		}

		private void SetEllipseOffset(double width)
		{
			SetCurrentValue(EllipseOffsetProperty, (width <= 180.0) ? 4.0 : ((width <= 280.0) ? 7.0 : 9.0));
		}

		private double CalcContainerAnimStart(double width)
		{
			if (!(width <= 180.0))
			{
				if (!(width <= 280.0))
				{
					return -63.0;
				}
				return -50.5;
			}
			return -34.0;
		}

		private double CalcContainerAnimEnd(double width)
		{
			double num = 0.4352 * width;
			if (!(width <= 180.0))
			{
				if (!(width <= 280.0))
				{
					return num + 58.862;
				}
				return num + 27.84;
			}
			return num - 25.731;
		}

		private double CalcEllipseAnimWell(double width)
		{
			return width * 1.0 / 3.0;
		}

		private double CalcEllipseAnimEnd(double width)
		{
			return width * 2.0 / 3.0;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			lock (lockme)
			{
				indeterminateStoryboard = (TryFindResource("IndeterminateStoryboard") as Storyboard);
			}
			base.Loaded -= LoadedHandler;
			base.Loaded += LoadedHandler;
		}

		private void LoadedHandler(object sender, RoutedEventArgs routedEventArgs)
		{
			base.Loaded -= LoadedHandler;
			SizeChangedHandler(null, null);
			base.SizeChanged += SizeChangedHandler;
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			UpdateEllipseProperties();
		}

		private void UpdateEllipseProperties()
		{
			double num = ActualSize(invalidateMeasureArrange: true);
			if (num > 0.0)
			{
				if (EllipseDiameter.Equals(0.0))
				{
					SetEllipseDiameter(num);
				}
				if (EllipseOffset.Equals(0.0))
				{
					SetEllipseOffset(num);
				}
			}
		}
	}
}
