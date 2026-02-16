using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IServices
{
    public interface IOrderTaxInfoService
    {
        Task<BaseResponse<long>> Create(OrderTaxInfo orderTaxInfo);

        Task<BaseResponse<long>> Update(OrderTaxInfo orderTaxInfo);

        Task<BaseResponse<long>> Delete(OrderTaxInfo orderTaxInfo);

        Task<BaseResponse<List<OrderTaxInfo>>> Get(OrderTaxInfo orderTaxInfo, int PageIndex, int PageSize, string Mode);

    }
}
