using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IServices
{
    public interface IRejectionTypeService
    {
        Task<BaseResponse<long>> Create(RejectionType rejectionType);

        Task<BaseResponse<long>> Update(RejectionType rejectionType);

        Task<BaseResponse<long>> Delete(RejectionType rejectionType);

        Task<BaseResponse<List<RejectionType>>> Get(RejectionType rejectionType, int PageIndex, int PageSize, string Mode);
    }
}
