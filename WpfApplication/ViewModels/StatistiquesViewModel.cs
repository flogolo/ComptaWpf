using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;
using MaCompta.Commands;
using MaCompta.Common;
using MaCompta.Controls;

namespace MaCompta.ViewModels
{
    public class StatistiquesViewModel : ViewModelBase<StatistiquesViewModel>
    {
        #region Membres
        private ComboBoxItem<DetailTriOption> _selectedTri;
        private readonly RubriqueViewModel _allRubriques;
        private readonly SousRubriqueViewModel _allSousRubriques;
        private readonly CompteViewModel _allComptes;
        private readonly CompteViewModel _noneCompte;
        private bool _isNotAvailable;
        private RubriqueViewModel _selectedRubrique;
        private SousRubriqueViewModel _selectedSousRubrique;
        private DateTime _dateDebut;
        private DateTime _dateFin;
        private string _statTitle;
        private string _legendTitle;
        //private ICommand m_CalculCommand;
        private readonly IOperationService _operationService;
        private SousRubriqueViewModel _selectedSousRubriqueDetail;
        private string _commentaire;
        #endregion

        #region Constructeur
        /// <summary>
        /// Constructeur
        /// </summary>
        public StatistiquesViewModel(CommonLibrary.IOC.IContainer container, IOperationService operationService)
            : base(container)
        {
            _operationService = operationService;
            IsNotAvailable = true;
            Stats = new ObservableCollection<DetailStat>();
            StatsMulti = new ObservableCollection<DetailStatMulti>();
            //ServiceFactory.Instance.Services.DetailManager.ListLoaded += UpdateDetails;
            _allRubriques = Container.Resolve<RubriqueViewModel>();
            _allRubriques.Id = 0;
            _allRubriques.Libelle = "Toutes les rubriques";
            _allSousRubriques = Container.Resolve<SousRubriqueViewModel>();
            _allSousRubriques.Id = 0;
            _allSousRubriques.Libelle = "Toutes les sous-rubriques";

            SousRubriques = new ObservableCollection<SousRubriqueViewModel> { _allSousRubriques };
            Rubriques = new ObservableCollection<RubriqueViewModel> { _allRubriques };
            foreach (var rubrique in WpfIocFactory.Instance.RubriquesVm.Rubriques)
            {
                Rubriques.Add(rubrique);
            }
            SelectedRubrique = _allRubriques;
            WpfIocFactory.Instance.RubriquesVm.Rubriques.CollectionChanged += RubriquesCollectionChanged;
            var now = DateTime.Now;
            DateDebut = new DateTime(now.Year, now.Month, 1);
            DateFin = DateDebut.AddMonths(1).AddDays(-1);

            _noneCompte = Container.Resolve<CompteViewModel>();
            _noneCompte.Id = -1;
            _noneCompte.Libelle = MultiSelectComboBox.NoneItems;
            _allComptes = Container.Resolve<CompteViewModel>();
            _allComptes.Id = 0;
            _allComptes.Libelle = MultiSelectComboBox.AllItems;
            Comptes = new ObservableCollection<IViewModel> { _noneCompte, _allComptes};
            foreach (var compte in WpfIocFactory.Instance.Comptes)
            {
                Comptes.Add(compte);
            }
            SelectedComptes = new ObservableCollection<IViewModel>();
            WpfIocFactory.Instance.Comptes.CollectionChanged +=ComptesCollectionChanged;

            ListeTri = new ComboBoxItemCollection<DetailTriOption> { 
                new ComboBoxItem<DetailTriOption>((int) DetailTriOption.DateOperation, "Par date d'opération", DetailTriOption.DateOperation),
                new ComboBoxItem<DetailTriOption>((int) DetailTriOption.DateValidation, "Par date de validation", DetailTriOption.DateValidation),
                new ComboBoxItem<DetailTriOption>((int) DetailTriOption.TypeAndDate, "Par type et date", DetailTriOption.TypeAndDate),
                new ComboBoxItem<DetailTriOption>((int) DetailTriOption.RubriqueAndSousRubrique, "Par rubrique et sous-rubrique", DetailTriOption.RubriqueAndSousRubrique),
            };
            DetailSousRubriques = new SortableObservableCollection<SousRubriqueViewModel>();
            StatsRubrique = new ObservableCollection<StatRubriqueModel>();
            IsAllRubriques = false;
        }

