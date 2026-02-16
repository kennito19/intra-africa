using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.IServices;
using User.Domain.Entity;
using User.Domain;
using Microsoft.AspNetCore.Authorization;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(CityLibrary cityLibrary)
        {
            cityLibrary.CreatedAt = DateTime.Now;
            var data = await _cityService.Create(cityLibrary);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(CityLibrary cityLibrary)
        {
            cityLibrary.ModifiedAt = DateTime.Now;
            var data = await _cityService.Update(cityLibrary);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int id)
        {

            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            CityLibrary city = new CityLibrary();
            city.Id = id;
            city.DeletedBy = DeletedBy;
            city.DeletedAt = DateTime.Now;


            var data = await _cityService.Delete(city);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<CityLibrary>>> Get(int? id = null, string? name = null, int? countryId = null, int? stateId = null, string? stateIds = null, string? countryIds = null, string? status = null, bool? isDeleted = false, string? countryname = null, string? statename = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get",string? searchText=null)
        {
            CityLibrary cityLibrary = new CityLibrary();
            if (id != null)
            {
                cityLibrary.Id = Convert.ToInt32(id);
            }
            if (countryId != null)
            {
                cityLibrary.CountryID = Convert.ToInt32(countryId);
            }
            if (stateId != null)
            {
                cityLibrary.StateID = Convert.ToInt32(stateId);
            }
            cityLibrary.Name = name;
            cityLibrary.Status = status;
            cityLibrary.searchText = searchText;
            cityLibrary.IsDeleted = Convert.ToBoolean(isDeleted);
            cityLibrary.CountryName = countryname;
            cityLibrary.StateName = statename;
            cityLibrary.stateIds = stateIds;
            cityLibrary.countryIds = countryIds;
            var data = await _cityService.Get(cityLibrary, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
