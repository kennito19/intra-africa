using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Application.IServices;
using User.Domain;
using User.Domain.Entity;

namespace User.Application.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        public Task<BaseResponse<long>> Create(CityLibrary cityLibrary)
        {
            var data = _cityRepository.Create(cityLibrary);
            return data;
        }
        public Task<BaseResponse<long>> Update(CityLibrary cityLibrary)
        {
            var data = _cityRepository.Update(cityLibrary);
            return data;
        }
        public Task<BaseResponse<long>> Delete(CityLibrary cityLibrary)
        {
            var data = _cityRepository.Delete(cityLibrary);
            return data;
        }

        public Task<BaseResponse<List<CityLibrary>>> Get(CityLibrary cityLibrary, int PageIndex, int PageSize, string Mode)
        {
            var data = _cityRepository.Get(cityLibrary, PageIndex, PageSize, Mode);
            return data;
        }

        
    }
}
