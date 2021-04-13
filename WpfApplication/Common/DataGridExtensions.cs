using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MaCompta.Common
{
    public class DataGridExtensions
    {
        static DataGridExtensions()
        {
            //Allows to set DataContextProperty on the columns. Must only be invoked once per application.
            FrameworkElement.DataContextProperty.AddOwner(typeof (DataGridColumn));
        }

        public static object GetDataContextForColumns(DependencyObject obj)
        {
            return obj.GetValue(DataContextForColumnsProperty);
        }

        public static void SetDataContextForColumns(DependencyObject obj, object value)
        {
            obj.SetValue(DataContextForColumnsProperty, value);
        }

        /// <summary>
        /// Allows to set DataContext property on columns of the DataGrid (DataGridColumn)
        /// </summary>
        /// <example><DataGridTextColumn Header="{Binding DataContext.ColumnHeader, RelativeSource={RelativeSource Self}}" /></example>
        public static readonly DependencyProperty DataContextForColumnsProperty =
            DependencyProperty.RegisterAttached(
                "DataContextForColumns",
                typeof (object),
                typeof (DataGridExtensions),
                new UIPropertyMetadata(OnDataContextChanged));

        /// <summary>
        /// Propogates the context change to all the DataGrid's columns
        /// </summary>
        private static void OnDataContextChanged(DependencyObject d,
                                                 DependencyPropertyChangedEventArgs e)
        {
            var grid = d as DataGrid;
            if (grid == null) return;

            foreach (DataGridColumn col in grid.Columns)
            {
                col.SetValue(FrameworkElement.DataContextProperty, e.NewValue);
            }
        }
    }

    public static class DataGridHelper
    {
        public static DataGridCell GetCell(DataGridCellInfo dataGridCellInfo)
        {
            if (!dataGridCellInfo.IsValid)
            {
                return null;
            }

            var cellContent = dataGridCellInfo.Column.GetCellContent(dataGridCellInfo.Item);
            if (cellContent != null)
            {
                return (DataGridCell)cellContent.Parent;
            }
            return null;
        }
        public static int GetRowIndex(DataGridCell dataGridCell)
        {
            // Use reflection to get DataGridCell.RowDataItem property value.
            PropertyInfo rowDataItemProperty = dataGridCell.GetType().GetProperty("RowDataItem", BindingFlags.Instance | BindingFlags.NonPublic);

            DataGrid dataGrid = GetDataGridFromChild(dataGridCell);

            return dataGrid.Items.IndexOf(rowDataItemProperty.GetValue(dataGridCell, null));
        }
        public static DataGrid GetDataGridFromChild(DependencyObject dataGridPart)
        {
            if (VisualTreeHelper.GetParent(dataGridPart) == null)
            {
                throw new NullReferenceException("Control is null.");
            }
            if (VisualTreeHelper.GetParent(dataGridPart) is DataGrid)
            {
                return (DataGrid)VisualTreeHelper.GetParent(dataGridPart);
            }
            return GetDataGridFromChild(VisualTreeHelper.GetParent(dataGridPart));
        }
    }
}
