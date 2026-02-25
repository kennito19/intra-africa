using API_Gateway.Helper;
using API_Gateway.Models;
using API_Gateway.Models.Dto;
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
    public class TaxTypeValueController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        Response<TaxTypeValueLibrary> res = new Response<TaxTypeValueLibrary>();
        BaseResponse<TaxTypeValueLibrary> baseResponse = new BaseResponse<TaxTypeValueLibrary>();
        public static string CatalogueUrl = string.Empty;
        private ApiHelper api;
        public TaxTypeValueController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            api = new ApiHelper(_httpContext);
            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(TaxTypeValueDTO model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.TaxTypeValue + "?taxTypeId=" + model.TaxTypeID + "&name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxTypeValueLibrary> tempList = baseResponse.Data as List<TaxTypeValueLibrary> ?? new List<TaxTypeValueLibrary>();
            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var temp1 = api.ApiCall(CatalogueUrl, EndPoints.TaxTypeValue + "?taxTypeId=" + model.TaxTypeID + "&name=" + model.Name + "&isDeleted=" + true, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp1);
                List<TaxTypeValueLibrary> tempList1 = baseResponse.Data as List<TaxTypeValueLibrary> ?? new List<TaxTypeValueLibrary>();

                TaxTypeValueLibrary taxType = new TaxTypeValueLibrary();
                if (tempList1.Any())
                {
                    var data = tempList1.FirstOrDefault();
                    taxType.TaxTypeID = model.TaxTypeID;
                    taxType.Name = model.Name;
                    taxType.Value = model.Value;
                    taxType.CreatedAt = DateTime.Now;
                    taxType.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    taxType.ModifiedAt = DateTime.Now;
                    taxType.ModifiedBy = null;
                    taxType.IsDeleted = false;
                    taxType.DeletedAt = null;
                    taxType.DeletedBy = null;
                    var response = api.ApiCall(CatalogueUrl, EndPoints.TaxTypeValue, "PUT", taxType);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                    if (baseResponse.code == 200)
                    {
                        baseResponse.Message = "Record added successfully.";
                    }

                }
                else
                {
                    taxType.TaxTypeID = model.TaxTypeID;
                    taxType.Name = model.Name;
                    taxType.Value = model.Value;
                    taxType.CreatedAt = DateTime.Now;
                    taxType.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    var response = api.ApiCall(CatalogueUrl, EndPoints.TaxTypeValue, "POST", taxType);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(TaxTypeValueDTO model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.TaxTypeValue + "?taxTypeId=" + model.TaxTypeID + "&name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxTypeValueLibrary> tempList = baseResponse.Data as List<TaxTypeValueLibrary> ?? new List<TaxTypeValueLibrary>();

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var temp1 = api.ApiCall(CatalogueUrl, EndPoints.TaxTypeValue + "?id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(temp1);

                if (baseResponse.code == 200)
                {
                    TaxTypeValueLibrary taxType = baseResponse.Data as TaxTypeValueLibrary;
                    taxType.TaxTypeID = model.TaxTypeID;
                    taxType.Value = model.Value;
                    taxType.Name = model.Name;
                    taxType.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    taxType.ModifiedAt = DateTime.Now;
                    var response = api.ApiCall(CatalogueUrl, EndPoints.TaxTypeValue, "PUT", taxType);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.TaxTypeValue + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxTypeValueLibrary> taxType = baseResponse.Data as List<TaxTypeValueLibrary> ?? new List<TaxTypeValueLibrary>();
            if (!taxType.Any())
            {
                baseResponse = baseResponse.NotExist();
            }
            else if (taxType.Any())
            {
                var tempAssignTaxRate = api.ApiCall(CatalogueUrl, EndPoints.AssignTaxRateToHSNCode + "?TaxValueId=" + id, "GET", null);
                BaseResponse<AssignTaxRateToHSNCode> baseResAssignTaxTypeValue = new BaseResponse<AssignTaxRateToHSNCode>();
                var TaxTypeValue = baseResAssignTaxTypeValue.JsonParseList(tempAssignTaxRate);
                List<AssignTaxRateToHSNCode> taxRateToHsnCode = TaxTypeValue.Data as List<AssignTaxRateToHSNCode> ?? new List<AssignTaxRateToHSNCode>();
                if (taxRateToHsnCode.Any())
                {
                    baseResponse = baseResponse.ChildAlreadyExists("AssignTaxRateToHSNCode", "TaxTypeValue");
                }
                else
                {
                    var response = api.ApiCall(CatalogueUrl, EndPoints.TaxTypeValue + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext=null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = api.ApiCall(CatalogueUrl, EndPoints.TaxTypeValue + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
