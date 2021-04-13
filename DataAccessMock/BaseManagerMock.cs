using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;

namespace DataAccessMock
{
    public class BaseManagerMock<T> : IServiceBase<T> where T : IModel
    {
        int id = 1;
        public event EventHandler<ErrorEventArgs> ErrorOccured;
        public event EventHandler<EventArgs<string>> LogRequested;

        public string ModelName
        {
            get;
            protected set;
        }
    
        public BaseManagerMock()
        {
            ItemsList = new Collection<T>();
        }

        public Collection<T> ItemsList { get; private set; }

        /// <summary>
        /// Recopie du model dans un autre (hors Id)
        /// </summary>
        /// <param name="modelDst"></param>
        /// <param name="modelSrc"></param>
        public virtual void CopyTo(T modelDst, T modelSrc) 
        {
            throw new NotImplementedException(ModelName + " : CopyTo not implemented");
        }

        protected void RaiseErrorOccured(String errorMessage)
        {
            if (ErrorOccured != null)
                ErrorOccured(this, new ErrorEventArgs(errorMessage));
        }

        public virtual void LoadItems()
        {
        }

        //public event EventHandler ItemsCreated;

        /// <summary>
        /// Suppression d'un item
        /// </summary>
        /// <param name="itemId"></param>
        public virtual void DeleteItem(long itemId, bool cascade)
        {
            Debug(String.Format("Deleting {0} {1} ...", ModelName, itemId));

            var item = ItemsList.First(i=>i.Id == itemId);
            ItemsList.Remove(item);
        }

        /// <summary>
        /// Création d'un item
        /// </summary>
        /// <param name="model"></param>
        public virtual void CreateItem(T model)
        {
            model.Id = id++;
            ItemsList.Add(model);
        }

        /// <summary>
        /// Mise à jour d'un item
        /// </summary>
        /// <param name="model"></param>
        public virtual void UpdateItem(T model)
        {
            Debug(String.Format("Updating {0} {1} ...", ModelName, model.Id));

            var data = ItemsList.FirstOrDefault(i => i.Id == model.Id);
            if (data != null)
            {
                CopyTo(data, model);
                data.UpdatedAt = DateTime.Now;
            }
        }

        public virtual void CreateItems(IEnumerable<T> list)
        {
            throw new NotImplementedException();
        }

        protected void Debug(String message)
        {
            System.Diagnostics.Debug.WriteLine("{0} : {1}", DateTime.Now, message);
        }


    }
}
