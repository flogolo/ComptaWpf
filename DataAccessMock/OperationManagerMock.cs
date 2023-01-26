using System.Collections.Generic;
using System.Linq;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using System;
using CommonLibrary.Tools;

namespace DataAccessMock
{
    public class OperationManagerMock: BaseManagerMock<OperationModel>, IOperationService
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

        public OperationManagerMock(IDetailService detailService)
        {
            ModelName = "OperationModel";
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

                //chargement des détails
                _DetailSrv.LoadItems();

                AllOrdres.Sort();

                Debug(String.Format("{0} loaded ({1} items)", ModelName, AllOperations.Count));

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
            var operations = AllOperations.Where(o => o.CompteId == compteId);
            foreach (var op in operations)
            {
                ItemsList.Add(op);
                if (op.Details.Count == 0)
                {
                    var detailModels = _DetailSrv.ItemsList.Where(d => d.OperationId == op.Id);
                    op.Details = new List<DetailModel>(detailModels);
                }
            }

            return ItemsList;
        }

        public void CreateOperationWithDetails(OperationModel model)
        {
            CreateItem(model);
            //op.Details = new List<DetailModel>();
            foreach (var detail in model.Details)
            {
                detail.OperationId = model.Id; 
                _DetailSrv.CreateItem(detail);
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

            base.DeleteItem(itemId, cascade);
            var operation = AllOperations.First(o=>o.Id == itemId);
            //supprimer ordre si il n'apparait qu'une seule fois
            var ordre = AllOrdres.Where(o => o == operation.Ordre);
            if (ordre.Count() == 1)
                AllOrdres.Remove(ordre.First());
            //supprimer l'opération
            if (operation != null)
                AllOperations.Remove(operation);
        }

        public override void CreateItem(OperationModel model)
        {
            base.CreateItem(model);
            AllOperations.Add(model);
            //ajout de l'ordre si nécessaire dans la liste de choix des ordres
            if (!AllOrdres.Contains(model.Ordre))
                AllOrdres.Add(model.Ordre);
        }

        public void UpdateOperationWithDetails(OperationModel model)
        {
            throw new NotImplementedException();
        }
    }
}
