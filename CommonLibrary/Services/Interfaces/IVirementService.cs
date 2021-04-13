using System;
using System.Collections.Generic;
using CommonLibrary.Models;
using CommonLibrary.Tools;

namespace CommonLibrary.Services.Interfaces
{
    public interface IVirementService : IServiceBase<VirementModel>
    {
        /// <summary>
        /// Met à jour une liste de virements
        /// </summary>
        /// <param name="list">liste de virements</param>
        void UpdateItems(IEnumerable<VirementModel> list);

        void CreateVirementWithDetails(VirementModel model);

        SortableObservableCollection<String> AllOrdres { get; }
    }
}
