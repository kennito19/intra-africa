using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Domain.Entity;
using User.Domain;
using User.Application.IServices;
using Microsoft.AspNetCore.Authorization;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryLibraryService;

        public CountryController(ICountryService countryLibraryService)
        {
            _countryLibraryService = countryLibraryService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(CountryLibrary countryLibrary)
        {
            countryLibrary.CreatedAt = DateTime.Now;

            var data = await _countryLibraryService.Create(countryLibrary);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(CountryLibrary countryLibrary)
        {
            countryLibrary.ModifiedAt = DateTime.Now;

            var data = await _countryLibraryService.Update(countryLibrary);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            CountryLibrary country = new CountryLibrary();
            country.Id = id;
            country.DeletedBy = DeletedBy;
            country.DeletedAt = DateTime.Now;


            var data = await _countryLibraryService.Delete(country);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<CountryLibrary>>> Get(int? id = null, string? name = null, string? status = null, bool? isDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? searchtext = null)
        {
            CountryLibrary countryLibrary = new CountryLibrary();
            if (id != null)
            {
                countryLibrary.Id = Convert.ToInt32(id);
            }
            countryLibrary.Name = name;
            countryLibrary.Searchtext = searchtext;
            countryLibrary.Status = status;
            countryLibrary.IsDeleted = Convert.ToBoolean(isDeleted);
            var data = await _countryLibraryService.Get(countryLibrary, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
