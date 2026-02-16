using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order.Domain.DTO;

namespace Order.Application.IRepositories
{
    public interface IOrderRepository
    {
        Task<BaseResponse<long>> Create(Orders orders);

        Task<BaseResponse<long>> Update(Orders orders);

        Task<BaseResponse<long>> Delete(Orders orders);

        Task<BaseResponse<List<Orders>>> Get(Orders orders, int PageIndex, int PageSize, string Mode);
        Task<BaseResponse<List<InvoiceDto>>> GetInvoice(string? Packageid, string? OrderNo);
    }
}
