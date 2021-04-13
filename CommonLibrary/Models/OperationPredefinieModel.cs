using System;

namespace CommonLibrary.Models
{
    public class OperationPredefinieModel: ModelBase
    {
        public virtual String Ordre { get; set; }
        public virtual String TypePaiement { get; set; }
        public virtual long RubriqueId { get; set; }
        public virtual long SousRubriqueId { get; set; }
        public virtual String Commentaire { get; set; }

    }
}