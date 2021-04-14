using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;
using IContainer = CommonLibrary.IOC.IContainer;

namespace MaCompta.ViewModels
{
    public class VirementViewModel : ModelViewModelBase<VirementViewModel, VirementModel>
    {
        #region Membres
        private int _jour;
        private int _duree;
        private string _ordre;
        private CompteViewModel _compteSrc;
        private CompteViewModel _compteDst;
        private DateTime? _dateDernierVirement;
        private FrequenceEnum _frequence;
        private Decimal _montant;
        //private string m_Description;
        //private readonly IVirementService m_VirementSrv;
        private readonly IVirementDetailMontantService _montantSrv;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructeur
        /// </summary>
        public VirementViewModel(IContainer container, IVirementService virementSrv, IVirementDetailMontantService montantSrv)
            :base(container, virementSrv)
        {
            ModelName = "Virement";
            _montantSrv = montantSrv;
            Model = new VirementModel();

            MoisList = new ObservableCollection<VirementMoisViewModel>();
            for (int i = 0; i < 12; i++)
            {
                var mois = container.Resolve<VirementMoisViewModel>();
                mois.Init(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[i], i+1);
                if (i == DateTime.Today.Month-1)
                    SelectedMois = mois;
                MoisList.Add(mois);
            }
            DetailsList = new ObservableCollection<VirementDetailViewModel>();
        } 

        #endregion

        #region Properties
        public string Poste { get { return string.Empty; } }

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
        /// Type de paiement sélectionné
        /// </summary>
        private string _selectedTypePaiement;
        public string SelectedTypePaiement
        {
            get
            {
                return _selectedTypePaiement;
            }
            set
            {
                if (value != null && value != _selectedTypePaiement)
                {
                    _selectedTypePaiement = value;
                    RaisePropertyChangedWithModification(vm => SelectedTypePaiement);
                }
            }
        }

        public bool IsMontantVisible
        {
            get { return Montant > 0; }
        }
        public IEnumerable<String> DayValues
        {
            get { return CultureInfo.CurrentUICulture.DateTimeFormat.DayNames; }
        }
        public IEnumerable<String> MonthValues
        {
            get { return CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames; }
        }
        /// <summary>
        /// Gets the Montant property.
        /// </summary>
        public decimal Montant
        {
            get
            {
                return _montant;
            }

            set
            {
                if (_montant == value)
                {
                    return;
                }

                _montant = value;

                // Update bindings, no broadcast
                RaisePropertyChangedWithModification(vm => vm.Montant);
            }
        }
        /// <summary>
        /// Liste des valeurs possibles pour la fréquence
        /// </summary>
        public IEnumerable<FrequenceEnum> FrequenceValues
        {
            get
            {
                return Enum.GetValues(typeof(FrequenceEnum)).Cast<FrequenceEnum>();
            }
        }

        public FrequenceEnum Frequence
        {
            get
            {
                return _frequence;
            }

            set
            {
                if (_frequence == value)
                {
                    return;
                }

                _frequence = value;

                RaisePropertyChangedWithModification(vm => vm.Frequence);
            }
        }
        ///// <summary>
        ///// Description du virement
        ///// </summary>
        //public string Description
        //{
        //    get { return m_Description; }
        //    set { m_Description = value;
        //    RaisePropertyChangedWithModification(vm => vm.Description);
        //    }
        //}

        /// <summary>
        /// ordres possibles
        /// </summary>
        public ObservableCollection<String> Ordres
        {
            get
            {
                return ((IVirementService) ModelServiceBase).AllOrdres;
            }
        }
        /// <summary>
        /// Détails associés
        /// </summary>
        public ObservableCollection<VirementDetailViewModel> DetailsList { get; private set; }
        /// <summary>
        /// Mois associés
        /// </summary>
        public ObservableCollection<VirementMoisViewModel> MoisList { get; private set; }

        private VirementMoisViewModel _selectedMois;
        public VirementMoisViewModel SelectedMois
        {
            get { return _selectedMois; } 
            set { _selectedMois = value;
            RaisePropertyChanged(vm => vm.SelectedMois);
            } 
        }

        /// <summary>
        /// Jour du virement
        /// </summary>
        public int Jour
        {
            get { return _jour; }
            set { _jour = value;
            RaisePropertyChangedWithModification(vm => vm.Jour);
            }
        }
        /// <summary>
        /// Durée du virement (-1 pour infinie)
        /// </summary>
        public int Duree
        {
            get { return _duree; }
            set { _duree = value;
            RaisePropertyChangedWithModification(vm => vm.Duree);
            }
        }

