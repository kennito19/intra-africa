using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IRepositories
{
    public interface IOrderInvoiceRepository
    {
        Task<BaseResponse<long>> Create(OrderInvoice orderInvoice);

        Task<BaseResponse<long>> Update(OrderInvoice orderInvoice);

        Task<BaseResponse<long>> Delete(OrderInvoice orderInvoice);

        Task<BaseResponse<List<OrderInvoice>>> Get(OrderInvoice orderInvoice, int PageIndex, int PageSize, string Mode);

    }
}
