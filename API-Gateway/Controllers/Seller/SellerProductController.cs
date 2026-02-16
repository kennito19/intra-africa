using API_Gateway.Common;
using API_Gateway.Common.products;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Seller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerProductController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public static string CatalogueUrl = string.Empty;
        public static string IdentityServerUrl = string.Empty;
        public static string userUrl = string.Empty;
        private PriceCalculation _priceCalculation;
        private ImageUpload imageUpload;

        public SellerProductController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, PriceCalculation priceCalculation)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;

            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IdentityServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            userUrl = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            imageUpload = new ImageUpload(_configuration);
            _priceCalculation = priceCalculation;
        }

        // Here i add same model as admin have.
        [HttpPost("Product")]
        [Authorize]
        public ActionResult<ApiHelper> SaveProduct(ProductDetails model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, userID);
            var res = saveProduct.SaveData(model);
            return Ok(res);
        }

        [HttpPut("Product")]
        [Authorize]
        public ActionResult<ApiHelper> UpdateProduct(ProductDetails model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            UpdateProduct updateProduct = new UpdateProduct(_configuration, _httpContext, userID);
            var res = updateProduct.SaveData(model);
            return Ok(res);
        }

        [HttpPut("QuickUpdate")]
        [Authorize]
        public ActionResult<ApiHelper> QuickUpdate(QuickProductUpdateDTO model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            UpdateProduct updateProduct = new UpdateProduct(_configuration, _httpContext, userID);
            var res = updateProduct.QuickUpdate(model);
            return Ok(res);
        }

        [HttpGet("SellerProducts")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> SellerProducts(string SellerId, int? pageIndex = 1, int? pageSize = 10)
        {
            GetProduct getProduct = new GetProduct(_configuration, _httpContext);
            var res = getProduct.GetSellerProducts(SellerId, pageIndex, pageSize);
            return Ok(res);
        }

    }
}
