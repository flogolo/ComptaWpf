using CommonLibrary.Tools;

namespace MaCompta.ViewModels
{
    public class StatRubriqueModel
    {
        public StatRubriqueModel()
        {
            SousRubriqueStats = new SortableObservableCollection<StatSousRubriqueModel>();
        }

        //libellé de la rubrique
        public string Libelle { get; set; }
        //identifiant de la rubrique
        public long Id { get; set; }
        //montant pour la rubrique
        public decimal Montant { get; set; }

        //stats des sous-rubriques
        public SortableObservableCollection<StatSousRubriqueModel> SousRubriqueStats { get; set; }
    }

}
