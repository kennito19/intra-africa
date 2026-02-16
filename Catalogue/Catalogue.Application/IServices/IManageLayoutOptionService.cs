using Catalogue.Domain.Entity;
using Catalogue.Domain;

namespace Catalogue.Application.IServices
{
    public interface IManageLayoutOptionService
    {
        Task<BaseResponse<long>> Create(ManageLayoutOption layoutOptions);

        Task<BaseResponse<long>> Update(ManageLayoutOption layoutOptions);

        Task<BaseResponse<long>> Delete(ManageLayoutOption layoutOptions);

        Task<BaseResponse<List<ManageLayoutOption>>> get(ManageLayoutOption layoutOptions, int PageIndex, int PageSize, string Mode);
    }
}
