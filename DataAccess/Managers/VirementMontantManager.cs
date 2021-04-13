using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace DataAccess.Managers
{
    public class VirementMontantManager: BaseManager<VirementMontantModel>,IVirementDetailMontantService
    {
        public VirementMontantManager()
        {
            ModelName = "VirementMontantModel";
        }

        public override void CopyTo(VirementMontantModel modelDst, VirementMontantModel modelSrc)
        {
            modelDst.Montant = modelSrc.Montant;
        }

    }
}
