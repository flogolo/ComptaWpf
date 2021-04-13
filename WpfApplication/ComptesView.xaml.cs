using System.Windows;
using System.Windows.Controls;

namespace MaCompta
{
    /// <summary>
    /// Interaction logic for ComptesView.xaml
    /// </summary>
    public partial class ComptesView
    {
        public ComptesView()
        {
            InitializeComponent();
        }

        private void DateLostFocus(object sender, RoutedEventArgs e)
        {
            var picker = sender as DatePicker;
            if (picker == null) return;
            var binding = picker.GetBindingExpression(DatePicker.SelectedDateProperty);
            if (binding != null)
            {
                binding.UpdateSource();
            }
        }

        private bool _isManualEditCommit;

        private void DataGridCellEditEnding(
            object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!_isManualEditCommit)
            {
                _isManualEditCommit = true;
                var grid = (DataGrid)sender;
                grid.CommitEdit(DataGridEditingUnit.Row, true);
                _isManualEditCommit = false;
            }
        }
    }
}
