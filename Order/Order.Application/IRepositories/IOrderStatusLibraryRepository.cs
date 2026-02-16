using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IRepositories
{
    public interface IOrderStatusLibraryRepository
    {
        Task<BaseResponse<long>> Create(OrderStatusLibrary orderStatusLibrary);

        Task<BaseResponse<long>> Update(OrderStatusLibrary orderStatusLibrary);

        Task<BaseResponse<long>> Delete(OrderStatusLibrary orderStatusLibrary);

        Task<BaseResponse<List<OrderStatusLibrary>>> Get(OrderStatusLibrary orderStatusLibrary, int PageIndex, int PageSize, string Mode);


    }
}
