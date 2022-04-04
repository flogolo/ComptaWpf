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
        /// Donnée de l'événement.
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
        /// liste des objets métier
        /// </summary>
        Collection<T> ItemsList { get; }
        /// <summary>
        /// Chargement de la liste des objets métiers
        /// </summary>
        void LoadItems();

        /// <summary>
        /// suppresion d'un item
        /// </summary>
        /// <param name="itemId">identifiant</param>
        /// <param name="cascade">faut-il supprimer les éléments dépendants</param>
        void DeleteItem(long itemId, bool cascade);

        /// <summary>
        /// Création d'un item.
        /// </summary>
        /// <param name="model">model à créer</param>
        /// <returns>retourne l'identifiant de l'item créé</returns>
        void CreateItem(T model);

        void UpdateItem(T model);

        /// <summary>
        /// Création d'une liste d'items
        /// </summary>
        /// <param name="list"></param>
        void CreateItems(IEnumerable<T> list);

        event EventHandler<ErrorEventArgs> ErrorOccured;
        event EventHandler<EventArgs<String>> LogRequested;

        bool TestConnexion();

        void BeginTransaction();
        void CommitTransaction();
        void EndTransaction();
    }
}