        /// <summary>
        /// Ordre du virement
        /// </summary>
        public string Ordre
        {
            get { return _ordre; }
            set
            {
                //System.Diagnostics.Debug.WriteLine("VirementViewModel - set ordre " + value);
                if (value != _ordre)
                {
                    //System.Diagnostics.Debug.WriteLine(string.Format("VirementViewModel - set ordre {0}=>{1}",m_Ordre, value));
                    _ordre = value;
                    RaisePropertyChangedWithModification(vm => vm.Ordre);
                }
            }
        }

        /// <summary>
        /// Date du dernier virement effectué
        /// </summary>
        public DateTime? DateDernierVirement
        {
            get { return _dateDernierVirement; }
            set { _dateDernierVirement = value;
            RaisePropertyChangedWithModification(vm => vm.DateDernierVirement);
            }
        }
        /// <summary>
        /// Compte source
        /// </summary>
        public CompteViewModel CompteSrc
        {
            get { return _compteSrc; }
            set
            {
                if (value != null)
                {
                    _compteSrc = value;
                    RaisePropertyChangedWithModification(vm => vm.CompteSrc);
                }
            }
        }
        /// <summary>
        /// Compte destination
        /// </summary>
        public CompteViewModel CompteDst
        {
            get { return _compteDst; }
            set { if (value!=null)
            {
                _compteDst = value;
                RaisePropertyChangedWithModification(vm => vm.CompteDst);
            } }
        }

        ///// <summary>
        ///// Liste des comptes pour choix
        ///// </summary>
        //public ObservableCollection<CompteViewModel> Comptes
        //{
        //    get; private set;
        //}

        /// <summary>
        /// Liste des comptes pour choix
        /// </summary>
        public ObservableCollection<CompteViewModel> Comptes
        {
            get {
                var comptes = new ObservableCollection<CompteViewModel>(WpfIocFactory.Instance.Comptes);
                //ajout du compte vide
                var compteExterne = Container.Resolve<CompteViewModel>();
                comptes.Add(compteExterne);
                return comptes; 
            }
        }

        #endregion

        #region Methods
        public bool HasCompte(int compteId)
        {
            return (CompteSrc != null && CompteSrc.Id == compteId)
                || (CompteDst != null && CompteDst.Id == compteId);
        }

        /// <summary>
        /// Utilisée lors de la gestion des virements automatiques
        /// </summary>
        public override void ReinitFromModel()
        {
            if (Model != null)
            {
                _duree = Model.Duree;
                _dateDernierVirement = Model.DateDernierVirement;
                RaisePropertyChanged(vm=>vm.Duree);
                RaisePropertyChanged(vm => vm.DateDernierVirement);
            }
        }

        private void InitVirement(VirementModel model)
        {
            if (model != null)
            {
                CompteSrc = Comptes.FirstOrDefault(c => c.Id == model.CompteSrcId);
                CompteDst = Comptes.FirstOrDefault(c => c.Id == model.CompteDstId);
                Jour = model.Jour;
                Duree = model.Duree;
                Ordre = model.Ordre;
                Libelle = model.Description;
                DateDernierVirement = model.DateDernierVirement;
                Frequence = (FrequenceEnum) model.Frequence;
                Montant = model.Montant;
                SelectedTypePaiement = PaiementHelper.GetPaiement(model.TypePaiement);
            }
        }

        public override void Annuler()
        {
            InitVirement(Model);
            IsModified = false;
        }

        /// <summary>
        /// Initialisation du view model à partir d'un objet model
        /// </summary>
        /// <param name="model"></param>
        public override VirementViewModel InitFromModel(VirementModel model)
        {
            Model = model;
            Id = model.Id;
            InitVirement(model);
            if (model.Details != null)
            {
                //pour chaque détail
                foreach (var detail in model.Details)
                {
                    var detailVm = Container.Resolve<VirementDetailViewModel>().InitFromModel(detail);
                    detailVm.IsNew = false;
                    InitDetailEvents(detailVm);
                    for (int mois = 1; mois <= 12; mois++)
                    {
                        //créer le view model du montant
                        var montantVm = Container.Resolve<VirementMontantViewModel>();
                        montantVm.DetailId = detail.Id;
                        montantVm.IsNew = true;
                        montantVm.NumeroMois = mois;
                        //rechercher si un montant existe déjà pour ce mois
                        var montant = detail.Montants.FirstOrDefault(m => m.Mois == mois);
                        if (montant != null)
                            montantVm.InitFromModel(montant);
                        detailVm.Montants.Add(montantVm);
                        MoisList[mois - 1].AjouterMontant(montantVm);
                    }
                    DetailsList.Add(detailVm);
                }
            }
            return this;
        }

