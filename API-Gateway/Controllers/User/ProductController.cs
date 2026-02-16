using API_Gateway.Common.products;
using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class UserProductController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public static string CatalogueUrl = string.Empty;
        public UserProductController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;

            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Get(int? CategoryId = 0, string? SellerIds = null, string? BrandIds = null, string? searchTexts = null, string? SizeIds = null, string? ColorIds = null, string? productCollectionId = null, string? MinPrice = null, string? MaxPrice = null, string? MinDiscount = null, bool? available = false, int? PriceSort = 0, string? SpecTypeValueIds = null, int? pageIndex = 1, int? pageSize = 30, string? userId = null, string? fby = null)
        {
            GetProduct product = new GetProduct(_configuration, _httpContext);
            if (userId == null)
            {
                userId = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault() != null ? HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value : null;
            }


            return Ok(product.GetCustomerProductLists1(CategoryId, SellerIds, BrandIds, searchTexts, SizeIds, ColorIds, productCollectionId, MinPrice, MaxPrice, MinDiscount, available, PriceSort, SpecTypeValueIds, pageIndex, pageSize, userId, fby));
        }

        [HttpGet("ById")]
        [Authorize]
        public ActionResult GetById(string ProductGUID, int? sizeId = 0, string? sellerId = null, string? status = null, bool? isProductExist = null, bool? isDeleted = null, bool? isArchive = null, string? userId = null)
        {
            GetProduct product = new GetProduct(_configuration, _httpContext);

            //return Ok(product.GetCustomerProductDetails(ProductGUID, sizeId, sellerId, status, isProductExist, isDeleted, isArchive));

            if (userId == null)
            {
                userId = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault() != null ? HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value : null;
            }
            return Ok(product.GetCustomerProductDetails(ProductGUID, userId));
        }

        [HttpGet("productComparison")]
        [Authorize]
        public ActionResult<string> productComparison(string SellerProductId)
        {
            GetProduct getProduct = new GetProduct(_configuration, _httpContext);
            var res = getProduct.GetProductComparision(SellerProductId);
            return Ok(res.ToString());
        }

        [HttpGet("productComparisonBrand")]
        [Authorize]
        public ActionResult productComparisonBrand(int CategoryId)
        {
            GetProduct getProduct = new GetProduct(_configuration, _httpContext);
            return Ok(getProduct.BindProductCompareBrand(CategoryId));
        }

        [HttpGet("productComparisonBrandProduct")]
        [Authorize]
        public ActionResult productComparisionBrandProduct(int CategoryId, int BrandId)
        {
            GetProduct getProduct = new GetProduct(_configuration, _httpContext);
            return Ok(getProduct.BindProductCompareBrandProduct(CategoryId, BrandId));
        }


        [HttpGet("searchsuggestion")]
        [Authorize]
        public ActionResult<ApiHelper> searchsuggestion(string searchText)
        {
            GetProduct getProduct = new GetProduct(_configuration, _httpContext);
            return Ok(getProduct.searchsuggestion(searchText));
        }

    }
}
