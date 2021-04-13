using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace DataAccess.Managers
{
    public class CompteManager: BaseManager<CompteModel>, ICompteService
    {
        public CompteManager()
        {
            ModelName = "CompteModel";
        }

        public override void CopyTo(CompteModel modelDst, CompteModel modelSrc)
        {
            modelDst.BanqueId = modelSrc.BanqueId;
            modelDst.CarteBancaire = modelSrc.CarteBancaire;
            modelDst.Designation = modelSrc.Designation;
            modelDst.Numero = modelSrc.Numero;
            modelDst.ComptaId = modelSrc.ComptaId;
            modelDst.IsActif = modelSrc.IsActif;
        }
    }
}
