using CommonLibrary.Tools;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MaCompta.ViewModels
{
    #region Stats rubrique/sous-rubrique
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

    #endregion

    #region stats rubrique/mois
    public class DetailStatMulti
    {
        public DetailStatMulti()
        {
            Stats = new SortableObservableCollection<DetailStatSerie>();
        }

        public string Libelle { get; set; }
        public long Id { get; set; }

        //        public Brush FavoriteColor { get; set; }
        public SortableObservableCollection<DetailStatSerie> Stats { get; set; }
    }

    public class DetailStatSerie : IComparable<DetailStatSerie>
    {
        public DetailStatSerie()
        {
            Details = new ObservableCollection<DetailDate>();
        }

        public ObservableCollection<DetailDate> Details { get; private set; }
        public decimal Montant { get; set; }
        public int Mois { get; set; }

        public string MoisStr
        {
            get
            {
                if (DateTimeFormatInfo.CurrentInfo != null) return DateTimeFormatInfo.CurrentInfo.MonthNames[Mois - 1];
                return string.Empty;
            }
        }

        /// <summary>
        /// Compare l'objet en cours à un autre objet du même type.
        /// </summary>
        /// <returns>
        /// Entier signé 32 bits qui indique l'ordre relatif des objets comparés. La valeur de retour a les significations suivantes : 
        ///                     Valeur 
        ///                     Signification 
        ///                     Inférieur à zéro 
        ///                     Cet objet est inférieur au paramètre <paramref name="other"/>.
        ///                     Zéro 
        ///                     Cet objet est égal à <paramref name="other"/>. 
        ///                     Supérieure à zéro 
        ///                     Cet objet est supérieur à <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">Objet à comparer avec cet objet.</param>
        public int CompareTo(DetailStatSerie other)
        {
            return Mois.CompareTo(other.Mois);
        }
    }

    #endregion


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
