using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class TaxTypeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper api;
        private readonly IConfiguration _configuration;
        BaseResponse<TaxTypeLibrary> baseResponse = new BaseResponse<TaxTypeLibrary>();
        public static string CatalogueUrl = string.Empty;

        public TaxTypeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            api = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(TaxTypeDTO model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?taxType=" + model.TaxType + "&ParentID=" + model.ParentId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxTypeLibrary> tempList = (List<TaxTypeLibrary>)baseResponse.Data;

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var temp1 = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?taxType=" + model.TaxType + "&ParentID=" + model.ParentId + "&isDeleted=" + true, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp1);
                List<TaxTypeLibrary> tempList1 = (List<TaxTypeLibrary>)baseResponse.Data;
                TaxTypeLibrary taxType = new TaxTypeLibrary();
                if (tempList1.Any())
                {
                    var data = tempList1.FirstOrDefault();
                    taxType.Id = data.Id;
                    taxType.TaxType = model.TaxType;
                    taxType.ParentId = model.ParentId;
                    taxType.CreatedAt = DateTime.Now;
                    taxType.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    taxType.ModifiedAt = DateTime.Now;
                    taxType.ModifiedBy = null;
                    taxType.IsDeleted = false;
                    taxType.DeletedAt = null;
                    taxType.DeletedBy = null;
                    var response = api.ApiCall(CatalogueUrl, EndPoints.TaxType, "PUT", taxType);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                    if (baseResponse.code == 200)
                    {
                        baseResponse.Message = "Record added successfully.";
                    }

                }
                else
                {
                    taxType.TaxType = model.TaxType;
                    taxType.ParentId = model.ParentId;
                    taxType.CreatedAt = DateTime.Now;
                    taxType.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    var response = api.ApiCall(CatalogueUrl, EndPoints.TaxType, "POST", taxType);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(TaxTypeDTO model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?taxType=" + model.TaxType + "&ParentID=" + model.ParentId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxTypeLibrary> tempList = (List<TaxTypeLibrary>)baseResponse.Data;

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var temp1 = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(temp1);

                if (baseResponse.code == 200)
                {
                    TaxTypeLibrary taxType = (TaxTypeLibrary)baseResponse.Data;
                    
                    taxType.TaxType = model.TaxType;
                    taxType.ParentId = model.ParentId;
                    taxType.ModifiedAt = DateTime.Now;
                    taxType.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    var response = api.ApiCall(CatalogueUrl, EndPoints.TaxType, "PUT", taxType);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxTypeLibrary> taxType = (List<TaxTypeLibrary>)baseResponse.Data;
            if (!taxType.Any())
            {
                baseResponse = baseResponse.NotExist();
            }
            else if (taxType.Any())
            {
                var tempTaxTypeValue = api.ApiCall(CatalogueUrl, EndPoints.TaxTypeValue + "?TaxTypeId=" + taxType[0].ParentId, "GET", null);
                BaseResponse<TaxTypeValueLibrary> baseResTaxTypeValue = new BaseResponse<TaxTypeValueLibrary>();
                var TaxTypeValue = baseResTaxTypeValue.JsonParseList(tempTaxTypeValue);
                List<TaxTypeValueLibrary> taxTypeValue = (List<TaxTypeValueLibrary>)TaxTypeValue.Data;
                if (taxTypeValue.Any())
                {
                    baseResponse = baseResponse.ChildAlreadyExists("TaxTypeValue", "TaxType");
                }
                else
                {
                    var response = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            return Ok(baseResponse);
        }

        [HttpGet("byParentId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByParentId(int parentId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?ParentID=" + parentId + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Getchild=" + true, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
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

            var response = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Getchild=" + true + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