        private void ComptesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ComptesCollectionChanged " + e.Action);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        Comptes.Add((CompteViewModel)item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        Comptes.Remove((CompteViewModel)item);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Comptes.Clear();
                    Comptes.Add(_noneCompte);
                    Comptes.Add(_allComptes);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Methodes
        /// <summary>
        /// Sur changement dans la liste des rubriques
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RubriquesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        Rubriques.Add((RubriqueViewModel)item);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        Rubriques.Remove((RubriqueViewModel)item);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Rubriques.Clear();
                    Rubriques.Add(_allRubriques);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CalculerToutesRubriques(IEnumerable<OperationModel> operations, List<DetailStat> result)
        {
            StatTitle = "Toutes les rubriques";
            LegendTitle = "Rubriques";
            Decimal credit = 0;
            Decimal debit = 0;
            IsAllRubriques = true;
            StatsRubrique.Clear();
            //recherche des détails concernées
            //ServiceFactory.Instance.Services.DetailManager.ItemsList.Where(d=>d.DateOperation)...
            foreach (var operation in operations)
            {
                foreach (var detail in operation.Details)
                {
                    if (!FiltrerCommentaire(detail))
                    {
                        continue;
                    }
                    //Affichage des stats sur toutes les rubriques
                    //mise à jour du credit/debit
                    if (detail.Montant > 0)
                        credit += detail.Montant;
                    else
                        debit += detail.Montant;

                    //recherche de la rubrique
                    var rubrique = WpfIocFactory.Instance.RubriquesVm.GetRubrique(detail.RubriqueId);
                    //recherche de la sous-rubrique
                    var sousrubrique = WpfIocFactory.Instance.RubriquesVm.GetSousRubrique(rubrique, detail.SousRubriqueId);
                    //var sousRubriqueLibelle = sousrubrique.Libelle;
                    StatRubriqueModel statsRubrique;

                    //recherche si la rubrique est déjà dans le résultat
                    var statdetail = result.FirstOrDefault(d => d.Id == rubrique.Id);
                    if (statdetail == null)
                    {
                        //si elle n'est pas présente, on crée un nouveau resultat
                        result.Add(new DetailStat
                        {
                            Id = rubrique.Id,
                            Libelle = rubrique.Libelle,
                            Montant = detail.Montant
                        });
                        statsRubrique = new StatRubriqueModel { Libelle = rubrique.Libelle, Id = rubrique.Id, Montant = detail.Montant };
                        StatsRubrique.Add(statsRubrique);
                    }
                    else
                    {
                        //si la rubrique est déjà présente, on la met à jour
                        statdetail.Montant += detail.Montant;
                        //mise à jour rubrique
                        statsRubrique = StatsRubrique.First(r => r.Id == rubrique.Id);
                        statsRubrique.Montant += detail.Montant;
                    }
                    //recherche de la sous-rubrique
                    var statsSous = statsRubrique.SousRubriqueStats.FirstOrDefault(sr => sr.Id == sousrubrique.Id);
                    if(statsSous == null)
                    {
                        //sous-rubrique non trouvée
                        statsSous = new StatSousRubriqueModel { Id = sousrubrique.Id, Libelle = sousrubrique.Libelle, Montant = detail.Montant };
                        statsRubrique.SousRubriqueStats.Add(statsSous);
                    }
                    else
                    {
                        //sous rubrique trouvée -> mise à jour du montant
                        statsSous.Montant += detail.Montant;
                    }
                    statsSous.Details.Add(new DetailDate {
                        Montant =detail.Montant,
                        Commentaire =detail.Commentaire,
                        DateDetail = 
                        operation.DateValidation.Value.ToShortDateString(),
                        Ordre =operation.Ordre
                    });
                }
            }
            StatTitle = String.Format("Toutes les rubriques : débit={0} crédit={1}", debit, credit);
            //                TotalParMois = total / 12;
        }

