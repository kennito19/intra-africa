using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;
using User.Domain.Entity;

namespace User.Application.IServices
{
    public interface IAssignBrandToSellerService
    {
        Task<BaseResponse<long>> Create(AssignBrandToSeller assignBrandToSeller);
        Task<BaseResponse<long>> Update(AssignBrandToSeller assignBrandToSeller);
        Task<BaseResponse<long>> Delete(AssignBrandToSeller assignBrandToSeller);
        Task<BaseResponse<List<AssignBrandToSeller>>> Get(AssignBrandToSeller assignBrandToSeller, int PageIndex, int PageSize, string Mode);
    }
}
