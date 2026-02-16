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
    public class OrderCancelReturnService: IOrderCancelReturnService
    {
        private readonly IOrderCancelReturnRepository _orderCancelReturnRepository;

        public OrderCancelReturnService(IOrderCancelReturnRepository orderCancelReturnRepository)
        {
            _orderCancelReturnRepository = orderCancelReturnRepository;
        }

        Task<BaseResponse<long>> IOrderCancelReturnService.Create(OrderCancelReturn orderCancelReturn)
        {
            var response = _orderCancelReturnRepository.Create(orderCancelReturn);
            return response;
        }
        Task<BaseResponse<long>> IOrderCancelReturnService.Update(OrderCancelReturn orderCancelReturn)
        {
            var response = _orderCancelReturnRepository.Update(orderCancelReturn);
            return response;
        }

        Task<BaseResponse<long>> IOrderCancelReturnService.Delete(OrderCancelReturn orderCancelReturn)
        {
            var response = _orderCancelReturnRepository.Delete(orderCancelReturn);
            return response;
        }

        Task<BaseResponse<List<OrderCancelReturn>>> IOrderCancelReturnService.Get(OrderCancelReturn orderCancelReturn, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderCancelReturnRepository.Get(orderCancelReturn, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
