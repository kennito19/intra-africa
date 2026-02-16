using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IRepositories
{
    public interface IOrderRefundRepository
    {
        Task<BaseResponse<long>> Create(OrderRefund orderRefund);

        Task<BaseResponse<long>> Update(OrderRefund orderRefund);

        Task<BaseResponse<long>> Delete(OrderRefund orderRefund);

        Task<BaseResponse<List<OrderRefund>>> Get(OrderRefund orderRefund, int PageIndex, int PageSize, string Mode);

    }
}
