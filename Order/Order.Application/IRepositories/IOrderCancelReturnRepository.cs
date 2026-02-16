using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IRepositories
{
    public interface IOrderCancelReturnRepository
    {
        Task<BaseResponse<long>> Create(OrderCancelReturn orderCancelReturn);

        Task<BaseResponse<long>> Update(OrderCancelReturn orderCancelReturn);

        Task<BaseResponse<long>> Delete(OrderCancelReturn orderCancelReturn);

        Task<BaseResponse<List<OrderCancelReturn>>> Get(OrderCancelReturn orderCancelReturn, int PageIndex, int PageSize, string Mode);

    }
}
