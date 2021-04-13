using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace DataAccess.Managers
{
    public class OperationPredefinieManager: BaseManager<OperationPredefinieModel>, IOperationPredefinieService
    {
        public OperationPredefinieManager()
        {
            ModelName = "OperationPredefinieModel";
        }

        public override void CopyTo(OperationPredefinieModel modelDst, OperationPredefinieModel modelSrc)
        {
            modelDst.Commentaire = modelSrc.Commentaire;
            modelDst.Ordre = modelSrc.Ordre;
            modelDst.RubriqueId = modelSrc.RubriqueId;
            modelDst.SousRubriqueId = modelSrc.SousRubriqueId;
            modelDst.TypePaiement = modelSrc.TypePaiement;
        }
    }
}
