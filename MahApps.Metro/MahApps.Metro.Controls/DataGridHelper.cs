using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MahApps.Metro.Controls
{
	public static class DataGridHelper
	{
		private static DataGrid _suppressComboAutoDropDown;

		public static readonly DependencyProperty EnableCellEditAssistProperty = DependencyProperty.RegisterAttached("EnableCellEditAssist", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(false, EnableCellEditAssistPropertyChangedCallback));

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(DataGrid))]
		public static bool GetEnableCellEditAssist(DependencyObject element)
		{
			return (bool)element.GetValue(EnableCellEditAssistProperty);
		}

		[Category("MahApps.Metro")]
		[AttachedPropertyBrowsableForType(typeof(DataGrid))]
		public static void SetEnableCellEditAssist(DependencyObject element, bool value)
		{
			element.SetValue(EnableCellEditAssistProperty, value);
		}

		private static void EnableCellEditAssistPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = d as DataGrid;
			if (dataGrid != null)
			{
				dataGrid.PreviewMouseLeftButtonDown -= DataGridOnPreviewMouseLeftButtonDown;
				if ((bool)e.NewValue)
				{
					dataGrid.PreviewMouseLeftButtonDown += DataGridOnPreviewMouseLeftButtonDown;
				}
			}
		}

		private static void DataGridOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)sender;
			DependencyObject dependencyObject = dataGrid.InputHitTest(e.GetPosition((DataGrid)sender)) as DependencyObject;
			DataGridCell dataGridCell;
			while (true)
			{
				if (dependencyObject == null)
				{
					return;
				}
				dataGridCell = (dependencyObject as DataGridCell);
				if (dataGridCell != null && dataGrid.Equals(dataGridCell.TryFindParent<DataGrid>()))
				{
					break;
				}
				dependencyObject = ((dependencyObject is Visual || dependencyObject is Visual3D) ? VisualTreeHelper.GetParent(dependencyObject) : null);
			}
			if (!dataGridCell.IsReadOnly)
			{
				ComboBox control2;
				if (IsDirectHitOnEditComponent(dataGridCell, e, out ToggleButton control))
				{
					dataGrid.CurrentCell = new DataGridCellInfo(dataGridCell);
					dataGrid.BeginEdit();
					control.SetCurrentValue(ToggleButton.IsCheckedProperty, !control.IsChecked);
					dataGrid.CommitEdit();
					e.Handled = true;
				}
				else if (IsDirectHitOnEditComponent(dataGridCell, e, out control2) && _suppressComboAutoDropDown == null)
				{
					dataGrid.CurrentCell = new DataGridCellInfo(dataGridCell);
					dataGrid.BeginEdit();
					if (IsDirectHitOnEditComponent(dataGridCell, e, out control2))
					{
						_suppressComboAutoDropDown = dataGrid;
						control2.DropDownClosed += ComboBoxOnDropDownClosed;
						control2.IsDropDownOpen = true;
					}
					e.Handled = true;
				}
			}
		}

		private static void ComboBoxOnDropDownClosed(object sender, EventArgs eventArgs)
		{
			_suppressComboAutoDropDown.CommitEdit();
			_suppressComboAutoDropDown = null;
			((ComboBox)sender).DropDownClosed -= ComboBoxOnDropDownClosed;
		}

		private static bool IsDirectHitOnEditComponent<TControl>(ContentControl contentControl, MouseEventArgs e, out TControl control) where TControl : Control
		{
			control = (contentControl.Content as TControl);
			if (control == null)
			{
				return false;
			}
			FrameworkElement frameworkElement = VisualTreeHelper.GetChild(contentControl, 0) as FrameworkElement;
			if (frameworkElement == null)
			{
				return false;
			}
			MatrixTransform matrixTransform = (MatrixTransform)control.TransformToAncestor(frameworkElement);
			return new Rect(new Point(matrixTransform.Value.OffsetX, matrixTransform.Value.OffsetY), new Size(control.ActualWidth, control.ActualHeight)).Contains(e.GetPosition(frameworkElement));
		}
	}
}
