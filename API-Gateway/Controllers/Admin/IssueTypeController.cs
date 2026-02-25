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
    public class IssueTypeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<IssueTypeLibrary> baseResponse = new BaseResponse<IssueTypeLibrary>();
        private ApiHelper helper;
        public IssueTypeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Save(IssueTypeLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.IssueType + "?Issue=" + model.Issue + "&actionId=" + model.ActionId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<IssueTypeLibrary> tmp = baseResponse.Data as List<IssueTypeLibrary> ?? new List<IssueTypeLibrary>();
            if (tmp.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
                //baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var temp1 = helper.ApiCall(URL, EndPoints.IssueType + "?Issue=" + model.Issue + "&actionId=" + model.ActionId + "&Isdeleted=" + true, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp1);
                List<IssueTypeLibrary> tmp1 = baseResponse.Data as List<IssueTypeLibrary> ?? new List<IssueTypeLibrary>();
                if (tmp1.Any())
                {
                    var data = tmp1.FirstOrDefault();
                    data.ActionId = model.ActionId;
                    data.Issue = model.Issue;
                    data.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    data.CreatedAt = DateTime.Now;
                    data.ModifiedAt = null;
                    data.ModifiedBy = null;
                    data.IsDeleted = false;
                    data.DeletedBy = null;
                    data.DeletedAt = null;

                    var response1 = helper.ApiCall(URL, EndPoints.IssueType, "PUT", data);
                    baseResponse = baseResponse.JsonParseInputResponse(response1);
                    if (baseResponse.code == 200)
                    {
                        baseResponse.Data = data.Id;
                        baseResponse.Message = "Record added successfully.";
                    }
                }
                else
                {
                    IssueTypeLibrary issue = new IssueTypeLibrary();
                    issue.Issue = model.Issue;
                    issue.ActionId = model.ActionId;
                    issue.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    issue.CreatedAt = DateTime.Now;

                    var response = helper.ApiCall(URL, EndPoints.IssueType, "POST", issue);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Update(IssueTypeLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.IssueType + "?Issue=" + model.Issue + "&actionId=" + model.ActionId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<IssueTypeLibrary> tmp = baseResponse.Data as List<IssueTypeLibrary> ?? new List<IssueTypeLibrary>();
            if (tmp.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(URL, EndPoints.IssueType + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(response);
                IssueTypeLibrary issue = baseResponse.Data as IssueTypeLibrary;

                issue.Id = model.Id;
                issue.ActionId = model.ActionId;
                issue.Issue = model.Issue;
                issue.ModifiedAt = DateTime.Now;
                issue.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                response = helper.ApiCall(URL, EndPoints.IssueType, "PUT", issue);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }
        [HttpDelete]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Delete(int? id = 0)
        {
            var temp = helper.ApiCall(URL, EndPoints.IssueType + "?Id=" + id + "&isDeleted=false", "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<IssueTypeLibrary> templist = baseResponse.Data as List<IssueTypeLibrary> ?? new List<IssueTypeLibrary>();
            if (templist.Any())
            {
                temp = helper.ApiCall(URL, EndPoints.IssueReason + "?IssueTypeId=" + id + "&isDeleted=false", "GET", null);
                BaseResponse<IssueReasonLibrary> baseresponse1 = new BaseResponse<IssueReasonLibrary>();
                baseresponse1 = baseresponse1.JsonParseList(temp);
                List<IssueReasonLibrary> tempList = baseresponse1.Data as List<IssueReasonLibrary> ?? new List<IssueReasonLibrary>();
                if (tempList.Count > 0)
                {
                    baseResponse = baseResponse.ChildExists();
                }
                else
                {
                    var response = helper.ApiCall(URL, EndPoints.IssueType + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet("byActionId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> byActionId(int ActionId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.IssueType + "?actionId=" + ActionId + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
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
            var response = helper.ApiCall(URL, EndPoints.IssueType + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }
    }
}
