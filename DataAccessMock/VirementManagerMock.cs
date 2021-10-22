using System.Collections.Generic;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;

namespace DataAccessMock
{
    public class VirementManagerMock: BaseManagerMock<VirementModel>,IVirementService
    {
        private readonly IVirementDetailService _DetailService ;
        private readonly IVirementDetailMontantService _MontantService;

        public SortableObservableCollection<string> AllOrdres
        {
            get;
            private set;
        }

        public VirementManagerMock(IVirementDetailService detailService, IVirementDetailMontantService montantService)
        {
            ModelName = "VirementModel";
            _DetailService = detailService;
            _MontantService = montantService;
            AllOrdres = new SortableObservableCollection<string>();
        }

        public override void CopyTo(VirementModel modelDst, VirementModel modelSrc)
        {
            modelDst.CompteDstId = modelSrc.CompteDstId;
            modelDst.CompteSrcId = modelSrc.CompteSrcId;
            modelDst.DateDernierVirement = modelSrc.DateDernierVirement;
            modelDst.Description = modelSrc.Description;
            modelDst.Duree = modelSrc.Duree;
            modelDst.Jour = modelSrc.Jour;
            modelDst.Ordre = modelSrc.Ordre;
            modelDst.Frequence = modelSrc.Frequence;
            modelDst.Montant = modelSrc.Montant;
        }


        public void CreateVirementWithDetails(VirementModel model)
        {
            CreateItem(model);
            foreach (var detail in model.Details)
            {
                detail.VirementId = model.Id;
                _DetailService.CreateItem(detail);
                foreach (var montant in detail.Montants)
                {
                    montant.VirementDetailId = detail.Id;
                    _MontantService.CreateItem(montant);
                }
            }
        }


    }
}
