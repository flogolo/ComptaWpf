using System;
using CommonLibrary.IOC;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace MaCompta.ViewModels
{
    public class SousRubriqueViewModel : ModelViewModelBase<SousRubriqueViewModel, SousRubriqueModel>, IComparable<SousRubriqueViewModel>
    {
        //readonly ISousRubriqueService m_SousRubriqueSrv;

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="container"> </param>
        /// <param name="sousRubriqueSrv"></param>
        public SousRubriqueViewModel(IContainer container, ISousRubriqueService sousRubriqueSrv)
            : base(container, sousRubriqueSrv)
        {
            Libelle = string.Empty;
            //m_SousRubriqueSrv = sousRubriqueSrv;
            ModelName = "Sous-rubrique";
            Model = new SousRubriqueModel();
        }
        #endregion

        public long RubriqueId { get; set; }

        ///// <summary>
        ///// Initialisation du view model à partir d'un objet model
        ///// </summary>
        ///// <param name="model"></param>
        //public override void CancelModel(SousRubriqueModel model)
        //{
        //    InitFromModel(model);
        //}

        public override SousRubriqueViewModel InitFromModel(SousRubriqueModel model)
        {
            Model = model;
            Id = model.Id;
            Libelle = model.Libelle;
            RubriqueId = model.RubriqueId;
            return this;
        }

        public override void SaveToModel()
        {
            //return new SousRubriqueModel
                       {
                Model.Id = Id;
                Model.Libelle = Libelle;
                Model.RubriqueId = RubriqueId;
                       }
        }

        public override SousRubriqueViewModel DuplicateViewModel()
        {
            throw new NotImplementedException("SousRrubriqueVieModel => DuplicateViewModel");
        }

        public int CompareTo(SousRubriqueViewModel other)
        {
            return String.Compare(Libelle, other.Libelle, StringComparison.CurrentCulture);
        }
        public override void UpdateProperties()
        {
            //nothing to do
        }
    }
}
