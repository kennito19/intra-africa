using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;

namespace User.Application.IRepositories
{
    public interface ICityRepository
    {
        Task<BaseResponse<long>> Create(CityLibrary cityLibrary);
        Task<BaseResponse<long>> Update(CityLibrary cityLibrary);
        Task<BaseResponse<long>> Delete(CityLibrary cityLibrary);
        Task<BaseResponse<List<CityLibrary>>> Get(CityLibrary cityLibrary, int PageIndex, int PageSize, string Mode);
    }
}