        private bool FiltrerCommentaire(DetailModel detail)
        {
            if (string.IsNullOrEmpty(Commentaire)) 
            { 
                return true; 
            }
            else
            {
                if (detail.Commentaire.Contains(Commentaire))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void CalculerUneRubrique(IEnumerable<OperationModel> operations, List<DetailStat> result)
        {
            LegendTitle = "Sous-Rubriques";
            IsSousRubrique = true;
            Decimal montantTotal = 0;
            //var colorIndex = 0;
            IsAllRubriques = false;
            DetailSousRubriques.Clear();

            foreach (var operation in operations)
            {
                foreach (var detail in operation.Details)
                {
                    if (detail.RubriqueId != SelectedRubrique.Id || !FiltrerCommentaire(detail))
                    {
                        continue;
                    }

                    //recherche de la sous-rubrique
                    var sousrubrique = WpfIocFactory.Instance.RubriquesVm.GetSousRubrique(SelectedRubrique, detail.SousRubriqueId);
                    var sousRubriqueLibelle = sousrubrique.Libelle;
                    //stat globale
                    var statDetail = result.FirstOrDefault(d => d.Id == detail.SousRubriqueId);
                    montantTotal += detail.Montant;
                    if (statDetail == null)
                    {
                        result.Add(new DetailStat
                        {
                            Id = detail.SousRubriqueId,
                            Libelle = sousRubriqueLibelle,
                            Montant = detail.Montant,
                            //FavoriteColor = SousRubriquesColors[colorIndex]
                        });
                        DetailSousRubriques.Add(sousrubrique);
                    }
                    else
                    {
                        statDetail.Montant += detail.Montant;
                    }

                    //stat mensuelle
                    var statSousRubrique = StatsMulti.FirstOrDefault(d => d.Id == detail.SousRubriqueId);
                    if (statSousRubrique == null)
                    {
                        statSousRubrique = new DetailStatMulti
                        {
                            Libelle = sousRubriqueLibelle,
                            Id = detail.SousRubriqueId,
                            //FavoriteColor = SousRubriquesColors[colorIndex]
                        };
                        //colorIndex++;
                        if (DateDebut.Month > DateFin.Month)
                        {
                            //à cheval sur 2 années
                            for (int i = DateDebut.Month; i <= 12; i++)
                            {
                                statSousRubrique.Stats.Add(new DetailStatSerie { Mois = i, Montant = 0 });
                            }
                            for (int i = 1; i <= DateFin.Month; i++)
                            {
                                statSousRubrique.Stats.Add(new DetailStatSerie { Mois = i, Montant = 0 });
                            }
                        }
                        else
                        {
                            for (int i = DateDebut.Month; i <= DateFin.Month; i++)
                            {
                                statSousRubrique.Stats.Add(new DetailStatSerie { Mois = i, Montant = 0 });
                            }
                        }
                        StatsMulti.Add(statSousRubrique);
                    }
                    if (operation.DateValidation != null)
                    {
                        var currentMois = operation.DateValidation.Value.Month;
                        var mois =
                            statSousRubrique.Stats.FirstOrDefault(s => s.Mois == currentMois);
                        if (mois != null)
                        {
                            mois.Montant += detail.Montant;
                            mois.Details.Add(new DetailDate
                            {
                                DateDetail = operation.DateValidation.Value.ToShortDateString(),
                                Ordre = operation.Ordre,
                                Montant = detail.Montant,
                                Commentaire = detail.Commentaire
                            });
                        }
                    }
                }
            }

            int nbMois = CalcNbMois();
            if (nbMois > 0)
                StatTitle = String.Format("{0} {1} ({2} par mois)", SelectedRubrique.Libelle, montantTotal, Math.Round(montantTotal / nbMois, 2));

            SelectRubriqueIfOnlyOne();
        }

        private int CalcNbMois()
        {
            if (DateDebut.Month > DateFin.Month)
            {
                //à cheval sur 2 années
                return  12 - DateDebut.Month + DateFin.Month + 2;
            }
            return DateFin.Month - DateDebut.Month + 1;
        }

        private void SelectRubriqueIfOnlyOne()
        {
            if (DetailSousRubriques.Count == 1)
                SelectedSousRubriqueDetail = DetailSousRubriques.First();
        }

        [BaseCommand("CalculCommand")]
        public void Calculer()
        {
            Stats.Clear();
            StatsMulti.Clear();
            SelectedSousRubriqueDetail = null;

            var result = new List<DetailStat>();
            //recherche des opérations concernées
            var operations = new Collection<OperationModel>();
            foreach (var compte in SelectedComptes)
            {
                var compteOperations = _operationService.LoadOperationsEnCours(((CompteViewModel)compte).Id).Where(o => o.DateValidation >= DateDebut && o.DateValidation <= DateFin);
                foreach (var operation in compteOperations)
                {
                    operations.Add(operation);
                }
            }

            IsSousRubrique = false;
            if (SelectedRubrique == _allRubriques)
            {
                CalculerToutesRubriques(operations, result);
            }
            else
            {
                if (SelectedSousRubrique == _allSousRubriques)
                {
                    CalculerUneRubrique(operations, result);
                }
                else
                {
                    CalculerUneSouRubrique(operations, result);
                }
                StatsUpdated.Raise(this, EventArgs.Empty);
            }
            RaisePropertyChanged(vm => vm.IsSousRubrique);
            RaisePropertyChanged(vm => vm.IsAllRubriques);
            //recopie des détails dans stats (pour mettre à jour l'IHM)
            result.ForEach(l => Stats.Add(l));

            if (Stats.Count > 0)
                IsNotAvailable = false;
        }

        private void CalculerUneSouRubrique(IEnumerable<OperationModel> operations, List<DetailStat> result)
        {
            //une seule sous-rubrique
            LegendTitle = "Sous-Rubrique " + SelectedSousRubrique.Libelle;
            IsSousRubrique = true;
            Decimal montantTotal = 0;
            IsAllRubriques = false;
            DetailSousRubriques.Clear();
            //var colorIndex = 0;
            foreach (var operation in operations)
            {
                foreach (var detail in operation.Details)
                {
                    if (detail.RubriqueId != SelectedRubrique.Id || detail.SousRubriqueId != SelectedSousRubrique.Id || !FiltrerCommentaire(detail))
                    {
                        continue;
                    }

                    var sousRubriqueLibelle = SelectedSousRubrique.Libelle;

                    //stat globale
                    var statDetail = result.FirstOrDefault(d => d.Id == detail.SousRubriqueId);
                    montantTotal += detail.Montant;
                    if (statDetail == null)
                    {
                        result.Add(new DetailStat
                                        {
                                            Id = detail.SousRubriqueId,
                                            Libelle = sousRubriqueLibelle,
                                            Montant = detail.Montant,
                                            //FavoriteColor = SousRubriquesColors[colorIndex]
                                        });
                        DetailSousRubriques.Add(SelectedSousRubrique);
                    }
                    else
                    {
                        statDetail.Montant += detail.Montant;
                    }

                    //stat mensuelle
                    var statSousRubrique = StatsMulti.FirstOrDefault(d => d.Id == detail.SousRubriqueId);
                    if (statSousRubrique == null)
                    {
                        statSousRubrique = new DetailStatMulti
                                                {
                                                    Libelle = sousRubriqueLibelle,
                                                    Id = detail.SousRubriqueId,
                                                    //FavoriteColor = SousRubriquesColors[colorIndex]
                                                };
                        StatsMulti.Add(statSousRubrique);
                        //colorIndex++;
                        if (DateDebut.Month > DateFin.Month)
                        {
                            //à cheval sur 2 années
                            for (int i = DateDebut.Month; i <= 12; i++)
                            {
                                statSousRubrique.Stats.Add(new DetailStatSerie { Mois = i, Montant = 0 });
                            }
                            for (int i = 1; i <= DateFin.Month; i++)
                            {
                                statSousRubrique.Stats.Add(new DetailStatSerie { Mois = i, Montant = 0 });
                            }
                        }
                        else
                        {
                            for (int i = DateDebut.Month; i <= DateFin.Month; i++)
                            {
                                statSousRubrique.Stats.Add(new DetailStatSerie { Mois = i, Montant = 0 });
                            }
                        }
                    }
                    if (operation.DateOperation != null)
                    {
                        var currentMois = operation.DateOperation.Value.Month;
                        var mois =
                            statSousRubrique.Stats.FirstOrDefault(s => s.Mois == currentMois);
                        if (mois != null)
                        {
                            mois.Montant += detail.Montant;
                            mois.Details.Add(new DetailDate
                                                {
                                                    DateDetail =
                                                        operation.DateValidation.Value.ToShortDateString(),
                                                    Ordre = operation.Ordre,
                                                    Montant = detail.Montant,
                                                    Commentaire = detail.Commentaire
                                                });
                        }
                    }

                }
            }
            var nbMois = CalcNbMois();
            StatTitle = String.Format("{0} : {1} ({2} par mois)", LegendTitle, montantTotal, Math.Round(montantTotal / nbMois, 2));

            SelectRubriqueIfOnlyOne();
        }

        #endregion

        #region Propriétés

        /// <summary>
        /// Statistiques disponibles?
        /// </summary>
        public bool IsNotAvailable
        {
            get { return _isNotAvailable; }
            set
            {
                _isNotAvailable = value;
                RaisePropertyChanged(vm => vm.IsNotAvailable);
            }
        }

        /// <summary>
        /// Liste des détails
        /// </summary>
        public ObservableCollection<DetailStat> Stats { get; set; }
        public ObservableCollection<DetailStatMulti> StatsMulti { get; set; }
        public ObservableCollection<StatRubriqueModel> StatsRubrique { get; set; }

        public Dictionary<int, ObservableCollection<DetailStat>> ListDetails { get; set; }

        public SortableObservableCollection<DetailStatSerie> ListSelectedDetails
        {
            get
            {
                if (SelectedSousRubriqueDetail != null && SelectedSousRubriqueDetail.Id != 0)
                {
                    var statMulti = StatsMulti.FirstOrDefault(s => s.Id == SelectedSousRubriqueDetail.Id);
                    if (statMulti != null)
                        return statMulti.Stats;
                    return null;
                }
                return null;
            }
        }

        public Decimal TotalDetailSousRubrique
        {
            get
            {
                if (ListSelectedDetails != null)
                    return ListSelectedDetails.Sum(s => s.Montant);
                return 0;
            }
        }

        public ObservableCollection<IViewModel> Comptes { get; private set; }

        //private CompteViewModel m_SelectedCompte;

        ///// <summary>
        ///// Sets and gets the MyProperty property.
        ///// Changes to that property's value raise the PropertyChanged event. 
        ///// </summary>
        //public CompteViewModel SelectedCompte
        //{
        //    get
        //    {
        //        return m_SelectedCompte;
        //    }

        //    set
        //    {
        //        if (m_SelectedCompte == value)
        //        {
        //            return;
        //        }
        //        //if (value.Id != m_AllComptes.Id)
        //        //{
        //        //    m_OperationService.LoadOperationsEnCours(value.Id);
        //        //}

        //        m_SelectedCompte = value;
        //        RaisePropertyChanged(v => v.SelectedCompte);
        //    }
        //}

        public ObservableCollection<IViewModel> SelectedComptes
        {
            get;
            private set;
        }

        /// <summary>
        /// Liste des rubriques
        /// </summary>
        public ObservableCollection<RubriqueViewModel> Rubriques
        {
            get;
            private set;
        }

        public RubriqueViewModel SelectedRubrique
        {
            get { return _selectedRubrique; }
            set
            {
                _selectedRubrique = value;
                SousRubriques.Clear();
                SousRubriques.Add(_allSousRubriques);
                if (value != null)
                {
                    foreach (var sousRubrique in value.SousRubriques)
                    {
                        SousRubriques.Add(sousRubrique);
                    }
                }
                SelectedSousRubrique = _allSousRubriques;
                RaisePropertyChanged(vm => vm.SelectedRubrique);
            }
        }

        public SousRubriqueViewModel SelectedSousRubrique
        {
            get { return _selectedSousRubrique; }
            set
            {
                _selectedSousRubrique = value;
                RaisePropertyChanged(vm => vm.SelectedSousRubrique);
            }
        }

        public SousRubriqueViewModel SelectedSousRubriqueDetail
        {
            get { return _selectedSousRubriqueDetail; }
            set
            {
                _selectedSousRubriqueDetail = value;
                RaisePropertyChanged(vm => vm.SelectedSousRubriqueDetail);
                RaisePropertyChanged(vm => vm.ListSelectedDetails);
                RaisePropertyChanged(vm => vm.TotalDetailSousRubrique);
            }
        }

        /// <summary>
        /// Liste des sous-rubriques
        /// </summary>
        public ObservableCollection<SousRubriqueViewModel> SousRubriques
        {
            get;
            private set;
        }

        /// <summary>
        /// Liste des sous-rubriques
        /// </summary>
        public SortableObservableCollection<SousRubriqueViewModel> DetailSousRubriques
        {
            get;
            private set;
        }
        
        public string Commentaire { get { return _commentaire; }
            set { 
                _commentaire = value;
                RaisePropertyChanged(vm => vm.Commentaire);
            } 
        }

        /// <summary>
        /// Date de début des stats
        /// </summary>
        public DateTime DateDebut
        {
            get { return _dateDebut; }
            set
            {
                _dateDebut = value;
                RaisePropertyChanged(vm => vm.DateDebut);
            }
        }

        /// <summary>
        /// Date de fin des stats
        /// </summary>
        public DateTime DateFin
        {
            get { return _dateFin; }
            set
            {
                _dateFin = value;
                RaisePropertyChanged(vm => vm.DateFin);
            }
        }

        public String StatTitle
        {
            get { return _statTitle; }
            set
            {
                _statTitle = value;
                RaisePropertyChanged(vm => vm.StatTitle);
            }
        }

        public String LegendTitle
        {
            get { return _legendTitle; }
            set
            {
                _legendTitle = value;
                RaisePropertyChanged(vm => vm.LegendTitle);
            }
        }
        //public ICommand CalculCommand
        //{
        //    get
        //    {
        //        if (m_CalculCommand == null)
        //        {
        //            m_CalculCommand = new RelayCommand(param => UpdateDetails());
        //        }
        //        return m_CalculCommand;
        //    }
        //}

        public bool IsSousRubrique { get; set; }

        public bool IsAllRubriques { get; set; }

        public ComboBoxItem<DetailTriOption> SelectedTri
        {
            get { return _selectedTri; }
            set
            {
                _selectedTri = value;
                RaisePropertyChanged(vm => vm.SelectedTri);
            }
        }

        public ComboBoxItemCollection<DetailTriOption> ListeTri
        { get; private set; }
        #endregion

        #region Evènements
        /// <summary>
        /// Les statistiques ont été mis à jour
        /// </summary>
        public event EventHandler StatsUpdated;
        #endregion
    }

    public enum DetailTriOption
    {
        [Description("Par rubrique et sous-rubrique")]
        RubriqueAndSousRubrique,
        [Description("Par type et date")]
        TypeAndDate,
        [Description("Par date d'opération")]
        DateOperation,
        [Description("Par date de validation")]
        DateValidation
    }


}
