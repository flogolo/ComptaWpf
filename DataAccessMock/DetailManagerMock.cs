using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace DataAccessMock
{
    public class DetailManagerMock: BaseManagerMock<DetailModel>,IDetailService
    {
        public DetailManagerMock()
        {
            ModelName = "DetailModel";
        }

        public override void CopyTo(DetailModel modelDst, DetailModel modelSrc)
        {
            modelDst.Montant = modelSrc.Montant;
            modelDst.RubriqueId = modelSrc.RubriqueId;
            modelDst.SousRubriqueId = modelSrc.SousRubriqueId;
            modelDst.Commentaire = modelSrc.Commentaire;
        }
    }
}
