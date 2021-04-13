using MaCompta.ViewModels;
using System.Windows;

namespace MaCompta.Dialogs
{
    /// <summary>
    /// Logique d'interaction pour DialogOperationFiltre.xaml
    /// </summary>
    public partial class DialogOperationFiltre : Window
    {
        public DialogOperationFiltre()
        {
            InitializeComponent();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as DialogOperationFiltreViewModel;
            if (dc != null)
            {
                if (dc.IsValidFilter)
                {
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Dates incorrectes", "Filtre incorrect", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
