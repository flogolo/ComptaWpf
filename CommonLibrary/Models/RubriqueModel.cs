namespace CommonLibrary.Models
{
    public class RubriqueModel: ModelBase
    {
        public virtual string Libelle { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", Libelle);
        }
    }
}
