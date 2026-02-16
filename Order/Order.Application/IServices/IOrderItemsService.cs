using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order.Domain.DTO;

namespace Order.Application.IServices
{
    public interface IOrderItemsService
    {
        Task<BaseResponse<long>> Create(OrderItems orderItems);

        Task<BaseResponse<long>> Update(OrderItems orderItems);

        Task<BaseResponse<long>> Delete(OrderItems orderItems);

        Task<BaseResponse<List<OrderItems>>> Get(OrderItems orderItems, int PageIndex, int PageSize, string Mode);
        Task<BaseResponse<List<OrderItemDetails>>> GetOrderDetails(OrderItemDetails orderItemDetails, int PageIndex, int PageSize, string Mode);

    }
}
