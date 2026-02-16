using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IServices
{
    public interface IOrderCountService
    {
        Task<BaseResponse<List<OrdersCount>>> get(string? sellerId, string? userId, string? days);
    }
}
