using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class LayoutInvalidationCatcher : Decorator
	{
		public Planerator PlaParent => base.Parent as Planerator;

		protected override Size MeasureOverride(Size constraint)
		{
			PlaParent?.InvalidateMeasure();
			return base.MeasureOverride(constraint);
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			PlaParent?.InvalidateArrange();
			return base.ArrangeOverride(arrangeSize);
		}
	}
}
