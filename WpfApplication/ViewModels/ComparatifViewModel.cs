using CommonLibrary.IOC;
using CommonLibrary.Services.Interfaces;
using MaCompta.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MaCompta.ViewModels
{
    internal class ComparatifViewModel:ViewModelBase<ComparatifViewModel>
    {
        private DateTime _selectedMonth;

        public DateTime SelectedMonth
        {
            get { return _selectedMonth; }
            set { _selectedMonth = value; RaisePropertyChanged(vm => SelectedMonth); }
        }

        private DateTime _selectedYear;

        public DateTime SelectedYear
        {
            get { return _selectedYear; }
            set { _selectedYear = value; RaisePropertyChanged(vm => SelectedYear); }
        }
        private CompteViewModel _SelectedCompte;
        public CompteViewModel SelectedCompte
        {
            get
            {
                return _SelectedCompte;
            }

            set
            {
                if (_SelectedCompte == value)
                {
                    return;
                }

                _SelectedCompte = value;
                RaisePropertyChanged(v => v.SelectedCompte);
            }
        }
        public ObservableCollection<CompteViewModel> Comptes {
            // get; private set;
           get { return WpfIocFactory.Instance.Comptes; }
        }

        //public ComparatifViewModel()
        //{
        //    Results = new ObservableCollection<ComparatifResult>();
        //    SelectedMonth = DateTime.Now;
        //    SelectedYear = DateTime.Now;
        //    //Comptes = new ObservableCollection<CompteViewModel>();
        //}

        IOperationService _opsvc;
        public ComparatifViewModel(IContainer container, IOperationService operationSrv)
            :base(container)
        {
            _opsvc = operationSrv;
            Results = new ObservableCollection<ComparatifResult>();
            SelectedMonth = DateTime.Now;
            SelectedYear = DateTime.Now;
            //Comptes = new ObservableCollection<CompteViewModel>();
            //foreach (var compte in WpfIocFactory.Instance.Comptes)
            //{
            //    Comptes.Add(compte);
            //}
        }

        public ObservableCollection<ComparatifResult> Results { get; set; }

        [BaseCommand("CalculCommand")]
        public void Calculer()
        {
           var allops =  _opsvc.LoadOperationsEnCours(SelectedCompte.Id);

            var ops = allops.Where(o=> o.DateOperation.HasValue
            && o.DateOperation.Value.Month == SelectedMonth.Month && o.DateOperation.Value.Year == SelectedMonth.Year).OrderBy(o=>o.DateOperation);

            Results.Clear();
            decimal creditBudgetTotal = 0;
            decimal creditReelTotal = 0;
            decimal debitBudgetTotal = 0;
            decimal debitReelTotal = 0;
            var results = new List<ComparatifResult>();
            foreach (var op in ops)
            {
                foreach (var detail in op.Details)
                {
                    var result = results.FirstOrDefault(r => r.RubriqueId == detail.RubriqueId
                    && r.SousRubriqueId == detail.SousRubriqueId);
                    if (result == null)
                    {
                        result = new ComparatifResult
                        {
                            //DateOperation = op.DateOperation.Value.ToShortDateString(),
                            //Ordre = op.Ordre,
                            //MontantBudget = op.MontantBudget,
                            //MontantReel = op.Details.Sum(d => d.Montant)
                            
                            DebitBudget = detail.MontantBudget < 0 ? detail.MontantBudget : 0,
                            DebitReel = detail.Montant<0? detail.Montant : 0,
                            CreditBudget = detail.MontantBudget > 0 ? detail.MontantBudget : 0,
                            CreditReel = detail.Montant > 0 ? detail.Montant : 0,
                            RubriqueId = detail.RubriqueId,
                            Rubrique = WpfIocFactory.Instance.GetRubriqueNom(detail.RubriqueId),
                            SousRubriqueId = detail.SousRubriqueId,
                            SousRubrique = WpfIocFactory.Instance.GetSousRubriqueNom(detail.RubriqueId, detail.SousRubriqueId),
                        };

                        results.Add(result);
                    }
                    else
                    {
                        result.DebitBudget += detail.MontantBudget < 0 ? detail.MontantBudget : 0;
                        result.DebitReel += detail.Montant < 0 ? detail.Montant : 0;
                        result.CreditBudget += detail.MontantBudget > 0 ? detail.MontantBudget : 0;
                        result.CreditReel += detail.Montant > 0 ? detail.Montant : 0;
                    }
                    creditBudgetTotal += detail.MontantBudget > 0 ? detail.MontantBudget : 0;
                    creditReelTotal += detail.Montant > 0 ? detail.Montant : 0;
                     debitBudgetTotal += detail.MontantBudget < 0 ? detail.MontantBudget : 0;
                     debitReelTotal += detail.Montant < 0 ? detail.Montant : 0;
                }
            }
            results.OrderBy(r => r.Rubrique).ThenBy(r => r.SousRubrique).ToList().ForEach(r => Results.Add(r));
            Results.Add(new ComparatifResult
            {
                CreditBudget = creditBudgetTotal,
                CreditReel = creditReelTotal,
                DebitBudget = debitBudgetTotal,
                DebitReel = debitReelTotal,
                Rubrique = "Total"
            });
        }
    }

    public class ComparatifResult
    {
        public decimal DebitReel { get; set; }
        public decimal DebitBudget { get; set; }
        public decimal CreditReel { get; set; }
        public decimal CreditBudget { get; set; }
        public decimal SoldeReel { get { return CreditReel + DebitReel; } }
        //public string Ordre { get; set; }
        //public String DateOperation { get; set; }
        public long RubriqueId { get; set; }
        public long SousRubriqueId { get; set; }
        public string Rubrique { get; set; }
        public string SousRubrique { get; set; }
    }
}
