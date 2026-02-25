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
    public class ExtraChargesController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;

        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ExtraChargesLibrary> baseResponse = new BaseResponse<ExtraChargesLibrary>();

        public ExtraChargesController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Save(ExtraChargesLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ExtraCharges + "?CatID=" + model.CatID + "&Name=" + model.Name.Replace("'", "''"), "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ExtraChargesLibrary> extrachar = baseResponse.Data as List<ExtraChargesLibrary> ?? new List<ExtraChargesLibrary>();
            if (extrachar.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ExtraChargesLibrary ec = new ExtraChargesLibrary();
                ec.CatID = model.CatID;
                ec.Name = model.Name;
                ec.ChargesPaidByID = model.ChargesPaidByID;
                ec.ChargesOn = "Specific Category";
                ec.IsCompulsary = false;
                ec.ChargesIn = model.ChargesIn;
                ec.PercentageValue = model.PercentageValue;
                ec.AmountValue = model.AmountValue;
                ec.MaxAmountValue = model.MaxAmountValue;
                ec.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ExtraCharges, "POST", ec);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ExtraChargesLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ExtraCharges + "?isDeleted=false&PageIndex=0&PageSize=0&Mode=get", "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ExtraChargesLibrary> templist = baseResponse.Data as List<ExtraChargesLibrary> ?? new List<ExtraChargesLibrary>();
            if (templist.Where(x => x.Id != model.Id && x.CatID == model.CatID && x.Name == model.Name.Replace("'", "''")).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var Getrecord = helper.ApiCall(URL, EndPoints.ExtraCharges + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(Getrecord);
                var record = baseResponse.Data as ExtraChargesLibrary;
                record.CatID = model.CatID;
                record.Name = model.Name;
                record.ChargesPaidByID = model.ChargesPaidByID;
                record.ChargesOn = "Specific Category";
                record.IsCompulsary = false;
                record.ChargesIn = model.ChargesIn;
                record.PercentageValue = model.PercentageValue;
                record.AmountValue = model.AmountValue;
                record.MaxAmountValue = model.MaxAmountValue;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ExtraCharges, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }


        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ExtraCharges + "?id=" + id + "&isDeleted=false", "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ExtraChargesLibrary> templist = baseResponse.Data as List<ExtraChargesLibrary> ?? new List<ExtraChargesLibrary>();

            if (templist.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ExtraCharges + "?Id=" + id + "&isDeleted=false", "DELETE", null);
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
            var response = helper.ApiCall(URL, EndPoints.ExtraCharges + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&isDeleted=false", "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("Search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(URL, EndPoints.ExtraCharges + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&isDeleted=false" + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }
    }
}
