using System.Collections.Generic;
using System.Linq;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using System;
using CommonLibrary.Tools;

namespace DataAccess.Managers
{
    public class OperationManager: BaseManager<OperationModel>, IOperationService
    {
        //private readonly IChequeService m_ChequeSrv;
        public List<OperationModel> AllOperations
        {
            get;
            private set;
        }


        public SortableObservableCollection<string> AllOrdres
        {
            get;
            private set;
        }

        //List<OperationModel> AllOperations = new List<OperationModel>();
        private readonly IDetailService _DetailSrv;

        public OperationManager(IDetailService detailService)
        {
            ModelName = "OperationModel";
            //m_ChequeSrv = chequeService;
            _DetailSrv = detailService;
            AllOperations = new List<OperationModel>();
            AllOrdres = new SortableObservableCollection<string>();
        }

        /// <summary>
        /// Chargement de toutes les opérations avec leurs détails
        /// </summary>
        public override void LoadItems()
        {
            try
            {
                Debug("Loading " + ModelName + "...");
                AllOperations.Clear();
                var session = HibernateTools.Instance.Session;

                //var tx = session.BeginTransaction();
                var operations = session.CreateQuery("from " + ModelName).List<OperationModel>();
                //tx.Commit();

                //chargement des détails
                _DetailSrv.LoadItems();

                foreach (var operation in operations)
                {
                    AllOperations.Add(operation);
                    //var detailModels = m_DetailSrv.ItemsList.Where(d => d.OperationId == operation.Id);
                    //operation.Details = new List<DetailModel>(detailModels);
                    if (!AllOrdres.Contains(operation.Ordre))
                        AllOrdres.Add(operation.Ordre);
                }
                AllOrdres.Sort();

                Debug(String.Format("{0} loaded ({1} items)", ModelName, operations.Count()));

            }
            catch (Exception ex)
            {
                RaiseErrorOccured(ex.Message);
            }
        }

        public ICollection<OperationModel> LoadOperationsEnCours(long compteId)
        {
            Debug("Loading " + ModelName + "...");
            ItemsList.Clear();
            //recherche des opérations du compte
            var operations = AllOperations.Where(o => o.CompteId == compteId).ToList();
            foreach (var op in operations)
            {
                ItemsList.Add(op);
                if (op.Details.Count == 0)
                {
                    var detailModels = _DetailSrv.ItemsList.Where(d => d.OperationId == op.Id);
                    op.Details = new List<DetailModel>(detailModels);
                }
            }

            Debug(ModelName + " loaded");
            return ItemsList;
        }

        public override void UpdateItem(OperationModel model, bool traiterLien)
        {
            Debug(String.Format("Updating {0} {1} ...", ModelName, model.Id));

            var item = AllOperations.FirstOrDefault(i => i.Id == model.Id);
            CopyTo(item, model);

            BeginTransaction();
            var data = Session.Get<OperationModel>(model.Id);
            if (data != null)
            {
                CopyTo(data, model);
                data.UpdatedAt = DateTime.Now;
                Session.Update(data);
            }

            CommitTransaction();
        }

        public void CreateOperationWithDetails(OperationModel model)
        {
            CreateItem(model);
            foreach (var detail in model.Details)
            {
                detail.OperationId = model.Id; 
                _DetailSrv.CreateItem(detail);
            }
        }

        public void UpdateOperationWithDetails(OperationModel model)
        {
            UpdateItem(model, false);
            foreach (var detail in model.Details)
            {
                detail.OperationId = model.Id;
                _DetailSrv.UpdateItem(detail, false);
            }
        }

        public override void CopyTo(OperationModel modelDst, OperationModel modelSrc)
        {
            modelDst.DateOperation = modelSrc.DateOperation;
            modelDst.CompteId = modelSrc.CompteId;
            modelDst.DateValidation = modelSrc.DateValidation;
            modelDst.DateValidationPartielle = modelSrc.DateValidationPartielle;
            modelDst.Ordre = modelSrc.Ordre;
            modelDst.TypePaiement = modelSrc.TypePaiement;
            modelDst.NumeroCheque = modelSrc.NumeroCheque;
            modelDst.Report = modelSrc.Report;
            modelDst.IsVirementAuto = modelSrc.IsVirementAuto;
        }

        /// <summary>
        /// Création d'une liste d'operations avec leurs détails
        /// </summary>
        /// <param name="list"></param>
        public override void CreateItems(IEnumerable<OperationModel> list)
        {
            Debug(String.Format("Creating {0} {1} ...", ModelName, list.Count()));

            foreach (var operationModel in list)
            {
                CreateOperationWithDetails(operationModel);
            }
        }


        /// <summary>
        /// Recherche la prochaine valeur de chèque correspondant au chiffre saisi
        /// </summary>
        /// <param name="value">début d'un numéro de chèque</param>
        /// <returns></returns>
        public string FindCheque(string value)
        {
            return PaiementHelper.FindCheque(value, AllOperations);
        }

        public override void DeleteItem(long itemId, bool cascade)
        {
            BeginTransaction();
            var operation = AllOperations.First(o=>o.Id == itemId);
            //supprimer ordre si il n'apparait qu'une seule fois
            var ordre = AllOrdres.Where(o => o == operation.Ordre);
            if (ordre.Count() == 1)
                AllOrdres.Remove(ordre.First());
            if (cascade)
            {
                //supprimer les détails
                foreach (var detail in operation.Details)
                {
                    _DetailSrv.DeleteItem(detail.Id, cascade);
                }
            }
            //supprimer l'opération de la liste des opérations
            AllOperations.Remove(operation);
            base.DeleteItem(itemId, cascade);
            EndTransaction();
        }

        public override void CreateItem(OperationModel model)
        {
            base.CreateItem(model);
            AllOperations.Add(model);
            //ajout de l'ordre si nécessaire dans la liste de choix des ordres
            if (!AllOrdres.Contains(model.Ordre))
                AllOrdres.Add(model.Ordre);
        }

    }
}
