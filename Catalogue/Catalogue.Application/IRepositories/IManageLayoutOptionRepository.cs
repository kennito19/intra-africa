using Catalogue.Domain.Entity;
using Catalogue.Domain;

namespace Catalogue.Application.IRepositories
{
    public interface IManageLayoutOptionRepository
    {
        Task<BaseResponse<long>> Create(ManageLayoutOption layoutOptions);

        Task<BaseResponse<long>> Update(ManageLayoutOption layoutOptions);

        Task<BaseResponse<long>> Delete(ManageLayoutOption layoutOptions);

        Task<BaseResponse<List<ManageLayoutOption>>> get(ManageLayoutOption layoutOptions, int PageIndex, int PageSize, string Mode);
    }
}
