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
    public class OrderShipmentInfoService : IOrderShipmentInfoService
    {
        private readonly IOrderShipmentInfoRepository _orderShipmentInfoRepository;

        public OrderShipmentInfoService(IOrderShipmentInfoRepository orderShipmentInfoRepository)
        {
            _orderShipmentInfoRepository = orderShipmentInfoRepository;
        }

        Task<BaseResponse<long>> IOrderShipmentInfoService.Create(OrderShipmentInfo orderShipmentInfo)
        {
            var response = _orderShipmentInfoRepository.Create(orderShipmentInfo);
            return response;
        }
        Task<BaseResponse<long>> IOrderShipmentInfoService.Update(OrderShipmentInfo orderShipmentInfo)
        {
            var response = _orderShipmentInfoRepository.Update(orderShipmentInfo);
            return response;
        }

        Task<BaseResponse<long>> IOrderShipmentInfoService.Delete(OrderShipmentInfo orderShipmentInfo)
        {
            var response = _orderShipmentInfoRepository.Delete(orderShipmentInfo);
            return response;
        }

        Task<BaseResponse<List<OrderShipmentInfo>>> IOrderShipmentInfoService.Get(OrderShipmentInfo orderShipmentInfo, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderShipmentInfoRepository.Get(orderShipmentInfo, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
