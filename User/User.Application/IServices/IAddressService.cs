using User.Domain.Entity;
using User.Domain;

namespace User.Application.IServices
{
    public interface IAddressService
    {
        Task<BaseResponse<long>> Create(Address address);
        Task<BaseResponse<long>> Update(Address address);
        Task<BaseResponse<long>> Delete(Address address);
        Task<BaseResponse<List<Address>>> Get(Address address, int PageIndex, int PageSize, string Mode);
    }
}
