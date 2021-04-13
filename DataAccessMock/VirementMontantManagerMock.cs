using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace DataAccessMock
{
    public class VirementMontantManagerMock: BaseManagerMock<VirementMontantModel>,IVirementDetailMontantService
    {
        public VirementMontantManagerMock()
        {
            ModelName = "VirementMontantModel";
        }

        public override void CopyTo(VirementMontantModel modelDst, VirementMontantModel modelSrc)
        {
            modelDst.Montant = modelSrc.Montant;
        }

    }
}
