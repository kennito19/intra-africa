using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;

namespace User.Application.IRepositories
{
    public interface IWarehouseRepository
    {
        Task<BaseResponse<long>> Create(Warehouse warehouse);
        Task<BaseResponse<long>> Update(Warehouse warehouse);
        Task<BaseResponse<long>> Delete(Warehouse warehouse);
        Task<BaseResponse<List<Warehouse>>> Get(Warehouse warehouse, int PageIndex, int PageSize, string Mode);
    }
}
