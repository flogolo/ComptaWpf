using CommonLibrary.Services.Interfaces;
using CommonLibrary.IOC;

namespace DataAccessMock
{
    public class WpfIocFactoryMock
    {
        public IContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Allocate ourselves. We have a private constructor, so no one else can.
        /// </summary>
        protected static WpfIocFactoryMock InternalInstance;

        /// <summary>
        /// Access to get the singleton object.
        /// Then call methods on that instance.
        /// </summary>
        public static WpfIocFactoryMock Instance
        {
            get
            {
                if (InternalInstance == null)
                {
                    System.Diagnostics.Debug.WriteLine("WpfIocFactoryMock Instance"); 
                    InternalInstance = new WpfIocFactoryMock();
                }
                return InternalInstance;
            }
        }

        protected WpfIocFactoryMock()
        {
            System.Diagnostics.Debug.WriteLine("WpfIocFactoryMock constructor : new container");
            Container = new SimpleIocContainer();
        }

        public void Configure()
        {
            //Attention ordre important
            //Container.Register<IRubriqueService, RubriqueManager>();
            //Container.Register<ISousRubriqueService, SousRubriqueManager>();

            //Container.Register<IComptaService, ComptaManager>();
            Container.Register<IDetailService, DetailManagerMock>();
            Container.Register<IOperationService, OperationManagerMock>();
            //Container.Register<IOperationPredefinieService, OperationPredefinieManager>();
            //Container.Register<ICompteService, CompteManager>();
            Container.Register<IVirementDetailMontantService, VirementMontantManagerMock>();
            Container.Register<IVirementDetailService, VirementDetailManagerMock>();
            Container.Register<IVirementService, VirementManagerMock>();
            //Container.Register<IBanqueService, BanqueManager>();
        }

    }
}
