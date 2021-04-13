using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace DataAccess.Managers
{
    public class ComptaManager: BaseManager<ComptaModel>, IComptaService
    {
        public ComptaManager()
        {
            ModelName = "ComptaModel";
        }
    }
}
