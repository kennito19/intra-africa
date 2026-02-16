using API_Gateway.Common.orders;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Order;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cmp;
using System.Data;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/Admin/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<Orders> baseResponse = new BaseResponse<Orders>();

        public OrderController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(true, pageIndex, pageSize, 0, null, null, null, null, null, null);
            return Ok(res);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ById(int id)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(true, 0, 0, id, null, null, null, null, null, null);
            return Ok(res);
        }

        [HttpGet("byOrderId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByGUID(string orderGuid)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(true, 0, 0, 0, orderGuid, null, null, null, null, null);
            return Ok(res);
        }

        [HttpGet("byOrderNo")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByOrderNo(string orderNo)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(true, 0, 0, 0, null, orderNo, null, null, null, null);
            return Ok(res);
        }

        [HttpGet("byOrderRefNo")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> byOrderRefNo(string orderRefNo)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(true, 0, 0, 0, null, null, orderRefNo, null, null, null);
            return Ok(res);
        }

        //[HttpGet("bySellerId")]
        //[Authorize(Roles = "Admin, Seller")]
        //public ActionResult<ApiHelper> BySellerId(string sellerId, int? pageIndex = 1, int? pageSize = 10)
        //{
        //    string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
        //    GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
        //    var res = getorder.Get(true, pageIndex, pageSize, 0, null, null, sellerId, null, null, null);
        //    return Ok(res);
        //}

        [HttpGet("byUserId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByUserId(string userId, int? pageIndex = 1, int? pageSize = 10)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(true, pageIndex, pageSize, 0, null, null, null, userId, null, null);
            return Ok(res);
        }

        [HttpGet("byStatus")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByStatus(string status, int? pageIndex = 1, int? pageSize = 10)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(false, pageIndex, pageSize, 0, null, null, null, null, status, null);
            return Ok(res);
        }

        [HttpGet("bysearchText")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> BysearchText(int? pageIndex = 1, int? pageSize = 10, string? SellerId = null, string? Status = null, string? Searchtext = null)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            //var res = getorder.Get(true, pageIndex, pageSize, 0, null, null, null, null, null, searchText);
            //return Ok(res);
            var res = getorder.Get(pageIndex, pageSize, SellerId, Status, Searchtext);
            return Ok(res);
        }

        [HttpGet("getOrderList")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> getOrderList()
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get();
            return Ok(res);
        }

        [HttpGet("CancelOrderList")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> cancelOrderList(int? pageIndex = 1, int? pageSize = 10)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(false, pageIndex, pageSize, 0, null, null, null, null, "Cancelled", null);
            return Ok(res);
        }
        [HttpGet("FailedOrderList")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> failedOrderList(int? pageIndex = 1, int? pageSize = 10)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(false, pageIndex, pageSize, 0, null, null, null, null, "Failed", null);
            return Ok(res);
        }


        [HttpGet("getOrderItemDetails")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> getOrderItemDetails(int orderId, string? SellerId = null, int? OrderItemId = 0, string? Status = null)
        {
            BaseResponse<OrderItems> baseresponseorderItems = new BaseResponse<OrderItems>();
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.BindOrderItems(orderId, SellerId, OrderItemId, Status);
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
        public ActionResult<ApiHelper> getOrderItemDetailsByOrderItemId(int OrderItemId, string? SellerId = null,  string? Status = null)
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
            if (res.Count>0)
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
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> GetOrderReturn(int? id = 0, int? orderID = 0, int? brandId = 0, int? orderItemID = 0, string? orderNo = null, string? sellerId = null, string? returnOrderNo = null, int? actionID = 0, string? userId = null, string? status = null, string? refundStatus = null, bool? withCancel = false, bool? isdeleted = false, int pageIndex = 1, int pageSize = 10, string? searchText = null)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.GetOrderReturn(id, orderID, brandId, orderItemID, orderNo, sellerId, returnOrderNo, actionID, userId, status, refundStatus, withCancel, isdeleted, pageIndex, pageSize, searchText);
            return Ok(res);
        }

        [HttpGet("GetOrderRefund")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> GetOrderRefund(int? id = 0, int? orderID = 0, int? OrderCancelReturnID = 0, int? orderItemID = 0, string? TransactionID = null, string? status = null, bool? isdeleted = false, int pageIndex = 1, int pageSize = 10, string? searchText = null)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.GetOrderRefund(id, orderID, OrderCancelReturnID, orderItemID, TransactionID, status, isdeleted, pageIndex, pageSize, searchText);
            return Ok(res);
        }

        [HttpGet("CustomerInvoice")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> CustomerInvoice(int? packageId = 0, int? OrderId = 0, string? sellerId = null, string? invoiceNo = null, int PageIndex = 1, int PageSize = 10, string? searchText = null)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.GetInvoiceList(packageId, OrderId, sellerId, invoiceNo, PageIndex, PageSize, searchText);
            return Ok(res);
        }

    }
}
