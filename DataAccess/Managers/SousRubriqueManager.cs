using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace DataAccess.Managers
{
    public class SousRubriqueManager: BaseManager<SousRubriqueModel>, ISousRubriqueService
    {
        public SousRubriqueManager()
        {
            ModelName = "SousRubriqueModel";
        }

        public override void CopyTo(SousRubriqueModel modelDst, SousRubriqueModel modelSrc)
        {
            modelDst.Libelle = modelSrc.Libelle;
            //rubriqueId ne peut pas être modifié
        }

    }
}
