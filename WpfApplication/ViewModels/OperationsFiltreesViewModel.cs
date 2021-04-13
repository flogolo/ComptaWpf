using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MaCompta.ViewModels
{
    public class OperationsFiltreesViewModel
    {

        public ObservableCollection<OperationViewModel> Operations { get; private set; }

        public decimal MontantTotal {get;private set;}

        public OperationsFiltreesViewModel()
        {
            Operations = new ObservableCollection<OperationViewModel>();
        }

        public OperationsFiltreesViewModel(IEnumerable<OperationViewModel> operationsList)
        {
            Operations = new ObservableCollection<OperationViewModel>(operationsList.OrderBy(o=>o.DateOperation));
            MontantTotal = Operations.Sum(o => o.Montant);        
        }
    }
}
