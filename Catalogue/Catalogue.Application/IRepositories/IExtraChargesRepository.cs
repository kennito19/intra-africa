using Catalogue.Domain.Entity;
using Catalogue.Domain;


namespace Catalogue.Application.IRepositories
{
    public interface IExtraChargesRepository
    {
        Task<BaseResponse<long>> AddExtraCharges(ExtraChargesLibrary extraChargesLibrary);

        Task<BaseResponse<long>> UpdateExtraCharges(ExtraChargesLibrary extraChargesLibrary);

        Task<BaseResponse<long>> DeleteExtraCharges(ExtraChargesLibrary extraChargesLibrary);

        Task<BaseResponse<List<ExtraChargesLibrary>>> GetExtraCharges(ExtraChargesLibrary extraChargesLibrary, int PageIndex, int PageSize, string Mode);
        Task<BaseResponse<List<ExtraChargesLibrary>>> GetCatExtraCharges(int CategoryId);
    }
}
