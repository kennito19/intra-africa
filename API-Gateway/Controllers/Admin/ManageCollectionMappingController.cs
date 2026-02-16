using API_Gateway.Helper;
using API_Gateway.Models.Dto;
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
    public class ManageCollectionMappingController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ManageCollectionMappingLibrary> baseResponse = new BaseResponse<ManageCollectionMappingLibrary>();
        private ApiHelper helper;
        public ManageCollectionMappingController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(List<ManageCollectionMappingDTO> model)
        {
            for (int i = 0; i < model.Count; i++)
            {
                var temp = helper.ApiCall(URL, EndPoints.ManageCollectionMapping + "?CollectionId=" + model[i].CollectionId + "&ProductId=" + model[i].ProductId, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<ManageCollectionMappingLibrary> tempList = (List<ManageCollectionMappingLibrary>)baseResponse.Data;

                if (tempList.Any())
                {
                    baseResponse = baseResponse.AlreadyExists();
                }
                else
                {
                    var temp1 = helper.ApiCall(URL, EndPoints.ManageCollectionMapping + "?CollectionId=" + model[i].CollectionId + "&ProductId=" + model[i].ProductId + "&IsDeleted=true", "GET", null);
                    baseResponse = baseResponse.JsonParseList(temp1);
                    List<ManageCollectionMappingLibrary> tempList1 = (List<ManageCollectionMappingLibrary>)baseResponse.Data;

                    if (tempList1.Any())
                    {
                        ManageCollectionMappingLibrary manageCollectionMapping = new ManageCollectionMappingLibrary();
                        manageCollectionMapping.Id = tempList1.FirstOrDefault().Id;
                        manageCollectionMapping.CollectionId = model[i].CollectionId;
                        manageCollectionMapping.ProductId = model[i].ProductId;
                        manageCollectionMapping.Status = model[i].Status;
                        manageCollectionMapping.ModifiedAt = DateTime.Now;
                        manageCollectionMapping.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                        manageCollectionMapping.DeletedAt = null;
                        manageCollectionMapping.DeletedBy = null;
                        manageCollectionMapping.IsDeleted = false;
                        var response = helper.ApiCall(URL, EndPoints.ManageCollectionMapping, "PUT", manageCollectionMapping);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                    else
                    {

                        ManageCollectionMappingLibrary manageCollectionMapping = new ManageCollectionMappingLibrary();
                        manageCollectionMapping.CollectionId = model[i].CollectionId;
                        manageCollectionMapping.ProductId = model[i].ProductId;
                        manageCollectionMapping.Status = model[i].Status;
                        manageCollectionMapping.CreatedAt = DateTime.Now;
                        manageCollectionMapping.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                        var response = helper.ApiCall(URL, EndPoints.ManageCollectionMapping, "POST", manageCollectionMapping);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                }
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ManageCollectionMappingDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageCollectionMapping + "?CollectionId=" + model.CollectionId + "?ProductId=" + model.ProductId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageCollectionMappingLibrary> tempList = (List<ManageCollectionMappingLibrary>)baseResponse.Data;

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageCollectionMapping + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageCollectionMappingLibrary record = (ManageCollectionMappingLibrary)baseResponse.Data;
                record.CollectionId = model.CollectionId;
                record.ProductId = model.ProductId;
                record.Status = model.Status;
                record.ModifiedAt = DateTime.Now;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                record.DeletedAt = null;
                record.DeletedBy = null;
                record.IsDeleted = false;
                var response = helper.ApiCall(URL, EndPoints.ManageCollectionMapping, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageCollectionMapping + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageCollectionMappingLibrary> tempList = (List<ManageCollectionMappingLibrary>)baseResponse.Data;
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ManageCollectionMapping + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize]
        public ActionResult<ApiHelper> GetById(int id)
        {
            BaseResponse<ManageCollectionMappingLibrary> baseResponse1 = new BaseResponse<ManageCollectionMappingLibrary>();
            var response = helper.ApiCall(URL, EndPoints.ManageCollectionMapping + "?Id=" + id, "GET", null);
            baseResponse1 = baseResponse1.JsonParseRecord(response);
            if (baseResponse1.code == 200)
            {
                var dataresponse = (ManageCollectionMappingLibrary)baseResponse1.Data;
                dataresponse.BrandName = !string.IsNullOrEmpty(dataresponse.ExtraDetails) ? JObject.Parse(dataresponse.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? dataresponse.ExtraDetails : null;
                baseResponse.code = baseResponse1.code;
                baseResponse.Message = baseResponse1.Message;
                baseResponse.pagination = baseResponse1.pagination;
                baseResponse.Data = dataresponse;
                //baseResponse.Data = dataresponse.(details => new ManageCollectionMappingLibrary
                //{
                //    Id = details.Id,
                //    CollectionId = details.CollectionId,
                //    ProductId = details.ProductId,
                //    ParentId = details.ParentId,
                //    CategoryId = (int)details.CategoryId,
                //    AssiCategoryId = (int)details.AssiCategoryId,
                //    SellerId = details.SellerId,
                //    BrandId = (int)details.BrandId,
                //    SellerProductId = (int)details.SellerProductId,
                //    ProductName = details.ProductName,
                //    CustomeProductName = details.CustomeProductName,
                //    CompanySKUCode = details.CompanySKUCode,
                //    MRP = (decimal)details.MRP,
                //    SellingPrice = (decimal)details.SellingPrice,
                //    Discount = (decimal)details.Discount,
                //    Quantity = (int)details.Quantity,
                //    TotalQty = (int)details.TotalQty,
                //    SaleMRP = (decimal)details.SaleMRP,
                //    SaleSellingPrice = (decimal)details.SaleSellingPrice,
                //    SaleDiscount = (decimal)details.SaleDiscount,
                //    CategoryName = details.CategoryName,
                //    CategoryPathIds = details.CategoryPathIds,
                //    CategoryPathNames = details.CategoryPathNames,
                //    CollectionName = details.CollectionName,
                //    BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? details.ExtraDetails : null,
                //    Status = details.Status,
                //    ProductStatus = details.ProductStatus,
                //    ProductLive = (bool)details.ProductLive,
                //    SaleStatus = details.SaleStatus,
                //    IsSellerOptIn = (bool)details.IsSellerOptIn,
                //    TotalVariant = (int)details.TotalVariant,
                //    Image1 = details.Image1,
                //}).ToList();
            }


            return Ok(baseResponse);
        }

        [HttpGet("byCollectionId")]
        [Authorize]
        public ActionResult<ApiHelper> GetByCollectionId(int collectionId, int? pageindex = 1, int? pageSize = 10)
        {
            BaseResponse<ManageCollectionMappingLibrary> baseResponse1 = new BaseResponse<ManageCollectionMappingLibrary>();
            var response = helper.ApiCall(URL, EndPoints.ManageCollectionMapping + "?CollectionId=" + collectionId + "&PageIndex=" + pageindex + "&PageSize=" + pageSize, "GET", null);
            baseResponse1 = baseResponse1.JsonParseList(response);
            if (baseResponse1.Data != null)
            {
                var dataresponse = (List<ManageCollectionMappingLibrary>)baseResponse1.Data;
                baseResponse.code = baseResponse1.code;
                baseResponse.Message = baseResponse1.Message;
                baseResponse.pagination = baseResponse1.pagination;
                baseResponse.Data = dataresponse.Select(details => new ManageCollectionMappingLibrary
                {
                    Id = details.Id,
                    CollectionId = details.CollectionId,
                    ProductId = details.ProductId,
                    ParentId = details.ParentId,
                    CategoryId = details.CategoryId,
                    AssiCategoryId = details.AssiCategoryId,
                    SellerId = details.SellerId,
                    BrandId = details.BrandId,
                    SellerProductId = details.SellerProductId,
                    ProductName = details.ProductName,
                    CustomeProductName = details.CustomeProductName,
                    CompanySKUCode = details.CompanySKUCode,
                    MRP = details.MRP,
                    SellingPrice = details.SellingPrice,
                    Discount = details.Discount,
                    Quantity = details.Quantity,
                    TotalQty = details.TotalQty,
                    SaleMRP = !string.IsNullOrEmpty(details.SaleMRP.ToString()) ? details.SaleMRP : null,
                    SaleSellingPrice = !string.IsNullOrEmpty(details.SaleSellingPrice.ToString()) ? details.SaleSellingPrice : null,
                    SaleDiscount = !string.IsNullOrEmpty(details.SaleDiscount.ToString()) ? details.SaleDiscount : null,
                    CategoryName = details.CategoryName,
                    CategoryPathIds = details.CategoryPathIds,
                    CategoryPathNames = details.CategoryPathNames,
                    CollectionName = details.CollectionName,
                    BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? details.ExtraDetails : null,
                    Status = details.Status,
                    ProductStatus = details.ProductStatus,
                    ProductLive = details.ProductLive,
                    SaleStatus = details.SaleStatus,
                    IsSellerOptIn = !string.IsNullOrEmpty(details.IsSellerOptIn.ToString()) ? details.IsSellerOptIn : null,
                    TotalVariant = !string.IsNullOrEmpty(details.TotalVariant.ToString()) ? details.TotalVariant : null,
                    Image1 = details.Image1,
                }).ToList();
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }


            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize]
        public ActionResult<ApiHelper> Search(string? searchText, int? pageIndex = 1, int? pageSize = 10)
        {
            BaseResponse<ManageCollectionMappingLibrary> baseResponse1 = new BaseResponse<ManageCollectionMappingLibrary>();
            var response = helper.ApiCall(URL, EndPoints.ManageCollectionMapping + "?Searchtext=" + HttpUtility.UrlEncode(searchText) + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            baseResponse1 = baseResponse1.JsonParseList(response);
            if (baseResponse1.code == 200)
            {
                var dataresponse = (List<ManageCollectionMappingLibrary>)baseResponse1.Data;
                baseResponse.code = baseResponse1.code;
                baseResponse.Message = baseResponse1.Message;
                baseResponse.pagination = baseResponse1.pagination;
                baseResponse.Data = dataresponse.Select(details => new ManageCollectionMappingLibrary
                {
                    Id = details.Id,
                    CollectionId = details.CollectionId,
                    ProductId = details.ProductId,
                    ParentId = details.ParentId,
                    CategoryId = details.CategoryId,
                    AssiCategoryId = details.AssiCategoryId,
                    SellerId = details.SellerId,
                    BrandId = details.BrandId,
                    SellerProductId = details.SellerProductId,
                    ProductName = details.ProductName,
                    CustomeProductName = details.CustomeProductName,
                    CompanySKUCode = details.CompanySKUCode,
                    MRP = details.MRP,
                    SellingPrice = details.SellingPrice,
                    Discount = details.Discount,
                    Quantity = details.Quantity,
                    TotalQty = details.TotalQty,
                    SaleMRP = !string.IsNullOrEmpty(details.SaleMRP.ToString()) ? details.SaleMRP : null,
                    SaleSellingPrice = !string.IsNullOrEmpty(details.SaleSellingPrice.ToString()) ? details.SaleSellingPrice : null,
                    SaleDiscount = !string.IsNullOrEmpty(details.SaleDiscount.ToString()) ? details.SaleDiscount : null,
                    CategoryName = details.CategoryName,
                    CategoryPathIds = details.CategoryPathIds,
                    CategoryPathNames = details.CategoryPathNames,
                    CollectionName = details.CollectionName,
                    BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? details.ExtraDetails : null,
                    Status = details.Status,
                    ProductStatus = details.ProductStatus,
                    ProductLive = details.ProductLive,
                    SaleStatus = details.SaleStatus,
                    IsSellerOptIn = !string.IsNullOrEmpty(details.IsSellerOptIn.ToString()) ? details.IsSellerOptIn : null,
                    TotalVariant = !string.IsNullOrEmpty(details.TotalVariant.ToString()) ? details.TotalVariant : null,
                    Image1 = details.Image1,
                }).ToList();
            }


            return Ok(baseResponse);
        }

        [HttpGet("ProductCheck")]
        [Authorize]
        public ActionResult<ApiHelper> ProductCheck(int ProductId, DateTime StartDate)
        {
            BaseResponse<bool> checkResponse = new BaseResponse<bool>() { code = 200, Message = "Product not Active is any FlashSale", Data = false };
            BaseResponse<ManageCollectionLibrary> CollectionResponse = new BaseResponse<ManageCollectionLibrary>();

            var response = helper.ApiCall(URL, EndPoints.ManageCollectionMapping + "?ProductId=" + ProductId, "GET", null);
            var collectionMappings = baseResponse.JsonParseList(response).Data as List<ManageCollectionMappingLibrary>;

            foreach (var i in collectionMappings)
            {
                var response2 = helper.ApiCall(URL, EndPoints.ManageCollection + "?Id=" + i.CollectionId, "GET", null);
                var collections = CollectionResponse.JsonParseList(response2).Data as List<ManageCollectionLibrary>;

                if (collections.Where(x => x.EndDate > StartDate).Any())
                {
                    checkResponse.code = 200;
                    checkResponse.Message = "Product Currently Active in Another Flash Sale";
                    checkResponse.Data = true;

                    return Ok(checkResponse);
                }

            }

            return Ok(checkResponse);

        }
    }
}
