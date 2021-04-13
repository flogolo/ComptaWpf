namespace CommonLibrary.Models
{
    public class SousRubriqueModel : ModelBase
    {
        public virtual long RubriqueId { get; set; }
        public virtual string Libelle { get; set; }


        public override string ToString()
        {
            return string.Format("{0}", Libelle);
        }
    }
}
