using Order.Domain;
using Order.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IRepositories
{
    public interface IOrderCountRepository
    {
        Task<BaseResponse<List<OrdersCount>>> get(string? sellerId, string? userId, string? days);
    }
}
