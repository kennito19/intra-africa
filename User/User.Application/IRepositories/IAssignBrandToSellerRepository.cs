using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;

namespace User.Application.IRepositories
{
    public interface IAssignBrandToSellerRepository
    {
        Task<BaseResponse<long>> Create(AssignBrandToSeller assignBrandToSeller);
        Task<BaseResponse<long>> Update(AssignBrandToSeller assignBrandToSeller);
        Task<BaseResponse<long>> Delete(AssignBrandToSeller assignBrandToSeller);
        Task<BaseResponse<List<AssignBrandToSeller>>> Get(AssignBrandToSeller assignBrandToSeller, int PageIndex, int PageSize, string Mode);
    }
}
