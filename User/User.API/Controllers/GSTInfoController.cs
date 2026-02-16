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
    
    public class GSTInfoController : ControllerBase
    {
        private readonly IGSTInfoService _gSTInfoService;

        public GSTInfoController(IGSTInfoService gSTInfoService)
        {
            _gSTInfoService = gSTInfoService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(GSTInfo gSTInfo)
        {
            gSTInfo.CreatedAt = DateTime.Now;
            var data = await _gSTInfoService.Create(gSTInfo);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(GSTInfo gSTInfo)
        {
            gSTInfo.ModifiedAt = DateTime.Now;
            var data = await _gSTInfoService.Update(gSTInfo);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(string? UserId, int id = 0)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GSTInfo gSTInfo = new GSTInfo();
            gSTInfo.Id = id;
            gSTInfo.UserID = UserId == null ? string.Empty : UserId;
            gSTInfo.DeletedBy = DeletedBy;
            gSTInfo.DeletedAt = DateTime.Now;

            var data = await _gSTInfoService.Delete(gSTInfo);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<GSTInfo>>> Get(int? Id = null, string? UserID = null,string? GSTNo = null,string? Pincode = null, int? CityId = null, int? StateId = null, int? CountryId = null, string? Status = null, bool? IsDeleted = false, bool? IsHeadOffice = null, string? CountryName = null,string? CityName = null, string? StateName = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get",string? searchText= null)
        {
            GSTInfo gSTInfo = new GSTInfo();
            if (Id != null)
            {
                gSTInfo.Id = Convert.ToInt32(Id);
            }
            if (CountryId != null)
            {
                gSTInfo.RegisteredCountryId = Convert.ToInt32(CountryId);
            }
            if (StateId != null)
            {
                gSTInfo.RegisteredStateId = Convert.ToInt32(StateId);
            }
            if (CityId != null)
            {
                gSTInfo.RegisteredCityId = Convert.ToInt32(CityId);
            }
            if (IsHeadOffice != null)
            {
                gSTInfo.IsHeadOffice = Convert.ToBoolean(IsHeadOffice);
            }
            gSTInfo.UserID = UserID;
            gSTInfo.GSTNo = GSTNo;
            gSTInfo.RegisteredPincode = Pincode;
            gSTInfo.Status = Status;
            gSTInfo.searchText = searchText;
            gSTInfo.IsDeleted = Convert.ToBoolean(IsDeleted);
            gSTInfo.CountryName = CountryName;
            gSTInfo.CityName = CityName;
            gSTInfo.StateName = StateName;
            var data = await _gSTInfoService.Get(gSTInfo, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
