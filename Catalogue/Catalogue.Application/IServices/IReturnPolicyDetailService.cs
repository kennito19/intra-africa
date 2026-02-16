using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IReturnPolicyDetailService
    {
        Task<BaseResponse<long>> AddReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail);

        Task<BaseResponse<long>> UpdateReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail);

        Task<BaseResponse<long>> DeleteReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail);

        Task<BaseResponse<List<ReturnPolicyDetail>>> GetReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail, int PageIndex, int PageSize, string Mode);
    }
}
