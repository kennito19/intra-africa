using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.IServices;
using User.Application.Services;
using User.Domain.Entity;
using User.Domain;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(Address address)
        {
            address.CreatedAt = DateTime.Now;
            var data = await _addressService.Create(address);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(Address address)
        {
            address.ModifiedAt = DateTime.Now;
            var data = await _addressService.Update(address);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            Address address = new Address();
            address.Id = id;

            var data = await _addressService.Delete(address);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<Address>>> Get(int? Id = null, string? UserId = null, string? FullName = null, string? MobileNo = null, string? Pincode = null, int? CityId = null, int? StateId = null, int? CountryId = null, string? Status = null, bool? SetDefault = null, string? CountryName = null, string? CityName = null, string? StateName = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? searchText = null)
        {
            Address address = new Address();
            if (Id != null)
            {
                address.Id = Convert.ToInt32(Id);
            }
            if (CountryId != null)
            {
                address.CountryId = Convert.ToInt32(CountryId);
            }
            if (StateId != null)
            {
                address.StateId = Convert.ToInt32(StateId);
            }
            if (CityId != null)
            {
                address.CityId = Convert.ToInt32(CityId);
            }
            address.UserId = UserId;
            address.FullName = FullName;
            address.MobileNo = MobileNo;
            address.Pincode = Pincode;
            address.searchText = searchText;
            address.Status = Status;
            if (SetDefault != null)
            {
                address.SetDefault = Convert.ToBoolean(SetDefault);
            }
            address.CountryName = CountryName;
            address.CityName = CityName;
            address.StateName = StateName;
            var data = await _addressService.Get(address, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
