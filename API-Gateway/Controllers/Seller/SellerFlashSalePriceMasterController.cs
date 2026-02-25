using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Seller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerFlashSalePriceMasterController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<FlashSalePriceMasterLibrary> baseResponse = new BaseResponse<FlashSalePriceMasterLibrary>();
        private ApiHelper helper;
        public SellerFlashSalePriceMasterController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }


        [HttpPost]
        [Authorize(Roles = "Seller")]
        public ActionResult<ApiHelper> Create(FlashSalePriceMasterDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?CollectionId=" + model.CollectionId + "&CollectionMappingId=" + model.CollectionMappingId + "&sellerProductId=" + model.SellerProductId + "&SellerWiseProductPriceMasterId=" + model.SellerWiseProductPriceMasterId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<FlashSalePriceMasterLibrary> tempList = baseResponse.Data as List<FlashSalePriceMasterLibrary> ?? new List<FlashSalePriceMasterLibrary>();

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
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
        [Authorize(Roles = "Seller")]
        public ActionResult<ApiHelper> Update(FlashSalePriceMasterDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?CollectionId=" + model.CollectionId + "&CollectionMappingId=" + model.CollectionMappingId + "&sellerProductId=" + model.SellerProductId + "&SellerWiseProductPriceMasterId=" + model.SellerWiseProductPriceMasterId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<FlashSalePriceMasterLibrary> tempList = baseResponse.Data as List<FlashSalePriceMasterLibrary> ?? new List<FlashSalePriceMasterLibrary>();

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                FlashSalePriceMasterLibrary record = baseResponse.Data as FlashSalePriceMasterLibrary;

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
        [Authorize(Roles = "Seller")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<FlashSalePriceMasterLibrary> tempList = baseResponse.Data as List<FlashSalePriceMasterLibrary> ?? new List<FlashSalePriceMasterLibrary>();
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

        [HttpGet("search")]
        [Authorize(Roles = "Seller")]
        public ActionResult<ApiHelper> Search(string? searchText, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?Searchtext=" + HttpUtility.UrlEncode(searchText) + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }


    }
}
