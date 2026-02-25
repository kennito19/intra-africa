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
    public class SizeTypeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public static string CatalogueUrl = string.Empty;
        BaseResponse<SizeLibrary> baseResponse = new BaseResponse<SizeLibrary>();
        private ApiHelper api;
        public SizeTypeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor; 
            _httpContext = _httpContextAccessor.HttpContext;
            api = new ApiHelper(_httpContext);
            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpPost("CreateParentSize")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> CreateParent(SizeTypeDTO model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?TypeName=" + model.TypeName, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<SizeLibrary> sizeLibraries = baseResponse.Data as List<SizeLibrary> ?? new List<SizeLibrary>();
            if (sizeLibraries.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                SizeLibrary size = new SizeLibrary();
                size.TypeName = model.TypeName;
                size.Status = "Active";
                size.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                var response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary, "POST", size);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(SizeTypeDTO model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?TypeName=" + model.TypeName, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<SizeLibrary> sizeLibraries = baseResponse.Data as List<SizeLibrary> ?? new List<SizeLibrary>();
            if (sizeLibraries.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                SizeLibrary record = new SizeLibrary();
                record.Id = model.Id;
                record.TypeName = model.TypeName;
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
            var tempSizeType = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?Id=" + id + "&Getparent=true&Getchild=false", "GET", null);
            var tempResponse = baseResponse.JsonParseList(tempSizeType);
            List<SizeLibrary> sizeTypeLibraries = tempResponse.Data as List<SizeLibrary> ?? new List<SizeLibrary>();

            if (sizeTypeLibraries.Any())
            {
                var tempSize = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?parentId=" + id + "&Getparent=false&Getchild=true", "GET", null);
                tempResponse = baseResponse.JsonParseList(tempSize);
                List<SizeLibrary> sizeLibraries = tempResponse.Data as List<SizeLibrary> ?? new List<SizeLibrary>();

                if (sizeLibraries.Any())
                {
                    baseResponse = baseResponse.ChildAlreadyExists("SizeValue", "SizeType");
                }
                else
                {
                    var response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?Id=" + id, "DELETE", null);
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
            var response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?Getparent=true&Getchild=false&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
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

            var response = api.ApiCall(CatalogueUrl, EndPoints.SizeLibrary + "?Getparent=true&Getchild=false&PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
