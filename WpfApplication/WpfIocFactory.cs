using System.Collections.ObjectModel;
using CommonLibrary.IOC;
using CommonLibrary.Services.Interfaces;
using DataAccess.Managers;
using MaCompta.ViewModels;
using CommonLibrary.Tools;
using System.Linq;
using System.Collections.Generic;

namespace MaCompta
{
    public class WpfIocFactory
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainViewModel MainVm { get; private set; }

        public RubriquesViewModel RubriquesVm { get; private set; }

        public IContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Allocate ourselves. We have a private constructor, so no one else can.
        /// </summary>
        protected static WpfIocFactory InternalInstance;

        /// <summary>
        /// Access to get the singleton object.
        /// Then call methods on that instance.
        /// </summary>
        public static WpfIocFactory Instance
        {
            get
            {
                if (InternalInstance == null)
                {
                    System.Diagnostics.Debug.WriteLine("WpfIocFactory Instance"); 
                    InternalInstance = new WpfIocFactory();
                }
                return InternalInstance;
            }
        }
        
        protected WpfIocFactory()
        {
            Log.Debug("WpfIocFactory constructor : new container");
            Comptes = new ObservableCollection<CompteViewModel>();
            Container = new SimpleIocContainer();
            OperationsPredefinies = new SortableObservableCollection<OperationPredefinieViewModel>();
        }

        public void Configure()
        {
            //Attention ordre important
            Container.Register<IRubriqueService, RubriqueManager>();
            Container.Register<ISousRubriqueService, SousRubriqueManager>();
            //Container.Register<IChequeService, ChequeManager>();

            Container.Register<IComptaService, ComptaManager>();
            Container.Register<IDetailService, DetailManager>();
            Container.Register<IOperationService, OperationManager>();
            Container.Register<IOperationPredefinieService, OperationPredefinieManager>();
            Container.Register<ICompteService, CompteManager>();
            Container.Register<IVirementDetailMontantService, VirementMontantManager>();
            Container.Register<IVirementDetailService, VirementDetailManager>();
            Container.Register<IVirementService, VirementManager>();
            Container.Register<IBanqueService, BanqueManager>();
            ConfigureViewModels();
        }

        public void  ConfigureViewModels()
        {
            Container.Register<MainViewModel, MainViewModel>();
            Container.Register<RubriquesViewModel, RubriquesViewModel>();

            MainVm = Container.Resolve<MainViewModel>();
            RubriquesVm = Container.Resolve<RubriquesViewModel>();

            Container.Register<ComptaViewModel, ComptaViewModel>(LifeCycle.Transient);

            Container.Register<RubriqueViewModel, RubriqueViewModel>(LifeCycle.Transient);
            Container.Register<SousRubriqueViewModel, SousRubriqueViewModel>(LifeCycle.Transient);

            Container.Register<VirementsViewModel, VirementsViewModel>();

            Container.Register<BanqueViewModel, BanqueViewModel>(LifeCycle.Transient);
            //Container.Register<ChequeViewModel, ChequeViewModel>(LifeCycle.Transient);
            Container.Register<OperationViewModel, OperationViewModel>(LifeCycle.Transient);
            Container.Register<OperationPredefinieViewModel, OperationPredefinieViewModel>(LifeCycle.Transient);
            Container.Register<OperationsPredefiniesViewModel, OperationsPredefiniesViewModel>(LifeCycle.Transient);
            Container.Register<CompteViewModel, CompteViewModel>(LifeCycle.Transient);
            Container.Register<DetailViewModel, DetailViewModel>(LifeCycle.Transient);
            Container.Register<VirementViewModel, VirementViewModel>(LifeCycle.Transient);
            Container.Register<VirementMontantViewModel, VirementMontantViewModel>(LifeCycle.Transient);
            Container.Register<VirementMoisViewModel, VirementMoisViewModel>(LifeCycle.Transient);
            Container.Register<VirementDetailViewModel, VirementDetailViewModel>(LifeCycle.Transient);
            Container.Register<StatistiquesViewModel, StatistiquesViewModel>(LifeCycle.Singleton);
            Container.Register<DialogOperationFiltreViewModel, DialogOperationFiltreViewModel>(LifeCycle.Transient);
            Container.Register<ComparatifViewModel, ComparatifViewModel>(LifeCycle.Singleton);
        }


        public void LogMessage(string format, params object[] args)
        {
            Log.DebugFormat(format, args);
            MainVm.DisplayMessage(string.Format(format, args));
        }

        public RubriqueViewModel GetRubrique(long rubriqueId)
        {
            return RubriquesVm.GetRubrique(rubriqueId);
        }

        public string GetRubriqueNom(long rubriqueId)
        {
            var vm = RubriquesVm.GetRubrique(rubriqueId);
            return vm!=null? vm.Libelle:"Non définie";
        }

        public SousRubriqueViewModel GetSousRubrique(RubriqueViewModel rubriqueVm, long sousRubriqueId)
        {
            return RubriquesVm.GetSousRubrique(rubriqueVm, sousRubriqueId);
        }
        public string GetSousRubriqueNom(long rubriqueId, long sousRubriqueId)
        {
            var vm = RubriquesVm.GetRubrique(rubriqueId);
            if (vm==null)
                return "Non définie";
            var sousvm = GetSousRubrique(vm, sousRubriqueId);
            return sousvm != null ? sousvm.Libelle : "Non définie";
        }
        public ObservableCollection<RubriqueViewModel> Rubriques
        {
            get { return RubriquesVm.Rubriques; }
        }

        public ObservableCollection<ComptaViewModel> Comptas
        {
            get { return MainVm.Comptas; }
        }

        public ObservableCollection<CompteViewModel> Comptes { get; private set; }

        public IEnumerable<CompteViewModel> ComptesActifs { get { return Comptes.Where(c => c.IsActif); } }

        public ObservableCollection<BanqueViewModel> Banques
        {
            get { return MainVm.Banques; }
        }

        /// <summary>
        /// Liste des opérations prédéfinies
        /// </summary>
        public SortableObservableCollection<OperationPredefinieViewModel> OperationsPredefinies { get; private set; }


    }
}
