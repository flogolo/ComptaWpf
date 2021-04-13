using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace DataAccess.Managers
{
    public class VirementDetailManager: BaseManager<VirementDetailModel>,IVirementDetailService
    {
        public VirementDetailManager()
        {
            ModelName = "VirementDetailModel";
        }

        public override void CopyTo(VirementDetailModel modelDst, VirementDetailModel modelSrc)
        {
            modelDst.Commentaire = modelSrc.Commentaire;
            modelDst.IsCompteDstOnly = modelSrc.IsCompteDstOnly;
            modelDst.IsCompteSrcOnly = modelSrc.IsCompteSrcOnly;
            modelDst.RubriqueId = modelSrc.RubriqueId;
            modelDst.SousRubriqueId = modelSrc.SousRubriqueId;

        }

    }
}
