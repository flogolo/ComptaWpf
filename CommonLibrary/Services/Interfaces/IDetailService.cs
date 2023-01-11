using CommonLibrary.Models;

namespace CommonLibrary.Services.Interfaces
{
    public interface IDetailService : IServiceBase<DetailModel>
    {
        new void CopyTo(DetailModel modelDst, DetailModel modelSrc);
    }
}