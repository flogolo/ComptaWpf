using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaCompta.Controls
{
    /// <summary>
    /// Logique d'interaction pour MyFilter.xaml
    /// </summary>
    public partial class MyFilter : UserControl
    {
        public MyFilter()
        {
            InitializeComponent();
        }

        private void btnOrdreFilter_Click(object sender, RoutedEventArgs e)
        {
            popOrdre.IsOpen = true;
        }
    }
}
