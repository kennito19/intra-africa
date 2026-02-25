using API_Gateway.Helper;
using API_Gateway.Models.Entity.IDServer;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/")]
    [ApiController]
    public class PageModuleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        public static string IDServerUrl = string.Empty;
        private ApiHelper api;
        public PageModuleController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpContext = _httpContextAccessor.HttpContext;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            api = new ApiHelper(_httpContext);
        }

        [HttpPost("PageModule")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreatePageModule(PageRoleModule module)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            BaseResponse<PageRoleModule> pagebaseResponse = new BaseResponse<PageRoleModule>();
            var pageresponse = api.ApiCall(IDServerUrl, EndPoints.GetAllPages + "?pageIndex=0&pageSize=0", "GET", null);
            pagebaseResponse = pagebaseResponse.JsonParseList(pageresponse);

            List<PageRoleModule> pageRoleModules = new List<PageRoleModule>();
            pageRoleModules = pagebaseResponse.Data as List<PageRoleModule> ?? new List<PageRoleModule>();
            var data = pageRoleModules.Where(x => x.Name.ToLower() == module.Name.ToLower()).ToList();
            if (data.Count > 0)
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.CreatePageRoleModule, "POST", module);
                baseResponse = baseResponse.JsonParseInputResponse(response);

                if (baseResponse.code == 200)
                {
                    BaseResponse<RoleType> RolebaseResponse = new BaseResponse<RoleType>();
                    var getresponse = api.ApiCall(IDServerUrl, EndPoints.GetAllRoleTypes + "?pageIndex=0&pageSize=0", "GET", null);
                    RolebaseResponse = RolebaseResponse.JsonParseList(getresponse);

                    List<RoleType> roleTypes = new List<RoleType>();
                    roleTypes = RolebaseResponse.Data as List<RoleType> ?? new List<RoleType>();
                    var data1 = roleTypes.Where(x => x.Name.ToLower() == "super admin").FirstOrDefault();
                    if (data1.Id != null && data1.Id != 0)
                    {
                        BaseResponse<string> _baseResponse = new BaseResponse<string>();

                        AssignPageRole assignPageRole = new AssignPageRole();
                        assignPageRole.RoleTypeId = data1.Id;
                        assignPageRole.UserID = null;
                        assignPageRole.PageRoleId = Convert.ToInt32(baseResponse.Data);
                        assignPageRole.Read = true;
                        assignPageRole.Add = true;
                        assignPageRole.Update = true;
                        assignPageRole.Delete = true;

                        var response1 = api.ApiCall(IDServerUrl, EndPoints.AssignPageRoles, "POST", assignPageRole);
                        _baseResponse = _baseResponse.JsonParseInputResponse(response1);

                    }
                }

            }

            return Ok(baseResponse);
        }

        [HttpPut("PageModule")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditPageModule(PageRoleModule module)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            BaseResponse<PageRoleModule> pagebaseResponse = new BaseResponse<PageRoleModule>();
            BaseResponse<RoleType> rolebaseResponse = new BaseResponse<RoleType>();
            var pageresponse = api.ApiCall(IDServerUrl, EndPoints.GetAllPages + "?pageIndex=0&pageSize=0", "GET", null);
            pagebaseResponse = pagebaseResponse.JsonParseList(pageresponse);

            List<PageRoleModule> pageRoleModules = new List<PageRoleModule>();
            pageRoleModules = pagebaseResponse.Data as List<PageRoleModule> ?? new List<PageRoleModule>();
            var data = pageRoleModules.Where(x => x.Name.ToLower() == module.Name.ToLower() && x.Id != module.Id).ToList();
            if (data.Count > 0)
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.EditPageRoleModule, "PUT", module);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                if (baseResponse.code == 200)
                {
                    var Roleresponse = api.ApiCall(IDServerUrl, EndPoints.GetAllRoleTypes, "GET", null);
                    rolebaseResponse = rolebaseResponse.JsonParseList(Roleresponse);
                    if (rolebaseResponse.code == 200)
                    {
                        List<RoleType> roleTypes = new List<RoleType>();
                        roleTypes = rolebaseResponse.Data as List<RoleType> ?? new List<RoleType>();
                        RoleType _roleType = new RoleType();
                        _roleType = roleTypes.Where(p => p.Name.ToLower() == "super admin").FirstOrDefault();
                        AssignPageRole role = new AssignPageRole();
                        role.PageRoleId = Convert.ToInt32(baseResponse.Data);
                        role.RoleTypeId = _roleType.Id;
                        role.Add = true;
                        role.Update = true;
                        role.Delete = true;
                        role.Read = true;
                        BaseResponse<string> _baseResponse = new BaseResponse<string>();
                        var _response = api.ApiCall(IDServerUrl, EndPoints.AssignPageRoles, "POST", role);
                        _baseResponse = _baseResponse.JsonParseInputResponse(_response);
                    }
                }
            }
            return Ok(baseResponse);
        }

        [HttpDelete("PageModule")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePageModule(int moduleId)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            var response = api.ApiCall(IDServerUrl, EndPoints.DeletePageRoleModule + "?Id=" + moduleId, "DELETE", null);
            return Ok(baseResponse.JsonParseInputResponse(response));
        }

        [HttpGet("PageModule")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPages(int pageIndex = 1, int pageSize = 10, string? searchText = null)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchText))
            {
                url = "&searchString=" + HttpUtility.UrlEncode(searchText);
            }
            BaseResponse<PageRoleModule> baseResponse = new BaseResponse<PageRoleModule>();
            var response = api.ApiCall(IDServerUrl, EndPoints.GetAllPages + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("PageModuleById")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPageById(int Id)
        {
            BaseResponse<PageRoleModule> baseResponse = new BaseResponse<PageRoleModule>();
            var response = api.ApiCall(IDServerUrl, EndPoints.GetPageById + "?Id=" + Id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }
    }
}
