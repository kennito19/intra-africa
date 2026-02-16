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
    public class ReturnPolicyController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ReturnPolicyLibrary> baseResponse = new BaseResponse<ReturnPolicyLibrary>();

        public ReturnPolicyController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Save(ReturnPolicyLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ReturnPolicy + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ReturnPolicyLibrary> returnpolicy = (List<ReturnPolicyLibrary>)baseResponse.Data;
            if (returnpolicy.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ReturnPolicyLibrary rp = new ReturnPolicyLibrary();
                rp.Name = model.Name;
                rp.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ReturnPolicy, "POST", rp);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ReturnPolicyLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ReturnPolicy + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ReturnPolicyLibrary> templist = (List<ReturnPolicyLibrary>)baseResponse.Data;
            if (templist.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var newTemp = helper.ApiCall(URL, EndPoints.ReturnPolicy + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(newTemp);
                ReturnPolicyLibrary record = (ReturnPolicyLibrary)baseResponse.Data;
                record.Id = model.Id;
                record.Name = model.Name;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ReturnPolicy, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ReturnPolicy + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ReturnPolicyLibrary> tmp = (List<ReturnPolicyLibrary>)baseResponse.Data;
            if (!tmp.Any())
            {
                baseResponse = baseResponse.NotExist();
            }
            temp = helper.ApiCall(URL, EndPoints.ReturnPolicyDetail + "?returnpolicyid=" + id, "GET", null);
            BaseResponse<ReturnPolicyDetail> baseresponse1 = new BaseResponse<ReturnPolicyDetail>();
            baseresponse1 = baseresponse1.JsonParseList(temp);
            List<ReturnPolicyDetail> tempList = (List<ReturnPolicyDetail>)baseresponse1.Data;
            if (tempList.Count > 0)
            {
                baseResponse = baseResponse.ChildExists();
            }
            else
            {
                var response = helper.ApiCall(URL, EndPoints.ReturnPolicy + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ReturnPolicy + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("getById")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.ReturnPolicy + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(URL, EndPoints.ReturnPolicy + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }
    }
}
