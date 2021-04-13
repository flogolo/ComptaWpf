using System;
using System.Linq;
using System.Windows.Data;
using CommonLibrary.IOC;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;
using MaCompta.Commands;

namespace MaCompta.ViewModels
{
    public class RubriquesViewModel: ViewModelBase<RubriquesViewModel>
    {
        private readonly IRubriqueService _rubriqueSrv;
        private readonly ISousRubriqueService _sousRubriqueSrv;

        public RubriquesViewModel(IContainer container, IRubriqueService rubriqueSrv, ISousRubriqueService sousRubSrv)
            : base(container)
        {
            Rubriques = new SortableObservableCollection<RubriqueViewModel>();
            _rubriqueSrv = rubriqueSrv;
            _sousRubriqueSrv = sousRubSrv;
            CollectionView = CollectionViewSource.GetDefaultView(Rubriques);
             //RubriquesView.CurrentChanged += (s,e) => RaisePropertyChanged(vm=>vm.SelectedRubrique);
        }


        #region Properties
        //public override PortableRubriqueViewModel SelectedRubrique
        //{ get {return null;} }

        #endregion

        #region Methods

        protected void SortRubriques()
        {
            Rubriques.Sort();
        }
        #endregion


        #region Properties
        //public abstract PortableRubriqueViewModel SelectedRubrique
        //{ get ; }

        public SortableObservableCollection<RubriqueViewModel> Rubriques
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        public void LoadRubriques()
        {
            LogMessage("Chargement des rubriques...");
            _rubriqueSrv.LoadItems();

            LogMessage("Rubriques chargées");
            foreach (var model in _rubriqueSrv.ItemsList.OrderBy(r=>r.Libelle))
            {
                var vm = Container.Resolve<RubriqueViewModel>().InitFromModel(model);
                vm.ViewModelDeleted += OnRubriqueDeleted;
                Rubriques.Add(vm);
            }

            //SortRubriques();
//            RubriquesView = new PaginatedObservableCollection<RubriqueViewModel>(Rubriques);

            LogMessage(
                "Chargement des sous-rubriques...");
            _sousRubriqueSrv.LoadItems();
            foreach (
                var model in _sousRubriqueSrv.ItemsList.OrderBy(r => r.Libelle))
            {
                var rubrique =
                    Rubriques.FirstOrDefault(r => r.Id == model.RubriqueId);
                if (rubrique != null)
                    rubrique.AjouterSousRubrique(model);
            }
            ////tri des sous-rubriques
            //foreach (var rubrique in Rubriques)
            //{
            //    rubrique.SousRubriques.Sort();
            //}
            LogMessage("Sous-rubriques chargées");
            //RubriquesLoaded.Raise(this, EventArgs.Empty);
            //if (RubriquesLoaded != null)
            //    RubriquesLoaded(null, EventArgs.Empty);
        }

        public void OnRubriqueDeleted(object sender, EventArgs e)
        {
            var vm = sender as RubriqueViewModel;
            if (vm != null)
            {
                vm.ViewModelDeleted -= OnRubriqueDeleted;
                Rubriques.Remove(vm);
            }
        }


        #endregion

        public RubriqueViewModel GetRubrique(long rubriqueId)
        {
            return Rubriques.FirstOrDefault(r => r.Id == rubriqueId);

        }
        public SousRubriqueViewModel GetSousRubrique(RubriqueViewModel rubriqueVm, long sousRubriqueId)
        {
            if (rubriqueVm != null)
            {
                var srVm = rubriqueVm.SousRubriques.FirstOrDefault(s => s.Id == sousRubriqueId);
                return srVm;
            }
            return null;
        }

        //public event EventHandler RubriquesLoaded;

        [BaseCommand("ActionAjouterCommand")]
        public override void ActionAjouter()
        {
            var vm = Container.Resolve<RubriqueViewModel>();
            vm.IsNew = true;
            vm.IsModified = true;
            vm.ViewModelDeleted += OnRubriqueDeleted;
            Rubriques.Add(vm);
            CollectionView.MoveCurrentTo(vm);
        }

    }
}
