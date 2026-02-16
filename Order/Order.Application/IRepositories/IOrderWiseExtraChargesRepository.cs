using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IRepositories
{
    public interface IOrderWiseExtraChargesRepository
    {
        Task<BaseResponse<long>> Create(OrderWiseExtraCharges orderWiseExtraCharges);

        Task<BaseResponse<long>> Update(OrderWiseExtraCharges orderWiseExtraCharges);

        Task<BaseResponse<long>> Delete(OrderWiseExtraCharges orderWiseExtraCharges);

        Task<BaseResponse<List<OrderWiseExtraCharges>>> Get(OrderWiseExtraCharges orderWiseExtraCharges, int PageIndex, int PageSize, string Mode);

    }
}
