using MaCompta.Tools;
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

        /// <summary>
        /// Change le compte de l'opération sélectionnée
        /// appelée depuis MainWindows.xaml.cs
        /// </summary>
        /// <param name="message"></param>
        /// <param name="mainVm"></param>
        internal void ChangeCompte(CompteViewModel compteVm, MainViewModel mainVm)
        {
            SelectedOperation.CompteId = SelectedCompte.Id;
            SelectedOperation.IsModified = true;
            foreach (var detail in SelectedOperation.DetailsList)
            {
                if (IsInverse)
                {
                    detail.Montant = -detail.Montant;
                }
                else
                {
                    detail.Montant = detail.Montant;
                }
            }

            SelectedOperation.ActionSauvegarder();
            //suppression de l'opération de la liste des opérations du compte sélectionné
            compteVm.RemoveSelectedOperation();
            //recherche si le compte cible est ouvert
            var compteVmDst = mainVm.OpenedComptes.FirstOrDefault(c => c.Id == SelectedCompte.Id);
            if (compteVmDst != null)
            {
                //s'il est ouvert -> ajout de l'opération
                compteVmDst.AddOperationViewModel(SelectedOperation);
            }
        }

    }
}
