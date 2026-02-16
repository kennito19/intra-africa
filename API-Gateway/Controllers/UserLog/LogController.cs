using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.UserLog;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace API_Gateway.Controllers.UserLog
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        public static string LogUrl = string.Empty;
        BaseResponse<ActivityLog> baseResponse = new BaseResponse<ActivityLog>();
        private ApiHelper api;
        public LogController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            LogUrl = _configuration.GetSection("ApiURLs").GetSection("UserLog").Value;
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            api = new ApiHelper(_httpContext);
        }

        [HttpPost]
        public async Task<IActionResult> Log(ActivityLogDto model)
        {
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(LogUrl, EndPoints.log, "POST", model);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Log(string? userId = null, string? userType = null, string? action = null, int pageIndex = 1, int pageSize = 10, string? searchText = null)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(userId))
            {
                url += "&UserId=" + userId;
            }
            if (!string.IsNullOrEmpty(userType))
            {
                url += "&UserType=" + userType;
            }
            if (!string.IsNullOrEmpty(action))
            {
                url += "&Action=" + action;
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                url += "&Searchtext=" + HttpUtility.UrlEncode(searchText);
            }

            var response = api.ApiCall(LogUrl, EndPoints.log + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET");
            return Ok(baseResponse.JsonParseList(response));

        }
    }
}
