using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IRepositories
{
    public interface IOrderProductVariantMappingRepository
    {
        Task<BaseResponse<long>> Create(OrderProductVariantMapping orderProductVariantMapping);

        Task<BaseResponse<long>> Update(OrderProductVariantMapping orderProductVariantMapping);

        Task<BaseResponse<long>> Delete(OrderProductVariantMapping orderProductVariantMapping);

        Task<BaseResponse<List<OrderProductVariantMapping>>> Get(OrderProductVariantMapping orderProductVariantMapping, int PageIndex, int PageSize, string Mode);

    }
}
