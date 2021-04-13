
namespace CommonLibrary.Models
{
    public class BanqueModel : ModelBase
    {
        public virtual string Description { get; set; }
        public virtual string Adresse { get; set; }
        public virtual string CodePostal { get; set; }
        public virtual string Ville { get; set; }
        public virtual string Notes { get; set; }
    }
}