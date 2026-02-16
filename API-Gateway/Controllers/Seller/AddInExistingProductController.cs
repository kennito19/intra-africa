using API_Gateway.Common.products;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Seller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddInExistingProductController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        private string token = string.Empty;
        public static string CatalogueUrl = string.Empty;
        public static string IdentityServerUrl = string.Empty;
        public static string userUrl = string.Empty;

        public AddInExistingProductController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IdentityServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            userUrl = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
        }

        [HttpPost("addExistingProduct")]
        [Authorize(Roles = "Seller,Admin")]
        public ActionResult<ApiHelper> SaveProduct(ExistingProduct model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, userID);
            var res = saveProduct.SaveExistingData(model);
            return Ok(res);
        }

        [HttpGet("getProductlist")]
        [Authorize(Roles = "Seller,Admin")]
        public ActionResult<ApiHelper> getProductlist(string sellerId, int? categoryId = 0, int? brandId = 0, string? companySkuCode = null,string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            GetProduct getProduct = new GetProduct(_configuration, _httpContext);
            //var res = getProduct.GetProductlist(pageIndex, pageSize, productId, productMasterId, categoryID, productGuid, productSKUCode, isMasterProduct, getparent, getchild, searchText, isDeleted);
            var res = getProduct.getAddInExistingProductList(sellerId, categoryId, brandId, companySkuCode, searchtext, pageIndex, pageSize);
            return Ok(res);
        }
    }
}
