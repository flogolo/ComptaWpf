using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using CommonLibrary.IOC;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;
using System.Collections.Specialized;
using System.Windows.Input;
using MaCompta.Commands;
using System.Collections.Generic;

namespace MaCompta.ViewModels
{
    public class MainViewModel : ViewModelBase<MainViewModel>
    {
        #region Membres
        private readonly IComptaService _comptaService;
        private readonly IBanqueService _banqueService;
        private readonly ICompteService _compteService;
        private readonly IOperationPredefinieService _opPredefService;
        private readonly IOperationService _opService;
        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        public MainViewModel(IContainer container,
            IComptaService comptaService,
            IBanqueService banqueService,
            ICompteService compteService,
            IOperationPredefinieService operationPredefinieService,
            IOperationService opService
            )
            : base(container)
        {
            //IsRubriqueEnabled = false;
            //IsVirementEnabled = false;
            //IsCompteEnabled = false;
            Comptas = new ObservableCollection<ComptaViewModel>();
            Banques = new ObservableCollection<BanqueViewModel>();

            _comptaService = comptaService;
            _comptaService.ErrorOccured += ComptaServiceErrorOccured;
            _comptaService.LogRequested += ComptaServiceLogRequested;
            _banqueService = banqueService;
            _banqueService.ErrorOccured += ComptaServiceErrorOccured;
            _banqueService.LogRequested += ComptaServiceLogRequested;
            _compteService = compteService;
            _compteService.ErrorOccured += ComptaServiceErrorOccured;
            _compteService.LogRequested += ComptaServiceLogRequested;
            _opPredefService = operationPredefinieService;
            _opPredefService.ErrorOccured += ComptaServiceErrorOccured;
            _opPredefService.LogRequested += ComptaServiceLogRequested;
            _opService = opService;
            _opService.ErrorOccured += ComptaServiceErrorOccured;
            _opService.LogRequested += ComptaServiceLogRequested;

            Messages = new ObservableCollection<string>();
            SelectedCompta = null;
            SelectedCompte = null;
            ComptesMainMenuItems = new ObservableCollection<MenuItemViewModel>();
            var menuCompta = new MenuItemViewModel(null) { Header = "Sélectionner un compte > " };
            ComptesMainMenuItems.Add(menuCompta);
            WpfIocFactory.Instance.Comptes.CollectionChanged += Comptes_CollectionChanged;
            CommandSelectCompte = new RelayCommand(param =>
            {
                var id = param as long?;
                if (id != null)
                {
                    SelectedCompte = WpfIocFactory.Instance.Comptes.FirstOrDefault(c => c.Id == id.Value);
                    if (SelectedCompte.Id > 0)
                        SelectedCompteChanged(this, new EventArgs<CompteViewModel> { Data = SelectedCompte });
                }
            });
            VersionsList = new List<VersionModifications>();
            VersionModifications.FillVersions(VersionsList);
        }

        internal void AddOperationPredefinieToMenu(OperationPredefinieViewModel opVm)
        {
            foreach (var compteVm in OpenedComptes)
            {
                compteVm.AddOperationPredefinieToMenu(opVm);
            }
        }

        internal void RemoveOperationPredefinieToMenu(OperationPredefinieViewModel opVm)
        {
            foreach (var compteVm in OpenedComptes)
            {
                compteVm.RemoveOperationPredefinieToMenu(opVm);
            }
        }

        internal void AddOpenedCompte(CompteViewModel cvm)
        {
            OpenedComptes.Add(cvm);
        }

        internal void ReloadCompteForOperation(long operationId)
        {
            bool isCompteFound = false;
            foreach (var compteVm in OpenedComptes)
            {
                foreach(var opVm in compteVm.OperationsList)
                {
                    if(opVm.Id == operationId)
                    {
                        //recharger
                        opVm.InitFromModel(opVm.Model);
                        isCompteFound = true;
                        break;
                    }
                }
                if (isCompteFound)
                {
                    break;
                }
            }
            
        }

        internal void ReloadComptes()
        {
            foreach (var compteVm in OpenedComptes)
            {
                compteVm.LoadCompteOperations();
            }
        }

