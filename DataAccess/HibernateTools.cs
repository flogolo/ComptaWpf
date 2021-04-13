using NHibernate;
using NHibernate.Cfg;

namespace DataAccess
{
    public class HibernateTools
    {
        private ISession _session;

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

    }
}
