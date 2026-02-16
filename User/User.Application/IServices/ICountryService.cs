using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;

namespace User.Application.IServices
{
    public interface ICountryService
    {
        Task<BaseResponse<long>> Create(CountryLibrary country);
        Task<BaseResponse<long>> Update(CountryLibrary country);
        Task<BaseResponse<long>> Delete(CountryLibrary country);
        Task<BaseResponse<List<CountryLibrary>>> Get(CountryLibrary country, int PageIndex, int PageSize, string Mode);
    }
}
