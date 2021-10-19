using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;
using System.Linq;

namespace DataAccess.Managers
{
    public class BaseManager<T> : IServiceBase<T> where T : IModel
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public event EventHandler<ErrorEventArgs> ErrorOccured;
        public event EventHandler<EventArgs<String>> LogRequested;

        public string ModelName
        {
            get;
            protected set;
        }
    
        public BaseManager()
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
            try
            {
                var dateBefore = DateTime.Now;
                Debug("Loading " + ModelName + "...");
                ItemsList.Clear();
                //ISession session = HibernateTools
                var session = HibernateTools.Instance.Session;

                var tx = session.BeginTransaction();

                var models = session.CreateQuery("from " + ModelName).List<T>();
                tx.Commit();

                foreach (var model in models)
                {
                    ItemsList.Add(model);
                }
                var dateAfter = DateTime.Now;
                Debug(String.Format("{0} loaded ({1} items) {2} ms", ModelName, models.Count,
                    (dateAfter - dateBefore).TotalMilliseconds));
            }
            catch (Exception ex)
            {
                Debug(ex.Message);
                RaiseErrorOccured(ex.Message);
            }
        }

        //public event EventHandler ItemsCreated;

        /// <summary>
        /// Suppression d'un item
        /// </summary>
        /// <param name="itemId"></param>
        public virtual void DeleteItem(long itemId, bool cascade)
        {
            if (itemId <= 0) return;

            Debug(String.Format("Deleting {0} {1} ...", ModelName, itemId));

            var session = HibernateTools.Instance.Session;

            var tx = session.BeginTransaction();
            var model = session.Get<T>(itemId);
            if (model != null)
            { 
                session.Delete(model);
                ItemsList.Remove(model);
            }
            tx.Commit();
        }

        /// <summary>
        /// Création d'un item en base -> retourne le model avec l'identifiant mis à jour
        /// </summary>
        /// <param name="model"></param>
        public virtual void CreateItem(T model)
        {
            Debug(String.Format("Creating {0} {1} ...", ModelName, model.Id));

            var session = HibernateTools.Instance.Session;

            var tx = session.BeginTransaction();

            model.CreatedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            var saved = session.Save(model);
            tx.Commit();
            model.Id = (long)saved;
            ItemsList.Add(model);
        }

        /// <summary>
        /// Mise à jour d'un item
        /// </summary>
        /// <param name="model"></param>
        public virtual void UpdateItem(T model)
        {
            Debug(String.Format("Updating {0} {1} ...", ModelName, model.Id));

            var session = HibernateTools.Instance.Session;

            var item = ItemsList.FirstOrDefault(i=>i.Id == model.Id);
            CopyTo(item, model);

            var tx = session.BeginTransaction();
            var data = session.Get<T>(model.Id);
            if (data != null)
            {
                CopyTo(data, model);
                data.UpdatedAt = DateTime.Now;
                session.Update(data);
            }

            tx.Commit();
        }

        public virtual void CreateItems(IEnumerable<T> list)
        {
            throw new NotImplementedException();
        }

        protected void Debug(String message)
        {
            System.Diagnostics.Debug.WriteLine(message);
            Log.DebugFormat("{0} : {1}", DateTime.Now, message);
            if (LogRequested!=null)
                LogRequested(this, new EventArgs<string>(message));
        }

        public bool TestConnexion()
        {
            try
            {
                var session = HibernateTools.Instance.Session;
                return true;
            } catch(Exception e)
            {
                RaiseErrorOccured("Impossible de se connecter à la base de données " + e.Message);
                return false;
            }
        }
    }
}
