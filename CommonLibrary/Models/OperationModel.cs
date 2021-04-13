using System;
using System.Collections.Generic;

namespace CommonLibrary.Models
{
    public class OperationModel: ModelBase
    {
        public virtual long CompteId { get; set; }
        public virtual DateTime? DateOperation { get; set; }
        public virtual DateTime? DateValidation { get; set; }
        public virtual DateTime? DateValidationPartielle { get; set; }
        public virtual String Ordre { get; set; }
        public virtual String TypePaiement { get; set; }
        public virtual String NumeroCheque { get; set; }
        public virtual Boolean Report { get; set; }
        public virtual Boolean IsVirementAuto { get; set; }
        public virtual decimal MontantBudget { get; set; }
        public virtual IList<DetailModel> Details { get; set; }
        public virtual long? LienOperationId { get; set; }

        public OperationModel()
        {
            Details = new List<DetailModel>();
        }
        //public virtual ChequeModel Cheque { get; set; }

        //public virtual String NumeroCheque
        //{
        //    get
        //    {
        //        if (Cheque != null) return Cheque.Numero;
        //        return String.Empty;
        //    }
        //}

        public virtual Boolean IsCheque
        { 
            get
            {
                return TypePaiement!=null && TypePaiement.Equals("CHQ");
            }
        }        

        //public OperationModel()
        //{
        //    Details = new List<DetailModel>();

        //}


        public override string ToString()
        {
            return string.Format("{0}", Ordre);
        }
    }
}