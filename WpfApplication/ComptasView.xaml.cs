using MaCompta.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace MaCompta
{
    /// <summary>
    /// Interaction logic for Comptas.xaml
    /// </summary>
    public partial class ComptasView
    {
        public ComptasView()
        {
            InitializeComponent();
        }

        private void UserControlSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
          //  System.Diagnostics.Debug.WriteLine("tab comptas new size {0} {1}", e.PreviousSize, e.NewSize);

        }
        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {

            if (sender is TreeViewItem)
            {
                if (!((TreeViewItem)sender).IsSelected)
                {
                    return;
                }

                var dc = DataContext as MainViewModel;
                var treeviewItem = sender as TreeViewItem;
                var selectedCompte = treeviewItem.DataContext as CompteViewModel;
                if (selectedCompte != null)
                {
                    dc.CommandSelectCompte.Execute(selectedCompte.Id);
                }
            }
        }

    }
}
