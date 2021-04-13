using PortableCommon.Models;
using PortableCommon.Services.Interfaces;

namespace DataAccess.Managers
{
    public class ChequeManager: BaseManager<ChequeModel>, IChequeService
    {
        public ChequeManager()
        {
            ModelName = "ChequeModel";
        }

        public override void CopyTo(ChequeModel modelDst, ChequeModel modelSrc)
        {
            modelDst.Numero = modelSrc.Numero;
        }
    }
}