        /// <summary>
        /// renvoie un objet model initialisé à partir du view model
        /// </summary>
        /// <returns></returns>
        public override void SaveToModel()
        {
            Model.Id = Id;
            Model.Ordre = Ordre;
            Model.DateDernierVirement = DateDernierVirement;
            Model.Duree = Duree;
            Model.Jour = Jour;
            Model.Description = Libelle;
            Model.Frequence = (int)Frequence;
            Model.Montant = Montant;
            Model.TypePaiement = PaiementHelper.GetCodePaiement(SelectedTypePaiement);
            
            //remise à 0 avant mise à jour, sinon on va garder le dernier utilisé
            //pour pouvoir modifier un virement avec un compte vide
            Model.CompteSrcId = 0;
            Model.CompteDstId = 0;
            if (CompteSrc != null && CompteSrc.Id > 0)
                Model.CompteSrcId = CompteSrc.Id;
            if (CompteDst != null && CompteDst.Id > 0)
                Model.CompteDstId = CompteDst.Id;
            foreach (var detail in DetailsList)
            {
                detail.SaveToModel();
                //var newDetail = new VirementDetailModel
                //{
                //    Id = detail.Id,
                //    Commentaire = detail.Commentaire,
                //};
                //if (detail.SelectedRubrique != null)
                //    newDetail.RubriqueId = detail.SelectedRubrique.Id;
                //if (detail.SelectedSousRubrique != null)
                //    newDetail.SousRubriqueId = detail.SelectedSousRubrique.Id;
                //virement.Details.Add(newDetail);
            }
            //return virement;
        }

        public override VirementViewModel DuplicateViewModel()
        {
            var model = new VirementModel
            {
                DateDernierVirement = DateDernierVirement,
                Description = Libelle,
                Duree = Duree,
                Jour = Jour,
                Ordre = Ordre,
                Frequence = (int) Frequence,
                Montant = Montant,
                TypePaiement = PaiementHelper.GetCodePaiement(SelectedTypePaiement),
                Details = new List<VirementDetailModel>(
                    from detail in DetailsList
                    select new VirementDetailModel
                    {
                        Commentaire = detail.Commentaire,
                        RubriqueId = detail.SelectedRubrique.Id,
                        SousRubriqueId = detail.SelectedSousRubrique.Id,
                        Montants = new List<VirementMontantModel> (
                            from montant in detail.Montants
                            where montant.Id > 0
                            select new VirementMontantModel
                            {
                                Mois = montant.NumeroMois,
                                Montant = montant.Montant
                            }
                        )
                    })
            };
            if (CompteDst != null && CompteDst.Id > 0)
                model.CompteDstId = CompteDst.Id;
            if (CompteSrc != null && CompteSrc.Id > 0)
                model.CompteSrcId = CompteSrc.Id;

            ((IVirementService) ModelServiceBase).CreateVirementWithDetails(model);
            var vrtVm = Container.Resolve<VirementViewModel>().InitFromModel(model);
            return vrtVm;
        }

        #endregion

