using API_Gateway.Helper;
using API_Gateway.Models.Entity.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class RejectionTypeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor; 
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<RejectionTypeLibrary> baseResponse = new BaseResponse<RejectionTypeLibrary>();
        private ApiHelper helper;
        public RejectionTypeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Save(RejectionTypeLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.RejectionType + "?Type=" + model.Type, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<RejectionTypeLibrary> tmp = baseResponse.Data as List<RejectionTypeLibrary> ?? new List<RejectionTypeLibrary>();
            if (tmp.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                RejectionTypeLibrary rejectionType = new RejectionTypeLibrary();
                rejectionType.Type = model.Type;

                rejectionType.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                rejectionType.CreatedAt = DateTime.Now;

                var response = helper.ApiCall(URL, EndPoints.RejectionType, "POST", rejectionType);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Update(RejectionTypeLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.RejectionType + "?Type=" + model.Type, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<RejectionTypeLibrary> tmp = baseResponse.Data as List<RejectionTypeLibrary> ?? new List<RejectionTypeLibrary>();
            if (tmp.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(URL, EndPoints.RejectionType + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(response);
                RejectionTypeLibrary rejectionType = baseResponse.Data as RejectionTypeLibrary;

                rejectionType.Id = model.Id;
                rejectionType.Type = model.Type;

                rejectionType.ModifiedAt = DateTime.Now;
                rejectionType.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                response = helper.ApiCall(URL, EndPoints.RejectionType, "PUT", rejectionType);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Delete(int? id = 0)
        {
            var temp = helper.ApiCall(URL, EndPoints.RejectionType + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<RejectionTypeLibrary> tmp = baseResponse.Data as List<RejectionTypeLibrary> ?? new List<RejectionTypeLibrary>();
            if (tmp.Any())
            {
                var tempIssue = helper.ApiCall(URL, EndPoints.IssueType + "?RejectionTypeId=" + id, "GET", null);
                BaseResponse<IssueTypeLibrary> baseResIssue = new BaseResponse<IssueTypeLibrary>();
                var IssueResponse = baseResIssue.JsonParseList(tempIssue);
                List<IssueTypeLibrary> IssueType = IssueResponse.Data as List<IssueTypeLibrary> ?? new List<IssueTypeLibrary>();

                if (IssueType.Any())
                {
                    baseResponse = baseResponse.ChildExists();
                }

                var tempIssueReason = helper.ApiCall(URL, EndPoints.IssueReason + "?RejectionTypeId=" + id, "GET", null);
                BaseResponse<IssueReasonLibrary> baseResIssueReasons = new BaseResponse<IssueReasonLibrary>();
                var IssueReasonsResponse = baseResIssueReasons.JsonParseList(tempIssueReason);
                List<IssueReasonLibrary> IssueReasons = IssueReasonsResponse.Data as List<IssueReasonLibrary> ?? new List<IssueReasonLibrary>();

                if (IssueReasons.Any())
                {
                    baseResponse = baseResponse.ChildExists();
                }
                else
                {
                    var response = helper.ApiCall(URL, EndPoints.RejectionType + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Search(string searchtext=null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(URL, EndPoints.RejectionType + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }
    }

}
