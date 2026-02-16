using API_Gateway.Common.orders;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.Order;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Seller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerManageOrderController : ControllerBase
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
        public SellerManageOrderController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            userURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            catlogUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
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

        [HttpGet("GetOrder")]
        [Authorize]
        public ActionResult<ApiHelper> GetOrder(string orderGuid)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders orders = new GetOrders(_configuration, _httpContext, userID);
            return Ok(orders.Get(orderGuid));
        }

        [HttpGet("OrderStatus")]
        [Authorize]
        public ActionResult<ApiHelper> OrderStatus(int orderId)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderStatus(orderId);
            return Ok(res);

            //BaseResponse<Orders> OrderbaseResponse = new BaseResponse<Orders>();

            //var temp = helper.ApiCall(URL, EndPoints.Orders + "?Id=" + orderId, "GET", null);
            //OrderbaseResponse = OrderbaseResponse.JsonParseList(temp);
            //List<Orders> tempList = (List<Orders>)OrderbaseResponse.Data;

            //var OrderItem = helper.ApiCall(URL, EndPoints.OrderItems + "?OrderId=" + orderId, "GET", null);
            //baseResponse = baseResponse.JsonParseList(OrderItem);
            //List<OrderItems> orderItems = (List<OrderItems>)baseResponse.Data;

            //if (orderItems.Any(x => x.Status.Contains("Delivered")))
            //{
            //    tempList[0].Status = "Delivered";
            //    tempList[0].ModifiedAt = DateTime.Now;
            //    tempList[0].ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            //    var response = helper.ApiCall(URL, EndPoints.Orders, "PUT", tempList[0]);
            //    OrderbaseResponse = OrderbaseResponse.JsonParseInputResponse(response);
            //}
            //return Ok(OrderbaseResponse);
        }

        [HttpPost("OrderPackage")]
        [Authorize]
        public ActionResult<ApiHelper> SaveOrderPackage(OrderPackageDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.SaveOrderPackage(model);
            return Ok(res);

            //BaseResponse<OrderItems> ItembaseResponse = new BaseResponse<OrderItems>();
            //var ordertemp = helper.ApiCall(URL, EndPoints.OrderItems + "?OrderId=" + model.OrderID, "Get", null);
            //ItembaseResponse = ItembaseResponse.JsonParseList(ordertemp);
            //if (ItembaseResponse.code == 200)
            //{
            //    List<OrderItems> orderItemslst = (List<OrderItems>)ItembaseResponse.Data;
            //    string[] ItemIds = model.OrderItemIDs?.Trim('{', '}').Split(',');
            //    List<OrderItems> matchingItems = orderItemslst.Where(p => ItemIds.Contains(p.Id.ToString())).ToList();
            //    matchingItems = matchingItems.Where(p => p.Status.ToLower() != "confirmed").ToList();

            //    if (matchingItems.Count > 0)
            //    {
            //        baseResponse.code = 204;
            //        baseResponse.Data = null;
            //        baseResponse.Message = "Order Item Status is not matched";
            //    }
            //    else
            //    {
            //        OrderPackage orderPack = new OrderPackage();
            //        orderPack.OrderID = model.OrderID;
            //        orderPack.OrderItemIDs = model.OrderItemIDs;
            //        orderPack.PackageNo = Guid.NewGuid().ToString();
            //        orderPack.TotalItems = model.TotalItems;
            //        orderPack.NoOfPackage = model.NoOfPackage;
            //        orderPack.PackageAmount = model.PackageAmount;
            //        orderPack.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            //        orderPack.CreatedAt = DateTime.Now;

            //        var response = helper.ApiCall(URL, EndPoints.OrderPackage, "POST", orderPack);
            //        baseResponse = baseResponse.JsonParseInputResponse(response);

            //        string[] orderItemIds = orderPack.OrderItemIDs?.Trim('{', '}').Split(',');
            //        UpdateOrderStatus(orderPack.OrderID, orderItemIds, "Packed");


            //        if (ItembaseResponse.code == 200)
            //        {
            //            UpdateMainOrderStstus(orderItemslst, model.OrderID);
            //        }
            //    }
            //}


            //return Ok(baseResponse);
        }

        [HttpPost("OrderShip")]
        [Authorize]
        public ActionResult<ApiHelper> SaveOrderShipment(OrderShipmentInfoDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.SaveOrderShipment(model);
            return Ok(res);
            //BaseResponse<OrderShipmentInfo> baseResponse = new BaseResponse<OrderShipmentInfo>();

            //BaseResponse<OrderItems> ItembaseResponse = new BaseResponse<OrderItems>();
            //var ordertemp = helper.ApiCall(URL, EndPoints.OrderItems + "?OrderId=" + model.OrderID, "Get", null);
            //ItembaseResponse = ItembaseResponse.JsonParseList(ordertemp);
            //if (ItembaseResponse.code == 200)
            //{
            //    List<OrderItems> orderItemslst = (List<OrderItems>)ItembaseResponse.Data;
            //    string[] ItemIds = model.OrderItemIDs?.Trim('{', '}').Split(',');
            //    List<OrderItems> matchingItems = orderItemslst.Where(p => ItemIds.Contains(p.Id.ToString())).ToList();
            //    matchingItems = matchingItems.Where(p => p.Status.ToLower() != "packed").ToList();

            //    if (matchingItems.Count > 0)
            //    {
            //        baseResponse.code = 204;
            //        baseResponse.Data = null;
            //        baseResponse.Message = "Order Item Status is not matched";
            //    }
            //    else
            //    {

            //        string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            //        GetOrders getorders = new GetOrders(_configuration, _httpContext, userID);

            //        Warehouse warehouse = getorders.getWarehouse(model.WarehouseId);
            //        SellerKycList sellerKyc = getorders.getsellerKyc(model.SellerID);

            //        var temp = helper.ApiCall(URL, EndPoints.ShipmentInfo + "?OrderID=" + model.OrderID + "&OrderItemIDs=" + model.OrderItemIDs, "GET", null);
            //        baseResponse = baseResponse.JsonParseList(temp);
            //        List<OrderShipmentInfo> tmp = (List<OrderShipmentInfo>)baseResponse.Data;
            //        if (tmp.Any())
            //        {
            //            baseResponse = baseResponse.AlreadyExists();
            //        }
            //        else
            //        {
            //            OrderShipmentInfo orderShip = new OrderShipmentInfo();

            //            orderShip.OrderID = model.OrderID;
            //            orderShip.OrderItemIDs = model.OrderItemIDs;
            //            orderShip.SellerID = model.SellerID;
            //            orderShip.PackageID = model.PackageID;
            //            orderShip.PaymentMode = model.PaymentMode;
            //            orderShip.Length = model.Length;
            //            orderShip.Width = model.Width;
            //            orderShip.Height = model.Height;
            //            orderShip.Weight = model.Weight;
            //            orderShip.InvoiceAmount = model.InvoiceAmount;
            //            orderShip.PackageDescription = model.PackageDescription;
            //            orderShip.IsShipmentInitiate = model.IsShipmentInitiate;
            //            orderShip.IsPaymentSuccess = model.IsPaymentSuccess;
            //            orderShip.CourierID = model.CourierID;
            //            orderShip.ServiceID = model.ServiceID;
            //            orderShip.ServiceType = model.ServiceType;
            //            orderShip.PickupContactPersonName = warehouse.ContactPersonName;
            //            orderShip.PickupContactPersonMobileNo = warehouse.ContactPersonMobileNo;
            //            orderShip.PickupContactPersonEmailID = sellerKyc.EmailID;
            //            orderShip.PickupCompanyName = sellerKyc.TradeName != null ? sellerKyc.TradeName : sellerKyc.DisplayName;
            //            orderShip.PickupAddressLine1 = warehouse.AddressLine1;
            //            orderShip.PickupAddressLine2 = warehouse.AddressLine2;
            //            orderShip.PickupLandmark = warehouse.Landmark;
            //            orderShip.PickupPincode = Convert.ToInt32(warehouse.Pincode);
            //            orderShip.PickupCity = warehouse.CityName;
            //            orderShip.PickupState = warehouse.StateName;
            //            orderShip.PickupCountry = warehouse.CountryName;
            //            orderShip.DropContactPersonName = model.DropContactPersonName;
            //            orderShip.DropContactPersonMobileNo = model.DropContactPersonMobileNo;
            //            orderShip.DropContactPersonEmailID = model.DropContactPersonEmailID;
            //            orderShip.DropCompanyName = model.DropCompanyName;
            //            orderShip.DropAddressLine1 = model.DropAddressLine1;
            //            orderShip.DropAddressLine2 = model.DropAddressLine2;
            //            orderShip.DropLandmark = model.DropLandmark;
            //            orderShip.DropPincode = model.DropPincode;
            //            orderShip.DropCity = model.DropCity;
            //            orderShip.DropState = model.DropState;
            //            orderShip.DropCountry = model.DropCountry;
            //            orderShip.ShipmentID = model.ShipmentID;
            //            orderShip.ShipmentOrderID = model.ShipmentOrderID;
            //            orderShip.ShippingPartner = model.ShippingPartner != null ? model.ShippingPartner : sellerKyc.ShipmentBy;
            //            orderShip.CourierName = model.CourierName;
            //            orderShip.ShippingAmountFromPartner = model.ShippingAmountFromPartner;
            //            orderShip.AwbNo = model.AwbNo;
            //            orderShip.IsShipmentSheduledByAdmin = model.IsShipmentSheduledByAdmin;
            //            orderShip.PickupLocationID = model.PickupLocationID;
            //            orderShip.ErrorMessage = model.ErrorMessage;
            //            orderShip.ForwardLable = model.ForwardLable;
            //            orderShip.ReturnLable = model.ReturnLable;
            //            orderShip.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            //            orderShip.CreatedAt = DateTime.Now;

            //            var response = helper.ApiCall(URL, EndPoints.ShipmentInfo, "POST", orderShip);
            //            baseResponse = baseResponse.JsonParseInputResponse(response);

            //            string[] orderItemIds = orderShip.OrderItemIDs?.Trim('{', '}').Split(',');
            //            UpdateOrderStatus(orderShip.OrderID, orderItemIds, "Shipped");

            //            //BaseResponse<OrderItems> ItembaseResponse = new BaseResponse<OrderItems>();
            //            //var ordertemp = helper.ApiCall(URL, EndPoints.OrderItems + "?OrderId=" + model.OrderID, "Get", null);
            //            //ItembaseResponse = ItembaseResponse.JsonParseList(ordertemp);
            //            if (ItembaseResponse.code == 200)
            //            {
            //                //List<OrderItems> orderItemslst = (List<OrderItems>)ItembaseResponse.Data;
            //                UpdateMainOrderStstus(orderItemslst, model.OrderID);
            //            }

            //            SaveInvoice(orderShip);

            //        }
            //    }
            //}

            //return Ok(baseResponse);
        }

        [HttpPut("OrderConfirm")]
        [Authorize]
        public ActionResult<ApiHelper> OrderConfirm(OrderConfirmDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderConfirm(model);
            return Ok(res);
            //var temp = helper.ApiCall(URL, EndPoints.OrderItems + "?OrderId=" + model.OrderId, "Get", null);
            //baseResponse = baseResponse.JsonParseList(temp);
            //if (baseResponse.code == 200)
            //{
            //    List<OrderItems> orderItemslst = (List<OrderItems>)baseResponse.Data;
            //    OrderItems orderRecord = orderItemslst.Where(p => p.Id == model.OrderItemId).FirstOrDefault();

            //    if (orderRecord.Status.ToLower() != "placed") {

            //        baseResponse.code = 204;
            //        baseResponse.Data = null;
            //        baseResponse.Message = "Order Item Status is not matched";
            //    }
            //    else
            //    {

            //        if (orderRecord.Id != null && orderRecord.Id != 0)
            //        {
            //            bool AllowWarehouseManagement = Convert.ToBoolean(_configuration.GetValue<string>("Allow_warehouse_management"));
            //            bool Orderconfirmation = false;
            //            ProductWareHouse _productWarehouse = new ProductWareHouse();
            //            if (AllowWarehouseManagement)
            //            {
            //                BaseResponse<ProductWareHouse> productWarehouse = new BaseResponse<ProductWareHouse>();
            //                var resp = helper.ApiCall(catlogUrl, EndPoints.ProductWarehouse + "?id=" + model.productWarehouseId, "GET", null);
            //                productWarehouse = productWarehouse.JsonParseRecord(resp);
            //                if (productWarehouse.code == 200)
            //                {
            //                    _productWarehouse = (ProductWareHouse)productWarehouse.Data;
            //                    if (_productWarehouse.Id != null || _productWarehouse.Id != 0)
            //                    {
            //                        if (_productWarehouse.ProductQuantity >= orderRecord.Qty)
            //                        {
            //                            int orderqty = Convert.ToInt32(_productWarehouse.ProductQuantity) - Convert.ToInt32(orderRecord.Qty);
            //                            _productWarehouse.ProductQuantity = orderqty;
            //                            var productWarehouseresponse = helper.ApiCall(catlogUrl, EndPoints.ProductWarehouse, "PUT", _productWarehouse);
            //                            productWarehouse = productWarehouse.JsonParseInputResponse(productWarehouseresponse);
            //                            Orderconfirmation = true;
            //                        }
            //                        else
            //                        {
            //                            Orderconfirmation = false;
            //                            baseResponse.code = 204;
            //                            baseResponse.Message = "Product warehouse stock is not available.";
            //                            baseResponse.Data = null;
            //                            baseResponse.pagination = null;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Orderconfirmation = false;
            //                        baseResponse.code = 204;
            //                        baseResponse.Message = "Product warehouse not available.";
            //                        baseResponse.Data = null;
            //                        baseResponse.pagination = null;
            //                    }

            //                }
            //                else
            //                {
            //                    Orderconfirmation = false;
            //                    baseResponse.code = 204;
            //                    baseResponse.Message = "Product warehouse not available.";
            //                    baseResponse.Data = null;
            //                    baseResponse.pagination = null;
            //                }

            //            }
            //            else
            //            {
            //                Orderconfirmation = true;
            //            }

            //            if (Orderconfirmation)
            //            {
            //                //OrderItems record = (OrderItems)baseResponse.Data;
            //                OrderItems record = orderRecord;
            //                record.Status = "Confirmed";
            //                record.WherehouseId = model.warehouseId;
            //                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            //                record.ModifiedAt = DateTime.Now;

            //                var response = helper.ApiCall(URL, EndPoints.OrderItems, "PUT", record);
            //                baseResponse = baseResponse.JsonParseInputResponse(response);

            //                OrderTrackEntries(model.OrderId, model.OrderItemId, record.Status);
            //                UpdateMainOrderStstus(orderItemslst, model.OrderId);
            //            }
            //        }
            //        else
            //        {
            //            baseResponse = baseResponse.NotExist();
            //        }
            //    }
            //}

            //return Ok(baseResponse);
        }

        [HttpPut("OrderDelivered")]
        [Authorize]
        public ActionResult<ApiHelper> OrderDelivered(OrderStausDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            string[] orderItemIds = model.OrderItemIds?.Trim('{', '}').Split(',');
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration );
            var res = saveOrder.UpdateOrderStatus(model.OrderId, orderItemIds, "Delivered");
            //UpdateOrderStatus(model.OrderId, orderItemIds, "Delivered");

            baseResponse.code = 200;
            baseResponse.Message = "Order delivered successfully";
            baseResponse.Data = 0;

            return Ok(baseResponse);
        }

        [HttpPost("orderRefund")]
        [Authorize]
        public ActionResult<ApiHelper> OrderRefund(OrderRefund model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderRefund(model);
            return Ok(res);
            //BaseResponse<OrderRefund> orderRefundresponse = new BaseResponse<OrderRefund>();

            //var temp = helper.ApiCall(URL, EndPoints.OrderRefund + "?OrderId=" + model.OrderID + "&OrderItemId=" + model.OrderItemID + "&OrderCancelReturnID=" + model.OrderCancelReturnID, "Get", null);
            //orderRefundresponse = orderRefundresponse.JsonParseRecord(temp);
            //List<OrderRefund> orderRefundRecord = (List<OrderRefund>)orderRefundresponse.Data;
            //if (orderRefundRecord.Any())
            //{
            //    orderRefundresponse = orderRefundresponse.AlreadyExists();
            //}
            //else
            //{
            //    OrderRefund refund = new OrderRefund();
            //    refund.OrderID = model.OrderID;
            //    refund.OrderItemID = model.OrderItemID;
            //    refund.OrderCancelReturnID = model.OrderCancelReturnID;
            //    refund.RefundAmount = model.RefundAmount;
            //    refund.TransactionID = model.TransactionID;
            //    refund.Comment = model.Comment;
            //    refund.Status = model.Status;
            //    refund.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            //    refund.CreatedAt = DateTime.Now;

            //    var response = helper.ApiCall(URL, EndPoints.OrderRefund, "POST", refund);
            //    orderRefundresponse = orderRefundresponse.JsonParseInputResponse(response);
            //}
            //return Ok(orderRefundresponse);
        }

        [HttpPost("OrderReturn")]
        [Authorize]
        public ActionResult<ApiHelper> OrderReturn(OrderReturnDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderReturn(model);
            return Ok(res);

            //BaseResponse<OrderCancelReturn> BaseResponse = new BaseResponse<OrderCancelReturn>();

            //var temp = helper.ApiCall(URL, EndPoints.CancelReturn + "?OrderID=" + model.OrderID + "&OrderItemID=" + model.OrderItemID + "&ActionID=" + model.ActionID + "&UserId=" + model.UserId, "GET", null);
            //BaseResponse = BaseResponse.JsonParseRecord(temp);
            //List<OrderCancelReturn> orderRecord = (List<OrderCancelReturn>)BaseResponse.Data;
            //if (orderRecord.Any())
            //{
            //    BaseResponse = BaseResponse.AlreadyExists();
            //}
            //else
            //{
            //    OrderCancelReturn orderCancelReturn = new OrderCancelReturn();

            //    orderCancelReturn.OrderID = model.OrderID;
            //    orderCancelReturn.OrderItemID = model.OrderItemID;
            //    orderCancelReturn.NewOrderNo = model.NewOrderNo;
            //    orderCancelReturn.Qty = model.Qty;
            //    orderCancelReturn.ActionID = model.ActionID;
            //    orderCancelReturn.ExchangeProductID = null;
            //    orderCancelReturn.ExchangeSize = null;
            //    orderCancelReturn.ExchangePriceDiff = null;
            //    orderCancelReturn.UserId = model.UserId;
            //    orderCancelReturn.UserName = model.UserName;
            //    orderCancelReturn.UserPhoneNo = model.UserPhoneNo;
            //    orderCancelReturn.UserEmail = model.UserEmail;
            //    orderCancelReturn.UserGSTNo = model.UserGSTNo;
            //    orderCancelReturn.AddressLine1 = model.AddressLine1;
            //    orderCancelReturn.AddressLine2 = model.AddressLine2;
            //    orderCancelReturn.Landmark = model.Landmark;
            //    orderCancelReturn.Pincode = model.Pincode;
            //    orderCancelReturn.City = model.City;
            //    orderCancelReturn.State = model.State;
            //    orderCancelReturn.Country = model.Country;
            //    orderCancelReturn.Issue = model.Issue;
            //    orderCancelReturn.Reason = model.Reason;
            //    orderCancelReturn.Comment = model.Comment;
            //    orderCancelReturn.PaymentMode = model.PaymentMode;
            //    orderCancelReturn.Attachment = model.Attachment;
            //    orderCancelReturn.RefundAmount = model.RefundAmount;
            //    orderCancelReturn.RefundType = model.RefundType;
            //    orderCancelReturn.BankName = model.BankName;
            //    orderCancelReturn.BankBranch = model.BankBranch;
            //    orderCancelReturn.BankIFSCCode = model.BankIFSCCode;
            //    orderCancelReturn.BankAccountNo = model.BankAccountNo;
            //    orderCancelReturn.AccountType = model.AccountType;
            //    orderCancelReturn.AccountHolderName = model.AccountHolderName;
            //    orderCancelReturn.ApprovedByID = null;
            //    orderCancelReturn.ApprovedByName = null;
            //    orderCancelReturn.Status = "Return Requested";
            //    orderCancelReturn.RefundStatus = "Pending";
            //    orderCancelReturn.CreatedAt = DateTime.Now;
            //    orderCancelReturn.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

            //    var response = helper.ApiCall(URL, EndPoints.CancelReturn, "POST", orderCancelReturn);
            //    BaseResponse = BaseResponse.JsonParseInputResponse(response);
            //    OrderTrackEntries(model.OrderID, model.OrderItemID, "Returned");

            //    BaseResponse<OrderItems> ItembaseResponse = new BaseResponse<OrderItems>();
            //    var temp1 = helper.ApiCall(URL, EndPoints.OrderItems + "?OrderId=" + model.OrderID, "Get", null);
            //    ItembaseResponse = ItembaseResponse.JsonParseList(temp1);
            //    if (baseResponse.code == 200)
            //    {
            //        List<OrderItems> orderItemslst = (List<OrderItems>)ItembaseResponse.Data;

            //        UpdateMainOrderStstus(orderItemslst, model.OrderID);
            //    }
            //}
            //return Ok(BaseResponse);
        }

        [HttpPost("OrderReturnRequest")]
        [Authorize]
        public ActionResult<ApiHelper> OrderReturnRequest(OrderReturnReuqestdto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.OrderReturnRequest(model);
            return Ok(res);

            //BaseResponse<OrderCancelReturn> BaseResponse = new BaseResponse<OrderCancelReturn>();

            //var temp = helper.ApiCall(URL, EndPoints.CancelReturn + "?Id=" + model.ReturnRequestId, "GET", null);
            //BaseResponse = BaseResponse.JsonParseRecord(temp);
            //if (baseResponse.code == 200)
            //{
            //    OrderCancelReturn orderRecord = (OrderCancelReturn)BaseResponse.Data;

            //    string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            //    GetOrders getorders = new GetOrders(_configuration, _httpContext, userID);
            //    SellerKycList sellerKyc = getorders.getsellerKyc(model.SellerID);

            //    //OrderCancelReturn orderCancelReturn = new OrderCancelReturn();

            //    //orderCancelReturn.OrderID = orderRecord.OrderID;
            //    //orderCancelReturn.OrderItemID = orderRecord.OrderItemID;
            //    //orderCancelReturn.NewOrderNo = orderRecord.NewOrderNo;
            //    //orderCancelReturn.Qty = orderRecord.Qty;
            //    //orderCancelReturn.ActionID = orderRecord.ActionID;
            //    //orderCancelReturn.ExchangeProductID = orderRecord.ExchangeProductID;
            //    //orderCancelReturn.ExchangeSize = orderRecord.ExchangeSize;
            //    //orderCancelReturn.ExchangePriceDiff = orderRecord.ExchangePriceDiff;
            //    //orderCancelReturn.UserId = orderRecord.UserId;
            //    //orderCancelReturn.UserName = orderRecord.UserName;
            //    //orderCancelReturn.UserPhoneNo = orderRecord.UserPhoneNo;
            //    //orderCancelReturn.UserEmail = orderRecord.UserEmail;
            //    //orderCancelReturn.UserGSTNo = orderRecord.UserGSTNo;
            //    //orderCancelReturn.AddressLine1 = orderRecord.AddressLine1;
            //    //orderCancelReturn.AddressLine2 = orderRecord.AddressLine2;
            //    //orderCancelReturn.Landmark = orderRecord.Landmark;
            //    //orderCancelReturn.Pincode = orderRecord.Pincode;
            //    //orderCancelReturn.City = orderRecord.City;
            //    //orderCancelReturn.State = orderRecord.State;
            //    //orderCancelReturn.Country = orderRecord.Country;
            //    //orderCancelReturn.Issue = orderRecord.Issue;
            //    //orderCancelReturn.Reason = orderRecord.Reason;
            //    //orderCancelReturn.Comment = orderRecord.Comment;
            //    //orderCancelReturn.PaymentMode = orderRecord.PaymentMode;
            //    //orderCancelReturn.Attachment = orderRecord.Attachment;
            //    //orderCancelReturn.RefundAmount = orderRecord.RefundAmount;
            //    //orderCancelReturn.RefundType = orderRecord.RefundType;
            //    //orderCancelReturn.BankName = orderRecord.BankName;
            //    //orderCancelReturn.BankBranch = orderRecord.BankBranch;
            //    //orderCancelReturn.BankIFSCCode = orderRecord.BankIFSCCode;
            //    //orderCancelReturn.BankAccountNo = orderRecord.BankAccountNo;
            //    //orderCancelReturn.AccountType = orderRecord.AccountType;
            //    //orderCancelReturn.AccountHolderName = orderRecord.AccountHolderName;
            //    orderRecord.ApprovedByID = model.ApprovedByID;
            //    orderRecord.ApprovedByName = model.ApprovedByName;
            //    orderRecord.Status = model.Status;
            //    orderRecord.RefundStatus = model.RefundStatus;
            //    orderRecord.ModifiedAt = DateTime.Now;
            //    orderRecord.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

            //    // here we are calling the pickup address.
            //    orderRecord.DropContactPersonName = model.DropContactPersonName;
            //    orderRecord.DropContactPersonMobileNo = model.DropContactPersonMobileNo;
            //    orderRecord.DropContactPersonEmailID = model.DropContactPersonEmailID;
            //    orderRecord.DropCompanyName = model.DropCompanyName;
            //    orderRecord.DropAddressLine1 = model.DropAddressLine1;
            //    orderRecord.DropAddressLine2 = model.DropAddressLine2;
            //    orderRecord.DropLandmark = model.DropLandmark;
            //    orderRecord.DropPincode = model.DropPincode;
            //    orderRecord.DropCity = model.DropCity;
            //    orderRecord.DropState = model.DropState;
            //    orderRecord.DropCountry = model.DropCountry;
            //    orderRecord.CustomeProductName = model.CustomeProductName;
            //    orderRecord.ShipmentID = model.ShipmentID;
            //    orderRecord.ShipmentOrderID = model.ShipmentOrderID;
            //    orderRecord.ShippingPartner = model.ShippingPartner != null ? model.ShippingPartner : sellerKyc.ShipmentBy;
            //    orderRecord.CourierName = model.CourierName;
            //    orderRecord.ShippingAmountFromPartner = model.ShippingAmountFromPartner;
            //    orderRecord.AwbNo = model.AwbNo;
            //    orderRecord.IsShipmentSheduledByAdmin = model.IsShipmentSheduledByAdmin;
            //    orderRecord.PickupLocationID = model.PickupLocationID;
            //    orderRecord.ErrorMessage = model.ErrorMessage;
            //    orderRecord.ForwardLable = model.ForwardLable;
            //    orderRecord.ReturnLable = model.ReturnLable;
            //    orderRecord.SellerID = model.SellerID;
            //    var response = helper.ApiCall(URL, EndPoints.CancelReturn, "PUT", orderRecord);
            //    BaseResponse = BaseResponse.JsonParseInputResponse(response);

            //    var orderCancelReturnId = BaseResponse.Data;
            //    if (orderRecord.ActionID == 1 && model.Status.ToLower() == "approved")
            //    {
            //        ReturnShipmentInfo(orderRecord, Convert.ToInt32(orderRecord.Id));
            //    }

            //}
            //else
            //{
            //    baseResponse = baseResponse.NotExist();
            //}
            //return Ok(BaseResponse);
        }

        [HttpPost("ReturnShipmentInfo")]
        public ActionResult<ApiHelper> ReturnShipmentInfoAPI(OrderCancelReturndto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.ReturnShipmentInfoAPI(model);
            return Ok(res);
            //BaseResponse<ReturnShipmentInfo> returnShipMentResponse = new BaseResponse<ReturnShipmentInfo>();
            //BaseResponse<SellerProduct> sellerProductResponse = new BaseResponse<SellerProduct>();
            //BaseResponse<OrderItems> orderItemResponse = new BaseResponse<OrderItems>();
            //BaseResponse<OrderShipmentInfo> orderShipResponse = new BaseResponse<OrderShipmentInfo>();


            //int SellerProductID = 0;
            //int? ProductID = 0;

            //// api call for getting SellerProductId and ProductId.
            //var orderdetail = helper.ApiCall(URL, EndPoints.OrderItems + "?orderId=" + model.OrderID + "&OrderItemID=" + model.OrderItemID, "GET", null);
            //orderItemResponse = orderItemResponse.JsonParseRecord(orderdetail);
            //OrderItems orderRecord = (OrderItems)orderItemResponse.Data;
            //SellerProductID = orderRecord.SellerProductID;
            //ProductID = orderRecord.ProductID;

            //// api call for getting sellerProduct Detail.
            //var sellerproduct = helper.ApiCall(catlogUrl, EndPoints.SellerProduct + "?Id=" + SellerProductID + "&ProductID=" + ProductID, "GET", null);
            //sellerProductResponse = sellerProductResponse.JsonParseRecord(sellerproduct);
            //SellerProduct sellerProduct = (SellerProduct)sellerProductResponse.Data;

            //var orderShip = helper.ApiCall(URL, EndPoints.ShipmentInfo + "?OrderID=" + model.OrderID + "&OrderItemID=" + model.OrderItemID, "GET", null);
            //orderShipResponse = orderShipResponse.JsonParseRecord(orderShip);
            //OrderShipmentInfo orderShipment = (OrderShipmentInfo)orderShipResponse.Data;


            //var temp = helper.ApiCall(URL, EndPoints.ReturnShipmentInfo + "?OrderId=" + model.OrderID + "&OrderItemId=" + model.OrderItemID + "&OrderCancelReturnID=" + model.Id, "GET", null);
            //returnShipMentResponse = returnShipMentResponse.JsonParseRecord(temp);
            //List<ReturnShipmentInfo> orderRefundRecord = (List<ReturnShipmentInfo>)returnShipMentResponse.Data;
            //if (orderRefundRecord.Any())
            //{
            //    returnShipMentResponse = returnShipMentResponse.AlreadyExists();
            //}
            //else
            //{
            //    ReturnShipmentInfo returnShipment = new ReturnShipmentInfo();
            //    returnShipment.OrderID = model.OrderID;
            //    returnShipment.OrderItemID = model.OrderItemID;
            //    returnShipment.OrderCancelReturnID = model.orderCancelReturnId;

            //    returnShipment.Qty = model.Qty;
            //    returnShipment.PaymentMode = "Online";
            //    returnShipment.Length = Convert.ToString(sellerProduct.PackingLength);
            //    returnShipment.Width = Convert.ToString(sellerProduct.PackingBreadth);
            //    returnShipment.Height = Convert.ToString(sellerProduct.PackingHeight);
            //    returnShipment.Weight = Convert.ToString(sellerProduct.PackingWeight);
            //    returnShipment.ReturnValueAmount = orderRecord.TotalAmount;
            //    returnShipment.PackageDescription = model.CustomeProductName;
            //    returnShipment.IsShipmentInitiate = false;
            //    returnShipment.IsPaymentSuccess = false;
            //    returnShipment.CourierID = null;
            //    returnShipment.ServiceID = null;
            //    returnShipment.ServiceType = null;
            //    returnShipment.PickupContactPersonName = model.PickupContactPersonName;
            //    returnShipment.PickupContactPersonMobileNo = model.PickupContactPersonMobileNo;
            //    returnShipment.PickupContactPersonEmailID = model.PickupContactPersonEmailID;
            //    returnShipment.PickupCompanyName = model.PickupCompanyName;
            //    returnShipment.PickupAddressLine1 = model.PickupAddressLine1;
            //    returnShipment.PickupAddressLine2 = model.PickupAddressLine2;
            //    returnShipment.PickupLandmark = model.PickupLandmark;
            //    returnShipment.PickupPincode = (int)model.PickupPincode;
            //    returnShipment.PickupCity = model.PickupCity;
            //    returnShipment.PickupState = model.PickupState;
            //    returnShipment.PickupCountry = model.PickupCountry;
            //    returnShipment.DropContactPersonName = orderShipment.PickupContactPersonName;
            //    returnShipment.DropContactPersonMobileNo = orderShipment.PickupContactPersonMobileNo;
            //    returnShipment.DropContactPersonEmailID = orderShipment.PickupContactPersonEmailID;
            //    returnShipment.DropCompanyName = orderShipment.PickupCompanyName;
            //    returnShipment.DropAddressLine1 = orderShipment.PickupAddressLine1;
            //    returnShipment.DropAddressLine2 = orderShipment.PickupAddressLine2;
            //    returnShipment.DropLandmark = orderShipment.PickupLandmark;
            //    returnShipment.DropPincode = orderShipment.PickupPincode;
            //    returnShipment.DropCity = orderShipment.PickupCity;
            //    returnShipment.DropState = orderShipment.PickupState;
            //    returnShipment.DropCountry = orderShipment.PickupCountry;
            //    // passing null here due to 3rd paty integration.
            //    returnShipment.ShipmentID = null;
            //    returnShipment.ShipmentOrderID = null;
            //    returnShipment.ShippingPartner = null;
            //    returnShipment.CourierName = null;
            //    returnShipment.ShippingAmountFromPartner = null;
            //    returnShipment.AwbNo = null;
            //    returnShipment.IsShipmentSheduledByAdmin = false;
            //    returnShipment.PickupLocationID = null;
            //    returnShipment.ErrorMessage = null;
            //    returnShipment.ForwardLable = null;
            //    returnShipment.ReturnLable = null;
            //    returnShipment.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            //    returnShipment.CreatedAt = DateTime.Now;

            //    var response = helper.ApiCall(URL, EndPoints.ReturnShipmentInfo, "POST", returnShipment);
            //    returnShipMentResponse = returnShipMentResponse.JsonParseInputResponse(response);
            //}
            //return Ok(returnShipMentResponse);
        }

        [HttpPost("ReExchangeTaxInfo")]
        [Authorize]
        public ActionResult<ApiHelper> ReplaceAndExchangeTaxInfo(OrderTaxInfo model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.ReplaceAndExchangeTaxInfo(model);
            return Ok(res);
            //BaseResponse<OrderTaxInfo> modelResponse = new BaseResponse<OrderTaxInfo>();

            //var getTaxInfo = helper.ApiCall(URL, EndPoints.OrderTaxInfo + "?OrderId=" + model.OrderID + "&OrderItemId=" + model.OrderItemID, "GET", null);
            //modelResponse = modelResponse.JsonParseRecord(getTaxInfo);
            //List<OrderTaxInfo> orderTax = (List<OrderTaxInfo>)modelResponse.Data;
            //if (orderTax.Any())
            //{
            //    modelResponse = modelResponse.AlreadyExists();
            //}
            //else
            //{
            //    OrderTaxInfo tax = new OrderTaxInfo();

            //    tax.OrderID = model.OrderID;
            //    tax.OrderItemID = model.OrderItemID;
            //    tax.ProductID = model.ProductID;
            //    tax.SellerProductID = model.SellerProductID;
            //    tax.ShippingCharge = model.ShippingCharge;
            //    tax.ShippingZone = model.ShippingZone;
            //    tax.TaxOnShipping = model.TaxOnShipping;
            //    tax.CommissionIn = model.CommissionIn;
            //    tax.CommissionRate = model.CommissionRate;
            //    tax.CommissionAmount = model.CommissionAmount;
            //    tax.TaxOnCommission = model.TaxOnCommission;
            //    tax.NetEarn = model.NetEarn;
            //    tax.OrderTaxRateId = model.OrderTaxRateId;
            //    tax.OrderTaxRate = model.OrderTaxRate;
            //    tax.HSNCode = model.HSNCode;
            //    tax.ShipmentBy = model.ShipmentBy;
            //    tax.ShipmentPaidBy = model.ShipmentPaidBy;
            //    tax.IsSellerWithGSTAtOrderTime = model.IsSellerWithGSTAtOrderTime;
            //    tax.WeightSlab = model.WeightSlab;

            //    tax.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            //    tax.CreatedAt = DateTime.Now;

            //    var callTaxInfo = helper.ApiCall(URL, EndPoints.OrderTaxInfo, "POST", tax);
            //    modelResponse = modelResponse.JsonParseInputResponse(callTaxInfo);
            //}
            //return Ok(modelResponse);


        }

        [HttpPut("exchangeOrderRequest")]
        [Authorize]
        public ActionResult<ApiHelper> ExchangeOrderRequest(OrderCancelReturn model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.ExchangeOrderRequest(model);
            return Ok(res);
            //BaseResponse<OrderCancelReturn> exchange = new BaseResponse<OrderCancelReturn>();

            //var temp = helper.ApiCall(URL, EndPoints.CancelReturn + "?OrderID=" + model.OrderID + "&OrderItemID=" + model.OrderItemID + "&ActionID=" + model.ActionID + "&UserId=" + model.UserId, "GET", null);
            //exchange = exchange.JsonParseRecord(temp);
            //List<OrderCancelReturn> exchange1 = (List<OrderCancelReturn>)exchange.Data;
            //if (exchange1.Any())
            //{
            //    exchange = exchange.NotExist();
            //}
            //else
            //{
            //    OrderCancelReturn orderCancel = new OrderCancelReturn();
            //    orderCancel.OrderID = model.OrderID;
            //    orderCancel.OrderItemID = model.OrderItemID;
            //    orderCancel.NewOrderNo = model.NewOrderNo;
            //    orderCancel.Qty = model.Qty;
            //    orderCancel.ActionID = model.ActionID;
            //    orderCancel.ExchangeProductID = model.ExchangeProductID;
            //    orderCancel.ExchangeSize = model.ExchangeSize;
            //    orderCancel.ExchangePriceDiff = model.ExchangePriceDiff;
            //    orderCancel.UserId = model.UserId;
            //    orderCancel.UserName = model.UserName;
            //    orderCancel.UserPhoneNo = model.UserPhoneNo;
            //    orderCancel.UserEmail = model.UserEmail;
            //    orderCancel.UserGSTNo = model.UserGSTNo;
            //    orderCancel.AddressLine1 = model.AddressLine1;
            //    orderCancel.AddressLine2 = model.AddressLine2;
            //    orderCancel.Landmark = model.Landmark;
            //    orderCancel.Pincode = model.Pincode;
            //    orderCancel.City = model.City;
            //    orderCancel.State = model.State;
            //    orderCancel.Country = model.Country;
            //    orderCancel.Issue = model.Issue;
            //    orderCancel.Reason = model.Reason;
            //    orderCancel.Comment = model.Comment;
            //    orderCancel.PaymentMode = model.PaymentMode;
            //    orderCancel.Attachment = model.Attachment;
            //    orderCancel.RefundAmount = model.RefundAmount;
            //    orderCancel.RefundType = model.RefundType;
            //    orderCancel.BankName = model.BankName;
            //    orderCancel.BankBranch = model.BankBranch;
            //    orderCancel.BankIFSCCode = model.BankIFSCCode;
            //    orderCancel.BankAccountNo = model.BankAccountNo;
            //    orderCancel.AccountType = model.AccountType;
            //    orderCancel.AccountHolderName = model.AccountHolderName;
            //    orderCancel.ApprovedByID = model.ApprovedByID;
            //    orderCancel.ApprovedByName = model.ApprovedByName;
            //    orderCancel.Status = model.Status;
            //    orderCancel.RefundStatus = model.RefundStatus;
            //    orderCancel.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            //    orderCancel.CreatedAt = DateTime.Now;

            //    var updateRecord = helper.ApiCall(URL, EndPoints.CancelReturn, "POST", orderCancel);
            //    exchange = exchange.JsonParseInputResponse(updateRecord);

            //    exchangeOrder(orderCancel.OrderID, orderCancel.OrderItemID);

            //}
            //return Ok(exchange);
        }

        [HttpPut("approvedRequest")]
        public ActionResult<ApiHelper> ApprovedStatus(ApprovedStatusDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveOrder saveOrder = new SaveOrder(_httpContext, userID, URL, catlogUrl, _configuration);
            var res = saveOrder.ApprovedStatus(model);
            return Ok(res);
            //BaseResponse<OrderCancelReturn> status = new BaseResponse<OrderCancelReturn>();

            //var uStatus = helper.ApiCall(URL, EndPoints.CancelReturn + "?orderId=" + model.orderId + "&orderItemId=" + model.orderItemId + "&actionId=" + model.actionId, "GET", null);
            //status = status.JsonParseRecord(uStatus);
            //OrderCancelReturn updateStatus = (OrderCancelReturn)status.Data;
            //if (updateStatus == null)
            //{
            //    status = status.NotExist();
            //}
            //else
            //{
            //    updateStatus.Status = model.approvedRequest;
            //    updateStatus.ExchangeProductID = model.exchangeProductId;
            //    updateStatus.ExchangeSize = model.exchangeSize;
            //    updateStatus.ExchangePriceDiff = model.exchangePriceDiff;

            //    var updateRecord = helper.ApiCall(URL, EndPoints.CancelReturn, "PUT", updateStatus);
            //    status = status.JsonParseInputResponse(updateRecord);

            //    switch (model.name)
            //    {
            //        case "exchange":
            //            exchangeOrder(model.orderId, model.orderItemId, model.exchangeProductId, model.exchangeSize, model.exchangePriceDiff);
            //            break;
            //        case "replace":
            //            replaceOrder(model.orderId, model.orderItemId);
            //            break;
            //        default:
            //            return null;
            //    }
            //}
            //return Ok(status);
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
            var getOrderDetails = helper.ApiCall(URL, EndPoints.OrderTrackDetails + "?Id=" + Id + "&OrderID=" + OrderID + "&OrderItemID" + OrderItemID + "&Isdeleted" + Isdeleted + "&PageIndex" + PageIndex + "&PageSize" + PageSize, "GET", null);
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

            //string[] orderItemIds = model.OrderItemIds?.Trim('{', '}').Split(',');
            //UpdateOrderStatus(model.OrderId, orderItemIds, "Cancelled");

            //baseResponse.code = 200;
            //baseResponse.Message = "Order cancelled successfully";
            //baseResponse.Data = 0;

            //return Ok(baseResponse);
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
