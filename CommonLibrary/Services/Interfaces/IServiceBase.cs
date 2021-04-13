using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommonLibrary.Models;
using CommonLibrary.Tools;

namespace CommonLibrary.Services.Interfaces
{

    public class HandledEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Donn�e de l'�v�nement.
        /// </summary>
        public T Data { get; set; }
    }
    
    public interface IServiceBase<T> where T : IModel
    {
        string ModelName
        {
            get;
        }
        /// <summary>
        /// liste des objets m�tier
        /// </summary>
        Collection<T> ItemsList { get; }
        /// <summary>
        /// Chargement de la liste des objets m�tiers
        /// </summary>
        void LoadItems();

        /// <summary>
        /// suppresion d'un item
        /// </summary>
        /// <param name="itemId">identifiant</param>
        /// <param name="cascade">faut-il supprimer les �l�ments d�pendants</param>
        void DeleteItem(long itemId, bool cascade);

        /// <summary>
        /// Cr�ation d'un item.
        /// </summary>
        /// <param name="model">model � cr�er</param>
        /// <returns>retourne l'identifiant de l'item cr��</returns>
        void CreateItem(T model);

        void UpdateItem(T model);

        /// <summary>
        /// Cr�ation d'une liste d'items
        /// </summary>
        /// <param name="list"></param>
        void CreateItems(IEnumerable<T> list);

        event EventHandler<ErrorEventArgs> ErrorOccured;
        event EventHandler<EventArgs<String>> LogRequested;
    }
}