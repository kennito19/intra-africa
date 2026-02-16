using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Gateway.Controllers.User
{

    [Route("api/user/[controller]")]
    [ApiController]
    public class GSTInfoController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private GSTInfoDetails GSTInfoDetails;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<GSTInfo> baseResponse = new BaseResponse<GSTInfo>();

        public GSTInfoController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;

            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            GSTInfoDetails = new GSTInfoDetails(_httpContext, URL,_configuration);
        }

        //[HttpPost]
        //[Authorize(Roles = "Admin, Customer")]
        //public ActionResult<ApiHelper> Save([FromForm] GSTInfo model)
        //{
        //    string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
        //    baseResponse = GSTInfoDetails.SaveGSTInfo(model, userID);
        //    return Ok(baseResponse);
        //}

        [HttpPost]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Save([FromForm] GSTInfoDTO model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = GSTInfoDetails.SaveGSTInfo(model, userID, false);
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Update([FromForm] GSTInfoDTO model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = GSTInfoDetails.UpdateGSTInfo(model, userID, false);
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            baseResponse = GSTInfoDetails.DeleteGSTInfo(id);
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            baseResponse = GSTInfoDetails.GetGSTInfo(pageIndex, pageSize);
            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ById(int id)
        {
            baseResponse = GSTInfoDetails.GSTInfoById(id);
            return Ok(baseResponse);
        }

        [HttpGet("byUserId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByUserID(string userId, bool? IsHeadOffice = null)
        {
            bool AllowmultipleGST = Convert.ToBoolean(_configuration.GetValue<string>("allow_multiplegst"));
            baseResponse = GSTInfoDetails.GSTInfoByUserID(userId, AllowmultipleGST, IsHeadOffice);
            return Ok(baseResponse);
        }

        [HttpGet("byGSTNo")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByGSTNo(string gstNo)
        {
            baseResponse = GSTInfoDetails.GSTInfoByGSTNo(gstNo);
            return Ok(baseResponse);
        }

        [HttpGet("byPincode")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByPincode(string pincode)
        {
            baseResponse = GSTInfoDetails.GSTInfoByPincode(pincode);
            return Ok(baseResponse);
        }

        [HttpGet("byCityId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByCityId(string cityId)
        {
            baseResponse = GSTInfoDetails.GSTInfoByCityId(cityId);
            return Ok(baseResponse);
        }

        [HttpGet("byStateId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByStateId(string stateId)
        {
            baseResponse = GSTInfoDetails.GSTInfoByStateId(stateId);
            return Ok(baseResponse);
        }

        [HttpGet("byCountryId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByCountryId(string countryId)
        {
            baseResponse = GSTInfoDetails.GSTInfoByCountryId(countryId);
            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Search(string? UserID = null, bool? IsHeadOffice = null, string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            baseResponse = GSTInfoDetails.GetGSTInfobySearch(UserID, IsHeadOffice, searchtext, pageIndex, pageSize);
            return Ok(baseResponse);
        }

    }
}
