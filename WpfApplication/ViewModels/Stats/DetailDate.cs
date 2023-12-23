namespace MaCompta.ViewModels
{
    public class DetailDate
    {
        public decimal Montant { get; set; }
        public string DateDetail { get; set; }
        public string Ordre { get; set; }

        public string Commentaire
        {
            get;
            set;
        }
    }

}
