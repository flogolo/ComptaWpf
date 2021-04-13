using PortableCommon.IOC;

namespace PortableCommon
{
    public abstract class SimpleIocFactory
    {



        ///// <summary>
        ///// Access to get the singleton object.
        ///// Then call methods on that instance.
        ///// </summary>
        //public abstract SimpleIocFactory Instance
        //{
        //    get;
        //}



        public abstract void ConfigureViewModels();

    }
}
