using API_Gateway.Common.orders;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.Order;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API_Gateway.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageOrderController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string userURL = string.Empty;
        public string URL = string.Empty;
        public string catlogUrl = string.Empty;
        public string IDServerUrl = string.Empty;
        public string UserUrl = string.Empty;
        BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
        private ApiHelper helper;
        public ManageOrderController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;

            _configuration = configuration;
            userURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            catlogUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost("SaveOrder")]
        [Authorize]
        public ActionResult<ApiHelper> SaveOrder(List<OrderDetails> model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.SaveData(model);
            return Ok(res);
        }

        [HttpPut("VerifyOrder")]
        [Authorize]
        public ActionResult<ApiHelper> VerifyOrder(int? OrderId = 0, string? orderRefNo = null)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.VerifyOrderStatus(OrderId, orderRefNo);
            return Ok(res);
        }

        [HttpGet("GetOrder")]
        [Authorize]
        public ActionResult<ApiHelper> GetOrder(string orderNo)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders orders = new GetOrders(_configuration, _httpContext, userID);
            var res = orders.Get(orderNo);
            return Ok(res);
            //return Ok(orders.Get(orderGuid));
        }

        [HttpGet("GetOrderByRefNo")]
        [Authorize]
        public ActionResult<ApiHelper> GetorderByRefNo(string orderRefNo)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders orders = new GetOrders(_configuration, _httpContext, userID);
            var res = orders.GetorderByRefNo(orderRefNo);
            return Ok(res);
            //return Ok(orders.Get(orderGuid));
        }

        [HttpGet("OrderStatus")]
        [Authorize]
        public ActionResult<ApiHelper> OrderStatus(int orderId)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderStatus(orderId);
            return Ok(res);

        }

        [HttpPost("OrderPackage")]
        [Authorize]
        public ActionResult<ApiHelper> SaveOrderPackage(OrderPackageDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.SaveOrderPackage(model);
            return Ok(res);

        }

        [HttpPost("OrderShip")]
        [Authorize]
        public ActionResult<ApiHelper> SaveOrderShipment(OrderShipmentInfoDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.SaveOrderShipment(model);
            return Ok(res);
            
        }

        [HttpPut("OrderConfirm")]
        [Authorize]
        public ActionResult<ApiHelper> OrderConfirm(OrderConfirmDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderConfirm(model);
            return Ok(res);
            
        }

        [HttpPut("OrderDelivered")]
        [Authorize]
        public ActionResult<ApiHelper> OrderDelivered(OrderStausDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            string[] orderItemIds = model.OrderItemIds?.Trim('{', '}').Split(',');
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.UpdateOrderStatus(model.OrderId, orderItemIds, "Delivered");
            //saveOrder.Sendmail(model.OrderId, "Delivered", HttpContext.User.Claims.Where(x => x.Type.Equals("role")).FirstOrDefault().Value);
            saveOrder.Sendmail(model.OrderId, "Delivered", HttpContext.User.Claims.Where(x => x.Type.Equals(System.Security.Claims.ClaimTypes.Role)).FirstOrDefault().Value);
            baseResponse.code = 200;
            baseResponse.Message = "Order delivered successfully";
            baseResponse.Data = 0;

            return Ok(baseResponse);
        }

        [HttpPut("orderRefund")]
        [Authorize]
        public ActionResult<ApiHelper> OrderRefund(OrderRefund model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderRefund(model);
            return Ok(res);
            
        }

        [HttpPost("OrderReturn")]
        [Authorize]
        public ActionResult<ApiHelper> OrderReturn(OrderReturnDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderReturn(model);
            return Ok(res);

        }
        
        [HttpPost("OrderReturnRequest")]
        [Authorize]
        public ActionResult<ApiHelper> OrderReturnRequest(OrderReturnReuqestdto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderReturnRequest(model);
            return Ok(res);

        }

        [HttpPost("CancelOrderReturnRequest")]
        [Authorize]
        public ActionResult<ApiHelper> CancelReturnRequest(CancelReturnReuqestdto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.CancelReturnRequest(model);
            return Ok(res);

        }

        [HttpPost("OrderReplace")]
        [Authorize]
        public ActionResult<ApiHelper> OrderReplace(OrderReplaceDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderReplace(model);
            return Ok(res);

        }

        [HttpPost("OrderReplaceRequest")]
        [Authorize]
        public ActionResult<ApiHelper> OrderReplaceRequest(OrderReplaceReuqestdto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderReplaceRequest(model);
            return Ok(res);

        }

        [HttpPost("CancelReplaceRequest")]
        [Authorize]
        public ActionResult<ApiHelper> CancelReplaceRequest(CancelReplaceReuqestdto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.CancelReplaceRequest(model);
            return Ok(res);

        }

        [HttpPost("OrderExchange")]
        [Authorize]
        public ActionResult<ApiHelper> OrderExchange(OrderExchangeDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderExchange(model);
            return Ok(res);

        }

        [HttpPost("OrderExchangeRequest")]
        [Authorize]
        public ActionResult<ApiHelper> OrderExchangeRequest(OrderExchangeReuqestdto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderExchangeRequest(model);
            return Ok(res);

        }

        [HttpPost("CancelExchangeReuqest")]
        [Authorize]
        public ActionResult<ApiHelper> CancelExchangeReuqest(CancelExchangeReuqestdto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.CancelExchangeReuqest(model);
            return Ok(res);

        }

        [HttpPost("ReturnShipmentInfo")]
        public ActionResult<ApiHelper> ReturnShipmentInfoAPI(OrderCancelReturndto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.ReturnShipmentInfoAPI(model);
            return Ok(res);
            
        }

        [HttpPost("ReExchangeTaxInfo")]
        [Authorize]
        public ActionResult<ApiHelper> ReplaceAndExchangeTaxInfo(OrderTaxInfo model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.ReplaceAndExchangeTaxInfo(model);
            return Ok(res);
            
        }

        [HttpPut("exchangeOrderRequest")]
        [Authorize]
        public ActionResult<ApiHelper> ExchangeOrderRequest(OrderCancelReturn model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.ExchangeOrderRequest(model);
            return Ok(res);
            
        }

        [HttpPut("approvedRequest")]
        public ActionResult<ApiHelper> ApprovedStatus(ApprovedStatusDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.ApprovedStatus(model);
            return Ok(res);
            
        }

        [HttpGet("GetShippingLabel")]
        public ActionResult<ApiHelper> GetShippingLabel(int orderId)
        {
            BaseResponse<Orders> orderResponse = new BaseResponse<Orders>();
            BaseResponse<OrderShipmentInfo> orderShipResponse = new BaseResponse<OrderShipmentInfo>();
            BaseResponse<OrderPackage> orderPackResponse = new BaseResponse<OrderPackage>();

            var getOrderDetails = helper.ApiCall(URL, EndPoints.Orders + "?Id=" + orderId, "GET", null);
            orderResponse = orderResponse.JsonParseList(getOrderDetails);
            List<Orders> orderbind = (List<Orders>)orderResponse.Data;


            var getOrderShipDetail = helper.ApiCall(URL, EndPoints.ShipmentInfo + "?orderId=" + orderId, "GET", null);
            orderShipResponse = orderShipResponse.JsonParseList(getOrderShipDetail);
            List<OrderShipmentInfo> orderShip = (List<OrderShipmentInfo>)orderShipResponse.Data;


            var getOrderPackedData = helper.ApiCall(URL, EndPoints.OrderPackage + "?orderId=" + orderId, "GET", null);
            orderPackResponse = orderPackResponse.JsonParseList(getOrderPackedData);
            List<OrderPackage> orderPack = (List<OrderPackage>)orderPackResponse.Data;

            var response = orderbind.Select(order => new
            {
                Id = order.Id,
                orderDate = order.OrderDate,
                ToDetail = orderShip.Where(x => x.OrderID == order.Id)
                .Select(ToDetail => new
                {
                    DropCompanyName = ToDetail.DropCompanyName,
                    DropContactPersonName = ToDetail.DropContactPersonName,
                    DropAddressLine1 = ToDetail.DropAddressLine1,
                    DropAddressLine2 = ToDetail.DropAddressLine2,
                    DropLandmark = ToDetail.DropLandmark,
                    DropCity = ToDetail.DropCity,
                    DropState = ToDetail.DropState,
                    DropPincode = ToDetail.DropPincode,
                    DropPhone = ToDetail.DropContactPersonMobileNo,
                }),
                FromDetail = orderShip.Where(y => y.OrderID == order.Id)
                .Select(FromDetail => new
                {
                    PickupCompanyName = FromDetail.PickupCompanyName,
                    PickupContactPersonName = FromDetail.PickupContactPersonName,
                    FromAddressLine1 = FromDetail.PickupAddressLine1,
                    FromAddressLine2 = FromDetail.PickupAddressLine2,
                    FromLendMark = FromDetail.PickupLandmark,
                    FromCity = FromDetail.PickupCity,
                    FromState = FromDetail.PickupState,
                    FromPincode = FromDetail.PickupPincode,
                    FromPhone = FromDetail.PickupContactPersonMobileNo,
                }),
                ShipmentDetail = orderShip.Where(z => z.OrderID == order.Id)
                .Select(ShippingBy => new
                {
                    ShippingByName = ShippingBy.PickupCompanyName,
                    ShippingDate = ShippingBy.CreatedAt,
                    ShippingWeight = ShippingBy.Weight,
                    PaymentMode = ShippingBy.PaymentMode,
                    AwbNo = ShippingBy.AwbNo,
                }),
                PackageDetail = orderPack.Where(p => p.OrderID == order.Id)
                .Select(Packed => new
                {
                    NoofItems = Packed.NoOfPackage,
                    TotalItems = Packed.TotalItems,
                    TotalPack = Packed.TotalItems - Packed.NoOfPackage,
                })
            });


            baseResponse.Data = response;
            return Ok(response);
        }

        [HttpGet("OrderPackageList")]
        public ActionResult<ApiHelper> GetPackageList(int orderId, int orderItemId)
        {
            BaseResponse<OrderPackage> packageResponse = new BaseResponse<OrderPackage>();
            var temp = helper.ApiCall(URL, EndPoints.OrderPackage + "?orderId=" + orderId + "&orderItemId=" + orderItemId, "GET", null);
            packageResponse = packageResponse.JsonParseRecord(temp);
            return Ok(packageResponse);
        }


        [HttpGet("OrderTrack")]
        [Authorize]
        public ActionResult<ApiHelper> Get(int Id = 0, int OrderID = 0, int OrderItemID = 0, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10)
        {
            BaseResponse<OrderTrackDetails> response = new BaseResponse<OrderTrackDetails>();
            var getOrderDetails = helper.ApiCall(URL, EndPoints.OrderTrackDetails + "?Id=" + Id + "&OrderID=" + OrderID + "&OrderItemID=" + OrderItemID + "&Isdeleted=" + Isdeleted + "&PageIndex=" + PageIndex + "&PageSize=" + PageSize, "GET", null);
            response = response.JsonParseList(getOrderDetails);
            //List<OrderTrackDetails> orderbind = (List<OrderTrackDetails>)response.Data;

            return Ok(response);
        }

        [HttpPost("OrderCancel")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> SaveOrderCancel(OrderCancelDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.SaveOrderCancel(model);
            return Ok(res);

        }

        [HttpGet("GetReturnActions")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> GetReturnActions(int PageIndex = 1, int PageSize = 10)
        {
            BaseResponse<OrderReturnAction> getOrderReturnAction = new BaseResponse<OrderReturnAction>();

            var temp = helper.ApiCall(URL, EndPoints.OrderReturnAction + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize, "GET", null);
            getOrderReturnAction = getOrderReturnAction.JsonParseList(temp);
            return Ok(getOrderReturnAction);
        }

    }
}
