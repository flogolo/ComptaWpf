using GalaSoft.MvvmLight.Threading;
using System.Windows;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace MaCompta
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        //private WpfPortabilityFactory m_Pf = new WpfPortabilityFactory();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            WpfIocFactory.Instance.Configure();
            DispatcherHelper.Initialize();
        }
    }
}
