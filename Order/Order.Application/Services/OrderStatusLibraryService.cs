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
    public class OrderStatusLibraryService : IOrderStatusLibraryService
    {
        private readonly IOrderStatusLibraryRepository _orderStatusLibraryRepository;

        public OrderStatusLibraryService(IOrderStatusLibraryRepository orderStatusLibraryRepository)
        {
            _orderStatusLibraryRepository = orderStatusLibraryRepository;
        }

        Task<BaseResponse<long>> IOrderStatusLibraryService.Create(OrderStatusLibrary orderStatusLibrary)
        {
            var response = _orderStatusLibraryRepository.Create(orderStatusLibrary);
            return response;
        }

        Task<BaseResponse<long>> IOrderStatusLibraryService.Delete(OrderStatusLibrary orderStatusLibrary)
        {
            var response = _orderStatusLibraryRepository.Delete(orderStatusLibrary);
            return response;
        }

        Task<BaseResponse<List<OrderStatusLibrary>>> IOrderStatusLibraryService.Get(OrderStatusLibrary orderStatusLibrary, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderStatusLibraryRepository.Get(orderStatusLibrary, PageIndex, PageSize, Mode);
            return response;
        }

        Task<BaseResponse<long>> IOrderStatusLibraryService.Update(OrderStatusLibrary orderStatusLibrary)
        {
            var response = _orderStatusLibraryRepository.Update(orderStatusLibrary);
            return response;
        }
    }
}
