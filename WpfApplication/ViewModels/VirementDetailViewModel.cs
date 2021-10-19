using System;
using System.Collections.ObjectModel;
using CommonLibrary.IOC;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace MaCompta.ViewModels
{
    public class VirementDetailViewModel : ModelViewModelBase<VirementDetailViewModel, VirementDetailModel>
    {

        #region Membres
        private RubriqueViewModel _selectedRubrique;
        private SousRubriqueViewModel _selectedSousRubrique;
        private string _commentaire;
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        public VirementDetailViewModel(IContainer container ,IVirementDetailService detailSrv)
            : base(container, detailSrv)
        {
            //m_DetailSrv = detailSrv;
            ModelName = "Virement Détail";
            Montants = new Collection<VirementMontantViewModel>();
            Model = new VirementDetailModel();
        }

        #region Propriétés

        private EnumDestinataire _destinataire;

        public EnumDestinataire Destinataire
        {
            get
            {
                //System.Diagnostics.Debug.WriteLine("get destinataire " + m_Destinataire);
                return _destinataire;
            }
            set
            {
                //System.Diagnostics.Debug.WriteLine("set destinataire " + value);
                _destinataire = value;
                RaisePropertyChangedWithModification(vm => vm.Destinataire);
            }
        }
        /// <summary>
        /// identifiant du virement
        /// </summary>
        public long VirementId { get; set; }

        /// <summary>
        /// Rubrique sélectionnée
        /// </summary>
        public RubriqueViewModel SelectedRubrique
        {
            get { return _selectedRubrique; }
            set
            {
                _selectedRubrique = value;
                RaisePropertyChangedWithModification(vm => vm.SelectedRubrique);
                RaisePropertyChanged(vm => vm.SousRubriques);
            }
        }

        /// <summary>
        /// sous-rubrique sélectionnée
        /// </summary>
        public SousRubriqueViewModel SelectedSousRubrique
        {
            get { return _selectedSousRubrique; }
            set
            {
                _selectedSousRubrique = value;
                RaisePropertyChangedWithModification(vm => vm.SelectedSousRubrique);
            }
        }

        /// <summary>
        /// Commentaire
        /// </summary>
        public String Commentaire
        {
            get { return _commentaire; }
            set
            {
                _commentaire = value;
                RaisePropertyChangedWithModification(vm => vm.Commentaire);
            }
        }
        /// <summary>
        /// Liste des rubriques
        /// </summary>
        public ObservableCollection<RubriqueViewModel> Rubriques
        {
            get
            {
                return WpfIocFactory.Instance.Rubriques;
            }
        }
        /// <summary>
        /// liste des sous-rubriques
        /// </summary>
        public ObservableCollection<SousRubriqueViewModel> SousRubriques
        {
            get
            {
                if (SelectedRubrique != null)
                    return SelectedRubrique.SousRubriques;
                return null;
            }
        }

        public Collection<VirementMontantViewModel> Montants { get; private set; }
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
        //public override void CancelModel(VirementDetailModel model)
        //{
        //    InitFromModel(model);
        //}

        /// <summary>
        /// Initialisation du view model à partir d'un objet model
        /// </summary>
        /// <param name="model"></param>
        public override VirementDetailViewModel InitFromModel(VirementDetailModel model)
        {
            Model = model;
            Id = model.Id;
            Commentaire = model.Commentaire;
            InitRubrique(model.RubriqueId, model.SousRubriqueId);
            VirementId = model.VirementId;
            Destinataire = model.IsCompteSrcOnly? EnumDestinataire.SourceOnly : (model.IsCompteDstOnly? EnumDestinataire.DestinataireOnly : EnumDestinataire.LesDeux);
            //IsCompteDstOnly = model.IsCompteDstOnly;
            //IsCompteSrcOnly = model.IsCompteSrcOnly;
            //IsTousComptes = !IsCompteDstOnly && !IsCompteSrcOnly;
            return this;
        }

        /// <summary>
        /// renvoie un objet model initialisé à partir du view model
        /// </summary>
        /// <returns></returns>
        public override void SaveToModel()
        {
           // return new VirementDetailModel
                       {
                Model.Commentaire = Commentaire;
                Model.Id = Id;
                Model.RubriqueId = SelectedRubrique.Id;
                Model.SousRubriqueId = SelectedSousRubrique.Id;
                Model.VirementId = VirementId;
                Model.IsCompteSrcOnly = Destinataire == EnumDestinataire.SourceOnly;
                Model.IsCompteDstOnly = Destinataire == EnumDestinataire.DestinataireOnly;
                       }
        }

        public override VirementDetailViewModel DuplicateViewModel()
        {
            var vm = Container.Resolve<VirementDetailViewModel>();

            vm.IsNew = true;
            vm.Commentaire = Commentaire;
            vm.SelectedRubrique = SelectedRubrique;
            vm.SelectedSousRubrique = SelectedSousRubrique;
            vm.VirementId = VirementId;
            vm.IsModified = true;
            return vm;
        }

        public override void ActionSauvegarder()
        {
            LogMessage("Detail en cours de sauvegarde...");
            if (IsNew)
            {
                SaveToModel();
                ModelServiceBase.CreateItem(Model);
                LogMessage("Detail créé");
                Id = Model.Id;
                IsNew = false;
                IsModified = false;
                //mise à jour de l'identifiant du detail pour chaque montant
                foreach (var montant in Montants)
                {
                    montant.DetailId = Id;
                }
            }
            else
            {
                SaveToModel();
                ModelServiceBase.UpdateItem(Model);
                LogMessage("Detail mis à jour");
                IsModified = false;
            }
        }

        public override void UpdateProperties()
        {
            //nothing to do
        }
    }

    public enum EnumDestinataire
    {
        LesDeux = 0,
        DestinataireOnly = 1,
        SourceOnly = 2,
    }
}
