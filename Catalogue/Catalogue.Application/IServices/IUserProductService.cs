using Catalogue.Domain.Entity;
using Catalogue.Domain;


namespace Catalogue.Application.IServices
{
    public interface IUserProductService
    {
        Task<BaseResponse<List<UserProductList>>> get(UserProductParams model, int? pageIndex = 0, int? pageSize = 0);
    }
}
