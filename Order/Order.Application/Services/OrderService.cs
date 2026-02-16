using Order.Application.IRepositories;
using Order.Application.IServices;
using Order.Domain;
using Order.Domain.DTO;
using Order.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        Task<BaseResponse<long>> IOrderService.Create(Orders orders)
        {
            var response = _orderRepository.Create(orders);
            return response;
        }
        Task<BaseResponse<long>> IOrderService.Update(Orders orders)
        {
            var response = _orderRepository.Update(orders);
            return response;
        }

        Task<BaseResponse<long>> IOrderService.Delete(Orders orders)
        {
            var response = _orderRepository.Delete(orders);
            return response;
        }

        Task<BaseResponse<List<Orders>>> IOrderService.Get(Orders orders, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderRepository.Get(orders, PageIndex, PageSize, Mode);
            return response;
        }

        public Task<BaseResponse<List<InvoiceDto>>> GetInvoice(string? Packageid, string? OrderNo)
        {
            var response = _orderRepository.GetInvoice(Packageid, OrderNo);
            return response;
        }
    }
}
