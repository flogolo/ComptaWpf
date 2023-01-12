using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommonLibrary.IOC;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;
using MaCompta.Commands;
using MaCompta.Tools;

namespace MaCompta.ViewModels
{
    public class OperationViewModel : ModelViewModelBase<OperationViewModel, OperationModel>
    {
        #region Constructor
        /// <summary>
        /// Constructeur
        /// </summary>
        public OperationViewModel(IContainer container, IOperationService opSrv)
            : base(container, opSrv)
        {
            _isReport = false;
            DetailsList = new ObservableCollection<DetailViewModel>();
            //DetailsList.CollectionChanged += OnDetailSaved;
            ModelName = "Opération";
            Model = new OperationModel();
        }

        public OperationViewModel()
        {
            // TODO: Complete member initialization
            DetailsList = new ObservableCollection<DetailViewModel>();
        }
        #endregion

        #region Properties
        public string OperationColor
        {
            get {
                if (IsModified || IsDetailModified) return "#FFFFD9CC";
                if (IsReport) return "Silver";
                if (IsVirementAuto) return "PowderBlue";
                
                return "White";
            }
        }

        public string ReportText
        {
            get { return IsReport ? "Réintégrer" : "Reporter"; }
        }
        private bool _isReport;

        /// <summary>
        /// Sets and gets the Report property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsReport
        {
            get
            {
                return _isReport;
            }

            set
            {
                _isReport = value;
                RaisePropertyChangedWithModification(vm=>vm.IsReport);
                RaisePropertyChanged(vm => vm.ReportText);
                RaisePropertyChanged(vm => OperationColor);
            }
        }

        private bool _isModified;
        public override bool IsModified
        {
            get { return _isModified; }
            set
            {
                _isModified = value;
                RaisePropertyChanged(vm => vm.IsModified);
                RaisePropertyChanged(vm => vm.OperationColor);
            }
        }

        public bool IsVirementAuto { get; private set; }


        ///// <summary>
        ///// Indique que l'opération est filtrée
        ///// </summary>
        //public bool IsFiltered { get; set; }

        /// <summary>
        /// Indique qu'un des détails est modifié
        /// </summary>
        public bool IsDetailModified
        {
            get
            {
                //si un des détails est modifié
                return DetailsList.Any(d => d.IsModified);
            }
        }
        ///// <summary>
        ///// Chèque associé
        ///// </summary>
        //public ChequeViewModel ChequeVm { get; set; }

        /// <summary>
        /// Numéro du compte associé
        /// </summary>
        public long CompteId { get; set; }

        /// <summary>
        /// Identitifant de l'opération liée
        /// </summary>
        public long? LienId { get; set; }

        /// <summary>
        /// Montant de l'opération
        /// </summary>
        public decimal Montant
        {
            //total des détails
            get { return DetailsList.Sum(v => v.Montant); }
        }
        
        /// <summary>
        /// Date de l'opération
        /// </summary>
        private DateTime? _dateOperation;
        public DateTime? DateOperation
        {
            get { return _dateOperation; }
            set {
                if (value == _dateOperation)
                    return;

                _dateOperation = GetRealDateTime(value);
                RaisePropertyChangedWithModification(vm => vm.DateOperation);
            }
        }

        /// <summary>
        /// Date de validation
        /// </summary>
        private DateTime? _dateValidation;
        public DateTime? DateValidation
        {
            get { return _dateValidation; }
            set {
                if (value == _dateValidation)
                    return;

                _dateValidation = GetRealDateTime(value);
                RaisePropertyChangedWithModification(vm => vm.DateValidation);
                RaisePropertyChanged(vm => vm.IsValidated);
                if (value != null)
                {
                    IsReport = false;
                }
            }
        }
        /// <summary>
        /// Indique que l'opération est validée
        /// </summary>
        public bool IsValidated
        {
            get { return DateValidation != null; }
        }

