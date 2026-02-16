using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionProductsListController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public static string CatalogueUrl = string.Empty;
        public static string IdentityServerUrl = string.Empty;
        public static string userUrl = string.Empty;
        private ApiHelper helper;
        public CollectionProductsListController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IdentityServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            userUrl = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpGet("getCollectionProductList")]
        [Authorize]
        public ActionResult<ApiHelper> GetCollectionProductList(int collectionId = 0, int id = 0, int categoryId = 0, string? sellerId = null, int? brandId = 0, int pageIndex = 1, int pageSize = 10, string? searchText = null)
        {
            BaseResponse<CollectionProductsListDto> baseResponse = new BaseResponse<CollectionProductsListDto>();

            string url = string.Empty;
            if (id != 0 && id != null)
            {
                url += "&Id=" + id;
            }
            if (collectionId != 0 && collectionId != null)
            {
                url += "&collectionId=" + collectionId;
            }
            if (categoryId != 0 && categoryId != null)
            {
                url += "&categoryId=" + categoryId;
            }
            if (brandId != 0 && brandId != null)
            {
                url += "&brandId=" + brandId;
            }
            if (!string.IsNullOrEmpty(sellerId))
            {
                url += "&sellerId=" + sellerId;
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                url += "&searchText=" + HttpUtility.UrlEncode(searchText);
            }

            BaseResponse<CollectionProductsList> baseResponse1 = new BaseResponse<CollectionProductsList>();
            var GetResponse = helper.ApiCall(CatalogueUrl, EndPoints.GetCollectionProductsList + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize + url, "GET", null);
            baseResponse1 = baseResponse1.JsonParseList(GetResponse);
            if (baseResponse1.code == 200)
            {
                var dataresponse = (List<CollectionProductsList>)baseResponse1.Data;
                baseResponse.code = baseResponse1.code;
                baseResponse.Message = baseResponse1.Message;
                baseResponse.pagination = baseResponse1.pagination;
                baseResponse.Data = dataresponse.Select(details => new CollectionProductsListDto
                {
                    Id = details.Id,
                    Guid = details.Guid,
                    IsMasterProduct = (bool)details.IsMasterProduct,
                    ParentId = details.ParentId,
                    CategoryId = (int)details.CategoryId,
                    ProductName = details.ProductName,
                    CustomeProductName = details.CustomeProductName,
                    CompanySKUCode = details.CompanySKUCode,
                    Image1 = details.Image1,
                    MRP = (decimal)details.MRP,
                    SellingPrice = (decimal)details.SellingPrice,
                    Discount = (decimal)details.Discount,
                    Quantity = (int)details.Quantity,
                    CategoryName = details.CategoryName,
                    CategoryPathIds = details.CategoryPathIds,
                    CategoryPathNames = details.CategoryPathNames,
                    SellerProductId = (int)details.SellerProductId,
                    SellerId = details.SellerId,
                    BrandId = (int)details.BrandId,
                    Status = details.Status,
                    Live = details.Live,
                    SellerSKU = details.SellerSKU,
                    ManufacturedDate = details.ManufacturedDate,
                    ExpiryDate = details.ExpiryDate,
                    SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                    BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? details.ExtraDetails : null,
                }).ToList();
            }
            else
            {
                baseResponse.code = baseResponse1.code;
                baseResponse.Message = baseResponse1.Message;
                baseResponse.pagination = baseResponse1.pagination;
                baseResponse.Data = baseResponse1.Data;
            }


            return Ok(baseResponse);

        }


        [NonAction]
        public ProductPrice getProductPrice(int SellerProductID = 0)
        {
            ProductPrice productPrice = new ProductPrice();
            BaseResponse<ProductPrice> baseResponse = new BaseResponse<ProductPrice>();
            var GetResponse = helper.ApiCall(CatalogueUrl, EndPoints.ProductPriceMaster + "?pageIndex=0&pageSize=0&SellerProductID=" + SellerProductID, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);
            if (baseResponse.code == 200)
            {
                var data = baseResponse.Data as List<ProductPrice>;

                productPrice = data.OrderBy(x => x.SellingPrice).FirstOrDefault();
            }

            return productPrice;
        }
    }
}
