using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.Entity;


namespace Catalogue.Application.Services
{
    public class UserProductService:IUserProductService
    {
        public readonly IUserProductsRepository _repository;
        public UserProductService(IUserProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<BaseResponse<List<UserProductList>>> get(UserProductParams model,int? pageIndex=0,int? pageSize=0)
        {
            var data = await _repository.get(model, pageIndex, pageSize);
            return data;
        }
    }
}
