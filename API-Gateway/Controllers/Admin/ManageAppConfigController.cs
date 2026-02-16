using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageAppConfigController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ManageAppConfig> baseResponse = new BaseResponse<ManageAppConfig>();
        private ApiHelper helper;
        public ManageAppConfigController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(ManageAppConfigDto model)
        {
            var temp = helper.ApiCall(URL, EndPoints.AppConfig + "?name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageAppConfig> tempList = (List<ManageAppConfig>)baseResponse.Data;

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ManageAppConfig manageConfigKey = new ManageAppConfig();
                manageConfigKey.Name = model.Name;
                manageConfigKey.Value = model.Value;
                manageConfigKey.Status = model.Status;
                manageConfigKey.CreatedAt = DateTime.Now;
                manageConfigKey.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.AppConfig, "POST", manageConfigKey);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ManageAppConfig model)
        {
            var temp = helper.ApiCall(URL, EndPoints.AppConfig + "?name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageAppConfig> tempList = (List<ManageAppConfig>)baseResponse.Data;

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.AppConfig + "?id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageAppConfig configKey = (ManageAppConfig)baseResponse.Data;
                configKey.Id = model.Id;
                configKey.Name = model.Name;
                configKey.Status = model.Status;
                configKey.ModifiedAt = DateTime.Now;
                configKey.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.AppConfig, "PUT", configKey);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.AppConfig + "?id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageAppConfig> tempList = (List<ManageAppConfig>)baseResponse.Data;
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.AppConfig + "?id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseList(response);

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
            var response = helper.ApiCall(URL, EndPoints.AppConfig + "?pageIndex=" + pageindex + "&pageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.AppConfig + "?id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
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
            var response = helper.ApiCall(URL, EndPoints.AppConfig + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
