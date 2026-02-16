
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerDataController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        BaseResponse<CustomerListModel> baseResponse = new BaseResponse<CustomerListModel>();
        public static string IDServerUrl = string.Empty;
        private ApiHelper api;

        public CustomerDataController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            _httpContextAccessor = httpContextAccessor;
            api = new ApiHelper(_httpContext);
        }


        [HttpGet("search")]
        [Authorize]
        public IActionResult search(string? searchtext=null, int pageIndex = 1, int pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&search=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = api.ApiCall(IDServerUrl, EndPoints.CustomerSearch + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            return Ok(baseResponse);
        }
    }
}
