using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommonLibrary.IOC;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using MaCompta.Tools;

namespace MaCompta.ViewModels
{
    public class DetailViewModel : ModelViewModelBase<DetailViewModel, DetailModel>
    {
        public DetailViewModel(IContainer container, IDetailService detailSrv)
            : base(container, detailSrv)
        {
            ModelName = "Opération Détail";
            Model = new DetailModel();
        }

        #region Properties
        private RubriqueViewModel _selectedRubrique;
        public RubriqueViewModel SelectedRubrique
        {
            get { return _selectedRubrique; }
            set { _selectedRubrique = value;
            RaisePropertyChangedWithModification(vm => vm.SelectedRubrique);
                if (value != null)
                {
                    RaisePropertyChanged(vm => vm.SousRubriques);
                    SelectedSousRubrique = SousRubriques.First();
                }
            }
        }

        private SousRubriqueViewModel _selectedSousRubrique;
        public SousRubriqueViewModel SelectedSousRubrique
        {
            get { return _selectedSousRubrique; }
            set { _selectedSousRubrique = value;
            RaisePropertyChangedWithModification(vm => vm.SelectedSousRubrique);
            }
        }

        private decimal _montant;
        public decimal Montant
        {
            get { return _montant; }
            set { _montant = value;
                RaisePropertyChangedWithModification(vm => vm.Montant);
                //RaiseMontantUpdated();
            }
        }

        private string _commentaire;
        public String Commentaire
        {
            get { return _commentaire; }
            set { _commentaire = value;
            RaisePropertyChangedWithModification(vm => vm.Commentaire);
            }
        }

        public ObservableCollection<RubriqueViewModel> Rubriques
        {
            get
            {
                return WpfIocFactory.Instance.Rubriques;
            }
        }

        public ObservableCollection<SousRubriqueViewModel> SousRubriques
        {
            get
            {
                if (SelectedRubrique != null)
                    return SelectedRubrique.SousRubriques;
                return null;
            }
        }

        /// <summary>
        /// identifiant de l'opération
        /// </summary>
        public long OperationId
        {
            get;set;
        }

        /// <summary>
        /// identifiant de l'opération liée
        /// </summary>
        public long? OperationLienId
        {
            get; set;
        }

        /// <summary>
        /// identifiant du détail lié
        /// </summary>
        public long? DetailLienId { get; set; }
        #endregion

        /// <summary>
        /// Initialise la rubrique est la sous-rubrique selon leurs Id
        /// </summary>
        /// <param name="rubriqueId">identifiant rubrique</param>
        /// <param name="sousRubriqueId">identifiant sous-rubrique</param>
        public void InitRubrique(long rubriqueId, long sousRubriqueId)
        {
            SelectedRubrique = WpfIocFactory.Instance.GetRubrique(rubriqueId);
            SelectedSousRubrique = WpfIocFactory.Instance.GetSousRubrique(_selectedRubrique, sousRubriqueId);
        }

        ///// <summary>
        ///// Initialisation du view model à partir d'un objet model
        ///// </summary>
        ///// <param name="model"></param>
        //public override void CancelModel(DetailModel model)
        //{
        //    InitFromModel(model);
        //}

        public override DetailViewModel InitFromModel(DetailModel model)
        {
            Model = model;
            Id = model.Id;
            Commentaire = model.Commentaire;
            Montant= model.Montant;
            OperationId = model.OperationId;
            DetailLienId = model.LienDetailId;
            InitRubrique(model.RubriqueId, model.SousRubriqueId);
            return this;
        }

        public override void SaveToModel()
        {
            var rubriqueId = SelectedRubrique == null ? 0 : SelectedRubrique.Id;
            var sousrubriqueId = SelectedSousRubrique == null ? 0 : SelectedSousRubrique.Id;
            Model.Commentaire = Commentaire;
            Model.Id = Id;
            Model.Montant = Montant;
            Model.OperationId = OperationId;
            Model.RubriqueId = rubriqueId;
            Model.SousRubriqueId = sousrubriqueId;
            Model.LienDetailId = DetailLienId;
        }

        public override DetailViewModel DuplicateViewModel()
        {
            var vm = Container.Resolve<DetailViewModel>();

            vm.IsNew = true;
            vm.IsModified = true;
            vm.Commentaire = Commentaire;
            vm.Montant = Montant;
            vm.SelectedRubrique = SelectedRubrique;
            vm.SelectedSousRubrique = SelectedSousRubrique;
            vm.OperationId = OperationId;
            return vm;
        }

        public override void Copier()
        {
            LogMessage("détail copié "+ ToString());
            CopyManager.CopieDetail = this;
            base.Copier();
        }

        public override string ToString()
        {
            return string.Format("{0}/{1} {2}",
                SelectedRubrique == null? "" : SelectedRubrique.Libelle,
                SelectedSousRubrique == null ? "" : SelectedSousRubrique.Libelle,
                Montant);
        }
        public override void UpdateProperties()
        {
            //nothing to do
        }

        public override void ActionSauvegarder()
        {
            ModelServiceBase.BeginTransaction();
            bool wasNew = IsNew;
            base.ActionSauvegarder();
            //si c'est une opération liée et que le détail vient d'être créé -> ajouter le détail lié sur l'opération liée
            //la modification est traitée dans DetailManager
            if (wasNew && OperationLienId != null)
            {
                //créer le détail dans l'opération liée
                var linkedDetail = new DetailModel();
                ModelServiceBase.CopyTo(linkedDetail, Model);
                linkedDetail.LienDetailId = Model.Id;
                linkedDetail.OperationId = OperationLienId.Value;
                ModelServiceBase.CreateItem(linkedDetail);
                Model.LienDetailId = linkedDetail.Id;
            }
            ModelServiceBase.EndTransaction();
            WpfIocFactory.Instance.MainVm.ReloadCompteForOperation(OperationLienId);
        }
    }
}