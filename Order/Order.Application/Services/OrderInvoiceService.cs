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
    public class OrderInvoiceService : IOrderInvoiceService
    {
        private readonly IOrderInvoiceRepository _orderInvoiceRepository;

        public OrderInvoiceService(IOrderInvoiceRepository orderInvoiceRepository)
        {
            _orderInvoiceRepository = orderInvoiceRepository;
        }

        Task<BaseResponse<long>> IOrderInvoiceService.Create(OrderInvoice orderInvoice)
        {
            var response = _orderInvoiceRepository.Create(orderInvoice);
            return response;
        }
        Task<BaseResponse<long>> IOrderInvoiceService.Update(OrderInvoice orderInvoice)
        {
            var response = _orderInvoiceRepository.Update(orderInvoice);
            return response;
        }

        Task<BaseResponse<long>> IOrderInvoiceService.Delete(OrderInvoice orderInvoice)
        {
            var response = _orderInvoiceRepository.Delete(orderInvoice);
            return response;
        }

        Task<BaseResponse<List<OrderInvoice>>> IOrderInvoiceService.Get(OrderInvoice orderInvoice, int PageIndex, int PageSize, string Mode)
        {
            var response = _orderInvoiceRepository.Get(orderInvoice, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
