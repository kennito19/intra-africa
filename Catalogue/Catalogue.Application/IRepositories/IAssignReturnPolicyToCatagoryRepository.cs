using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IAssignReturnPolicyToCatagoryRepository
    {
        Task<BaseResponse<long>> Create(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory);

        Task<BaseResponse<long>> Update(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory);

        Task<BaseResponse<long>> Delete(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory);

        Task<BaseResponse<List<AssignReturnPolicyToCatagory>>> get(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory, int PageIndex, int PageSize, string Mode);
    }
}