        private DateTime? GetRealDateTime(DateTime? date)
        {
            if (date != null)
            {
                var newDate = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day,
                    DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                return newDate;
            }
            else {
                return date;
            }
        }
        /// <summary>
        /// Date de validation partielle
        /// </summary>
        private DateTime? _dateValidationPartielle;
        public DateTime? DateValidationPartielle
        {
            get { return _dateValidationPartielle; }
            set {
                if (value == _dateValidationPartielle)
                    return;

                _dateValidationPartielle = GetRealDateTime(value);
                RaisePropertyChangedWithModification(vm => vm.DateValidationPartielle);
                if (value != null)
                {
                    IsReport = false;
                }
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

        public string Poste
        {
            get {
                return DetailsList.Count() == 0? string.Empty:
                    (DetailsList.Count() > 1 ?
                    "Ventilé sur " + DetailsList.Count() + " postes"
                    :
                    DetailsList.Count()==1 && DetailsList.First().SelectedRubrique !=null ? 
                        DetailsList.First().SelectedRubrique.Libelle + "/" 
                        + (DetailsList.First().SelectedSousRubrique != null ? 
                        DetailsList.First().SelectedSousRubrique.Libelle : string.Empty): 
                        string.Empty);
                }
        }


        //private EnumPaiement m_TypePaiement;
        //public EnumPaiement TypePaiement
        //{
        //    get { return m_TypePaiement; }
        //    set { m_TypePaiement = value;
        //        RaisePropertyChanged(vm => vm.TypePaiement);
        //        RaisePropertyChanged(vm => vm.SelectedPaiement);
        //    }
        //}

        ////private string m_SelectedPaiement;

        //public string SelectedPaiement
        //{
        //    get
        //    {
        //        string paiement;
        //        TypesPaiement.TryGetValue((int)TypePaiement, out paiement);
        //        return paiement;
        //    }
        //    set
        //    {
        //        foreach (var paiement in TypesPaiement)
        //        {
        //            if (paiement.Value.Equals(value))
        //                TypePaiement = (EnumPaiement)paiement.Key;
        //        }
        //    }
        //}

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
            set {
                if (value != null && value != _selectedPaiement)
                {
                    _selectedPaiement = value;
                    RaisePropertyChangedWithModification(vm => SelectedPaiement);
                    if (!PaiementHelper.IsCheque(SelectedPaiement) && !string.IsNullOrEmpty(NumeroCheque))
                        NumeroCheque = String.Empty;
                    RaisePropertyChangedWithModification(vm => vm.NumeroCheque);
                    RaisePropertyChanged(vm => vm.IsCheque);
                }
            }
        }

        private string _numeroCheque;
        public String NumeroCheque
        {
            get { return _numeroCheque; }
            set
            {
                if (value != null)
                {
                    _numeroCheque = value;
                    if (value.Length == 3)
                    {
                        //recherche du numéro suivant
                        _numeroCheque = ((IOperationService) ModelServiceBase).FindCheque(value);
                    }
                    RaisePropertyChangedWithModification(vm => vm.NumeroCheque);
                }
            }
        }

        /// <summary>
        /// Indique que c'est un paiement par chèque
        /// </summary>
        public bool IsCheque
        {
            get
            {
                if (_selectedPaiement != null)
                    return PaiementHelper.IsCheque(_selectedPaiement);
                return false;
            }
        }

        public ObservableCollection<String> Ordres
        {
            get { return ((IOperationService) ModelServiceBase).AllOrdres; }
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
        /// <summary>
        /// Liste des détails
        /// </summary>
        public ObservableCollection<DetailViewModel> DetailsList { get; private set; }
        #endregion

        #region Methods
        public override void ActionSauvegarder()
        {
            bool creeDetail = IsNew;

            base.ActionSauvegarder();

            if (creeDetail)
            {
                //en cas de nouvelle opération -> on crée un détail par défaut
                ActionAjouter();
            }
            else
            {
                //en cas de modification -> on sauvegarde aussi les détails
                foreach (var detail in DetailsList)
                {
                    detail.ActionSauvegarder();
                }
            }
            
            RaiseMontantUpdated();
            RaiseSavedEvent();
        }

        /// <summary>
        /// Ajout d'une ligne de détail
        /// </summary>
        public override void ActionAjouter()
        {
            var vm = Container.Resolve<DetailViewModel>();
            vm.IsNew = true;
            vm.OperationId = Id;
            vm.OperationLienId = LienId;

            //IsExpanded = true;
            InitEvent(vm);
            DetailsList.Add(vm);
            Model.Details.Add(vm.Model);
            RaisePropertyChanged(v => v.Poste);
        }

        /// <summary>
        /// Appelé quand un détail est supprimé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDetailDeleted(object sender, EventArgs<DetailModel> e)
        {
            var vmodel = sender as DetailViewModel;
            if (vmodel != null)
            {
                ClearEvent(vmodel);
                DetailsList.Remove(vmodel);
                if (vmodel.Id > 0 && e.Data != null)
                {
                    Model.Details.Remove(e.Data);
                }
                updateOperationProperties();
            }
        }

        #endregion

        public override OperationViewModel InitFromModel(OperationModel model)
        {
            Model = model;

            CompteId = model.CompteId;
            Id = model.Id;
            LienId = model.LienOperationId;
            IsReport = model.Report;
            IsVirementAuto = model.IsVirementAuto;
            RaisePropertyChanged(vm => vm.IsVirementAuto);
            //pour ne pas changer l'heure
            _dateOperation = model.DateOperation;
            _dateValidation = model.DateValidation;
            _dateValidationPartielle = model.DateValidationPartielle;
            RaisePropertyChanged(vm => vm.DateOperation);
            RaisePropertyChanged(vm => vm.DateValidationPartielle);
            RaisePropertyChanged(vm => vm.DateValidation);
            Ordre = model.Ordre;
            SelectedPaiement = PaiementHelper.GetPaiement(model.TypePaiement);
            NumeroCheque = model.NumeroCheque;

            if (model.Details != null)
            {
                DetailsList.Clear();
                foreach (var detail in model.Details)
                {
                    var detailVm = Container.Resolve<DetailViewModel>().InitFromModel(detail);
                    detailVm.OperationLienId = model.LienOperationId;
                    InitEvent(detailVm);
                    DetailsList.Add(detailVm);
                }
            }
            RaisePropertyChanged(v => v.Poste);
            return this;
        }

        public override void SaveToModel()
        {
            
            Model.Id = Id;
            Model.LienOperationId = LienId;
            Model.DateOperation = DateOperation;
            Model.DateValidation = DateValidation;
            Model.DateValidationPartielle = DateValidationPartielle;
            Model.CompteId = CompteId;
            Model.Ordre = Ordre;
            Model.TypePaiement = PaiementHelper.GetCodePaiement(SelectedPaiement);
            //Details = new List<DetailModel>(),
            Model.NumeroCheque = NumeroCheque;
            Model.Report = IsReport;

            foreach (var detail in DetailsList)
            {
                detail.SaveToModel();
            }

        }

        public override OperationViewModel DuplicateViewModel()
        {
            var model = new OperationModel
            {
                //Cheque = new ChequeModel {Numero = NumeroCheque},
                NumeroCheque = NumeroCheque,
                CompteId = CompteId,
                DateOperation = DateTime.Today,
                Ordre = Ordre,
                TypePaiement = PaiementHelper.GetCodePaiement(SelectedPaiement),
                Details = new List<DetailModel>(
                    from detail in DetailsList
                    select new DetailModel
                    {
                        Commentaire = detail.Commentaire,
                        Montant = detail.Montant,
                        RubriqueId = detail.SelectedRubrique.Id,
                        SousRubriqueId = detail.SelectedSousRubrique.Id
                    })
            };
            if (IsCheque)
            {
                model.NumeroCheque = "remplir";
            }

            ((IOperationService) ModelServiceBase).CreateOperationWithDetails(model);
            var opVm = Container.Resolve<OperationViewModel>().InitFromModel(model);
            return opVm;
        }


        private void OnDetailSaved(object sender, EventArgs<DetailModel> e)
        {
            //si le détail n'existe pas -> l'ajouter au modèle
            if (Model.Details.FirstOrDefault(d=>d.Id == e.Data.Id) == null)
                Model.Details.Add(e.Data);
            updateOperationProperties();
        }

        /// <summary>
        /// Mise à jour des prorpiétés d'IHM liées à l'opération
        /// </summary>
        public void updateOperationProperties()
        {
            RaisePropertyChanged(op => op.Poste);
            RaisePropertyChanged(op => op.Montant);
            RaiseMontantUpdated();

        }

        private void InitEvent(DetailViewModel vm)
        {
            vm.PropertyChanged += OnDetailPropertyChanged;
            //vm.MontantUpdated += OnMontantUpdated;
            vm.ViewModelSaved += OnDetailSaved;
            vm.ViewModelDeleted += OnDetailDeleted;
            vm.ViewModelDuplicated += OnDetailDuplicated;
        }


        void OnDetailPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsModified")
            {
                RaisePropertyChanged(vm => vm.IsDetailModified);
                RaisePropertyChanged(vm => OperationColor);
                RaisePropertyChanged(vm => vm.Poste);
            }
        }

        private void ClearEvent(DetailViewModel vm)
        {
            vm.ViewModelSaved -= OnDetailSaved;
            vm.ViewModelDeleted -= OnDetailDeleted;
            vm.ViewModelDuplicated -= OnDetailDuplicated;
            vm.PropertyChanged -= OnDetailPropertyChanged;
        }

        private void OnDetailDuplicated(object sender, EventArgs e)
        {
            var detailVm = sender as DetailViewModel;
            if (detailVm != null)
            {
                InitEvent(detailVm);
                Model.Details.Add(detailVm.Model);
                DetailsList.Add(detailVm);
                IsModified = true;
            }
            updateOperationProperties();
        }

        public void Valider()
        {
            if (DateValidationPartielle != null)
            {
                DateValidation = DateValidationPartielle;
                //sauvegarder
                ActionSauvegarder();
            }
            else
                DateValidation = DateOperation;
            //RaiseMontantUpdate();
        }

        public void ValiderPartiel()
        {
            DateValidationPartielle = DateOperation;
            //RaiseMontantUpdate();
        }

        /// <summary>
        /// Suppression de l'opération
        /// </summary>
        public override void Supprimer(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (!IsNew)
                {
                    LogMessage("Opération en cours de suppression...");
                    //supprimer les détails associés
                    var tmplist = new List<DetailViewModel>(DetailsList);
                    foreach (var detailViewModel in tmplist)
                    {
                        detailViewModel.Supprimer(MessageBoxResult.Yes);
                    }
                    ModelServiceBase.DeleteItem(Id, false);
                }
                ////supprimer ordre si il n'apparait qu'une seule fois
                //var ordre = Ordres.Where(o => o == Ordre);
                //if (ordre != null && ordre.Count() == 1)
                //    Ordres.Remove(ordre.First());

                LogMessage("Opération supprimée "+ToString());
                RaiseDeletedEvent();
            }
        }

