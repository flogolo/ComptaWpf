using MaCompta.ViewModels;
using System.Windows;

namespace MaCompta.Dialogs
{
    /// <summary>
    /// Interaction logic for OperationPredefinieView.xaml
    /// </summary>
    public partial class DialogOperationPredefinie
    {
        public DialogOperationPredefinie()
        {
            DataContext = WpfIocFactory.Instance.Container.Resolve<OperationsPredefiniesViewModel>();
            InitializeComponent();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
