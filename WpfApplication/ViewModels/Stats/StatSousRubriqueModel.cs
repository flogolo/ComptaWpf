using System.Collections.ObjectModel;

namespace MaCompta.ViewModels
{
    #region Stats rubrique/sous-rubrique

    public class StatSousRubriqueModel
    {
        public StatSousRubriqueModel()
        {
            Details = new ObservableCollection<DetailDate>();
        }

        //libellé de la sous-rubrique
        public string Libelle { get; set; }
        //identifiant de la sous-rubrique
        public long Id { get; set; }
        //montant pour la sous-rubrique
        public decimal Montant { get; set; }

        //détails des opérations
        public ObservableCollection<DetailDate> Details { get; private set; }

        /// <summary>
        /// Compare l'objet en cours à un autre objet du même type.
        /// </summary>
        /// <returns></returns>
        /// <param name="other">Objet à comparer avec cet objet.</param>
        public int CompareTo(StatSousRubriqueModel other)
        {
            return Libelle.CompareTo(other.Libelle);
        }
    }

}
