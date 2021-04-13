using CommonLibrary.IOC;
using CommonLibrary.Tools;
using MaCompta.Commands;
using System.Linq;

namespace MaCompta.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class OperationsPredefiniesViewModel : ViewModelBase<OperationsPredefiniesViewModel>
    {

        /// <summary>
        /// Liste des opérations prédéfinies
        /// </summary>
        public SortableObservableCollection<OperationPredefinieViewModel> OperationsPredefinies { get; private set; }

        public OperationsPredefiniesViewModel(IContainer container):base(container)
        {
            OperationsPredefinies = WpfIocFactory.Instance.OperationsPredefinies;
                //new SortableObservableCollection<OperationPredefinieViewModel>();
        }

        private OperationPredefinieViewModel _selectedOperationPredefinie;
        public OperationPredefinieViewModel SelectedOperationPredefinie
        {
            get { return _selectedOperationPredefinie; }
            set
            {
                _selectedOperationPredefinie = value;
                RaisePropertyChanged(vm => vm.SelectedOperationPredefinie);
            }
        }


        [BaseCommand("AjouterOperationPredefinieCommand")]
        public void AjouterOperationPredefinie()
        {
            var op = Container.Resolve<OperationPredefinieViewModel>();
            op.IsNew = true;
            OperationsPredefinies.Add(op);
            op.ViewModelSaved += OperationPredefinieViewModelSaved;

            SelectedOperationPredefinie = OperationsPredefinies.Last();

            OperationsPredefinies.Sort();
        }

        private void OperationPredefinieViewModelSaved(object sender, EventArgs<CommonLibrary.Models.OperationPredefinieModel> e)
        {
            //l'opération a été sauvegardée, il faut l'ajouter au menu des opérations prédéfinies
            OperationPredefinieViewModel opVm = sender as OperationPredefinieViewModel;
            opVm.ViewModelSaved -= OperationPredefinieViewModelSaved;
            WpfIocFactory.Instance.MainVm.AddOperationPredefinieToMenu(opVm);
        }
    }
}
