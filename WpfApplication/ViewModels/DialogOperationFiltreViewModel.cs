using CommonLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace MaCompta.ViewModels
{
    internal class DialogOperationFiltreViewModel : ViewModelBase<DialogOperationFiltreViewModel>
    {
        private readonly IOperationService _operationSrv;

        public DialogOperationFiltreViewModel(IOperationService opService)
        {
            _operationSrv = opService;
        }

        public EnumOperationFilter FiltreType;

        public IEnumerable<String> EtatFiltres
        {
            get
            {
                return new List<String>
                       {
                           "Tout",
                           "Validées",
                           "Non validées",
                           "Partielles",
                           "Non partielles"
                       };
            }
        }

        internal bool IsValidFilter
        {
            get
            {
                if ((FiltreDate1 == null && FiltreDate2 == null)
                    || (FiltreDate1 == null && FiltreDate2 != null)
                    || (FiltreDate1 != null && FiltreDate2 == null)
                    || (FiltreDate2 != null && FiltreDate1 != null && FiltreDate2.Value < FiltreDate1.Value))
                {
                    LogMessage("Dates de filtre incorrectes");
                    return false;
                }
                return true;
            }
        }

        public int FiltreTypeIndex
        {
            get { return (int)FiltreType; }
            set
            {
                FiltreType = (EnumOperationFilter)value;
                RaisePropertyChanged(vm => vm.FiltreTypeIndex);
            }
        }

        /// <summary>
        /// Date de validation date basse
        /// </summary>
        private DateTime? _filtreDate1;
        public DateTime? FiltreDate1
        {
            get { return _filtreDate1; }
            set
            {
                _filtreDate1 = value;
                RaisePropertyChanged(vm => vm.FiltreDate1);
            }
        }
        /// <summary>
        /// Date de validation date haute
        /// </summary>
        private DateTime? _filtreDate2;
        public DateTime? FiltreDate2
        {
            get { return _filtreDate2; }
            set
            {
                _filtreDate2 = value;
                RaisePropertyChanged(vm => vm.FiltreDate2);
            }
        }
        
    }
}
