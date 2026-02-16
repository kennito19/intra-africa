using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class AssignReturnPolicyToCatagoryController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<AssignReturnPolicyToCatagoryLibrary> baseResponse = new BaseResponse<AssignReturnPolicyToCatagoryLibrary>();

        public AssignReturnPolicyToCatagoryController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Save(AssignReturnPolicyToCatagoryDto model)
        {
            var temp = helper.ApiCall(URL, EndPoints.AssignReturnPolicyToCatagory + "?ReturnPolicyDetailID=" + model.ReturnPolicyDetailID + "&CategoryID=" + model.CategoryID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignReturnPolicyToCatagoryLibrary> asrpc = (List<AssignReturnPolicyToCatagoryLibrary>)baseResponse.Data;
            if (asrpc.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                AssignReturnPolicyToCatagoryLibrary arptc = new AssignReturnPolicyToCatagoryLibrary();
                arptc.ReturnPolicyDetailID = model.ReturnPolicyDetailID;
                arptc.CategoryID = model.CategoryID;
                arptc.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.AssignReturnPolicyToCatagory, "POST", arptc);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(AssignReturnPolicyToCatagoryDto model)
        {
            var temp = helper.ApiCall(URL, EndPoints.AssignReturnPolicyToCatagory + "?CategoryID=" + model.CategoryID + "&ReturnPolicyDetailID=" + model.ReturnPolicyDetailID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignReturnPolicyToCatagoryLibrary> templist = (List<AssignReturnPolicyToCatagoryLibrary>)baseResponse.Data;
            if (templist.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var responsemessage = helper.ApiCall(URL, EndPoints.AssignReturnPolicyToCatagory + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(responsemessage);
                var record = (AssignReturnPolicyToCatagoryLibrary)baseResponse.Data;
                record.Id = model.Id;
                record.ReturnPolicyDetailID = model.ReturnPolicyDetailID;
                record.CategoryID = model.CategoryID;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.AssignReturnPolicyToCatagory, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            //token = TokenRelay.GetBearerToken(HttpContext);
            var temp = helper.ApiCall(URL, EndPoints.AssignReturnPolicyToCatagory + "?id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignReturnPolicyToCatagoryLibrary> tempList = (List<AssignReturnPolicyToCatagoryLibrary>)baseResponse.Data;
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.AssignReturnPolicyToCatagory + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.AssignReturnPolicyToCatagory + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
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

            var response = helper.ApiCall(URL, EndPoints.AssignReturnPolicyToCatagory + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        public class AssignReturnPolicyToCatagoryDto
        {
            public int Id { get; set; }
            public int CategoryID { get; set; }
            public int ReturnPolicyDetailID { get; set; }
        }

    }
}
