using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IServices
{
    public interface IOrderWiseProductSeriesNoService
    {
        Task<BaseResponse<long>> Create(OrderWiseProductSeriesNo orderWiseProductSeriesNo);

        Task<BaseResponse<long>> Update(OrderWiseProductSeriesNo orderWiseProductSeriesNo);

        Task<BaseResponse<long>> Delete(OrderWiseProductSeriesNo orderWiseProductSeriesNo);

        Task<BaseResponse<List<OrderWiseProductSeriesNo>>> Get(OrderWiseProductSeriesNo orderWiseProductSeriesNo, int PageIndex, int PageSize, string Mode);

    }
}
