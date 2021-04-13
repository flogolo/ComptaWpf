using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace DataAccess.Managers
{
    public class DetailManager: BaseManager<DetailModel>,IDetailService
    {
        public DetailManager()
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
