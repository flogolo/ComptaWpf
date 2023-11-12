using System;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using MaCompta.ViewModels;
using System.ComponentModel;

namespace MaCompta
{
    /// <summary>
    /// Interaction logic for StatsView.xaml
    /// </summary>
    public partial class StatsView
    {
        private StatistiquesViewModel _statViewModel;

        public StatsView()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(StatsViewLoaded);
        }

        void StatsViewLoaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _statViewModel = WpfIocFactory.Instance.Container.Resolve<StatistiquesViewModel>();
                DataContext = _statViewModel;
                _statViewModel.StatsUpdated += StatViewModelStatsUpdated;
            }
        }

        private void StatViewModelStatsUpdated(object sender, EventArgs e)
        {
            //ChartColumn.Series.Clear();
            //foreach (var stat in _statViewModel.StatsMulti)
            //{
            //    ChartColumn.Series.Add(new ColumnSeries
            //    {
            //        Title = stat.Libelle,
            //        ItemsSource = stat.Stats,
            //        IndependentValueBinding = new System.Windows.Data.Binding("Mois"),
            //        DependentValueBinding = new System.Windows.Data.Binding("Montant"),
            //    });
            //}
        }

        private void MoisPrecedentClick(object sender, RoutedEventArgs e)
        {
            _statViewModel.DateFin = _statViewModel.DateDebut.AddDays(-1);
            _statViewModel.DateDebut = _statViewModel.DateDebut.AddMonths(-1);
        }

        private void MoisSuivantClick(object sender, RoutedEventArgs e)
        {
            _statViewModel.DateDebut = _statViewModel.DateFin.AddDays(1);
            _statViewModel.DateFin = _statViewModel.DateDebut.AddMonths(1).AddDays(-1);
        }

        private void CurrentMois(object sender, RoutedEventArgs e)
        {
            _statViewModel.DateDebut = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            _statViewModel.DateFin = _statViewModel.DateDebut.AddMonths(1).AddDays(-1);
        }

        private void CurrentAnnee(object sender, RoutedEventArgs e)
        {
            _statViewModel.DateDebut = new DateTime(DateTime.Today.Year, 1, 1);
            _statViewModel.DateFin = new DateTime(DateTime.Today.Year, 12, 31);
        }

        private void AnneePrecedenteClick(object sender, RoutedEventArgs e)
        {
            _statViewModel.DateDebut = new DateTime(_statViewModel.DateDebut.Year - 1, 1, 1);
            _statViewModel.DateFin = new DateTime(_statViewModel.DateDebut.Year, 12, 31);
        }

        private void AnneeSuivanteClick(object sender, RoutedEventArgs e)
        {
            _statViewModel.DateDebut = new DateTime(_statViewModel.DateDebut.Year + 1, 1, 1);
            _statViewModel.DateFin = new DateTime(_statViewModel.DateDebut.Year, 12, 31);
        }

        private void UserControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("tab stats new size {0} {1}", e.PreviousSize, e.NewSize);
            DgRoot.RowDefinitions[1].Height = new GridLength(e.NewSize.Height - StkParams.ActualHeight);
            DgGraph.Width = e.NewSize.Width - DgGraph.Margin.Left - DgGraph.Margin.Right;
            TvStats.Height = e.NewSize.Height - StkParams.ActualHeight;
            TvStatsRubrique.Height = e.NewSize.Height - StkParams.ActualHeight;
        }

        private void AnneeScolaireCouranteClick(object sender, RoutedEventArgs e)
        {
            if (DateTime.Today.Month >= 9)
            {
                _statViewModel.DateDebut = new DateTime(DateTime.Today.Year, 9, 1);
                _statViewModel.DateFin = new DateTime(DateTime.Today.Year + 1, 7, 31);
            }
            else
            {
                _statViewModel.DateDebut = new DateTime(DateTime.Today.Year - 1, 9, 1);
                _statViewModel.DateFin = new DateTime(DateTime.Today.Year, 7, 31);
            }
        }

        private void AnneeScolairePrecedenteClick(object sender, RoutedEventArgs e)
        {
            if (_statViewModel.DateDebut.Month != 9)
                AnneeScolaireCouranteClick(sender, e);
            _statViewModel.DateDebut = _statViewModel.DateDebut.AddYears(-1);
            _statViewModel.DateFin = _statViewModel.DateFin.AddYears(-1);
        }

        private void AnneeScolaireSuivanteClick(object sender, RoutedEventArgs e)
        {
            if (_statViewModel.DateDebut.Month != 9)
                AnneeScolaireCouranteClick(sender, e);
            _statViewModel.DateDebut = _statViewModel.DateDebut.AddYears(1);
            _statViewModel.DateFin = _statViewModel.DateFin.AddYears(1);
        }

        private void dgGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }


    }
}
