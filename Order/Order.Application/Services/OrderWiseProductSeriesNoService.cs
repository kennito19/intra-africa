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
    public class OrderWiseProductSeriesNoService : IOrderWiseProductSeriesNoService
    {
        private readonly IOrderWiseProductSeriesNoRepository _orderWiseProductSeriesNoRepository;

        public OrderWiseProductSeriesNoService(IOrderWiseProductSeriesNoRepository orderWiseProductSeriesNoRepository)
        {
            _orderWiseProductSeriesNoRepository = orderWiseProductSeriesNoRepository;
        }

        Task<BaseResponse<long>> IOrderWiseProductSeriesNoService.Create(OrderWiseProductSeriesNo orderWiseProductSeriesNo)
        {
            var response = _orderWiseProductSeriesNoRepository.Create(orderWiseProductSeriesNo);
            return response;
        }
        Task<BaseResponse<long>> IOrderWiseProductSeriesNoService.Update(OrderWiseProductSeriesNo orderWiseProductSeriesNo)
        {
            var response = _orderWiseProductSeriesNoRepository.Update(orderWiseProductSeriesNo);
            return response;
        }

        Task<BaseResponse<long>> IOrderWiseProductSeriesNoService.Delete(OrderWiseProductSeriesNo orderWiseProductSeriesNo)
        {
            var response = _orderWiseProductSeriesNoRepository.Delete(orderWiseProductSeriesNo);
            return response;
        }

        Task<BaseResponse<List<OrderWiseProductSeriesNo>>> IOrderWiseProductSeriesNoService.Get(OrderWiseProductSeriesNo orderWiseProductSeriesNo, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderWiseProductSeriesNoRepository.Get(orderWiseProductSeriesNo, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
