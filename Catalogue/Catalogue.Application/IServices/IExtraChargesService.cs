using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IExtraChargesService
    {
        Task<BaseResponse<long>> AddExtraCharges(ExtraChargesLibrary extraChargesLibrary);

        Task<BaseResponse<long>> UpdateExtraCharges(ExtraChargesLibrary extraChargesLibrary);

        Task<BaseResponse<long>> DeleteExtraCharges(ExtraChargesLibrary extraChargesLibrary);

        Task<BaseResponse<List<ExtraChargesLibrary>>> GetExtraCharges(ExtraChargesLibrary extraChargesLibrary, int PageIndex, int PageSize, string Mode);
        Task<BaseResponse<List<ExtraChargesLibrary>>> GetCatExtraCharges(int CategoryId);
    }
}
