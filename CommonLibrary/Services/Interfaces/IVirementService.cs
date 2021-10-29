using System;
using System.Collections.Generic;
using CommonLibrary.Models;
using CommonLibrary.Tools;

namespace CommonLibrary.Services.Interfaces
{
    public interface IVirementService : IServiceBase<VirementModel>
    {
        void CreateVirementWithDetails(VirementModel model);

        SortableObservableCollection<String> AllOrdres { get; }
    }
}
