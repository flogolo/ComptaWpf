namespace MaCompta.Common
{
    /// <namespace>SilverlightApplication.Commun</namespace>
    /// <file>ComboBoxItem.cs</file>
    /// <created>04/04/2010</created>
    /// <author>FSC</author>
    /// <summary>
    /// Object servant au provisionning des combobox
    /// </summary>
    /// <typeparam name="T">Classe de base de l'objet</typeparam>
    public class ComboBoxItem<T>
    {

        #region Constructors
        /// <summary>
        /// Construteur de la classe
        /// </summary>
        /// <param name="id">L'identifiant unique associé à l'item</param>
        /// <param name="name">Le nom (string) tel qu'il apparaitra dans la combobox</param>
        /// <param name="typedValue">L'object asssocié</param>
        public ComboBoxItem(int id, string name, T typedValue)
        {
            Id = id;
            Name = name;
            TypedValue = typedValue;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Nom de l'objet
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Identifiant de l'objet
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// instance de l'Objet associé
        /// </summary>
        public T TypedValue { get; private set; }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }

}
