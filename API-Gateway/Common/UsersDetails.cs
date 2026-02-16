using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using System.Web;

namespace API_Gateway.Common
{
    public class UsersDetails
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public string IdserverURL = string.Empty;
        public string UserURL = string.Empty;
        private readonly HttpContext _httpContext;
        BaseResponse<CustomerListModel> baseResponse = new BaseResponse<CustomerListModel>();
        private ApiHelper helper;
        public UsersDetails(IConfiguration configuration, HttpContext httpContext)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            IdserverURL = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            _configuration = configuration;
            helper = new ApiHelper(_httpContext);
        }
        public BaseResponse<CustomerListModel> GetCustomer(int? PageIndex, int? PageSize)
        {
            var response = helper.ApiCall(IdserverURL, EndPoints.CustomerList + "?pageIndex=" + PageIndex + "&pageSize=" + PageSize, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<CustomerListModel> GetCustomerById(string? id = null)
        {
            var response = helper.ApiCall(IdserverURL, EndPoints.CustomerList + "?ID=" + id, "GET", null);
            return baseResponse.JsonParseRecord(response);
        }

        public BaseResponse<CustomerListModel> Search(string searchText = null)
        {
            var response = helper.ApiCall(IdserverURL, EndPoints.CustomerSearch + "?searchString=" + HttpUtility.UrlEncode(searchText), "GET", null);
            return baseResponse.JsonParseList(response);
        }
    }
}
