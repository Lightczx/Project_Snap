using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public static class DataGridRowHelper
	{
		public static readonly DependencyProperty SelectionUnitProperty = DependencyProperty.RegisterAttached("SelectionUnit", typeof(DataGridSelectionUnit), typeof(DataGridRowHelper), new FrameworkPropertyMetadata(DataGridSelectionUnit.FullRow));

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(DataGridRow))]
		public static DataGridSelectionUnit GetSelectionUnit(UIElement element)
		{
			return (DataGridSelectionUnit)element.GetValue(SelectionUnitProperty);
		}

		public static void SetSelectionUnit(UIElement element, DataGridSelectionUnit value)
		{
			element.SetValue(SelectionUnitProperty, value);
		}
	}
}