        private void Comptes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var menuCompta = ComptesMainMenuItems.First();
                foreach (var item in e.NewItems)
                {
                    var compteVm = item as CompteViewModel;
                    if (compteVm != null)
                    {
                        //var comptaMenuItem = menuCompta.MenuItems.FirstOrDefault(c => c.Header == compteVm.SelectedCompta.Libelle);
                        var comptaMenuItem = ComptesMainMenuItems.FirstOrDefault(c => c.Header == compteVm.SelectedCompta.Libelle);
                        if (comptaMenuItem == null)
                        {
                            comptaMenuItem = new MenuItemViewModel(null) { Header = compteVm.SelectedCompta.Libelle };
                            //menuCompta.MenuItems.Add(comptaMenuItem);
                            ComptesMainMenuItems.Add(comptaMenuItem);
                        }
                        var compteMenuItem = new MenuItemViewModel(CommandSelectCompte) { Header = compteVm.Libelle, Id = compteVm.Id };
                        comptaMenuItem.MenuItems.Add(compteMenuItem);
                    }

                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var compteVm = item as CompteViewModel;
                    if (compteVm != null)
                    {
                        var comptaMenuItem = ComptesMainMenuItems.FirstOrDefault(c => c.Header == compteVm.SelectedCompta.Libelle);
                        var compteMenuItem = comptaMenuItem.MenuItems.FirstOrDefault(c => c.Id == compteVm.Id);
                        if (compteMenuItem != null)
                        {
                            comptaMenuItem.MenuItems.Remove(compteMenuItem);
                        }
                        //pour fermer d'onglet
                        RaiseCompteDelete(compteVm);
                    }
                }
            }
        }

        public void OnCompteCreated(CompteModel e)
        {
            //un nouveau compte a été créé
            var compte = Container.Resolve<CompteViewModel>().InitFromModel(e);
            //le rattacher à la bonne compta
            var compta = Comptas.FirstOrDefault(c => c.Id == compte.ComptaId);
            if (compta != null)
                compta.InitEvents(compte);
        }
        #endregion

        #region Properties

        public List<VersionModifications> VersionsList { get; private set; }

        public Collection<CompteViewModel> OpenedComptes = new Collection<CompteViewModel>();

        public ICommand CommandSelectCompte { get; private set; }

        public ObservableCollection<MenuItemViewModel> ComptesMainMenuItems { get; set; }

        public ObservableCollection<ComptaViewModel> Comptas { get; private set; }
        public ObservableCollection<BanqueViewModel> Banques { get; private set; }

        /// <summary>
        /// Message pour la barre d'état
        /// </summary>
        private string _messageService;
        public String MessageService
        {
            get { return _messageService; }
            set
            {
                _messageService = value;
                Messages.Insert(0, value);
                RaisePropertyChanged(vm => vm.MessageService);
            }
        }

        public ObservableCollection<string> Messages { get; private set; }

        //private bool m_IsRubriqueEnabled;
        //public bool IsRubriqueEnabled
        //{
        //    get { return m_IsRubriqueEnabled; }
        //    set { m_IsRubriqueEnabled = value;
        //    RaisePropertyChanged(vm=>IsRubriqueEnabled);}
        //}

        //private bool m_IsCompteEnabled;
        //public bool IsCompteEnabled
        //{
        //    get { return m_IsCompteEnabled; }
        //    set { m_IsCompteEnabled = value;
        //    RaisePropertyChanged(vm=>vm.IsCompteEnabled);}
        //}

        //private bool m_IsVirementEnabled;
        //public bool IsVirementEnabled
        //{
        //    get { return m_IsVirementEnabled; }
        //    set { m_IsVirementEnabled = value;
        //    RaisePropertyChanged(vm=>vm.IsVirementEnabled);}
        //}

        private object _item;
        public object Item
        {
            get { return _item; }
            set
            {
                if (_item != value)
                {
                    _item = value;

                    RaisePropertyChanged(vm => vm.Item);

                    //si c'est un compte, sélectionner l'onglet comptes
                    var selectedCompte = Item as CompteViewModel;
                    if (selectedCompte != null)
                    {
                        SelectedCompte = selectedCompte;
                        //CommandSelectCompte.Execute(selectedCompte.Id);
                        //SelectedCompteChanged(this, new EventArgs<CompteViewModel> { Data = Item as CompteViewModel });
                        SelectedCompta = null;
                    }
                    //si c'est une compta, afficher ses propriétés
                    else if (Item is ComptaViewModel)
                    {
                        SelectedCompta = Item as ComptaViewModel;
                        SelectedCompte = null;
                    }
                    RaisePropertyChanged(vm => vm.SelectedCompta);
                    RaisePropertyChanged(vm => vm.IsComptaSelected);
                    RaisePropertyChanged(vm => vm.SelectedCompte);
                }
            }
        }

        public event EventHandler<EventArgs<CompteViewModel>> SelectedCompteChanged;
        public event EventHandler<EventArgs<CompteViewModel>> CompteDeleted;

        public void RaiseCompteDelete(CompteViewModel compteVm)
        {
            if( CompteDeleted != null)
            {
                CompteDeleted(this, new EventArgs<CompteViewModel>(compteVm));
            }
        }

        public CompteViewModel SelectedCompte { get; set; }
        public bool IsCompteSelected { get { return SelectedCompte != null; } }

        public ComptaViewModel SelectedCompta { get; set; }

        public bool IsComptaSelected { get { return SelectedCompta != null; } }

        public bool HasError { get; private set; }
        #endregion

        #region Méthodes

        public bool TestDatabase()
        {
            var isTestOk = _comptaService.TestConnexion();
            if(!isTestOk)
            {
                Messages.Add("Erreur de connexion à la base");
            }
            return isTestOk;
        }

        /// <summary>
        /// Chargement des données
        /// </summary>
        public void LoadMain()
        {
            //Chargement des comptas
            _comptaService.LoadItems();
            foreach (var item in _comptaService.ItemsList.OrderBy(c => c.Description))
            {
                Comptas.Add(Container.Resolve<ComptaViewModel>().InitFromModel(item));
            }
            //chargement des banques
            _banqueService.LoadItems();
            foreach (var item in _banqueService.ItemsList.OrderBy(b => b.Description))
            {
                Banques.Add(Container.Resolve<BanqueViewModel>().InitFromModel(item));
            }
            //chargement des comptes
            _compteService.LoadItems();
            foreach (var compte in _compteService.ItemsList.OrderBy(c => c.Designation))
            {
                //if (compte.IsActif)
                {
                    var compteVm = Container.Resolve<CompteViewModel>().InitFromModel(compte);
                    //recherche compta associée
                    var compta = Comptas.First(c => c.Id == compteVm.ComptaId);
                    compta.IsExpanded = true;
                    compta.InitEvents(compteVm);
                }
            }

            //chargement des opérations prédéfinies
            _opPredefService.LoadItems();
            //LogMessage("Opérations prédéfinies chargées");
            WpfIocFactory.Instance.OperationsPredefinies.Clear();
            foreach (
                var op in _opPredefService.ItemsList)
            {
                var vm = Container.Resolve<OperationPredefinieViewModel>().InitFromModel(op);
                    WpfIocFactory.Instance.OperationsPredefinies.Add(vm);
            }
            WpfIocFactory.Instance.OperationsPredefinies.Sort();
            //chargement de toutes les opérations
            _opService.LoadItems();
        }

        void ComptaServiceLogRequested(object sender, EventArgs<string> e)
        {
            DisplayMessage(e.Data);
        }

        void ComptaServiceErrorOccured(object sender, ErrorEventArgs e)
        {
            HasError = true;
            MessageBoxShow(null, e.ErrorMessage, "Erreur d'accès au service");
            //DisplayMessage(e.ErrorMessage);
        }

        /// <summary>
        /// Affichage d'un message de log dans la vue principale
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void DisplayMessage(string message)
        {

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() =>
            {
                MessageService = message;
            }));
        }

        #endregion

        #region Evènements
        public event EventHandler<MvvmMessageBoxEventArgs> MessageBoxRequest;

        public void MessageBoxShow(Action<MessageBoxResult> resultAction, string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None)
        {
            if (MessageBoxRequest != null)
            {
                MessageBoxRequest(this, new MvvmMessageBoxEventArgs(resultAction, messageBoxText, caption, button, icon, defaultResult, options));
            }
        }
        #endregion
    }

}
