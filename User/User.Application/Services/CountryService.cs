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
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryLibraryRepository;

        public CountryService(ICountryRepository countryLibraryRepository)
        {
            _countryLibraryRepository = countryLibraryRepository;
        }
        public Task<BaseResponse<long>> Create(CountryLibrary country)
        {
            var data = _countryLibraryRepository.Create(country);
            return data;
        }
        public Task<BaseResponse<long>> Update(CountryLibrary country)
        {
            var data = _countryLibraryRepository.Update(country);
            return data;
        }
        public Task<BaseResponse<long>> Delete(CountryLibrary country)
        {
            var data = _countryLibraryRepository.Delete(country);
            return data;
        }

        public Task<BaseResponse<List<CountryLibrary>>> Get(CountryLibrary country, int PageIndex, int PageSize, string Mode)
        {
            var data = _countryLibraryRepository.Get(country, PageIndex, PageSize, Mode);
            return data;
        }

        
    }
}
