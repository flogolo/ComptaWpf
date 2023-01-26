using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using System;
using System.Linq;

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

        public override void DeleteItem(long itemId, bool cascade)
        {
            if (itemId <= 0) return;

            Debug(string.Format("Deleting {0} {1} ...", ModelName, itemId));

            BeginTransaction();
            var model = Session.Get<DetailModel>(itemId);
            if (model != null)
            {
                Session.Delete(model);
                ItemsList.Remove(model);
                if (model.LienDetailId != null)
                {
                    var linkedModel = Session.Get<DetailModel>(model.LienDetailId);
                    Session.Delete(linkedModel);
                    ItemsList.Remove(linkedModel);
                }
            }
            CommitTransaction();
        }

        public override void UpdateItem(DetailModel model, bool traiterLien)
        {
            Debug(string.Format("Updating {0} {1} ...", ModelName, model.Id));

            var item = ItemsList.FirstOrDefault(i => i.Id == model.Id);
            CopyTo(item, model);

            BeginTransaction();
            var data = Session.Get<DetailModel>(model.Id);
            if (data != null)
            {
                CopyTo(data, model);
                data.UpdatedAt = DateTime.Now;
                Session.Update(data);
            }
            if (traiterLien && model.LienDetailId != null)
            {
                var linkedData = Session.Get<DetailModel>(model.LienDetailId);
                if (linkedData != null)
                {
                    //mise à jour de l'item lié si déjà chargé en mémoire
                    var linkedItem = ItemsList.FirstOrDefault(i => i.Id == linkedData.Id);
                    if (linkedItem != null)
                    {
                        CopyTo(linkedItem, model);
                    }

                    CopyTo(linkedData, model);
                    linkedData.UpdatedAt = DateTime.Now;
                    Session.Update(linkedData);
                }
            }
            CommitTransaction();
        }
    }
}
