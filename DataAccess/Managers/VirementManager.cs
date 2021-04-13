using System.Collections.Generic;
using System.Linq;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;
using System;

namespace DataAccess.Managers
{
    public class VirementManager: BaseManager<VirementModel>,IVirementService
    {
        private readonly IVirementDetailService _detailService ;
        private readonly IVirementDetailMontantService _montantService;

        public SortableObservableCollection<string> AllOrdres
        {
            get;
            private set;
        }

        public VirementManager(IVirementDetailService detailService, IVirementDetailMontantService montantService)
        {
            ModelName = "VirementModel";
            _detailService = detailService;
            _montantService = montantService;
            AllOrdres = new SortableObservableCollection<string>();
        }

        public override void LoadItems()
        {
            base.LoadItems();
            _detailService.LoadItems();
            _montantService.LoadItems();

            foreach (var virement in ItemsList)
            {
                if (!AllOrdres.Contains(virement.Ordre))
                    AllOrdres.Add(virement.Ordre);

                var details = _detailService.ItemsList.Where(
                    i => i.VirementId == virement.Id);
                virement.Details = new List<VirementDetailModel>();
                foreach (var detail in details)
                {
                    virement.Details.Add(detail);
                    var montants = _montantService.ItemsList.Where(
                        m => m.VirementDetailId == detail.Id);
                    //detail.Montants = new List<VirementMontantModel>();
                    foreach (var montant in montants)
                    {
                        detail.Montants.Add(montant);
                    }
                }
            }
            AllOrdres.Sort();
            Debug(String.Format("VirementDetailModel loaded ({1} items)", ModelName, _detailService.ItemsList.Count));
            Debug(String.Format("VirementMontantModel loaded ({1} items)", ModelName, _montantService.ItemsList.Count));
        }

        public void UpdateItems(IEnumerable<VirementModel> list)
        {
            foreach (var model in list)
            {
                UpdateItem(model);
            }
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
            modelDst.TypePaiement = modelSrc.TypePaiement;
        }


        public void CreateVirementWithDetails(VirementModel model)
        {
            CreateItem(model);
            foreach (var detail in model.Details)
            {
                detail.VirementId = model.Id;
                _detailService.CreateItem(detail);
                foreach (var montant in detail.Montants)
                {
                    montant.VirementDetailId = detail.Id;
                    _montantService.CreateItem(montant);
                }
            }
        }

        public override void DeleteItem(long itemId, bool cascade)
        {
            if (cascade)
            {
                var virement = ItemsList.First(o => o.Id == itemId);
                foreach (var detail in virement.Details)
                {
                    //supprimer les montants
                    foreach (var montant in detail.Montants)
                    {
                        _montantService.DeleteItem(montant.Id, cascade);
                    }
                    _detailService.DeleteItem(detail.Id, cascade);
                }
            }
            base.DeleteItem(itemId, cascade);
        }
    }
}
