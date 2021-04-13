using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CommonLibrary.IOC;

namespace MaCompta.ViewModels
{
    public class VirementMoisViewModel: ViewModelBase<VirementMoisViewModel>
    {
        #region Constructor
        public VirementMoisViewModel(IContainer container)
            : base(container)
        {
            Montants = new ObservableCollection<VirementMontantViewModel>();
            Montants.CollectionChanged += MontantsCollectionChanged;    
        }

        private void MontantsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(vm=>vm.TotalMois);
        }

        #endregion
        
        #region Properties
        public String Mois { get; set; }
        public int NumeroMois { get; set; }

        public ObservableCollection<VirementMontantViewModel> Montants
        {
            get;
            private set;
        }

        public decimal TotalMois
        {
            get { return Montants.Sum(m => m.Montant); }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Initialisation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
		public VirementMoisViewModel Init(string name, int numero)
        {
            Mois = name;
            NumeroMois = numero;
            return this;
        } 

        ///// <summary>
        ///// Ajouter une colonne montant
        ///// </summary>
        ///// <param name="id"></param>
        //public void AjouterMontant(int id)
        //{
        //    var vm = new VirementMontantViewModel(id, 0);
        //    vm.MontantUpdated += OnMontantUpdated;
        //    Montants.Add(vm);
        //}
        /// <summary>
        /// Supprimer une colonne montant
        /// </summary>
        /// <param name="id"></param>
        public void SupprimerMontant(long id)
        {
            var vm = Montants.FirstOrDefault(m => m.DetailId == id);
            if (vm!=null)
            {
                vm.MontantUpdated -= OnMontantUpdated;
                Montants.Remove(vm);
            }
        }

        private void OnMontantUpdated(object sender, EventArgs e)
        {
            RaisePropertyChanged(vm=>vm.TotalMois);
        }

        public void RazMontants()
        {
            foreach (var montant in Montants)
            {
                montant.Montant = 0;
            }
        }

	    #endregion

        internal void AjouterMontant(VirementMontantViewModel montantVm)
        {
            montantVm.MontantUpdated += OnMontantUpdated;
            Montants.Add(montantVm);
        }
    }
}
