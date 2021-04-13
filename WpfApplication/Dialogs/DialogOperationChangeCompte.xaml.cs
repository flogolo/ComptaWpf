using System.Windows;

namespace MaCompta.Dialogs
{
    /// <summary>
    /// Logique d'interaction pour DialogOperationChangeCompte.xaml
    /// </summary>
    public partial class DialogOperationChangeCompte : Window
    {
        public DialogOperationChangeCompte()
        {
            InitializeComponent();
        }

        private void ButtonAnnuler_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ButtonValiderClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Etes-vous sûr(e) de vouloir changer le compte de l'opération sélectionnée?",
                                "Changement de compte", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                DialogResult = true;
                Close();
            }
        }
    }
}
