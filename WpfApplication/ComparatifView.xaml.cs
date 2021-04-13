using System;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using MaCompta.ViewModels;
using System.ComponentModel;

namespace MaCompta
{
    /// <summary>
    /// Interaction logic for ComparatifView.xaml
    /// </summary>
    public partial class ComparatifView
    {
        private ComparatifViewModel _vm;

        public ComparatifView()
        {
            InitializeComponent();
            Loaded += ComparatifView_Loaded;
        }

        private void ComparatifView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _vm = WpfIocFactory.Instance.Container.Resolve<ComparatifViewModel>();
                DataContext = _vm;
            }
        }

        private void UserControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("tab stats new size {0} {1}", e.PreviousSize, e.NewSize);
//            DgRoot.RowDefinitions[1].Height = new GridLength(e.NewSize.Height - StkParams.ActualHeight);
  //          DgGraph.Width = e.NewSize.Width - DgGraph.Margin.Left - DgGraph.Margin.Right;
       //     TvStats.Height = e.NewSize.Height - StkParams.ActualHeight;
         //   TvStatsRubrique.Height = e.NewSize.Height - StkParams.ActualHeight;
        }

    }
}
