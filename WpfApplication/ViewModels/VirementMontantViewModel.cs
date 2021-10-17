using System;
using CommonLibrary.IOC;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace MaCompta.ViewModels
{
    public class VirementMontantViewModel : ModelViewModelBase<VirementMontantViewModel, VirementMontantModel>
    {
        #region Constructeur
        public VirementMontantViewModel(IContainer container, IVirementDetailMontantService service) 
            : base(container, service)
        {
            ModelName = "Virement Montant";
            Model = new VirementMontantModel();
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Numéro du mois associé
        /// </summary>
        public int NumeroMois { get; set; }

        private decimal _montant;
        /// <summary>
        /// montant
        /// </summary>
        public decimal Montant
        {
            get { return _montant; }
            set
            {
                _montant = value;
                RaisePropertyChangedWithModification(vm => vm.Montant);
                RaiseMontantUpdated();
            }
        }

        private long _detailId;

        /// <summary>
        /// identifiant du détail
        /// </summary>
        public long DetailId
        {
            get { return _detailId; }
            set { _detailId = value;
            RaisePropertyChanged(vm => vm.DetailId);
            }
        }

        #endregion

        #region Méthodes

        ///// <summary>
        ///// Initialisation du view model à partir d'un objet model
        ///// </summary>
        ///// <param name="model"></param>
        //public override void CancelModel(VirementMontantModel model)
        //{
        //    throw new NotImplementedException();
        //}

        public override VirementMontantViewModel InitFromModel(VirementMontantModel model)
        {
            Id = model.Id;
            Montant = model.Montant;
            DetailId = model.VirementDetailId;
            NumeroMois = model.Mois;
            IsNew = false;
            return this;
        }
        public override void SaveToModel()
        {
            //return new VirementMontantModel
                       {
                Model.Id = Id;
                Model.Montant = Montant;
                Model.VirementDetailId = DetailId;
                Model.Mois = NumeroMois;
                       }
        }

        public override VirementMontantViewModel DuplicateViewModel()
        {
            throw new NotImplementedException();
        }
        public override void UpdateProperties()
        {
            //nothing to do
        }
        #endregion
    }
}
