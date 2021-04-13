using System.Collections.Generic;

namespace CommonLibrary.Models
{
    public class VirementDetailModel : ModelBase
    {
        /// <summary>
        /// Commentaire
        /// </summary>
        public virtual string Commentaire { get; set; }
        /// <summary>
        /// detail uniquement pour le compte source
        /// </summary>
        public virtual bool IsCompteSrcOnly { get; set; }
        /// <summary>
        /// detail uniquement pour le compte destination
        /// </summary>
        public virtual bool IsCompteDstOnly { get; set; }
        /// <summary>
        /// rubrique associée
        /// </summary>
        public virtual long RubriqueId { get; set; }
        /// <summary>
        /// sous-rubrique associée
        /// </summary>
        public virtual long SousRubriqueId { get; set; }
        /// <summary>
        /// Identifiant du virement
        /// </summary>
        public virtual long VirementId { get; set; }

        /// <summary>
        /// Liste des montants
        /// </summary>
        public virtual List<VirementMontantModel> Montants
        {
            get;
            set;
        }

        public VirementDetailModel()
        {
            Montants = new List<VirementMontantModel>();
        }


        public override string ToString()
        {
            return string.Format("{0}", Commentaire);
        }
    }
}
