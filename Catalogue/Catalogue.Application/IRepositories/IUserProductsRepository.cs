using Catalogue.Domain.Entity;
using Catalogue.Domain;

namespace Catalogue.Application.IRepositories
{
    public interface IUserProductsRepository
    {
        Task<BaseResponse<List<UserProductList>>> get(UserProductParams productList, int? PageIndex = 0, int? PageSize = 0);
    }
}
