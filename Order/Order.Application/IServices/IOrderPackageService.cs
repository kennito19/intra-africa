using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IServices
{
    public interface IOrderPackageService
    {
        Task<BaseResponse<long>> Create(OrderPackage orderPackage);

        Task<BaseResponse<long>> Update(OrderPackage orderPackage);

        Task<BaseResponse<long>> Delete(OrderPackage orderPackage);

        Task<BaseResponse<List<OrderPackage>>> Get(OrderPackage orderPackage, int PageIndex, int PageSize, string Mode);

    }
}
