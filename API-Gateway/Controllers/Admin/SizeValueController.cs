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
    public class SizeValueController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        public static string CatalogueUrl = string.Empty;
        BaseResponse<SizeLibrary> baseResponse = new BaseResponse<SizeLibrary>();
        private ApiHelper api;
        public SizeValueController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            api = new ApiHelper(_httpContext);
        }

        [HttpPost("CreateSizeValue")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> CreateChild(SizeValueDTO model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?TypeName=" + model.TypeName + "&parentId=" + model.ParentId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<SizeLibrary> sizeLibraries = (List<SizeLibrary>)baseResponse.Data;
            if (sizeLibraries.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                SizeLibrary size = new SizeLibrary();
                size.TypeName = model.TypeName;
                size.Status = "Active";
                size.ParentId = model.ParentId;
                size.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary, "POST", size);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(SizeValueDTO model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?TypeName=" + model.TypeName + "&parentId=" + model.ParentId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<SizeLibrary> sizeLibraries = (List<SizeLibrary>)baseResponse.Data;

            if (sizeLibraries.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                SizeLibrary record = new();
                record.Id = model.Id;
                record.TypeName = model.TypeName;
                record.ParentId = model.ParentId;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?Id=" + id + "&Getparent=false&Getchild=true", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<SizeLibrary> sizeLibraries = (List<SizeLibrary>)baseResponse.Data;

            if (sizeLibraries.Any())
            {
                var tempSizeValueToCategory = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?SizeId=" + id, "GET", null);
                BaseResponse<AssignSizeValueToCategory> baseResTaxRateToHsnCode = new BaseResponse<AssignSizeValueToCategory>();
                var SizeValueTOCategory = baseResTaxRateToHsnCode.JsonParseList(tempSizeValueToCategory);
                List<AssignSizeValueToCategory> assignSizeValues = (List<AssignSizeValueToCategory>)SizeValueTOCategory.Data;

                if (assignSizeValues.Any())
                {
                    baseResponse = baseResponse.ChildAlreadyExists("AssignSizeValueToCategory", "SizeValue");
                }

                var productPriceMaster = api.ApiCall(CatalogueUrl, EndPoints.ProductPriceMaster + "?SizeID=" + id, "Get", null);
                BaseResponse<ProductPrice> baseProductPriceResponse = new BaseResponse<ProductPrice>();
                var ProudctPriceBaseresponse = baseProductPriceResponse.JsonParseList(productPriceMaster);
                List<ProductPrice> productsPrice = (List<ProductPrice>)ProudctPriceBaseresponse.Data;
                if (productsPrice.Any())
                {
                    baseResponse = baseResponse.ChildAlreadyExists("ProductPrice", "SizeValue");
                }
                else
                {
                    response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?Id=" + id, "DELETE", null);
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
            var response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?Getparent=false&Getchild=true&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?Id=" + id + "&Getparent=false&Getchild=true", "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("byParentId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByParentId(int parentId, int? pageindex = 1, int? pageSize = 1)
        {
            var response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?parentId=" + parentId + "&Getparent=false&Getchild=true&PageIndex=" + pageindex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byName")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByName(string typeName)
        {
            var response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?Getparent=false&Getchild=true&TypeName=" + typeName, "GET", null);
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
            var response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?Getparent=false&Getchild=true&PageIndex=" + pageIndex + "&PageSize=" + pageSize+ url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }


    }
}
