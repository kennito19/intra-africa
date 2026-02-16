using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;

namespace User.Application.IRepositories
{
    public interface IBrandRepository
    {
        Task<BaseResponse<long>> Create(Brand brand);
        Task<BaseResponse<long>> Update(Brand brand);
        Task<BaseResponse<long>> Delete(Brand brand);
        Task<BaseResponse<List<Brand>>> Get(Brand brand, int PageIndex, int PageSize, string Mode);
    }
}
