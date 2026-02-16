using API_Gateway.Helper;
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
    public class MasterProductListController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ProductListLibrary> baseResponse = new BaseResponse<ProductListLibrary>();
        public string IDServerUrl = string.Empty;
        public string UserUrl = string.Empty;
        private ApiHelper helper;
        public MasterProductListController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            UserUrl = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? categoryId = 0, int? brandId = 0, string? sellerId = null, string? status = null, bool? Live = null, string? searchText = null, int? pageindex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            bool isSellerList = false;
            if (categoryId != null && categoryId != 0)
            {
                url += "&CategoryID=" + categoryId;
            }
            if (brandId != null && brandId != 0)
            {
                url += "&BrandId=" + brandId;
            }

            if (!string.IsNullOrEmpty(sellerId) && sellerId != "")
            {
                url += "&SellerId=" + sellerId;
                isSellerList = true;
            }

            if (!string.IsNullOrEmpty(status) && status != "")
            {
                url += "&Status=" + status;
            }

            if (!string.IsNullOrEmpty(searchText) && searchText != "")
            {
                url += "&searchText=" + HttpUtility.UrlEncode(searchText);
            }

            if (Live != null)
            {
                url += "&Live=" + Live;
            }

            var response = helper.ApiCall(URL, EndPoints.MasterProductListDetail + "?PageIndex=" + pageindex + "&PageSize=" + pageSize + url + "&IsDeleted=false", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ProductListLibrary> tempList = (List<ProductListLibrary>)baseResponse.Data;

            List<ProductListLibrary> parentList = tempList.Where(p => p.ParentId == null || p.ParentId == 0).ToList();

            List<ProductListLibrary> tempchildList = new List<ProductListLibrary>();

            parentList = (from ProductListLibrary item in parentList
                select new ProductListLibrary()
                {

                    RowNumber = item.RowNumber,
                    PageCount = item.PageCount,
                    RecordCount = item.RecordCount,
                    Id = item.Id,
                    Guid = item.Guid,
                    IsMasterProduct = item.IsMasterProduct,
                    ParentId = item.ParentId,
                    CategoryId = item.CategoryId,
                    AssiCategoryId = item.AssiCategoryId,
                    ProductName = item.ProductName,
                    CustomeProductName = item.CustomeProductName,
                    CompanySKUCode = item.CompanySKUCode,
                    Image1 = item.Image1,
                    MRP = item.MRP,
                    SellingPrice = item.SellingPrice,
                    Discount = item.Discount,
                    Quantity = item.Quantity,
                    CategoryName = item.CategoryName,
                    CategoryPathIds = item.CategoryPathIds,
                    CategoryPathNames = item.CategoryPathNames,
                    SellerProductId = item.SellerProductId,
                    SellerId = item.SellerId,
                    SellerName = item.SellerName,
                    IsExistingProduct = item.IsExistingProduct,
                    BrandId = tempList.Where(p => p.ParentId == item.Id && (p.ParentId != null || p.ParentId != 0)).Select(p => p.BrandId).FirstOrDefault(),
                    BrandName = tempList.Where(p => p.ParentId == item.Id && (p.ParentId != null || p.ParentId != 0)).Select(p => p.BrandName).FirstOrDefault(),
                    TotalQTY = item.TotalQTY,
                    Status = item.Status,
                    Live = item.Live,
                    TotalVariant = item.TotalVariant,
                    CreatedBy = item.CreatedBy,
                    CreatedAt = item.CreatedAt,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedAt = item.ModifiedAt,
                    DeletedBy = item.DeletedBy,
                    DeletedAt = item.DeletedAt,
                    IsDeleted = item.IsDeleted,
                    SearchText = item.SearchText,
                    SizeVariant = item.SizeVariant,
                    ColorVariant = item.ColorVariant,
                    SpecificationVariant = item.SpecificationVariant,
                    totalSellerCount = item.totalSellerCount,
                    IsAllowVariant = item.IsAllowVariant,

                    ChildList = CL(tempList.Where(p => p.ParentId == item.Id && (p.ParentId != null || p.ParentId != 0)).ToList(), isSellerList)
                }).ToList();

            baseResponse.Data = parentList;
            return Ok(baseResponse);
        }

        [NonAction]
        public List<ProductListLibrary> CL(List<ProductListLibrary> dr, bool isSellerList)
        {
            List<ProductListLibrary> List = new List<ProductListLibrary>();

            foreach (var item in dr)
            {
                if (item.IsExistingProduct == false||dr.Count==1)
                {


                    ProductListLibrary productListLibrary = new ProductListLibrary();
                    productListLibrary = item;
                    productListLibrary.totalSellerCount = dr.Where(x => x.Id == item.Id).ToList().Count;
                    productListLibrary.BrandId = item.BrandId;
                    productListLibrary.BrandName = !string.IsNullOrEmpty(item.ExtraDetails) ? JObject.Parse(item.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? item.ExtraDetails : null;

                    productListLibrary.BrandName = !string.IsNullOrEmpty(item.ExtraDetails) ? JObject.Parse(item.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? item.ExtraDetails : null;

                    productListLibrary.SellerName = !string.IsNullOrEmpty(item.ExtraDetails) ? JObject.Parse(item.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(item.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null;

                    //var tempColor = helper.ApiCall(URL, EndPoints.ProductColorMapping + "?ProductId=" + item.Id, "Get", null);
                    //BaseResponse<ProductColorMapp> baseResponse = new BaseResponse<ProductColorMapp>();
                    //var ColorResponse = baseResponse.JsonParseList(tempColor);
                    //List<ProductColorMapp> ProductColorMapp = (List<ProductColorMapp>)ColorResponse.Data;
                    //if (ProductColorMapp.Count > 0)
                    //{
                    //    productListLibrary.Color = ProductColorMapp;
                    //}

                    //var tempSize = helper.ApiCall(URL, EndPoints.ProductPriceMaster + "?SellerProductId=" + item.SellerProductId, "Get", null);
                    //BaseResponse<ProductPrice> baseResponse1 = new BaseResponse<ProductPrice>();
                    //var SizeResponse = baseResponse1.JsonParseList(tempSize);
                    //List<ProductPrice> sellerProducts = (List<ProductPrice>)SizeResponse.Data;
                    //if (sellerProducts.Count > 0)
                    //{
                    //    productListLibrary.Size = sellerProducts;
                    //}
                    List.Add(productListLibrary);
                }
                else
                {
                    if (isSellerList && ( item.IsExistingProduct == true || dr.Count == 1))
                    {


                        ProductListLibrary productListLibrary = new ProductListLibrary();
                        productListLibrary = item;
                        productListLibrary.totalSellerCount = dr.Where(x => x.Id == item.Id).ToList().Count;
                        productListLibrary.BrandId = item.BrandId;
                        productListLibrary.BrandName = !string.IsNullOrEmpty(item.ExtraDetails) ? JObject.Parse(item.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? item.ExtraDetails : null;

                        productListLibrary.BrandName = !string.IsNullOrEmpty(item.ExtraDetails) ? JObject.Parse(item.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? item.ExtraDetails : null;

                        productListLibrary.SellerName = !string.IsNullOrEmpty(item.ExtraDetails) ? JObject.Parse(item.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(item.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null;

                        //var tempColor = helper.ApiCall(URL, EndPoints.ProductColorMapping + "?ProductId=" + item.Id, "Get", null);
                        //BaseResponse<ProductColorMapp> baseResponse = new BaseResponse<ProductColorMapp>();
                        //var ColorResponse = baseResponse.JsonParseList(tempColor);
                        //List<ProductColorMapp> ProductColorMapp = (List<ProductColorMapp>)ColorResponse.Data;
                        //if (ProductColorMapp.Count > 0)
                        //{
                        //    productListLibrary.Color = ProductColorMapp;
                        //}

                        //var tempSize = helper.ApiCall(URL, EndPoints.ProductPriceMaster + "?SellerProductId=" + item.SellerProductId, "Get", null);
                        //BaseResponse<ProductPrice> baseResponse1 = new BaseResponse<ProductPrice>();
                        //var SizeResponse = baseResponse1.JsonParseList(tempSize);
                        //List<ProductPrice> sellerProducts = (List<ProductPrice>)SizeResponse.Data;
                        //if (sellerProducts.Count > 0)
                        //{
                        //    productListLibrary.Size = sellerProducts;
                        //}
                        List.Add(productListLibrary);
                    }
                }
            }

            return List;

        }

        [HttpGet("getProductCounts")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult GetProductCounts()
        {
            BaseResponse<ProductCounts> baseResponse = new BaseResponse<ProductCounts>();
            var response = helper.ApiCall(URL, EndPoints.GetProductCounts, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            ProductCounts productCounts = (ProductCounts)baseResponse.Data;
            return Ok(productCounts);
        }

        [HttpGet("getProductCountsWithSellerId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult GetProductCounts(string sellerId)
        {
            BaseResponse<ProductCounts> baseResponse = new BaseResponse<ProductCounts>();
            var response = helper.ApiCall(URL, EndPoints.GetProductCounts + "?sellerId=" + sellerId, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            ProductCounts productCounts = (ProductCounts)baseResponse.Data;
            return Ok(productCounts);
        }

    }
}
