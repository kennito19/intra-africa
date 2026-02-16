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
    public class OrderReturnActionService: IOrderReturnActionService
    {
        private readonly IOrderReturnActionRepository _orderReturnActionRepository;

        public OrderReturnActionService(IOrderReturnActionRepository orderReturnActionRepository)
        {
            _orderReturnActionRepository = orderReturnActionRepository;
        }

        Task<BaseResponse<long>> IOrderReturnActionService.Create(OrderReturnAction orderReturnAction)
        {
            var response = _orderReturnActionRepository.Create(orderReturnAction);
            return response;
        }
        Task<BaseResponse<long>> IOrderReturnActionService.Update(OrderReturnAction orderReturnAction)
        {
            var response = _orderReturnActionRepository.Update(orderReturnAction);
            return response;
        }

        Task<BaseResponse<long>> IOrderReturnActionService.Delete(OrderReturnAction orderReturnAction)
        {
            var response = _orderReturnActionRepository.Delete(orderReturnAction);
            return response;
        }

        Task<BaseResponse<List<OrderReturnAction>>> IOrderReturnActionService.Get(OrderReturnAction orderReturnAction, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderReturnActionRepository.Get(orderReturnAction, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
