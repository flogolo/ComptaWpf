using System;
using PortableCommon.IOC;
using PortableCommon.Models;

namespace WpfApplication.ViewModels
{
    public class ChequeViewModel: ModelViewModelBase<ChequeViewModel, ChequeModel>
    {
        private string m_Numero;

        public ChequeViewModel(IContainer container) : base(container)
        {
        }

        public string Numero
        {
            get { return m_Numero; }
            set { m_Numero = value;
                RaisePropertyChanged(vm=>vm.Numero);
            }
        }

        public int OperationId { get; set; }

        public override void ActionDupliquer()
        {
            throw new NotImplementedException();
        }

        public override void ActionSupprimer()
        {
            throw new NotImplementedException();
        }

        public override void ActionSauvegarder()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialisation du view model à partir d'un objet model
        /// </summary>
        /// <param name="model"></param>
        public override void CancelModel(ChequeModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialisation du view model à partir d'un objet model
        /// </summary>
        /// <param name="model"></param>
        public override ChequeViewModel InitFromModel(ChequeModel model)
        {
            Id = model.Id;
            OperationId = model.OperationId;
            Numero = model.Numero;
            return this;
        }

        /// <summary>
        /// renvoie un objet model initialisé à partir du view model
        /// </summary>
        /// <returns></returns>
        public override ChequeModel SaveToModel()
        {
            return new ChequeModel
                       {
                           Id = Id,
                           Numero = Numero,
                           OperationId = OperationId
                       };
        }

        public override ChequeViewModel DuplicateViewModel()
        {
            throw new NotImplementedException();
        }
    }
}
