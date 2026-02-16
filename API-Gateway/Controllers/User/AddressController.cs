using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<Address> baseResponse = new BaseResponse<Address>();
        private ApiHelper helper;
        public AddressController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Save(AddressDto model)
        {
            Address address = new Address();
            address.UserId = model.UserId;
            address.AddressType = model.AddressType;
            address.FullName = model.FullName;
            address.MobileNo = model.MobileNo;
            address.AddressLine1 = model.AddressLine1;
            address.AddressLine2 = model.AddressLine2;
            address.Landmark = model.Landmark;
            address.Pincode = model.Pincode;
            address.CityId = model.CityId;
            address.StateId = model.StateId;
            address.CountryId = model.CountryId;
            address.GSTNo = model.GSTNo;
            address.Status = "Active";
            address.SetDefault = model.SetDefault;
            address.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            address.CreatedAt = DateTime.Now;

            var response = helper.ApiCall(URL, EndPoints.Address, "POST", address);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Update(AddressDto model)
        {
            var response = helper.ApiCall(URL, EndPoints.Address + "?Id=" + model.Id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            Address address = (Address)baseResponse.Data;

            address.Id = Convert.ToInt32(model.Id);
            address.UserId = model.UserId;
            address.AddressType = model.AddressType;
            address.FullName = model.FullName;
            address.MobileNo = model.MobileNo;
            address.AddressLine1 = model.AddressLine1;
            address.AddressLine2 = model.AddressLine2;
            address.Landmark = model.Landmark;
            address.Pincode = model.Pincode;
            address.CityId = model.CityId;
            address.StateId = model.StateId;
            address.CountryId = model.CountryId;
            address.GSTNo = model.GSTNo;
            address.Status = model.Status;
            address.SetDefault = model.SetDefault;
            address.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            address.ModifiedAt = DateTime.Now;

            var fresponse = helper.ApiCall(URL, EndPoints.Address, "PUT", address);
            baseResponse = baseResponse.JsonParseInputResponse(fresponse);
            return Ok(baseResponse);
        }

        [HttpPut("setDefault")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> UpdateSetDefault(SetDefaultAddressDto model)
        {
            var response = helper.ApiCall(URL, EndPoints.Address + "?UserId=" + model.UserId + "&SetDefault="+true, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            if (baseResponse.code == 200)
            {
                Address address = (Address)baseResponse.Data;
                address.SetDefault = false;
                address.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                address.ModifiedAt = DateTime.Now;

                var fresponse = helper.ApiCall(URL, EndPoints.Address, "PUT", address);
                baseResponse = baseResponse.JsonParseInputResponse(fresponse);
            }
            var Uresponse = helper.ApiCall(URL, EndPoints.Address + "?Id=" + model.Id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(Uresponse);
            if (baseResponse.code == 200)
            {
                Address _address = (Address)baseResponse.Data;
                _address.SetDefault = model.SetDefault;
                _address.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                _address.ModifiedAt = DateTime.Now;

                var updateresponse = helper.ApiCall(URL, EndPoints.Address, "PUT", _address);
                baseResponse = baseResponse.JsonParseInputResponse(updateresponse);
            }

            return Ok(baseResponse);
        }

        [HttpPut("updateStatus")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> UpdateStatus(UpdateStatusAddressDto model)
        {
            var Uresponse = helper.ApiCall(URL, EndPoints.Address + "?Id=" + model.Id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(Uresponse);
            Address _address = (Address)baseResponse.Data;
            _address.Status = model.Status;
            _address.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            _address.ModifiedAt = DateTime.Now;

            var updateresponse = helper.ApiCall(URL, EndPoints.Address, "PUT", _address);
            baseResponse = baseResponse.JsonParseInputResponse(updateresponse);


            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Delete(int? id = 0)
        {
            var temp = helper.ApiCall(URL, EndPoints.Address + "?id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<Address> tmp = (List<Address>)baseResponse.Data;
            if (tmp.Count > 0)
            {
                var tempdeleteRecord = helper.ApiCall(URL, EndPoints.Address + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(tempdeleteRecord);
            }
            else
            {
                baseResponse = baseResponse.NotExist();

            }

            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.Address + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.Address + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byUserId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByUserId(string userId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.Address + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&UserId=" + userId, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Search(int? Id, int? CountryId, int? StateId, int? CityId, string? UserId, string? FullName, string? MobileNo, string? Pincode, string? Status, string? CountryName, string? CityName, string? StateName, string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;

            if (Id != null && Id != 0)
            {
                url += "&Id=" + Id;
            }

            if (StateId != null && StateId != 0)
            {
                url += "&StateId=" + StateId;
            }

            if (CountryId != null && CountryId != 0)
            {
                url += "&CountryId=" + CountryId;
            }

            if (CityId != null && CityId != 0)
            {
                url += "&CityId=" + CityId;
            }

            if (!string.IsNullOrEmpty(UserId) && UserId != "")
            {
                url += "&UserId=" + UserId;
            }

            if (!string.IsNullOrEmpty(FullName) && FullName != "")
            {
                url += "&FullName=" + FullName;
            }

            if (!string.IsNullOrEmpty(MobileNo) && FullName != "")
            {
                url += "&MobileNo=" + MobileNo;
            }

            if (!string.IsNullOrEmpty(Pincode) && Pincode != "")
            {
                url += "&Pincode=" + Pincode;
            }

            if (!string.IsNullOrEmpty(CountryName) && CountryName != "")
            {
                url += "&CountryName=" + CountryName;
            }

            if (!string.IsNullOrEmpty(CityName) && CityName != "")
            {
                url += "&CityName=" + CityName;
            }

            if (!string.IsNullOrEmpty(StateName) && StateName != "")
            {
                url += "&StateName=" + StateName;
            }

            if (!string.IsNullOrEmpty(Status) && Status != "")
            {
                url += "&Status=" + Status;
            }

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url += "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(URL, EndPoints.Address + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }
    }
}
