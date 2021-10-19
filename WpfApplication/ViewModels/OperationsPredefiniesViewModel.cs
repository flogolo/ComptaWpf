using CommonLibrary.IOC;
using CommonLibrary.Models;
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
            //tri à l'ouverture de la fenêtre
            OperationsPredefinies.Sort();
        }

        public bool IsEnabledEdition
        {
            get { return SelectedOperationPredefinie != null; }
        }

        private OperationPredefinieViewModel _selectedOperationPredefinie;
        public OperationPredefinieViewModel SelectedOperationPredefinie
        {
            get { return _selectedOperationPredefinie; }
            set
            {
                _selectedOperationPredefinie = value;
                RaisePropertyChanged(vm => vm.SelectedOperationPredefinie);
                RaisePropertyChanged(vm => IsEnabledEdition);
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

        [BaseCommand("SupprimerOperationPredefinieCommand")]
        public void SupprimerOperationPredefinie()
        {
            //demander confirmation
            if (SelectedOperationPredefinie != null) 
            {
                SelectedOperationPredefinie.ViewModelDeleted += SelectedOperationPredefinie_ViewModelDeleted;
                SelectedOperationPredefinie.ActionSupprimer();
            }
        }

        private void SelectedOperationPredefinie_ViewModelDeleted(object sender, EventArgs<OperationPredefinieModel> e)
        {
            SelectedOperationPredefinie.ViewModelDeleted -= SelectedOperationPredefinie_ViewModelDeleted;
            //supprimer du menu
            WpfIocFactory.Instance.MainVm.RemoveOperationPredefinieToMenu(SelectedOperationPredefinie);
            //supprimer de la liste
            OperationsPredefinies.Remove(SelectedOperationPredefinie);
            //remettre à null l'operation selectionnée
            SelectedOperationPredefinie = null;
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
