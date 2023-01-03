using NHibernate;
using NHibernate.Connection;
using System.Collections.Generic;

namespace DataAccess
{
    public class MaComptaConnectionProvider : DriverConnectionProvider
    {
        private string _connectionString;
        public override void Configure(IDictionary<string, string> settings)
        {

            // Connection string in the configuration overrides named connection string
            if (!settings.TryGetValue(NHibernate.Cfg.Environment.ConnectionString, out _connectionString))
                _connectionString = GetNamedConnectionString(settings);

#if DEBUG
           //_connectionString = _connectionString.Replace("macompta", "macompta_test");
#endif
            if (_connectionString == null)
            {
                throw new HibernateException("Could not find connection string setting (set "
                    + NHibernate.Cfg.Environment.ConnectionString + " or "
                    + NHibernate.Cfg.Environment.ConnectionStringName + " property)");
            }
            ConfigureDriver(settings);
        }

        protected override string ConnectionString
        {
            get {
                return _connectionString;
            }
        }
    }
}
