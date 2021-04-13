using System.Windows;
using System.Windows.Controls;
using MaCompta.Common;

namespace MaCompta.Controls
{
    public class MyDataGrid:DataGrid
    {
        public MyDataGrid()
        {
            SelectionChanged += MyDataGridSelectionChanged;
           EventManager.RegisterClassHandler(typeof(DataGridCell),PreviewMouseLeftButtonDownEvent,new RoutedEventHandler(OnPreviewMouseLeftButtonDown));
        }

        private void OnPreviewMouseLeftButtonDown(object sender, RoutedEventArgs args)
        {
            var cell = sender as DataGridCell;
            if (cell != null && !cell.IsEditing && !cell.IsReadOnly)
            {
                if (!cell.IsFocused)
                {
                    cell.Focus();
                }
                var dataGrid = cell.FindAncestor<DataGrid>();
                if (dataGrid != null)
                {
                    if (dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow)
                    {
                        if (!cell.IsSelected)
                            cell.IsSelected = true;
                    }
                    else
                    {
                        var row = cell.FindAncestor<DataGridRow>();
                        if (row != null && !row.IsSelected)
                        {
                            row.IsSelected = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handle sur le changement de la sélection pour mettre a jour la position de la scrollbar si besoin.
        /// (exemple lors de la création d'un nouvel item)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e != null && e.AddedItems != null && e.AddedItems.Count == 1 && e.AddedItems[0] != null)
            {
                if (Items.Contains(e.AddedItems[0]))
                {
                    if (IsLoaded)
                    {
                        ScrollToItem(Items.IndexOf(e.AddedItems[0]));
                    }
                    else
                    {
                        if (Items.Count > 0)
                        {
                            ScrollToItem(0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Permet de déplacer la scroolbar sur l'index de l'item désiré.
        /// </summary>
        /// <param name="index">L'index de l'item à afficher</param>
        public void ScrollToItem(int index)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                (System.Windows.Threading.DispatcherOperationCallback)delegate
                {
                    int count = Items.Count;
                    if (count == 0)
                        return null;

                    if (index < 0)
                    {
                        ScrollIntoView(Items[0]); // scroll to first
                    }
                    else
                    {
                        if (index < count)
                        {
                            ScrollIntoView(Items[index]); // scroll to item
                        }
                        else
                        {
                            ScrollIntoView(Items[count - 1]); // scroll to last
                        }
                    }
                    return null;
                }, null);

        }

    }
}
