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

        void CreateOperationWithDetails(OperationModel model);
        void UpdateOperationWithDetails(OperationModel model);

        string FindCheque(string value);

        List<OperationModel> AllOperations { get; }

        SortableObservableCollection<string> AllOrdres { get; }
    }

    public interface IOperationPredefinieService : IServiceBase<OperationPredefinieModel>
    {
    }
}