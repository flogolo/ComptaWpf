using System;
using CommonLibrary.IOC;
using CommonLibrary.Models;

namespace MaCompta.ViewModels
{
    public class BanqueViewModel : ModelViewModelBase<BanqueViewModel, BanqueModel>
    {
        ///// <summary>
        ///// Libellé de la banque
        ///// </summary>
        //private string m_Designation;

        public BanqueViewModel(IContainer container) : base(container)
        {
            Model = new BanqueModel();
        }

        //public string Designation
        //{
        //    get { return m_Designation; }
        //    set
        //    {
        //        m_Designation = value;
        //        RaisePropertyChangedWithModification(vm => vm.Designation);
        //    }
        //}

        ///// <summary>
        ///// Initialisation du view model à partir d'un objet model
        ///// </summary>
        ///// <param name="model"></param>
        //public override void CancelModel(BanqueModel model)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Initialisation du view model à partir d'un objet model
        /// </summary>
        /// <param name="model"></param>
        public override BanqueViewModel InitFromModel(BanqueModel model)
        {
            Id = model.Id;
            Libelle = model.Description;
            return this;
        }

        /// <summary>
        /// renvoie un objet model initialisé à partir du view model
        /// </summary>
        /// <returns></returns>
        public override void SaveToModel()
        {
            Model.Description = Libelle;
        }

        /// <summary>
        /// Duplication du view model hors Id
        /// </summary>
        /// <returns></returns>
        public override BanqueViewModel DuplicateViewModel()
        {
            throw new NotImplementedException("BanqueVieModel => DuplicateViewModel");
        }

        public override void UpdateProperties()
        {
            //nothing to do
        }
    }
}
