namespace CommonLibrary.Models
{
    /// <summary>
    /// Contient le montant pour un detail
    /// </summary>
    public class VirementMontantModel : ModelBase
    {
        public virtual long VirementDetailId { get; set; }
        public virtual decimal Montant { get; set; }
        public virtual int Mois { get; set; }

    }
}
