using System;

namespace CommonLibrary.Tools
{
    /// <summary>
    /// EventArgs avec paramètre fortement typé.
    /// </summary>
    /// <typeparam name="T">Type de paramètre.</typeparam>
    public class EventArgs<T> : EventArgs
    {
        #region Constructors
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public EventArgs()
        {
        }

        /// <summary>
        /// Constructeur avec donnée
        /// </summary>
        /// <param name="data">donnée associé à l'événement.</param>
        public EventArgs(T data)
        {
            Data = data;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Donnée de l'événement.
        /// </summary>
        public T Data { get; set; }

        #endregion
    }
}
