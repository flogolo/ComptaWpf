using System;
using System.Collections.ObjectModel;
using CommonLibrary.IOC;
using CommonLibrary.Models;

namespace MaCompta.ViewModels
{
    public class ComptaViewModel : ModelViewModelBase<ComptaViewModel, ComptaModel>
    {
        public ComptaViewModel(IContainer container)
            : base(container)
        {
            Comptes = new ObservableCollection<CompteViewModel>();
            Model = new ComptaModel();
        }
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged(vm => vm.IsExpanded);
            }
        }

        public bool IsCompta { get { return true; } }

        //private bool m_IsSelected;
        //public bool IsSelected
        //{
        //    get { return m_IsSelected; }
        //    set
        //    {
        //        m_IsSelected = value;
        //        RaisePropertyChangedWithoutModification(vm => vm.IsSelected);
        //    }
        //}

        public ObservableCollection<CompteViewModel> Comptes
        { get; set; }


        ///// <summary>
        ///// Libellé de la compta
        ///// </summary>
        //private string m_Designation;
        //public string Designation
        //{
        //    get { return m_Designation; }
        //    set { m_Designation = value;
        //    RaisePropertyChangedWithModification(vm => vm.Designation);
        //    }
        //}

        ///// <summary>
        ///// Initialisation du view model à partir d'un objet model
        ///// </summary>
        ///// <param name="model"></param>
        //public override void CancelModel(ComptaModel model)
        //{
        //    throw new NotImplementedException();
        //}

        public override ComptaViewModel InitFromModel(ComptaModel model)
        {
            Id = model.Id;
            Libelle = model.Description;
            return this;
        }

        public override void SaveToModel()
        {
            throw new NotImplementedException("ComptaVieModel => SaveToModel");
        }

        public override ComptaViewModel DuplicateViewModel()
        {
            throw new NotImplementedException("ComptaVieModel => DuplicateViewModel");
        }

         /// <summary>
        /// Ajout d'un sous-élément
        /// </summary>
        public override void ActionAjouter()
        {
            var compte = Container.Resolve<CompteViewModel>();
            compte.IsNew = true;
            compte.SelectedCompta = this;
            compte.Libelle = "Nouveau compte";
            InitEvents(compte);
        }

        public void InitEvents(CompteViewModel vm)
        {
            WpfIocFactory.Instance.Comptes.Add(vm);
            Comptes.Add(vm);
            vm.ViewModelDeleted += CompteViewModelDeleted;
        }

        private void CompteViewModelDeleted(object sender, CommonLibrary.Tools.EventArgs<CompteModel> e)
        {
            var vm = sender as CompteViewModel;
            if (vm != null)
            {
                vm.ViewModelDeleted -= CompteViewModelDeleted;
                WpfIocFactory.Instance.Comptes.Remove(vm);
                Comptes.Remove(vm);
            }
        }
        public override void UpdateProperties()
        {
            //nothing to do
        }
    }
}
