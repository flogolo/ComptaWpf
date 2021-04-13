using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MaCompta.Common
{
    /// <namespace>SilverlightApplication.Commun</namespace>
    /// <file>ComboBoxItemCollection.cs</file>
    /// <created>08/06/2010</created>
    /// <author>FSC</author>
    /// <summary>
    /// Collection servant au provisionning des combobox
    /// </summary>
    /// <typeparam name="T">Classe de base de la collection</typeparam>
    public class ComboBoxItemCollection<T> : ObservableCollection<ComboBoxItem<T>>
    {
        #region Members
        /// <summary>
        /// Obtient le BaseListObjet relatif à l'élement "Aucun"
        /// </summary>
        public ComboBoxItem<T> NoneObject { get; private set; }
        /// <summary>
        /// Obtient le BaseListObjet relatif à l'élement "Aucune"
        /// </summary>
        public ComboBoxItem<T> None2Object { get; private set; }
        #endregion

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public ComboBoxItemCollection()
        {
            NoneObject = new ComboBoxItem<T>(-1, "Aucun", default(T));
            None2Object = new ComboBoxItem<T>(-1, "aucune", default(T));
        }

        /// <summary>
        /// Constructeur avec une liste d'items en paramètres
        /// </summary>
        /// <param name="enumerable">Liste d'items à ajouter à la liste</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public ComboBoxItemCollection(IEnumerable<ComboBoxItem<T>> enumerable): this()
        {
            foreach (var item in enumerable)
            {
                Add(item);                
            }
        }

        /// <summary>
        /// Recherche si l'élément est dans la liste fournie
        /// </summary>
        /// <param name="id">identifiant de l'élément à rechercher</param>
        /// <returns>élément trouvé de la liste ou null si l'élément n'a pas été trouvé</returns>
        public ComboBoxItem<T> FindObject(int id)
        {
            if (Count > 0)
                return this.Where(o => o.Id == id).FirstOrDefault();
            return null;
        }

        /// <summary>
        /// Recherche si l'élément est dans la liste fournie et retourne l'obejt par défaut si pas trouvé
        /// </summary>
        /// <param name="id">identifiant de l'élément à rechercher</param>
        /// <param name="defaultObject">objet par défaut à retourner</param>
        /// <returns>élément trouvé de la liste ou defaultobject si l'élément n'a pas été trouvé</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ObjectOr")]
        public ComboBoxItem<T> FindObjectOrDefault(int id, ComboBoxItem<T> defaultObject)
        {
            ComboBoxItem<T> objectFound = null;
            if (Count > 0)
                objectFound = this.Where(o => o.Id == id).FirstOrDefault();
            if (objectFound != null)
                return objectFound;
            return defaultObject;
        }

        /// <summary>
        /// Recherche si l'élément est dans la liste fournie et retourne l'obejt par défaut si pas trouvé
        /// </summary>
        /// <param name="id">identifiant de l'élément à rechercher</param>
        /// <param name="defaultId">identifiant de l'objet par défaut à retourner</param>
        /// <returns>élément trouvé de la liste ou defaultobject si l'élément n'a pas été trouvé</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ObjectOr")]
        public ComboBoxItem<T> FindObjectOrDefaultId(int id, int defaultId)
        {
            ComboBoxItem<T> objectFound = null;
            if (Count > 0)
                objectFound = this.Where(o => o.Id == id).FirstOrDefault();
            if (objectFound != null)
                return objectFound;
            return this.Where(o => o.Id == defaultId).FirstOrDefault();
        }

        ///// <summary>
        ///// Remplit la ComboxBoxItemCollection à partir d'un Enum
        ///// le nom sera recherché dans le manager de ressources fourni
        ///// </summary>
        ///// <param name="typeEnum">Type Enum</param>
        ///// <param name="manager">Manager de ressources</param>
        //public void CreateListFromEnum(Type typeEnum, ResourceManager manager)
        //{
        //    foreach (var value in Enum.GetValues(typeEnum))
        //    {
        //        string name = manager.GetString(Enum.GetName(typeEnum, value),CultureInfo.CurrentCulture);
        //        Add(
        //            new ComboBoxItem<T>(
        //            (int)value,
        //            name,
        //            (T)value
        //            ));
        //    }
        //}

        /// <summary>
        /// Supprime l'élément de la liste
        /// </summary>
        /// <param name="id">Identifiant à supprimer</param>
        /// <returns>Nouvelle liste sans l'objet demandé</returns>
        public void RemoveId(int id)
        {
            if (Count > 0)
            {
                var res = this.Where(o => o.Id == id).FirstOrDefault();
                if (res != null && Contains(res))
                    Remove(res);
            }
        }
    }
}
