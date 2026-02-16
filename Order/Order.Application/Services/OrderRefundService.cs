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
    public class OrderRefundService: IOrderRefundService
    {
        private readonly IOrderRefundRepository _orderRefundRepository;

        public OrderRefundService(IOrderRefundRepository orderRefundRepository)
        {
            _orderRefundRepository = orderRefundRepository;
        }

        Task<BaseResponse<long>> IOrderRefundService.Create(OrderRefund orderRefund)
        {
            var response = _orderRefundRepository.Create(orderRefund);
            return response;
        }
        Task<BaseResponse<long>> IOrderRefundService.Update(OrderRefund orderRefund)
        {
            var response = _orderRefundRepository.Update(orderRefund);
            return response;
        }

        Task<BaseResponse<long>> IOrderRefundService.Delete(OrderRefund orderRefund)
        {
            var response = _orderRefundRepository.Delete(orderRefund);
            return response;
        }

        Task<BaseResponse<List<OrderRefund>>> IOrderRefundService.Get(OrderRefund orderRefund, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderRefundRepository.Get(orderRefund, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
