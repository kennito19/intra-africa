using API_Gateway.Helper;
using API_Gateway.Models.Entity;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignTaxRateToHSNCodeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<AssignTaxRateToHSNCode> baseResponse = new BaseResponse<AssignTaxRateToHSNCode>();
        private ApiHelper helper;
        public AssignTaxRateToHSNCodeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(AssignTaxRateToHSNCode model)
        {
            var temp = helper.ApiCall(URL, EndPoints.AssignTaxRateToHSNCode + "?HsnCodeId=" + model.HsnCodeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignTaxRateToHSNCode> tempList = baseResponse.Data as List<AssignTaxRateToHSNCode> ?? new List<AssignTaxRateToHSNCode>();
            if (tempList.Count > 0)
            {
                tempList = tempList.Where(p => p.HsnCodeId == model.HsnCodeId).ToList();
            }
            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                AssignTaxRateToHSNCode rateToHSNCode = new AssignTaxRateToHSNCode();
                rateToHSNCode.HsnCodeId = model.HsnCodeId;
                rateToHSNCode.TaxValueId = model.TaxValueId;
                rateToHSNCode.CreatedAt = DateTime.Now;
                rateToHSNCode.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.AssignTaxRateToHSNCode, "POST", rateToHSNCode);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(AssignTaxRateToHSNCode model)
        {
            var temp = helper.ApiCall(URL, EndPoints.AssignTaxRateToHSNCode + "?HsnCodeId=" + model.HsnCodeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignTaxRateToHSNCode> tempList = baseResponse.Data as List<AssignTaxRateToHSNCode> ?? new List<AssignTaxRateToHSNCode>();

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.AssignTaxRateToHSNCode + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                AssignTaxRateToHSNCode rateToHSNCode = baseResponse.Data as AssignTaxRateToHSNCode;
                rateToHSNCode.Id = model.Id;
                rateToHSNCode.HsnCodeId = model.HsnCodeId;
                rateToHSNCode.TaxValueId = model.TaxValueId;
                rateToHSNCode.ModifiedAt = DateTime.Now;
                rateToHSNCode.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.AssignTaxRateToHSNCode, "PUT", rateToHSNCode);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.AssignTaxRateToHSNCode + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(temp);
            AssignTaxRateToHSNCode tempList = baseResponse.Data as AssignTaxRateToHSNCode;
            if (tempList != null)
            {
                var tempTaxRateToHsnCode = helper.ApiCall(URL, EndPoints.Product + "?HsnCode=" + tempList.HsnCode + "&TaxValueId=" + tempList.TaxValueId, "GET", null);
                BaseResponse<Products> checkproductBaseresponse = new BaseResponse<Products>();
                var assignTaxRateToHsnCode = checkproductBaseresponse.JsonParseList(tempTaxRateToHsnCode);
                List<Products> taxRateToHsnCode = assignTaxRateToHsnCode.Data as List<Products> ?? new List<Products>();
                if (taxRateToHsnCode.Any())
                {
                    baseResponse = baseResponse.ChildAlreadyExists("Product", "AssignTaxRateToHsnCode");
                }
                else
                {
                    var response = helper.ApiCall(URL, EndPoints.AssignTaxRateToHSNCode + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
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
            var response = helper.ApiCall(URL, EndPoints.AssignTaxRateToHSNCode + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
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

            var response = helper.ApiCall(URL, EndPoints.AssignTaxRateToHSNCode + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }
    }
}
