using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;

namespace API_Gateway.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<Wishlist> baseResponse = new BaseResponse<Wishlist>();
        private ApiHelper helper;
        public string catalougeURL = string.Empty;
        public WishlistController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            catalougeURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }


        [HttpPost]
        [Authorize(Roles = "Customer")]
        public ActionResult<ApiHelper> Save(Wishlist model)
        {
            var temp = helper.ApiCall(URL, EndPoints.Wishlist + "?UserId=" + model.UserId + "&ProductId=" + model.ProductId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<Wishlist> tmp = baseResponse.Data as List<Wishlist> ?? new List<Wishlist>();
            if (tmp.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                Wishlist wish = new Wishlist();
                wish.UserId = model.UserId;
                wish.ProductId = model.ProductId;
                wish.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                wish.CreatedAt = DateTime.Now;

                var response = helper.ApiCall(URL, EndPoints.Wishlist, "POST", wish);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                baseResponse.Message = "Added to your Wishlist";
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Customer")]
        public ActionResult<ApiHelper> Update(Wishlist model)
        {
            var temp = helper.ApiCall(URL, EndPoints.Wishlist + "?UserId=" + model.UserId + "&ProductId=" + model.ProductId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<Wishlist> tmp = baseResponse.Data as List<Wishlist> ?? new List<Wishlist>();
            if (tmp.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(URL, EndPoints.Wishlist + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(response);
                Wishlist wish = baseResponse.Data as Wishlist;

                wish.Id = model.Id;
                wish.UserId = model.UserId;
                wish.ProductId = model.ProductId;
                wish.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                wish.ModifiedAt = DateTime.Now;

                response = helper.ApiCall(URL, EndPoints.Wishlist, "PUT", wish);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                baseResponse.Message = "Added to your Wishlist";
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Customer")]
        public ActionResult<ApiHelper> Delete(string? userId = null, string? productId = null)
        {
            var temp = helper.ApiCall(URL, EndPoints.Wishlist + "?UserID=" + userId + "&ProductID=" + productId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<Wishlist> templist = baseResponse.Data as List<Wishlist> ?? new List<Wishlist>();
            if (templist.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.Wishlist + "?UserID=" + userId + "&ProductID=" + productId, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                baseResponse.Message = "Removed from your Wishlist";
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.Wishlist + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Customer")]
        public ActionResult<ApiHelper> ById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.Wishlist + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("byUserId")]
        [Authorize(Roles = "Customer")]
        public ActionResult<ApiHelper> byUserId(string userId)
        {
            
            var response = helper.ApiCall(URL, EndPoints.Wishlist + "?UserID=" + userId + "&pageIndex=0&pageSize=0", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<Wishlist> lstwish = baseResponse.Data as List<Wishlist> ?? new List<Wishlist>();

            string productIds = string.Join(",", lstwish.Select(x => x.ProductId));
            BaseResponse<UserProductList> baseResponseProduct = new BaseResponse<UserProductList>();
            var responseProduct = helper.ApiCall(catalougeURL, EndPoints.UserProductList + "?guIds=" + productIds + "&availableProduct=" + true + "&PriceSort=0&pageIndex=0&pageSize=0", "GET", null);

            baseResponseProduct = baseResponseProduct.JsonParseList(responseProduct);
            List<UserProductList> lstUserProduct = baseResponseProduct.Data as List<UserProductList> ?? new List<UserProductList>();

            List<UserProductList> ss = lstUserProduct.Where(detail => detail.flag == 'p' && detail.Status.ToLower() == "active" && detail.Live == true).ToList();

            List<Wishlist> Matchwishlist = lstwish.Where(w => ss.Any(s => s.Guid == w.ProductId)).ToList();

            List<Wishlist> wishlist = Matchwishlist.Select(x => new Wishlist
            {
                RowNumber = x.RowNumber,
                PageCount = x.PageCount,
                RecordCount = x.RecordCount,
                Id = x.Id,
                UserId = x.UserId,
                ProductId = x.ProductId,

                CreatedBy = x.CreatedBy,
                CreatedAt = x.CreatedAt,
                ModifiedBy = x.ModifiedBy,
                ModifiedAt = x.ModifiedAt,
                Products = lstUserProduct.Where(detail => detail.flag == 'p' && detail.Guid == x.ProductId)
                        .Select(y => new UserProductsDTO
                        {
                            Id = y.Id,
                            Guid = y.Guid,
                            IsMasterProduct = y.IsMasterProduct,
                            ParentId = y.ParentId,
                            CategoryId = y.CategoryId,
                            AssiCategoryId = y.AssiCategoryId,
                            ProductName = y.ProductName,
                            CustomeProductName = y.CustomeProductName,
                            CompanySKUCode = y.CompanySKUCode,
                            Image1 = y.Image1,
                            MRP = y.MRP,
                            SellingPrice = y.SellingPrice,
                            Discount = y.Discount,
                            Quantity = y.Quantity,
                            CreatedAt = y.CreatedAt,
                            ModifiedAt = y.ModifiedAt,
                            CategoryName = y.CategoryName,
                            CategoryPathIds = y.CategoryPathIds,
                            CategoryPathNames = y.CategoryPathNames,
                            SellerProductId = y.SellerProductId,
                            SellerId = y.SellerId,
                            SellerName = !string.IsNullOrEmpty(y.ExtraDetails) ? JObject.Parse(y.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(y.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                            BrandId = y.BrandId,
                            BrandName = !string.IsNullOrEmpty(y.ExtraDetails) ? JObject.Parse(y.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? y.ExtraDetails : null,
                            TotalQty = y.TotalQty,
                            Status = y.Status,
                            Live = y.Live,
                            TotalVariant = y.TotalVariant
                        }).FirstOrDefault(),
            }).ToList();

            baseResponseProduct.Data = wishlist;
            return Ok(baseResponseProduct);
            //return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Customer")]
        public ActionResult<ApiHelper> Search(string userId = null, string productId = null)
        {
            var response = helper.ApiCall(URL, EndPoints.Wishlist + "?UserID=" + userId + "&ProductID=" + productId, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
