using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MaCompta.ViewModels
{
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

}
