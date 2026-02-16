using User.Domain;
using User.Domain.Entity;

namespace User.Application.IRepositories
{
    public interface IAddressRepository
    {
        Task<BaseResponse<long>> Create(Address address);
        Task<BaseResponse<long>> Update(Address address);
        Task<BaseResponse<long>> Delete(Address address);
        Task<BaseResponse<List<Address>>> Get(Address address, int PageIndex, int PageSize, string Mode);
    }
}
