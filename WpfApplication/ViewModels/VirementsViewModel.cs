using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using CommonLibrary.IOC;
using CommonLibrary.Managers;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;
using MaCompta.Commands;
using System.Linq;
using MaCompta.Common;

namespace MaCompta.ViewModels
{
    /// <summary>
    /// View model représentant l'ensemble des virements
    /// </summary>
    public class VirementsViewModel: ViewModelBase<VirementsViewModel>
    {
        private readonly IOperationService _operationSrv;
        private readonly IVirementService _virementSrv;

        private const string TousVirements = "Tous les virements";
        private const string VirementsActifs = "Virements actifs";
        private const string VirementsEnCours = "Virements en cours";
        private const string VirementsPermanent = "Virements permanents";

        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="container"> </param>
        /// <param name="opSrv"></param>
        /// <param name="virementSrv"></param>
        public VirementsViewModel(IContainer container, IOperationService opSrv, IVirementService virementSrv)
            :base(container)
        {
            _operationSrv = opSrv;
            _virementSrv = virementSrv;
            _virementSrv.LogRequested += (s,e) => LogMessageRequested(s, e);
            Virements = new List<VirementViewModel>();
            FilteredVirements = new ObservableCollection<VirementViewModel>();
            CollectionView = CollectionViewSource.GetDefaultView(FilteredVirements);

            FilterFrom = new FilterViewModel();
            FilterDest = new FilterViewModel();
            FilterTime = new FilterViewModel();
            FilterOrdre = new FilterViewModel();
            FilterTime.OnlyOneChoice = true;
            //PagedVirements = new PagedCollectionView(FilteredVirements);
            //PagedVirements.CurrentChanged += VirementsView_CurrentChanged;

            //Ordres = new ObservableCollection<string>();
        }

        private void FilterHasChanged(object sender, EventArgs e)
        {
            FilteredVirements.Clear();
            var filteredCompteSources = FilterFrom.FilteredItems;
            var filteredCompteDests = FilterDest.FilteredItems;
            var filteredTime = FilterTime.FilteredItems;
            var filteredOrdre = FilterOrdre.FilteredItems;
            foreach (var vrt in Virements)
            {
                var addVirement = false;
                if ((FilterFrom.IsAllSelected 
                    ||((vrt.CompteSrc == null && filteredCompteSources.Contains("Vide"))
                        || (vrt.CompteSrc != null && filteredCompteSources.Contains(vrt.CompteSrc.Libelle))))
                    && (FilterDest.IsAllSelected 
                    || ((vrt.CompteDst == null && filteredCompteDests.Contains("Vide"))
                        || (vrt.CompteDst != null && filteredCompteDests.Contains(vrt.CompteDst.Libelle))))
                    && (FilterOrdre.IsAllSelected || (vrt.Ordre != null && filteredOrdre.Contains(vrt.Ordre)))
                    && (FilterTime.IsAllSelected
                    || (filteredTime.Contains(VirementsEnCours) && vrt.Duree != 0)
                    || (filteredTime.Contains(VirementsPermanent) && vrt.Duree == -1)
                    || (filteredTime.Contains(VirementsActifs) && vrt.Duree > 0 )))
                {
                    addVirement = true;
                }
                if (addVirement)
                {
                    FilteredVirements.Add(vrt);
                }
            }
        }

        #region Properties
        /// <summary>
        /// Filtre sur la durée
        /// </summary>
        public FilterViewModel FilterTime { get; private set; }
        /// <summary>
        /// Filtre sur la source
        /// </summary>
        public FilterViewModel FilterFrom { get; private set; }
        /// <summary>
        /// Filtre sur l'ordre
        /// </summary>
        public FilterViewModel FilterOrdre { get; private set; }
        /// <summary>
        /// Filtre sur la destination
        /// </summary>
        public FilterViewModel FilterDest { get; private set; }

        /// <summary>
        /// Liste des virements
        /// </summary>
        private List<VirementViewModel> Virements { get; set; }
        /// <summary>
        /// Liste des virements
        /// </summary>
        public ObservableCollection<VirementViewModel> FilteredVirements { get; private set; }
        ///// <summary>
        ///// Liste des virements paginés
        ///// </summary>
        //public PagedCollectionView PagedVirements { get; private set; }

        /// <summary>
        /// Virement sélectionné
        /// </summary>
        private VirementViewModel _virement;
        public VirementViewModel SelectedVirement
        {
            get { return _virement; }
            set
            {
                _virement = value;
                //System.Diagnostics.Debug.WriteLine("set selectedvirement " + value.Description);
                RaisePropertyChanged(vm => vm.SelectedVirement);
            }
        }

        /// <summary>
        /// Liste des comptes pour choix
        /// </summary>
        public ObservableCollection<CompteViewModel> Comptes
        {
            get { return WpfIocFactory.Instance.Comptes; }
        }

        //private CompteViewModel _selectedCompte;

        //public CompteViewModel SelectedCompte
        //{
        //    get { return _selectedCompte; }
        //    set {
        //        _selectedCompte = value;
        //        if (value != null)
        //        {
        //            ActionFiltrer(EnumFiltrerVirement.UnCompte);
        //        }
        //        RaisePropertyChanged(vm => vm.SelectedCompte);
        //    }
        //}

        //public List<String> CompteSensFiltre
        //{
        //    get { return new List<string> { "De", "Vers", "Tout" }; }
        //}

