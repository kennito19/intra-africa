using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.Order;
using API_Gateway.Models.Entity.User;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using ICSharpCode.SharpZipLib.Zip;
using Irony.Parsing;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace API_Gateway.Common.orders
{
    public class GetOrders
    {
        private readonly IConfiguration _configuration;
        public string CatelogueURL = string.Empty;
        public string OrderURL = string.Empty;
        public string IdServerURL = string.Empty;
        public string UserURL = string.Empty;
        public string UserId = string.Empty;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        public GetOrders(IConfiguration configuration, HttpContext httpContext, string Userid)
        {
            UserId = Userid;
            _httpContext = httpContext;
            _configuration = configuration;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IdServerURL = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            UserURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            OrderURL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            helper = new ApiHelper(_httpContext);
        }

        public BaseResponse<OrderDetails> Get()
        {
            BaseResponse<OrderDetails> baseResponse = new BaseResponse<OrderDetails>();
            var Oredrs = BindOrderDetails();

            foreach (var item in Oredrs)
            {
                int oid = (int)item.OrderId;
                item.OrderItems = BindOrderItems(oid);
                //item.ProductVideoLinks = BindVideos(pid);
                //item.ProductColorMapping = BindColors(pid);

            }
            baseResponse.code = 200;
            baseResponse.Message = "Records Bind Sucessfully";
            baseResponse.Data = Oredrs;

            return baseResponse;
        }

        public BaseResponse<OrderDetails> Get(string orderId)
        {
            BaseResponse<OrderDetails> detailBaseresponse = new BaseResponse<OrderDetails>();
            var ordersItem = BindOrderDetails(orderId);
            detailBaseresponse.code = 200;
            detailBaseresponse.Message = "Records Bind Successfully";
            detailBaseresponse.Data = ordersItem;
            return detailBaseresponse;
        }

        public List<OrderDetails> GetorderByRefNo(string orderRefNo)
        {
            var ordersItem = BindOrderDetailsByRefNo(orderRefNo);
            return ordersItem;
        }

        public BaseResponse<OrderDetails> Get(int? pageIndex = 0, int? pageSize = 0, string? SellerId = null, string? Status = null, string? Searchtext = null)
        {
            BaseResponse<OrderDetails> baseresponse = new BaseResponse<OrderDetails>();
            var ordersItem = BindOrderDetails(pageIndex, pageSize, SellerId, Status, Searchtext);

            baseresponse.code = 200;
            baseresponse.Message = ordersItem.Count==0? "Record does not Exist." : "Record Bind Successfully";
            baseresponse.Data = ordersItem;
            baseresponse.pagination = new Pagination { PageCount = ordersItem.Count() > 0 ? ordersItem.FirstOrDefault().PageCount : 0, RecordCount = ordersItem.Count() > 0 ? ordersItem.FirstOrDefault().RecordCount : 0 };
            //if (pageIndex != 0 && pageSize != 0)
            //{
            //    int totalCount = ordersItem.Count;
            //    int TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            //    List<OrderDetails> items = ordersItem.Skip((Convert.ToInt32(pageIndex) - 1) * Convert.ToInt32(pageSize)).Take(Convert.ToInt32(pageSize)).ToList();
            //    baseresponse.Data = ordersItem;
            //    baseresponse.pagination = new Pagination { PageCount = ordersItem.FirstOrDefault().PageCount, RecordCount = ordersItem.FirstOrDefault().RecordCount };
            //}
            return baseresponse;
        }

        public BaseResponse<OrderDetails> Get(bool NotInStatus, int? pageIndex = 0, int? pageSize = 0, int? Id = 0, string? GUID = null, string? OrderNo = null, string? OrderRefNo = null, string? SellerId = null, string? UserId = null, string? Status = null, string? Searchtext = null)
        {
            BaseResponse<OrderDetails> baseResponse = new BaseResponse<OrderDetails>();
            var ordersItem = BindOrderDetails(NotInStatus, pageIndex, pageSize, Id, GUID, OrderNo, OrderRefNo, SellerId, UserId, Status, Searchtext);
            if (ordersItem.Count() > 0)
            {
                baseResponse.code = 200;
                baseResponse.Message = "Records Bind Sucessfully";

                if (pageIndex != 0 && pageSize != 0)
                {
                    //int totalCount = ordersItem.Count;
                    //int TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                    //List<OrderDetails> items = ordersItem.Skip((Convert.ToInt32(pageIndex) - 1) * Convert.ToInt32(pageSize)).Take(Convert.ToInt32(pageSize)).ToList();
                    baseResponse.Data = ordersItem;
                    baseResponse.pagination = new Pagination { PageCount = ordersItem[0].PageCount, RecordCount = ordersItem[0].RecordCount };
                }
                else
                {
                    if (Id != 0 || GUID != null || OrderNo != null)
                    {
                        if (ordersItem.Count() == 1)
                        {
                            OrderDetails obj = ordersItem.FirstOrDefault();
                            baseResponse.Data = obj;
                        }
                        else
                        {
                            baseResponse.Data = ordersItem;
                        }

                    }
                    else
                    {
                        baseResponse.Data = ordersItem;
                    }
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return baseResponse;
        }

        public List<OrderDetails> BindOrderDetails(int? pageIndex = 0, int? pageSize = 0, string? SellerId = null, string? Status = null, string? Searchtext = null)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(Status))
            {
                url = url + "&status=" + Status;

                if (Status.ToLower() == "failed" || Status.ToLower() == "initiate")
                {
                    url = url + "&notInStatus=" + false;
                }
                else
                {
                    url = url + "&notInStatus=" + true;
                }
            }
            else
            {
                url = url + "&notInStatus=" + true;
            }

            if (!string.IsNullOrEmpty(Searchtext))
            {
                url = url + "&searchtext=" + HttpUtility.UrlEncode(Searchtext);
            }
            if (!string.IsNullOrEmpty(SellerId))
            {

                url = url + "&SellerID=" + SellerId;
            }
            BaseResponse<Orders> baseResponse = new BaseResponse<Orders>();
            var GetResponse = helper.ApiCall(OrderURL, EndPoints.Orders + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            List<OrderDetails> orders = new List<OrderDetails>();
            List<Orders> ordersdetail = baseResponse.Data as List<Orders>;

            //
            //List<SellerKycList> sellerlist = getsellerKyclst();

            orders = (from details in ordersdetail
                          //join s in sellerlist on details.SellerID equals s.Id into kycGroup
                          //from kyc in kycGroup.DefaultIfEmpty()
                      select new OrderDetails
                      {
                          RowNumber = details.RowNumber,
                          PageCount = details.PageCount,
                          RecordCount = details.RecordCount,
                          OrderId = details.Id,
                          OrderNo = details.OrderNo,
                          UserId = details.UserId,
                          UserName = details.UserName,
                          UserPhoneNo = details.UserPhoneNo,
                          UserEmail = details.UserEmail,
                          UserAddressLine1 = details.UserAddressLine1,
                          UserAddressLine2 = details.UserAddressLine2,
                          UserLendMark = details.UserLandmark,
                          UserPincode = details.UserPincode,
                          UserCity = details.UserCity,
                          UserState = details.UserState,
                          UserCountry = details.UserCountry,
                          UserGSTNo = details.UserGSTNo,
                          PaymentMode = details.PaymentMode,
                          TotalShippingCharge = (decimal)details.TotalShippingCharge,
                          TotalExtraCharges = (decimal)details.TotalExtraCharges,
                          TotalAmount = (decimal)details.TotalAmount,
                          IsCouponApplied = details.IsCouponApplied,
                          Coupon = details.Coupon,
                          CoupontDiscount = (decimal)details.CoupontDiscount,
                          CoupontDetails = details.CoupontDetails,
                          PaidAmount = (decimal)details.PaidAmount,
                          CODCharge = details.CODCharge != null ? (decimal)details.CODCharge : 0,
                          IsSale = details.IsSale,
                          SaleType = details.SaleType,
                          OrderDate = (DateTime)details.OrderDate,
                          Status = details.Status,
                          PaymentInfo = details.PaymentInfo,
                          OrderBy = details.OrderBy,
                          IsRetailer = details.IsRetailer,
                          IsVertualRetailer = details.IsVertualRetailer,
                          IsReplace = details.IsReplace,
                          ParentId = details.ParentID,
                          OrderReferenceNo = details.OrderReferenceNo,
                          //SellerName = kyc.TradeName != null ? kyc.TradeName : kyc.DisplayName,
                          //SellerPhoneNo = kyc.PhoneNumber,
                          //SellerEmailId = kyc.EmailID,
                          //SellerStatus = kyc.SellerStatus,
                          //SellerKycStatus = kyc.Status
                      }).ToList();

            //foreach (var details in ordersdetail)
            //{
            //    OrderDetails order = new OrderDetails();
            //    order.OrderId = details.Id;
            //    order.OrderNo = details.OrderNo;
            //    order.SellerId = details.SellerID;
            //    order.UserId = details.UserId;
            //    order.UserName = details.UserName;
            //    order.UserPhoneNo = details.UserPhoneNo;
            //    order.UserEmail = details.UserEmail;
            //    order.UserAddressLine1 = details.UserAddressLine1;
            //    order.UserAddressLine2 = details.UserAddressLine2;
            //    order.UserLendMark = details.UserLandmark;
            //    order.UserPincode = details.UserPincode;
            //    order.UserCity = details.UserCity;
            //    order.UserState = details.UserState;
            //    order.UserCountry = details.UserCountry;
            //    order.UserGSTNo = details.UserGSTNo;
            //    order.PaymentMode = details.PaymentMode;
            //    order.TotalShippingCharge = (decimal)details.TotalShippingCharge;
            //    order.TotalExtraCharges = (decimal)details.TotalExtraCharges;
            //    order.TotalAmount = (decimal)details.TotalAmount;
            //    order.IsCouponApplied = details.IsCouponApplied;
            //    order.Coupon = details.Coupon;
            //    order.CoupontDiscount = (decimal)details.CoupontDiscount;
            //    order.CoupontDetails = details.CoupontDetails;
            //    order.PaidAmount = (decimal)details.PaidAmount;
            //    order.IsSale = details.IsSale;
            //    order.SaleType = details.SaleType;
            //    order.OrderDate = (DateTime)details.OrderDate;
            //    order.Status = details.Status;
            //    order.PaymentInfo = details.PaymentInfo;
            //    order.OrderBy = details.OrderBy;
            //    order.IsRetailer = details.IsRetailer;
            //    order.IsVertualRetailer = details.IsVertualRetailer;
            //    order.IsReplace = details.IsReplace;
            //    order.ParentId = (int)details.ParentID;
            //    SellerKycList SellerDetails = getsellerKyc(details.SellerID);
            //    order.SellerName = SellerDetails.TradeName;
            //    order.SellerPhoneNo = SellerDetails.ContactPersonMobileNo;
            //    order.SellerEmailId = SellerDetails.EmailID;


            //    orders.Add(order);
            //}
            return orders;
        }


        public List<OrderDetails> BindOrderDetails(bool NotInStatus, int? pageIndex = 0, int? pageSize = 0, int? Id = 0, string? GUID = null, string? OrderNo = null, string? OrderRefNo = null, string? SellerId = null, string? UserId = null, string? Status = null, string? Searchtext = null)
        {
            string url = "?PageIndex=" + pageIndex + "&PageSize=" + pageSize;
            if (Id != null && Id != 0)
            {
                url = url + "&Id=" + Id;
            }
            if (!string.IsNullOrEmpty(GUID))
            {
                url = url + "&GUID=" + GUID;
            }
            if (!string.IsNullOrEmpty(OrderNo))
            {
                url = url + "&OrderNo=" + OrderNo;
            }
            if (!string.IsNullOrEmpty(OrderRefNo))
            {
                url = url + "&OrderRefNo=" + OrderRefNo;
            }
            if (!string.IsNullOrEmpty(SellerId))
            {
                url = url + "&SellerId=" + SellerId;
            }
            if (!string.IsNullOrEmpty(UserId))
            {
                url = url + "&UserId=" + UserId;
            }
            if (!string.IsNullOrEmpty(Status))
            {
                url = url + "&status=" + Status;
            }
            if (!string.IsNullOrEmpty(Searchtext))
            {
                url = url + "&searchtext=" + HttpUtility.UrlEncode(Searchtext);
            }
            if (NotInStatus != null)
            {
                url = url + "&NotInStatus=" + NotInStatus;
            }
            BaseResponse<Orders> baseResponse = new BaseResponse<Orders>();
            var GetResponse = helper.ApiCall(OrderURL, EndPoints.Orders + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            //List<OrderDetails> orders = new List<OrderDetails>();
            List<Orders> ordersdetail = baseResponse.Data as List<Orders>;

            List<OrderDetails> orders = ordersdetail.Select(details => new OrderDetails
            {
                OrderId = details.Id,
                OrderNo = details.OrderNo,
                //order.SellerId = details.SellerID,
                UserId = details.UserId,
                UserName = details.UserName,
                UserPhoneNo = details.UserPhoneNo,
                UserEmail = details.UserEmail,
                UserAddressLine1 = details.UserAddressLine1,
                UserAddressLine2 = details.UserAddressLine2,
                UserLendMark = details.UserLandmark,
                UserPincode = details.UserPincode,
                UserCity = details.UserCity,
                UserState = details.UserState,
                UserCountry = details.UserCountry,
                UserGSTNo = details.UserGSTNo,
                PaymentMode = details.PaymentMode,
                TotalShippingCharge = (decimal)details.TotalShippingCharge,
                TotalExtraCharges = (decimal)details.TotalExtraCharges,
                TotalAmount = (decimal)details.TotalAmount,
                IsCouponApplied = details.IsCouponApplied,
                Coupon = details.Coupon,
                CoupontDiscount = (decimal)details.CoupontDiscount,
                CoupontDetails = details.CoupontDetails,
                CODCharge = details.CODCharge != null ? (decimal)details.CODCharge : 0,
                PaidAmount = (decimal)details.PaidAmount,
                IsSale = details.IsSale,
                SaleType = details.SaleType,
                OrderDate = (DateTime)details.OrderDate,
                DeliveryDate = (DateTime)details.DeliveryDate,
                Status = details.Status,
                PaymentInfo = details.PaymentInfo,
                OrderBy = details.OrderBy,
                IsRetailer = details.IsRetailer,
                IsVertualRetailer = details.IsVertualRetailer,
                IsReplace = details.IsReplace,
                ParentId = details.ParentID,
                OrderReferenceNo = details.OrderReferenceNo,

                RowNumber = details.RowNumber,
                PageCount = details.PageCount,
                RecordCount = details.RecordCount,
                //SellerKycList SellerDetails = getsellerKyc(details.SellerID),
                //order.SellerName = SellerDetails.TradeName,
                //order.SellerPhoneNo = SellerDetails.ContactPersonMobileNo,
                //order.SellerEmailId = SellerDetails.EmailID,
                //order.SellerStatus = SellerDetails.SellerStatus,
                //order.SellerKycStatus = SellerDetails.Status,
                OrderItems = BindOrderItems(details.Id),
            }).ToList();

            //foreach (var details in ordersdetail)
            //{
            //    OrderDetails order = new OrderDetails();
            //    order.OrderId = details.Id;
            //    order.OrderNo = details.OrderNo;
            //    //order.SellerId = details.SellerID;
            //    order.UserId = details.UserId;
            //    order.UserName = details.UserName;
            //    order.UserPhoneNo = details.UserPhoneNo;
            //    order.UserEmail = details.UserEmail;
            //    order.UserAddressLine1 = details.UserAddressLine1;
            //    order.UserAddressLine2 = details.UserAddressLine2;
            //    order.UserLendMark = details.UserLandmark;
            //    order.UserPincode = details.UserPincode;
            //    order.UserCity = details.UserCity;
            //    order.UserState = details.UserState;
            //    order.UserCountry = details.UserCountry;
            //    order.UserGSTNo = details.UserGSTNo;
            //    order.PaymentMode = details.PaymentMode;
            //    order.TotalShippingCharge = (decimal)details.TotalShippingCharge;
            //    order.TotalExtraCharges = (decimal)details.TotalExtraCharges;
            //    order.TotalAmount = (decimal)details.TotalAmount;
            //    order.IsCouponApplied = details.IsCouponApplied;
            //    order.Coupon = details.Coupon;
            //    order.CoupontDiscount = (decimal)details.CoupontDiscount;
            //    order.CoupontDetails = details.CoupontDetails;
            //    order.CODCharge = details.CODCharge != null ? (decimal)details.CODCharge : 0;
            //    order.PaidAmount = (decimal)details.PaidAmount;
            //    order.IsSale = details.IsSale;
            //    order.SaleType = details.SaleType;
            //    order.OrderDate = (DateTime)details.OrderDate;
            //    order.Status = details.Status;
            //    order.PaymentInfo = details.PaymentInfo;
            //    order.OrderBy = details.OrderBy;
            //    order.IsRetailer = details.IsRetailer;
            //    order.IsVertualRetailer = details.IsVertualRetailer;
            //    order.IsReplace = details.IsReplace;
            //    order.ParentId = details.ParentID;

            //    order.RowNumber = details.RowNumber;
            //    order.PageCount = details.PageCount;
            //    order.RecordCount = details.RecordCount;
            //    //SellerKycList SellerDetails = getsellerKyc(details.SellerID);
            //    //order.SellerName = SellerDetails.TradeName;
            //    //order.SellerPhoneNo = SellerDetails.ContactPersonMobileNo;
            //    //order.SellerEmailId = SellerDetails.EmailID;
            //    //order.SellerStatus = SellerDetails.SellerStatus;
            //    //order.SellerKycStatus = SellerDetails.Status;
            //    order.OrderItems = BindOrderItems(details.Id);

            //    orders.Add(order);
            //}
            return orders;
        }

        public List<OrderItemDTO> OldBindOrderItems(int orderId, string? SellerId = null, int? OrderItemId = 0, string? Status = null)
        {
            string url = "";
            if (!string.IsNullOrEmpty(SellerId))
            {
                url = url + "&SellerId=" + SellerId;
            }
            if (!string.IsNullOrEmpty(Status))
            {
                url = url + "&Status=" + Status;
            }
            if (OrderItemId != 0)
            {
                url = url + "&Id=" + OrderItemId;
            }
            BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
            var GetResponse = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + orderId + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);
            List<OrderItemDTO> ordersItem = new List<OrderItemDTO>();
            List<OrderItems> ordersdetail = baseResponse.Data as List<OrderItems>;
            if (ordersdetail.Count > 0)
            {
                BrandLibrary brand = getBrand(Convert.ToInt32(ordersdetail[0].BrandID));

                //ordersItem = ordersdetail.Select(details => new OrderItemDTO
                //{
                //    Id = details.Id,
                //    OrderID = details.OrderID,
                //    SubOrderNo = details.SubOrderNo,
                //    SellerID = details.SellerID,
                //    BrandID = details.BrandID,
                //    ProductID = details.ProductID,
                //    SellerProductID = details.SellerProductID,
                //    ProductName = details.ProductName,
                //    ProductSKUCode = details.ProductSKUCode,
                //    MRP = details.MRP,
                //    SellingPrice = details.SellingPrice,
                //    Discount = details.Discount,
                //    Qty = details.Qty,
                //    TotalAmount = details.TotalAmount,
                //    PriceTypeID = details.PriceTypeID,
                //    PriceType = details.PriceType,
                //    SizeID = details.SizeID,
                //    SizeValue = details.SizeValue,
                //    Coupon = details.Coupon,
                //    CoupontDetails = details.CoupontDetails,
                //    IsCouponApplied = details.IsCouponApplied != null ? Convert.ToBoolean(details.IsCouponApplied) : null,
                //    CoupontDiscount = details.CoupontDiscount,
                //    ShippingZone = details.ShippingZone,
                //    ShippingCharge = details.ShippingCharge,
                //    ShippingChargePaidBy = details.ShippingChargePaidBy,
                //    SubTotal = details.SubTotal,
                //    Status = details.Status,
                //    WherehouseId = details.WherehouseId,
                //    IsReplace = details.IsReplace,
                //    ParentID = details.ParentID,
                //    ReturnPolicyName = details.ReturnPolicyName,
                //    ReturnPolicyTitle = details.ReturnPolicyTitle,
                //    ReturnPolicyCovers = details.ReturnPolicyCovers,
                //    ReturnPolicyDescription = details.ReturnPolicyDescription,
                //    ReturnValidDays = details.ReturnValidDays,
                //    ReturnValidTillDate = details.ReturnValidTillDate,
                //    BrandName = brand.Name,
                //    ProductImage = string.IsNullOrEmpty(GetImages(details.ProductID).Image) ? null : GetImages(details.ProductID).Image.ToString(),

                //    //SellerKycList SellerDetails = getsellerKyc(details.SellerID),
                //    SellerName = getsellerKyc(details.SellerID).TradeName,
                //    SellerPhoneNo = getsellerKyc(details.SellerID).ContactPersonMobileNo,
                //    SellerEmailId = getsellerKyc(details.SellerID).EmailID,
                //    SellerStatus = getsellerKyc(details.SellerID).SellerStatus,
                //    SellerKycStatus = getsellerKyc(details.SellerID).Status,


                //    //OrderPackageDtoList pckg = GetPackage(details.OrderID, details.Id);
                //    //PackageId = (pckg.Id != null || pckg.Id != 0) ? Convert.ToInt32(pckg.Id) : 0;
                //    //PackageNo = pckg.PackageNo != null ? pckg.PackageNo : null;
                //    //TotalPakedItems = (pckg.TotalItems != null || pckg.TotalItems != 0) ? Convert.ToInt32(pckg.TotalItems) : 0;
                //    //NoOfPackage = (pckg.NoOfPackage != null || pckg.NoOfPackage != 0) ? Convert.ToInt32(pckg.NoOfPackage) : 0;
                //    //PackageAmount = (pckg.PackageAmount != null || pckg.PackageAmount != 0) ? Convert.ToDecimal(pckg.PackageAmount) : 0;
                //    //PackageItemIds = pckg.OrderItemIds != null ? pckg.OrderItemIds : null;
                //}).ToList();
                foreach (var details in ordersdetail)
                {
                    OrderItemDTO orderItems = new OrderItemDTO();
                    orderItems.Id = details.Id;
                    orderItems.OrderID = details.OrderID;
                    orderItems.SubOrderNo = details.SubOrderNo;
                    orderItems.SellerID = details.SellerID;
                    orderItems.BrandID = details.BrandID;
                    orderItems.CategoryId = details.CategoryId;
                    orderItems.ProductID = details.ProductID;
                    orderItems.ProductGUID = details.ProductGUID;
                    orderItems.SellerProductID = details.SellerProductID;
                    orderItems.ProductName = details.ProductName;
                    orderItems.ProductSKUCode = details.ProductSKUCode;
                    orderItems.MRP = details.MRP;
                    orderItems.SellingPrice = details.SellingPrice;
                    orderItems.Discount = details.Discount;
                    orderItems.Qty = details.Qty;
                    orderItems.TotalAmount = details.TotalAmount;
                    orderItems.PriceTypeID = details.PriceTypeID;
                    orderItems.PriceType = details.PriceType;
                    orderItems.SizeID = details.SizeID;
                    orderItems.SizeValue = details.SizeValue;
                    orderItems.IsCouponApplied = details.IsCouponApplied;
                    orderItems.Coupon = details.Coupon;
                    orderItems.CoupontDetails = details.CoupontDetails;
                    orderItems.IsCouponApplied = details.IsCouponApplied;
                    orderItems.Coupon = details.Coupon;
                    orderItems.CoupontDiscount = details.CoupontDiscount;
                    orderItems.CoupontDetails = details.CoupontDetails;
                    orderItems.ShippingZone = details.ShippingZone;
                    orderItems.ShippingCharge = details.ShippingCharge;
                    orderItems.ShippingChargePaidBy = details.ShippingChargePaidBy;
                    orderItems.SubTotal = details.SubTotal;
                    orderItems.Status = details.Status;
                    orderItems.WherehouseId = details.WherehouseId;
                    orderItems.IsReplace = details.IsReplace;
                    orderItems.ParentID = details.ParentID;
                    orderItems.ReturnPolicyName = details.ReturnPolicyName;
                    orderItems.ReturnPolicyTitle = details.ReturnPolicyTitle;
                    orderItems.ReturnPolicyCovers = details.ReturnPolicyCovers;
                    orderItems.ReturnPolicyDescription = details.ReturnPolicyDescription;
                    orderItems.ReturnValidDays = details.ReturnValidDays;
                    orderItems.ReturnValidTillDate = details.ReturnValidTillDate;
                    orderItems.BrandName = brand.Name;

                    orderItems.ProductImage = string.IsNullOrEmpty(GetImages(details.ProductID).Url) ? null : GetImages(details.ProductID).Url.ToString();
                    orderItems.Color = string.Join("-", GetColor(Convert.ToInt32(details.ProductID))?.Select(c => c.ColorName) ?? Enumerable.Empty<string>());

                    SellerKycList SellerDetails = getsellerKyc(details.SellerID);
                    orderItems.SellerName = SellerDetails.TradeName;
                    orderItems.SellerPhoneNo = SellerDetails.ContactPersonMobileNo;
                    orderItems.SellerEmailId = SellerDetails.EmailID;
                    orderItems.SellerStatus = SellerDetails.SellerStatus;
                    orderItems.SellerKycStatus = SellerDetails.Status;
                    orderItems.OrderTaxInfos = oldBindOrderTax(orderId, orderItems.Id);
                    orderItems.ShippmentBy = SellerDetails.ShipmentBy;

                    orderItems.OrderWiseExtraCharges = oldBindOrderWiseExtraCharges(orderId, orderItems.Id);


                    OrderPackageDtoList pckg = GetPackage(details.OrderID, details.Id);
                    orderItems.PackageId = (pckg.Id != null || pckg.Id != 0) ? Convert.ToInt32(pckg.Id) : 0;
                    orderItems.PackageNo = pckg.PackageNo != null ? pckg.PackageNo : null;
                    orderItems.TotalPakedItems = (pckg.TotalItems != null || pckg.TotalItems != 0) ? Convert.ToInt32(pckg.TotalItems) : 0;
                    orderItems.NoOfPackage = (pckg.NoOfPackage != null || pckg.NoOfPackage != 0) ? Convert.ToInt32(pckg.NoOfPackage) : 0;
                    orderItems.PackageAmount = (pckg.PackageAmount != null || pckg.PackageAmount != 0) ? Convert.ToDecimal(pckg.PackageAmount) : 0;
                    orderItems.PackageCodCharges = (pckg.CodCharges != null || pckg.CodCharges != 0) ? Convert.ToDecimal(pckg.CodCharges) : 0;
                    orderItems.PackageItemIds = pckg.OrderItemIds != null ? pckg.OrderItemIds : null;
                    ordersItem.Add(orderItems);
                }
            }
            return ordersItem;
        }

        public List<OrderTaxInfoDTO> oldBindOrderTax(int orderId, int orderItemId)
        {
            BaseResponse<OrderTaxInfoDTO> baseResponse = new BaseResponse<OrderTaxInfoDTO>();
            var GetResponse = helper.ApiCall(OrderURL, EndPoints.OrderTaxInfo + "?OrderId=" + orderId + "&orderItemId=" + orderItemId, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            List<OrderTaxInfoDTO> ordersItem = new List<OrderTaxInfoDTO>();
            List<OrderTaxInfoDTO> ordersdetail = baseResponse.Data as List<OrderTaxInfoDTO>;

            foreach (var details in ordersdetail)
            {
                OrderTaxInfoDTO orderItems = new OrderTaxInfoDTO();
                orderItems.Id = details.Id;
                orderItems.OrderID = details.OrderID;
                orderItems.OrderItemID = details.OrderItemID;
                orderItems.ProductID = details.ProductID;
                orderItems.SellerProductID = details.SellerProductID;
                orderItems.ShippingCharge = 0;
                orderItems.ShippingZone = null;
                orderItems.TaxOnShipping = 0;
                orderItems.CommissionIn = details.CommissionIn;
                orderItems.CommissionRate = details.CommissionRate;
                orderItems.CommissionAmount = details.CommissionAmount;
                orderItems.TaxOnCommission = details.TaxOnCommission;
                orderItems.NetEarn = details.NetEarn;
                orderItems.OrderTaxRateId = details.OrderTaxRateId;
                orderItems.OrderTaxRate = details.OrderTaxRate;
                orderItems.HSNCode = details.HSNCode;
                orderItems.ShipmentBy = null;
                orderItems.ShipmentPaidBy = null;
                orderItems.IsSellerWithGSTAtOrderTime = details.IsSellerWithGSTAtOrderTime;
                orderItems.WeightSlab = details.WeightSlab;

                ordersItem.Add(orderItems);
            }
            return ordersItem;
        }


        public List<OrderWiseExtraChargesDTO> oldBindOrderWiseExtraCharges(int orderId, int orderItemId)
        {
            BaseResponse<OrderWiseExtraChargesDTO> baseResponse = new BaseResponse<OrderWiseExtraChargesDTO>();
            var GetResponse = helper.ApiCall(OrderURL, EndPoints.OrderWiseExtraCharges + "?OrderId=" + orderId + "&orderItemId=" + orderItemId, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            List<OrderWiseExtraChargesDTO> ordersItem = new List<OrderWiseExtraChargesDTO>();
            List<OrderWiseExtraChargesDTO> ordersdetail = baseResponse.Data as List<OrderWiseExtraChargesDTO>;

            foreach (var details in ordersdetail)
            {
                OrderWiseExtraChargesDTO orderItems = new OrderWiseExtraChargesDTO();
                orderItems.Id = details.Id;
                orderItems.OrderID = details.OrderID;
                orderItems.OrderItemID = details.OrderItemID;
                orderItems.ChargesType = details.ChargesType;
                orderItems.ChargesPaidBy = details.ChargesPaidBy;
                orderItems.ChargesIn = details.ChargesIn;
                orderItems.ChargesValueInPercentage = details.ChargesValueInPercentage;
                orderItems.ChargesValueInAmount = details.ChargesValueInAmount;
                orderItems.ChargesMaxAmount = details.ChargesMaxAmount;
                orderItems.TaxOnChargesAmount = details.TaxOnChargesAmount;
                orderItems.TotalCharges = details.TotalCharges;
                orderItems.ChargesAmountWithoutTax = details.ChargesAmountWithoutTax;

                ordersItem.Add(orderItems);
            }
            return ordersItem;
        }


        public OrderItemDTO oldgetOrderItemsDetails(int OrderItemId, string? SellerId = null, string? Status = null)
        {
            string url = "";
            if (!string.IsNullOrEmpty(SellerId))
            {
                url = url + "&SellerId=" + SellerId;
            }

            if (!string.IsNullOrEmpty(Status))
            {
                url = url + "&Status=" + Status;
            }

            BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
            var GetResponse = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?Id=" + OrderItemId + url, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(GetResponse);
            OrderItemDTO orderItems = new OrderItemDTO();
            OrderItems details = baseResponse.Data as OrderItems;

            if (details != null && details.Id != 0 && details.Id != null)
            {
                BrandLibrary brand = getBrand(Convert.ToInt32(details.BrandID));
                orderItems.Id = details.Id;
                orderItems.OrderID = details.OrderID;
                orderItems.SubOrderNo = details.SubOrderNo;
                orderItems.SellerID = details.SellerID;
                orderItems.BrandID = details.BrandID;
                orderItems.CategoryId = details.CategoryId;
                orderItems.ProductID = details.ProductID;
                orderItems.ProductGUID = details.ProductGUID;
                orderItems.SellerProductID = details.SellerProductID;
                orderItems.ProductName = details.ProductName;
                orderItems.ProductSKUCode = details.ProductSKUCode;
                orderItems.MRP = details.MRP;
                orderItems.SellingPrice = details.SellingPrice;
                orderItems.Discount = details.Discount;
                orderItems.Qty = details.Qty;
                orderItems.TotalAmount = details.TotalAmount;
                orderItems.PriceTypeID = details.PriceTypeID;
                orderItems.PriceType = details.PriceType;
                orderItems.SizeID = details.SizeID;
                orderItems.SizeValue = details.SizeValue;
                orderItems.IsCouponApplied = details.IsCouponApplied;
                orderItems.Coupon = details.Coupon;
                orderItems.CoupontDetails = details.CoupontDetails;
                orderItems.IsCouponApplied = details.IsCouponApplied;
                orderItems.Coupon = details.Coupon;
                orderItems.CoupontDiscount = details.CoupontDiscount;
                orderItems.CoupontDetails = details.CoupontDetails;
                orderItems.ShippingZone = details.ShippingZone;
                orderItems.ShippingCharge = details.ShippingCharge;
                orderItems.ShippingChargePaidBy = details.ShippingChargePaidBy;
                orderItems.SubTotal = details.SubTotal;
                orderItems.Status = details.Status;
                orderItems.WherehouseId = details.WherehouseId;
                orderItems.IsReplace = details.IsReplace;
                orderItems.ParentID = details.ParentID;
                orderItems.ReturnPolicyName = details.ReturnPolicyName;
                orderItems.ReturnPolicyTitle = details.ReturnPolicyTitle;
                orderItems.ReturnPolicyCovers = details.ReturnPolicyCovers;
                orderItems.ReturnPolicyDescription = details.ReturnPolicyDescription;
                orderItems.ReturnValidDays = details.ReturnValidDays;
                orderItems.ReturnValidTillDate = details.ReturnValidTillDate;
                orderItems.BrandName = brand.Name;
                orderItems.ProductImage = string.IsNullOrEmpty(GetImages(details.ProductID).Url) ? null : GetImages(details.ProductID).Url.ToString();
                orderItems.ProductImage = string.IsNullOrEmpty(GetImages(details.ProductID).Type) ? null : GetImages(details.ProductID).Type.ToString();
                orderItems.Color = string.Join("-", GetColor(Convert.ToInt32(details.ProductID))?.Select(c => c.ColorName) ?? Enumerable.Empty<string>());

                SellerKycList SellerDetails = getsellerKyc(details.SellerID);
                orderItems.SellerName = SellerDetails.TradeName;
                orderItems.SellerPhoneNo = SellerDetails.ContactPersonMobileNo;
                orderItems.SellerEmailId = SellerDetails.EmailID;
                orderItems.SellerStatus = SellerDetails.SellerStatus;
                orderItems.SellerKycStatus = SellerDetails.Status;
                orderItems.OrderTaxInfos = oldBindOrderTax(details.OrderID, OrderItemId);

                orderItems.OrderWiseExtraCharges = oldBindOrderWiseExtraCharges(details.OrderID, OrderItemId);

                OrderPackageDtoList pckg = GetPackage(details.OrderID, details.Id);
                orderItems.PackageId = (pckg.Id != null || pckg.Id != 0) ? Convert.ToInt32(pckg.Id) : 0;
                orderItems.PackageNo = pckg.PackageNo != null ? pckg.PackageNo : null;
                orderItems.TotalPakedItems = (pckg.TotalItems != null || pckg.TotalItems != 0) ? Convert.ToInt32(pckg.TotalItems) : 0;
                orderItems.NoOfPackage = (pckg.NoOfPackage != null || pckg.NoOfPackage != 0) ? Convert.ToInt32(pckg.NoOfPackage) : 0;
                orderItems.PackageAmount = (pckg.PackageAmount != null || pckg.PackageAmount != 0) ? Convert.ToDecimal(pckg.PackageAmount) : 0;
                orderItems.PackageItemIds = pckg.OrderItemIds != null ? pckg.OrderItemIds : null;

            }

            return orderItems;
        }

        public OrderDetails BindOrderDetails(string orderId)
        {
            BaseResponse<Orders> baseResponse = new BaseResponse<Orders>();
            var GetResponse = helper.ApiCall(OrderURL, EndPoints.Orders + "?OrderNo=" + orderId, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(GetResponse);

            OrderDetails orderDetails = new OrderDetails();
            Orders details = baseResponse.Data as Orders;

            orderDetails.OrderId = details.Id;
            orderDetails.OrderNo = details.OrderNo;
            //orderDetails.SellerId = details.SellerID;
            orderDetails.UserId = details.UserId;
            orderDetails.UserName = details.UserName;
            orderDetails.UserPhoneNo = details.UserPhoneNo;
            orderDetails.UserEmail = details.UserEmail;
            orderDetails.UserAddressLine1 = details.UserAddressLine1;
            orderDetails.UserAddressLine2 = details.UserAddressLine2;
            orderDetails.UserLendMark = details.UserLandmark;
            orderDetails.UserPincode = details.UserPincode;
            orderDetails.UserCity = details.UserCity;
            orderDetails.UserState = details.UserState;
            orderDetails.UserCountry = details.UserCountry;
            orderDetails.UserGSTNo = details.UserGSTNo;
            orderDetails.PaymentMode = details.PaymentMode;

            orderDetails.TotalShippingCharge = details.TotalShippingCharge;
            orderDetails.TotalExtraCharges = details.TotalExtraCharges;
            orderDetails.TotalAmount = details.TotalAmount;
            orderDetails.IsCouponApplied = details.IsCouponApplied;
            orderDetails.Coupon = details.Coupon;
            orderDetails.CoupontDiscount = details.CoupontDiscount;
            orderDetails.CoupontDetails = details.CoupontDetails;
            orderDetails.CODCharge = details.CODCharge != null ? (decimal)details.CODCharge : 0;
            orderDetails.PaidAmount = details.PaidAmount;
            orderDetails.IsSale = details.IsSale;
            orderDetails.SaleType = details.SaleType;
            orderDetails.OrderDate = (DateTime)details.OrderDate;
            orderDetails.DeliveryDate = (DateTime)details.DeliveryDate;
            orderDetails.Status = details.Status;
            orderDetails.PaymentInfo = details.PaymentInfo;
            orderDetails.OrderBy = details.OrderBy;
            orderDetails.IsRetailer = details.IsRetailer;
            orderDetails.IsVertualRetailer = details.IsVertualRetailer;
            orderDetails.IsReplace = details.IsReplace;
            orderDetails.ParentId = (int)details.ParentID;
            orderDetails.OrderReferenceNo = details.OrderReferenceNo;

            //SellerKycList SellerDetails = getsellerKyc(details.SellerID);
            //orderDetails.SellerName = SellerDetails.TradeName;
            //orderDetails.SellerPhoneNo = SellerDetails.ContactPersonMobileNo;
            //orderDetails.SellerEmailId = SellerDetails.EmailID;
            //orderDetails.SellerStatus = SellerDetails.SellerStatus;
            //orderDetails.SellerKycStatus = SellerDetails.Status;

            orderDetails.OrderItems = BindOrderItems(details.Id);

            return orderDetails;
        }

        public List<OrderDetails> BindOrderDetailsByRefNo(string OrderRefNo)
        {
            BaseResponse<Orders> baseResponse = new BaseResponse<Orders>();
            var GetResponse = helper.ApiCall(OrderURL, EndPoints.Orders + "?OrderRefNo=" + OrderRefNo, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            List<OrderDetails> lstorderDetails = new List<OrderDetails>();
            List<Orders> orderList = baseResponse.Data as List<Orders>;

            foreach (Orders details in orderList)
            {
                OrderDetails orderDetails = new OrderDetails();

                orderDetails.OrderId = details.Id;
                orderDetails.OrderNo = details.OrderNo;
                //orderDetails.SellerId = details.SellerID;
                orderDetails.UserId = details.UserId;
                orderDetails.UserName = details.UserName;
                orderDetails.UserPhoneNo = details.UserPhoneNo;
                orderDetails.UserEmail = details.UserEmail;
                orderDetails.UserAddressLine1 = details.UserAddressLine1;
                orderDetails.UserAddressLine2 = details.UserAddressLine2;
                orderDetails.UserLendMark = details.UserLandmark;
                orderDetails.UserPincode = details.UserPincode;
                orderDetails.UserCity = details.UserCity;
                orderDetails.UserState = details.UserState;
                orderDetails.UserCountry = details.UserCountry;
                orderDetails.UserGSTNo = details.UserGSTNo;
                orderDetails.PaymentMode = details.PaymentMode;

                orderDetails.TotalShippingCharge = details.TotalShippingCharge;
                orderDetails.TotalExtraCharges = details.TotalExtraCharges;
                orderDetails.TotalAmount = details.TotalAmount;
                orderDetails.IsCouponApplied = details.IsCouponApplied;
                orderDetails.Coupon = details.Coupon;
                orderDetails.CoupontDiscount = details.CoupontDiscount;
                orderDetails.CoupontDetails = details.CoupontDetails;
                orderDetails.CODCharge = details.CODCharge != null ? (decimal)details.CODCharge : 0;
                orderDetails.PaidAmount = details.PaidAmount;
                orderDetails.IsSale = details.IsSale;
                orderDetails.SaleType = details.SaleType;
                orderDetails.OrderDate = (DateTime)details.OrderDate;
                orderDetails.DeliveryDate = (DateTime)details.DeliveryDate;
                orderDetails.Status = details.Status;
                orderDetails.PaymentInfo = details.PaymentInfo;
                orderDetails.OrderBy = details.OrderBy;
                orderDetails.IsRetailer = details.IsRetailer;
                orderDetails.IsVertualRetailer = details.IsVertualRetailer;
                orderDetails.IsReplace = details.IsReplace;
                orderDetails.ParentId = (int)details.ParentID;
                orderDetails.OrderReferenceNo = details.OrderReferenceNo;

                //SellerKycList SellerDetails = getsellerKyc(details.SellerID);
                //orderDetails.SellerName = SellerDetails.TradeName;
                //orderDetails.SellerPhoneNo = SellerDetails.ContactPersonMobileNo;
                //orderDetails.SellerEmailId = SellerDetails.EmailID;
                //orderDetails.SellerStatus = SellerDetails.SellerStatus;
                //orderDetails.SellerKycStatus = SellerDetails.Status;

                orderDetails.OrderItems = BindOrderItems(details.Id);

                lstorderDetails.Add(orderDetails);

            }
            return lstorderDetails;
        }

        public SellerKycList getsellerKyc(string sellerId)
        {
            UserDetailsDTO sellerKycList = new UserDetailsDTO();
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            List<UserDetailsDTO> lst = seller.bindSellerDetails(null, null, null, null, sellerId, 0, 0);
            sellerKycList = lst.FirstOrDefault();

            SellerKycList slst = new SellerKycList();

            GSTInfo GSTDetails = getGSTDetails(sellerKycList);

            slst.Id = sellerKycList.Id;
            slst.FirstName = sellerKycList.FirstName;
            slst.LastName = sellerKycList.LastName;
            slst.FullName = sellerKycList.FirstName + " " + sellerKycList.LastName;
            slst.EmailID = sellerKycList.Email;
            slst.PhoneNumber = sellerKycList.Phone;
            slst.ProfileImage = sellerKycList.ProfileImage;
            slst.SellerStatus = sellerKycList.UserStatus;
            slst.IsEmailConfirmed = sellerKycList.IsEmailConfirmed;
            slst.IsPhoneConfirmed = sellerKycList.IsPhoneConfirmed;
            slst.KycId = sellerKycList.KYCDetailsId;
            slst.KYCFor = sellerKycList.KYCFor;
            slst.DisplayName = sellerKycList.DisplayName;
            slst.OwnerName = sellerKycList.OwnerName;
            slst.ContactPersonName = sellerKycList.ContactPersonName;
            slst.ContactPersonMobileNo = sellerKycList.ContactPersonMobileNo;
            slst.PanCardNo = sellerKycList.PanCardNo;
            slst.NameOnPanCard = sellerKycList.NameOnPanCard;
            slst.DateOfBirth = sellerKycList.DateOfBirth;
            slst.AadharCardNo = sellerKycList.AadharCardNo;
            slst.IsUserWithGST = sellerKycList.IsUserWithGST;
            slst.TypeOfCompany = sellerKycList.TypeOfCompany;
            slst.CompanyRegistrationNo = sellerKycList.CompanyRegistrationNo;
            slst.BussinessType = sellerKycList.BussinessType;
            slst.MSMENo = sellerKycList.MSMENo;
            slst.Logo = sellerKycList.Logo;
            slst.DigitalSign = sellerKycList.DigitalSign;
            slst.ShipmentBy = sellerKycList.ShipmentBy;
            slst.ShipmentChargesPaidBy = sellerKycList.ShipmentChargesPaidBy;
            slst.ShipmentChargesPaidByName = sellerKycList.ShipmentChargesPaidByName;
            slst.Note = sellerKycList.Note;
            slst.Status = sellerKycList.UserStatus;
            slst.TradeName = GSTDetails.TradeName;
            slst.LegalName = GSTDetails.LegalName;
            slst.GSTNo = GSTDetails.GSTNo;
            slst.GSTType = GSTDetails.GSTType;
            slst.RegisteredAddressLine1 = GSTDetails.RegisteredAddressLine1;
            slst.RegisteredAddressLine2 = GSTDetails.RegisteredAddressLine2;
            slst.RegisteredLandmark = GSTDetails.RegisteredLandmark;
            slst.RegisteredPincode = GSTDetails.RegisteredPincode;
            slst.City = GSTDetails.CityName;
            slst.State = GSTDetails.StateName;
            slst.Country = GSTDetails.CountryName;
            slst.GSTStatus = GSTDetails.Status;


            return slst;
        }

        public GSTInfo getGSTDetails(UserDetailsDTO sellerKycList)
        {
            List<GSTInfo> gSTInfolst = new List<GSTInfo>();
            GSTInfo gSTInfo = new GSTInfo();
            string json = sellerKycList.GSTInfoDetails.ToString();
            gSTInfolst = JsonConvert.DeserializeObject<List<GSTInfo>>(json);
            if (gSTInfolst.Count > 0)
            {
                gSTInfo = gSTInfolst.Where(p => p.IsHeadOffice == true).FirstOrDefault();
            }
            return gSTInfo;
        }

        public List<SellerKycList> getsellerKyclst()
        {
            List<SellerKycList> sellerKycLists = new List<SellerKycList>();
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            List<GSTInfo> gSTInfolst = new List<GSTInfo>();
            GSTInfo gSTInfo = new GSTInfo();

            List<UserDetailsDTO> lst = seller.bindSellerDetails(null, null, null, null, null, 0, 0);

            foreach (UserDetailsDTO sellerr in lst)
            {
                SellerKycList slst = new SellerKycList();
                string json = sellerr.GSTInfoDetails.ToString();
                gSTInfolst = JsonConvert.DeserializeObject<List<GSTInfo>>(json);
                if (gSTInfolst.Count > 0)
                {
                    gSTInfo = gSTInfolst.Where(p => p.IsHeadOffice == true).FirstOrDefault();
                }

                slst.Id = sellerr.Id;
                slst.FirstName = sellerr.FirstName;
                slst.LastName = sellerr.LastName;
                slst.FullName = sellerr.FirstName + " " + sellerr.LastName;
                slst.EmailID = sellerr.Email;
                slst.PhoneNumber = sellerr.Phone;
                slst.ProfileImage = sellerr.ProfileImage;
                slst.SellerStatus = sellerr.UserStatus;
                slst.IsEmailConfirmed = sellerr.IsEmailConfirmed;
                slst.IsPhoneConfirmed = sellerr.IsPhoneConfirmed;
                slst.KycId = sellerr.KYCDetailsId;
                slst.KYCFor = sellerr.KYCFor;
                slst.DisplayName = sellerr.DisplayName;
                slst.OwnerName = sellerr.OwnerName;
                slst.ContactPersonName = sellerr.ContactPersonName;
                slst.ContactPersonMobileNo = sellerr.ContactPersonMobileNo;
                slst.PanCardNo = sellerr.PanCardNo;
                slst.NameOnPanCard = sellerr.NameOnPanCard;
                slst.DateOfBirth = sellerr.DateOfBirth;
                slst.AadharCardNo = sellerr.AadharCardNo;
                slst.IsUserWithGST = sellerr.IsUserWithGST;
                slst.TypeOfCompany = sellerr.TypeOfCompany;
                slst.CompanyRegistrationNo = sellerr.CompanyRegistrationNo;
                slst.BussinessType = sellerr.BussinessType;
                slst.MSMENo = sellerr.MSMENo;
                slst.Logo = sellerr.Logo;
                slst.DigitalSign = sellerr.DigitalSign;
                slst.ShipmentBy = sellerr.ShipmentBy;
                slst.ShipmentChargesPaidBy = sellerr.ShipmentChargesPaidBy;
                slst.ShipmentChargesPaidByName = sellerr.ShipmentChargesPaidByName;
                slst.Note = sellerr.Note;
                slst.Status = sellerr.UserStatus;
                slst.TradeName = gSTInfo.TradeName;
                slst.LegalName = gSTInfo.LegalName;
                slst.GSTNo = gSTInfo.GSTNo;
                slst.GSTType = gSTInfo.GSTType;
                slst.RegisteredAddressLine1 = gSTInfo.RegisteredAddressLine1;
                slst.RegisteredAddressLine2 = gSTInfo.RegisteredAddressLine2;
                slst.RegisteredLandmark = gSTInfo.RegisteredLandmark;
                slst.RegisteredPincode = gSTInfo.RegisteredPincode;
                slst.City = gSTInfo.CityName;
                slst.State = gSTInfo.StateName;
                slst.Country = gSTInfo.CountryName;
                slst.GSTStatus = gSTInfo.Status;

                sellerKycLists.Add(slst);
            }

            return sellerKycLists;
        }

        public BrandLibrary getBrand(int brandId)
        {
            BaseResponse<BrandLibrary> brandResponse = new BaseResponse<BrandLibrary>();
            var response = helper.ApiCall(UserURL, EndPoints.Brand + "?Id=" + brandId, "GET", null);
            brandResponse = brandResponse.JsonParseRecord(response);

            BrandLibrary brand = new BrandLibrary();
            brand = brandResponse.Data as BrandLibrary;

            return brand;
        }

        public ProductImages GetImages(int productId)
        {
            BaseResponse<ProductImages> brandResponse = new BaseResponse<ProductImages>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductsImage + "?ProductID=" + productId, "GET", null);
            brandResponse = brandResponse.JsonParseList(response);

            List<ProductImages> imglist = new List<ProductImages>();
            imglist = brandResponse.Data as List<ProductImages>;

            ProductImages Images = new ProductImages();
            if (imglist != null && imglist.Count > 0)
            {
                Images = imglist.OrderBy(p => p.Sequence).FirstOrDefault();
            }

            return Images;
        }


        public OrderTaxInfo getOrderTaxinfo(int orderId, int OrderItemId)
        {
            BaseResponse<OrderTaxInfo> baseresponse = new BaseResponse<OrderTaxInfo>();
            var response = helper.ApiCall(OrderURL, EndPoints.OrderTaxInfo + "?orderId=" + orderId + "&OrderItemId=" + OrderItemId, "GET", null);
            baseresponse = baseresponse.JsonParseList(response);

            OrderTaxInfo orderTaxInfo = new OrderTaxInfo();
            orderTaxInfo = baseresponse.Data as OrderTaxInfo;

            return orderTaxInfo;

        }


        public OrderPackageDtoList GetPackage(int OrderId,int orderItemId)
        {
            BaseResponse<OrderPackage> pckgResponse = new BaseResponse<OrderPackage>();
            var response = helper.ApiCall(OrderURL, EndPoints.OrderPackage + "?Orderid=" + OrderId + "&OrderItemIDs=" + orderItemId, "GET", null);
            pckgResponse = pckgResponse.JsonParseRecord(response);

            OrderPackage pckg = new OrderPackage();
            OrderPackageDtoList packlst = new OrderPackageDtoList();
            if (pckgResponse.code == 200)
            {
                pckg = pckgResponse.Data as OrderPackage;

                packlst.Id = pckg.Id;
                packlst.TotalItems = pckg.TotalItems;
                packlst.NoOfPackage = pckg.NoOfPackage;
                packlst.PackageNo = pckg.PackageNo;
                packlst.PackageAmount = pckg.PackageAmount;
                packlst.OrderItemIds = pckg.OrderItemIDs;
                packlst.CodCharges = pckg.CodCharges;
            }
            return packlst;
        }

        public Warehouse getWarehouse(int Warehouseid)
        {
            BaseResponse<Warehouse> baseResponse = new BaseResponse<Warehouse>();
            var response = helper.ApiCall(UserURL, EndPoints.Warehouse + "?Id=" + Warehouseid, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            Warehouse warehouse = new Warehouse();
            if (baseResponse.code == 200)
            {
                warehouse = baseResponse.Data as Warehouse;
            }
            return warehouse;
        }


        public InvoicedataDTO GetInvoice(string Packageid)
        {

            InvoicedataDTO invoicedataDTO = new InvoicedataDTO();
            List<InvoiceDto> Invoice = new List<InvoiceDto>();
            BaseResponse<InvoiceDto> baseResponse = new BaseResponse<InvoiceDto>();


            var response = helper.ApiCall(OrderURL, EndPoints.Orders + "/Invoice?Packageid=" + Packageid, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            if (baseResponse.code == 200)
            {
                Invoice = baseResponse.Data as List<InvoiceDto>;

                List<InvoiceProductDTO> InvoiceProduct = BindProduct(Invoice);

                invoicedataDTO = Invoice.Select(x => new InvoicedataDTO
                {
                    Id = x.Id,
                    OrderID = x.OrderID,
                    OrderNo = x.OrderNo,
                    OrderItemIDs = x.OrderItemIDs,
                    InvoiceNo = x.InvoiceNo,
                    SellerTradeName = x.SellerTradeName,
                    SellerLegalName = x.SellerLegalName,
                    SellerGSTNo = x.SellerGSTNo,
                    SellerRegisteredAddressLine1 = x.SellerRegisteredAddressLine1,
                    SellerRegisteredAddressLine2 = x.SellerRegisteredAddressLine2,
                    SellerRegisteredLandmark = x.SellerRegisteredLandmark,
                    SellerRegisteredPincode = x.SellerRegisteredPincode,
                    SellerRegisteredCity = x.SellerRegisteredCity,
                    SellerRegisteredState = x.SellerRegisteredState,
                    SellerRegisteredCountry = x.SellerRegisteredCountry,
                    SellerPickupAddressLine1 = x.SellerPickupAddressLine1,
                    SellerPickupAddressLine2 = x.SellerPickupAddressLine2,
                    SellerPickupLandmark = x.SellerPickupLandmark,
                    SellerPickupPincode = x.SellerPickupPincode,
                    SellerPickupCity = x.SellerPickupCity,
                    SellerPickupState = x.SellerPickupState,
                    SellerPickupCountry = x.SellerPickupCountry,
                    SellerPickupContactPersonName = x.SellerPickupContactPersonName,
                    SellerPickupContactPersonMobileNo = x.SellerPickupContactPersonMobileNo,
                    InvoiceAmount = x.InvoiceAmount,
                    InvoiceDate = x.InvoiceDate,
                    IsCouponApplied = x.IsCouponApplied,
                    Coupon = x.Coupon,
                    CoupontDiscount = x.CoupontDiscount,
                    CoupontDetails = x.CoupontDetails,
                    Status = x.Status,
                    OrderDate = x.OrderDate,
                    DropContactPersonName = x.DropContactPersonName,
                    DropContactPersonMobileNo = x.DropContactPersonMobileNo,
                    DropContactPersonEmailID = x.DropContactPersonEmailID,
                    DropCompanyName = x.DropCompanyName,
                    DropAddressLine1 = x.DropAddressLine1,
                    DropAddressLine2 = x.DropAddressLine2,
                    DropLandmark = x.DropLandmark,
                    DropPincode = x.DropPincode,
                    DropCity = x.DropCity,
                    DropState = x.DropState,
                    DropCountry = x.DropCountry,
                    PaymentMode = x.PaymentMode,
                    ShippingZone = x.ShippingZone,
                    ShippingCharge = x.ShippingCharge,
                    ShippingChargePaidBy = x.ShippingChargePaidBy,
                    InvoiceProduct = InvoiceProduct,
                    TotalSubAmount = InvoiceProduct.Sum(x => x.TotalAmount),
                    AwbNo = x.AwbNo,
                    CourierName = x.CourierName,
                    ShippingPartner = x.ShippingPartner,
                    CODCharge = x.CODCharge,
                    TaxAbleCODCharge = Convert.ToDecimal(Convert.ToDecimal((Convert.ToDecimal(x.CODCharge) / (1 + 18 / 100))).ToString("N2")),
                    ITaxOnCODCharge = (x.DropState == x.SellerPickupState ? (18 / 100.0m) * x.SellingPrice : 0.00m),
                    STaxOnCODCharge = (x.DropState == x.SellerPickupState ? (9 / 100.0m) * x.SellingPrice : 0.00m),
                    CTaxOnCODCharge = (x.DropState != x.SellerPickupState ? (9 / 100.0m) * x.SellingPrice : 0.00m),
                    ExtrachargesName = x.ExtrachargesName,
                    TotalExtracharges = x.TotalExtracharges

                }).FirstOrDefault();

            }

            return invoicedataDTO;

        }

        public List<InvoiceProductDTO> BindProduct(List<InvoiceDto> ss)
        {

            List<InvoiceDto> tempList = (List<InvoiceDto>)ss;



            var res = tempList.Select(x => new InvoiceProductDTO
            {
                SellerID = x.SellerID,
                BrandID = x.BrandID,
                BrandName = getBrand(Convert.ToInt32(x.BrandID)).Name.ToString(),
                ProductID = x.ProductID,
                SellerProductID = x.SellerProductID,
                ProductName = x.ProductName,
                ProductSKUCode = x.ProductSKUCode,
                MRP = x.MRP,
                SellingPrice = x.SellingPrice,
                Discount = x.Discount,
                Qty = x.Qty,
                TotalAmount = x.TotalAmount,
                PriceTypeID = x.PriceTypeID,
                PriceType = x.PriceType,
                SizeID = x.SizeID,
                SizeValue = x.SizeValue,
                OrderTaxRate = x.OrderTaxRate,
                CGST = JObject.Parse(x.OrderTaxRate)["CGST"]?.Value<decimal>(),
                SGST = JObject.Parse(x.OrderTaxRate)["SGST"]?.Value<decimal>(),
                IGST = JObject.Parse(x.OrderTaxRate)["IGST"]?.Value<decimal>(),
                TaxableAmount = Convert.ToDecimal(Convert.ToDecimal((Convert.ToDecimal(x.SellingPrice) / (1 + (Convert.ToDecimal(JObject.Parse(x.OrderTaxRate)["IGST"]?.Value<decimal>()) / 100)))).ToString("N2")),
                CGSTAmount = (x.DropState == x.SellerPickupState ? (JObject.Parse(x.OrderTaxRate)["CGST"]?.Value<decimal>() / 100.0m) * x.SellingPrice : 0.00m),
                SGSTAmount = (x.DropState == x.SellerPickupState ? (JObject.Parse(x.OrderTaxRate)["SGST"]?.Value<decimal>() / 100.0m) * x.SellingPrice : 0.00m),
                IGSTAmount = (x.DropState != x.SellerPickupState ? (JObject.Parse(x.OrderTaxRate)["IGST"]?.Value<decimal>() / 100.0m) * x.SellingPrice : 0.00m),
                HSNCode = x.HSNCode,
                SubTotal = x.SubTotal,
                Color = string.Join("-", GetColor(Convert.ToInt32(x.ProductID))?.Select(c => c.ColorName) ?? Enumerable.Empty<string>())

            });

            return res.ToList();
        }

        public List<ShippingLabelDto> GetShippingLabel(string Packageid)
        {

            List<ShippingLabelDto> shippingdataDTO = new List<ShippingLabelDto>();
            List<InvoiceDto> Invoice = new List<InvoiceDto>();
            BaseResponse<InvoiceDto> baseResponse = new BaseResponse<InvoiceDto>();


            var response = helper.ApiCall(OrderURL, EndPoints.Orders + "/Invoice?Packageid=" + Packageid, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            if (baseResponse.code == 200)
            {
                Invoice = baseResponse.Data as List<InvoiceDto>;

                shippingdataDTO = Invoice.Select(x => new ShippingLabelDto
                {

                    OrderID = x.OrderID,
                    OrderNo = x.OrderNo,
                    SellerTradeName = x.SellerTradeName,
                    SellerLegalName = x.SellerLegalName,
                    SellerPickupAddressLine1 = x.SellerPickupAddressLine1,
                    SellerPickupAddressLine2 = x.SellerPickupAddressLine2,
                    SellerPickupLandmark = x.SellerPickupLandmark,
                    SellerPickupPincode = x.SellerPickupPincode,
                    SellerPickupCity = x.SellerPickupCity,
                    SellerPickupState = x.SellerPickupState,
                    SellerPickupCountry = x.SellerPickupCountry,
                    SellerPickupContactPersonName = x.SellerPickupContactPersonName,
                    SellerPickupContactPersonMobileNo = x.SellerPickupContactPersonMobileNo,
                    SellerPickupTaxNo = x.SellerPickupTaxNo,
                    InvoiceAmount = x.InvoiceAmount,
                    DropContactPersonName = x.DropContactPersonName,
                    DropContactPersonMobileNo = x.DropContactPersonMobileNo,
                    DropContactPersonEmailID = x.DropContactPersonEmailID,
                    DropCompanyName = x.DropCompanyName,
                    DropAddressLine1 = x.DropAddressLine1,
                    DropAddressLine2 = x.DropAddressLine2,
                    DropLandmark = x.DropLandmark,
                    DropPincode = x.DropPincode,
                    DropCity = x.DropCity,
                    DropState = x.DropState,
                    DropCountry = x.DropCountry,
                    DropTaxNo = x.DropTaxNo,
                    PaymentMode = x.PaymentMode,
                    AwbNo = x.AwbNo,
                    ShippingPartner = x.ShippingPartner,
                    CourierName = x.CourierName,
                    NoOfPackage = x.NoOfPackage,
                    Weight = x.Weight,
                    ShippingDate = x.ShippingDate,
                    OrderDate = x.OrderDate
                }).ToList();

            }

            return shippingdataDTO;

        }

        public BaseResponse<OrderCancelReturn> GetOrderReturn(int? Id = 0, int? OrderID = 0, int? BrandId = 0, int? OrderItemID = 0, string? OrderNo = null, string? SellerId = null, string? NewOrderNo = null, int? ActionID = 0, string? UserId = null, string? Status = null, string? RefundStatus = null, bool? withCancel = false, bool? Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? searchText = null)
        {
            string url = string.Empty;

            if (Id != 0)
            {
                url += "&Id=" + Id;
            }

            if (OrderID != 0)
            {
                url += "&OrderID=" + OrderID;
            }

            if (BrandId != 0)
            {
                url += "&BrandId=" + BrandId;
            }

            if (OrderItemID != 0)
            {
                url += "&OrderItemID=" + OrderItemID;
            }
            if (ActionID != 0)
            {
                url += "&ActionID=" + ActionID;
            }

            if (!string.IsNullOrEmpty(OrderNo))
            {
                url += "&OrderNo=" + OrderNo;
            }
            if (!string.IsNullOrEmpty(SellerId))
            {
                url += "&SellerId=" + SellerId;
            }
            if (!string.IsNullOrEmpty(NewOrderNo))
            {
                url += "&NewOrderNo=" + NewOrderNo;
            }
            if (!string.IsNullOrEmpty(UserId))
            {
                url += "&UserId=" + UserId;
            }

            if (!string.IsNullOrEmpty(Status))
            {
                url += "&Status=" + Status;
            }

            if (!string.IsNullOrEmpty(RefundStatus))
            {
                url += "&RefundStatus=" + RefundStatus;
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                url += "&searchText=" + HttpUtility.UrlEncode(searchText);
            }
            if (Isdeleted != null)
            {
                url += "&Isdeleted=" + Isdeleted;
            }
            if (withCancel != null)
            {
                url += "&withCancel=" + withCancel;
            }

            BaseResponse<OrderCancelReturn> BaseResponse = new BaseResponse<OrderCancelReturn>();

            var temp = helper.ApiCall(OrderURL, EndPoints.CancelReturn + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
            BaseResponse = BaseResponse.JsonParseList(temp);
            return BaseResponse;

        }

        public BaseResponse<OrderRefund> GetOrderRefund(int? Id = 0, int? OrderID = 0, int? OrderCancelReturnID = 0, int? OrderItemID = 0, string? TransactionID = null, string? Status = null, bool? Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? searchText = null)
        {
            string url = string.Empty;

            if (Id != 0)
            {
                url += "&Id=" + Id;
            }

            if (OrderID != 0)
            {
                url += "&OrderID=" + OrderID;
            }

            if (OrderCancelReturnID != 0)
            {
                url += "&OrderCancelReturnID=" + OrderCancelReturnID;
            }

            if (OrderItemID != 0)
            {
                url += "&OrderItemID=" + OrderItemID;
            }
            if (!string.IsNullOrEmpty(TransactionID))
            {
                url += "&TransactionID=" + TransactionID;
            }

            if (!string.IsNullOrEmpty(Status))
            {
                url += "&Status=" + Status;
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                url += "&searchText=" + HttpUtility.UrlEncode(searchText);
            }
            if (Isdeleted != null)
            {
                url += "&Isdeleted=" + Isdeleted;
            }

            BaseResponse<OrderRefund> BaseResponse = new BaseResponse<OrderRefund>();

            var temp = helper.ApiCall(OrderURL, EndPoints.OrderRefund + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
            BaseResponse = BaseResponse.JsonParseList(temp);
            return BaseResponse;

        }

        public BaseResponse<OrderInvoice> GetInvoiceList(int? packageId = 0, int? OrderId = 0, string? sellerId = null, string? invoiceNo = null, int PageIndex = 1, int PageSize = 10, string? searchText = null)
        {
            string url = string.Empty;

            if (packageId != 0)
            {
                url += "&PackageID=" + packageId;
            }

            if (OrderId != 0)
            {
                url += "&OrderID=" + OrderId;
            }

            if (!string.IsNullOrEmpty(sellerId))
            {
                url += "&SellerId=" + sellerId;
            }

            if (!string.IsNullOrEmpty(invoiceNo))
            {
                url += "&InvoiceNo=" + invoiceNo;
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                url += "&searchText=" + HttpUtility.UrlEncode(searchText);
            }

            BaseResponse<OrderInvoice> BaseResponse = new BaseResponse<OrderInvoice>();

            var temp = helper.ApiCall(OrderURL, EndPoints.Invoice + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
            BaseResponse = BaseResponse.JsonParseList(temp);
            return BaseResponse;

        }

        public List<ProductColorMapp> GetColor(int productId)
        {
            BaseResponse<ProductColorMapp> brandResponse = new BaseResponse<ProductColorMapp>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductColorMapping + "?ProductID=" + productId, "GET", null);
            brandResponse = brandResponse.JsonParseList(response);

            List<ProductColorMapp> colorlist = new List<ProductColorMapp>();
            colorlist = brandResponse.Data as List<ProductColorMapp>;

            return colorlist;
        }

        public List<OrderItemDTO> BindOrderItems(int orderId, string? SellerId = null, int? OrderItemId = 0, string? Status = null)
        {
            string url = "";
            if (!string.IsNullOrEmpty(SellerId))
            {
                url = url + "&SellerID=" + SellerId;
            }
            if (!string.IsNullOrEmpty(Status))
            {
                url = url + "&ItemStatus=" + Status;
            }
            if (OrderItemId != 0)
            {
                url = url + "&OrderItemId =" + OrderItemId;
            }
            BaseResponse<OrderItemDetails> baseResponse = new BaseResponse<OrderItemDetails>();
            var GetResponse = helper.ApiCall(OrderURL, EndPoints.OrderItems + "/GetOrderDetails" + "?OrderId=" + orderId + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);
            List<OrderItemDTO> ordersItem = new List<OrderItemDTO>();
            List<OrderItemDetails> ordersdetail = baseResponse.Data as List<OrderItemDetails>;
            if (ordersdetail.Count > 0)
            {
                foreach (var details in ordersdetail)
                {
                    OrderItemDTO orderItems = new OrderItemDTO();
                    orderItems.Id = Convert.ToInt32(details.OrderItemId);
                    orderItems.OrderID = Convert.ToInt32(details.OrderId);
                    orderItems.SubOrderNo = details.SubOrderNo;
                    orderItems.SellerID = details.SellerID;
                    orderItems.BrandID = Convert.ToInt32(details.BrandID);
                    orderItems.CategoryId = Convert.ToInt32(details.CategoryId);
                    orderItems.ProductID = Convert.ToInt32(details.ProductID);
                    orderItems.ProductGUID = details.ProductGUID;
                    orderItems.SellerProductID = Convert.ToInt32(details.SellerProductID);
                    orderItems.ProductName = details.ProductName;
                    orderItems.ProductSKUCode = details.ProductSKUCode;
                    orderItems.MRP = Convert.ToDecimal(details.MRP);
                    orderItems.SellingPrice = Convert.ToDecimal(details.SellingPrice);
                    orderItems.Discount = Convert.ToDecimal(details.Discount);
                    orderItems.Qty = Convert.ToInt32(details.Qty);
                    orderItems.TotalAmount = Convert.ToDecimal(details.TotalAmount);
                    orderItems.PriceTypeID = details.PriceTypeID;
                    orderItems.PriceType = details.PriceType;
                    orderItems.SizeID = details.SizeID;
                    orderItems.SizeValue = details.SizeValue;
                    orderItems.Coupon = details.ItemCoupon;
                    orderItems.CoupontDetails = details.ItemCoupontDetails;
                    orderItems.IsCouponApplied = details.ItemIsCouponApplied;
                    orderItems.CoupontDiscount = details.ItemCouponDiscount;
                    orderItems.ShippingZone = details.ShippingZone;
                    orderItems.ShippingCharge = details.ShippingCharge;
                    orderItems.ShippingChargePaidBy = details.ShippingChargePaidBy;
                    orderItems.SubTotal = Convert.ToDecimal(details.SubTotal);
                    orderItems.Status = details.ItemStatus;
                    orderItems.WherehouseId = details.WherehouseId;
                    orderItems.IsReplace = details.IsReplace;
                    orderItems.ParentID = details.ParentID;
                    orderItems.ReturnPolicyName = details.ReturnPolicyName;
                    orderItems.ReturnPolicyTitle = details.ReturnPolicyTitle;
                    orderItems.ReturnPolicyCovers = details.ReturnPolicyCovers;
                    orderItems.ReturnPolicyDescription = details.ReturnPolicyDescription;
                    orderItems.ReturnValidDays = details.ReturnValidDays;
                    orderItems.ReturnValidTillDate = details.ReturnValidTillDate;
                    //orderItems.BrandName = brand.Name;
                    //orderItems.ProductImage = string.IsNullOrEmpty(GetImages(details.ProductID).Image) ? null : GetImages(details.ProductID).Image.ToString();
                    //orderItems.Color = string.Join("-", GetColor(Convert.ToInt32(details.ProductID))?.Select(c => c.ColorName) ?? Enumerable.Empty<string>());
                    orderItems.BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? details.ExtraDetails : null;
                    orderItems.ProductImage = details.ProductImage;
                    orderItems.Color = details.ColorName;

                    if (string.IsNullOrEmpty(details.ExtraDetails))
                    {
                        SellerKycList SellerDetails = getsellerKyc(details.SellerID);
                        orderItems.SellerName = SellerDetails.TradeName;
                        orderItems.SellerPhoneNo = SellerDetails.ContactPersonMobileNo;
                        orderItems.SellerEmailId = SellerDetails.EmailID;
                        orderItems.SellerStatus = SellerDetails.SellerStatus;
                        orderItems.SellerKycStatus = SellerDetails.Status;
                        orderItems.ShippmentBy = SellerDetails.ShipmentBy;
                    }
                    else
                    {
                        orderItems.SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null;
                        orderItems.SellerPhoneNo = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["PhoneNumber"]?.ToString() : null;
                        orderItems.SellerEmailId = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Email"]?.ToString() : null;
                        orderItems.SellerStatus = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["SellerStatus"]?.ToString() : null;
                        orderItems.SellerKycStatus = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["KycStatus"]?.ToString() : null;
                        orderItems.ShippmentBy = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["ShipmentBy"]?.ToString() : null;
                    }
                    orderItems.OrderTaxInfos = BindOrderTax(details);
                    orderItems.OrderWiseExtraCharges = BindOrderWiseExtraCharges(details);

                    orderItems.OrderTaxInfos = BindOrderTax(details);
                    //OrderPackageDtoList pckg = GetPackage(details.OrderID, details.Id);
                    orderItems.PackageId = (details.PackageId != null || details.PackageId != 0) ? Convert.ToInt32(details.PackageId) : 0;
                    orderItems.PackageNo = details.PackageNo != null ? details.PackageNo : null;
                    orderItems.TotalPakedItems = (details.TotalItems != null || details.TotalItems != 0) ? Convert.ToInt32(details.TotalItems) : 0;
                    orderItems.NoOfPackage = (details.NoOfPackage != null || details.NoOfPackage != 0) ? Convert.ToInt32(details.NoOfPackage) : 0;
                    orderItems.PackageAmount = (details.PackageAmount != null || details.PackageAmount != 0) ? Convert.ToDecimal(details.PackageAmount) : 0;
                    orderItems.PackageCodCharges = (details.PackageCodCharges != null || details.PackageCodCharges != 0) ? Convert.ToDecimal(details.PackageCodCharges) : 0;
                    orderItems.PackageItemIds = details.OrderItemIDs != null ? details.OrderItemIDs : null;
                    ordersItem.Add(orderItems);
                }
            }
            return ordersItem;
        }

        public OrderItemDTO getOrderItemsDetails(int OrderItemId, string? SellerId = null, string? Status = null)
        {
            string url = "";
            if (!string.IsNullOrEmpty(SellerId))
            {
                url = url + "&SellerID=" + SellerId;
            }
            if (!string.IsNullOrEmpty(Status))
            {
                url = url + "&ItemStatus=" + Status;
            }
            BaseResponse<OrderItemDetails> baseResponse = new BaseResponse<OrderItemDetails>();
            var GetResponse = helper.ApiCall(OrderURL, EndPoints.OrderItems + "/GetOrderDetails" + "?OrderItemId =" + OrderItemId + url, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(GetResponse);
            OrderItemDTO orderItems = new OrderItemDTO();
            OrderItemDetails details = baseResponse.Data as OrderItemDetails;

            if (details != null && details.OrderItemId != 0 && details.OrderItemId != null)
            {

                orderItems.Id = Convert.ToInt32(details.OrderItemId);
                orderItems.OrderID = Convert.ToInt32(details.OrderId);
                orderItems.SubOrderNo = details.SubOrderNo;
                orderItems.SellerID = details.SellerID;
                orderItems.BrandID = Convert.ToInt32(details.BrandID);
                orderItems.CategoryId = Convert.ToInt32(details.CategoryId);
                orderItems.ProductID = Convert.ToInt32(details.ProductID);
                orderItems.ProductGUID = details.ProductGUID;
                orderItems.SellerProductID = Convert.ToInt32(details.SellerProductID);
                orderItems.ProductName = details.ProductName;
                orderItems.ProductSKUCode = details.ProductSKUCode;
                orderItems.MRP = Convert.ToDecimal(details.MRP);
                orderItems.SellingPrice = Convert.ToDecimal(details.SellingPrice);
                orderItems.Discount = Convert.ToDecimal(details.Discount);
                orderItems.Qty = Convert.ToInt32(details.Qty);
                orderItems.TotalAmount = Convert.ToDecimal(details.TotalAmount);
                orderItems.PriceTypeID = details.PriceTypeID;
                orderItems.PriceType = details.PriceType;
                orderItems.SizeID = details.SizeID;
                orderItems.SizeValue = details.SizeValue;
                orderItems.Coupon = details.ItemCoupon;
                orderItems.CoupontDetails = details.ItemCoupontDetails;
                orderItems.IsCouponApplied = details.ItemIsCouponApplied;
                orderItems.CoupontDiscount = details.ItemCouponDiscount;
                orderItems.ShippingZone = details.ShippingZone;
                orderItems.ShippingCharge = details.ShippingCharge;
                orderItems.ShippingChargePaidBy = details.ShippingChargePaidBy;
                orderItems.SubTotal = Convert.ToDecimal(details.SubTotal);
                orderItems.Status = details.ItemStatus;
                orderItems.WherehouseId = details.WherehouseId;
                orderItems.IsReplace = details.IsReplace;
                orderItems.ParentID = details.ParentID;
                orderItems.ReturnPolicyName = details.ReturnPolicyName;
                orderItems.ReturnPolicyTitle = details.ReturnPolicyTitle;
                orderItems.ReturnPolicyCovers = details.ReturnPolicyCovers;
                orderItems.ReturnPolicyDescription = details.ReturnPolicyDescription;
                orderItems.ReturnValidDays = details.ReturnValidDays;
                orderItems.ReturnValidTillDate = details.ReturnValidTillDate;
                //orderItems.BrandName = brand.Name;
                //orderItems.ProductImage = string.IsNullOrEmpty(GetImages(details.ProductID).Image) ? null : GetImages(details.ProductID).Image.ToString();
                //orderItems.Color = string.Join("-", GetColor(Convert.ToInt32(details.ProductID))?.Select(c => c.ColorName) ?? Enumerable.Empty<string>());
                orderItems.BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? details.ExtraDetails : null;
                orderItems.ProductImage = details.ProductImage;
                orderItems.Color = details.ColorName;

                if (string.IsNullOrEmpty(details.ExtraDetails))
                {
                    SellerKycList SellerDetails = getsellerKyc(details.SellerID);
                    orderItems.SellerName = SellerDetails.TradeName;
                    orderItems.SellerPhoneNo = SellerDetails.ContactPersonMobileNo;
                    orderItems.SellerEmailId = SellerDetails.EmailID;
                    orderItems.SellerStatus = SellerDetails.SellerStatus;
                    orderItems.SellerKycStatus = SellerDetails.Status;
                    orderItems.ShippmentBy = SellerDetails.ShipmentBy;
                }
                else
                {
                    orderItems.SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null;
                    orderItems.SellerPhoneNo = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["PhoneNumber"]?.ToString() : null;
                    orderItems.SellerEmailId = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Email"]?.ToString() : null;
                    orderItems.SellerStatus = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["SellerStatus"]?.ToString() : null;
                    orderItems.SellerKycStatus = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["KycStatus"]?.ToString() : null;
                    orderItems.ShippmentBy = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["ShipmentBy"]?.ToString() : null;
                }
                orderItems.OrderTaxInfos = BindOrderTax(details);
                orderItems.OrderWiseExtraCharges = BindOrderWiseExtraCharges(details);



                //OrderPackageDtoList pckg = GetPackage(details.OrderID, details.Id);
                orderItems.PackageId = (details.PackageId != null || details.PackageId != 0) ? Convert.ToInt32(details.PackageId) : 0;
                orderItems.PackageNo = details.PackageNo != null ? details.PackageNo : null;
                orderItems.TotalPakedItems = (details.TotalItems != null || details.TotalItems != 0) ? Convert.ToInt32(details.TotalItems) : 0;
                orderItems.NoOfPackage = (details.NoOfPackage != null || details.NoOfPackage != 0) ? Convert.ToInt32(details.NoOfPackage) : 0;
                orderItems.PackageAmount = (details.PackageAmount != null || details.PackageAmount != 0) ? Convert.ToDecimal(details.PackageAmount) : 0;
                orderItems.PackageCodCharges = (details.PackageCodCharges != null || details.PackageCodCharges != 0) ? Convert.ToDecimal(details.PackageCodCharges) : 0;
                orderItems.PackageItemIds = details.OrderItemIDs != null ? details.OrderItemIDs : null;


            }
            return orderItems;
        }

        public List<OrderTaxInfoDTO> BindOrderTax(OrderItemDetails orderItemDetails)
        {
            List<OrderTaxInfoDTO> ordersItem = new List<OrderTaxInfoDTO>();
            if (orderItemDetails.OrderTaxInfoId != 0 && orderItemDetails.OrderTaxInfoId != null)
            {
                OrderTaxInfoDTO orderItems = new OrderTaxInfoDTO();
                orderItems.Id = Convert.ToInt32(orderItemDetails.OrderTaxInfoId);
                orderItems.OrderID = Convert.ToInt32(orderItemDetails.OrderId);
                orderItems.OrderItemID = Convert.ToInt32(orderItemDetails.OrderItemId);
                orderItems.ProductID = Convert.ToInt32(orderItemDetails.ProductID);
                orderItems.SellerProductID = Convert.ToInt32(orderItemDetails.SellerProductID);
                orderItems.NetEarn = Convert.ToDecimal(orderItemDetails.NetEarn);
                orderItems.OrderTaxRateId = Convert.ToInt32(orderItemDetails.OrderTaxRateId);
                orderItems.OrderTaxRate = orderItemDetails.OrderTaxRate;
                orderItems.HSNCode = orderItemDetails.HSNCode;
                orderItems.IsSellerWithGSTAtOrderTime = Convert.ToBoolean(orderItemDetails.IsSellerWithGSTAtOrderTime);
                orderItems.WeightSlab = orderItemDetails.WeightSlab;

                ordersItem.Add(orderItems);
            }
            return ordersItem;
        }


        public List<OrderWiseExtraChargesDTO> BindOrderWiseExtraCharges(OrderItemDetails orderItemDetails)
        {
            List<OrderWiseExtraChargesDTO> ordersItem = new List<OrderWiseExtraChargesDTO>();
            if (!string.IsNullOrEmpty(orderItemDetails.OrderWiseExtraCharges))
            {
                var orderItems = JsonConvert.DeserializeObject<List<OrderWiseExtraChargesDTO>>(orderItemDetails.OrderWiseExtraCharges);
                foreach (var details in orderItems)
                {
                    OrderWiseExtraChargesDTO _orderItems = new OrderWiseExtraChargesDTO();
                    _orderItems.Id = details.Id;
                    _orderItems.OrderID = details.OrderID;
                    _orderItems.OrderItemID = details.OrderItemID;
                    _orderItems.ChargesType = details.ChargesType;
                    _orderItems.ChargesPaidBy = details.ChargesPaidBy;
                    _orderItems.ChargesIn = details.ChargesIn;
                    _orderItems.ChargesValueInPercentage = details.ChargesValueInPercentage;
                    _orderItems.ChargesValueInAmount = details.ChargesValueInAmount;
                    _orderItems.ChargesMaxAmount = details.ChargesMaxAmount;
                    _orderItems.TaxOnChargesAmount = details.TaxOnChargesAmount;
                    _orderItems.TotalCharges = details.TotalCharges;
                    _orderItems.ChargesAmountWithoutTax = details.ChargesAmountWithoutTax;

                    ordersItem.Add(_orderItems);
                }
            }
            return ordersItem;
        }
    }
}
