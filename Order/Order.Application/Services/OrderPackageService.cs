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
    public class OrderPackageService: IOrderPackageService
    {
        private readonly IOrderPackageRepository _orderPackageRepository;

        public OrderPackageService(IOrderPackageRepository orderPackageRepository)
        {
            _orderPackageRepository = orderPackageRepository;
        }

        Task<BaseResponse<long>> IOrderPackageService.Create(OrderPackage orderPackage)
        {
            var response = _orderPackageRepository.Create(orderPackage);
            return response;
        }
        Task<BaseResponse<long>> IOrderPackageService.Update(OrderPackage orderPackage)
        {
            var response = _orderPackageRepository.Update(orderPackage);
            return response;
        }

        Task<BaseResponse<long>> IOrderPackageService.Delete(OrderPackage orderPackage)
        {
            var response = _orderPackageRepository.Delete(orderPackage);
            return response;
        }

        Task<BaseResponse<List<OrderPackage>>> IOrderPackageService.Get(OrderPackage orderPackage, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderPackageRepository.Get(orderPackage, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
