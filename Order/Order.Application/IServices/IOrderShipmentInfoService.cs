using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IServices
{
    public interface IOrderShipmentInfoService
    {
        Task<BaseResponse<long>> Create(OrderShipmentInfo orderShipmentInfo);

        Task<BaseResponse<long>> Update(OrderShipmentInfo orderShipmentInfo);

        Task<BaseResponse<long>> Delete(OrderShipmentInfo orderShipmentInfo);

        Task<BaseResponse<List<OrderShipmentInfo>>> Get(OrderShipmentInfo orderShipmentInfo, int PageIndex, int PageSize, string Mode);

    }
}
