using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;
using User.Domain.Entity;

namespace User.Application.IServices
{
    public interface ICityService
    {
        Task<BaseResponse<long>> Create(CityLibrary cityLibrary);
        Task<BaseResponse<long>> Update(CityLibrary cityLibrary);
        Task<BaseResponse<long>> Delete(CityLibrary cityLibrary);
        Task<BaseResponse<List<CityLibrary>>> Get(CityLibrary cityLibrary, int PageIndex, int PageSize, string Mode);
    }
}
