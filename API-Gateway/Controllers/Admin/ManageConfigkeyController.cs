using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageConfigkeyController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ManageConfigKeyLibrary> baseResponse = new BaseResponse<ManageConfigKeyLibrary>();
        private ApiHelper helper;
        public ManageConfigkeyController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(ManageConfigKeyLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageConfigKey + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageConfigKeyLibrary> tempList = (List<ManageConfigKeyLibrary>)baseResponse.Data;

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ManageConfigKeyLibrary manageConfigKey = new ManageConfigKeyLibrary();
                manageConfigKey.Name = model.Name;
                manageConfigKey.CreatedAt = DateTime.Now;
                manageConfigKey.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageConfigKey, "POST", manageConfigKey);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ManageConfigKeyLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageConfigKey + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageConfigKeyLibrary> tempList = (List<ManageConfigKeyLibrary>)baseResponse.Data;

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageConfigKey + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageConfigKeyLibrary configKey = (ManageConfigKeyLibrary)baseResponse.Data;
                configKey.Id = model.Id;
                configKey.Name = model.Name;
                configKey.ModifiedAt = DateTime.Now;
                configKey.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageConfigKey, "PUT", configKey);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageConfigKey + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageConfigKeyLibrary> tempList = (List<ManageConfigKeyLibrary>)baseResponse.Data;
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ManageConfigKey + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageindex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageConfigKey + "?PageIndex=" + pageindex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }
            var response = helper.ApiCall(URL, EndPoints.ManageConfigKey + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
