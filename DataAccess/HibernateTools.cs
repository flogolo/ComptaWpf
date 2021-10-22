using NHibernate;
using NHibernate.Cfg;

namespace DataAccess
{
    public class HibernateTools
    {
        private ISession _session;
        private  ITransaction _transaction;
        private bool _withCommit;

        public ISession Session
        {
            get
            {
                if (_session == null)
                    LoadDataBase();
                return _session;
            }
        }

        private static HibernateTools _instance;

        private HibernateTools() { }

        public static HibernateTools Instance
        {
            get { return _instance ?? (_instance = new HibernateTools()); }
        }

        private void LoadDataBase()
        {
            var config = new Configuration();
            config.Configure();
            ISessionFactory factory = config.BuildSessionFactory();
            _session = factory.OpenSession();
        }

        public void CloseSession()
        {
            _session.Close();
            _session = null;
        }

        /// <summary>
        /// début d'une transaction
        /// appelé au début d'un série d'opération ou dans une opération
        /// </summary>
        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = Session.BeginTransaction();
                _withCommit = true;
            }
            else
            {
                //transation déjà existante -> pas de commit
                _withCommit = false;
            }
        }

        /// <summary>
        /// commit de la transaction
        /// si suite d'opération -> pas de commit
        /// </summary>
        public void CommitTransaction()
        {
            if (_withCommit)
            {
                _transaction.Commit();
                _transaction = null;
            }
        }

        /// <summary>
        /// appelé en fin de suite d'opération
        /// </summary>
        public void EndTransaction()
        {
            _withCommit = true;
            CommitTransaction();
        }
    }
}
