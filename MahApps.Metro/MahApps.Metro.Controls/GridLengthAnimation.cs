using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	public class GridLengthAnimation : AnimationTimeline
	{
		public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(GridLength), typeof(GridLengthAnimation));

		public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(GridLength), typeof(GridLengthAnimation));

		public GridLength From
		{
			get
			{
				return (GridLength)GetValue(FromProperty);
			}
			set
			{
				SetValue(FromProperty, value);
			}
		}

		public GridLength To
		{
			get
			{
				return (GridLength)GetValue(ToProperty);
			}
			set
			{
				SetValue(ToProperty, value);
			}
		}

		public override Type TargetPropertyType => typeof(GridLength);

		public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
		{
			GridLength gridLength = (GridLength)GetValue(FromProperty);
			GridLength gridLength2 = (GridLength)GetValue(ToProperty);
			if (gridLength.GridUnitType != gridLength2.GridUnitType)
			{
				return gridLength2;
			}
			double value = gridLength.Value;
			double value2 = gridLength2.Value;
			if (value > value2)
			{
				return new GridLength((1.0 - animationClock.CurrentProgress.Value) * (value - value2) + value2, GridUnitType.Star);
			}
			return new GridLength(animationClock.CurrentProgress.Value * (value2 - value) + value, GridUnitType.Star);
		}

		protected override Freezable CreateInstanceCore()
		{
			return new GridLengthAnimation();
		}
	}
}
