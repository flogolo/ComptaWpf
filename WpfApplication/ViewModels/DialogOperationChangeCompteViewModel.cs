using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MaCompta.ViewModels
{
    public class DialogOperationChangeCompteViewModel:ViewModelBase<DialogOperationChangeCompteViewModel>
    {
        private bool _isInverse;

        public bool IsInverse
        {
            get { return _isInverse; }
            set { _isInverse = value; RaisePropertyChanged(vm=>vm.IsInverse); }
        }

        public IEnumerable<CompteViewModel> AllComptes
        {
            get { return WpfIocFactory.Instance.ComptesActifs; }
        }

        public OperationViewModel SelectedOperation { get; set; }

        public string CurrentCompte { get { return AllComptes.FirstOrDefault(c=>c.Id==SelectedOperation.CompteId).Libelle; } }

        public CompteViewModel SelectedCompte { get; set; }

    }
}
