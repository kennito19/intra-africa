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
    public class OrderWiseExtraChargesService : IOrderWiseExtraChargesService
    {
        private readonly IOrderWiseExtraChargesRepository _orderWiseExtraChargesRepository;

        public OrderWiseExtraChargesService(IOrderWiseExtraChargesRepository orderWiseExtraChargesRepository)
        {
            _orderWiseExtraChargesRepository = orderWiseExtraChargesRepository;
        }

        Task<BaseResponse<long>> IOrderWiseExtraChargesService.Create(OrderWiseExtraCharges orderWiseExtraCharges)
        {
            var response = _orderWiseExtraChargesRepository.Create(orderWiseExtraCharges);
            return response;
        }
        Task<BaseResponse<long>> IOrderWiseExtraChargesService.Update(OrderWiseExtraCharges orderWiseExtraCharges)
        {
            var response = _orderWiseExtraChargesRepository.Update(orderWiseExtraCharges);
            return response;
        }

        Task<BaseResponse<long>> IOrderWiseExtraChargesService.Delete(OrderWiseExtraCharges orderWiseExtraCharges)
        {
            var response = _orderWiseExtraChargesRepository.Delete(orderWiseExtraCharges);
            return response;
        }

        Task<BaseResponse<List<OrderWiseExtraCharges>>> IOrderWiseExtraChargesService.Get(OrderWiseExtraCharges orderWiseExtraCharges, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderWiseExtraChargesRepository.Get(orderWiseExtraCharges, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
