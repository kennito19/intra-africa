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
    public class OrderProductVariantMappingService : IOrderProductVariantMappingService
    {
        private readonly IOrderProductVariantMappingRepository _orderProductVariantMappingRepository;

        public OrderProductVariantMappingService(IOrderProductVariantMappingRepository orderProductVariantMappingRepository)
        {
            _orderProductVariantMappingRepository = orderProductVariantMappingRepository;
        }

        Task<BaseResponse<long>> IOrderProductVariantMappingService.Create(OrderProductVariantMapping orderProductVariantMapping)
        {
            var response = _orderProductVariantMappingRepository.Create(orderProductVariantMapping);
            return response;
        }
        Task<BaseResponse<long>> IOrderProductVariantMappingService.Update(OrderProductVariantMapping orderProductVariantMapping)
        {
            var response = _orderProductVariantMappingRepository.Update(orderProductVariantMapping);
            return response;
        }

        Task<BaseResponse<long>> IOrderProductVariantMappingService.Delete(OrderProductVariantMapping orderProductVariantMapping)
        {
            var response = _orderProductVariantMappingRepository.Delete(orderProductVariantMapping);
            return response;
        }

        Task<BaseResponse<List<OrderProductVariantMapping>>> IOrderProductVariantMappingService.Get(OrderProductVariantMapping orderProductVariantMapping, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderProductVariantMappingRepository.Get(orderProductVariantMapping, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
