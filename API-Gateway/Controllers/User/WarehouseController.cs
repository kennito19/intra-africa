using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Gateway.Controllers.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private WarehouseDetails warehouseDetails;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<Warehouse> baseResponse = new BaseResponse<Warehouse>();

        public WarehouseController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            warehouseDetails = new WarehouseDetails(_httpContext, URL);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Save(Warehouse model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = warehouseDetails.SaveWarehouse(model, userID);
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Update(Warehouse model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = warehouseDetails.UpdateWarehouse(model, userID);
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Delete(int? id = 0)
        {
            baseResponse = warehouseDetails.DeleteUserWarehouse(id);
            return Ok(baseResponse);
        }

        [HttpDelete("DeleteUserWarehouseWithGST")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> DeleteWithGST(int? id = 0, int? gstInfoId = 0)
        {
            baseResponse = warehouseDetails.DeleteWarehouseWithGst(id, gstInfoId);
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            warehouseDetails = new WarehouseDetails(_httpContext, URL);
            baseResponse = warehouseDetails.GetWarehouse(pageIndex, pageSize);
            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ById(int id)
        {
            baseResponse = warehouseDetails.GetWarehouseById(id);
            return Ok(baseResponse);
        }

        [HttpGet("byGSTInfoId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByGSTInfoId(int gstInfoId)
        {
            baseResponse = warehouseDetails.GetWarehouseByGSTInfoId(gstInfoId);
            return Ok(baseResponse);
        }

        [HttpGet("byCountryId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByCountryId(int countryId)
        {
            baseResponse = warehouseDetails.GetWarehouseByCountryId(countryId);
            return Ok(baseResponse);
        }

        [HttpGet("byStateId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByStateId(int stateId)
        {
            baseResponse = warehouseDetails.GetWarehouseByStateId(stateId);
            return Ok(baseResponse);
        }

        [HttpGet("byCityId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByCityId(int cityId)
        {
            baseResponse = warehouseDetails.GetWarehouseByCityId(cityId);
            return Ok(baseResponse);
        }

        [HttpGet("byUserId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByUserID(string userId)
        {
            baseResponse = warehouseDetails.GetWarehouseByUserID(userId);
            return Ok(baseResponse);
        }

        [HttpGet("byName")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByName(string name)
        {
            baseResponse = warehouseDetails.GetWarehouseByName(name);
            return Ok(baseResponse);
        }

        [HttpGet("byContactPersonName")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByContactPersonName(string contactPersonName)
        {
            baseResponse = warehouseDetails.GetWarehouseByContactPersonName(contactPersonName);
            return Ok(baseResponse);
        }

        [HttpGet("byContactPersonMobileNo")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByContactPersonMobileNo(string contactPersonMobileNo)
        {
            baseResponse = warehouseDetails.GetWarehouseByContactPersonMobileNo(contactPersonMobileNo);
            return Ok(baseResponse);
        }

        [HttpGet("byPincode")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByPincode(string pincode)
        {
            baseResponse = warehouseDetails.GetWarehouseByPincode(pincode);
            return Ok(baseResponse);
        }

        [HttpGet("WarehouseSearch")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> WarehouseSearch(int? Id, int? GSTInfoId, int? CountryId, int? StateId, int? CityId, string? UserID, string? Name, string? ContactPersonName, string? ContactPersonMobileNo, string? Pincode, string? Status, string? CountryName, string? CityName, string? StateName)
        {
            baseResponse = warehouseDetails.WarehouseSearch(Id, GSTInfoId, CountryId, StateId, CityId, UserID, Name, ContactPersonName, ContactPersonMobileNo, Pincode, Status, CountryName, CityName, StateName);
            return Ok(baseResponse);
        }

        [HttpGet("Search")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Search(int? GSTInfoId = null, string? UserID = null, string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            baseResponse = warehouseDetails.Search(GSTInfoId,UserID, searchtext, pageIndex, pageSize);
            return Ok(baseResponse);
        }

    }
}
