using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IServices
{
    public interface IOrderTrackDetailsService
    {
        Task<BaseResponse<long>> Create(OrderTrackDetails orderTrackDetails);

        Task<BaseResponse<long>> Update(OrderTrackDetails orderTrackDetails);

        Task<BaseResponse<long>> Delete(OrderTrackDetails orderTrackDetails);

        Task<BaseResponse<List<OrderTrackDetails >>> Get(OrderTrackDetails orderTrackDetails, int PageIndex, int PageSize, string Mode);

    }
}
