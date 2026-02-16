using API_Gateway.Helper;
using API_Gateway.Models.Entity.Order;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueReasonController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<IssueReasonLibrary> baseResponse = new BaseResponse<IssueReasonLibrary>();
        private ApiHelper helper;
        public IssueReasonController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize]
        public ActionResult<ApiHelper> Save(IssueReasonLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.IssueReason + "?IssueTypeId=" + model.IssueTypeId + "&Reasons=" + model.Reasons + "&actionId=" + model.ActionId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<IssueReasonLibrary> tmp = (List<IssueReasonLibrary>)baseResponse.Data;
            if (tmp.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var temp1 = helper.ApiCall(URL, EndPoints.IssueReason + "?IssueTypeId=" + model.IssueTypeId + "&Reasons=" + model.Reasons + "&actionId=" + model.ActionId + "&Isdeleted=" + true, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<IssueReasonLibrary> tmp1 = (List<IssueReasonLibrary>)baseResponse.Data;
                if (tmp1.Any())
                {
                    var data = tmp1.FirstOrDefault();
                    data.IssueTypeId = model.IssueTypeId;
                    data.ActionId = model.ActionId;
                    data.Reasons = model.Reasons;
                    data.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    data.CreatedAt = DateTime.Now;
                    data.ModifiedAt = null;
                    data.ModifiedBy = null;
                    data.IsDeleted = false;
                    data.DeletedBy = null;
                    data.DeletedAt = null;

                    var response1 = helper.ApiCall(URL, EndPoints.IssueReason, "PUT", data);
                    baseResponse = baseResponse.JsonParseInputResponse(response1);
                    if (baseResponse.code == 200)
                    {
                        baseResponse.Data = data.Id;
                        baseResponse.Message = "Record added successfully.";
                    }
                }
                else
                {

                    IssueReasonLibrary issue = new IssueReasonLibrary();
                    issue.IssueTypeId = model.IssueTypeId;
                    issue.ActionId = model.ActionId;
                    issue.Reasons = model.Reasons;
                    issue.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    issue.CreatedAt = DateTime.Now;

                    var response = helper.ApiCall(URL, EndPoints.IssueReason, "POST", issue);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize]
        public ActionResult<ApiHelper> Update(IssueReasonLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.IssueReason + "?IssueTypeId=" + model.IssueTypeId + "&Reasons=" + model.Reasons + "&actionId=" + model.ActionId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<IssueReasonLibrary> tmp = (List<IssueReasonLibrary>)baseResponse.Data;
            if (tmp.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(URL, EndPoints.IssueReason + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(response);
                IssueReasonLibrary reason = (IssueReasonLibrary)baseResponse.Data;

                reason.Id = model.Id;
                reason.IssueTypeId = model.IssueTypeId;
                reason.ActionId = model.ActionId;
                reason.Reasons = model.Reasons;
                reason.ModifiedAt = DateTime.Now;
                reason.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;


                response = helper.ApiCall(URL, EndPoints.IssueReason, "PUT", reason);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize]
        public ActionResult<ApiHelper> Delete(int? id = 0)
        {
            var temp = helper.ApiCall(URL, EndPoints.IssueReason + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<IssueReasonLibrary> templist = (List<IssueReasonLibrary>)baseResponse.Data;
            if (templist.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.IssueReason + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet("ByIssueTypeId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> ByIssueTypeId(int issueTypeId)
        {
            var response = helper.ApiCall(URL, EndPoints.IssueReason + "?IssueTypeId=" + issueTypeId, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("Search")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }
            var response = helper.ApiCall(URL, EndPoints.IssueReason + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }
    }
}
