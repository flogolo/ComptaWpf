using System;

namespace CommonLibrary.Models
{
    public class DetailModel : ModelBase
    {
        public virtual decimal Montant { get; set; }
        public virtual String Commentaire { get; set; }
        public virtual long RubriqueId { get; set; }
        public virtual long SousRubriqueId { get; set; }
        public virtual long OperationId { get; set; }
        public virtual long? LienDetailId { get; set; }

        public virtual decimal MontantBudget { get; set; }

        //       public virtual OperationModel Operation { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", Commentaire);
        }
    }
}