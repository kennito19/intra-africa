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
    public class OrderTaxInfoService : IOrderTaxInfoService
    {
        private readonly IOrderTaxInfoRepository _orderTaxInfoRepository;

        public OrderTaxInfoService(IOrderTaxInfoRepository orderTaxInfoRepository)
        {
            _orderTaxInfoRepository = orderTaxInfoRepository;
        }

        Task<BaseResponse<long>> IOrderTaxInfoService.Create(OrderTaxInfo orderTaxInfo)
        {
            var response = _orderTaxInfoRepository.Create(orderTaxInfo);
            return response;
        }
        Task<BaseResponse<long>> IOrderTaxInfoService.Update(OrderTaxInfo orderTaxInfo)
        {
            var response = _orderTaxInfoRepository.Update(orderTaxInfo);
            return response;
        }

        Task<BaseResponse<long>> IOrderTaxInfoService.Delete(OrderTaxInfo orderTaxInfo)
        {
            var response = _orderTaxInfoRepository.Delete(orderTaxInfo);
            return response;
        }

        Task<BaseResponse<List<OrderTaxInfo>>> IOrderTaxInfoService.Get(OrderTaxInfo orderTaxInfo, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderTaxInfoRepository.Get(orderTaxInfo, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
