using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper api;
        private readonly IConfiguration _configuration;
        BaseResponse<TaxTypeLibrary> baseResponse = new BaseResponse<TaxTypeLibrary>();
        public static string CatalogueUrl = string.Empty;

        public TaxController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            api = new ApiHelper(_httpContext);
            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(TaxDTO model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?taxType=" + model.TaxType + "&Getparent=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxTypeLibrary> taxTypes = baseResponse.Data as List<TaxTypeLibrary> ?? new List<TaxTypeLibrary>();
            if (taxTypes.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                TaxTypeLibrary tax = new TaxTypeLibrary();
                tax.TaxType = model.TaxType;
                tax.ParentId = null;
                tax.CreatedAt = DateTime.Now;
                tax.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = api.ApiCall(CatalogueUrl, EndPoints.TaxType, "POST", tax);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(TaxDTO model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?taxType=" + model.TaxType + "&Getparent=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxTypeLibrary> taxTypes = baseResponse.Data as List<TaxTypeLibrary> ?? new List<TaxTypeLibrary>();

            if (taxTypes.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                TaxTypeLibrary tax = new TaxTypeLibrary();
                tax.Id = model.Id;
                tax.TaxType = model.TaxType;
                tax.ModifiedAt = DateTime.Now;
                tax.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = api.ApiCall(CatalogueUrl, EndPoints.TaxType, "PUT", tax);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int Id)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?Id=" + Id + "&Getparent=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxTypeLibrary> tmp = baseResponse.Data as List<TaxTypeLibrary> ?? new List<TaxTypeLibrary>();
            if (tmp.Any())
            {
                baseResponse = baseResponse.NotExist();
            }
            temp = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?ParentID=" + Id + "&Getchild=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<TaxTypeLibrary> tempList = baseResponse.Data as List<TaxTypeLibrary> ?? new List<TaxTypeLibrary>();

            if (tempList.Any())
            {
                baseResponse = baseResponse.ChildExists();
            }
            else
            {
                var response = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?Id=" + Id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
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

            var response = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Getparent=" + true + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }


    }
}
