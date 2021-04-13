using System.Windows.Controls;

namespace MaCompta
{
    /// <summary>
    /// Interaction logic for RubriquesView.xaml
    /// </summary>
    public partial class RubriquesView
    {
        public RubriquesView()
        {
            InitializeComponent();
        }

        private bool _isManualEditCommit;

        private void DataGridCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
        //    System.Diagnostics.Debug.WriteLine("cellendediting " + e.Column);
            if (!_isManualEditCommit && e.EditAction == DataGridEditAction.Commit)
            {
                _isManualEditCommit = true;
                var grid = (DataGrid)sender;
                grid.CommitEdit(DataGridEditingUnit.Row, true);
                _isManualEditCommit = false;
            }
        }

    }
}
