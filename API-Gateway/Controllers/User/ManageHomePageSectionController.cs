using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API_Gateway.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageHomePageSectionController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string UserURL = string.Empty;
        BaseResponse<ManageHomePageSectionsLibrary> baseResponse = new BaseResponse<ManageHomePageSectionsLibrary>();
        private readonly ApiHelper helper;

        public ManageHomePageSectionController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            UserURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpGet("GetHomePageSection")]
        [Authorize]
        public ActionResult<ApiHelper> GetHomePageSection(string? status = null)
        {
            getHomePageSections getHomePage = new getHomePageSections(_configuration, _httpContext);
            JObject res = getHomePage.setSections(status);
            return Ok(res);
        }


        [HttpGet("GetMenu")]
        [Authorize]
        public ActionResult<ApiHelper> GetMenu()
        {
            getHomePageSections getHomePage = new getHomePageSections(_configuration, _httpContext);
            var res = getHomePage.getmenuList();
            BaseResponse<HomepageMenu> baseResponse = new BaseResponse<HomepageMenu>();
            baseResponse.Data = res;
            baseResponse.Message = "Records bind successfully";
            baseResponse.code = 200;

            return Ok(baseResponse);
        }

        [HttpGet("GetBrands")]
        [AllowAnonymous]
        public ActionResult<ApiHelper> GetBrands(int? pageIndex = 0, int? pageSize = 0, string? status = "Active")
        {
            BaseResponse<BrandLibrary> brandResponse = new BaseResponse<BrandLibrary>();
            var response = helper.ApiCall(
                UserURL,
                EndPoints.Brand + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize,
                "GET",
                null
            );

            brandResponse = brandResponse.JsonParseList(response);
            List<BrandLibrary> brands = brandResponse.Data as List<BrandLibrary> ?? new List<BrandLibrary>();

            if (!string.IsNullOrWhiteSpace(status))
            {
                brands = brands.Where(b =>
                    string.Equals(b.Status, status, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            if (brands.Count > 0)
            {
                brandResponse.code = 200;
                brandResponse.Message = "Record bind successfully.";
                brandResponse.Data = brands;
            }
            else
            {
                brandResponse.code = 204;
                brandResponse.Message = "Record does not exist.";
                brandResponse.Data = new List<BrandLibrary>();
            }

            return Ok(brandResponse);
        }

        [HttpGet("GetFeaturedProducts")]
        [AllowAnonymous]
        public ActionResult<ApiHelper> GetFeaturedProducts(int? topProduct = 8)
        {
            BaseResponse<ProductHomePageSectionLibrary> productResponse = new BaseResponse<ProductHomePageSectionLibrary>();
            var response = helper.ApiCall(
                URL,
                EndPoints.ManageProductHomePageSections + "?categoryId=0&topProduct=" + (topProduct ?? 8) + "&productId=",
                "GET",
                null
            );

            productResponse = productResponse.JsonParseList(response);
            List<ProductHomePageSectionLibrary> products =
                productResponse.Data as List<ProductHomePageSectionLibrary> ?? new List<ProductHomePageSectionLibrary>();

            if (products.Count > 0)
            {
                productResponse.code = 200;
                productResponse.Message = "Record bind successfully.";
                productResponse.Data = products;
            }
            else
            {
                productResponse.code = 204;
                productResponse.Message = "Record does not exist.";
                productResponse.Data = new List<ProductHomePageSectionLibrary>();
            }

            return Ok(productResponse);
        }
    }
}
