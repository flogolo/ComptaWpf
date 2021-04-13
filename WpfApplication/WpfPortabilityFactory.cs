using System.Collections.ObjectModel;
using WpfApplication.ViewModels;

namespace WpfApplication
{
    class WpfIocFactory
    {
        private static WpfIocFactory m_Factory;
        
        public static WpfIocFactory Instance
        {
            get
            {
                if (m_Factory == null)
                {
                    m_Factory = new WpfIocFactory();
                }
                return m_Factory;
            }
        }


        //public ObservableCollection<string> Ordres
        //{
        //    get { throw new System.NotImplementedException(); }
        //}


    }
}
