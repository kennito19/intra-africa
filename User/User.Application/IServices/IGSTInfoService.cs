using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;

namespace User.Application.IServices
{
    public interface IGSTInfoService
    {
        Task<BaseResponse<long>> Create(GSTInfo gSTInfo);
        Task<BaseResponse<long>> Update(GSTInfo gSTInfo);
        Task<BaseResponse<long>> Delete(GSTInfo gSTInfo);
        Task<BaseResponse<List<GSTInfo>>> Get(GSTInfo gSTInfo, int PageIndex, int PageSize, string Mode);
    }
}