        //private string _selectedCompteSens;
        //public string SelectedCompteSens { get { return _selectedCompteSens; } 
        //    set {_selectedCompteSens = value;
        //        //tous les virements
        //        if (value == CompteSensFiltre[2])
        //        {
        //            ActionFiltrer(EnumFiltrerVirement.TousVirements);
        //            SelectedCompte = null;
        //        }
        //        else
        //        {
        //            ActionFiltrer(EnumFiltrerVirement.UnCompte);
        //        }
        //        RaisePropertyChanged(vm => vm.SelectedCompteSens);
        //    }
        //}

        #endregion

        #region Actions

        [BaseCommand("ActionAjouterCommand")]
        public override void ActionAjouter()
        {
            var vm = Container.Resolve<VirementViewModel>();
            vm.IsNew = true;
            vm.IsModified = true;
            InitVirementEvents(vm);
            Virements.Add(vm);
            FilteredVirements.Add(vm);
            CollectionView.MoveCurrentTo(vm);
            //PagedVirements.MoveCurrentTo(vm);
        }

        private void InitVirementEvents(VirementViewModel vm)
        {
            vm.ViewModelDeleted += OnVirementDeleted;
            vm.ViewModelDuplicated += OnVirementDuplicated;
        }

        private void ResetVirementEvents(VirementViewModel vm)
        {
            vm.ViewModelDeleted -= OnVirementDeleted;
            vm.ViewModelDuplicated -= OnVirementDuplicated;
        }

        private void OnVirementDuplicated(object sender, EventArgs e)
        {
            var virement = sender as VirementViewModel;
            if (virement != null)
            {
                Virements.Add(virement);
                FilteredVirements.Add(virement);
                //PagedVirements.MoveCurrentTo(virement);
            }
        }

        void OnVirementDeleted(object sender, EventArgs<VirementModel> e)
        {
            var vm = sender as VirementViewModel;
            if (vm != null)
            {
                ResetVirementEvents(vm);
                Virements.Remove(vm);
                FilteredVirements.Remove(vm);
            }
        }

        private void InitFilters()
        {
            FilterFrom.FilterHasChanged -= FilterHasChanged;
            FilterDest.FilterHasChanged -= FilterHasChanged;
            FilterTime.FilterHasChanged -= FilterHasChanged;
            FilterOrdre.FilterHasChanged -= FilterHasChanged;

            FilterFrom.InitFilterList(FilteredVirements.Where(v=>v.CompteSrc!=null).Select(v => v.CompteSrc.Libelle).Distinct());
            FilterFrom.AddFilter("Vide", false);
            FilterDest.InitFilterList(FilteredVirements.Where(v => v.CompteDst != null).Select(v => v.CompteDst.Libelle).Distinct());
            FilterDest.AddFilter("Vide", false);
            FilterOrdre.InitFilterList(FilteredVirements.Where(v => v.Ordre != null).Select(v => v.Ordre).Distinct());
            //FilterTime.AddFilter(TousVirements, true);
            FilterTime.AddFilter(VirementsActifs, true);
            FilterTime.AddFilter(VirementsEnCours, false);
            FilterTime.AddFilter(VirementsPermanent, false);
            LogMessage(String.Format("{0} / {1} virements filtrés", FilteredVirements.Count, Virements.Count));
            
            FilterFrom.FilterHasChanged += FilterHasChanged;
            FilterDest.FilterHasChanged += FilterHasChanged;
            FilterTime.FilterHasChanged += FilterHasChanged;
            FilterOrdre.FilterHasChanged += FilterHasChanged;

        }


        #endregion   

        #region Methods
        /// <summary>
        /// Chargement des virements
        /// </summary>
        public void LoadVirements()
        {
            LogMessage( "Chargement des Virements...");
            //suppression des abonnements et vidage de la liste
            Virements.ForEach(v => ResetVirementEvents(v));
            Virements.Clear();

            //chargement des virements
            _virementSrv.LoadItems();

            foreach (
                var item in
                    _virementSrv.
                        ItemsList)
            {
                var virement =
                    Container.Resolve<VirementViewModel>().InitFromModel(item);
                virement.ViewModelDeleted += OnVirementDeleted;
                virement.ViewModelDuplicated += OnVirementDuplicated;
                Virements.Add(virement);
                FilteredVirements.Add(virement);
            }
            //SimpleIocFactory.Instance.MainVm.IsVirementEnabled = true;
            //RemplirOrdres();
            InitFilters();
        }

        //private void RemplirOrdres()
        //{
        //    Ordres.Clear();
        //    var ordres = (from op in Virements.OrderBy(o => o.Ordre)
        //                  select op.Ordre).Distinct();
        //    foreach (var ordre in ordres)
        //    {
        //        Ordres.Add(ordre);
        //    }
        //}

        private delegate void OneArgDelegate(DateTime arg);
        /// <summary>
        /// Effectuer les virements en cours
        /// </summary>
        public void EffectuerVirements()
        {
            var manager = new VirementsTools(_virementSrv, _operationSrv);
            //ou comptesvm s'abonne à un changement de collection des opérations
            manager.LogMessageRequested += LogMessageRequested;
            manager.VirementsAdded += ManagerVirementsAdded;
            var fetcher = new OneArgDelegate(
                manager.EffectuerVirementsNew);

            fetcher.BeginInvoke(DateTime.Today, null, null);

            //manager.EffectuerVirementsNew(DateTime.Today);

        }

        void ManagerVirementsAdded(object sender, EventArgs e)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                foreach (var virementViewModel in FilteredVirements)
                {
                    //pur la mise à jour de l'affichage
                    virementViewModel.ReinitFromModel();
                }

                WpfIocFactory.Instance.MainVm.ReloadComptes();
                
            });
        }

        void LogMessageRequested(object sender, EventArgs<string> e)
        {
            LogMessage(e.Data);
        }

        #endregion

    }

}
