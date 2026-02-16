using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IReturnPolicyRepository
    {
        Task<BaseResponse<long>> AddReturnPolicy(ReturnPolicy returnPolicy);

        Task<BaseResponse<long>> UpdateReturnPolicy(ReturnPolicy returnPolicy);

        Task<BaseResponse<long>> DeleteReturnPolicy(ReturnPolicy returnPolicy);

        Task<BaseResponse<List<ReturnPolicy>>> GetReturnPolicy(ReturnPolicy returnPolicy, int PageIndex, int PageSize, string Mode);
    }
}
