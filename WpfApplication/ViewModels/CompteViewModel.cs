using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using CommonLibrary.IOC;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;
using MaCompta.Commands;
using MaCompta.Tools;
using System.Windows.Threading;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace MaCompta.ViewModels
{
    public enum EnumOperationFilter
    {
        Tout,
        Valide,
        EnCours,
        Partiel,
        NonPartiel
    }

    public class CompteViewModel : ModelViewModelBase<CompteViewModel, CompteModel>
    {
        public const string AllOrdres = "Tous";

        #region Membres
        //private readonly CollectionViewSource m_OperationViewSource;
        //private string m_Designation;
        private string _numero;
        private string _carteBancaire;
        private EnumOperationFilter _filtreType;
        private ICompteService _compteSrv;
        private IOperationService _operationSrv;
        private IVirementService _virementService;
        private MenuItemViewModel menuPredifined;
        private bool _isLoaded = false;
        private bool _isActif;
        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="container"> </param>
        /// <param name="compteSrv"></param>
        /// <param name="operationSrv"></param>
        public CompteViewModel(IContainer container, ICompteService compteSrv, IOperationService operationSrv,
            IDetailService detailService, IVirementService virementService)
            : base(container, compteSrv)
        {
            _compteSrv = compteSrv;
            _operationSrv = operationSrv;
            _virementService = virementService;
            Model = new CompteModel();
            OperationsList = new List<OperationViewModel>();
            FilterOrdre = new FilterViewModel();
            FilterRubrique = new FilterViewModel();
            FilterPaiement = new FilterViewModel();
            FilteredOperationsList = new ObservableCollection<OperationViewModel>();
            CollectionView = CollectionViewSource.GetDefaultView(FilteredOperationsList);
            _filtreType = EnumOperationFilter.NonPartiel;
            ModelName = "Compte";
            FilteredOperationsList.Add(new OperationViewModel());
            ComptesMainMenuItems = new ObservableCollection<MenuItemViewModel>();
        }

        public void Load()
        {
            if (_isLoaded) return;
            InitCommands();
            InitMenu();
            LoadCompteOperations();
            _isLoaded = true;
        }

        private void InitCommands()
        {
            CommandAjouterOperation = new RelayCommand(param =>
            {
                ActionAjouter();
            });
            CommandOperationChangeCompte = new RelayCommand(param =>
            {
                WeakReferenceMessenger.Default.Send(
                new ShowDialogMessage(ShowDialogMessageEnum.OperationChangeCompte)
                { CompteVM = this, SelectedOperation = SelectedOperation });
            }, (p) => { return SelectedOperation != null; });
            CommandSauvegarderToutesOperations = new RelayCommand(param =>
            {
                ActionSauvegarderOperations();
            });

            CommandValiderToutesOperations = new RelayCommand(param =>
            {
                ActionValider();
            });
            CommandVoir30DernieresCartes = new RelayCommand(param =>
            {
                WeakReferenceMessenger.Default.Send(
                new ShowDialogMessage(ShowDialogMessageEnum.OperationsFiltrees)
                { CompteVM = this });
            });
            CommandCreerModifierOperationPredefinie = new RelayCommand(param =>
            {
                WeakReferenceMessenger.Default.Send(
                new ShowDialogMessage(ShowDialogMessageEnum.OperationPredefinie)
                { CompteVM = this });
            });
            CommandFiltrerCompte = new RelayCommand(param =>
            {
                var id = param as long?;
                if (id != null)
                {
                    FiltreTypeIndex = (int)id.Value;
                }});
            CommandOperationFiltre = new RelayCommand(param =>
            {
                WeakReferenceMessenger.Default.Send(
                new ShowDialogMessage(ShowDialogMessageEnum.OperationFiltre)
                { CompteVM = this });
            });
            CommandAjouterOperationPredefinie = new RelayCommand(param =>
            {
                var id = param as long?;
                if (id != null)
                {
                    CreerOperationPredefinie(WpfIocFactory.Instance.OperationsPredefinies.FirstOrDefault(o => o.Id == id));
                }
            });
        }

        private void InitMenu()
        {
            ComptesMainMenuItems.Add(new MenuItemViewModel(null) { Header = CompteHeader + " > " });
            var menuOperations = new MenuItemViewModel(null) { Header = "Opérations" };
            ComptesMainMenuItems.Add(menuOperations);
            menuOperations.MenuItems.Add(new MenuItemViewModel(CommandAjouterOperation) { Header = "Ajouter une opération" });
            menuOperations.MenuItems.Add(new MenuItemViewModel(CommandOperationChangeCompte) { Header = "Changer le compte de l'opération sélectionnée..." });
            menuOperations.MenuItems.Add(new MenuItemViewModel(CommandSauvegarderToutesOperations) { Header = "Sauvegarder toutes les opérations modifiées" });
            menuOperations.MenuItems.Add(new MenuItemViewModel(CommandValiderToutesOperations) { Header = "Valider toutes les opérations partielles" });
            menuOperations.MenuItems.Add(new MenuItemViewModel(CommandVoir30DernieresCartes) { Header = "Voir les opérations de type carte des 30 derniers jours" });

            menuPredifined = new MenuItemViewModel(null) { Header = "Prédéfinies" };
            ComptesMainMenuItems.Add(menuPredifined);
            menuPredifined.MenuItems.Add(new MenuItemViewModel(CommandCreerModifierOperationPredefinie) { Header = "Créer ou modifier une opération prédéfinie..." });

            var menuFiltre = new MenuItemViewModel(null) { Header = "Filtrer" };
            ComptesMainMenuItems.Add(menuFiltre);
            menuFiltre.MenuItems.Add(new MenuItemViewModel(CommandFiltrerCompte) { Header = "En cours", Id = (long)EnumOperationFilter.EnCours });
            menuFiltre.MenuItems.Add(new MenuItemViewModel(CommandFiltrerCompte) { Header = "Partielles", Id = (long)EnumOperationFilter.Partiel });
            menuFiltre.MenuItems.Add(new MenuItemViewModel(CommandFiltrerCompte) { Header = "Non partielles", Id = (long)EnumOperationFilter.NonPartiel });
            menuFiltre.MenuItems.Add(new MenuItemViewModel(CommandFiltrerCompte) { Header = "Validées", Id = (long)EnumOperationFilter.Valide });
            menuFiltre.MenuItems.Add(new MenuItemViewModel(CommandFiltrerCompte) { Header = "Tout", Id = (long)EnumOperationFilter.Tout });
            menuFiltre.MenuItems.Add(new MenuItemViewModel(CommandOperationFiltre) { Header = "Filtre avancé..." });

            foreach (var item in WpfIocFactory.Instance.OperationsPredefinies.OrderBy(op=>op.Description))
            {
                AddOperationPredefinieToMenu(item);
            }
        }

        public void AddOperationPredefinieToMenu(OperationPredefinieViewModel item)
        {
            menuPredifined.MenuItems.Add(new MenuItemViewModel(CommandAjouterOperationPredefinie)
            {
                Header = item.Description,
                Id = item.Id
            });
        }
        public void RemoveOperationPredefinieToMenu(OperationPredefinieViewModel item)
        {
            var menuItem = menuPredifined.MenuItems.FirstOrDefault(m => m.Id == item.Id);
            if (menuItem != null)
            {
                menuPredifined.MenuItems.Remove(menuItem);
            }
        }

        private void CreerOperationPredefinie(OperationPredefinieViewModel operationPredefinieViewModel)
        {
            //ajouter une opération initialisée avec l'opération prédéfinie sélectionnée
            //SelectedCompte.
            var model = new OperationModel
            {
                CompteId = Id,
                DateOperation = DateTime.Today,
                Ordre = operationPredefinieViewModel.Ordre,
                TypePaiement = PaiementHelper.GetCodePaiement(operationPredefinieViewModel.SelectedPaiement),
                Details = new List<DetailModel>
                                              {
                                                  new DetailModel
                                                      {
                                                          Commentaire = operationPredefinieViewModel.Commentaire,
                                                          RubriqueId = operationPredefinieViewModel.SelectedRubrique.Id,
                                                          SousRubriqueId =
                                                              operationPredefinieViewModel.SelectedSousRubrique.Id
                                                      }
                                              }
            };
            if (PaiementHelper.IsCheque(operationPredefinieViewModel.SelectedPaiement))
                //model.Cheque = new ChequeModel {Numero = "inconnu"};
                model.NumeroCheque = "remplir";

            _operationSrv.CreateOperationWithDetails(model);
            var opVm = Container.Resolve<OperationViewModel>().InitFromModel(model);
            AddOperationViewModel(opVm);
        }

  

        #endregion

        #region Propriétés
        public ICommand CommandAjouterOperation { get; private set; }
        public ICommand CommandOperationChangeCompte { get; private set; }
        public ICommand CommandSauvegarderToutesOperations { get; private set; }
        public ICommand CommandValiderToutesOperations { get; private set; }
        public ICommand CommandVoir30DernieresCartes { get; private set; }
        public ICommand CommandCreerModifierOperationPredefinie { get; private set; }
        public ICommand CommandFiltrerCompte { get; private set; }
        public ICommand CommandOperationFiltre { get; private set; }
        public ICommand CommandAjouterOperationPredefinie { get; private set; }

        public ObservableCollection<MenuItemViewModel> ComptesMainMenuItems { get; set; }

        /// <summary>
        /// Libellé du compte sélectionné pour le header du tab
        /// </summary>
        public string CompteHeader
        {
            get
            {
                return SelectedCompta.Libelle + "/" + Model.Designation;
            }
        }

        /// <summary>
        /// Libellé du compte sélectionné
        /// </summary>
        public string CompteLibelle
        {
            get
            {
                return SelectedCompta.Libelle + "/" + Model.Designation + " (" + _filtreType.ToString() + ")";
            }
        }

        public bool IsCompta { get { return false; } }

        private int _anneeArchive;

        /// <summary>
        /// Gets the AnneeArchive property.
        /// </summary>
        public int AnneeArchive
        {
            get
            {
                return _anneeArchive;
            }

            set
            {
                if (_anneeArchive == value)
                {
                    return;
                }

                _anneeArchive = value;
                RaisePropertyChanged(vm => vm.AnneeArchive);
            }
        }

        /// <summary>
        /// n° de compta associé
        /// </summary>
        public long ComptaId
        {
            get
            {
                if (Model != null)
                    return Model.ComptaId;
                return -1;
            }
        }

        //pour la vue hierarchique
        public ObservableCollection<CompteViewModel> Comptes
        { get { return null; } }

        /// <summary>
        /// Numéro du compte
        /// </summary>
        public string Numero
        {
            get { return _numero; }
            set { _numero = value;
            RaisePropertyChangedWithModification(vm => vm.Numero);
            }
        }

        /// <summary>
        /// Carte bancaire associé
        /// </summary>
        public string CarteBancaire
        {
            get { return _carteBancaire; }
            set { _carteBancaire = value;
            RaisePropertyChangedWithModification(vm => vm.CarteBancaire);
            }
        }

        public bool IsActif { get { return _isActif; }
            set
            {
                _isActif = value;
                RaisePropertyChangedWithModification(vm => vm.IsActif);
            }
        }

        public bool IsOperationsModified { get
            {
                return OperationsList.Any(o => o.IsModified || o.IsDetailModified);
            } }

        /// <summary>
        /// Liste des opérations du compte
        /// </summary>
        public List<OperationViewModel> OperationsList 
        {get; set;}

        /// <summary>
        /// Liste des opérations filtrées
        /// </summary>
        public ObservableCollection<OperationViewModel> FilteredOperationsList
        { get; private set; }

        /// <summary>
        /// Liste des ordres utilisés pour le filtrage
        /// </summary>
        //public ObservableCollection<CheckedListItem<string>> OrdreFilters { get; private set; }
        public FilterViewModel FilterOrdre { get; private set; }
        public FilterViewModel FilterRubrique { get; private set; }
        public FilterViewModel FilterPaiement { get; private set; }

        public decimal SoldeValide
        {
            get
            {
                return OperationsList.Where(op => op.DateValidation != null).Sum(op => op.Montant);
            }
        }
        public decimal SoldePrevu
        {
            get
            {
                return OperationsList.Where(op=>op.IsReport == false).Sum(op => op.Montant);
            }
        }

        #region Solde à la date
        /// <summary>
        /// Date du solde
        /// </summary>
        private DateTime? _dateSolde;
        public DateTime? DateSolde
        {
            get { return _dateSolde; }
            set
            {
                _dateSolde = value;
                RaisePropertyChanged(vm => vm.DateSolde);
                RaisePropertyChanged(vm => vm.SoldeDate);
            }
        }

        public decimal SoldeDate
        {
            get
            {
                if (DateSolde!=null)
                    return OperationsList.Where(op => op.DateValidation != null && op.DateValidation <= _dateSolde).Sum(op => op.Montant);
                return 0;
            }
        }

        private decimal _operationTotal;
        public decimal OperationTotal
        {
            get {return _operationTotal;}
            set {
                _operationTotal = value;
                RaisePropertyChanged(vm => vm.OperationReste);
            }
        }
        public decimal OperationReste
        {
            get { 
                if (SelectedOperation != null)
                return OperationTotal + SelectedOperation.Montant;
            return 0;
            }
        }
        #endregion

        public decimal SoldeValidePartiel
        {
            get
            {
                return OperationsList.Where(op => op.DateValidation != null ||
                    (op.DateValidation == null && op.DateValidationPartielle != null && op.IsReport==false)).Sum(op => op.Montant);
            }
        }

        //public ICollectionView OperationView { get { return m_OperationViewSource.View; } }
        public Collection<ComptaViewModel> Comptas
        {
            get { return WpfIocFactory.Instance.Comptas; }
        }

        private ComptaViewModel _selectedCompta;
        public ComptaViewModel SelectedCompta
        {
            get { return _selectedCompta; }
            set
            {
                _selectedCompta = value;
                RaisePropertyChangedWithModification(vm => vm.SelectedCompta);
            }
        }

        public Collection<BanqueViewModel> Banques
        {
            get { return WpfIocFactory.Instance.Banques; }
        }

        private BanqueViewModel _selectedBanque;
        public BanqueViewModel SelectedBanque
        {
            get { return _selectedBanque; }
            set
            {
                _selectedBanque = value;
                RaisePropertyChangedWithModification(vm => vm.SelectedBanque);
            }
        }
        public string LastValidationPartielleDate
        {
            get {
                var opWithValidationPartielle = OperationsList.Where(o => o.DateValidationPartielle != null);
                if (opWithValidationPartielle.Count() == 0)
                    return null;
                var maxdate = opWithValidationPartielle.Max(o => o.DateValidationPartielle.Value);
                                return maxdate==null? null : maxdate.ToShortDateString(); }
        }

        #endregion

        #region Méthodes

        /// <summary>
        /// remplit les filtres avec les opérations affichées
        /// </summary>
        private void InitFilters()
        {
            FilterOrdre.FilterHasChanged -= FilterHasChanged;
            FilterRubrique.FilterHasChanged -= FilterHasChanged;
            FilterPaiement.FilterHasChanged -= FilterHasChanged;

            FilterOrdre.InitFilterList(FilteredOperationsList.Select(w => w.Ordre).Distinct());
            var rubriques = new List<string>();
            foreach (var item in FilteredOperationsList)
            {
                foreach (var detail in item.DetailsList)
                {
                    if (detail.SelectedRubrique != null)
                    {
                        rubriques.Add(detail.SelectedRubrique.Libelle);
                        if (detail.SelectedSousRubrique != null)
                        {
                            rubriques.Add(detail.SelectedRubrique.Libelle + " / " + detail.SelectedSousRubrique.Libelle);
                        }
                    }
                }
            }
            FilterRubrique.InitFilterList(rubriques.Distinct());
            FilterPaiement.InitFilterList(PaiementHelper.TypesPaiement);

            FilterOrdre.FilterHasChanged += FilterHasChanged;
            FilterRubrique.FilterHasChanged += FilterHasChanged;
            FilterPaiement.FilterHasChanged += FilterHasChanged;
        }
        DateTime? _filtreDate1;
        DateTime? _filtreDate2;
        public void Filtrer(EnumOperationFilter filtreType, 
            DateTime? filtreDate1, DateTime? filtreDate2)
        {
            FilteredOperationsList.Clear();
            _filtreDate1 = filtreDate1;
            _filtreDate2 = filtreDate2;
            _filtreType = filtreType;
            //if (OneFilterAllDeselected)
            //    return;
            foreach (var operation in OperationsList.OrderBy(o=>o.DateOperation))
            {
                //filtre sur l'état
                FiltrerOperation(operation);
            }
           
            InitFilters();
            LogMessage(String.Format("{0} / {1} opérations filtrées", FilteredOperationsList.Count, OperationsList.Count));
        }

        private void FiltrerOperation(OperationViewModel operation)
        {
            if (operation.FilterDate(_filtreType, _filtreDate1, _filtreDate2))
            {
                if (
                    (FilterRubrique.IsAllSelected ||
                    (FilterRubrique.IsNotAllSelected && operation.FilterRubrique(FilterRubrique.FilterItems.Where(f => f.IsChecked).Select(f => f.Item)))
                    )
                    &&
                    (FilterOrdre.IsAllSelected ||
                    (FilterOrdre.IsNotAllSelected && operation.FilterOrdre(FilterOrdre.FilterItems.Where(f => f.IsChecked).Select(f => f.Item)))
                    )
                    && (
                    FilterPaiement.IsAllSelected ||
                    (FilterPaiement.IsNotAllSelected && operation.FiltrerType(FilterPaiement.FilterItems.Where(f => f.IsChecked).Select(f => f.Item)))))

                {
                    FilteredOperationsList.Add(operation);
                }
            }
        }

        private void FilterHasChanged(object sender, EventArgs e)
        {
            FilteredOperationsList.Clear();
            //if (OneFilterAllDeselected)
              //  return;
            foreach (var operation in OperationsList.OrderBy(o => o.DateOperation))
            {
                FiltrerOperation(operation);
            }
        }

        //private void FiltrerDates()
        //{
        //    if ((FiltreDate1 == null && FiltreDate2 != null)
        //        || (FiltreDate2 == null && FiltreDate1 != null)
        //        || (FiltreDate2!=null && FiltreDate1!=null && FiltreDate2.Value < FiltreDate1.Value))
        //    {
        //        LogMessage("Dates de filtre incorrectes");
        //        return;
        //    }
        //    Filtrer();
        //}

        private void CalculSoldes()
        {
            RaisePropertyChanged(vm => vm.SoldePrevu);
            RaisePropertyChanged(vm => vm.SoldeValide);
            RaisePropertyChanged(vm => vm.SoldeValidePartiel);
            RaisePropertyChanged(vm => vm.OperationReste);
        }

        private OperationViewModel _selectedOperation;
        public OperationViewModel SelectedOperation
        {
            get
            { return _selectedOperation;}
            set {
                if (value != null)
                { 
                    _selectedOperation = value;
                //m_SelectedOperation.IsExpanded = true;
                }
                RaisePropertyChanged(vm => vm.SelectedOperation);
                RaisePropertyChanged(vm => vm.OperationReste);
            }
        }

        /// <summary>
        /// Ajouter une opération vide
        /// </summary>
        public override void ActionAjouter()
        {
            var newVm = Container.Resolve<OperationViewModel>();

            newVm.DateOperation = DateTime.Now;
            newVm.IsNew = true;
            newVm.CompteId = Id;
            newVm.IsModified = true;
            EventsInit(newVm);
            OperationsList.Add(newVm);
            if (newVm.FilterDate(_filtreType, _filtreDate1, _filtreDate2))
                FilteredOperationsList.Add(newVm);
            //AddFilteredOperation(newVm);
            SelectedOperation = newVm;
            CollectionView.MoveCurrentTo(newVm);
            //            PagedOperations.MoveCurrentTo(newVm);
        }

        private void OnOperationDeleted(object sender, EventArgs e)
        {
            var opVm = sender as OperationViewModel;
            if (opVm != null)
            {
                EventsClear(opVm);
                OperationsList.Remove(opVm);
                FilteredOperationsList.Remove(opVm);
                CalculSoldes();
            }
        }

        private void OnMontantUpdated(object sender, EventArgs e)
        {
            RaisePropertyChanged(vm => vm.LastValidationPartielleDate);
            CalculSoldes();
        }

        private void EventsInit(OperationViewModel vm)
        {
            vm.PropertyChanged += OperationPropertyChanged;
            //montant de l'opération nmodifié (détail modifié)
            vm.MontantUpdated += OnMontantUpdated;
            //opération modifiée (dates de validation)
            vm.ViewModelSaved += OnMontantUpdated;
            //opération supprimée
            vm.ViewModelDeleted += OnOperationDeleted;
            //opération dupliquée
            vm.ViewModelDuplicated += OnOperationDuplicated;
        }

        private void OperationPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsModified" || e.PropertyName == "IsDetailModified")
            {
                RaisePropertyChanged(vm => vm.IsOperationsModified);
            }
        }

        private void EventsClear(OperationViewModel vm)
        {
            vm.MontantUpdated -= OnMontantUpdated;
            vm.ViewModelSaved -= OnMontantUpdated;
            vm.ViewModelDeleted -= OnOperationDeleted;
            vm.ViewModelDuplicated -= OnOperationDuplicated;
            vm.PropertyChanged -= OperationPropertyChanged;
        }

        void OnOperationDuplicated(object sender, EventArgs e)
        {
            var vm = sender as OperationViewModel;
            EventsInit(vm);
            OperationsList.Add(vm);
            //AddFilteredOperation(vm);
           //if (vm.FilterDate(m_Filter))
                FilteredOperationsList.Add(vm);

            CalculSoldes();
        }

        public override CompteViewModel InitFromModel(CompteModel model)
        {
            Model = model;
            Id = model.Id;
            SelectedCompta = Comptas.FirstOrDefault(c => c.Id == model.ComptaId);
            Libelle = model.Designation;
            CarteBancaire = model.CarteBancaire;
            IsActif = model.IsActif;
            Numero = model.Numero;
            SelectedBanque = Banques.FirstOrDefault(c => c.Id == model.BanqueId);
            return this;
        }

        public override void SaveToModel()
        {
            //return new CompteModel
            Model.Id = Id;
            Model.ComptaId = SelectedCompta.Id;
            Model.Designation = Libelle;
            Model.CarteBancaire = CarteBancaire;
            Model.Numero = Numero;
            Model.BanqueId = SelectedBanque.Id;
            Model.IsActif = IsActif;
        }

        public override CompteViewModel DuplicateViewModel()
        {
            throw new NotImplementedException("CompteVieModel => DuplicateViewModel");
        }

        public void AddOperationViewModel(OperationViewModel vm)
        {
            OperationsList.Add(vm);
            EventsInit(vm);
            if (vm.FilterDate(_filtreType, _filtreDate1, _filtreDate2))
                FilteredOperationsList.Add(vm);
            CollectionView.MoveCurrentTo(vm);
            CalculSoldes();
        }

        [BaseCommand("ActionValiderCommand")]
        public void ActionValider()
        {
            foreach (var op in OperationsList)
            {
                if (op.DateValidationPartielle != null && op.DateValidation == null)
                    op.DateValidation = op.DateValidationPartielle;
            }
            CalculSoldes();
        }

        [BaseCommand("ActionArchiverCommand")]
        public void ActionArchiver()
        {
            if (AnneeArchive > 0)
            {
                var model = new CompteModel
                {
                    BanqueId = Model.BanqueId,
                    CarteBancaire = Model.CarteBancaire,
                    ComptaId = Model.ComptaId,
                    Designation = Model.Designation + " archives " + AnneeArchive.ToString(CultureInfo.InvariantCulture),
                    Numero = Model.Numero,
                    IsActif = Model.IsActif
                };
                //créer le compte d'archivage
                _compteSrv.CreateItem(model);
                OnCompteCreated(model.Id, model.Designation);
                WpfIocFactory.Instance.MainVm.OnCompteCreated(model);
            }
        }

        #region Archiver
        [BaseCommand("ActionDeleteCommand")]
        public void ActionDelete()
        {
            WpfIocFactory.Instance.MainVm.MessageBoxShow(SupprimerCompte, String.Format("Etes-vous sûr de vouloir supprimer ce {0}?", ModelName), "Alerte", MessageBoxButton.YesNo);
        }

        public void SupprimerCompte(MessageBoxResult result)
        {
            if (result != MessageBoxResult.Yes) return;
            
            //supprimer les opérations
            foreach (var op in OperationsList)
            {
                _operationSrv.DeleteItem(op.Id, true);
            }
            //supprimer les virements utilisés par le compte
            var virements = _virementService.ItemsList.Where(i => i.CompteSrcId == Model.Id || i.CompteDstId == Model.Id).ToList();
            foreach (var virement in virements)
            {
                //si l'autre n'est pas rempli -> on supprime
                if (virement.CompteSrcId == Model.Id && virement.CompteDstId == 0
                    || virement.CompteDstId == Model.Id && virement.CompteSrcId == 0)
                {
                    _virementService.DeleteItem(virement.Id, true);
                }
                else
                {
                    //si l'autre compte est rempli -> on met à jour
                    if (virement.CompteSrcId == Model.Id)
                    {
                        virement.CompteSrcId = 0;
                    }
                    else
                    {
                        virement.CompteDstId = 0;
                    }
                    _virementService.UpdateItem(virement, true);
                }
            }
            _compteSrv.DeleteItem(Model.Id, true);
            RaiseDeletedEvent();

        }

        /// <summary>
        /// un compte d'archivage a été créé
        /// </summary>
        /// <param name="compteId">id du nouveau compte créé</param>
        /// <param name="designation">désignation du nouveau compte créé</param>
        void OnCompteCreated(long compteId, string designation)
        {
            //copier les opérations concernées
            var totalAnnee = 0m;
            foreach (var op in OperationsList)
            {
                if (op.DateOperation != null && op.DateOperation.Value.Year == AnneeArchive)
                {
                    //changemnt du numéro de compte
                    op.CompteId = compteId;
                    //l'opération a été modifiée
                    op.IsModified = true;
                    totalAnnee += op.Montant;
                }
            }

            //créer l'opération de bilan
            //31/12/anneearchive bilanannée, rubrique/sousrubrique?, totalAnnee
            var newop = Container.Resolve<OperationViewModel>();
            newop.DateOperation = DateTime.Now;
            newop.SelectedPaiement = PaiementHelper.GetPaiement("VRT");
            //ordre = désignation du compte cible
            newop.Ordre = designation;
            var detail = Container.Resolve<DetailViewModel>();
            detail.Commentaire = "Bilan année " + AnneeArchive;
            detail.Montant = totalAnnee;
            //detail.SelectedRubrique = ;
            //detail.SelectedSousRubrique = ;
            newop.DetailsList.Add(detail);

            //OperationsList.Add(newop);
            System.Diagnostics.Debug.WriteLine("total année {0}", totalAnnee);

            ActionSauvegarderOperations();
        }
        #endregion

        [BaseCommand("ActionSauvegarderToutCommand")]
        public void ActionSauvegarderOperations()
        {
            {
                //mise à jour des opérations modifiées
                var modified = OperationsList.Where(o => (o.IsModified && !o.IsNew) || o.IsDetailModified).ToList();
                if (modified.Any())
                {
                    modified.ForEach(op=>op.ActionSauvegarder());
                }
                //création des opérations ajoutées
                var added = OperationsList.Where(o => o.IsModified && o.IsNew).ToList();
                if (added.Any())
                {
                    added.ForEach(op => op.ActionSauvegarder());
                }
            }
        }

        private void LoadOperations()
        {
            OperationsList.Clear();
            var operations = _operationSrv.LoadOperationsEnCours(Id);
            //WpfIocFactory.Instance.LogMessage("Opérations chargées nb=" + m_OperationSrv.ItemsList.Count);

            foreach (var operation in OperationsList)
            {
                EventsClear(operation);
            }
            OperationsList.Clear();
            //foreach (var model in m_OperationSrv.ItemsList.OrderBy(o => o.DateOperation))
            foreach (var model in operations.OrderBy(o => o.DateOperation))
            {
                var vm = Container.Resolve<OperationViewModel>().InitFromModel(model);
                EventsInit(vm);
                OperationsList.Add(vm);
            }
            RaisePropertyChanged(vm => vm.LastValidationPartielleDate);
            CalculSoldes();
        }
        /// <summary>
        /// Chargement des opérations du compte
        /// </summary>
        public void LoadCompteOperations()
        {
            WeakReferenceMessenger.Default.Send(
                new ShowDialogMessage(ShowDialogMessageEnum.MessageAttente));
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate
            {
                System.Diagnostics.Debug.WriteLine("{0} Chargement des opérations ...", DateTime.Now);
                LogMessage("Chargement des opérations du compte sélectionné...");
                LoadOperations();
                Filtrer(_filtreType, null, null);
                System.Diagnostics.Debug.WriteLine(
                        "{0} Chargement des opérations terminé",
                        DateTime.Now);
                LogMessage("Chargement des opérations terminé");
                WeakReferenceMessenger.Default.Send(
                    new ShowDialogMessage(ShowDialogMessageEnum.FinAttente));
            }));
        }

        #endregion

        #region Filter

        public int FiltreTypeIndex
        {
            get { return (int) _filtreType; }
            set {
                _filtreType = (EnumOperationFilter) value;
                if (FiltreTypeIndex >= 0)
                {
                    Filtrer(_filtreType, null, null);
                }
                RaisePropertyChanged(vm => vm.FiltreTypeIndex);
                RaisePropertyChanged(vm => vm.CompteLibelle);
            }
        }

        //public bool OneFilterAllDeselected
        //{
        //    get
        //    {
        //        return (FilterRubrique.IsAllDeSelected 
        //            || FilterOrdre.IsAllDeSelected 
        //            || FilterPaiement.IsAllDeSelected);
        //    }
        //}


        #endregion

            /// <summary>
            /// Suppression de l'opération de la liste des opérations du compte sélectionné
            /// </summary>
        public void RemoveSelectedOperation()
        {
            //System.Diagnostics.Debug.WriteLine("RemoveSelectedOperation");
            OperationsList.Remove(SelectedOperation);
            FilteredOperationsList.Remove(SelectedOperation);
            CalculSoldes();
        }

        internal IEnumerable<OperationViewModel> LastCarteOperation()
        {
            var dateNow = DateTime.Now;
            var dateLast = dateNow.AddDays(-30);
            return OperationsList.Where(o => o.DateOperation <= dateNow && o.DateOperation >= dateLast && o.SelectedPaiement == PaiementHelper.GetPaiement("CB"));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _compteSrv = null;
                _operationSrv = null;
                menuPredifined = null;
                CommandAjouterOperation = null;
                CommandOperationChangeCompte = null;
                CommandSauvegarderToutesOperations = null;
                CommandValiderToutesOperations = null;
                CommandVoir30DernieresCartes = null;
                CommandCreerModifierOperationPredefinie = null;
                CommandFiltrerCompte = null;
                CommandOperationFiltre = null;
                CommandAjouterOperationPredefinie = null;

                ComptesMainMenuItems.Clear();
                ComptesMainMenuItems = null;
                OperationsList.Clear();
                OperationsList = null;
                FilteredOperationsList.Clear();
                FilteredOperationsList = null;
                FilterOrdre = null;
                FilterRubrique = null;
                FilterPaiement = null;
                _selectedCompta = null;
                _selectedBanque = null;
                _selectedOperation = null;
            }
            base.Dispose(disposing);
        }

        public override void UpdateProperties()
        {
            //nothing to do
        }
    }

}