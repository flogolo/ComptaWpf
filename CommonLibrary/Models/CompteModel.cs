using System.Collections.Generic;

namespace CommonLibrary.Models
{
    public class CompteModel: ModelBase
    {
        public virtual string Numero { get; set; }
        public virtual string Designation { get; set; }
        public virtual string CarteBancaire { get; set; }
        public virtual long ComptaId { get; set; }
        public virtual long BanqueId { get; set; }
        public virtual bool IsActif { get; set; }

        public virtual List<OperationModel> Operations { get; protected set; }

        //public CompteModel()
        //{
        //    Operations = new List<OperationModel>();
        //}

        public override string ToString()
        {
            return string.Format("{0}", Designation);
        }
    }
}