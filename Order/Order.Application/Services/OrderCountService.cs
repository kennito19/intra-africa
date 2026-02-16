using Order.Application.IRepositories;
using Order.Application.IServices;
using Order.Domain;
using Order.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public class OrderCountService : IOrderCountService
    {
        private readonly IOrderCountRepository _orderCountRepository;

        public OrderCountService(IOrderCountRepository orderCountRepository)
        {
            _orderCountRepository = orderCountRepository;
        }
        public async Task<BaseResponse<List<OrdersCount>>> get(string? sellerId, string? userId, string? days)
        {
            var data = await _orderCountRepository.get(sellerId, userId, days);
            return data;
        }
    }
}