        #region Commande Valider
        private ICommand _actionValiderCommand;

        /// <summary>
        /// Commande d'ajout
        /// </summary>
        public ICommand ActionValiderCommand
        {
            get {
                return _actionValiderCommand ??
                       (_actionValiderCommand = new RelayCommand(param => ActionValider(Boolean.Parse((string) param))));
            }
        }

        private void ActionValider(bool isPartiel)
        {
            IsReport = false;
            if (isPartiel)
                ValiderPartiel();
            else
                Valider();
        }
        #endregion

        #region Commande Reporter
        private ICommand _actionReporterCommand;
        public ICommand ActionReporterCommand
        {
            get
            {
                return _actionReporterCommand ??
                       (_actionReporterCommand = new RelayCommand(param => ActionReporter()));
            }
        }

        private void ActionReporter()
        {
            IsReport = !IsReport;
            //sauvegarder
            ActionSauvegarder();
        }
        #endregion

        /// <summary>
        /// indique si l'opération appartient au filter
        /// </summary>
        public bool FilterDate(EnumOperationFilter filter, DateTime? filtreDate1, DateTime? filtreDate2)
        {
            //if (filtreDate1 == null || filtreDate2 == null)
            //    return true;
            
            switch (filter)
            {
                case EnumOperationFilter.Valide:
                    if (DateValidation != null)
                    {
                        if (filtreDate1 == null || filtreDate2 == null)
                            return true;

                        if (DateValidation >= filtreDate1
                            && DateValidation <= filtreDate2)
                            return true;
                        else
                            return false;

                    }
                    break;
                case EnumOperationFilter.EnCours:
                    if (DateValidation == null)
                    {
                        if (filtreDate1 == null || filtreDate2 == null)
                            return true;

                        if (DateOperation >= filtreDate1
                            && DateOperation <= filtreDate2)
                            return true;
                        else
                            return false;
                    }
                    break;
                case EnumOperationFilter.Partiel:
                    if (DateValidation == null && DateValidationPartielle != null)
                    {
                        if (filtreDate1 == null || filtreDate2 == null)
                            return true;

                        if (DateValidationPartielle >= filtreDate1
                            && DateValidationPartielle <= filtreDate2)
                            return true;
                        else
                            return false;
                    }
                    break;
                case EnumOperationFilter.NonPartiel:
                    if (DateValidation == null && DateValidationPartielle == null && !IsReport)
                    {
                        if (filtreDate1 == null || filtreDate2 == null)
                            return true;

                        if (DateOperation >= filtreDate1
                            && DateOperation <= filtreDate2)
                            return true;
                        else
                            return false;
                    }
                    break;
                case EnumOperationFilter.Tout:
                    if (filtreDate1 == null || filtreDate2 == null)
                        return true;

                    if (DateOperation >= filtreDate1
                            && DateOperation <= filtreDate2)
                        return true;
                    else
                        return false;
            }
            return false;
        }

