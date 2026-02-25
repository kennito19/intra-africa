using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxMappingController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper api;
        private readonly IConfiguration _configuration;
        BaseResponse<TaxMapping> baseResponse = new BaseResponse<TaxMapping>();
        public static string CatalogueUrl = string.Empty;

        public TaxMappingController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            api = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(TaxMappingDto model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.TaxMapping + "?taxId=" + model.TaxId + "&taxTypeId=" + model.TaxTypeId+ "&taxMapBy=" + model.TaxMapBy, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxMapping> tempList = baseResponse.Data as List<TaxMapping> ?? new List<TaxMapping>();

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var temp1 = api.ApiCall(CatalogueUrl, EndPoints.TaxMapping + "?taxId=" + model.TaxId + "&taxTypeId=" + model.TaxTypeId + "&taxMapBy=" + model.TaxMapBy + "&isDeleted=" + true, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp1);
                List<TaxMapping> tempList1 = baseResponse.Data as List<TaxMapping> ?? new List<TaxMapping>();
                TaxMapping taxMap = new TaxMapping();
                if (tempList1.Any())
                {
                    var data = tempList1.FirstOrDefault();
                    taxMap.Id = data.Id;
                    taxMap.TaxId = model.TaxId;
                    taxMap.TaxTypeId = model.TaxTypeId;
                    taxMap.TaxMapBy = model.TaxMapBy;
                    taxMap.SpecificState = model.SpecificState;
                    taxMap.SpecificTaxTypeId = model.SpecificTaxTypeId;
                    taxMap.CreatedAt = DateTime.Now;
                    taxMap.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    taxMap.ModifiedAt = DateTime.Now;
                    taxMap.ModifiedBy = null;
                    taxMap.IsDeleted = false;
                    taxMap.DeletedAt = null;
                    taxMap.DeletedBy = null;

                    var response = api.ApiCall(CatalogueUrl, EndPoints.TaxMapping, "PUT", taxMap);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                    if(baseResponse.code == 200)
                    {
                        baseResponse.Message = "Record added successfully.";
                    }

                }
                else
                {
                    taxMap.TaxId = model.TaxId;
                    taxMap.TaxTypeId = model.TaxTypeId;
                    taxMap.TaxMapBy = model.TaxMapBy;
                    taxMap.SpecificState = model.SpecificState;
                    taxMap.SpecificTaxTypeId = model.SpecificTaxTypeId;
                    taxMap.CreatedAt = DateTime.Now;
                    taxMap.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    var response = api.ApiCall(CatalogueUrl, EndPoints.TaxMapping, "POST", taxMap);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(TaxMappingDto model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.TaxMapping + "?taxId=" + model.TaxId + "&taxTypeId=" + model.TaxTypeId + "&taxMapBy=" + model.TaxMapBy, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxMapping> tempList = baseResponse.Data as List<TaxMapping> ?? new List<TaxMapping>();


            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var temp1 = api.ApiCall(CatalogueUrl, EndPoints.TaxMapping + "?id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(temp1);

                if (baseResponse.code == 200)
                {
                    TaxMapping taxMap = baseResponse.Data as TaxMapping;

                    taxMap.TaxId = model.TaxId;
                    taxMap.TaxTypeId = model.TaxTypeId;
                    taxMap.TaxMapBy = model.TaxMapBy;
                    taxMap.SpecificState = model.SpecificState;
                    taxMap.SpecificTaxTypeId = model.SpecificTaxTypeId;
                    taxMap.ModifiedAt = DateTime.Now;
                    taxMap.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    var response = api.ApiCall(CatalogueUrl, EndPoints.TaxMapping, "PUT", taxMap);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.TaxMapping + "?id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxMapping> taxMap = baseResponse.Data as List<TaxMapping> ?? new List<TaxMapping>();
            if (!taxMap.Any())
            {
                baseResponse = baseResponse.NotExist();
            }
            else if (taxMap.Any())
            {
                var response = api.ApiCall(CatalogueUrl, EndPoints.TaxMapping + "?id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);

            }
            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(int? id = 0,int? taxId = 0,int? taxTypeId = 0,int? taxValueId = 0, string? tax = null,string? taxType = null,string? taxMapBy = null,string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;

            if (id != 0 && id != null)
            {
                url += "&id=" + id;
            }
            if (taxId != 0 && taxId != null)
            {
                url += "&taxId=" + taxId;
            }
            if (taxTypeId != 0 && taxTypeId != null)
            {
                url += "&taxTypeId=" + taxTypeId;
            }
            if (taxValueId != 0 && taxValueId != null)
            {
                url += "&taxValueId=" + taxValueId;
            }
            if (!string.IsNullOrEmpty(tax) && tax != "")
            {
                url += "&tax=" + HttpUtility.UrlEncode(tax);
            }
            if (!string.IsNullOrEmpty(taxType) && taxType != "")
            {
                url += "&taxType=" + HttpUtility.UrlEncode(taxType);
            }
            if (!string.IsNullOrEmpty(taxMapBy) && taxMapBy != "")
            {
                url += "&taxMapBy=" + HttpUtility.UrlEncode(taxMapBy);
            }
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url += "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = api.ApiCall(CatalogueUrl, EndPoints.TaxMapping + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Getchild=" + true + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
