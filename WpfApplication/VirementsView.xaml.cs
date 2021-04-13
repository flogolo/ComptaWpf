using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaCompta.ViewModels;

namespace MaCompta
{
    /// <summary>
    /// Interaction logic for VirementsView.xaml
    /// </summary>
    public partial class VirementsView
    {
        private VirementsViewModel ViewModel { get { return DataContext as VirementsViewModel; } }
        public VirementsView()
        {
            InitializeComponent();
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

        private void CopierMoisClick(object sender, RoutedEventArgs e)
        {
            var dc = ((Button)sender).DataContext as VirementMoisViewModel;
            if (dc != null)
            {
                if (ViewModel != null)
                    ViewModel.SelectedVirement.CopierMois(dc);
            }
        }

        private void CollerMoisClick(object sender, RoutedEventArgs e)
        {
            var dc = ((Button)sender).DataContext as VirementMoisViewModel;
            if (dc != null)
            {
                if (ViewModel != null)
                    ViewModel.SelectedVirement.CollerMois(dc);
            }
        }

        private void RazMoisClick(object sender, RoutedEventArgs e)
        {
            var dc = ((Button)sender).DataContext as VirementMoisViewModel;
            if (dc != null)
            {
                dc.RazMontants();
            }
        }

        private void CopierToutMoisClick(object sender, RoutedEventArgs e)
        {
            var dc = ((Button)sender).DataContext as VirementMoisViewModel;
            if (dc != null)
            {
                if (ViewModel != null)
                    ViewModel.SelectedVirement.CopierTout(dc);
            }
        }

        private void DateLostFocus(object sender, RoutedEventArgs e)
        {
            var picker = sender as DatePicker;
            //if (picker != null)
            //  MessageBox.Show(picker.SelectedDate.ToString());
            if (picker != null)
            {
                var binding = picker.GetBindingExpression(DatePicker.SelectedDateProperty);
                if (binding != null)
                {
                    binding.UpdateSource();
                }
            }
        }
        public delegate void VirementDelegate();

        private void EffectuerVirements(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                Cursor = Cursors.Wait;
                ViewModel.EffectuerVirements();
                Cursor = null;
            }
        }

        private void SauvegarderMontantsClick(object sender, RoutedEventArgs e)
        {
            var dc = ((Button)sender).DataContext as VirementViewModel;
            if (dc != null)
            {
                dc.SauvegarderMontants();
            }
        }

        private void ExpanderSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //var expanderHeader = ChildFinder.FindVisualChildren<ToggleButton>(expMontants).First();

            var newHeight = e.NewSize.Height
                - BtnSauver.ActualHeight - BtnSauver.Margin.Top - BtnSauver.Margin.Bottom
                - DgMois.Margin.Top - DgMois.Margin.Bottom
                - ExpMontants.Padding.Bottom - ExpMontants.Padding.Top - ExpMontants.Margin.Top - ExpMontants.Margin.Bottom
                - StkMontants.Margin.Top - StkMontants.Margin.Bottom - 17;
            //expanderHeader.ActualHeight;
            //System.Diagnostics.Debug.WriteLine("Expander_SizeChanged {0} {1}" , e.NewSize, newHeight);
            if (newHeight > 0)
                DgMois.Height = newHeight; 
        }

        private readonly List<double> _headerSize = new List<double>();

        //private int[] _headerSize = new int[12];
        private void LbHeaderSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("lbHeader_SizeChanged " + sender);
            var lb = sender as ListBox;
            //System.Diagnostics.Debug.WriteLine("lbHeader_SizeChanged taille colonne " + dgMois.Columns[1].ActualWidth);
            _headerSize.Clear();
            if (lb != null && lb.Items.Count > 0)
            {
                for (int i = 0; i < lb.Items.Count; i++)
                {
                    var lbi = (lb.ItemContainerGenerator.ContainerFromIndex(i)) as ListBoxItem;
                    if (lbi != null) _headerSize.Add(lbi.ActualWidth);
                    //System.Diagnostics.Debug.WriteLine("lbHeader_SizeChanged taille header " + lbi.ActualWidth);
                }
            }
        }

        private void DgMoisDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //var vm = e.NewValue as VirementViewModel;
            //if (vm!= null && vm.DetailsList!=null)
            //    System.Diagnostics.Debug.WriteLine("dgMois_DataContextChanged " + vm.DetailsList.Count);
            //else
            //    System.Diagnostics.Debug.WriteLine("dgMois_DataContextChanged null");
            //reforcer le calcul de la largeur de la liste des détails/montants
            DgMois.Columns[1].Width = 20;
            DgMois.Columns[1].Width = new DataGridLength(0, DataGridLengthUnitType.Auto);
        }

        //List<ListBox> _tabListBox = new List<ListBox>();
        private void LstDataLoaded(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("lstData_Loaded " + sender);
            var lbdata = sender as ListBox;
            if (lbdata != null)
                for (int i = 0; i < lbdata.Items.Count; i++)
                {
                    var lbidata = (lbdata.ItemContainerGenerator.ContainerFromIndex(i)) as ListBoxItem;
                    if (lbidata != null)
                        lbidata.Width = _headerSize.ElementAt(i);
                }
        }
    }
}
