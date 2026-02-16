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
    public class FlashSalePriceMasterController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<FlashSalePriceMasterLibrary> baseResponse = new BaseResponse<FlashSalePriceMasterLibrary>();
        private ApiHelper helper;
        public FlashSalePriceMasterController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(FlashSalePriceMasterDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?CollectionId=" + model.CollectionId + "&CollectionMappingId=" + model.CollectionMappingId + "&sellerProductId=" + model.SellerProductId + "&SellerWiseProductPriceMasterId=" + model.SellerWiseProductPriceMasterId, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(temp);
            if (baseResponse.code == 200)
            {
                FlashSalePriceMasterLibrary tempList = (FlashSalePriceMasterLibrary)baseResponse.Data;

                if (tempList.Id != 0 && tempList.Id != null)
                {
                    //baseResponse = baseResponse.AlreadyExists();
                    tempList.SellerProductId = model.SellerProductId;
                    tempList.SellerWiseProductPriceMasterId = model.SellerWiseProductPriceMasterId;
                    tempList.CollectionId = model.CollectionId;
                    tempList.CollectionMappingId = model.CollectionMappingId;
                    tempList.MRP = model.MRP;
                    tempList.SellingPrice = model.SellingPrice;
                    tempList.Discount = model.Discount;
                    tempList.Status = model.Status;
                    tempList.IsSellerOptIn = model.IsSellerOptIn;
                    tempList.ModifiedAt = DateTime.Now;
                    tempList.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    tempList.IsDeleted = false;
                    tempList.DeletedBy = null;
                    tempList.DeletedAt = null;
                    var response = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster, "PUT", tempList);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
                else
                {
                    FlashSalePriceMasterLibrary flashSale = new FlashSalePriceMasterLibrary();
                    flashSale.SellerProductId = model.SellerProductId;
                    flashSale.SellerWiseProductPriceMasterId = model.SellerWiseProductPriceMasterId;
                    flashSale.CollectionId = model.CollectionId;
                    flashSale.CollectionMappingId = model.CollectionMappingId;
                    flashSale.MRP = model.MRP;
                    flashSale.SellingPrice = model.SellingPrice;
                    flashSale.Discount = model.Discount;
                    flashSale.Status = model.Status;
                    flashSale.IsSellerOptIn = model.IsSellerOptIn;
                    flashSale.CreatedAt = DateTime.Now;
                    flashSale.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    var response = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster, "POST", flashSale);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                FlashSalePriceMasterLibrary flashSale = new FlashSalePriceMasterLibrary();
                flashSale.SellerProductId = model.SellerProductId;
                flashSale.SellerWiseProductPriceMasterId = model.SellerWiseProductPriceMasterId;
                flashSale.CollectionId = model.CollectionId;
                flashSale.CollectionMappingId = model.CollectionMappingId;
                flashSale.MRP = model.MRP;
                flashSale.SellingPrice = model.SellingPrice;
                flashSale.Discount = model.Discount;
                flashSale.Status = model.Status;
                flashSale.IsSellerOptIn = model.IsSellerOptIn;
                flashSale.CreatedAt = DateTime.Now;
                flashSale.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster, "POST", flashSale);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(FlashSalePriceMasterDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?CollectionId=" + model.CollectionId + "&CollectionMappingId=" + model.CollectionMappingId + "&sellerProductId=" + model.SellerProductId + "&SellerWiseProductPriceMasterId=" + model.SellerWiseProductPriceMasterId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<FlashSalePriceMasterLibrary> tempList = (List<FlashSalePriceMasterLibrary>)baseResponse.Data;

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                FlashSalePriceMasterLibrary record = (FlashSalePriceMasterLibrary)baseResponse.Data;

                record.SellerProductId = model.SellerProductId;
                record.SellerWiseProductPriceMasterId = model.SellerWiseProductPriceMasterId;
                record.CollectionId = model.CollectionId;
                record.CollectionMappingId = model.CollectionMappingId;
                record.MRP = model.MRP;
                record.SellingPrice = model.SellingPrice;
                record.Discount = model.Discount;
                record.Status = model.Status;
                record.IsSellerOptIn = model.IsSellerOptIn;
                record.ModifiedAt = DateTime.Now;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<FlashSalePriceMasterLibrary> tempList = (List<FlashSalePriceMasterLibrary>)baseResponse.Data;
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageindex = 1, int? pageSize = 10, bool isDeleted = false)
        {
            var response = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?PageIndex=" + pageindex + "&PageSize=" + pageSize + "&IsDeleted=" + isDeleted, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("bysellerProductId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetBysellerProductId(int sellerProductId, bool isDeleted = false)
        {
            var response = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?sellerProductId=" + sellerProductId + "&IsDeleted=" + isDeleted, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byCollectionMappingId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByCollectionMappingId(int CollectionMappingId, bool isDeleted = false)
        {
            var response = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?CollectionMappingId=" + CollectionMappingId + "&IsDeleted=" + isDeleted, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchText, int? pageIndex = 1, int? pageSize = 10, bool isDeleted = false)
        {
            var response = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?Searchtext=" + HttpUtility.UrlEncode(searchText) + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&IsDeleted=" + isDeleted, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
