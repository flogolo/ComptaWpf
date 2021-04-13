using System;
using System.Collections.Generic;

namespace CommonLibrary.Models
{
    public class VirementModel: ModelBase
    {
        public virtual string Description { get; set; }
        public virtual long CompteSrcId { get; set; }
        public virtual long CompteDstId { get; set; }
        public virtual int Jour { get; set; }
        public virtual int Duree { get; set; }
        public virtual string Ordre { get; set; }
        public virtual int Frequence { get; set; }
        //utilisé si fréquence annuelle ou hebdomadaire
        public virtual decimal Montant { get; set; }

        public virtual DateTime? DateDernierVirement { get; set; }
        public virtual String TypePaiement { get; set; }

        /// <summary>
        /// Détails des opérations
        /// </summary>
        public virtual List<VirementDetailModel> Details { get; set; }

        public VirementModel()
        {
            Details = new List<VirementDetailModel>();
  
        }

        public override string ToString()
        {
            return string.Format("{0}", Ordre);
        }
    }
}
