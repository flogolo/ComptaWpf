namespace PortableCommon.Models
{
    public class ChequeModel : ModelBase
    {
        public virtual string Numero { get; set; }
        public virtual int OperationId { get; set; }

    }
}