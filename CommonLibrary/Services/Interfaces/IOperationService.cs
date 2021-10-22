using System;
using System.Collections.Generic;
using CommonLibrary.Models;
using CommonLibrary.Tools;

namespace CommonLibrary.Services.Interfaces
{
    public interface IOperationService: IServiceBase<OperationModel>
    {
        /// <summary>
        /// Chargement des opérations d'un compte
        /// </summary>
        /// <param name="compteId"></param>
        ICollection<OperationModel> LoadOperationsEnCours(long compteId);

        ///// <summary>
        ///// Suppression d'une opération
        ///// </summary>
        ///// <param name="operationId">identifiant de l'opération</param>
        //void DeleteOperation(int operationId);

        void CreateOperationWithDetails(OperationModel model);

        string FindCheque(string value);

        List<OperationModel> AllOperations { get; }

        SortableObservableCollection<String> AllOrdres { get; }
    }

    public interface IOperationPredefinieService : IServiceBase<OperationPredefinieModel>
    {
    }
}