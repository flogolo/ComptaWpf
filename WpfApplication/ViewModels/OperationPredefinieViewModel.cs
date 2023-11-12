using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommonLibrary.IOC;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;

namespace MaCompta.ViewModels
{
    public class OperationPredefinieViewModel : ModelViewModelBase<OperationPredefinieViewModel, OperationPredefinieModel>, IComparable<OperationPredefinieViewModel>
    {
        private readonly IOperationService _operationService;

        public OperationPredefinieViewModel(IContainer container, IOperationPredefinieService opPredSrv, IOperationService opSrv)
            : base(container, opPredSrv)
        {
            _operationService = opSrv;
            Model = new OperationPredefinieModel();
            ModelName = "Opération prédéfinie";
        }

        #region Properties
        /// <summary>
        /// Champ d'affichage dans une liste
        /// </summary>
        public string Description
        {
            get
            {
                if (IsNew)
                    return "Nouvelle opération";

                //pas encore chargé
                if (SelectedRubrique == null)
                    SelectedRubrique = WpfIocFactory.Instance.GetRubrique(Model.RubriqueId);
                if (SelectedSousRubrique == null)
                    SelectedSousRubrique = WpfIocFactory.Instance.GetSousRubrique(_selectedRubrique, Model.SousRubriqueId);

                if (SelectedRubrique != null && SelectedSousRubrique != null)
                    return String.Format("{0}/{1} ({2})", SelectedRubrique.Libelle, SelectedSousRubrique.Libelle, Ordre);
                return "Description NOK";
            }
        }

        /// <summary>
        /// Ordre
        /// </summary>
        private string _ordre;
        public String Ordre
        {
            get { return _ordre; }
            set
            {
                if (value == _ordre)
                    return;

                _ordre = value;
                RaisePropertyChangedWithModification(vm => vm.Ordre);
            }
        }

        /// <summary>
        /// Type de paiement sélectionné
        /// </summary>
        private string _selectedPaiement;
        public string SelectedPaiement
        {
            get
            {
                //System.Diagnostics.Debug.WriteLine("get selectedpaiement " + m_SelectedPaiement);
                return _selectedPaiement;
            }
            set
            {
                if (value != null && value != _selectedPaiement)
                {
                    _selectedPaiement = value;
                    RaisePropertyChangedWithModification(vm => SelectedPaiement);
                }
            }
        }

        public ObservableCollection<String> Ordres
        {
            get { return _operationService.AllOrdres; }
        }

        /// <summary>
        /// types de paiement possibles
        /// </summary>
        public static IEnumerable<string> TypesPaiement
        {
            get
            {
                return PaiementHelper.TypesPaiement;
            }
        }
        private RubriqueViewModel _selectedRubrique;
        public RubriqueViewModel SelectedRubrique
        {
            get { return _selectedRubrique; }
            set
            {
                _selectedRubrique = value;
//                if (value != null) System.Diagnostics.Debug.WriteLine("prédéfinie - set rubrique " + value.Libelle);
  //              else System.Diagnostics.Debug.WriteLine("prédéfinie - set rubrique null");
                RaisePropertyChangedWithModification(vm => vm.SelectedRubrique);
                RaisePropertyChanged(vm => vm.SousRubriques);
                //RaisePropertyChanged(vm => vm.Description);
            }
        }

        private SousRubriqueViewModel _selectedSousRubrique;
        public SousRubriqueViewModel SelectedSousRubrique
        {
            get { return _selectedSousRubrique; }
            set
            {
                if (value != null)
                {
                    _selectedSousRubrique = value;
                    //System.Diagnostics.Debug.WriteLine("prédéfinie - set sous-rubrique " + value.Libelle);
                    RaisePropertyChangedWithModification(vm => vm.SelectedSousRubrique);
                    RaisePropertyChanged(vm => vm.Description);
                }
            }
        }

        public ObservableCollection<RubriqueViewModel> Rubriques
        {
            get { return WpfIocFactory.Instance.Rubriques; }
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


        private string _commentaire;
        public String Commentaire
        {
            get { return _commentaire; }
            set
            {
                _commentaire = value;
                RaisePropertyChangedWithModification(vm => vm.Commentaire);
            }
        }
        #endregion

        public override OperationPredefinieViewModel InitFromModel(OperationPredefinieModel model)
        {
            Model = model;
            Id = model.Id;
            Ordre = model.Ordre;
            Commentaire = model.Commentaire;
            SelectedPaiement = PaiementHelper.GetPaiement(model.TypePaiement);
            SelectedRubrique = WpfIocFactory.Instance.GetRubrique(model.RubriqueId);
            SelectedSousRubrique = WpfIocFactory.Instance.GetSousRubrique(_selectedRubrique, model.SousRubriqueId);
            return this;
        }

        public override void SaveToModel()
        {
            //var operation = new OperationPredefinieModel
            {
                Model.Id = Id;
                Model.Ordre = Ordre;
                Model.TypePaiement = PaiementHelper.GetCodePaiement(SelectedPaiement);
                Model.RubriqueId = SelectedRubrique.Id;
                Model.SousRubriqueId = SelectedSousRubrique.Id;
                Model.Commentaire = Commentaire;
            };
            //return operation;
        }

        /// <summary>
        /// Duplication du view model hors Id
        /// </summary>
        /// <returns></returns>
        public override OperationPredefinieViewModel DuplicateViewModel()
        {
            throw new NotImplementedException("OperationPredefineVieModel => DuplicateViewModel");
        }

        public override string ToString()
        {
            return Description;
        }

        public int CompareTo(OperationPredefinieViewModel other)
        {
            return ToString().CompareTo(other.ToString());
        }

        public override void UpdateProperties()
        {
            RaisePropertyChanged(vm => vm.Description);
        }
    }
}