using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.Order;
using API_Gateway.Models.Entity.User;
using ClosedXML.Graphics;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace API_Gateway.Common.orders
{
    public class SaveOrder
    {
        public string OrderURL = string.Empty;
        public string _catlogUrl = string.Empty;
        private readonly HttpContext _httpContext;
        public string UserId = string.Empty;
        public string Token { get; set; }
        private ApiHelper helper;
        private readonly IConfiguration _configuration;
        public string UserUrl = string.Empty;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SaveOrder(HttpContext httpContext, string Userid, string URL, string catlogUrl, IConfiguration configuration)
        {
            UserId = Userid;
            _httpContext = httpContext;
            OrderURL = URL;
            _catlogUrl = catlogUrl;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            UserUrl = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            _httpContextAccessor = new HttpContextAccessor();
        }

        public Orders BindOrder(OrderDetails model)
        {
            Orders order = new Orders();
            order.OrderNo = model.OrderNo;
            order.SellerID = model.SellerID;
            order.UserId = model.UserId;
            order.UserName = model.UserName;
            order.UserPhoneNo = model.UserPhoneNo;
            order.UserEmail = model.UserEmail;
            order.UserAddressLine1 = model.UserAddressLine1;
            order.UserAddressLine2 = model.UserAddressLine2;
            order.UserLandmark = model.UserLendMark;
            order.UserPincode = model.UserPincode;
            order.UserCity = model.UserCity;
            order.UserState = model.UserState;
            order.UserCountry = model.UserCountry;
            order.UserGSTNo = model.UserGSTNo;
            order.PaymentMode = model.PaymentMode;
            order.TotalShippingCharge = model.TotalShippingCharge;
            order.TotalExtraCharges = model.TotalExtraCharges;
            order.TotalAmount = model.TotalAmount;
            order.IsCouponApplied = model.IsCouponApplied;
            order.Coupon = model.Coupon;
            order.CoupontDetails = model.CoupontDetails;
            order.CoupontDiscount = model.CoupontDiscount;
            order.CODCharge = model.CODCharge;
            order.PaidAmount = model.PaidAmount;
            order.IsSale = model.IsSale;
            order.SaleType = model.SaleType;
            order.OrderDate = DateTime.Now;
            order.DeliveryDate = DateTime.Now.AddDays(Convert.ToInt32(model.Deliverydays));
            order.Status = model.Status;
            order.PaymentInfo = model.PaymentInfo;
            order.OrderBy = model.OrderBy;
            order.IsRetailer = model.IsRetailer;
            order.IsVertualRetailer = model.IsVertualRetailer;
            order.IsReplace = model.IsReplace;
            order.ParentID = model.ParentId;
            order.OrderReferenceNo = model.OrderReferenceNo;

            order.CreatedBy = UserId;
            order.CreatedAt = DateTime.Now;

            return order;
        }

        public List<OrderItems> BindOrderItems(IEnumerable<OrderItemDTO>? orderItems, int OrderId)
        {
            List<OrderItems> oi = new List<OrderItems>();

            foreach (var item in orderItems)
            {
                OrderItems orderItem = new OrderItems();
                orderItem.OrderID = OrderId;
                orderItem.SubOrderNo = Guid.NewGuid().ToString();
                orderItem.SellerID = item.SellerID;
                orderItem.BrandID = item.BrandID;
                orderItem.CategoryId = item.CategoryId;
                orderItem.ProductID = item.ProductID;
                orderItem.ProductGUID = item.ProductGUID;
                orderItem.SellerProductID = item.SellerProductID;
                orderItem.ProductName = item.ProductName;
                orderItem.ProductSKUCode = item.ProductSKUCode;
                orderItem.MRP = item.MRP;
                orderItem.SellingPrice = item.SellingPrice;
                orderItem.Discount = item.Discount;
                orderItem.Qty = item.Qty;
                orderItem.TotalAmount = item.TotalAmount;
                orderItem.PriceTypeID = item.PriceTypeID != null ? item.PriceTypeID : 0;
                orderItem.PriceType = item.PriceType != null ? item.PriceType : null;
                orderItem.SizeID = item.SizeID != null ? item.SizeID : 0;
                orderItem.SizeValue = item.SizeValue != null ? item.SizeValue : null;
                orderItem.IsCouponApplied = item.IsCouponApplied != null ? item.IsCouponApplied : false;
                orderItem.Coupon = item.Coupon;
                orderItem.CoupontDetails = item.CoupontDetails;
                orderItem.CoupontDiscount = item.CoupontDiscount;
                orderItem.ShippingZone = item.ShippingZone;
                orderItem.ShippingCharge = item.ShippingCharge;
                orderItem.ShippingChargePaidBy = item.ShippingChargePaidBy;
                orderItem.SubTotal = item.SubTotal;
                orderItem.Status = item.Status;
                orderItem.WherehouseId = item.WherehouseId;
                orderItem.IsReplace = item.IsReplace;
                orderItem.ParentID = item.ParentID;
                orderItem.ColorName = item.Color;
                orderItem.ProductImage = item.ProductImage;
                orderItem.ExtraDetails = item.ExtraDetails;

                if (!string.IsNullOrEmpty(item.ProductImage))
                {
                    ImageUpload imgUpload = new ImageUpload(_configuration);
                    var img = imgUpload.CopyImage("ProductImage", item.ProductImage);
                }
                ReturnPolicyDTO returnPolicy = BindReturnPolicy(Convert.ToInt32(item.CategoryId));
                if (returnPolicy != null)
                {
                    orderItem.ReturnPolicyName = returnPolicy.ReturnPolicyName;
                    orderItem.ReturnPolicyTitle = returnPolicy.Title;
                    orderItem.ReturnPolicyCovers = returnPolicy.Covers;
                    orderItem.ReturnPolicyDescription = returnPolicy.Description;
                    orderItem.ReturnValidDays = returnPolicy.ValidityDays;
                }
                orderItem.ReturnValidTillDate = null;
                orderItem.CreatedBy = UserId;
                orderItem.CreatedAt = DateTime.Now;
                oi.Add(orderItem);
            }
            return oi;
        }

        public OrderTrackDetails BindOrderTrackDetails(int OrderId, int OrderItemId)
        {
            OrderTrackDetails ot = new OrderTrackDetails();

            OrderTrackDetails ordertrack = new OrderTrackDetails();
            ordertrack.OrderID = OrderId;
            ordertrack.OrderItemID = OrderItemId;
            ordertrack.OrderStage = ot.OrderStage;
            ordertrack.OrderStatus = ot.OrderStatus;
            ordertrack.OrderTrackDetail = ot.OrderTrackDetail;
            ordertrack.TrackDate = ot.TrackDate;
            ordertrack.RejectionType = ot.RejectionType;
            ordertrack.RejectionBy = ot.RejectionBy;
            ordertrack.ReasonForRejection = ot.ReasonForRejection;
            ordertrack.Comment = ot.Comment;
            ordertrack.CreatedBy = UserId;
            ordertrack.CreatedAt = DateTime.Now;
            return ordertrack;
        }

        public List<OrderTaxInfo> BindOrderTaxInfo(IEnumerable<OrderTaxInfoDTO>? orderTaxInfo, int OrderId, int OrderItemId)
        {
            List<OrderTaxInfo> ot = new List<OrderTaxInfo>();

            foreach (var item in orderTaxInfo)
            {
                OrderTaxInfo orderTax = new OrderTaxInfo();

                orderTax.OrderID = OrderId;
                orderTax.OrderItemID = OrderItemId;
                orderTax.ProductID = item.ProductID;
                orderTax.SellerProductID = item.SellerProductID;
                orderTax.NetEarn = item.NetEarn;
                orderTax.OrderTaxRateId = item.OrderTaxRateId;
                orderTax.OrderTaxRate = item.OrderTaxRate;
                orderTax.HSNCode = item.HSNCode;
                orderTax.ShipmentBy = item.ShipmentBy;
                orderTax.ShipmentPaidBy = item.ShipmentPaidBy;
                orderTax.IsSellerWithGSTAtOrderTime = item.IsSellerWithGSTAtOrderTime;
                orderTax.WeightSlab = item.WeightSlab;
                orderTax.ShippingCharge = item.ShippingCharge == null ? 0 : Convert.ToDecimal(item.ShippingCharge);
                orderTax.ShippingZone = item.ShippingZone;
                orderTax.TaxOnShipping = item.TaxOnShipping == null ? 0 : Convert.ToDecimal(item.TaxOnShipping);
                orderTax.CommissionIn = item.CommissionIn;
                orderTax.CommissionRate = item.CommissionRate;
                orderTax.CommissionAmount = item.CommissionAmount;
                orderTax.TaxOnCommission = item.TaxOnCommission == null ? 0 : Convert.ToDecimal(item.TaxOnCommission);

                orderTax.CreatedBy = UserId;
                orderTax.CreatedAt = DateTime.Now;
                ot.Add(orderTax);
            }
            return ot;
        }

        public List<OrderWiseExtraCharges> BindOrderWiseExtraCharges(IEnumerable<OrderWiseExtraChargesDTO>? orderExtraCharges, int OrderId, int OrderItemId)
        {
            List<OrderWiseExtraCharges> oec = new List<OrderWiseExtraCharges>();

            foreach (var item in orderExtraCharges)
            {
                OrderWiseExtraCharges extraCharges = new OrderWiseExtraCharges();

                extraCharges.OrderID = OrderId;
                extraCharges.OrderItemID = OrderItemId;
                extraCharges.ChargesType = item.ChargesType;
                extraCharges.ChargesPaidBy = item.ChargesPaidBy;
                extraCharges.ChargesIn = item.ChargesIn;
                extraCharges.ChargesValueInPercentage = item.ChargesValueInPercentage;
                extraCharges.ChargesValueInAmount = item.ChargesValueInAmount;
                extraCharges.ChargesMaxAmount = item.ChargesMaxAmount;
                extraCharges.TaxOnChargesAmount = item.TaxOnChargesAmount;
                extraCharges.ChargesAmountWithoutTax = item.ChargesAmountWithoutTax;
                extraCharges.TotalCharges = item.TotalCharges;
                extraCharges.CreatedBy = UserId;
                extraCharges.CreatedAt = DateTime.Now;
                oec.Add(extraCharges);
            }
            return oec;
        }

        public BaseResponse<string> SaveData(List<OrderDetails> _orderdetails)
        {
            BaseResponse<string> baseR = new BaseResponse<string>();
            string OrderRef = Guid.NewGuid().ToString();
            foreach (OrderDetails model in _orderdetails)
            {
                model.OrderReferenceNo = OrderRef;
                var orderTableEntry = OrderTableEntry(model);

                int orderId = Convert.ToInt32(orderTableEntry.Data);

                string orderno = Convert.ToString(orderTableEntry.Data);

                try
                {
                    if (orderId != null && orderId != 0)
                    {

                        var OrderItems = OrderItemsEntries(model.OrderItems, orderId);

                        var OrderItemIds = OrderItems.Select(x => Convert.ToInt32(x.Data)).ToList();

                    for (int i = 0; i < OrderItems.Count; i++)
                    {
                        var orderTaxInfos = OrderTaxInfoEntries(model.OrderItems.ElementAt(i).OrderTaxInfos, orderId, OrderItemIds.ElementAt(i));

                        var orderExtraCharges = OrderExtraChargesEntries(model.OrderItems.ElementAt(i).OrderWiseExtraCharges, orderId, OrderItemIds.ElementAt(i));

                    }


                        baseR.code = 200;
                        baseR.Message = "Order Data Inserted Successfully";
                        baseR.Data = OrderRef;

                    }
                    else
                    {
                        //BaseResponse<string> baseR = new BaseResponse<string>();
                        baseR.code = 200;
                        baseR.Message = "Error";

                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return baseR;
        }

        public BaseResponse<Orders> OrderTableEntry(OrderDetails model)
        {
            model.OrderNo = Guid.NewGuid().ToString();
            BaseResponse<Orders> baseResponse = new BaseResponse<Orders>();
            //var GetResponse = helper.ApiCall(OrderURL, EndPoints.Orders + "?OrderNo=" + model.OrderNo, "GET", null);
            //baseResponse = baseResponse.JsonParseList(GetResponse);
            //List<Orders> tempList = (List<Orders>)baseResponse.Data;
            //if (tempList.Where(x => x.OrderNo == model.OrderNo).Any())
            //{
            //    baseResponse = baseResponse.AlreadyExists();
            //    int orderId = tempList[0].Id;
            //    baseResponse.Data = orderId;
            //}
            //else
            //{
            Orders orders = BindOrder(model);

            var Response = helper.ApiCall(OrderURL, EndPoints.Orders, "POST", orders);
            baseResponse = baseResponse.JsonParseInputResponse(Response);
            //}
            return baseResponse;
        }

        public List<BaseResponse<OrderItems>> OrderItemsEntries(IEnumerable<OrderItemDTO> orderItems, int orderId)
        {
            List<BaseResponse<OrderItems>> bases = new List<BaseResponse<OrderItems>>();
            List<OrderItems> ois = BindOrderItems(orderItems, orderId);
            int count = 0;
            foreach (var oi in ois)
            {
                BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
                var response = helper.ApiCall(OrderURL, EndPoints.OrderItems, "POST", oi);

                baseResponse = baseResponse.JsonParseInputResponse(response);
                bases.Add(baseResponse);

                OrderTrackEntries(orderId, (int)bases[count].Data, "Initiate", "Order has been Initiated", "Waithing for payment conformation");
                count++;
            }
            return bases;
        }

        public List<BaseResponse<OrderTaxInfo>> OrderTaxInfoEntries(IEnumerable<OrderTaxInfoDTO> orderTaxInfo, int orderId, int OrderItemId)
        {
            List<BaseResponse<OrderTaxInfo>> orderTaxbases = new List<BaseResponse<OrderTaxInfo>>();
            List<OrderTaxInfo> ois = BindOrderTaxInfo(orderTaxInfo, orderId, OrderItemId);

            foreach (var item in ois)
            {
                BaseResponse<OrderTaxInfo> baseResponse = new BaseResponse<OrderTaxInfo>();
                var response = helper.ApiCall(OrderURL, EndPoints.OrderTaxInfo, "POST", item);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                orderTaxbases.Add(baseResponse);
            }

            return orderTaxbases;
        }

        public BaseResponse<string> VerifyOrderStatus(int? orderId, string? orderRefNo)
        {

            BaseResponse<string> baseR = new BaseResponse<string>();
            try
            {
                if (orderId != null && orderId != 0)
                {

                    BaseResponse<Orders> OrderbaseResponse = new BaseResponse<Orders>();
                    BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
                    var temp = helper.ApiCall(OrderURL, EndPoints.Orders + "?Id=" + orderId, "GET", null);
                    OrderbaseResponse = OrderbaseResponse.JsonParseRecord(temp);
                    Orders Order = new Orders();
                    if (OrderbaseResponse.code == 200)
                    {
                        Order = (Orders)OrderbaseResponse.Data;
                    }


                    var OrderItem = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + orderId + "&status=Initiate", "GET", null);
                    baseResponse = baseResponse.JsonParseList(OrderItem);
                    List<OrderItems> orderItems = new List<OrderItems>();

                    if (baseResponse.code == 200)
                    {
                        orderItems = (List<OrderItems>)baseResponse.Data;
                    }

                    if (Order.Status.ToString().ToLower() == "initiate" && orderItems.Count() != 0)
                    {
                        if (Order.PaymentMode.ToString().ToLower() == "cod")
                        {

                            Order.Status = "Placed";
                            Order.PaymentInfo = null;
                            var Orderresponse = helper.ApiCall(OrderURL, EndPoints.Orders, "PUT", Order);

                            foreach (var item in orderItems)
                            {

                                OrderItems record = item;
                                record.Status = "Placed";
                                var OrderItemresponse = helper.ApiCall(OrderURL, EndPoints.OrderItems, "PUT", record);

                                BaseResponse<ProductPrice> PbaseResponse = new BaseResponse<ProductPrice>();
                                string url = string.Empty;
                                if (item.SizeID != null)
                                {
                                    url = "&SizeID=" + item.SizeID;
                                }

                                var presponse = helper.ApiCall(_catlogUrl, EndPoints.ProductPriceMaster + "?SellerProductID=" + item.SellerProductID + url, "get", null);
                                PbaseResponse = PbaseResponse.JsonParseList(presponse);
                                List<ProductPrice> DataProductPrices = (List<ProductPrice>)PbaseResponse.Data;
                                if (DataProductPrices.Count > 0)
                                {
                                    ProductPrice PP = new ProductPrice();
                                    PP = DataProductPrices[0];
                                    PP.Quantity = PP.Quantity - Convert.ToInt32(item.Qty);
                                    var priceresponse = helper.ApiCall(_catlogUrl, EndPoints.ProductPriceMaster, "PUT", PP);
                                }

                                OrderTrackEntries(Convert.ToInt32(orderId), (int)item.Id, "Placed", "Order has been placed", "Payment approved");

                            }
                            baseR.code = 200;
                            baseR.Message = "Order Placed Successfully";


                            //Sendmail(Convert.ToInt32(orderId), "Placed", _httpContext.User.Claims.Where(x => x.Type.Equals("role")).FirstOrDefault().Value);
                            Sendmail(Convert.ToInt32(orderId), "Placed", _httpContext.User.Claims.Where(x => x.Type.Equals(System.Security.Claims.ClaimTypes.Role)).FirstOrDefault().Value);



                        }
                        else
                        {
                            baseR.code = 201;
                            baseR.Message = "Order Failed";

                        }
                    }
                    else
                    {
                        baseR.code = 200;
                        baseR.Message = "Order Placed Successfully";

                    }

                }
                else if (!string.IsNullOrEmpty(orderRefNo))
                {

                    BaseResponse<Orders> OrderbaseResponse = new BaseResponse<Orders>();
                    BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
                    var temp = helper.ApiCall(OrderURL, EndPoints.Orders + "?OrderRefNo=" + orderRefNo, "GET", null);
                    OrderbaseResponse = OrderbaseResponse.JsonParseList(temp);
                    List<Orders> Order = new List<Orders>();
                    if (OrderbaseResponse.code == 200)
                    {
                        Order = (List<Orders>)OrderbaseResponse.Data;
                    }

                    foreach (Orders _order in Order)
                    {


                        var OrderItem = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + _order.Id + "&status=Initiate", "GET", null);
                        baseResponse = baseResponse.JsonParseList(OrderItem);
                        List<OrderItems> orderItems = new List<OrderItems>();

                        if (baseResponse.code == 200)
                        {
                            orderItems = (List<OrderItems>)baseResponse.Data;
                        }

                        if (_order.Status.ToString().ToLower() == "initiate" && orderItems.Count() != 0)
                        {
                            if (_order.PaymentMode.ToString().ToLower() == "cod")
                            {

                                _order.Status = "Placed";
                                _order.PaymentInfo = null;
                                var Orderresponse = helper.ApiCall(OrderURL, EndPoints.Orders, "PUT", _order);

                                foreach (var item in orderItems)
                                {

                                    OrderItems record = item;
                                    record.Status = "Placed";
                                    var OrderItemresponse = helper.ApiCall(OrderURL, EndPoints.OrderItems, "PUT", record);

                                    BaseResponse<ProductPrice> PbaseResponse = new BaseResponse<ProductPrice>();
                                    string url = string.Empty;
                                    if (item.SizeID != null)
                                    {
                                        url = "&SizeID=" + item.SizeID;
                                    }

                                    var presponse = helper.ApiCall(_catlogUrl, EndPoints.ProductPriceMaster + "?SellerProductID=" + item.SellerProductID + url, "get", null);
                                    PbaseResponse = PbaseResponse.JsonParseList(presponse);
                                    List<ProductPrice> DataProductPrices = (List<ProductPrice>)PbaseResponse.Data;
                                    if (DataProductPrices.Count > 0)
                                    {
                                        ProductPrice PP = new ProductPrice();
                                        PP = DataProductPrices[0];
                                        PP.Quantity = PP.Quantity - Convert.ToInt32(item.Qty);
                                        var priceresponse = helper.ApiCall(_catlogUrl, EndPoints.ProductPriceMaster, "PUT", PP);
                                    }

                                    OrderTrackEntries(_order.Id, (int)item.Id, "Placed", "Order has been placed", "Payment approved");

                                }
                                baseR.code = 200;
                                baseR.Message = "Order Placed Successfully";


                                //Sendmail(_order.Id, "Placed", _httpContext.User.Claims.Where(x => x.Type.Equals("role")).FirstOrDefault().Value);
                                Sendmail(_order.Id, "Placed", _httpContext.User.Claims.Where(x => x.Type.Equals(System.Security.Claims.ClaimTypes.Role)).FirstOrDefault().Value);

                            }
                            else
                            {
                                baseR.code = 201;
                                baseR.Message = "Order Failed";

                            }
                        }
                        else
                        {
                            baseR.code = 200;
                            baseR.Message = "Order Placed Successfully";

                        }
                    }

                }
                else
                {
                    baseR.code = 201;
                    baseR.Message = "Error";

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return baseR;
        }

        public BaseResponse<Orders> OrderStatus(int orderId)
        {
            BaseResponse<Orders> OrderbaseResponse = new BaseResponse<Orders>();
            BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
            var temp = helper.ApiCall(OrderURL, EndPoints.Orders + "?Id=" + orderId, "GET", null);
            OrderbaseResponse = OrderbaseResponse.JsonParseList(temp);
            List<Orders> tempList = (List<Orders>)OrderbaseResponse.Data;

            var OrderItem = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + orderId, "GET", null);
            baseResponse = baseResponse.JsonParseList(OrderItem);
            List<OrderItems> orderItems = (List<OrderItems>)baseResponse.Data;

            if (orderItems.Any(x => x.Status.Contains("Delivered")))
            {
                tempList[0].Status = "Delivered";
                tempList[0].ModifiedAt = DateTime.Now;
                tempList[0].ModifiedBy = UserId;
                var response = helper.ApiCall(OrderURL, EndPoints.Orders, "PUT", tempList[0]);
                OrderbaseResponse = OrderbaseResponse.JsonParseInputResponse(response);
            }

            return OrderbaseResponse;
        }

        public BaseResponse<OrderItems> SaveOrderPackage(OrderPackageDto model)
        {
            BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();

            BaseResponse<OrderItems> ItembaseResponse = new BaseResponse<OrderItems>();
            var ordertemp = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + model.OrderID, "Get", null);
            ItembaseResponse = ItembaseResponse.JsonParseList(ordertemp);
            if (ItembaseResponse.code == 200)
            {
                List<OrderItems> orderItemslst = (List<OrderItems>)ItembaseResponse.Data;
                string[] ItemIds = model.OrderItemIDs?.Trim('{', '}').Split(',');
                List<OrderItems> matchingItems = orderItemslst.Where(p => ItemIds.Contains(p.Id.ToString())).ToList();
                matchingItems = matchingItems.Where(p => p.Status.ToLower() != "confirmed").ToList();

                if (matchingItems.Count > 0)
                {
                    baseResponse.code = 204;
                    baseResponse.Data = null;
                    baseResponse.Message = "Order Item Status is not matched";
                }
                else
                {
                    List<OrderItems> matchingItems1 = orderItemslst.Where(p => p.Status.ToLower() == "placed" || p.Status.ToLower() == "confirmed").ToList();

                    decimal totalitemSubtotal = matchingItems1.Sum(p => p.SubTotal);
                    decimal ratio = Convert.ToDecimal(model.PackageAmount) / totalitemSubtotal;
                    decimal Codcharge = 0;
                    decimal OrderCodcharge = orderItemslst.FirstOrDefault().OrderCodCharges != null ? Convert.ToDecimal(orderItemslst.FirstOrDefault().OrderCodCharges) : 0;
                    decimal _Codcharge = orderItemslst.FirstOrDefault().OrderCodCharges != null ? Convert.ToDecimal(orderItemslst.FirstOrDefault().OrderCodCharges) : 0;
                    ratio = Convert.ToDecimal(Convert.ToDecimal(ratio).ToString("N2"));

                    BaseResponse<OrderPackage> baseResponseOrderPakage = new BaseResponse<OrderPackage>();
                    List<OrderPackage> packlist = new List<OrderPackage>();
                    var response1 = helper.ApiCall(OrderURL, EndPoints.OrderPackage + "?PageIndex=" + 0 + "&PageSize=" + 0 + "&Orderid=" + model.OrderID, "GET", null);
                    baseResponseOrderPakage = baseResponseOrderPakage.JsonParseList(response1);
                    if (baseResponseOrderPakage.code == 200)
                    {
                        packlist = (List<OrderPackage>)baseResponseOrderPakage.Data;

                        if (packlist.Count > 0)
                        {
                            Codcharge = packlist.Sum(p => p.CodCharges);
                            decimal c = Convert.ToDecimal(Convert.ToDecimal(Codcharge).ToString("N0"));
                            if (c >= _Codcharge)
                            {
                                _Codcharge = 0;
                            }
                            else
                            {

                                _Codcharge = _Codcharge - Codcharge;
                                decimal _d = _Codcharge + c;
                                decimal diff = Math.Abs(_d - OrderCodcharge);
                                if (diff > 0)
                                {
                                    _Codcharge = 0;
                                }
                            }
                        }
                    }

                    decimal totalCod = Convert.ToDecimal(Convert.ToDecimal(ratio * _Codcharge).ToString("N2"));


                    OrderPackage orderPack = new OrderPackage();
                    orderPack.OrderID = model.OrderID;
                    orderPack.OrderItemIDs = model.OrderItemIDs;
                    orderPack.PackageNo = Guid.NewGuid().ToString();
                    orderPack.TotalItems = model.TotalItems;
                    orderPack.NoOfPackage = model.NoOfPackage;
                    orderPack.PackageAmount = model.PackageAmount;
                    orderPack.CodCharges = totalCod;
                    orderPack.CreatedBy = UserId;
                    orderPack.CreatedAt = DateTime.Now;

                    var response = helper.ApiCall(OrderURL, EndPoints.OrderPackage, "POST", orderPack);
                    baseResponse = baseResponse.JsonParseInputResponse(response);

                    string[] orderItemIds = orderPack.OrderItemIDs?.Trim('{', '}').Split(',');
                    UpdateOrderStatus(orderPack.OrderID, orderItemIds, "Packed");


                    //if (ItembaseResponse.code == 200)
                    //{
                    //    UpdateMainOrderStstus(orderItemslst, model.OrderID);
                    //}
                }
            }

            return baseResponse;
        }

        public BaseResponse<OrderShipmentInfo> SaveOrderShipment(OrderShipmentInfoDto model)
        {
            BaseResponse<OrderShipmentInfo> baseResponse = new BaseResponse<OrderShipmentInfo>();

            BaseResponse<OrderItems> ItembaseResponse = new BaseResponse<OrderItems>();
            var ordertemp = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + model.OrderID, "Get", null);
            ItembaseResponse = ItembaseResponse.JsonParseList(ordertemp);
            if (ItembaseResponse.code == 200)
            {
                List<OrderItems> orderItemslst = (List<OrderItems>)ItembaseResponse.Data;
                string[] ItemIds = model.OrderItemIDs?.Trim('{', '}').Split(',');
                List<OrderItems> matchingItems = orderItemslst.Where(p => ItemIds.Contains(p.Id.ToString())).ToList();
                matchingItems = matchingItems.Where(p => p.Status.ToLower() != "packed").ToList();

                if (matchingItems.Count > 0)
                {
                    baseResponse.code = 204;
                    baseResponse.Data = null;
                    baseResponse.Message = "Order Item Status is not matched";
                }
                else
                {


                    string userID = UserId;
                    GetOrders getorders = new GetOrders(_configuration, _httpContext, userID);

                    Warehouse warehouse = getorders.getWarehouse(model.WarehouseId);
                    SellerKycList sellerKyc = getorders.getsellerKyc(model.SellerID);

                    //method fir DHL reletad data 
                    JObject res = CreateShipmentDHL(orderItemslst, model, warehouse, sellerKyc);
                    JObject respick = PickupShipment(orderItemslst, model, warehouse, sellerKyc);

                    var temp = helper.ApiCall(OrderURL, EndPoints.ShipmentInfo + "?OrderID=" + model.OrderID + "&OrderItemIDs=" + model.OrderItemIDs, "GET", null);
                    baseResponse = baseResponse.JsonParseList(temp);
                    List<OrderShipmentInfo> tmp = (List<OrderShipmentInfo>)baseResponse.Data;
                    if (tmp.Any())
                    {
                        baseResponse = baseResponse.AlreadyExists();
                    }
                    else
                    {
                        OrderShipmentInfo orderShip = new OrderShipmentInfo();

                        orderShip.OrderID = model.OrderID;
                        orderShip.OrderItemIDs = model.OrderItemIDs;
                        orderShip.SellerID = model.SellerID;
                        orderShip.PackageID = model.PackageID;
                        orderShip.PaymentMode = model.PaymentMode;
                        orderShip.Length = model.Length;
                        orderShip.Width = model.Width;
                        orderShip.Height = model.Height;
                        orderShip.Weight = model.Weight;
                        orderShip.InvoiceAmount = model.InvoiceAmount;
                        orderShip.InvoiceCodCharges = model.InvoiceCodCharges;
                        orderShip.PackageDescription = model.PackageDescription;
                        orderShip.IsShipmentInitiate = model.IsShipmentInitiate;
                        orderShip.IsPaymentSuccess = model.IsPaymentSuccess;
                        orderShip.CourierID = model.CourierID;
                        orderShip.ServiceID = model.ServiceID;
                        orderShip.ServiceType = model.ServiceType;
                        orderShip.PickupContactPersonName = warehouse.ContactPersonName;
                        orderShip.PickupContactPersonMobileNo = warehouse.ContactPersonMobileNo;
                        orderShip.PickupContactPersonEmailID = sellerKyc.EmailID;
                        orderShip.PickupCompanyName = sellerKyc.TradeName != null ? sellerKyc.TradeName : sellerKyc.DisplayName;
                        orderShip.PickupAddressLine1 = warehouse.AddressLine1;
                        orderShip.PickupAddressLine2 = warehouse.AddressLine2;
                        orderShip.PickupLandmark = warehouse.Landmark;
                        orderShip.PickupPincode = Convert.ToInt32(warehouse.Pincode);
                        orderShip.PickupCity = warehouse.CityName;
                        orderShip.PickupState = warehouse.StateName;
                        orderShip.PickupCountry = warehouse.CountryName;
                        orderShip.PickupTaxNo = warehouse.GSTNo;

                        orderShip.DropContactPersonName = model.DropContactPersonName;
                        orderShip.DropContactPersonMobileNo = model.DropContactPersonMobileNo;
                        orderShip.DropContactPersonEmailID = model.DropContactPersonEmailID;
                        orderShip.DropCompanyName = model.DropCompanyName;
                        orderShip.DropAddressLine1 = model.DropAddressLine1;
                        orderShip.DropAddressLine2 = model.DropAddressLine2;
                        orderShip.DropLandmark = model.DropLandmark;
                        orderShip.DropPincode = model.DropPincode;
                        orderShip.DropCity = model.DropCity;
                        orderShip.DropState = model.DropState;
                        orderShip.DropCountry = model.DropCountry;
                        orderShip.DropTaxNo = model.DropTaxNo;

                        orderShip.ShipmentID = model.ShipmentID;
                        orderShip.ShipmentOrderID = model.ShipmentOrderID;
                        orderShip.ShippingAmountFromPartner = model.ShippingAmountFromPartner;
                        orderShip.IsShipmentSheduledByAdmin = model.IsShipmentSheduledByAdmin;
                        orderShip.PickupLocationID = model.PickupLocationID;
                        orderShip.ErrorMessage = model.ErrorMessage;
                        orderShip.ForwardLable = model.ForwardLable;
                        orderShip.ReturnLable = model.ReturnLable;
                        orderShip.CreatedBy = UserId;
                        orderShip.CreatedAt = DateTime.Now;
                        orderShip.ShippingPartner = model.ShippingPartner != null ? model.ShippingPartner : sellerKyc.ShipmentBy;
                        if (!string.IsNullOrEmpty(model.ShippingPartner) && model.ShippingPartner.ToLower() == "dhl")
                        {
                            orderShip.CourierName = "DHL";
                            orderShip.AwbNo = (string)res["packages"][0]["trackingNumber"];
                            orderShip.ShipmentTrackingNo = (string)res["shipmentTrackingNumber"];
                            orderShip.TrackingNo = (string)res["packages"][0]["trackingNumber"];
                            orderShip.ShipmentInfo = res.ToString();
                        }
                        else
                        {
                            orderShip.CourierName = model.CourierName;
                            orderShip.AwbNo = model.AwbNo;
                            orderShip.ShipmentTrackingNo = model.ShipmentTrackingNo;
                            orderShip.TrackingNo = model.TrackingNo;
                            orderShip.ShipmentInfo = model.ShipmentInfo;
                        }
                        //var response = helper.ApiCall(OrderURL, EndPoints.ShipmentInfo, "POST", orderShip);
                        //baseResponse = baseResponse.JsonParseInputResponse(response);

                        string[] orderItemIds = orderShip.OrderItemIDs?.Trim('{', '}').Split(',');
                        //UpdateOrderStatus(orderShip.OrderID, orderItemIds, "Shipped");


                        if (ItembaseResponse.code == 200)
                        {
                            //UpdateMainOrderStstus(orderItemslst, model.OrderID);

                            //Sendmail(model.OrderID, "Shipped", _httpContext.User.Claims.Where(x => x.Type.Equals("role")).FirstOrDefault().Value);
                            Sendmail(model.OrderID, "Shipped", _httpContext.User.Claims.Where(x => x.Type.Equals(System.Security.Claims.ClaimTypes.Role)).FirstOrDefault().Value);
                        }

                        //SaveInvoice(orderShip);

                    }
                }
            }

            return baseResponse;
        }

        public BaseResponse<OrderItems> OrderConfirm(OrderConfirmDto model)
        {
            BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
            var temp = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + model.OrderId, "Get", null);
            baseResponse = baseResponse.JsonParseList(temp);
            if (baseResponse.code == 200)
            {
                List<OrderItems> orderItemslst = (List<OrderItems>)baseResponse.Data;
                OrderItems orderRecord = orderItemslst.Where(p => p.Id == model.OrderItemId).FirstOrDefault();

                if (orderRecord.Status.ToLower() != "placed")
                {

                    baseResponse.code = 204;
                    baseResponse.Data = null;
                    baseResponse.Message = "Order Item Status is not matched";
                }
                else
                {

                    if (orderRecord.Id != null && orderRecord.Id != 0)
                    {
                        bool AllowWarehouseManagement = Convert.ToBoolean(_configuration.GetValue<string>("Allow_warehouse_management"));
                        bool Orderconfirmation = false;
                        ProductWareHouse _productWarehouse = new ProductWareHouse();
                        if (AllowWarehouseManagement)
                        {
                            BaseResponse<ProductWareHouse> productWarehouse = new BaseResponse<ProductWareHouse>();
                            var resp = helper.ApiCall(_catlogUrl, EndPoints.ProductWarehouse + "?id=" + model.productWarehouseId, "GET", null);
                            productWarehouse = productWarehouse.JsonParseRecord(resp);
                            if (productWarehouse.code == 200)
                            {
                                _productWarehouse = (ProductWareHouse)productWarehouse.Data;
                                if (_productWarehouse.Id != null || _productWarehouse.Id != 0)
                                {
                                    if (_productWarehouse.ProductQuantity >= orderRecord.Qty)
                                    {
                                        int orderqty = Convert.ToInt32(_productWarehouse.ProductQuantity) - Convert.ToInt32(orderRecord.Qty);
                                        _productWarehouse.ProductQuantity = orderqty;
                                        var productWarehouseresponse = helper.ApiCall(_catlogUrl, EndPoints.ProductWarehouse, "PUT", _productWarehouse);
                                        productWarehouse = productWarehouse.JsonParseInputResponse(productWarehouseresponse);
                                        Orderconfirmation = true;
                                    }
                                    else
                                    {
                                        Orderconfirmation = false;
                                        baseResponse.code = 204;
                                        baseResponse.Message = "Product warehouse stock is not available.";
                                        baseResponse.Data = null;
                                        baseResponse.pagination = null;
                                    }
                                }
                                else
                                {
                                    Orderconfirmation = false;
                                    baseResponse.code = 204;
                                    baseResponse.Message = "Product warehouse not available.";
                                    baseResponse.Data = null;
                                    baseResponse.pagination = null;
                                }

                            }
                            else
                            {
                                Orderconfirmation = false;
                                baseResponse.code = 204;
                                baseResponse.Message = "Product warehouse not available.";
                                baseResponse.Data = null;
                                baseResponse.pagination = null;
                            }

                        }
                        else
                        {
                            Orderconfirmation = true;
                        }

                        if (Orderconfirmation)
                        {
                            //OrderItems record = (OrderItems)baseResponse.Data;
                            OrderItems record = orderRecord;
                            record.Status = "Confirmed";
                            record.WherehouseId = model.warehouseId;
                            record.ModifiedBy = UserId;
                            record.ModifiedAt = DateTime.Now;

                            var response = helper.ApiCall(OrderURL, EndPoints.OrderItems, "PUT", record);
                            baseResponse = baseResponse.JsonParseInputResponse(response);

                            OrderTrackEntries(model.OrderId, model.OrderItemId, record.Status);
                            UpdateMainOrderStstus(orderItemslst, model.OrderId);
                            //Sendmail(model.OrderId, "Confirmed", _httpContext.User.Claims.Where(x => x.Type.Equals("role")).FirstOrDefault().Value);
                            Sendmail(model.OrderId, "Confirmed", _httpContext.User.Claims.Where(x => x.Type.Equals(System.Security.Claims.ClaimTypes.Role)).FirstOrDefault().Value);
                        }
                    }
                    else
                    {
                        baseResponse = baseResponse.NotExist();
                    }
                }
            }

            return baseResponse;
        }

        public BaseResponse<OrderRefund> OrderRefund(OrderRefund model)
        {
            BaseResponse<OrderRefund> orderRefundresponse = new BaseResponse<OrderRefund>();

            var temp = helper.ApiCall(OrderURL, EndPoints.OrderRefund + "?Id=" + model.Id, "Get", null);
            orderRefundresponse = orderRefundresponse.JsonParseRecord(temp);
            if (orderRefundresponse.code == 200)
            {
                OrderRefund refund = new OrderRefund();
                refund = (OrderRefund)orderRefundresponse.Data;
                refund.TransactionID = model.TransactionID;
                refund.Comment = model.Comment;
                refund.Status = model.Status;
                refund.ModifiedBy = UserId;
                refund.ModifiedAt = DateTime.Now;

                var response = helper.ApiCall(OrderURL, EndPoints.OrderRefund, "PUT", refund);
                orderRefundresponse = orderRefundresponse.JsonParseInputResponse(response);
            }
            return orderRefundresponse;
        }

        public BaseResponse<OrderRefund> OrderRefundRequest(OrderRefund model)
        {
            BaseResponse<OrderRefund> orderRefundresponse = new BaseResponse<OrderRefund>();

            var Refundresponse = helper.ApiCall(OrderURL, EndPoints.OrderRefund, "POST", model);
            orderRefundresponse = orderRefundresponse.JsonParseInputResponse(Refundresponse);

            return orderRefundresponse;
        }

        public BaseResponse<OrderCancelReturn> OrderReturn(OrderReturnDto model)
        {
            BaseResponse<OrderCancelReturn> BaseResponse = new BaseResponse<OrderCancelReturn>();

            var temp = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?OrderID=" + model.OrderID + "&OrderItemID=" + model.OrderItemID + "&ActionID=" + model.ActionID + "&UserId=" + model.UserId, "GET", null);
            BaseResponse = BaseResponse.JsonParseRecord(temp);
            List<OrderCancelReturn> orderRecord = (List<OrderCancelReturn>)BaseResponse.Data;
            if (orderRecord.Any())
            {
                BaseResponse = BaseResponse.AlreadyExists();
            }
            else
            {
                OrderCancelReturn orderCancelReturn = new OrderCancelReturn();

                orderCancelReturn.OrderID = model.OrderID;
                orderCancelReturn.OrderItemID = model.OrderItemID;
                orderCancelReturn.NewOrderNo = Guid.NewGuid().ToString();
                orderCancelReturn.Qty = model.Qty;
                orderCancelReturn.ActionID = model.ActionID;
                orderCancelReturn.ExchangeProductID = null;
                orderCancelReturn.ExchangeSize = null;
                orderCancelReturn.ExchangePriceDiff = null;
                orderCancelReturn.UserId = model.UserId;
                orderCancelReturn.UserName = model.UserName;
                orderCancelReturn.UserPhoneNo = model.UserPhoneNo;
                orderCancelReturn.UserEmail = model.UserEmail;
                orderCancelReturn.UserGSTNo = model.UserGSTNo;
                orderCancelReturn.AddressLine1 = model.AddressLine1;
                orderCancelReturn.AddressLine2 = model.AddressLine2;
                orderCancelReturn.Landmark = model.Landmark;
                orderCancelReturn.Pincode = model.Pincode;
                orderCancelReturn.City = model.City;
                orderCancelReturn.State = model.State;
                orderCancelReturn.Country = model.Country;
                orderCancelReturn.Issue = model.Issue;
                orderCancelReturn.Reason = model.Reason;
                orderCancelReturn.Comment = model.Comment;
                orderCancelReturn.PaymentMode = model.PaymentMode;
                orderCancelReturn.Attachment = model.Attachment;
                orderCancelReturn.RefundAmount = model.RefundAmount;
                orderCancelReturn.RefundType = model.RefundType;
                orderCancelReturn.BankName = model.BankName;
                orderCancelReturn.BankBranch = model.BankBranch;
                orderCancelReturn.BankIFSCCode = model.BankIFSCCode;
                orderCancelReturn.BankAccountNo = model.BankAccountNo;
                orderCancelReturn.AccountType = model.AccountType;
                orderCancelReturn.AccountHolderName = model.AccountHolderName;
                orderCancelReturn.ApprovedByID = null;
                orderCancelReturn.ApprovedByName = null;
                orderCancelReturn.Status = "Return Requested";
                orderCancelReturn.RefundStatus = "Pending";
                orderCancelReturn.CreatedAt = DateTime.Now;
                orderCancelReturn.CreatedBy = UserId;

                var response = helper.ApiCall(OrderURL, EndPoints.CancelReturn, "POST", orderCancelReturn);
                BaseResponse = BaseResponse.JsonParseInputResponse(response);
                string[] orderItemIds = model.OrderItemID.ToString()?.Trim('{', '}').Split(',');
                UpdateOrderStatus(model.OrderID, orderItemIds, "Return Requested");
            }
            return BaseResponse;
        }

        public BaseResponse<OrderCancelReturn> OrderReturnRequest(OrderReturnReuqestdto model)
        {
            BaseResponse<OrderCancelReturn> BaseResponse = new BaseResponse<OrderCancelReturn>();

            var temp = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?Id=" + model.ReturnRequestId, "GET", null);
            BaseResponse = BaseResponse.JsonParseRecord(temp);
            if (BaseResponse.code == 200)
            {
                OrderCancelReturn orderRecord = (OrderCancelReturn)BaseResponse.Data;

                string userID = UserId;
                GetOrders getorders = new GetOrders(_configuration, _httpContext, userID);
                SellerKycList sellerKyc = getorders.getsellerKyc(model.SellerID);

                //OrderCancelReturn orderCancelReturn = new OrderCancelReturn();

                //orderCancelReturn.OrderID = orderRecord.OrderID;
                //orderCancelReturn.OrderItemID = orderRecord.OrderItemID;
                //orderCancelReturn.NewOrderNo = orderRecord.NewOrderNo;
                //orderCancelReturn.Qty = orderRecord.Qty;
                //orderCancelReturn.ActionID = orderRecord.ActionID;
                //orderCancelReturn.ExchangeProductID = orderRecord.ExchangeProductID;
                //orderCancelReturn.ExchangeSize = orderRecord.ExchangeSize;
                //orderCancelReturn.ExchangePriceDiff = orderRecord.ExchangePriceDiff;
                //orderCancelReturn.UserId = orderRecord.UserId;
                //orderCancelReturn.UserName = orderRecord.UserName;
                //orderCancelReturn.UserPhoneNo = orderRecord.UserPhoneNo;
                //orderCancelReturn.UserEmail = orderRecord.UserEmail;
                //orderCancelReturn.UserGSTNo = orderRecord.UserGSTNo;
                //orderCancelReturn.AddressLine1 = orderRecord.AddressLine1;
                //orderCancelReturn.AddressLine2 = orderRecord.AddressLine2;
                //orderCancelReturn.Landmark = orderRecord.Landmark;
                //orderCancelReturn.Pincode = orderRecord.Pincode;
                //orderCancelReturn.City = orderRecord.City;
                //orderCancelReturn.State = orderRecord.State;
                //orderCancelReturn.Country = orderRecord.Country;
                //orderCancelReturn.Issue = orderRecord.Issue;
                //orderCancelReturn.Reason = orderRecord.Reason;
                //orderCancelReturn.Comment = orderRecord.Comment;
                //orderCancelReturn.PaymentMode = orderRecord.PaymentMode;
                //orderCancelReturn.Attachment = orderRecord.Attachment;
                //orderCancelReturn.RefundAmount = orderRecord.RefundAmount;
                //orderCancelReturn.RefundType = orderRecord.RefundType;
                //orderCancelReturn.BankName = orderRecord.BankName;
                //orderCancelReturn.BankBranch = orderRecord.BankBranch;
                //orderCancelReturn.BankIFSCCode = orderRecord.BankIFSCCode;
                //orderCancelReturn.BankAccountNo = orderRecord.BankAccountNo;
                //orderCancelReturn.AccountType = orderRecord.AccountType;
                //orderCancelReturn.AccountHolderName = orderRecord.AccountHolderName;
                orderRecord.ApprovedByID = model.ApprovedByID;
                orderRecord.ApprovedByName = model.ApprovedByName;
                orderRecord.Status = model.Status;
                orderRecord.RefundStatus = model.RefundStatus;
                orderRecord.ModifiedAt = DateTime.Now;
                orderRecord.ModifiedBy = UserId;

                // here we are calling the pickup address.
                orderRecord.DropContactPersonName = model.DropContactPersonName;
                orderRecord.DropContactPersonMobileNo = model.DropContactPersonMobileNo;
                orderRecord.DropContactPersonEmailID = model.DropContactPersonEmailID;
                orderRecord.DropCompanyName = model.DropCompanyName;
                orderRecord.DropAddressLine1 = model.DropAddressLine1;
                orderRecord.DropAddressLine2 = model.DropAddressLine2;
                orderRecord.DropLandmark = model.DropLandmark;
                orderRecord.DropPincode = model.DropPincode;
                orderRecord.DropCity = model.DropCity;
                orderRecord.DropState = model.DropState;
                orderRecord.DropCountry = model.DropCountry;
                orderRecord.CustomeProductName = model.CustomeProductName;
                orderRecord.ShipmentID = model.ShipmentID;
                orderRecord.ShipmentOrderID = model.ShipmentOrderID;
                orderRecord.ShippingPartner = model.ShippingPartner != null ? model.ShippingPartner : sellerKyc.ShipmentBy;
                orderRecord.CourierName = model.CourierName;
                orderRecord.ShippingAmountFromPartner = model.ShippingAmountFromPartner;
                orderRecord.AwbNo = model.AwbNo;
                orderRecord.IsShipmentSheduledByAdmin = model.IsShipmentSheduledByAdmin;
                orderRecord.PickupLocationID = model.PickupLocationID;
                orderRecord.ErrorMessage = model.ErrorMessage;
                orderRecord.ForwardLable = model.ForwardLable;
                orderRecord.ReturnLable = model.ReturnLable;
                orderRecord.SellerID = model.SellerID;
                orderRecord.ShipmentTrackingNo = model.ShipmentTrackingNo;
                orderRecord.TrackingNo = model.TrackingNo;
                orderRecord.ShipmentInfo = model.ShipmentInfo;
                var response = helper.ApiCall(OrderURL, EndPoints.CancelReturn, "PUT", orderRecord);
                BaseResponse = BaseResponse.JsonParseInputResponse(response);

                var orderCancelReturnId = BaseResponse.Data;

                BaseResponse<OrderReturnAction> getOrderReturnAction = new BaseResponse<OrderReturnAction>();
                var _temp = helper.ApiCall(OrderURL, EndPoints.OrderReturnAction + "?Id=" + orderRecord.ActionID, "GET", null);
                getOrderReturnAction = getOrderReturnAction.JsonParseRecord(_temp);


                if (getOrderReturnAction.code == 200)
                {
                    OrderReturnAction orderReturnAction = new OrderReturnAction();
                    orderReturnAction = (OrderReturnAction)getOrderReturnAction.Data;
                    if (orderRecord.ActionID == orderReturnAction.Id && model.Status.ToLower() == "return approved")
                    {
                        string[] orderItemIds = model.OrderItemID.ToString()?.Trim('{', '}').Split(',');
                        UpdateOrderStatus(model.OrderID, orderItemIds, "Returned", Convert.ToInt32(model.WarehouseId));

                        OrderRefund refund = new OrderRefund();
                        refund.OrderID = model.OrderID;
                        refund.OrderItemID = model.OrderItemID;
                        refund.OrderCancelReturnID = model.ReturnRequestId;
                        refund.RefundAmount = Convert.ToDecimal(model.RefundAmount);
                        refund.TransactionID = "";
                        refund.Comment = "";
                        refund.Status = "In Process";
                        refund.CreatedBy = UserId;
                        refund.CreatedAt = DateTime.Now;

                        //BaseResponse<OrderRefund> orderRefundresponse = new BaseResponse<OrderRefund>();
                        //var Refundresponse = helper.ApiCall(OrderURL, EndPoints.OrderRefund, "POST", refund);
                        //orderRefundresponse = orderRefundresponse.JsonParseInputResponse(Refundresponse);

                        OrderRefundRequest(refund);

                        ReturnShipmentInfo(orderRecord, Convert.ToInt32(orderRecord.Id));
                    }
                    else
                    {
                        string[] orderItemIds = model.OrderItemID.ToString()?.Trim('{', '}').Split(',');
                        UpdateOrderStatus(model.OrderID, orderItemIds, "Return Rejected");
                        //BaseResponse<OrderItems> ItembaseResponse = new BaseResponse<OrderItems>();
                        //var temp1 = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + model.OrderID, "Get", null);
                        //ItembaseResponse = ItembaseResponse.JsonParseList(temp1);
                        //if (ItembaseResponse.code == 200)
                        //{
                        //    List<OrderItems> orderItemslst = (List<OrderItems>)ItembaseResponse.Data;

                        //    UpdateMainOrderStstus(orderItemslst, model.OrderID);
                        //}
                    }
                }

            }
            else
            {
                BaseResponse = BaseResponse.NotExist();
            }
            return BaseResponse;
        }

        public BaseResponse<OrderCancelReturn> CancelReturnRequest(CancelReturnReuqestdto model)
        {
            BaseResponse<OrderCancelReturn> BaseResponse = new BaseResponse<OrderCancelReturn>();

            var temp = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?Id=" + model.ReturnRequestId, "GET", null);
            BaseResponse = BaseResponse.JsonParseRecord(temp);
            if (BaseResponse.code == 200)
            {
                OrderCancelReturn orderRecord = (OrderCancelReturn)BaseResponse.Data;

                string userID = UserId;
                GetOrders getorders = new GetOrders(_configuration, _httpContext, userID);
                orderRecord.ApprovedByID = model.ApprovedByID;
                orderRecord.ApprovedByName = model.ApprovedByName;
                orderRecord.Status = "Return Rejected";
                orderRecord.RefundStatus = "Rejected";
                orderRecord.ModifiedAt = DateTime.Now;
                orderRecord.ModifiedBy = UserId;

                var response = helper.ApiCall(OrderURL, EndPoints.CancelReturn, "PUT", orderRecord);
                BaseResponse = BaseResponse.JsonParseInputResponse(response);

                string[] orderItemIds = model.OrderItemID.ToString()?.Trim('{', '}').Split(',');
                UpdateOrderStatus(model.OrderID, orderItemIds, "Return Rejected");
            }
            else
            {
                BaseResponse = BaseResponse.NotExist();
            }
            return BaseResponse;
        }

        public BaseResponse<ReturnShipmentInfo> ReturnShipmentInfoAPI(OrderCancelReturndto model)
        {
            BaseResponse<ReturnShipmentInfo> returnShipMentResponse = new BaseResponse<ReturnShipmentInfo>();
            BaseResponse<SellerProduct> sellerProductResponse = new BaseResponse<SellerProduct>();
            BaseResponse<OrderItems> orderItemResponse = new BaseResponse<OrderItems>();
            BaseResponse<OrderShipmentInfo> orderShipResponse = new BaseResponse<OrderShipmentInfo>();


            int SellerProductID = 0;
            int? ProductID = 0;

            // api call for getting SellerProductId and ProductId.
            var orderdetail = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?orderId=" + model.OrderID + "&OrderItemID=" + model.OrderItemID, "GET", null);
            orderItemResponse = orderItemResponse.JsonParseRecord(orderdetail);
            OrderItems orderRecord = (OrderItems)orderItemResponse.Data;
            SellerProductID = orderRecord.SellerProductID;
            ProductID = orderRecord.ProductID;

            // api call for getting sellerProduct Detail.
            var sellerproduct = helper.ApiCall(_catlogUrl, EndPoints.SellerProduct + "?Id=" + SellerProductID + "&ProductID=" + ProductID, "GET", null);
            sellerProductResponse = sellerProductResponse.JsonParseRecord(sellerproduct);
            SellerProduct sellerProduct = (SellerProduct)sellerProductResponse.Data;

            var orderShip = helper.ApiCall(OrderURL, EndPoints.ShipmentInfo + "?OrderID=" + model.OrderID + "&OrderItemID=" + model.OrderItemID, "GET", null);
            orderShipResponse = orderShipResponse.JsonParseRecord(orderShip);
            OrderShipmentInfo orderShipment = (OrderShipmentInfo)orderShipResponse.Data;


            var temp = helper.ApiCall(OrderURL, EndPoints.ReturnShipmentInfo + "?OrderId=" + model.OrderID + "&OrderItemId=" + model.OrderItemID + "&OrderCancelReturnID=" + model.Id, "GET", null);
            returnShipMentResponse = returnShipMentResponse.JsonParseRecord(temp);
            List<ReturnShipmentInfo> orderRefundRecord = (List<ReturnShipmentInfo>)returnShipMentResponse.Data;
            if (orderRefundRecord.Any())
            {
                returnShipMentResponse = returnShipMentResponse.AlreadyExists();
            }
            else
            {
                ReturnShipmentInfo returnShipment = new ReturnShipmentInfo();
                returnShipment.OrderID = model.OrderID;
                returnShipment.OrderItemID = model.OrderItemID;
                returnShipment.OrderCancelReturnID = model.orderCancelReturnId;

                returnShipment.Qty = model.Qty;
                returnShipment.PaymentMode = "Online";
                returnShipment.Length = Convert.ToString(sellerProduct.PackingLength);
                returnShipment.Width = Convert.ToString(sellerProduct.PackingBreadth);
                returnShipment.Height = Convert.ToString(sellerProduct.PackingHeight);
                returnShipment.Weight = Convert.ToString(sellerProduct.PackingWeight);
                returnShipment.ReturnValueAmount = orderRecord.TotalAmount;
                returnShipment.PackageDescription = model.CustomeProductName;
                returnShipment.IsShipmentInitiate = false;
                returnShipment.IsPaymentSuccess = false;
                returnShipment.CourierID = null;
                returnShipment.ServiceID = null;
                returnShipment.ServiceType = null;
                returnShipment.PickupContactPersonName = model.PickupContactPersonName;
                returnShipment.PickupContactPersonMobileNo = model.PickupContactPersonMobileNo;
                returnShipment.PickupContactPersonEmailID = model.PickupContactPersonEmailID;
                returnShipment.PickupCompanyName = model.PickupCompanyName;
                returnShipment.PickupAddressLine1 = model.PickupAddressLine1;
                returnShipment.PickupAddressLine2 = model.PickupAddressLine2;
                returnShipment.PickupLandmark = model.PickupLandmark;
                returnShipment.PickupPincode = (int)model.PickupPincode;
                returnShipment.PickupCity = model.PickupCity;
                returnShipment.PickupState = model.PickupState;
                returnShipment.PickupCountry = model.PickupCountry;
                returnShipment.DropContactPersonName = orderShipment.PickupContactPersonName;
                returnShipment.DropContactPersonMobileNo = orderShipment.PickupContactPersonMobileNo;
                returnShipment.DropContactPersonEmailID = orderShipment.PickupContactPersonEmailID;
                returnShipment.DropCompanyName = orderShipment.PickupCompanyName;
                returnShipment.DropAddressLine1 = orderShipment.PickupAddressLine1;
                returnShipment.DropAddressLine2 = orderShipment.PickupAddressLine2;
                returnShipment.DropLandmark = orderShipment.PickupLandmark;
                returnShipment.DropPincode = orderShipment.PickupPincode;
                returnShipment.DropCity = orderShipment.PickupCity;
                returnShipment.DropState = orderShipment.PickupState;
                returnShipment.DropCountry = orderShipment.PickupCountry;
                // passing null here due to 3rd paty integration.
                returnShipment.ShipmentID = null;
                returnShipment.ShipmentOrderID = null;
                returnShipment.ShippingPartner = null;
                returnShipment.CourierName = null;
                returnShipment.ShippingAmountFromPartner = null;
                returnShipment.AwbNo = null;
                returnShipment.IsShipmentSheduledByAdmin = false;
                returnShipment.PickupLocationID = null;
                returnShipment.ErrorMessage = null;
                returnShipment.ForwardLable = null;
                returnShipment.ReturnLable = null;
                returnShipment.CreatedBy = UserId;
                returnShipment.CreatedAt = DateTime.Now;

                var response = helper.ApiCall(OrderURL, EndPoints.ReturnShipmentInfo, "POST", returnShipment);
                returnShipMentResponse = returnShipMentResponse.JsonParseInputResponse(response);
            }
            return returnShipMentResponse;
        }

        public BaseResponse<OrderCancelReturn> OrderReplace(OrderReplaceDto model)
        {
            BaseResponse<OrderCancelReturn> BaseResponse = new BaseResponse<OrderCancelReturn>();

            var temp = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?OrderID=" + model.OrderID + "&OrderItemID=" + model.OrderItemID + "&ActionID=" + model.ActionID + "&UserId=" + model.UserId, "GET", null);
            BaseResponse = BaseResponse.JsonParseRecord(temp);
            List<OrderCancelReturn> orderRecord = (List<OrderCancelReturn>)BaseResponse.Data;
            if (orderRecord.Any())
            {
                BaseResponse = BaseResponse.AlreadyExists();
            }
            else
            {
                OrderCancelReturn orderCancelReturn = new OrderCancelReturn();

                orderCancelReturn.OrderID = model.OrderID;
                orderCancelReturn.OrderItemID = model.OrderItemID;
                orderCancelReturn.NewOrderNo = Guid.NewGuid().ToString();
                orderCancelReturn.Qty = model.Qty;
                orderCancelReturn.ActionID = model.ActionID;
                orderCancelReturn.ExchangeProductID = null;
                orderCancelReturn.ExchangeSize = null;
                orderCancelReturn.ExchangePriceDiff = null;
                orderCancelReturn.UserId = model.UserId;
                orderCancelReturn.UserName = model.UserName;
                orderCancelReturn.UserPhoneNo = model.UserPhoneNo;
                orderCancelReturn.UserEmail = model.UserEmail;
                orderCancelReturn.UserGSTNo = model.UserGSTNo;
                orderCancelReturn.AddressLine1 = model.AddressLine1;
                orderCancelReturn.AddressLine2 = model.AddressLine2;
                orderCancelReturn.Landmark = model.Landmark;
                orderCancelReturn.Pincode = model.Pincode;
                orderCancelReturn.City = model.City;
                orderCancelReturn.State = model.State;
                orderCancelReturn.Country = model.Country;
                orderCancelReturn.Issue = model.Issue;
                orderCancelReturn.Reason = model.Reason;
                orderCancelReturn.Comment = model.Comment;
                orderCancelReturn.PaymentMode = model.PaymentMode;
                orderCancelReturn.Attachment = model.Attachment;
                orderCancelReturn.ApprovedByID = null;
                orderCancelReturn.ApprovedByName = null;
                orderCancelReturn.Status = "Replace Requested";
                orderCancelReturn.RefundStatus = null;
                orderCancelReturn.CreatedAt = DateTime.Now;
                orderCancelReturn.CreatedBy = UserId;

                var response = helper.ApiCall(OrderURL, EndPoints.CancelReturn, "POST", orderCancelReturn);
                BaseResponse = BaseResponse.JsonParseInputResponse(response);
                string[] orderItemIds = model.OrderItemID.ToString()?.Trim('{', '}').Split(',');
                UpdateOrderStatus(model.OrderID, orderItemIds, "Replace Requested");
            }
            return BaseResponse;
        }

        public BaseResponse<OrderCancelReturn> OrderReplaceRequest(OrderReplaceReuqestdto model)
        {
            BaseResponse<OrderCancelReturn> BaseResponse = new BaseResponse<OrderCancelReturn>();

            var temp = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?Id=" + model.ReturnRequestId, "GET", null);
            BaseResponse = BaseResponse.JsonParseRecord(temp);
            if (BaseResponse.code == 200)
            {
                OrderCancelReturn orderRecord = (OrderCancelReturn)BaseResponse.Data;

                string userID = UserId;
                GetOrders getorders = new GetOrders(_configuration, _httpContext, userID);
                SellerKycList sellerKyc = getorders.getsellerKyc(model.SellerID);
                orderRecord.ApprovedByID = model.ApprovedByID;
                orderRecord.ApprovedByName = model.ApprovedByName;
                orderRecord.Status = model.Status;
                orderRecord.ModifiedAt = DateTime.Now;
                orderRecord.ModifiedBy = UserId;

                // here we are calling the pickup address.
                orderRecord.DropContactPersonName = model.DropContactPersonName;
                orderRecord.DropContactPersonMobileNo = model.DropContactPersonMobileNo;
                orderRecord.DropContactPersonEmailID = model.DropContactPersonEmailID;
                orderRecord.DropCompanyName = model.DropCompanyName;
                orderRecord.DropAddressLine1 = model.DropAddressLine1;
                orderRecord.DropAddressLine2 = model.DropAddressLine2;
                orderRecord.DropLandmark = model.DropLandmark;
                orderRecord.DropPincode = model.DropPincode;
                orderRecord.DropCity = model.DropCity;
                orderRecord.DropState = model.DropState;
                orderRecord.DropCountry = model.DropCountry;
                orderRecord.ShipmentID = model.ShipmentID;
                orderRecord.ShipmentOrderID = model.ShipmentOrderID;
                orderRecord.ShippingPartner = model.ShippingPartner != null ? model.ShippingPartner : sellerKyc.ShipmentBy;
                orderRecord.CourierName = model.CourierName;
                orderRecord.ShippingAmountFromPartner = model.ShippingAmountFromPartner;
                orderRecord.AwbNo = model.AwbNo;
                orderRecord.IsShipmentSheduledByAdmin = model.IsShipmentSheduledByAdmin;
                orderRecord.PickupLocationID = model.PickupLocationID;
                orderRecord.ErrorMessage = model.ErrorMessage;
                orderRecord.ForwardLable = model.ForwardLable;
                orderRecord.ReturnLable = model.ReturnLable;
                orderRecord.SellerID = model.SellerID;
                orderRecord.ShipmentTrackingNo = model.ShipmentTrackingNo;
                orderRecord.TrackingNo = model.TrackingNo;
                orderRecord.ShipmentInfo = model.ShipmentInfo;
                var response = helper.ApiCall(OrderURL, EndPoints.CancelReturn, "PUT", orderRecord);
                BaseResponse = BaseResponse.JsonParseInputResponse(response);

                var orderCancelReturnId = BaseResponse.Data;

                BaseResponse<OrderReturnAction> getOrderReturnAction = new BaseResponse<OrderReturnAction>();
                var _temp = helper.ApiCall(OrderURL, EndPoints.OrderReturnAction + "?Id=" + orderRecord.ActionID, "GET", null);
                getOrderReturnAction = getOrderReturnAction.JsonParseRecord(_temp);
                if (getOrderReturnAction.code == 200)
                {
                    OrderReturnAction orderReturnAction = new OrderReturnAction();
                    orderReturnAction = (OrderReturnAction)getOrderReturnAction.Data;
                    if (orderRecord.ActionID == orderReturnAction.Id && model.Status.ToLower() == "replace approved")
                    {
                        string[] orderItemIds = model.OrderItemID.ToString()?.Trim('{', '}').Split(',');
                        UpdateOrderStatus(model.OrderID, orderItemIds, "Replaced", Convert.ToInt32(model.WarehouseId));
                        ReturnShipmentInfo(orderRecord, Convert.ToInt32(orderRecord.Id));
                        replaceOrder(model.OrderID, model.OrderItemID);
                    }
                    else
                    {
                        string[] orderItemIds = model.OrderItemID.ToString()?.Trim('{', '}').Split(',');
                        UpdateOrderStatus(model.OrderID, orderItemIds, "Replace Rejected");
                        //BaseResponse<OrderItems> ItembaseResponse = new BaseResponse<OrderItems>();
                        //var temp1 = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + model.OrderID, "Get", null);
                        //ItembaseResponse = ItembaseResponse.JsonParseList(temp1);
                        //if (ItembaseResponse.code == 200)
                        //{
                        //    List<OrderItems> orderItemslst = (List<OrderItems>)ItembaseResponse.Data;
                        //    UpdateMainOrderStstus(orderItemslst, model.OrderID);
                        //}
                    }
                }

            }
            else
            {
                BaseResponse = BaseResponse.NotExist();
            }
            return BaseResponse;
        }

        public BaseResponse<OrderCancelReturn> CancelReplaceRequest(CancelReplaceReuqestdto model)
        {
            BaseResponse<OrderCancelReturn> BaseResponse = new BaseResponse<OrderCancelReturn>();

            var temp = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?Id=" + model.ReturnRequestId, "GET", null);
            BaseResponse = BaseResponse.JsonParseRecord(temp);
            if (BaseResponse.code == 200)
            {
                OrderCancelReturn orderRecord = (OrderCancelReturn)BaseResponse.Data;

                string userID = UserId;
                GetOrders getorders = new GetOrders(_configuration, _httpContext, userID);
                orderRecord.ApprovedByID = model.ApprovedByID;
                orderRecord.ApprovedByName = model.ApprovedByName;
                orderRecord.Status = "Replace Rejected";
                orderRecord.ModifiedAt = DateTime.Now;
                orderRecord.ModifiedBy = UserId;

                var response = helper.ApiCall(OrderURL, EndPoints.CancelReturn, "PUT", orderRecord);
                BaseResponse = BaseResponse.JsonParseInputResponse(response);

                string[] orderItemIds = model.OrderItemID.ToString()?.Trim('{', '}').Split(',');
                UpdateOrderStatus(model.OrderID, orderItemIds, "Replace Rejected");
            }
            else
            {
                BaseResponse = BaseResponse.NotExist();
            }
            return BaseResponse;
        }

        public BaseResponse<OrderCancelReturn> OrderExchange(OrderExchangeDto model)
        {
            BaseResponse<OrderCancelReturn> BaseResponse = new BaseResponse<OrderCancelReturn>();

            var temp = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?OrderID=" + model.OrderID + "&OrderItemID=" + model.OrderItemID + "&ActionID=" + model.ActionID + "&UserId=" + model.UserId, "GET", null);
            BaseResponse = BaseResponse.JsonParseRecord(temp);
            List<OrderCancelReturn> orderRecord = (List<OrderCancelReturn>)BaseResponse.Data;
            if (orderRecord.Any())
            {
                BaseResponse = BaseResponse.AlreadyExists();
            }
            else
            {
                OrderCancelReturn orderCancelReturn = new OrderCancelReturn();

                orderCancelReturn.OrderID = model.OrderID;
                orderCancelReturn.OrderItemID = model.OrderItemID;
                orderCancelReturn.NewOrderNo = Guid.NewGuid().ToString();
                orderCancelReturn.Qty = model.Qty;
                orderCancelReturn.ActionID = model.ActionID;
                orderCancelReturn.ExchangeProductID = model.ExchangeProductID;
                orderCancelReturn.ExchangeSize = model.ExchangeSize;
                orderCancelReturn.ExchangePriceDiff = model.ExchangePriceDiff;
                orderCancelReturn.CustomeProductName = model.CustomeProductName;
                orderCancelReturn.UserId = model.UserId;
                orderCancelReturn.UserName = model.UserName;
                orderCancelReturn.UserPhoneNo = model.UserPhoneNo;
                orderCancelReturn.UserEmail = model.UserEmail;
                orderCancelReturn.UserGSTNo = model.UserGSTNo;
                orderCancelReturn.AddressLine1 = model.AddressLine1;
                orderCancelReturn.AddressLine2 = model.AddressLine2;
                orderCancelReturn.Landmark = model.Landmark;
                orderCancelReturn.Pincode = model.Pincode;
                orderCancelReturn.City = model.City;
                orderCancelReturn.State = model.State;
                orderCancelReturn.Country = model.Country;
                orderCancelReturn.Issue = model.Issue;
                orderCancelReturn.Reason = model.Reason;
                orderCancelReturn.Comment = model.Comment;
                orderCancelReturn.PaymentMode = model.PaymentMode;
                orderCancelReturn.Attachment = model.Attachment;
                orderCancelReturn.ApprovedByID = null;
                orderCancelReturn.ApprovedByName = null;
                orderCancelReturn.Status = "Exchange Requested";
                orderCancelReturn.RefundStatus = null;
                orderCancelReturn.CreatedAt = DateTime.Now;
                orderCancelReturn.CreatedBy = UserId;

                var response = helper.ApiCall(OrderURL, EndPoints.CancelReturn, "POST", orderCancelReturn);
                BaseResponse = BaseResponse.JsonParseInputResponse(response);
                string[] orderItemIds = model.OrderItemID.ToString()?.Trim('{', '}').Split(',');
                UpdateOrderStatus(model.OrderID, orderItemIds, "Exchange Requested");
            }
            return BaseResponse;
        }

        public BaseResponse<OrderCancelReturn> OrderExchangeRequest(OrderExchangeReuqestdto model)
        {
            BaseResponse<OrderCancelReturn> BaseResponse = new BaseResponse<OrderCancelReturn>();

            var temp = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?Id=" + model.ReturnRequestId, "GET", null);
            BaseResponse = BaseResponse.JsonParseRecord(temp);
            if (BaseResponse.code == 200)
            {
                OrderCancelReturn orderRecord = (OrderCancelReturn)BaseResponse.Data;

                string userID = UserId;
                GetOrders getorders = new GetOrders(_configuration, _httpContext, userID);
                SellerKycList sellerKyc = getorders.getsellerKyc(model.SellerID);
                orderRecord.ApprovedByID = model.ApprovedByID;
                orderRecord.ApprovedByName = model.ApprovedByName;
                orderRecord.Status = model.Status;
                orderRecord.ModifiedAt = DateTime.Now;
                orderRecord.ModifiedBy = UserId;

                // here we are calling the pickup address.
                orderRecord.DropContactPersonName = model.DropContactPersonName;
                orderRecord.DropContactPersonMobileNo = model.DropContactPersonMobileNo;
                orderRecord.DropContactPersonEmailID = model.DropContactPersonEmailID;
                orderRecord.DropCompanyName = model.DropCompanyName;
                orderRecord.DropAddressLine1 = model.DropAddressLine1;
                orderRecord.DropAddressLine2 = model.DropAddressLine2;
                orderRecord.DropLandmark = model.DropLandmark;
                orderRecord.DropPincode = model.DropPincode;
                orderRecord.DropCity = model.DropCity;
                orderRecord.DropState = model.DropState;
                orderRecord.DropCountry = model.DropCountry;
                orderRecord.ShipmentID = model.ShipmentID;
                orderRecord.ShipmentOrderID = model.ShipmentOrderID;
                orderRecord.ShippingPartner = model.ShippingPartner != null ? model.ShippingPartner : sellerKyc.ShipmentBy;
                orderRecord.CourierName = model.CourierName;
                orderRecord.ShippingAmountFromPartner = model.ShippingAmountFromPartner;
                orderRecord.AwbNo = model.AwbNo;
                orderRecord.IsShipmentSheduledByAdmin = model.IsShipmentSheduledByAdmin;
                orderRecord.PickupLocationID = model.PickupLocationID;
                orderRecord.ErrorMessage = model.ErrorMessage;
                orderRecord.ForwardLable = model.ForwardLable;
                orderRecord.ReturnLable = model.ReturnLable;
                orderRecord.SellerID = model.SellerID;
                orderRecord.ShipmentTrackingNo = model.ShipmentTrackingNo;
                orderRecord.TrackingNo = model.TrackingNo;
                orderRecord.ShipmentInfo = model.ShipmentInfo;

                var response = helper.ApiCall(OrderURL, EndPoints.CancelReturn, "PUT", orderRecord);
                BaseResponse = BaseResponse.JsonParseInputResponse(response);

                var orderCancelReturnId = BaseResponse.Data;

                BaseResponse<OrderReturnAction> getOrderReturnAction = new BaseResponse<OrderReturnAction>();
                var _temp = helper.ApiCall(OrderURL, EndPoints.OrderReturnAction + "?Id=" + orderRecord.ActionID, "GET", null);
                getOrderReturnAction = getOrderReturnAction.JsonParseRecord(_temp);

                if (getOrderReturnAction.code == 200)
                {
                    OrderReturnAction orderReturnAction = new OrderReturnAction();
                    orderReturnAction = (OrderReturnAction)getOrderReturnAction.Data;

                    if (orderRecord.ActionID == orderReturnAction.Id && model.Status.ToLower() == "exchange approved")
                    {
                        string[] orderItemIds = model.OrderItemID.ToString()?.Trim('{', '}').Split(',');
                        UpdateOrderStatus(model.OrderID, orderItemIds, "Exchanged", Convert.ToInt32(model.WarehouseId));
                        ReturnShipmentInfo(orderRecord, Convert.ToInt32(orderRecord.Id));
                        exchangeOrder(model.OrderID, model.OrderItemID, Convert.ToInt32(orderRecord.ExchangeProductID), Convert.ToInt32(orderRecord.ExchangeSizeId), orderRecord.ExchangeSize, orderRecord.ExchangePriceDiff);
                    }
                    else
                    {
                        string[] orderItemIds = model.OrderItemID.ToString()?.Trim('{', '}').Split(',');
                        UpdateOrderStatus(model.OrderID, orderItemIds, "Exchange Rejected");
                        //BaseResponse<OrderItems> ItembaseResponse = new BaseResponse<OrderItems>();
                        //var temp1 = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + model.OrderID, "Get", null);
                        //ItembaseResponse = ItembaseResponse.JsonParseList(temp1);
                        //if (ItembaseResponse.code == 200)
                        //{
                        //    List<OrderItems> orderItemslst = (List<OrderItems>)ItembaseResponse.Data;
                        //    UpdateMainOrderStstus(orderItemslst, model.OrderID);
                        //}
                    }
                }

            }
            else
            {
                BaseResponse = BaseResponse.NotExist();
            }
            return BaseResponse;
        }

        public BaseResponse<OrderCancelReturn> CancelExchangeReuqest(CancelExchangeReuqestdto model)
        {
            BaseResponse<OrderCancelReturn> BaseResponse = new BaseResponse<OrderCancelReturn>();

            var temp = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?Id=" + model.ReturnRequestId, "GET", null);
            BaseResponse = BaseResponse.JsonParseRecord(temp);
            if (BaseResponse.code == 200)
            {
                OrderCancelReturn orderRecord = (OrderCancelReturn)BaseResponse.Data;

                string userID = UserId;
                GetOrders getorders = new GetOrders(_configuration, _httpContext, userID);
                orderRecord.ApprovedByID = model.ApprovedByID;
                orderRecord.ApprovedByName = model.ApprovedByName;
                orderRecord.Status = "Exchange Rejected";
                orderRecord.RefundStatus = null;
                orderRecord.ModifiedAt = DateTime.Now;
                orderRecord.ModifiedBy = UserId;

                var response = helper.ApiCall(OrderURL, EndPoints.CancelReturn, "PUT", orderRecord);
                BaseResponse = BaseResponse.JsonParseInputResponse(response);

                string[] orderItemIds = model.OrderItemID.ToString()?.Trim('{', '}').Split(',');
                UpdateOrderStatus(model.OrderID, orderItemIds, "Exchange Rejected");
            }
            else
            {
                BaseResponse = BaseResponse.NotExist();
            }
            return BaseResponse;
        }

        public BaseResponse<OrderTaxInfo> ReplaceAndExchangeTaxInfo(OrderTaxInfo model)
        {
            BaseResponse<OrderTaxInfo> modelResponse = new BaseResponse<OrderTaxInfo>();

            var getTaxInfo = helper.ApiCall(OrderURL, EndPoints.OrderTaxInfo + "?OrderId=" + model.OrderID + "&OrderItemId=" + model.OrderItemID, "GET", null);
            modelResponse = modelResponse.JsonParseRecord(getTaxInfo);
            List<OrderTaxInfo> orderTax = (List<OrderTaxInfo>)modelResponse.Data;
            if (orderTax.Any())
            {
                modelResponse = modelResponse.AlreadyExists();
            }
            else
            {
                OrderTaxInfo tax = new OrderTaxInfo();

                tax.OrderID = model.OrderID;
                tax.OrderItemID = model.OrderItemID;
                tax.ProductID = model.ProductID;
                tax.SellerProductID = model.SellerProductID;
                tax.ShippingCharge = model.ShippingCharge;
                tax.ShippingZone = model.ShippingZone;
                tax.TaxOnShipping = model.TaxOnShipping;
                tax.CommissionIn = model.CommissionIn;
                tax.CommissionRate = model.CommissionRate;
                tax.CommissionAmount = model.CommissionAmount;
                tax.TaxOnCommission = model.TaxOnCommission;
                tax.NetEarn = model.NetEarn;
                tax.OrderTaxRateId = model.OrderTaxRateId;
                tax.OrderTaxRate = model.OrderTaxRate;
                tax.HSNCode = model.HSNCode;
                tax.ShipmentBy = model.ShipmentBy;
                tax.ShipmentPaidBy = model.ShipmentPaidBy;
                tax.IsSellerWithGSTAtOrderTime = model.IsSellerWithGSTAtOrderTime;
                tax.WeightSlab = model.WeightSlab;

                tax.CreatedBy = UserId;
                tax.CreatedAt = DateTime.Now;

                var callTaxInfo = helper.ApiCall(OrderURL, EndPoints.OrderTaxInfo, "POST", tax);
                modelResponse = modelResponse.JsonParseInputResponse(callTaxInfo);
            }
            return modelResponse;


        }

        public BaseResponse<OrderCancelReturn> ExchangeOrderRequest(OrderCancelReturn model)
        {
            BaseResponse<OrderCancelReturn> exchange = new BaseResponse<OrderCancelReturn>();

            var temp = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?OrderID=" + model.OrderID + "&OrderItemID=" + model.OrderItemID + "&ActionID=" + model.ActionID + "&UserId=" + model.UserId, "GET", null);
            exchange = exchange.JsonParseRecord(temp);
            List<OrderCancelReturn> exchange1 = (List<OrderCancelReturn>)exchange.Data;
            if (exchange1.Any())
            {
                exchange = exchange.AlreadyExists();
            }
            else
            {
                OrderCancelReturn orderCancel = new OrderCancelReturn();
                orderCancel.OrderID = model.OrderID;
                orderCancel.OrderItemID = model.OrderItemID;
                orderCancel.NewOrderNo = model.NewOrderNo;
                orderCancel.Qty = model.Qty;
                orderCancel.ActionID = model.ActionID;
                orderCancel.ExchangeProductID = model.ExchangeProductID;
                orderCancel.ExchangeSize = model.ExchangeSize;
                orderCancel.ExchangePriceDiff = model.ExchangePriceDiff;
                orderCancel.UserId = model.UserId;
                orderCancel.UserName = model.UserName;
                orderCancel.UserPhoneNo = model.UserPhoneNo;
                orderCancel.UserEmail = model.UserEmail;
                orderCancel.UserGSTNo = model.UserGSTNo;
                orderCancel.AddressLine1 = model.AddressLine1;
                orderCancel.AddressLine2 = model.AddressLine2;
                orderCancel.Landmark = model.Landmark;
                orderCancel.Pincode = model.Pincode;
                orderCancel.City = model.City;
                orderCancel.State = model.State;
                orderCancel.Country = model.Country;
                orderCancel.Issue = model.Issue;
                orderCancel.Reason = model.Reason;
                orderCancel.Comment = model.Comment;
                orderCancel.PaymentMode = model.PaymentMode;
                orderCancel.Attachment = model.Attachment;
                orderCancel.RefundAmount = model.RefundAmount;
                orderCancel.RefundType = model.RefundType;
                orderCancel.BankName = model.BankName;
                orderCancel.BankBranch = model.BankBranch;
                orderCancel.BankIFSCCode = model.BankIFSCCode;
                orderCancel.BankAccountNo = model.BankAccountNo;
                orderCancel.AccountType = model.AccountType;
                orderCancel.AccountHolderName = model.AccountHolderName;
                orderCancel.ApprovedByID = model.ApprovedByID;
                orderCancel.ApprovedByName = model.ApprovedByName;
                orderCancel.Status = model.Status;
                orderCancel.RefundStatus = model.RefundStatus;
                orderCancel.CreatedBy = UserId;
                orderCancel.CreatedAt = DateTime.Now;

                var updateRecord = helper.ApiCall(OrderURL, EndPoints.CancelReturn, "POST", orderCancel);
                exchange = exchange.JsonParseInputResponse(updateRecord);

                exchangeOrder(orderCancel.OrderID, orderCancel.OrderItemID);

            }
            return exchange;
        }

        public BaseResponse<OrderCancelReturn> ApprovedStatus(ApprovedStatusDto model)
        {
            BaseResponse<OrderCancelReturn> status = new BaseResponse<OrderCancelReturn>();

            var uStatus = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?orderId=" + model.orderId + "&orderItemId=" + model.orderItemId + "&actionId=" + model.actionId, "GET", null);
            status = status.JsonParseRecord(uStatus);
            OrderCancelReturn updateStatus = (OrderCancelReturn)status.Data;
            if (updateStatus == null)
            {
                status = status.NotExist();
            }
            else
            {
                updateStatus.Status = model.approvedRequest;
                updateStatus.ExchangeProductID = model.exchangeProductId;
                updateStatus.ExchangeSize = model.exchangeSize;
                updateStatus.ExchangePriceDiff = model.exchangePriceDiff;

                var updateRecord = helper.ApiCall(OrderURL, EndPoints.CancelReturn, "PUT", updateStatus);
                status = status.JsonParseInputResponse(updateRecord);

                switch (model.name)
                {
                    case "exchange":
                        exchangeOrder(model.orderId, model.orderItemId, Convert.ToInt32(model.exchangeProductId), Convert.ToInt32(model.exchangeSizeid), model.exchangeSize, model.exchangePriceDiff);
                        break;
                    case "replace":
                        replaceOrder(model.orderId, model.orderItemId);
                        break;
                    default:
                        return null;
                }
            }
            return status;
        }

        public BaseResponse<OrderItems> SaveOrderCancel(OrderCancelDto model)
        {
            BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
            string[] orderItemIds = model.OrderItemIds?.Trim('{', '}').Split(',');
            //UpdateOrderStatus(model.OrderId, orderItemIds, "Cancelled");

            //baseResponse.code = 200;
            //baseResponse.Message = "Order cancelled successfully";
            //baseResponse.Data = 0;

            BaseResponse<OrderCancelReturn> returnBaseResponse = new BaseResponse<OrderCancelReturn>();

            for (int i = 0; i < orderItemIds.Length; i++)
            {
                var temp = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?OrderID=" + model.OrderId + "&OrderItemID=" + orderItemIds[i] + "&ActionID=" + model.ActionID + "&UserId=" + model.UserId, "GET", null);
                returnBaseResponse = returnBaseResponse.JsonParseList(temp);
                List<OrderCancelReturn> orderRecord = (List<OrderCancelReturn>)returnBaseResponse.Data;
                if (orderRecord.Any())
                {
                    returnBaseResponse = returnBaseResponse.AlreadyExists();
                }
                else
                {
                    OrderCancelReturn orderCancelReturn = new OrderCancelReturn();

                    orderCancelReturn.OrderID = model.OrderId;
                    orderCancelReturn.OrderItemID = Convert.ToInt32(orderItemIds[i]);
                    orderCancelReturn.NewOrderNo = null;
                    orderCancelReturn.Qty = model.Qty;
                    orderCancelReturn.ActionID = model.ActionID;
                    orderCancelReturn.ExchangeProductID = null;
                    orderCancelReturn.ExchangeSize = null;
                    orderCancelReturn.ExchangePriceDiff = null;
                    orderCancelReturn.UserId = model.UserId;
                    orderCancelReturn.UserName = model.UserName;
                    orderCancelReturn.UserPhoneNo = model.UserPhoneNo;
                    orderCancelReturn.UserEmail = model.UserEmail;
                    orderCancelReturn.UserGSTNo = null;
                    orderCancelReturn.AddressLine1 = null;
                    orderCancelReturn.AddressLine2 = null;
                    orderCancelReturn.Landmark = null;
                    orderCancelReturn.Pincode = null;
                    orderCancelReturn.City = null;
                    orderCancelReturn.State = null;
                    orderCancelReturn.Country = null;
                    orderCancelReturn.Issue = model.Issue;
                    orderCancelReturn.Reason = model.Reason;
                    orderCancelReturn.Comment = model.Comment;
                    orderCancelReturn.PaymentMode = model.PaymentMode;
                    orderCancelReturn.Attachment = model.Attachment;
                    orderCancelReturn.RefundAmount = model.RefundAmount;
                    orderCancelReturn.RefundType = model.RefundType;
                    orderCancelReturn.BankName = model.BankName;
                    orderCancelReturn.BankBranch = model.BankBranch;
                    orderCancelReturn.BankIFSCCode = model.BankIFSCCode;
                    orderCancelReturn.BankAccountNo = model.BankAccountNo;
                    orderCancelReturn.AccountType = model.AccountType;
                    orderCancelReturn.AccountHolderName = model.AccountHolderName;
                    orderCancelReturn.ApprovedByID = null;
                    orderCancelReturn.ApprovedByName = null;
                    orderCancelReturn.Status = "Cancelled";
                    orderCancelReturn.RefundStatus = model.PaymentMode.ToLower() == "online" ? "Pending" : null;
                    orderCancelReturn.CreatedAt = DateTime.Now;
                    orderCancelReturn.CreatedBy = UserId;

                    var response = helper.ApiCall(OrderURL, EndPoints.CancelReturn, "POST", orderCancelReturn);
                    returnBaseResponse = returnBaseResponse.JsonParseInputResponse(response);
                    if (returnBaseResponse.code == 200)
                    {
                        string[] _orderItemIds = orderItemIds[i].ToString()?.Trim('{', '}').Split(',');
                        UpdateOrderStatus(model.OrderId, _orderItemIds, "Cancelled");

                        if (model.PaymentMode.ToLower() == "online")
                        {

                            OrderRefund refund = new OrderRefund();
                            refund.OrderID = model.OrderId;
                            refund.OrderItemID = Convert.ToInt32(orderItemIds[i]);
                            refund.OrderCancelReturnID = Convert.ToInt32(returnBaseResponse.Data);
                            refund.RefundAmount = Convert.ToDecimal(model.RefundAmount);
                            refund.TransactionID = "";
                            refund.Comment = "";
                            refund.Status = "In Process";
                            refund.CreatedBy = UserId;
                            refund.CreatedAt = DateTime.Now;

                            OrderRefundRequest(refund);
                        }

                        baseResponse.code = 200;
                        baseResponse.Data = 0;
                        baseResponse.Message = "Order cancelled successfully.";
                    }

                    //Sendmail(model.OrderId, "Cancelled", _httpContext.User.Claims.Where(x => x.Type.Equals("role")).FirstOrDefault().Value);
                    Sendmail(model.OrderId, "Cancelled", _httpContext.User.Claims.Where(x => x.Type.Equals(System.Security.Claims.ClaimTypes.Role)).FirstOrDefault().Value);
                }
            }

            return baseResponse;
        }

        public List<BaseResponse<OrderWiseExtraCharges>> OrderExtraChargesEntries(IEnumerable<OrderWiseExtraChargesDTO> orderWiseExtras, int orderId, int OrderItemId)
        {
            List<BaseResponse<OrderWiseExtraCharges>> orderTaxbases = new List<BaseResponse<OrderWiseExtraCharges>>();
            List<OrderWiseExtraCharges> ois = BindOrderWiseExtraCharges(orderWiseExtras, orderId, OrderItemId);

            foreach (var item in ois)
            {
                BaseResponse<OrderWiseExtraCharges> baseResponse = new BaseResponse<OrderWiseExtraCharges>();
                var response = helper.ApiCall(OrderURL, EndPoints.OrderWiseExtraCharges, "POST", item);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                orderTaxbases.Add(baseResponse);
            }

            return orderTaxbases;
        }


        [NonAction]
        public BaseResponse<OrderTrackDetails> OrderTrackEntries(int orderId, int orderItemId, string OrderStage, string OrderStatus, string OrderTrackDetail)
        {
            BaseResponse<OrderTrackDetails> bases = new BaseResponse<OrderTrackDetails>();
            OrderTrackDetails ois = BindOrderTrackDetails(orderId, orderItemId);

            BaseResponse<OrderTrackDetails> baseResponse = new BaseResponse<OrderTrackDetails>();

            OrderTrackDetails orderTrackDetails = new OrderTrackDetails();
            orderTrackDetails.OrderID = ois.OrderID;
            orderTrackDetails.OrderItemID = ois.OrderItemID;
            orderTrackDetails.OrderStage = OrderStage;
            orderTrackDetails.OrderStatus = OrderStatus;
            orderTrackDetails.OrderTrackDetail = OrderTrackDetail;
            orderTrackDetails.TrackDate = DateTime.Now;
            orderTrackDetails.CreatedBy = UserId;
            orderTrackDetails.CreatedAt = DateTime.Now;

            var response = helper.ApiCall(OrderURL, EndPoints.OrderTrackDetails, "POST", orderTrackDetails);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return baseResponse;
        }

        [NonAction]
        public BaseResponse<OrderTrackDetails> OrderTrackEntries(int orderId, int orderItemId, string OrderStatus)
        {
            BaseResponse<OrderTrackDetails> baseResponse = new BaseResponse<OrderTrackDetails>();

            OrderTrackDetails orderTrackDetails = new OrderTrackDetails();
            orderTrackDetails.OrderID = orderId;
            orderTrackDetails.OrderItemID = orderItemId;
            orderTrackDetails.OrderStage = OrderStatus;
            orderTrackDetails.OrderStatus = "processing " + OrderStatus;
            orderTrackDetails.OrderTrackDetail = "Order is in " + OrderStatus;
            orderTrackDetails.TrackDate = DateTime.Now;
            orderTrackDetails.CreatedBy = UserId;
            orderTrackDetails.CreatedAt = DateTime.Now;

            var response = helper.ApiCall(OrderURL, EndPoints.OrderTrackDetails, "POST", orderTrackDetails);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return baseResponse;
        }

        [NonAction]
        public BaseResponse<OrderItems> UpdateOrderStatus(int OrderId, string[] OrderItemId, string Status, int? WarehouseId = 0)
        {
            BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
            var temp = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + OrderId, "Get", null);
            baseResponse = baseResponse.JsonParseList(temp);
            if (baseResponse.code == 200)
            {
                List<OrderItems> orderItemslst = (List<OrderItems>)baseResponse.Data;

                foreach (var item in OrderItemId)
                {
                    //var temp = helper.ApiCall(URL, EndPoints.OrderItems + "?OrderId=" + OrderId + "&Id=" + item, "Get", null);
                    //baseResponse = baseResponse.JsonParseRecord(temp);
                    //OrderItems orderRecord = (OrderItems)baseResponse.Data;
                    OrderItems orderRecord = orderItemslst.Where(p => p.Id == Convert.ToInt32(item)).FirstOrDefault();
                    if (orderRecord != null && orderRecord.Id != 0)
                    {
                        //OrderItems record = (OrderItems)baseResponse.Data;
                        OrderItems record = orderItemslst.Where(p => p.Id == Convert.ToInt32(item)).FirstOrDefault();
                        record.Status = Status;
                        if (Status.ToLower() == "delivered")
                        {
                            record.ReturnValidTillDate = record.ReturnValidDays != null ? DateTime.Now.AddDays(Convert.ToInt32(record.ReturnValidDays)) : DateTime.Now.AddDays(0);
                        }
                        record.ModifiedBy = UserId;
                        record.ModifiedAt = DateTime.Now;
                        var response = helper.ApiCall(OrderURL, EndPoints.OrderItems, "PUT", record);
                        baseResponse = baseResponse.JsonParseInputResponse(response);

                        OrderTrackEntries(OrderId, orderRecord.Id, record.Status);

                        if (Status.ToLower() == "cancelled" || Status.ToLower() == "returned" || Status.ToLower() == "exchanged")
                        {
                            bool AllowWarehouseManagement = Convert.ToBoolean(_configuration.GetValue<string>("Allow_warehouse_management"));
                            ProductWareHouse _productWarehouse = new ProductWareHouse();
                            if (AllowWarehouseManagement)
                            {
                                if (Status.ToLower() == "returned")
                                {
                                    orderRecord.WherehouseId = Convert.ToInt32(WarehouseId);
                                }

                                if (orderRecord.WherehouseId != null)
                                {

                                    BaseResponse<ProductWareHouse> productWarehouse = new BaseResponse<ProductWareHouse>();
                                    var resp = helper.ApiCall(_catlogUrl, EndPoints.ProductWarehouse + "?id=" + orderRecord.WherehouseId, "GET", null);
                                    productWarehouse = productWarehouse.JsonParseRecord(resp);
                                    if (productWarehouse.code == 200)
                                    {
                                        _productWarehouse = (ProductWareHouse)productWarehouse.Data;
                                        if (_productWarehouse.Id != null || _productWarehouse.Id != 0)
                                        {

                                            int orderqty = Convert.ToInt32(_productWarehouse.ProductQuantity) + Convert.ToInt32(orderRecord.Qty);
                                            _productWarehouse.ProductQuantity = orderqty;
                                            var productWarehouseresponse = helper.ApiCall(_catlogUrl, EndPoints.ProductWarehouse, "PUT", _productWarehouse);
                                            productWarehouse = productWarehouse.JsonParseInputResponse(productWarehouseresponse);

                                        }
                                    }
                                }
                            }

                            BaseResponse<ProductPrice> PbaseResponse = new BaseResponse<ProductPrice>();
                            string url = string.Empty;
                            if (orderRecord.SizeID != null)
                            {
                                url = "&SizeID=" + orderRecord.SizeID;
                            }

                            var presponse = helper.ApiCall(_catlogUrl, EndPoints.ProductPriceMaster + "?SellerProductID=" + orderRecord.SellerProductID + url, "get", null);
                            PbaseResponse = PbaseResponse.JsonParseList(presponse);
                            List<ProductPrice> DataProductPrices = (List<ProductPrice>)PbaseResponse.Data;
                            if (DataProductPrices.Count > 0)
                            {
                                ProductPrice PP = new ProductPrice();
                                PP = DataProductPrices[0];
                                PP.Quantity = PP.Quantity + Convert.ToInt32(orderRecord.Qty);
                                var priceresponse = helper.ApiCall(_catlogUrl, EndPoints.ProductPriceMaster, "PUT", PP);
                            }
                        }
                    }
                    else
                    {
                        baseResponse = baseResponse.NotExist();
                    }
                }

                UpdateMainOrderStstus(orderItemslst, OrderId);
            }



            return baseResponse;
        }

        [NonAction]
        public BaseResponse<Orders> UpdateMainOrderStstus(List<OrderItems> orderItems, int orderId)
        {
            var orderStatus = orderItems.Select(p => p.Status).Distinct().ToList();
            string status = string.Empty;
            if (orderStatus.Count > 0)
            {
                var _orderStatus = orderStatus.Where(p => p.ToLower() != "cancelled" && p.ToLower() != "returned" && p.ToLower() != "return rejected" && p.ToLower() != "return requested" && p.ToLower() != "replaced" && p.ToLower() != "replace rejected" && p.ToLower() != "replace requested" && p.ToLower() != "exchanged" && p.ToLower() != "exchange rejected" && p.ToLower() != "exchange requested").ToList();
                if (_orderStatus.Count > 0)
                {
                    var delivered = _orderStatus.Where(p => p.ToLower() == "delivered").ToList();
                    if (delivered.Count > 0)
                    {
                        if (delivered.Count == _orderStatus.Count)
                        {
                            status = "Delivered";
                        }
                        else
                        {
                            status = "Partial Delivered";
                        }
                    }
                    else
                    {
                        var shipped = _orderStatus.Where(p => p.ToLower() == "shipped").ToList();
                        if (shipped.Count > 0)
                        {
                            if (shipped.Count == _orderStatus.Count)
                            {
                                status = "Shipped";
                            }
                            else
                            {
                                status = "Partial Shipped";
                            }
                        }
                        else
                        {
                            var packed = _orderStatus.Where(p => p.ToLower() == "packed").ToList();
                            if (packed.Count > 0)
                            {
                                if (packed.Count == _orderStatus.Count)
                                {
                                    status = "Packed";
                                }
                                else
                                {
                                    status = "Partial Packed";
                                }
                            }
                            else
                            {
                                var confirmed = _orderStatus.Where(p => p.ToLower() == "confirmed").ToList();
                                if (confirmed.Count > 0)
                                {
                                    if (confirmed.Count == _orderStatus.Count)
                                    {
                                        status = "Confirmed";
                                    }
                                    else
                                    {
                                        status = "Partial Confirmed";
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var Replaced = orderStatus.Where(p => p.ToLower() == "replaced").ToList();
                    var replaceRequested = orderStatus.Where(p => p.ToLower() == "replace requested").ToList();
                    var replaceRejected = orderStatus.Where(p => p.ToLower() == "replace rejected").ToList();
                    var returned = orderStatus.Where(p => p.ToLower() == "returned").ToList();
                    var returnRequested = orderStatus.Where(p => p.ToLower() == "return requested").ToList();
                    var returnRejected = orderStatus.Where(p => p.ToLower() == "return rejected").ToList();
                    var exchanged = orderStatus.Where(p => p.ToLower() == "exchanged").ToList();
                    var exchangeRequested = orderStatus.Where(p => p.ToLower() == "exchange requested").ToList();
                    var exchangeRejected = orderStatus.Where(p => p.ToLower() == "exchange rejected").ToList();
                    var cancelled = orderStatus.Where(p => p.ToLower() == "cancelled").ToList();
                    if (Replaced.Count > 0)
                    {
                        if (Replaced.Count == orderStatus.Count)
                        {
                            status = "Replaced";
                        }
                        else
                        {
                            status = "Delivered";
                        }
                    }
                    else if (replaceRequested.Count > 0)
                    {
                        if (replaceRequested.Count == orderStatus.Count)
                        {
                            status = "Replace Requested";
                        }
                        else
                        {
                            status = "Delivered";
                        }
                    }
                    else if (replaceRejected.Count > 0)
                    {
                        if (replaceRejected.Count == orderStatus.Count)
                        {
                            status = "Replace Rejected";
                        }
                        else
                        {
                            status = "Delivered";
                        }
                    }
                    else if (returnRequested.Count > 0)
                    {
                        if (returnRequested.Count == orderStatus.Count)
                        {
                            status = "Return Requested";
                        }
                        else
                        {
                            status = "Delivered";
                        }
                    }
                    else if (returnRejected.Count > 0)
                    {
                        if (returnRejected.Count == orderStatus.Count)
                        {
                            status = "Return Rejected";
                        }
                        else
                        {
                            status = "Delivered";
                        }
                    }
                    else if (returned.Count > 0)
                    {
                        if (returned.Count == orderStatus.Count)
                        {
                            status = "Returned";
                        }
                        else
                        {
                            status = "Delivered";
                        }
                    }
                    else if (exchanged.Count > 0)
                    {
                        if (exchanged.Count == orderStatus.Count)
                        {
                            status = "Exchanged";
                        }
                        else
                        {
                            status = "Delivered";
                        }
                    }
                    else if (exchangeRequested.Count > 0)
                    {
                        if (exchangeRequested.Count == orderStatus.Count)
                        {
                            status = "Exchange Requested";
                        }
                        else
                        {
                            status = "Delivered";
                        }
                    }
                    else if (exchangeRejected.Count > 0)
                    {
                        if (exchangeRejected.Count == orderStatus.Count)
                        {
                            status = "Exchange Rejected";
                        }
                        else
                        {
                            status = "Delivered";
                        }
                    }
                    else
                    {
                        if (cancelled.Count > 0)
                        {
                            if (cancelled.Count == orderStatus.Count)
                            {
                                status = "Cancelled";
                            }
                        }
                    }


                }
            }

            BaseResponse<Orders> baseResponse = new BaseResponse<Orders>();
            if (!string.IsNullOrEmpty(status))
            {
                var temp = helper.ApiCall(OrderURL, EndPoints.Orders + "?Id=" + orderId, "Get", null);
                baseResponse = baseResponse.JsonParseRecord(temp);
                if (baseResponse.code == 200)
                {
                    Orders order = (Orders)baseResponse.Data;
                    order.Status = status;
                    order.ModifiedBy = UserId;
                    order.ModifiedAt = DateTime.Now;
                    var response = helper.ApiCall(OrderURL, EndPoints.Orders, "PUT", order);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            return baseResponse;
        }

        [NonAction]
        public BaseResponse<OrderInvoice> SaveInvoice(OrderShipmentInfo shipModel)
        {
            BaseResponse<OrderInvoice> InvoicebaseResponse = new BaseResponse<OrderInvoice>();
            BaseResponse<Orders> ordersBaseResponse = new BaseResponse<Orders>();
            BaseResponse<OrderPackage> packageBaseResponse = new BaseResponse<OrderPackage>();
            BaseResponse<GSTInfo> gstInfoResponse = new BaseResponse<GSTInfo>();

            string SellerId = string.Empty;

            //var orderdetail = helper.ApiCall(URL, EndPoints.Orders + "?Id=" + shipModel.OrderID, "GET", null);
            //ordersBaseResponse = ordersBaseResponse.JsonParseRecord(orderdetail);
            //Orders orderRecord = (Orders)ordersBaseResponse.Data;
            //SellerId = orderRecord.SellerID;
            SellerId = shipModel.SellerID;

            var orderPackage = helper.ApiCall(OrderURL, EndPoints.OrderPackage + "?Id=" + shipModel.PackageID, "GET", null);
            packageBaseResponse = packageBaseResponse.JsonParseRecord(orderPackage);
            OrderPackage package = (OrderPackage)packageBaseResponse.Data;

            var gstDetail = helper.ApiCall(UserUrl, EndPoints.GSTInfo + "?UserID=" + SellerId + "&IsHeadOffice=" + true, "GET", null);
            gstInfoResponse = gstInfoResponse.JsonParseRecord(gstDetail);
            GSTInfo gst = (GSTInfo)gstInfoResponse.Data;

            OrderInvoice orderInvoice = new OrderInvoice();
            orderInvoice.SellerTradeName = gst.TradeName;
            orderInvoice.PackageID = shipModel.PackageID;
            orderInvoice.OrderID = shipModel.OrderID;
            orderInvoice.OrderItemIDs = package.OrderItemIDs;
            orderInvoice.InvoiceNo = "Inv_ord_" + shipModel.OrderID + "_" + shipModel.PackageID;
            orderInvoice.SellerTradeName = gst.TradeName;
            orderInvoice.SellerLegalName = gst.LegalName;
            orderInvoice.SellerGSTNo = gst.GSTNo;
            orderInvoice.SellerRegisteredAddressLine1 = gst.RegisteredAddressLine1;
            orderInvoice.SellerRegisteredAddressLine2 = gst.RegisteredAddressLine2;
            orderInvoice.SellerRegisteredLandmark = gst.RegisteredLandmark;
            orderInvoice.SellerRegisteredPincode = Convert.ToInt32(gst.RegisteredPincode);
            orderInvoice.SellerRegisteredCity = gst.CityName;
            orderInvoice.SellerRegisteredState = gst.StateName;
            orderInvoice.SellerRegisteredCountry = gst.CountryName;
            orderInvoice.SellerPickupAddressLine1 = shipModel.PickupAddressLine1;
            orderInvoice.SellerPickupAddressLine2 = shipModel.PickupAddressLine2;
            orderInvoice.SellerPickupLandmark = shipModel.PickupLandmark;
            orderInvoice.SellerPickupPincode = shipModel.PickupPincode;
            orderInvoice.SellerPickupCity = shipModel.PickupCity;
            orderInvoice.SellerPickupState = shipModel.PickupState;
            orderInvoice.SellerPickupCountry = shipModel.PickupCountry;
            orderInvoice.SellerPickupContactPersonName = shipModel.PickupContactPersonName;
            orderInvoice.SellerPickupContactPersonMobileNo = shipModel.PickupContactPersonMobileNo;
            orderInvoice.SellerPickupTaxNo = shipModel.PickupTaxNo;
            orderInvoice.InvoiceAmount = shipModel.InvoiceAmount;
            orderInvoice.InvoiceCodCharges = shipModel.InvoiceCodCharges;
            orderInvoice.Status = "Due";
            orderInvoice.CreatedBy = UserId;
            orderInvoice.CreatedAt = DateTime.Now;

            var saveinvoice = helper.ApiCall(OrderURL, EndPoints.Invoice, "POST", orderInvoice);
            InvoicebaseResponse = InvoicebaseResponse.JsonParseInputResponse(saveinvoice);
            return InvoicebaseResponse;
        }

        [NonAction]
        public BaseResponse<ReturnShipmentInfo> ReturnShipmentInfo(OrderCancelReturn model, int orderCancelReturnId)
        {
            BaseResponse<ReturnShipmentInfo> returnShipMentResponse = new BaseResponse<ReturnShipmentInfo>();
            BaseResponse<SellerProduct> sellerProductResponse = new BaseResponse<SellerProduct>();
            BaseResponse<OrderItems> orderItemResponse = new BaseResponse<OrderItems>();
            BaseResponse<OrderShipmentInfo> orderShipResponse = new BaseResponse<OrderShipmentInfo>();

            int SellerProductID = 0;
            int? ProductID = 0;
            try
            {
                // api call for getting SellerProductId and ProductId.
                var orderdetail = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?orderId=" + model.OrderID + "&OrderItemID=" + model.OrderItemID, "GET", null);
                orderItemResponse = orderItemResponse.JsonParseRecord(orderdetail);
                OrderItems orderRecord = (OrderItems)orderItemResponse.Data;
                SellerProductID = orderRecord.SellerProductID;
                ProductID = orderRecord.ProductID;
                try
                {
                    // api call for getting sellerProduct Detail.
                    var sellerproduct = helper.ApiCall(_catlogUrl, EndPoints.SellerProduct + "?Id=" + SellerProductID + "&ProductID=" + ProductID, "GET", null);
                    sellerProductResponse = sellerProductResponse.JsonParseRecord(sellerproduct);
                    SellerProduct sellerProduct = (SellerProduct)sellerProductResponse.Data;
                    try
                    {
                        // api call for getting orderShipmnet Detail.
                        var orderShip = helper.ApiCall(OrderURL, EndPoints.ShipmentInfo + "?OrderID=" + model.OrderID + "&OrderItemID=" + model.OrderItemID, "GET", null);
                        orderShipResponse = orderShipResponse.JsonParseRecord(orderShip);
                        OrderShipmentInfo orderShipment = (OrderShipmentInfo)orderShipResponse.Data;

                        var temp = helper.ApiCall(OrderURL, EndPoints.ReturnShipmentInfo + "?OrderId=" + model.OrderID + "&OrderItemId=" + model.OrderItemID + "&OrderCancelReturnID=" + model.Id, "GET", null);
                        returnShipMentResponse = returnShipMentResponse.JsonParseRecord(temp);
                        List<ReturnShipmentInfo> orderRefundRecord = (List<ReturnShipmentInfo>)returnShipMentResponse.Data;
                        if (orderRefundRecord.Any())
                        {
                            returnShipMentResponse = returnShipMentResponse.AlreadyExists();
                        }
                        else
                        {
                            ReturnShipmentInfo returnShipment = new ReturnShipmentInfo();
                            returnShipment.OrderID = model.OrderID;
                            returnShipment.OrderItemID = model.OrderItemID;
                            returnShipment.OrderCancelReturnID = orderCancelReturnId;

                            returnShipment.Qty = model.Qty;
                            returnShipment.PaymentMode = "Online";
                            returnShipment.Length = Convert.ToString(sellerProduct.PackingLength);
                            returnShipment.Width = Convert.ToString(sellerProduct.PackingBreadth);
                            returnShipment.Height = Convert.ToString(sellerProduct.PackingHeight);
                            returnShipment.Weight = Convert.ToString(sellerProduct.PackingWeight);
                            returnShipment.ReturnValueAmount = orderRecord.TotalAmount;
                            returnShipment.PackageDescription = model.CustomeProductName;
                            returnShipment.IsShipmentInitiate = false;
                            returnShipment.IsPaymentSuccess = false;
                            returnShipment.CourierID = null;
                            returnShipment.ServiceID = null;
                            returnShipment.ServiceType = null;
                            returnShipment.PickupContactPersonName = model.UserName;
                            returnShipment.PickupContactPersonMobileNo = model.UserPhoneNo;
                            returnShipment.PickupContactPersonEmailID = model.UserEmail;
                            returnShipment.PickupCompanyName = null;
                            returnShipment.PickupAddressLine1 = model.AddressLine1;
                            returnShipment.PickupAddressLine2 = model.AddressLine2;
                            returnShipment.PickupLandmark = model.Landmark;
                            returnShipment.PickupPincode = (int)model.Pincode;
                            returnShipment.PickupCity = model.City;
                            returnShipment.PickupState = model.State;
                            returnShipment.PickupCountry = model.Country;
                            returnShipment.DropContactPersonName = model.DropContactPersonName;
                            returnShipment.DropContactPersonMobileNo = model.DropContactPersonMobileNo;
                            returnShipment.DropContactPersonEmailID = model.DropContactPersonEmailID;
                            returnShipment.DropCompanyName = model.DropCompanyName;
                            returnShipment.DropAddressLine1 = model.DropAddressLine1;
                            returnShipment.DropAddressLine2 = model.DropAddressLine2;
                            returnShipment.DropLandmark = model.DropLandmark;
                            returnShipment.DropPincode = Convert.ToInt32(model.DropPincode);
                            returnShipment.DropCity = model.DropCity;
                            returnShipment.DropState = model.DropState;
                            returnShipment.DropCountry = model.DropCountry;
                            // passing null here due to 3rd paty integration.
                            returnShipment.ShipmentID = model.ShipmentID;
                            returnShipment.ShipmentOrderID = model.ShipmentOrderID;
                            returnShipment.ShippingPartner = model.ShippingPartner;
                            returnShipment.CourierName = model.CourierName;
                            returnShipment.ShippingAmountFromPartner = model.ShippingAmountFromPartner;
                            returnShipment.AwbNo = model.AwbNo;
                            returnShipment.IsShipmentSheduledByAdmin = model.IsShipmentSheduledByAdmin;
                            returnShipment.PickupLocationID = model.PickupLocationID;
                            returnShipment.ErrorMessage = model.ErrorMessage;
                            returnShipment.ForwardLable = model.ForwardLable;
                            returnShipment.ReturnLable = model.ReturnLable;
                            returnShipment.CreatedBy = UserId;
                            returnShipment.CreatedAt = DateTime.Now;

                            var response = helper.ApiCall(OrderURL, EndPoints.ReturnShipmentInfo, "POST", returnShipment);
                            returnShipMentResponse = returnShipMentResponse.JsonParseInputResponse(response);

                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception("Error in getting orderShipment detail: " + ex.Message);
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception("Error in getting sellerProduct detail: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getting order details: " + ex.Message);
            }

            return returnShipMentResponse;
        }

        [NonAction]
        public BaseResponse<Orders> exchangeOrder(int orderId, int OrderItemId, int newProudctId = 0, int newSizeId = 0, string? ExchangeSize = null, decimal? ExchangePriceDiff = 0)
        {
            BaseResponse<Orders> orderBaseResponse = new BaseResponse<Orders>();

            var getOrder = helper.ApiCall(OrderURL, EndPoints.Orders + "?Id=" + orderId, "GET", null);
            orderBaseResponse = orderBaseResponse.JsonParseRecord(getOrder);
            Orders order = (Orders)orderBaseResponse.Data;
            if (order == null)
            {
                orderBaseResponse = orderBaseResponse.AlreadyExists();
            }
            else
            {
                Orders fillOrder = new Orders();

                fillOrder.Id = order.Id;
                fillOrder.Guid = Guid.NewGuid().ToString();
                fillOrder.OrderNo = Guid.NewGuid().ToString();
                //fillOrder.SellerID = order.SellerID;
                fillOrder.UserId = order.UserId;
                fillOrder.UserName = order.UserName;
                fillOrder.UserPhoneNo = order.UserPhoneNo;
                fillOrder.UserEmail = order.UserEmail;
                fillOrder.UserAddressLine1 = order.UserAddressLine1;
                fillOrder.UserAddressLine2 = order.UserAddressLine2;
                fillOrder.UserLandmark = order.UserLandmark;
                fillOrder.UserPincode = order.UserPincode;
                fillOrder.UserCity = order.UserCity;
                fillOrder.UserState = order.UserState;
                fillOrder.UserCountry = order.UserCountry;
                fillOrder.UserGSTNo = order.UserGSTNo;
                fillOrder.PaymentMode = "Online";
                fillOrder.TotalShippingCharge = order.TotalShippingCharge;
                fillOrder.TotalExtraCharges = order.TotalExtraCharges;
                fillOrder.TotalAmount = order.TotalAmount;
                fillOrder.IsCouponApplied = order.IsCouponApplied;
                fillOrder.Coupon = order.Coupon;
                fillOrder.CoupontDiscount = order.CoupontDiscount;
                fillOrder.CoupontDetails = order.CoupontDetails;
                fillOrder.CODCharge = order.CODCharge;
                fillOrder.PaidAmount = 0;
                fillOrder.IsSale = order.IsSale;
                fillOrder.SaleType = order.SaleType;
                fillOrder.OrderDate = DateTime.Now;
                fillOrder.DeliveryDate = DateTime.Now.AddDays(7);
                fillOrder.Status = "Confirmed";
                fillOrder.PaymentInfo = order.PaymentInfo;
                fillOrder.OrderBy = order.OrderBy;
                fillOrder.IsRetailer = order.IsRetailer;
                fillOrder.IsVertualRetailer = order.IsVertualRetailer;
                fillOrder.IsReplace = true;
                fillOrder.ParentID = orderId;
                fillOrder.CreatedBy = UserId;
                fillOrder.CreatedAt = DateTime.Now;

                var createOrder = helper.ApiCall(OrderURL, EndPoints.Orders, "POST", fillOrder);
                orderBaseResponse = orderBaseResponse.JsonParseInputResponse(createOrder);

                int newOrderId = (int)orderBaseResponse.Data;
                exchangeOrderItem(newOrderId, orderId, OrderItemId, newProudctId, newSizeId, ExchangeSize, ExchangePriceDiff);
            }
            return orderBaseResponse;
        }

        [NonAction]
        public BaseResponse<OrderItems> exchangeOrderItem(int newOrderId, int orderId, int orderItemId, int newProductId = 0, int newSizeId = 0, string? ExchangeSize = null, decimal? ExchangePriceDiff = 0)
        {
            BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
            var OrderItem = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?Id=" + orderItemId + "&orderId=" + orderId, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(OrderItem);
            OrderItems orderItems = (OrderItems)baseResponse.Data;
            if (orderItems == null)
            {
                baseResponse = baseResponse.NotExist();
            }
            else
            {
                OrderItems item = new OrderItems();
                item.OrderID = newOrderId;
                item.Guid = Guid.NewGuid().ToString();
                item.SubOrderNo = Guid.NewGuid().ToString();
                item.SellerID = orderItems.SellerID;
                item.BrandID = orderItems.BrandID;
                item.ProductID = newProductId;
                item.SellerProductID = orderItems.SellerProductID;
                item.ProductName = orderItems.ProductName;
                item.ProductSKUCode = orderItems.ProductSKUCode;
                item.MRP = orderItems.MRP;
                item.SellingPrice = orderItems.SellingPrice;
                item.Discount = orderItems.Discount;
                item.Qty = orderItems.Qty;
                item.TotalAmount = orderItems.TotalAmount;
                item.PriceTypeID = orderItems.PriceTypeID;
                item.PriceType = orderItems.PriceType;
                item.SizeID = orderItems.SizeID;
                item.SizeValue = ExchangeSize;
                item.IsCouponApplied = orderItems.IsCouponApplied;
                item.Coupon = orderItems.Coupon;
                item.CoupontDiscount = orderItems.CoupontDiscount;
                item.CoupontDetails = orderItems.CoupontDetails;
                item.ShippingZone = orderItems.ShippingZone;
                item.ShippingCharge = 0;
                item.ShippingChargePaidBy = orderItems.ShippingChargePaidBy;
                item.SubTotal = 0;
                item.Status = "Confirmed";
                item.WherehouseId = orderItems.WherehouseId;
                item.IsReplace = orderItems.IsReplace;
                item.ParentID = orderItemId;
                item.ReturnPolicyName = orderItems.ReturnPolicyName;
                item.ReturnPolicyTitle = orderItems.ReturnPolicyTitle;
                item.ReturnPolicyCovers = orderItems.ReturnPolicyCovers;
                item.ReturnPolicyDescription = orderItems.ReturnPolicyDescription;
                item.ReturnValidDays = orderItems.ReturnValidDays;
                item.ReturnValidTillDate = orderItems.ReturnValidTillDate;
                item.CreatedBy = UserId;
                item.CreatedAt = DateTime.Now;

                var exchangeOrderItem = helper.ApiCall(OrderURL, EndPoints.OrderItems, "POST", item);
                baseResponse = baseResponse.JsonParseInputResponse(exchangeOrderItem);

                orderItemId = (int)baseResponse.Data;
                OrderTrackEntries(item.OrderID, orderItemId, item.Status);
                ReplaceOrdertaxinfo(orderItems.OrderID, orderItems.Id, newOrderId, orderItemId);
            }
            return baseResponse;
        }

        [NonAction]
        public BaseResponse<Orders> replaceOrder(int orderId, int OrderItemId)
        {
            BaseResponse<Orders> orderBaseResponse = new BaseResponse<Orders>();

            var getOrder = helper.ApiCall(OrderURL, EndPoints.Orders + "?Id=" + orderId, "GET", null);
            orderBaseResponse = orderBaseResponse.JsonParseRecord(getOrder);
            Orders order = (Orders)orderBaseResponse.Data;
            if (order == null)
            {
                orderBaseResponse = orderBaseResponse.NotExist();
            }
            else
            {
                Orders fillOrder = new Orders();

                fillOrder.Id = order.Id;
                fillOrder.Guid = Guid.NewGuid().ToString();
                fillOrder.OrderNo = Guid.NewGuid().ToString();
                //fillOrder.SellerID = order.SellerID;
                fillOrder.UserId = order.UserId;
                fillOrder.UserName = order.UserName;
                fillOrder.UserPhoneNo = order.UserPhoneNo;
                fillOrder.UserEmail = order.UserEmail;
                fillOrder.UserAddressLine1 = order.UserAddressLine1;
                fillOrder.UserAddressLine2 = order.UserAddressLine2;
                fillOrder.UserLandmark = order.UserLandmark;
                fillOrder.UserPincode = order.UserPincode;
                fillOrder.UserCity = order.UserCity;
                fillOrder.UserState = order.UserState;
                fillOrder.UserCountry = order.UserCountry;
                fillOrder.UserGSTNo = order.UserGSTNo;
                fillOrder.PaymentMode = "Online";
                fillOrder.TotalShippingCharge = order.TotalShippingCharge;
                fillOrder.TotalExtraCharges = order.TotalExtraCharges;
                fillOrder.TotalAmount = order.TotalAmount;
                fillOrder.IsCouponApplied = order.IsCouponApplied;
                fillOrder.Coupon = order.Coupon;
                fillOrder.CoupontDiscount = order.CoupontDiscount;
                fillOrder.CoupontDetails = order.CoupontDetails;
                fillOrder.CODCharge = 0;
                fillOrder.PaidAmount = 0;
                fillOrder.IsSale = order.IsSale;
                fillOrder.SaleType = order.SaleType;
                fillOrder.OrderDate = DateTime.Now;
                fillOrder.DeliveryDate = DateTime.Now.AddDays(7);
                fillOrder.Status = "Confirmed";
                fillOrder.PaymentInfo = order.PaymentInfo;
                fillOrder.OrderBy = order.OrderBy;
                fillOrder.IsRetailer = order.IsRetailer;
                fillOrder.IsVertualRetailer = order.IsVertualRetailer;
                fillOrder.IsReplace = true;
                fillOrder.ParentID = orderId;
                fillOrder.CreatedBy = UserId;
                fillOrder.CreatedAt = DateTime.Now;

                var createOrder = helper.ApiCall(OrderURL, EndPoints.Orders, "POST", fillOrder);
                orderBaseResponse = orderBaseResponse.JsonParseInputResponse(createOrder);

                int newOrderId = (int)orderBaseResponse.Data;
                ReplaceOrderItem(newOrderId, OrderItemId);
            }
            return orderBaseResponse;
        }

        [NonAction]
        public BaseResponse<OrderItems> ReplaceOrderItem(int OrderId, int OrderItemId)
        {
            BaseResponse<OrderItems> orderItemBaseResponse = new BaseResponse<OrderItems>();

            var getOrderItems = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?Id=" + OrderItemId, "GET", null);
            orderItemBaseResponse = orderItemBaseResponse.JsonParseRecord(getOrderItems);
            OrderItems orderItems = (OrderItems)orderItemBaseResponse.Data;
            if (orderItems == null)
            {
                orderItemBaseResponse = orderItemBaseResponse.NotExist();
            }
            else
            {
                OrderItems items = new OrderItems();

                items.Id = orderItems.Id;
                items.OrderID = OrderId;
                items.Guid = Guid.NewGuid().ToString();
                items.SubOrderNo = Guid.NewGuid().ToString();
                items.SellerID = orderItems.SellerID;
                items.BrandID = orderItems.BrandID;
                items.ProductID = orderItems.ProductID;
                items.SellerProductID = orderItems.SellerProductID;
                items.ProductName = orderItems.ProductName;
                items.ProductSKUCode = orderItems.ProductSKUCode;
                items.MRP = orderItems.MRP;
                items.SellingPrice = orderItems.SellingPrice;
                items.Discount = orderItems.Discount;
                items.Qty = orderItems.Qty;
                items.TotalAmount = orderItems.TotalAmount;
                items.PriceTypeID = orderItems.PriceTypeID;
                items.PriceType = orderItems.PriceType;
                items.SizeID = orderItems.SizeID;
                items.SizeValue = orderItems.SizeValue;
                items.IsCouponApplied = orderItems.IsCouponApplied;
                items.Coupon = orderItems.Coupon;
                items.CoupontDiscount = orderItems.CoupontDiscount;
                items.CoupontDetails = orderItems.CoupontDetails;

                items.ShippingZone = orderItems.ShippingZone;
                items.ShippingCharge = 0;
                items.ShippingChargePaidBy = orderItems.ShippingChargePaidBy;

                items.SubTotal = 0;
                items.Status = "Confirmed";
                items.WherehouseId = orderItems.WherehouseId;
                items.IsReplace = true;
                items.ParentID = OrderItemId;

                items.ReturnPolicyName = orderItems.ReturnPolicyName;
                items.ReturnPolicyTitle = orderItems.ReturnPolicyTitle;
                items.ReturnPolicyCovers = orderItems.ReturnPolicyCovers;
                items.ReturnPolicyDescription = orderItems.ReturnPolicyDescription;
                items.ReturnValidDays = orderItems.ReturnValidDays;
                items.ReturnValidTillDate = orderItems.ReturnValidTillDate;

                items.CreatedBy = UserId;
                items.CreatedAt = DateTime.Now;

                var createOrderItem = helper.ApiCall(OrderURL, EndPoints.OrderItems, "POST", items);
                orderItemBaseResponse = orderItemBaseResponse.JsonParseInputResponse(createOrderItem);


                int orderItemId = (int)orderItemBaseResponse.Data;

                OrderTrackEntries(items.OrderID, orderItemId, items.Status);
                ReplaceOrdertaxinfo(orderItems.OrderID, orderItems.Id, OrderId, orderItemId);

            }
            return orderItemBaseResponse;

        }

        [NonAction]
        public BaseResponse<OrderTaxInfo> ReplaceOrdertaxinfo(int OrderId, int OrderItemId, int NewOrderId, int NewOrderItemId)
        {
            BaseResponse<OrderTaxInfo> modelResponse = new BaseResponse<OrderTaxInfo>();

            var getTaxInfo = helper.ApiCall(OrderURL, EndPoints.OrderTaxInfo + "?OrderId=" + OrderId + "&OrderItemId=" + OrderItemId, "GET", null);
            modelResponse = modelResponse.JsonParseRecord(getTaxInfo);
            OrderTaxInfo orderTax = (OrderTaxInfo)modelResponse.Data;
            if (orderTax == null)
            {
                modelResponse = modelResponse.NotExist();
            }
            else
            {
                orderTax.OrderID = NewOrderId;
                orderTax.OrderItemID = NewOrderItemId;
                orderTax.CreatedBy = UserId;
                orderTax.CreatedAt = DateTime.Now;

                var callTaxInfo = helper.ApiCall(OrderURL, EndPoints.OrderTaxInfo, "POST", orderTax);
                modelResponse = modelResponse.JsonParseInputResponse(callTaxInfo);

            }
            return modelResponse;

        }

        public ReturnPolicyDTO BindReturnPolicy(int Categoryid)
        {
            List<CategoryLibrary> categoryLists = GetCategoryWithParent(Categoryid, _catlogUrl);
            ReturnPolicyDTO returnPolicy = new ReturnPolicyDTO();
            BaseResponse<AssignReturnPolicyToCatagoryLibrary> baseResponse = new BaseResponse<AssignReturnPolicyToCatagoryLibrary>();

            if (categoryLists != null && categoryLists.Count > 0)
            {
                categoryLists = categoryLists.OrderByDescending(p => p.Id).ToList();
                foreach (var item in categoryLists)
                {
                    var response = helper.ApiCall(_catlogUrl, EndPoints.AssignReturnPolicyToCatagory + "?CategoryID=" + item.Id, "GET", null);
                    baseResponse = baseResponse.JsonParseRecord(response);
                    AssignReturnPolicyToCatagoryLibrary policy = new AssignReturnPolicyToCatagoryLibrary();
                    if (baseResponse.code == 200)
                    {
                        policy = (AssignReturnPolicyToCatagoryLibrary)baseResponse.Data;
                        returnPolicy.Id = policy.Id;
                        returnPolicy.ReturnPolicyID = Convert.ToInt32(policy.ReturnPolicyID);
                        returnPolicy.ValidityDays = Convert.ToInt32(policy.ValidityDays);
                        returnPolicy.Title = policy.Title;
                        returnPolicy.Covers = policy.Covers;
                        returnPolicy.Description = policy.Description;
                        returnPolicy.ReturnPolicyName = policy.ReturnPolicy;
                        break;
                    }
                    else
                    {
                        returnPolicy = new ReturnPolicyDTO();
                    }
                }
            }

            return returnPolicy;

        }

        public List<CategoryLibrary> GetCategoryWithParent(int Categoryid, string URL)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "/GetCategoryWithParent?Categoryid=" + Categoryid, "GET", null);
            List<CategoryLibrary> lstCategoryLibrary = new List<CategoryLibrary>();
            if (response != null)
            {
                BaseResponse<CategoryLibrary> baseResponse = new BaseResponse<CategoryLibrary>();
                baseResponse = baseResponse.JsonParseList(response);
                lstCategoryLibrary = (List<CategoryLibrary>)baseResponse.Data;
            }
            return lstCategoryLibrary;

        }

        public string Sendmail(int OrderId, string OrderStage, string ByWhom)
        {
            string res = "";
            GetOrders getOrders = new GetOrders(_configuration, _httpContext, UserId);
            List<OrderDetails> ordersItem = getOrders.BindOrderDetails(true, 0, 0, OrderId, null, null, null, null, null, null);
            OrderDetails orderDetails = ordersItem[0];
            string table_contain = "";

            string subject = "";
            string UserMailTemplet = "";
            string AdminMailTemplet = "";
            string SellerMailTemplet = "";

            switch (OrderStage)
            {
                case "Placed":
                    UserMailTemplet = "order_placed.html";
                    AdminMailTemplet = "order_placed.html";
                    SellerMailTemplet = "order_placed.html";
                    subject = "Order Placed Successfully";
                    break;
                case "Confirmed":
                    UserMailTemplet = "order_confirmation.html";
                    AdminMailTemplet = "order_confirmation_seller.html";
                    SellerMailTemplet = "order_confirmation_admin.html";
                    subject = "Order Confirmed Successfully";
                    break;

                case "Shipped":
                    UserMailTemplet = "order_shipped.html";
                    AdminMailTemplet = "order_shipped_seller.html";
                    SellerMailTemplet = "order_shipped_admin.html";
                    subject = "Order Shipped";
                    break;
                case "Delivered":
                    UserMailTemplet = "order_delivery.html";
                    AdminMailTemplet = "order_delivery.html";
                    SellerMailTemplet = "order_delivery.html";
                    subject = "Order Delivered";
                    break;
                case "Cancelled":
                    UserMailTemplet = ByWhom.ToString().ToLower() == "customer" ? "order_cancelled.html" : "order_cancelled_admin_seller.html";
                    AdminMailTemplet = ByWhom.ToString().ToLower() == "customer"? "order_cancelled.html": "order_cancelled_seller.html";
                    SellerMailTemplet = ByWhom.ToString().ToLower() == "customer" ? "order_cancelled.html" : "order_cancelled_admin.html";
                    subject = "Order Delivered";
                    break;
                case "Failed":
                    UserMailTemplet = "order_failed.html";
                    AdminMailTemplet = "order_failed.html";
                    SellerMailTemplet = "order_failed.html";
                    subject = "Order Failed";
                    break;
                default:
                    UserMailTemplet = "";
                    AdminMailTemplet = "";
                    subject = "";
                    break;
            }


            //List<OrderItemDTO> orderItemDTO = new List<OrderItemDTO>();
            List<OrderItemDTO> orderItemDTO = orderDetails.OrderItems.ToList();
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";

            foreach (var item in orderItemDTO)
            {

                table_contain = $"<tr>" +
                                    $"<td style='padding:10px'>" +
                                        $"<table>" +
                                            $"<tbody>" +
                                                $"<tr>" +
                                                    $"<td style='padding:0 10px 0 0'>" +
                                                        $"<a href='#' target='_blank'><img src='" + baseUrl + "Resources/ProductImage/" + item.ProductImage + "' width='80px' /></a>" +//product image wiht link
                                                    $"</td>" +
                                                    $"<td style='padding:0px'>" +
                                                        $"<a href='#' style='color:#000000' target='_blank'>" +
                                                            $"<span style='color: #05215B;display:block;font-size:12px;padding-bottom:10px;font-weight:600'>" +
                                                                $"" + item.ProductName +  //produtc name 
                                                            $"</span>" +
                                                            $"<span style='display:block;font-size:12px'>" +
                                                                $"SKU Code: " + item.ProductSKUCode + //product sku
                                                            $"</span>" +
                                                            $"<span style='display:block;font-size:12px'>" +
                                                                $"Brand Name: " + item.BrandName + //product brand name
                                                            $"</span>" +
                                                        $"</a>" +
                                                    $"</td>" +
                                                $"</tr>" +
                                            $"</tbody>" +
                                        $"</table>" +
                                    $"</td>" +
                                    $"<td style='padding:10px'>" +
                                        $"<span style='color: #05215B;display:block;font-size:12px;padding-bottom:10px;text-wrap: nowrap;'>" +
                                            $"₹" + item.SellingPrice +//product MRP
                                        $"</span>" +
                                    $"</td>" +
                                    $"<td style='padding:10px'>" +
                                        $"<span style='color: #05215B;display:block;font-size:12px;padding-bottom:10px'>" +
                                            +item.Qty + //product qty
                                        $"</span>" +
                                    $"</td>" +
                                    $"<td style='padding:10px'>" +
                                        $"<span style='color: #05215B;display:block;font-size:12px;padding-bottom:10px;text-wrap: nowrap;'>" +
                                            $"₹" + item.TotalAmount + //product selling price
                                        $"</span>" +
                                    $"</td>" +
                                $"</tr>";
            }

            #region send mail
            MailSendSES objses = new MailSendSES(_configuration);


            List<string> ReceiverEmail = new List<string>();
            string htmlBody;
            StreamReader reader;
            string readFile;

            if (!string.IsNullOrEmpty(UserMailTemplet) && !string.IsNullOrEmpty(AdminMailTemplet))
            {

                #region User Mail

                reader = new StreamReader("Resources" + "\\EmailTemplate" + "\\user" + "\\" + UserMailTemplet);

                readFile = reader.ReadToEnd();

                readFile = readFile.Replace("{{image_server_path}}", baseUrl);
                readFile = readFile.Replace("{{user_name}}", orderDetails.UserName);
                readFile = readFile.Replace("{{date}}", orderDetails.OrderDate.ToString());
                readFile = readFile.Replace("{{orderid}}", orderDetails.OrderNo);
                readFile = readFile.Replace("{{table_contain}}", table_contain);
                readFile = readFile.Replace("{{discount}}", orderDetails.CoupontDiscount.ToString());
                readFile = readFile.Replace("{{shipping}}", orderDetails.TotalShippingCharge.ToString());
                readFile = readFile.Replace("{{cod}}", orderDetails.CODCharge.ToString());
                readFile = readFile.Replace("{{extra_charge}}", orderDetails.TotalExtraCharges.ToString());
                readFile = readFile.Replace("{{total}}", orderDetails.PaidAmount.ToString());
                readFile = readFile.Replace("{{payment_mode}}", orderDetails.PaymentMode);
                readFile = readFile.Replace("{{Phone}}", orderDetails.UserPhoneNo);
                readFile = readFile.Replace("{{Address}}", orderDetails.UserAddressLine1 + " " + orderDetails.UserAddressLine2);
                readFile = readFile.Replace("{{City}}", orderDetails.UserCity);
                readFile = readFile.Replace("{{Pincode}}", orderDetails.UserPincode);
                readFile = readFile.Replace("{{State}}", orderDetails.UserState);
                readFile = readFile.Replace("{{seller_name}}", ByWhom.ToString().ToLower() == "seller" ? "Seller" : "");
                readFile = readFile.Replace("{{admin_name}}", ByWhom.ToString().ToLower() == "admin" ? "admin" : "");

                htmlBody = readFile;

                ReceiverEmail.Add(orderDetails.UserEmail);
                objses.sendMail(subject, htmlBody, ReceiverEmail);

                reader.Dispose();
                #endregion

                #region Admin Mail

                reader = new StreamReader("Resources" + "\\EmailTemplate" + "\\admin" + "\\" + AdminMailTemplet);

                readFile = reader.ReadToEnd();

                readFile = readFile.Replace("{{image_server_path}}", baseUrl);
                readFile = readFile.Replace("{{user_name}}", orderDetails.UserName);
                readFile = readFile.Replace("{{admin_name}}", "Hashkart Team");
                readFile = readFile.Replace("{{date}}", orderDetails.OrderDate.ToString());
                readFile = readFile.Replace("{{orderid}}", orderDetails.OrderNo);
                readFile = readFile.Replace("{{table_contain}}", table_contain);
                readFile = readFile.Replace("{{discount}}", orderDetails.CoupontDiscount.ToString());
                readFile = readFile.Replace("{{shipping}}", orderDetails.TotalShippingCharge.ToString());
                readFile = readFile.Replace("{{cod}}", orderDetails.CODCharge.ToString());
                readFile = readFile.Replace("{{extra_charge}}", orderDetails.TotalExtraCharges.ToString());
                readFile = readFile.Replace("{{total}}", orderDetails.PaidAmount.ToString());
                readFile = readFile.Replace("{{payment_mode}}", orderDetails.PaymentMode);
                readFile = readFile.Replace("{{Phone}}", orderDetails.UserPhoneNo);
                readFile = readFile.Replace("{{Address}}", orderDetails.UserAddressLine1 + " " + orderDetails.UserAddressLine2);
                readFile = readFile.Replace("{{City}}", orderDetails.UserCity);
                readFile = readFile.Replace("{{Pincode}}", orderDetails.UserPincode);
                readFile = readFile.Replace("{{State}}", orderDetails.UserState);

                htmlBody = readFile;
                ReceiverEmail = new List<string>();
                ReceiverEmail.Add(_configuration.GetSection("AdminMail").GetSection("SenderMail").Value);

                if (ByWhom.ToString().ToLower() == "seller" || OrderStage.ToString().ToLower() == "delivered")
                {
                    objses.sendMail(subject, htmlBody, ReceiverEmail);
                }

                reader.Dispose();
                #endregion

                #region Seller Mail
                table_contain = "";

                List<string> sellerId = new List<string>();

                sellerId = orderItemDTO.DistinctBy(p => p.SellerID).Select(p => p.SellerID).ToList();
                string sellername = "";
                for (int i = 0; i < sellerId.Count; i++)
                {
                    foreach (var item in orderItemDTO.Where(p => p.SellerID.ToString() == sellerId[i]).ToList())
                    {
                        table_contain = $"<tr>" +
                                            $"<td style='padding:10px'>" +
                                                $"<table>" +
                                                    $"<tbody>" +
                                                        $"<tr>" +
                                                            $"<td style='padding:0 10px 0 0'>" +
                                                                $"<a href='#' target='_blank'><img src='" + baseUrl + "Resources/ProductImage/" + item.ProductImage + "' width='80px' /></a>" +//product image wiht link
                                                            $"</td>" +
                                                            $"<td style='padding:0px'>" +
                                                                $"<a href='#' style='color:#000000' target='_blank'>" +
                                                                    $"<span style='color: #05215B;display:block;font-size:12px;padding-bottom:10px;font-weight:600'>" +
                                                                        $"" + item.ProductName +  //produtc name 
                                                                    $"</span>" +
                                                                    $"<span style='display:block;font-size:12px'>" +
                                                                        $"SKU Code: " + item.ProductSKUCode + //product sku
                                                                    $"</span>" +
                                                                    $"<span style='display:block;font-size:12px'>" +
                                                                        $"Brand Name: " + item.BrandName + //product brand name
                                                                    $"</span>" +
                                                                $"</a>" +
                                                            $"</td>" +
                                                        $"</tr>" +
                                                    $"</tbody>" +
                                                $"</table>" +
                                            $"</td>" +
                                            $"<td style='padding:10px'>" +
                                                $"<span style='color: #05215B;display:block;font-size:12px;padding-bottom:10px;text-wrap: nowrap;'>" +
                                                    $"₹" + item.SellingPrice +//product MRP
                                                $"</span>" +
                                            $"</td>" +
                                            $"<td style='padding:10px'>" +
                                                $"<span style='color: #05215B;display:block;font-size:12px;padding-bottom:10px'>" +
                                                    +item.Qty + //product qty
                                                $"</span>" +
                                            $"</td>" +
                                            $"<td style='padding:10px'>" +
                                                $"<span style='color: #05215B;display:block;font-size:12px;padding-bottom:10px;text-wrap: nowrap;'>" +
                                                    $"₹" + item.TotalAmount + //product selling price
                                                $"</span>" +
                                            $"</td>" +
                                        $"</tr>";
                    }
                    sellername = orderItemDTO.Where(p => p.SellerID.ToString() == sellerId[i]).Select(a => a.SellerName.ToString()).FirstOrDefault();

                    reader = new StreamReader("Resources" + "\\EmailTemplate" + "\\admin" + "\\order_placed.html");

                    readFile = reader.ReadToEnd();

                    readFile = readFile.Replace("{{image_server_path}}", baseUrl);
                    readFile = readFile.Replace("{{user_name}}", orderDetails.UserName);
                    readFile = readFile.Replace("{{seller_name}}", sellername);
                    readFile = readFile.Replace("{{admin_name}}", "Hashkart Team");
                    readFile = readFile.Replace("{{date}}", orderDetails.OrderDate.ToString());
                    readFile = readFile.Replace("{{orderid}}", orderDetails.OrderNo);
                    readFile = readFile.Replace("{{table_contain}}", table_contain);
                    readFile = readFile.Replace("{{discount}}", orderDetails.CoupontDiscount.ToString());
                    readFile = readFile.Replace("{{shipping}}", orderDetails.TotalShippingCharge.ToString());
                    readFile = readFile.Replace("{{cod}}", orderDetails.CODCharge.ToString());
                    readFile = readFile.Replace("{{extra_charge}}", orderDetails.TotalExtraCharges.ToString());
                    readFile = readFile.Replace("{{total}}", orderDetails.PaidAmount.ToString());
                    readFile = readFile.Replace("{{payment_mode}}", orderDetails.PaymentMode);
                    readFile = readFile.Replace("{{Phone}}", orderDetails.UserPhoneNo);
                    readFile = readFile.Replace("{{Address}}", orderDetails.UserAddressLine1 + " " + orderDetails.UserAddressLine2);
                    readFile = readFile.Replace("{{City}}", orderDetails.UserCity);
                    readFile = readFile.Replace("{{Pincode}}", orderDetails.UserPincode);
                    readFile = readFile.Replace("{{State}}", orderDetails.UserState);


                    htmlBody = readFile;
                    ReceiverEmail = new List<string>();
                    ReceiverEmail.Add(orderItemDTO.Where(p => p.SellerID.ToString() == sellerId[i]).Select(a => a.SellerEmailId.ToString()).FirstOrDefault());
                    if (ByWhom.ToString().ToLower() == "admin" || OrderStage.ToString().ToLower() == "delivered")
                    {
                        objses.sendMail(subject, htmlBody, ReceiverEmail);
                    }
                    reader.Dispose();

                }


                #endregion
            }
            #endregion

            return res;
        }

        public JObject CreateShipmentDHL(List<OrderItems> orderItemslst, OrderShipmentInfoDto model, Warehouse warehouse, SellerKycList sellerKyc)
        {

            decimal totalSum = orderItemslst.Select(p => p.SellingPrice).Sum();

            string des = "";

            JsonObject shipmentObj = new JsonObject();
            JsonObject pickup = new JsonObject();
            JsonObject accounts = new JsonObject();
            JsonObject shipperDetails = new JsonObject();
            JsonObject shipperPostalAddress = new JsonObject();
            JsonObject shippercontactInfo = new JsonObject();
            JsonObject receiverDetails = new JsonObject();
            JsonObject receiverPostalAddress = new JsonObject();
            JsonObject receivercontactInfo = new JsonObject();
            JsonObject content = new JsonObject();
            JsonObject exportDeclaration = new JsonObject();
            JsonArray lineItems = new JsonArray();
            JsonObject lineItemsData = new JsonObject();
            JsonObject lineItemsquantity = new JsonObject();
            JsonObject lineItemsDataweight = new JsonObject();
            JsonObject lineItemsDatainvoice = new JsonObject();

            JsonObject packages = new JsonObject();
            JsonObject dimensions = new JsonObject();

            pickup["isRequested"] = false;

            accounts["typeCode"] = "shipper";
            accounts["number"] = "351341977";



            shipperPostalAddress["postalCode"] = warehouse.Pincode;
            shipperPostalAddress["cityName"] = "Nairobi";
            //shipperPostalAddress["cityName"] = warehouse.CityName;
            shipperPostalAddress["countryCode"] = "KE";
            shipperPostalAddress["addressLine1"] = warehouse.AddressLine1;
            shipperPostalAddress["addressLine2"] = warehouse.AddressLine2;
            shipperPostalAddress["countyName"] = "Nairobi";
            //shipperPostalAddress["countyName"] = warehouse.CountryName;


            shippercontactInfo["email"] = sellerKyc.EmailID;
            shippercontactInfo["phone"] = sellerKyc.PhoneNumber;
            shippercontactInfo["mobilePhone"] = sellerKyc.ContactPersonMobileNo;
            shippercontactInfo["companyName"] = sellerKyc.TradeName;
            shippercontactInfo["fullName"] = sellerKyc.FirstName + " " + sellerKyc.LastName;

            shipperDetails = new JsonObject {
                { "postalAddress", shipperPostalAddress },
                { "contactInformation", shippercontactInfo }
            };


            receiverPostalAddress["cityName"] = "SWAKOPMUND";
            //receiverPostalAddress["cityName"] = model.DropCity;
            receiverPostalAddress["countryCode"] = "NA";
            receiverPostalAddress["provinceCode"] = "N/A";
            receiverPostalAddress["postalCode"] = model.DropPincode.ToString();
            receiverPostalAddress["addressLine1"] = model.DropAddressLine1;
            receiverPostalAddress["addressLine2"] = model.DropAddressLine2;

            receivercontactInfo["mobilePhone"] = model.DropContactPersonMobileNo;
            receivercontactInfo["phone"] = model.DropContactPersonMobileNo;
            receivercontactInfo["companyName"] = "N/A";
            receivercontactInfo["fullName"] = model.DropContactPersonName;
            receivercontactInfo["email"] = model.DropContactPersonEmailID;

            receiverDetails = new JsonObject {
                {"postalAddress",receiverPostalAddress },
                {"contactInformation",receivercontactInfo }
            };

            foreach (var item in orderItemslst)
            {

                lineItemsquantity["unitOfMeasurement"] = "PCS";
                lineItemsquantity["value"] = Convert.ToInt32(item.Qty);

                lineItemsDataweight["netValue"] = Convert.ToDecimal(Convert.ToDecimal(model.Weight).ToString("N1"));
                lineItemsDataweight["grossValue"] = Convert.ToDecimal(Convert.ToDecimal(model.Weight).ToString("N1"));


                lineItemsData["number"] = Convert.ToInt32(1);
                lineItemsData["quantity"] = lineItemsquantity;
                lineItemsData["price"] = Convert.ToDecimal(Convert.ToDecimal(totalSum).ToString("N2"));
                lineItemsData["description"] = item.ProductName;
                lineItemsData["weight"] = lineItemsDataweight;
                lineItemsData["manufacturerCountry"] = "KE";

                lineItems.Add(lineItemsData);
                des += item.ProductName + " | ";
            }


            lineItemsDatainvoice["date"] = "2023-12-27";
            lineItemsDatainvoice["number"] = "Inv_ord_" + model.OrderID + "_" + model.PackageID;


            exportDeclaration["lineItems"] = lineItems;
            exportDeclaration["invoice"] = lineItemsDatainvoice;

            dimensions["length"] = Convert.ToInt32(model.Length);
            dimensions["width"] = Convert.ToInt32(model.Width);
            dimensions["height"] = Convert.ToInt32(model.Height);

            packages["weight"] = Convert.ToDecimal(Convert.ToDecimal(model.Weight).ToString("N1"));
            packages["dimensions"] = dimensions;


            content["exportDeclaration"] = exportDeclaration;
            content["unitOfMeasurement"] = "metric";
            content["isCustomsDeclarable"] = true;
            content["incoterm"] = "DAP";
            content["description"] = des;
            content["packages"] = new JsonArray { packages };
            content["declaredValueCurrency"] = "KES";
            content["declaredValue"] = Convert.ToDecimal(Convert.ToDecimal(totalSum).ToString("N2"));


            shipmentObj["productCode"] = "P";
            shipmentObj["plannedShippingDateAndTime"] = DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "GMT+00:00";
            shipmentObj["pickup"] = pickup;
            shipmentObj["accounts"] = new JsonArray { accounts };
            shipmentObj["customerDetails"] = new JsonObject { { "shipperDetails", shipperDetails }, { "receiverDetails", receiverDetails } };
            shipmentObj["content"] = content;

            string ss = shipmentObj.ToString();

            JObject res;
            var response = helper.ApiCallDHL("https://express.api.dhl.com/mydhlapi/test/", "shipments", "POST", shipmentObj.ToString());
            var result = response.Content.ReadAsStringAsync();
            res = JObject.Parse(result.Result);
            return res;
        }

        public JObject PickupShipment(List<OrderItems> orderItemslst, OrderShipmentInfoDto model, Warehouse warehouse, SellerKycList sellerKyc)
        {

            decimal totalSum = orderItemslst.Select(p => p.SellingPrice).Sum();

            JsonObject pickupObj = new JsonObject();
            JsonObject accounts = new JsonObject();
            JsonObject accounts1 = new JsonObject();


            JsonObject shipperDetails = new JsonObject();
            JsonObject shipperPostalAddress = new JsonObject();
            JsonObject shippercontactInfo = new JsonObject();
            JsonObject pickupDetails = new JsonObject();
            JsonObject pickupPostalAddress = new JsonObject();
            JsonObject pickupcontactInfo = new JsonObject();
            JsonObject shipmentDetailsData = new JsonObject();
            JsonObject packages = new JsonObject();
            JsonObject dimensions = new JsonObject();


            accounts["typeCode"] = "shipper";
            accounts["number"] = "351341977";
            accounts1["typeCode"] = "shipper";
            accounts1["number"] = "351341977";




            shipperPostalAddress["postalCode"] = warehouse.Pincode;
            shipperPostalAddress["cityName"] = "Nairobi";
            //shipperPostalAddress["cityName"] = warehouse.CityName;
            shipperPostalAddress["countryCode"] = "KE";
            shipperPostalAddress["addressLine1"] = warehouse.AddressLine1;
            shipperPostalAddress["addressLine2"] = warehouse.AddressLine2;
            shipperPostalAddress["countyName"] = "Nairobi";
            //shipperPostalAddress["countyName"] = warehouse.CountryName;



            shippercontactInfo["email"] = sellerKyc.EmailID;
            shippercontactInfo["phone"] = sellerKyc.PhoneNumber;
            shippercontactInfo["mobilePhone"] = sellerKyc.ContactPersonMobileNo;
            shippercontactInfo["companyName"] = sellerKyc.TradeName;
            shippercontactInfo["fullName"] = sellerKyc.FirstName + " " + sellerKyc.LastName;

            shipperDetails = new JsonObject {
                { "postalAddress", shipperPostalAddress },
                { "contactInformation", shippercontactInfo }
            };

            pickupPostalAddress["cityName"] = "Nairobi";
            //pickupPostalAddress["cityName"] = warehouse.CityName;
            pickupPostalAddress["countryCode"] = "KE";
            pickupPostalAddress["provinceCode"] = "N/A";
            pickupPostalAddress["postalCode"] = warehouse.Pincode;
            pickupPostalAddress["addressLine1"] = warehouse.AddressLine1;
            pickupPostalAddress["addressLine2"] = warehouse.AddressLine2;

            pickupcontactInfo["mobilePhone"] = warehouse.ContactPersonMobileNo;
            pickupcontactInfo["phone"] = warehouse.ContactPersonMobileNo;
            pickupcontactInfo["companyName"] = sellerKyc.TradeName;
            pickupcontactInfo["fullName"] = sellerKyc.FirstName + " " + sellerKyc.LastName;
            pickupcontactInfo["email"] = sellerKyc.EmailID;

            pickupDetails = new JsonObject {
                {"postalAddress",pickupPostalAddress },
                {"contactInformation",pickupcontactInfo }
            };

            dimensions["length"] = Convert.ToInt32(model.Length);
            dimensions["width"] = Convert.ToInt32(model.Width);
            dimensions["height"] = Convert.ToInt32(model.Height);
            packages["weight"] = Convert.ToDecimal(Convert.ToDecimal(model.Weight).ToString("N1"));
            packages["dimensions"] = dimensions;

            shipmentDetailsData["productCode"] = "P";
            shipmentDetailsData["accounts"] = new JsonArray { accounts };
            shipmentDetailsData["isCustomsDeclarable"] = true;
            shipmentDetailsData["unitOfMeasurement"] = "metric";
            shipmentDetailsData["declaredValue"] = totalSum;
            shipmentDetailsData["declaredValueCurrency"] = "KES";
            shipmentDetailsData["packages"] = new JsonArray { packages };


            pickupObj["plannedPickupDateAndTime"] = DateTimeOffset.Now.AddDays(1).ToString("yyyy-MM-ddT") + "00:00:00GMT+00:00";
            pickupObj["accounts"] = new JsonArray { accounts1 };
            pickupObj["customerDetails"] = new JsonObject { { "shipperDetails", shipperDetails }, { "pickupDetails", pickupDetails } };
            pickupObj["shipmentDetails"] = new JsonArray { shipmentDetailsData };

            JObject res;
            var response = helper.ApiCallDHL("https://express.api.dhl.com/mydhlapi/test/", "pickups", "POST", pickupObj.ToString());
            var result = response.Content.ReadAsStringAsync();
            res = JObject.Parse(result.Result);
            return res;
        }
    }
}
