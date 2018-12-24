using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MahApps.Metro.Controls
{
	public class DataGridCellHelper
	{
		[Obsolete("This property will be deleted in the next release.")]
		public static readonly DependencyProperty SaveDataGridProperty = DependencyProperty.RegisterAttached("SaveDataGrid", typeof(bool), typeof(DataGridCellHelper), new FrameworkPropertyMetadata(false, CellPropertyChangedCallback));

		[Obsolete("This property will be deleted in the next release.")]
		public static readonly DependencyProperty DataGridProperty = DependencyProperty.RegisterAttached("DataGrid", typeof(DataGrid), typeof(DataGridCellHelper), new FrameworkPropertyMetadata((object)null));

		public static readonly DependencyProperty SelectionUnitProperty = DependencyProperty.RegisterAttached("SelectionUnit", typeof(DataGridSelectionUnit), typeof(DataGridCellHelper), new FrameworkPropertyMetadata(DataGridSelectionUnit.Cell, SelectionUnitOnPropertyChangedCallback));

		public static readonly DependencyProperty IsCellOrRowHeaderProperty = DependencyProperty.RegisterAttached("IsCellOrRowHeader", typeof(bool), typeof(DataGridCellHelper), new FrameworkPropertyMetadata(true));

		private static void CellPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			DataGridCell dataGridCell = dependencyObject as DataGridCell;
			if (dataGridCell != null && e.NewValue != e.OldValue && e.NewValue is bool)
			{
				dataGridCell.Loaded -= DataGridCellLoaded;
				dataGridCell.Unloaded -= DataGridCellUnloaded;
				DataGrid value = null;
				if ((bool)e.NewValue)
				{
					value = dataGridCell.TryFindParent<DataGrid>();
					dataGridCell.Loaded += DataGridCellLoaded;
					dataGridCell.Unloaded += DataGridCellUnloaded;
				}
				SetDataGrid(dataGridCell, value);
			}
		}

		private static void DataGridCellLoaded(object sender, RoutedEventArgs e)
		{
			DataGridCell dataGridCell = (DataGridCell)sender;
			if (GetDataGrid(dataGridCell) == null)
			{
				DataGrid value = dataGridCell.TryFindParent<DataGrid>();
				SetDataGrid(dataGridCell, value);
			}
		}

		private static void DataGridCellUnloaded(object sender, RoutedEventArgs e)
		{
			SetDataGrid((DataGridCell)sender, null);
		}

		[AttachedPropertyBrowsableForType(typeof(DataGridCell))]
		[Obsolete("This property will be deleted in the next release.")]
		public static bool GetSaveDataGrid(UIElement element)
		{
			return (bool)element.GetValue(SaveDataGridProperty);
		}

		[Obsolete("This property will be deleted in the next release.")]
		public static void SetSaveDataGrid(UIElement element, bool value)
		{
			element.SetValue(SaveDataGridProperty, value);
		}

		[AttachedPropertyBrowsableForType(typeof(DataGridCell))]
		[Obsolete("This property will be deleted in the next release.")]
		public static DataGrid GetDataGrid(UIElement element)
		{
			return (DataGrid)element.GetValue(DataGridProperty);
		}

		[Obsolete("This property will be deleted in the next release.")]
		public static void SetDataGrid(UIElement element, DataGrid value)
		{
			element.SetValue(DataGridProperty, value);
		}

		private static void SelectionUnitOnPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			if (args.OldValue != args.NewValue)
			{
				SetIsCellOrRowHeader((DataGridCell)dependencyObject, !object.Equals(args.NewValue, DataGridSelectionUnit.FullRow));
			}
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(DataGridCell))]
		public static DataGridSelectionUnit GetSelectionUnit(UIElement element)
		{
			return (DataGridSelectionUnit)element.GetValue(SelectionUnitProperty);
		}

		public static void SetSelectionUnit(UIElement element, DataGridSelectionUnit value)
		{
			element.SetValue(SelectionUnitProperty, value);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(DataGridCell))]
		public static bool GetIsCellOrRowHeader(UIElement element)
		{
			return (bool)element.GetValue(IsCellOrRowHeaderProperty);
		}

		internal static void SetIsCellOrRowHeader(UIElement element, bool value)
		{
			element.SetValue(IsCellOrRowHeaderProperty, value);
		}
	}
}
