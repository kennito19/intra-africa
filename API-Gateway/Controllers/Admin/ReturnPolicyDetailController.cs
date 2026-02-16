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
    public class ReturnPolicyDetailController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private string token = string.Empty;
        private ApiHelper helper;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ReturnPolicyDetail> baseResponse = new BaseResponse<ReturnPolicyDetail>();
        public ReturnPolicyDetailController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(ReturnPolicyDetail model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ReturnPolicyDetail + "?days=" + model.ValidityDays + "&title=" + model.Title + "&returnpolicyid=" + model.ReturnPolicyID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ReturnPolicyDetail> templist = (List<ReturnPolicyDetail>)baseResponse.Data;
            if (templist.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {

                ReturnPolicyDetail returnPolicy = new ReturnPolicyDetail();
                returnPolicy.ReturnPolicyID = model.ReturnPolicyID;
                returnPolicy.ValidityDays = model.ValidityDays;
                returnPolicy.Title = model.Title;
                returnPolicy.Covers = model.Covers;
                returnPolicy.Description = model.Description;
                returnPolicy.CreatedAt = DateTime.Now;
                returnPolicy.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                var response = helper.ApiCall(URL, EndPoints.ReturnPolicyDetail, "POST", returnPolicy);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ReturnPolicyDetail model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ReturnPolicyDetail + "?days=" + model.ValidityDays + "&title=" + model.Title + "&returnpolicyid=" + model.ReturnPolicyID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ReturnPolicyDetail> templist = (List<ReturnPolicyDetail>)baseResponse.Data;
            if (templist.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {

                var temp1 = helper.ApiCall(URL, EndPoints.ReturnPolicyDetail + "?id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp1);
                List<ReturnPolicyDetail> templist1 = (List<ReturnPolicyDetail>)baseResponse.Data;
                if (!templist1.Any())
                {
                    baseResponse = baseResponse.NotExist();
                }
                else
                {
                    ReturnPolicyDetail returnPolicy = templist.FirstOrDefault();
                    returnPolicy.Id = model.Id;
                    returnPolicy.ReturnPolicyID = model.ReturnPolicyID;
                    returnPolicy.ValidityDays = model.ValidityDays;
                    returnPolicy.Title = model.Title;
                    returnPolicy.Covers = model.Covers;
                    returnPolicy.Description = model.Description;
                    returnPolicy.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                    var response = helper.ApiCall(URL, EndPoints.ReturnPolicyDetail, "PUT", returnPolicy);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ReturnPolicyDetail + "?id=" + id, "GET", null);

            baseResponse = baseResponse.JsonParseList(temp);
            List<ReturnPolicyDetail> returnPolicy = (List<ReturnPolicyDetail>)baseResponse.Data;

            if (!returnPolicy.Any())
            {
                baseResponse = baseResponse.NotExist();
            }
            else
            {
                temp = helper.ApiCall(URL, EndPoints.AssignReturnPolicyToCatagory + "?ReturnPolicyDetailID=" + id, "GET", null);
                BaseResponse<AssignReturnPolicyToCatagoryLibrary> tmp = new BaseResponse<AssignReturnPolicyToCatagoryLibrary>();
                var tempList = tmp.JsonParseList(temp);
                List<AssignReturnPolicyToCatagoryLibrary> assignReturnPolicy = (List<AssignReturnPolicyToCatagoryLibrary>)tempList.Data;
                if (assignReturnPolicy.Any())
                {
                    baseResponse = baseResponse.ChildExists();
                }
                else
                {
                    var response = helper.ApiCall(URL, EndPoints.ReturnPolicyDetail + "?Id=" + id, "Delete", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }

            }
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ReturnPolicyDetail + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.ReturnPolicyDetail + "?id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("byReturnPolicyId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByReturnPolicyId(int returnpolicyid, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ReturnPolicyDetail + "?returnpolicyid=" + returnpolicyid + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetBySearch(string? searchtext, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(URL, EndPoints.ReturnPolicyDetail + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
