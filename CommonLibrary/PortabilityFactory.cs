using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PortableCommon.ViewModels;

namespace PortableCommon
{
    public abstract class PortabilityFactory
    {
        private static PortabilityFactory m_Factory;
        
        public static PortabilityFactory Instance
        {
            get
            {
                if (m_Factory == null)
                {
                    throw new InvalidOperationException();
                }
                return m_Factory;
            }
            set
            {
                m_Factory = value;
            }
        }

        protected PortabilityFactory()
        {
            Instance = this;
        }

        public virtual Collection<T> CreateList<T>()
        {
            return new Collection<T>();
        }

        public virtual IList<T> CreateSortableList<T>()
        {
            return new List<T>();
        }


        public abstract string UrlEncode(string url);
        public abstract string UrlDecode(string url);

        public abstract void LogMessage(string message);

        public abstract PortableRubriqueViewModel GetRubrique(int rubriqueId);

        public abstract PortableSousRubriqueViewModel GetSousRubrique(PortableRubriqueViewModel rubriqueVm,
                                                                      int sousRubriqueId);

        public abstract IList<PortableRubriqueViewModel> Rubriques { get;  }
        public abstract IList<string> Ordres { get;  }
        public abstract IList<PortableComptaViewModel> Comptas { get; }
        public abstract IList<PortableCompteViewModel> Comptes { get; }
        public abstract IList<PortableBanqueViewModel> Banques { get; }

    }
}

