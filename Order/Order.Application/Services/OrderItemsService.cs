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
    public class OrderItemsService : IOrderItemsService
    {
        private readonly IOrderItemsRepository _orderItemsRepository;

        public OrderItemsService(IOrderItemsRepository orderItemsRepository)
        {
            _orderItemsRepository = orderItemsRepository;
        }

        Task<BaseResponse<long>> IOrderItemsService.Create(OrderItems orderItems)
        {
            var response = _orderItemsRepository.Create(orderItems);
            return response;
        }
        Task<BaseResponse<long>> IOrderItemsService.Update(OrderItems orderItems)
        {
            var response = _orderItemsRepository.Update(orderItems);
            return response;
        }

        Task<BaseResponse<long>> IOrderItemsService.Delete(OrderItems orderItems)
        {
            var response = _orderItemsRepository.Delete(orderItems);
            return response;
        }

        Task<BaseResponse<List<OrderItems>>> IOrderItemsService.Get(OrderItems orderItems, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderItemsRepository.Get(orderItems, PageIndex, PageSize, Mode);
            return response;
        }

        public Task<BaseResponse<List<OrderItemDetails>>> GetOrderDetails(OrderItemDetails orderItemDetails, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderItemsRepository.GetOrderDetails(orderItemDetails, PageIndex, PageSize, Mode);
            return response;
        }
    }
}