        internal bool FilterOrdre(IEnumerable<string> ordres)
        {
            if (ordres.Contains(Ordre))
                return true;
            return false;
        }

        internal bool FilterRubrique(IEnumerable<string> enumerable)
        {
            foreach (var item in enumerable)
            {
                var rubriques = item.Split('/');
                var rubrique = rubriques[0].Trim();
                string sousRubrique = string.Empty;
                if (rubriques.Length > 1)
                    sousRubrique = rubriques[1].Trim();
                foreach (var detail in DetailsList)
                {
                    if(string.IsNullOrEmpty(sousRubrique) && detail.SelectedRubrique!=null && detail.SelectedRubrique.Libelle.Equals(rubrique)
                        || !string.IsNullOrEmpty(sousRubrique) && detail.SelectedRubrique != null && detail.SelectedRubrique.Libelle.Equals(rubrique)
                        && detail.SelectedSousRubrique !=null && detail.SelectedSousRubrique.Libelle.Equals(sousRubrique))
                        return true;
                }
            }
            return false;
        }



        internal bool FilterOrdre(string selectedOrdre)
        {
 //           if (IsFiltered)
   //         {
                ////déjà filtré sur la date
                if (Ordre == selectedOrdre)
                    return true;
                return false;
     //       }
       //     return false;
        }

        internal bool FiltrerType(IEnumerable<string> selectedType)
        {
            if (selectedType.Contains(SelectedPaiement))
                return true;
            return false;
        }

        public override void ActionColler()
        {
            //coller le détail copié
            if (CopyManager.CopieDetail != null)
            {
                var detailVm = CopyManager.CopieDetail.DuplicateViewModel();
                detailVm.OperationId = Id;
                InitEvent(detailVm);
                DetailsList.Add(detailVm);
                //ajouter au model
                //var modelDetail = detailVm.SaveToModel();
                Model.Details.Add(detailVm.Model);
                IsModified = true;
                updateOperationProperties();    
            }
            base.ActionColler();
        }

        public override string ToString()
        {
            return Model != null ? string.Format("#{0} {1} {2}", Model.Id, Model.Ordre, Montant) : string.Empty;
        }
        public override void UpdateProperties()
        {
            //nothing to do
        }
    }
}