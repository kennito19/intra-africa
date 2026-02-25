using API_Gateway.Common.orders;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.Order;
using API_Gateway.Models.Entity.User;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Seller
{
    [Route("api/Seller/[controller]")]
    [ApiController]
    public class SellerOrderController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string UserUrl = string.Empty;
        public string CatelogueURL = string.Empty;
        public static string IDServerUrl = string.Empty;
        BaseResponse<Orders> baseResponse = new BaseResponse<Orders>();
        private ApiHelper helper;
        public SellerOrderController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            UserUrl = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
        }

        [HttpGet("bySellerId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> BySellerId(string SellerId)
        {
            if (SellerId != null)
            {
                var response = helper.ApiCall(URL, EndPoints.Orders + "?SellerId=" + SellerId, "GET", null);
                baseResponse = baseResponse.JsonParseList(response);
            }
            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ById(string SellerId, int id)
        {
            if (SellerId != null)
            {
                var response = helper.ApiCall(URL, EndPoints.Orders + "?SellerId=" + SellerId + "&Id=" + id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(response);
            }
            return Ok(baseResponse);
        }

        [HttpGet("byGUID")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByGUID(string SellerId, string GUID)
        {
            if (SellerId != null)
            {
                var response = helper.ApiCall(URL, EndPoints.Orders + "?SellerId=" + SellerId + "&GUID=" + GUID, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(response);
            }
            return Ok(baseResponse);
        }

        [HttpGet("byOrderNo")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByOrderNo(string SellerId, string OrderNo)
        {
            if (SellerId != null)
            {
                var response = helper.ApiCall(URL, EndPoints.Orders + "?SellerId=" + SellerId + "&OrderNo=" + OrderNo, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(response);
            }
            return Ok(baseResponse);
        }

        [HttpGet("byUserId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByUserId(string SellerId, string UserId)
        {
            if (SellerId != null)
            {
                var response = helper.ApiCall(URL, EndPoints.Orders + "?SellerId=" + SellerId + "&UserId=" + UserId, "GET", null);
                baseResponse = baseResponse.JsonParseList(response);
            }
            return Ok(baseResponse);
        }

        [HttpGet("Getwarehouse")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Getwarehouse(string sellerId, int sellerProductId, int productId, int sizeId, int quantity)
        {
            List<GetWarehouseList> PWarehouselst = new List<GetWarehouseList>();
            List<Warehouse> Warehouselst = new List<Warehouse>();
            BaseResponse<Warehouse> baseResponse = new BaseResponse<Warehouse>();
            BaseResponse<GetWarehouseList> GetWarehousebaseResponse = new BaseResponse<GetWarehouseList>();

            BaseResponse<SellerListModel> SellerbaseResponse = new BaseResponse<SellerListModel>();
            var getresponse = helper.ApiCall(IDServerUrl, EndPoints.SellerById + "?ID=" + sellerId, "GET", null);
            SellerbaseResponse = SellerbaseResponse.JsonParseRecord(getresponse);
            if (SellerbaseResponse.code == 200)
            {
                SellerListModel seller = SellerbaseResponse.Data as SellerListModel;
                if (seller != null)
                {
                    if (seller.Status.ToLower() == "active")
                    {
                        var response = helper.ApiCall(UserUrl, EndPoints.Warehouse + "?UserID=" + sellerId + "&Status=" + "Active" + "&PageIndex=0&PageSize=0", "GET", null);
                        baseResponse = baseResponse.JsonParseList(response);
                        if (baseResponse.code == 200)
                        {
                            Warehouselst = baseResponse.Data as List<Warehouse> ?? new List<Warehouse>();
                        }
                        bool AllowWarehouseManagement = Convert.ToBoolean(_configuration.GetValue<string>("Allow_warehouse_management"));
                        if (AllowWarehouseManagement)
                        {

                            List<SizeWiseWarehouse> ProductWarehouselst = new List<SizeWiseWarehouse>();
                            BaseResponse<SizeWiseWarehouse> productWarehouse = new BaseResponse<SizeWiseWarehouse>();
                            var resp = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse + "/GetSizeWiseWarehouse" + "?sellerproductid=" + sellerProductId + "&productId=" + productId + "&sizeId=" + sizeId, "GET", null);
                            productWarehouse = productWarehouse.JsonParseList(resp);

                            if (productWarehouse.code == 200)
                            {
                                ProductWarehouselst = productWarehouse.Data as List<SizeWiseWarehouse> ?? new List<SizeWiseWarehouse>();
                                if (ProductWarehouselst.Count > 0)
                                {
                                    ProductWarehouselst = ProductWarehouselst.Where(p => p.Quantity >= quantity).ToList();
                                }
                            }

                            PWarehouselst = (from pw in ProductWarehouselst
                                             join
                                       w in Warehouselst on pw.WarehouseId equals w.Id
                                             select new GetWarehouseList
                                             {
                                                 ProductWarehouseId = pw.ProductWarehouseId != null ? Convert.ToInt32(pw.ProductWarehouseId) : 0,
                                                 WarehouseId = pw.WarehouseId != null ? Convert.ToInt32(pw.WarehouseId) : 0,
                                                 WarehouseName = w.Name != null ? Convert.ToString(w.Name) : null,
                                                 ContactPersonName = w.ContactPersonName != null ? Convert.ToString(w.ContactPersonName) : null,
                                                 ContactPersonMobileNo = w.ContactPersonMobileNo != null ? Convert.ToString(w.ContactPersonMobileNo) : null,
                                                 AddressLine1 = w.AddressLine1 != null ? Convert.ToString(w.AddressLine1) : null,
                                                 AddressLine2 = w.AddressLine2 != null ? Convert.ToString(w.AddressLine2) : null,
                                                 Landmark = w.Landmark != null ? Convert.ToString(w.Landmark) : null,
                                                 Pincode = w.Pincode != null ? Convert.ToString(w.Pincode) : null,
                                                 CityName = w.CityName != null ? Convert.ToString(w.CityName) : null,
                                                 StateName = w.StateName != null ? Convert.ToString(w.StateName) : null,
                                                 CountryName = w.CountryName != null ? Convert.ToString(w.CountryName) : null,
                                             }).ToList();
                        }
                        else
                        {
                            PWarehouselst = Warehouselst.Select(pw => new GetWarehouseList
                            {
                                ProductWarehouseId = 0,
                                WarehouseId = pw.Id != null ? Convert.ToInt32(pw.Id) : 0,
                                WarehouseName = pw.Name != null ? Convert.ToString(pw.Name) : null,
                                ContactPersonName = pw.ContactPersonName != null ? Convert.ToString(pw.ContactPersonName) : null,
                                ContactPersonMobileNo = pw.ContactPersonMobileNo != null ? Convert.ToString(pw.ContactPersonMobileNo) : null,
                                AddressLine1 = pw.AddressLine1 != null ? Convert.ToString(pw.AddressLine1) : null,
                                AddressLine2 = pw.AddressLine2 != null ? Convert.ToString(pw.AddressLine2) : null,
                                Landmark = pw.Landmark != null ? Convert.ToString(pw.Landmark) : null,
                                Pincode = pw.Pincode != null ? Convert.ToString(pw.Pincode) : null,
                                CityName = pw.CityName != null ? Convert.ToString(pw.CityName) : null,
                                StateName = pw.StateName != null ? Convert.ToString(pw.StateName) : null,
                                CountryName = pw.CountryName != null ? Convert.ToString(pw.CountryName) : null,
                            }).ToList();
                        }

                        if (PWarehouselst.Count > 0)
                        {
                            GetWarehousebaseResponse.code = 200;
                            GetWarehousebaseResponse.Message = "Record bind successfully";
                            GetWarehousebaseResponse.Data = PWarehouselst;
                            GetWarehousebaseResponse.pagination = null;
                        }
                        else
                        {
                            GetWarehousebaseResponse = GetWarehousebaseResponse.NotExist();
                        }
                    }
                }
                else
                {
                    GetWarehousebaseResponse = GetWarehousebaseResponse.NotExist();
                }
            }
            else
            {
                GetWarehousebaseResponse = GetWarehousebaseResponse.NotExist();
            }

            return Ok(GetWarehousebaseResponse);
        }

        [HttpGet("bysearchText")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> BysearchText(int? pageIndex = 1, int? pageSize = 10, string SellerId = null, string? Status = null, string? Searchtext = null)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            //var res = getorder.Get(true, pageIndex, pageSize, 0, null, null, null, null, null, searchText);
            //return Ok(res);
            var res = getorder.Get(pageIndex, pageSize, SellerId, Status, Searchtext);
            return Ok(res);
        }
        [HttpGet("getOrderItemDetails")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> getOrderItemDetails(int orderId, string SellerId = null)
        {
            BaseResponse<OrderItems> baseresponseorderItems = new BaseResponse<OrderItems>();
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.BindOrderItems(orderId, SellerId);
            if (res.Count > 0)
            {
                baseresponseorderItems.code = 200;
                baseresponseorderItems.Data = res;
                baseresponseorderItems.Message = "Record bind successfully.";
                baseresponseorderItems.pagination = null;
            }
            else
            {
                baseresponseorderItems = baseresponseorderItems.NotExist();
            }
            return Ok(baseresponseorderItems);
        }
        [HttpGet("ItemByOrderItemId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> getOrderItemDetailsByOrderItemId(int OrderItemId, string SellerId, string? Status = null)
        {
            BaseResponse<OrderItems> baseresponseorderItems = new BaseResponse<OrderItems>();
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.getOrderItemsDetails(OrderItemId, SellerId, Status);
            if (res != null && res.Id != 0 && res.Id != null)
            {
                baseresponseorderItems.code = 200;
                baseresponseorderItems.Data = res;
                baseresponseorderItems.Message = "Record bind successfully.";
                baseresponseorderItems.pagination = null;
            }
            else
            {
                baseresponseorderItems = baseresponseorderItems.NotExist();
            }
            return Ok(baseresponseorderItems);
        }
        [HttpGet("Invoice")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> Invoice(string Packageid)
        {
            BaseResponse<InvoiceDto> baseresponseInvoice = new BaseResponse<InvoiceDto>();
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.GetInvoice(Packageid);
            if (res != null)
            {
                baseresponseInvoice.code = 200;
                baseresponseInvoice.Data = res;
                baseresponseInvoice.Message = "Record bind successfully.";
                baseresponseInvoice.pagination = null;
            }
            else
            {
                baseresponseInvoice = baseresponseInvoice.NotExist();
            }
            return Ok(baseresponseInvoice); ;
        }

        [HttpGet("ShippingLabel")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ShippingLabel(string Packageid)
        {
            BaseResponse<ShippingLabelDto> baseresponseInvoice = new BaseResponse<ShippingLabelDto>();
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.GetShippingLabel(Packageid);
            if (res.Count > 0)
            {
                baseresponseInvoice.code = 200;
                baseresponseInvoice.Data = res;
                baseresponseInvoice.Message = "Record bind successfully.";
                baseresponseInvoice.pagination = null;
            }
            else
            {
                baseresponseInvoice = baseresponseInvoice.NotExist();
            }
            return Ok(baseresponseInvoice); ;
        }

        [HttpGet("GetOrderReturn")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetOrderReturn(string sellerId, int? id = 0, int? orderID = 0, int? brandId = 0, int? orderItemID = 0, string? orderNo = null, string? returnOrderNo = null, int? actionID = 0, string? userId = null, string? status = null, string? refundStatus = null, bool? withCancel = false, bool? isdeleted = false, int pageIndex = 1, int pageSize = 10, string? searchText = null)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.GetOrderReturn(id, orderID, brandId, orderItemID, orderNo, sellerId, returnOrderNo, actionID, userId, status, refundStatus, withCancel, isdeleted, pageIndex, pageSize, searchText);
            return Ok(res);
        }

        [HttpGet("CustomerInvoice")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> CustomerInvoice(int? packageId = 0, int? OrderId = 0, string? invoiceNo = null, int PageIndex = 1, int PageSize = 10, string? searchText = null)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.GetInvoiceList(packageId, OrderId, userID, invoiceNo, PageIndex, PageSize, searchText);
            return Ok(res);
        }

        public class GetWarehouseList
        {
            public int? ProductWarehouseId { get; set; }
            public int? WarehouseId { get; set; }
            public string? WarehouseName { get; set; }
            public string? ContactPersonName { get; set; }
            public string? ContactPersonMobileNo { get; set; }
            public string? AddressLine1 { get; set; }
            public string? AddressLine2 { get; set; }
            public string? Landmark { get; set; }
            public string? Pincode { get; set; }
            public string? CityName { get; set; }
            public string? StateName { get; set; }
            public string? CountryName { get; set; }
        }

    }
}
