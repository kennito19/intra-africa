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
    
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(DeliveryLibrary deliveryLibrary)
        {
            deliveryLibrary.CreatedAt = DateTime.Now;
            var data = await _deliveryService.Create(deliveryLibrary);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(DeliveryLibrary deliveryLibrary)
        {
            deliveryLibrary.ModifiedAt = DateTime.Now;
            var data = await _deliveryService.Update(deliveryLibrary);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            DeliveryLibrary deliveryLibrary = new DeliveryLibrary();
            deliveryLibrary.Id = id;
            deliveryLibrary.DeletedBy = DeletedBy;
            deliveryLibrary.DeletedAt = DateTime.Now;
            var data = await _deliveryService.Delete(deliveryLibrary);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<DeliveryLibrary>>> Get(int? id = null, int? countryid = null, int? stateid = null, int? cityid = null, string? locality = null, int? pincode = null, string? countryname = null, string? statename = null, string? cityname = null, string? status = null, bool? isDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get",string? searchText=null)
        {
            DeliveryLibrary deliveryLibrary = new DeliveryLibrary();
            if (id != null)
            {
                deliveryLibrary.Id = Convert.ToInt32(id);
            }
            deliveryLibrary.CountryID = Convert.ToInt32(countryid);
            deliveryLibrary.StateID = Convert.ToInt32(stateid);
            deliveryLibrary.CityID = Convert.ToInt32(cityid);
            deliveryLibrary.Locality = locality;
            deliveryLibrary.Pincode = Convert.ToInt32(pincode);
            deliveryLibrary.CountryName = countryname;
            deliveryLibrary.StateName = statename;
            deliveryLibrary.CityName = cityname;
            deliveryLibrary.Status = status;
            deliveryLibrary.searchText = searchText;
            deliveryLibrary.IsDeleted = Convert.ToBoolean(isDeleted);
            var data = await _deliveryService.Get(deliveryLibrary, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
