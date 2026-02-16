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
    public class OrderTrackDetailsService : IOrderTrackDetailsService
    {
        private readonly IOrderTrackDetailsRepository _orderTrackDetailsRepository;

        public OrderTrackDetailsService(IOrderTrackDetailsRepository orderTrackDetailsRepository)
        {
            _orderTrackDetailsRepository = orderTrackDetailsRepository;
        }

        Task<BaseResponse<long>> IOrderTrackDetailsService.Create(OrderTrackDetails orderTrackDetails)
        {
            var response = _orderTrackDetailsRepository.Create(orderTrackDetails);
            return response;
        }
        Task<BaseResponse<long>> IOrderTrackDetailsService.Update(OrderTrackDetails orderTrackDetails)
        {
            var response = _orderTrackDetailsRepository.Update(orderTrackDetails);
            return response;
        }

        Task<BaseResponse<long>> IOrderTrackDetailsService.Delete(OrderTrackDetails orderTrackDetails)
        {
            var response = _orderTrackDetailsRepository.Delete(orderTrackDetails);
            return response;
        }

        Task<BaseResponse<List<OrderTrackDetails>>> IOrderTrackDetailsService.Get(OrderTrackDetails orderTrackDetails, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderTrackDetailsRepository.Get(orderTrackDetails, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
