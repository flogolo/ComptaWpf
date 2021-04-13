using System;
using System.Collections.Generic;
using PortableCommon.IOC;
using PortableCommon.Models;
using PortableCommon.Services.Interfaces;

namespace PortableCommon.ViewModels
{
    public class PortableRubriqueViewModel: ModelViewModelBase<PortableRubriqueViewModel, RubriqueModel>, IComparable<PortableRubriqueViewModel>
    {
        readonly IRubriqueService m_RubriqueSrv;

        #region Constructeur

        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="container"> </param>
        /// <param name="rubriqueSrv"></param>
        public PortableRubriqueViewModel(IContainer container, IRubriqueService rubriqueSrv)
            : base(container)
        {
            SousRubriques = PortabilityFactory.Instance.CreateSortableList<PortableSousRubriqueViewModel>();
            m_RubriqueSrv = rubriqueSrv;
        }
        #endregion

        private bool m_IsSelected;
        public bool IsSelected
        {
            get { return m_IsSelected; }
            set { m_IsSelected = value;
                RaisePropertyChanged(vm=>vm.IsSelected); }
        }

        private string m_Libelle;
        public String Libelle
        {
            get { return m_Libelle; }
            set { m_Libelle = value; 
                RaisePropertyChanged(vm=>vm.Libelle);}
        }

        public IList<PortableSousRubriqueViewModel> SousRubriques { get; private set; }

        //private SousRubriqueViewModel m_Model;
        //public void SupprimerSousRubrique(SousRubriqueViewModel vm)
        //{
        //    if (IsNew)
        //        SousRubriques.Remove(vm);
        //    else
        //    {
        //        m_Model = vm;
        //        ServiceFactory.Instance.MainVm.MessageService =
        //            "La sous-rubrique est en cours de suppression";
        //        ServiceFactory.Instance.Services.SousRubriqueManager.ItemDeleted += SousRubriqueManager_ItemDeleted;
        //        ServiceFactory.Instance.Services.SousRubriqueManager.DeleteItem(vm.Id);
        //    }
        //}

        //public void SousRubriqueManager_ItemDeleted(object sender, EventArgs e)
        //{
        //    SimpleIocFactory.Instance.UIAction(() =>
        //    {
        //        SousRubriques.Remove(m_Model);
        //        ServiceFactory.Instance.Services.SousRubriqueManager.ItemDeleted -= SousRubriqueManager_ItemDeleted;
        //        ServiceFactory.Instance.MainVm.MessageService =
        //            "La sous-rubrique a été supprimée";
        //    });
        //}

        /// <summary>
        /// Initialisation du view model à partir d'un objet model
        /// </summary>
        /// <param name="model"></param>
        public override void CancelModel(RubriqueModel model)
        {
            InitFromModel(model);
        }

        public override PortableRubriqueViewModel InitFromModel(RubriqueModel model)
        {
            Model = model;
            Id = model.Id;
            Libelle = model.Libelle;
            return this;
        }

        public override RubriqueModel SaveToModel()
        {
            return new RubriqueModel
                       {
                           Id = Id ,
                           Libelle = Libelle,
                       };
        }

        public override PortableRubriqueViewModel DuplicateViewModel()
        {
            throw new NotImplementedException();
        }

        public override void ActionAjouter()
        {
            IsSelected = true;
            var vm = Container.Resolve<PortableSousRubriqueViewModel>();
            vm.IsNew = true;
            vm.RubriqueId = Id;
            vm.IsModified = true;
            vm.ViewModelDeleted += OnSousRubriqueDeleted;
            SousRubriques.Add(vm);
        }

        void OnSousRubriqueDeleted(object sender, EventArgs e)
        {
            var vm = sender as PortableSousRubriqueViewModel;
            if (vm != null)
            {
                vm.ViewModelDeleted -= OnSousRubriqueDeleted;
                SousRubriques.Remove(vm);
            }
        }

        public override void ActionSupprimer()
        {
            if (!IsNew)
                m_RubriqueSrv.DeleteItem(Id);
            RaiseDeletedEvent(this);
        }

        public override void  ActionSauvegarder()
        {
            //sauvegarde de la rubrique
            PortabilityFactory.Instance.LogMessage("Rubrique en cours de sauvegarde...");
            if (IsNew)
            {
                m_RubriqueSrv.ItemCreated += RubriqueManagerItemCreated;
                m_RubriqueSrv.CreateItem(SaveToModel());
            }
            else
            {
                m_RubriqueSrv.ItemUpdated += RubriqueManagerItemUpdated;
                m_RubriqueSrv.UpdateItem(SaveToModel());
            }
        }

        private void RubriqueManagerItemCreated(object sender, HandledEventArgs<RubriqueModel> e)
        {
                PortabilityFactory.Instance.LogMessage("Rubrique créée");
                m_RubriqueSrv.ItemCreated -= RubriqueManagerItemCreated;
                Id = e.Data.Id;
                IsNew = false;
                IsModified = false;
                Model = e.Data;
        }

        void RubriqueManagerItemUpdated(object sender, EventArgs e1)
        {
            m_RubriqueSrv.ItemUpdated -= RubriqueManagerItemUpdated;
            IsModified = false;
            PortabilityFactory.Instance.LogMessage("Rubrique sauvegardée");
        }

        public int CompareTo(PortableRubriqueViewModel other)
        {
            return String.Compare(Libelle, other.Libelle,StringComparison.CurrentCulture);
        }

        public void AjouterSousRubrique(SousRubriqueModel model)
        {
            var vm = Container.Resolve<PortableSousRubriqueViewModel>().InitFromModel(model);
            vm.ViewModelDeleted += OnSousRubriqueDeleted;
            SousRubriques.Add(vm);
        }

        public override string ToString()
        {
            return Libelle;
        }
    }
}
