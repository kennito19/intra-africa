using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IRepositories
{
    public interface IOrderReturnActionRepository
    {
        Task<BaseResponse<long>> Create(OrderReturnAction orderReturnAction);

        Task<BaseResponse<long>> Update(OrderReturnAction orderReturnAction);

        Task<BaseResponse<long>> Delete(OrderReturnAction orderReturnAction);

        Task<BaseResponse<List<OrderReturnAction>>> Get(OrderReturnAction orderReturnAction, int PageIndex, int PageSize, string Mode);
    }
}