        #region Actions
        public override void ActionSauvegarder()
        {
            var createDetail = IsNew;

            base.ActionSauvegarder();

            if (createDetail)
            {
                //création d'un détail par défaut
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
        }

        private void InitDetailEvents(VirementDetailViewModel vm)
        {
            vm.ViewModelDeleted += OnVirementDetailDeleted;
            vm.ViewModelDuplicated += OnVirementDetailDuplicated;
        }

        private void ClearDetailEvents(VirementDetailViewModel vm)
        {
            vm.ViewModelDeleted -= OnVirementDetailDeleted;
            vm.ViewModelDuplicated -= OnVirementDetailDuplicated;
        }

        void OnVirementDetailDuplicated(object sender, EventArgs e)
        {
            var detail = sender as VirementDetailViewModel;
            if (detail != null)
            {
                InitDetailEvents(detail);
                DetailsList.Add(detail);
                //ajouter les montants 
                foreach (var mois in MoisList)
                {
                    var vm = Container.Resolve<VirementMontantViewModel>();
                    vm.DetailId = detail.Id;
                    vm.IsNew = true;
                    vm.NumeroMois = mois.NumeroMois;
                    mois.AjouterMontant(vm);
                    detail.Montants.Add(vm);
                }
            }
        }
         
        void OnVirementDetailDeleted(object sender, EventArgs e)
        {
            var detail = sender as VirementDetailViewModel;
            if (detail != null)
            {
                ClearDetailEvents(detail);
                DetailsList.Remove(detail);
                //dans les mois supprimer les montants associés
                foreach (var mois in MoisList)
                {
                    mois.SupprimerMontant(detail.Id);
                }
            }
        }

        public override void ActionAjouter()
        {
            var detail = Container.Resolve<VirementDetailViewModel>();
            detail.IsNew = true;
            detail.VirementId = Id;
            detail.IsModified = true;

            InitDetailEvents(detail);
            DetailsList.Add(detail);
            Model.Details.Add(detail.Model);

            //ajouter les montants 
            foreach (var mois in MoisList)
            {
                var vm = Container.Resolve<VirementMontantViewModel>();
                vm.DetailId = detail.Id;
                vm.IsNew = true;
                vm.NumeroMois = mois.NumeroMois;
                mois.AjouterMontant(vm);
                detail.Montants.Add(vm);
                detail.Model.Montants.Add(vm.Model);
            }
        }

        private VirementMoisViewModel _moisVm;
        public void CopierMois(VirementMoisViewModel vm)
        {
            _moisVm = vm;
        }

        public void CollerMois(VirementMoisViewModel vm)
        {
            for (int i = 0; i < vm.Montants.Count; i++)
            {
                vm.Montants[i].Montant = _moisVm.Montants[i].Montant;
            }
        }

        #endregion

        /// <summary>
        /// recopier les montants du mois sélectionné dans tous les autres mois
        /// </summary>
        /// <param name="model"></param>
        public void CopierTout(VirementMoisViewModel model)
        {
            foreach (VirementMoisViewModel t in MoisList)
            {
                //uniquement les autres mois
                if (t.NumeroMois != model.NumeroMois)
                    foreach (var montant in t.Montants)
                    {
                            var virementMontantViewModel = model.Montants.FirstOrDefault(m => m.DetailId == montant.DetailId && m.IsModified);
                            if (virementMontantViewModel != null && virementMontantViewModel.IsModified)
                                montant.Montant = virementMontantViewModel.Montant;
                    }
            }
        }

        /// <summary>
        /// Sauvegarde de tous les montants modifiés
        /// </summary>
        public void SauvegarderMontants()
        {
            foreach (var detail in DetailsList)
            {
                foreach (var montant in detail.Montants)
                {
                    if (montant.IsModified)
                    {
                        LogMessage("Sauvegarde montant {0}={1}", montant.DetailId, montant.Montant);
                        if (montant.IsNew)
                        {
                            montant.SaveToModel();
                            _montantSrv.CreateItem(montant.Model);
                            LogMessage("Montant créé {0}={1}", montant.DetailId, montant.Montant);
                            montant.Id = Model.Id;
                            montant.IsNew = false;
                            montant.IsModified = false;
                        }
                        else
                        {
                            montant.SaveToModel();
                            _montantSrv.UpdateItem(montant.Model);
                            LogMessage(String.Format("Montant sauvegardé {0}={1}", montant.DetailId, montant.Montant));
                        }
                        montant.IsModified = false;
                    }
                }
            }
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
                    LogMessage("Virement en cours de suppression...");
                    //supprimer les détails associés
                    var tmplist = new List<VirementDetailViewModel>(DetailsList);
                    foreach (var detailViewModel in tmplist)
                    {
                        var montantlist = new List<VirementMontantViewModel>(detailViewModel.Montants);
                        foreach (var montantVm in montantlist)
                        {
                            montantVm.Supprimer(MessageBoxResult.Yes);
                        }
                        detailViewModel.Supprimer(MessageBoxResult.Yes);
                    }
                    ModelServiceBase.DeleteItem(Id, false);
                }
                //supprimer ordre si il n'apparait qu'une seule fois
                var ordre = Ordres.Where(o => o == Ordre).ToList();
                if (ordre.Count == 1)
                    Ordres.Remove(ordre.First());

                LogMessage("Virement supprimé...");
                RaiseDeletedEvent();
            }
        }

    }
}
