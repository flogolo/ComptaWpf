using System;
using CommonLibrary.IOC;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;

namespace MaCompta.ViewModels
{
    public class RubriqueViewModel: ModelViewModelBase<RubriqueViewModel, RubriqueModel>, IComparable<RubriqueViewModel>
    {
        #region Constructeur

        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="container"> </param>
        /// <param name="rubriqueSrv"></param>
        public RubriqueViewModel(IContainer container, IRubriqueService rubriqueSrv)
            : base(container, rubriqueSrv)
        {
            SousRubriques = new SortableObservableCollection<SousRubriqueViewModel>();
            ModelName = "Rubrique";
            Model = new RubriqueModel();
        }
        #endregion

        public SortableObservableCollection<SousRubriqueViewModel> SousRubriques { get; private set; }

        public override RubriqueViewModel InitFromModel(RubriqueModel model)
        {
            Model = model;
            Id = model.Id;
            Libelle = model.Libelle;
            return this;
        }

        public override void SaveToModel()
        {
            //return new RubriqueModel
                       {
                Model.Id = Id;
                Model.Libelle = Libelle;
                       }
        }

        public override RubriqueViewModel DuplicateViewModel()
        {
            throw new NotImplementedException();
        }

        public override void ActionAjouter()
        {
            IsSelected = true;
            var vm = Container.Resolve<SousRubriqueViewModel>();
            vm.IsNew = true;
            vm.RubriqueId = Id;
            vm.IsModified = true;
            vm.ViewModelDeleted += OnSousRubriqueDeleted;
            SousRubriques.Add(vm);
        }

        void OnSousRubriqueDeleted(object sender, EventArgs e)
        {
            var vm = sender as SousRubriqueViewModel;
            if (vm != null)
            {
                vm.ViewModelDeleted -= OnSousRubriqueDeleted;
                SousRubriques.Remove(vm);
            }
        }

        public int CompareTo(RubriqueViewModel other)
        {
            return String.Compare(Libelle, other.Libelle,StringComparison.CurrentCulture);
        }

        public void AjouterSousRubrique(SousRubriqueModel model)
        {
            var vm = Container.Resolve<SousRubriqueViewModel>().InitFromModel(model);
            vm.ViewModelDeleted += OnSousRubriqueDeleted;
            SousRubriques.Add(vm);
        }

    }
}
