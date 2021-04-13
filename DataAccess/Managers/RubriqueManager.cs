using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace DataAccess.Managers
{
    public class RubriqueManager: BaseManager<RubriqueModel>, IRubriqueService
    {
        public RubriqueManager()
        {
            ModelName = "RubriqueModel";
        }

        public override void CopyTo(RubriqueModel modelDst, RubriqueModel modelSrc)
        {
            modelDst.Libelle = modelSrc.Libelle;
        }
    }
}
