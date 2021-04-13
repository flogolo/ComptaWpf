using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;

namespace DataAccess.Managers
{
    public class BanqueManager: BaseManager<BanqueModel>, IBanqueService
    {
        public BanqueManager()
        {
            ModelName = "BanqueModel";
        }
    }
}
