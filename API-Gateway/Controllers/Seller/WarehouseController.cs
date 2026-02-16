using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Gateway.Controllers.Seller
{
    [Route("api/seller/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private WarehouseDetails warehouseDetails;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string cataURL = string.Empty;
        BaseResponse<Warehouse> baseResponse = new BaseResponse<Warehouse>();

        public WarehouseController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            cataURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            warehouseDetails = new WarehouseDetails(_httpContext, URL, cataURL);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Save(Warehouse model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = warehouseDetails.SaveWarehouse(model, userID);
            return Ok(baseResponse);
        }


        [HttpPut]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Update(Warehouse model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = warehouseDetails.UpdateWarehouse(model, userID);
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Delete(int? id = 0, string? userID = "")
        {
            baseResponse = warehouseDetails.DeleteWarehouse(id, userID);
            return Ok(baseResponse);
        }

        [HttpDelete("DeleteWarehouseWithGST")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> DeleteWithGST(int? id = 0, int? gstInfoId = 0)
        {
            baseResponse = warehouseDetails.DeleteWarehouseWithGst(id, gstInfoId);
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? PageIndex = 1, int? PageSize = 10)
        {
            baseResponse = warehouseDetails.GetWarehouse(PageIndex, PageSize);
            return Ok(baseResponse);
        }

        [HttpGet("ById")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ById(int id)
        {
            baseResponse = warehouseDetails.GetWarehouseById(id);
            return Ok(baseResponse);
        }

        [HttpGet("ByGSTInfoId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByGSTInfoId(int GSTInfoId)
        {
            baseResponse = warehouseDetails.GetWarehouseByGSTInfoId(GSTInfoId);
            return Ok(baseResponse);
        }

        [HttpGet("ByCountryId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByCountryId(int CountryId)
        {
            baseResponse = warehouseDetails.GetWarehouseByCountryId(CountryId);
            return Ok(baseResponse);
        }

        [HttpGet("ByStateId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByStateId(int StateId)
        {
            baseResponse = warehouseDetails.GetWarehouseByStateId(StateId);
            return Ok(baseResponse);
        }

        [HttpGet("ByCityId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByCityId(int CityId)
        {
            baseResponse = warehouseDetails.GetWarehouseByCityId(CityId);
            return Ok(baseResponse);
        }

        [HttpGet("ByUserID")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByUserID(string UserID)
        {
            baseResponse = warehouseDetails.GetWarehouseByUserID(UserID);
            return Ok(baseResponse);
        }

        [HttpGet("ByName")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByName(string Name)
        {
            baseResponse = warehouseDetails.GetWarehouseByName(Name);
            return Ok(baseResponse);
        }

        [HttpGet("ByContactPersonName")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByContactPersonName(string ContactPersonName)
        {
            baseResponse = warehouseDetails.GetWarehouseByContactPersonName(ContactPersonName);
            return Ok(baseResponse);
        }

        [HttpGet("ByContactPersonMobileNo")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByContactPersonMobileNo(string ContactPersonMobileNo)
        {
            baseResponse = warehouseDetails.GetWarehouseByContactPersonMobileNo(ContactPersonMobileNo);
            return Ok(baseResponse);
        }

        [HttpGet("ByPincode")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByPincode(string Pincode)
        {
            baseResponse = warehouseDetails.GetWarehouseByPincode(Pincode);
            return Ok(baseResponse);
        }

        [HttpGet("WarehouseSearch")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> WarehouseSearch(int? Id, int? GSTInfoId, int? CountryId, int? StateId, int? CityId, string? UserID, string? Name, string? ContactPersonName, string? ContactPersonMobileNo, string? Pincode, string? Status, string? CountryName, string? CityName, string? StateName)
        {
            baseResponse = warehouseDetails.WarehouseSearch(Id, GSTInfoId, CountryId, StateId, CityId, UserID, Name, ContactPersonName, ContactPersonMobileNo, Pincode, Status, CountryName, CityName, StateName);
            return Ok(baseResponse);
        }

        [HttpGet("Search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(int? GSTInfoId = null, string? UserID = null, string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            baseResponse = warehouseDetails.Search(GSTInfoId, UserID, searchtext, pageIndex, pageSize);
            return Ok(baseResponse);
        }

    }
}
