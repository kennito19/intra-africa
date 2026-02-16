using API_Gateway.Common;
using API_Gateway.Common.orders;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.Order;
using API_Gateway.Models.Entity.User;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        BaseResponse<DeliveryLibrary> baseReponse = new BaseResponse<DeliveryLibrary>();
        public static string UserApi = string.Empty;
        public static string IDServerUrl = string.Empty;
        public static string OrderUrl = string.Empty;
        public static string CatalogueUrl = string.Empty;
        private ApiHelper api;
        public ReportsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            UserApi = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            OrderUrl = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            api = new ApiHelper(_httpContext);
        }

        [HttpGet("SellerReport")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> DownloadSellerReport(string? fromdate = null, string? todate = null)
        {
            int rowCount = 0;
            List<int> totalCount = new List<int>();

            DataTable Seller_Data = new DataTable();
            Seller_Data.Columns.Add("FullName", typeof(string));
            Seller_Data.Columns.Add("EmailID", typeof(string));
            Seller_Data.Columns.Add("PhoneNumber", typeof(string));
            Seller_Data.Columns.Add("DisplayName", typeof(string));
            Seller_Data.Columns.Add("OwnerName", typeof(string));
            Seller_Data.Columns.Add("ContactPersonName", typeof(string));
            Seller_Data.Columns.Add("ContactPersonMobileNo", typeof(string));
            Seller_Data.Columns.Add("PanCardNo", typeof(string));
            Seller_Data.Columns.Add("NameOnPanCard", typeof(string));
            Seller_Data.Columns.Add("TypeOfCompany", typeof(string));
            Seller_Data.Columns.Add("CompanyRegistrationNo", typeof(string));
            Seller_Data.Columns.Add("BussinessType", typeof(string));
            Seller_Data.Columns.Add("MSMENo", typeof(string));
            Seller_Data.Columns.Add("ShipmentChargesPaidBy", typeof(string));
            Seller_Data.Columns.Add("TradeName", typeof(string));
            Seller_Data.Columns.Add("LegalName", typeof(string));
            Seller_Data.Columns.Add("GSTNo", typeof(string));
            Seller_Data.Columns.Add("GSTType", typeof(string));
            Seller_Data.Columns.Add("RegisteredAddressLine1", typeof(string));
            Seller_Data.Columns.Add("RegisteredAddressLine2", typeof(string));
            Seller_Data.Columns.Add("RegisteredLandmark", typeof(string));
            Seller_Data.Columns.Add("RegisteredPincode", typeof(string));
            Seller_Data.Columns.Add("City", typeof(string));
            Seller_Data.Columns.Add("State", typeof(string));
            Seller_Data.Columns.Add("Country", typeof(string));
            Seller_Data.Columns.Add("SellerStatus", typeof(string));
            Seller_Data.Columns.Add("KYCStatus", typeof(string));
            Seller_Data.Columns.Add("GSTStatus", typeof(string));
            Seller_Data.Columns.Add("SellerCreatedAt", typeof(string));
            Seller_Data.Columns.Add("SellerModifiedAt", typeof(string));
            Seller_Data.Columns.Add("KYCCreatedAt", typeof(string));
            Seller_Data.Columns.Add("KYCModifiedAt", typeof(string));

            rowCount = 0;

            string url = string.Empty;
            BaseResponse<SellerListModel> baseResponse2 = new BaseResponse<SellerListModel>();
            if (!string.IsNullOrEmpty(fromdate))
            {
                url += "&fromDate=" + fromdate;
            }
            if (!string.IsNullOrEmpty(todate))
            {
                url += "&todate=" + todate;
            }
            //var response2 = api.ApiCall(IDServerUrl, EndPoints.SellerList + "?pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
            //baseResponse2 = baseResponse2.JsonParseList(response2);

            //if (baseResponse2.code == 200)
            //{
            List<SellerListModel> sellerLists = (List<SellerListModel>)baseResponse2.Data;
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            GetOrders sellerr = new GetOrders(_configuration, _httpContext,null);
            //sellerLists = sellerLists.Where(x => x.Status.ToLower() != "archived").ToList();
            //List<UserDetailsDTO> lst = seller.bindSellerDetails(false);
            List<SellerKycList> getsellerKyclst = sellerr.getsellerKyclst();

            if (getsellerKyclst.Count > 0)
            {
                totalCount.Add(getsellerKyclst.Count());
                foreach (var item in getsellerKyclst)
                {
                    Seller_Data.Rows.Add();
                    Seller_Data.Rows[rowCount]["FullName"] = item.FullName;
                    Seller_Data.Rows[rowCount]["EmailID"] = item.EmailID;
                    Seller_Data.Rows[rowCount]["PhoneNumber"] = item.PhoneNumber;
                    Seller_Data.Rows[rowCount]["DisplayName"] = item.DisplayName;
                    Seller_Data.Rows[rowCount]["OwnerName"] = item.OwnerName;
                    Seller_Data.Rows[rowCount]["ContactPersonName"] = item.ContactPersonName;
                    Seller_Data.Rows[rowCount]["ContactPersonMobileNo"] = item.ContactPersonMobileNo;
                    Seller_Data.Rows[rowCount]["PanCardNo"] = item.PanCardNo;
                    Seller_Data.Rows[rowCount]["NameOnPanCard"] = item.NameOnPanCard;
                    Seller_Data.Rows[rowCount]["TypeOfCompany"] = item.TypeOfCompany;
                    Seller_Data.Rows[rowCount]["CompanyRegistrationNo"] = item.CompanyRegistrationNo;
                    Seller_Data.Rows[rowCount]["BussinessType"] = item.BussinessType;
                    Seller_Data.Rows[rowCount]["MSMENo"] = item.MSMENo;
                    Seller_Data.Rows[rowCount]["ShipmentChargesPaidBy"] = item.ShipmentChargesPaidBy;
                    Seller_Data.Rows[rowCount]["TradeName"] = item.TradeName;
                    Seller_Data.Rows[rowCount]["LegalName"] = item.LegalName;
                    Seller_Data.Rows[rowCount]["GSTNo"] = item.GSTNo;
                    Seller_Data.Rows[rowCount]["GSTType"] = item.GSTType;
                    Seller_Data.Rows[rowCount]["RegisteredAddressLine1"] = item.RegisteredAddressLine1;
                    Seller_Data.Rows[rowCount]["RegisteredAddressLine2"] = item.RegisteredAddressLine2;
                    Seller_Data.Rows[rowCount]["RegisteredLandmark"] = item.RegisteredLandmark;
                    Seller_Data.Rows[rowCount]["RegisteredPincode"] = item.RegisteredPincode;
                    Seller_Data.Rows[rowCount]["City"] = item.City;
                    Seller_Data.Rows[rowCount]["State"] = item.State;
                    Seller_Data.Rows[rowCount]["Country"] = item.Country;
                    Seller_Data.Rows[rowCount]["SellerStatus"] = item.SellerStatus;
                    Seller_Data.Rows[rowCount]["KYCStatus"] = item.Status;
                    Seller_Data.Rows[rowCount]["GSTStatus"] = item.GSTStatus;
                    Seller_Data.Rows[rowCount]["SellerCreatedAt"] = item.UserCreatedAt;
                    Seller_Data.Rows[rowCount]["SellerModifiedAt"] = item.UserModifiedAt;
                    Seller_Data.Rows[rowCount]["KYCCreatedAt"] = item.CreatedAt;
                    Seller_Data.Rows[rowCount]["KYCModifiedAt"] = item.ModifiedAt;
                    rowCount++;
                }
            }
            //}
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(Seller_Data, "Seller_Data");

                var DeliveryRange = ws.RangeUsed();

                ws.Tables.FirstOrDefault().ShowAutoFilter = false;


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SellerReport.xlsx");
                }
            }
        }


        [HttpGet("GSTReport")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> DownloadGSTReport(string? sellerId = null, string? fromdate = null, string? todate = null)
        {
            int rowCount = 0;
            List<int> totalCount = new List<int>();

            DataTable Seller_Data = new DataTable();
            Seller_Data.Columns.Add("FullName", typeof(string));
            Seller_Data.Columns.Add("EmailID", typeof(string));
            Seller_Data.Columns.Add("PhoneNumber", typeof(string));
            Seller_Data.Columns.Add("DisplayName", typeof(string));
            Seller_Data.Columns.Add("OwnerName", typeof(string));
            Seller_Data.Columns.Add("TradeName", typeof(string));
            Seller_Data.Columns.Add("LegalName", typeof(string));
            Seller_Data.Columns.Add("GSTNo", typeof(string));
            Seller_Data.Columns.Add("GSTType", typeof(string));
            Seller_Data.Columns.Add("RegisteredAddressLine1", typeof(string));
            Seller_Data.Columns.Add("RegisteredAddressLine2", typeof(string));
            Seller_Data.Columns.Add("RegisteredLandmark", typeof(string));
            Seller_Data.Columns.Add("RegisteredPincode", typeof(string));
            Seller_Data.Columns.Add("City", typeof(string));
            Seller_Data.Columns.Add("State", typeof(string));
            Seller_Data.Columns.Add("Country", typeof(string));
            Seller_Data.Columns.Add("GSTStatus", typeof(string));
            Seller_Data.Columns.Add("CreatedAt", typeof(string));
            Seller_Data.Columns.Add("ModifiedAt", typeof(string));

            rowCount = 0;

            string url = string.Empty;
            BaseResponse<SellerListModel> baseResponse2 = new BaseResponse<SellerListModel>();
            BaseResponse<GSTReport> baseResponse3 = new BaseResponse<GSTReport>();
            if (!string.IsNullOrEmpty(fromdate))
            {
                url += string.IsNullOrEmpty(url) ? "?fromDate=" + fromdate : "&fromDate=" + fromdate;
            }
            if (!string.IsNullOrEmpty(todate))
            {
                url += string.IsNullOrEmpty(url) ? "?todate=" + todate : "&todate=" + todate;
            }
            if (!string.IsNullOrEmpty(sellerId))
            {
                url += string.IsNullOrEmpty(url) ? "?sellerId=" + sellerId : "&sellerId=" + sellerId;
            }
            var response2 = api.ApiCall(IDServerUrl, EndPoints.SellerList + "?pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
            baseResponse2 = baseResponse2.JsonParseList(response2);

            var response = api.ApiCall(UserApi, EndPoints.Reports + "/getGSTReport" + url, "GET", null);
            baseResponse3 = baseResponse3.JsonParseList(response);

            if (baseResponse2.code == 200 && baseResponse3.code == 200)
            {
                List<SellerListModel> sellerLists = (List<SellerListModel>)baseResponse2.Data;
                List<GSTReport> lst = (List<GSTReport>)baseResponse3.Data;

                if (lst.Count > 0)
                {
                    totalCount.Add(lst.Count());
                    foreach (var item in lst)
                    {
                        var sellers = sellerLists.Where(p => p.Id == item.UserID).FirstOrDefault();

                        Seller_Data.Rows.Add();
                        Seller_Data.Rows[rowCount]["FullName"] = sellers.Id != null ? sellers.FirstName + " " + sellers.LastName : null;
                        Seller_Data.Rows[rowCount]["EmailID"] = sellers.Id != null ? sellers.UserName : null;
                        Seller_Data.Rows[rowCount]["PhoneNumber"] = sellers.Id != null ? sellers.MobileNo : null;
                        Seller_Data.Rows[rowCount]["DisplayName"] = item.DisplayName;
                        Seller_Data.Rows[rowCount]["OwnerName"] = item.OwnerName;
                        Seller_Data.Rows[rowCount]["TradeName"] = item.TradeName;
                        Seller_Data.Rows[rowCount]["LegalName"] = item.LegalName;
                        Seller_Data.Rows[rowCount]["GSTNo"] = item.GSTNo;
                        Seller_Data.Rows[rowCount]["GSTType"] = item.GSTType;
                        Seller_Data.Rows[rowCount]["RegisteredAddressLine1"] = item.RegisteredAddressLine1;
                        Seller_Data.Rows[rowCount]["RegisteredAddressLine2"] = item.RegisteredAddressLine2;
                        Seller_Data.Rows[rowCount]["RegisteredLandmark"] = item.RegisteredLandmark;
                        Seller_Data.Rows[rowCount]["RegisteredPincode"] = item.RegisteredPincode;
                        Seller_Data.Rows[rowCount]["City"] = item.City;
                        Seller_Data.Rows[rowCount]["State"] = item.State;
                        Seller_Data.Rows[rowCount]["Country"] = item.Country;
                        Seller_Data.Rows[rowCount]["GSTStatus"] = item.GSTStatus;
                        Seller_Data.Rows[rowCount]["CreatedAt"] = item.CreatedAt;
                        Seller_Data.Rows[rowCount]["ModifiedAt"] = item.ModifiedAt;
                        rowCount++;
                    }
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(Seller_Data, "Seller_Data");

                var DeliveryRange = ws.RangeUsed();

                ws.Tables.FirstOrDefault().ShowAutoFilter = false;


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WarehouseReport.xlsx");
                }
            }
        }

        [HttpGet("WarehouseReport")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> DownloadWarehouseReport(string? sellerId = null, string? fromdate = null, string? todate = null)
        {
            int rowCount = 0;
            List<int> totalCount = new List<int>();

            DataTable Seller_Data = new DataTable();
            Seller_Data.Columns.Add("FullName", typeof(string));
            Seller_Data.Columns.Add("EmailID", typeof(string));
            Seller_Data.Columns.Add("PhoneNumber", typeof(string));
            Seller_Data.Columns.Add("DisplayName", typeof(string));
            Seller_Data.Columns.Add("OwnerName", typeof(string));
            Seller_Data.Columns.Add("TradeName", typeof(string));
            Seller_Data.Columns.Add("LegalName", typeof(string));
            Seller_Data.Columns.Add("GSTNo", typeof(string));
            Seller_Data.Columns.Add("GSTType", typeof(string));
            Seller_Data.Columns.Add("AddressLine1", typeof(string));
            Seller_Data.Columns.Add("AddressLine2", typeof(string));
            Seller_Data.Columns.Add("Landmark", typeof(string));
            Seller_Data.Columns.Add("Pincode", typeof(string));
            Seller_Data.Columns.Add("City", typeof(string));
            Seller_Data.Columns.Add("State", typeof(string));
            Seller_Data.Columns.Add("Country", typeof(string));
            Seller_Data.Columns.Add("RegisteredAddressLine1", typeof(string));
            Seller_Data.Columns.Add("RegisteredAddressLine2", typeof(string));
            Seller_Data.Columns.Add("RegisteredLandmark", typeof(string));
            Seller_Data.Columns.Add("RegisteredPincode", typeof(string));
            Seller_Data.Columns.Add("RegisteredCity", typeof(string));
            Seller_Data.Columns.Add("RegisteredState", typeof(string));
            Seller_Data.Columns.Add("RegisteredCountry", typeof(string));
            Seller_Data.Columns.Add("Status", typeof(string));
            Seller_Data.Columns.Add("CreatedAt", typeof(string));
            Seller_Data.Columns.Add("ModifiedAt", typeof(string));

            rowCount = 0;

            string url = string.Empty;
            BaseResponse<SellerListModel> baseResponse2 = new BaseResponse<SellerListModel>();
            BaseResponse<WarehouseReport> baseResponse3 = new BaseResponse<WarehouseReport>();
            if (!string.IsNullOrEmpty(fromdate))
            {
                url += string.IsNullOrEmpty(url) ? "?fromDate=" + fromdate : "&fromDate=" + fromdate;
            }
            if (!string.IsNullOrEmpty(todate))
            {
                url += string.IsNullOrEmpty(url) ? "?todate=" + todate : "&todate=" + todate;
            }
            if (!string.IsNullOrEmpty(sellerId))
            {
                url += string.IsNullOrEmpty(url) ? "?sellerId=" + sellerId : "&sellerId=" + sellerId;
            }
            var response2 = api.ApiCall(IDServerUrl, EndPoints.SellerList + "?pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
            baseResponse2 = baseResponse2.JsonParseList(response2);

            var response = api.ApiCall(UserApi, EndPoints.Reports + "/getWarehouseReport" + url, "GET", null);
            baseResponse3 = baseResponse3.JsonParseList(response);

            if (baseResponse2.code == 200 && baseResponse3.code == 200)
            {
                List<SellerListModel> sellerLists = (List<SellerListModel>)baseResponse2.Data;
                List<WarehouseReport> lst = (List<WarehouseReport>)baseResponse3.Data;

                if (lst.Count > 0)
                {
                    totalCount.Add(lst.Count());
                    foreach (var item in lst)
                    {
                        var sellers = sellerLists.Where(p => p.Id == item.UserID).FirstOrDefault();

                        Seller_Data.Rows.Add();
                        Seller_Data.Rows[rowCount]["FullName"] = sellers.Id != null ? sellers.FirstName + " " + sellers.LastName : null;
                        Seller_Data.Rows[rowCount]["EmailID"] = sellers.Id != null ? sellers.UserName : null;
                        Seller_Data.Rows[rowCount]["PhoneNumber"] = sellers.Id != null ? sellers.MobileNo : null;
                        Seller_Data.Rows[rowCount]["DisplayName"] = item.DisplayName;
                        Seller_Data.Rows[rowCount]["OwnerName"] = item.OwnerName;
                        Seller_Data.Rows[rowCount]["TradeName"] = item.TradeName;
                        Seller_Data.Rows[rowCount]["LegalName"] = item.LegalName;
                        Seller_Data.Rows[rowCount]["GSTNo"] = item.GSTNo;
                        Seller_Data.Rows[rowCount]["GSTType"] = item.GSTType;
                        Seller_Data.Rows[rowCount]["AddressLine1"] = item.AddressLine1;
                        Seller_Data.Rows[rowCount]["AddressLine2"] = item.AddressLine2;
                        Seller_Data.Rows[rowCount]["Landmark"] = item.Landmark;
                        Seller_Data.Rows[rowCount]["Pincode"] = item.Pincode;
                        Seller_Data.Rows[rowCount]["City"] = item.City;
                        Seller_Data.Rows[rowCount]["State"] = item.State;
                        Seller_Data.Rows[rowCount]["Country"] = item.Country;
                        Seller_Data.Rows[rowCount]["RegisteredAddressLine1"] = item.RegisteredAddressLine1;
                        Seller_Data.Rows[rowCount]["RegisteredAddressLine2"] = item.RegisteredAddressLine2;
                        Seller_Data.Rows[rowCount]["RegisteredLandmark"] = item.RegisteredLandmark;
                        Seller_Data.Rows[rowCount]["RegisteredPincode"] = item.RegisteredPincode;
                        Seller_Data.Rows[rowCount]["RegisteredCity"] = item.RegisteredCity;
                        Seller_Data.Rows[rowCount]["RegisteredState"] = item.RegisteredState;
                        Seller_Data.Rows[rowCount]["RegisteredCountry"] = item.RegisteredCountry;
                        Seller_Data.Rows[rowCount]["Status"] = item.Status;
                        Seller_Data.Rows[rowCount]["CreatedAt"] = item.CreatedAt;
                        Seller_Data.Rows[rowCount]["ModifiedAt"] = item.ModifiedAt;
                        rowCount++;
                    }
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(Seller_Data, "Seller_Data");

                var DeliveryRange = ws.RangeUsed();

                ws.Tables.FirstOrDefault().ShowAutoFilter = false;


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WarehouseReport.xlsx");
                }
            }
        }


        [HttpGet("OrderReport")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> DownloadOrderReport(string? sellerId = null, string? fromdate = null, string? todate = null)
        {
            int rowCount = 0;
            List<int> totalCount = new List<int>();
            BaseResponse<OrderWiseExtraCharges> ExbaseResponse = new BaseResponse<OrderWiseExtraCharges>();
            var Exresponse = api.ApiCall(OrderUrl, EndPoints.OrderWiseExtraCharges + "?PageIndex=" + 0 + "&PageSize=" + 0, "GET", null);
            ExbaseResponse = ExbaseResponse.JsonParseList(Exresponse);

            DataTable Order_Data = new DataTable();
            Order_Data.Columns.Add("OrderNo", typeof(string));
            Order_Data.Columns.Add("OrderBy", typeof(string));
            Order_Data.Columns.Add("OrderDate", typeof(string));
            Order_Data.Columns.Add("SellerName", typeof(string));
            Order_Data.Columns.Add("TradeName", typeof(string));
            Order_Data.Columns.Add("LegalName", typeof(string));
            Order_Data.Columns.Add("BrandName", typeof(string));
            Order_Data.Columns.Add("ProductName", typeof(string));
            Order_Data.Columns.Add("ProductSKUCode", typeof(string));
            Order_Data.Columns.Add("MRP", typeof(string));
            Order_Data.Columns.Add("SellingPrice", typeof(string));
            Order_Data.Columns.Add("Discount", typeof(string));
            Order_Data.Columns.Add("Quantity", typeof(string));
            Order_Data.Columns.Add("IsCouponApplied", typeof(string));
            Order_Data.Columns.Add("Coupon", typeof(string));
            Order_Data.Columns.Add("CoupontDiscount", typeof(string));
            Order_Data.Columns.Add("ShippingChargePaidBy", typeof(string));
            Order_Data.Columns.Add("ShippingZone", typeof(string));
            Order_Data.Columns.Add("UserName", typeof(string));
            Order_Data.Columns.Add("UserPhoneNo", typeof(string));
            Order_Data.Columns.Add("UserEmail", typeof(string));
            Order_Data.Columns.Add("UserAddressLine1", typeof(string));
            Order_Data.Columns.Add("UserAddressLine2", typeof(string));
            Order_Data.Columns.Add("UserLandmark", typeof(string));
            Order_Data.Columns.Add("UserCity", typeof(string));
            Order_Data.Columns.Add("UserState", typeof(string));
            Order_Data.Columns.Add("UserPincode", typeof(string));
            Order_Data.Columns.Add("ExtraCharges", typeof(string));

            //if (ExbaseResponse.code == 200)
            //{
            //    List<OrderWiseExtraCharges> extraChargesLibraries = new List<OrderWiseExtraCharges>();
            //    extraChargesLibraries = (List<OrderWiseExtraCharges>)ExbaseResponse.Data;
            //    var UniqueName = extraChargesLibraries.Where(e => !string.IsNullOrEmpty(e.ChargesType)).DistinctBy(e => e.ChargesType).ToList();
            //    if (UniqueName.Count > 0)
            //    {
            //        foreach (var item in UniqueName)
            //        {
            //            Order_Data.Columns.Add(item.ChargesType, typeof(string));
            //            Order_Data.Columns.Add(item.ChargesType+"_"+item.ChargesPaidBy, typeof(string));
            //            Order_Data.Columns.Add(item.ChargesType+"_"+item.ChargesIn, typeof(string));
            //            Order_Data.Columns.Add(item.ChargesType+"_"+item.TotalCharges, typeof(string));
            //        }
            //    }
            //}


            Order_Data.Columns.Add("ItemTotalAmount", typeof(string));
            Order_Data.Columns.Add("ItemSubTotal", typeof(string));
            Order_Data.Columns.Add("PaidAmount", typeof(string));
            Order_Data.Columns.Add("TotalAmount", typeof(string));
            Order_Data.Columns.Add("PaymentMode", typeof(string));
            Order_Data.Columns.Add("CODCharge", typeof(string));
            Order_Data.Columns.Add("OrderStatus", typeof(string));
            Order_Data.Columns.Add("Status", typeof(string));
            Order_Data.Columns.Add("CreatedAt", typeof(string));
            Order_Data.Columns.Add("ModifiedAt", typeof(string));

            rowCount = 0;

            string url = string.Empty;
            BaseResponse<OrderReports> baseResponse3 = new BaseResponse<OrderReports>();
            if (!string.IsNullOrEmpty(fromdate))
            {
                url += string.IsNullOrEmpty(url) ? "?fromDate=" + fromdate : "&fromDate=" + fromdate;
            }
            if (!string.IsNullOrEmpty(todate))
            {
                url += string.IsNullOrEmpty(url) ? "?todate=" + todate : "&todate=" + todate;
            }
            if (!string.IsNullOrEmpty(sellerId))
            {
                url += string.IsNullOrEmpty(url) ? "?sellerId=" + sellerId : "&sellerId=" + sellerId;
            }
            var response = api.ApiCall(OrderUrl, EndPoints.Reports + "/getOrderReport" + url, "GET", null);
            baseResponse3 = baseResponse3.JsonParseList(response);

            if (baseResponse3.code == 200)
            {
                List<OrderReports> lst = (List<OrderReports>)baseResponse3.Data;
                GetOrders seller = new GetOrders(_configuration, _httpContext, null);
                List<SellerKycList> SellerDetailslst = seller.getsellerKyclst();
                List<BrandLibrary> brandlst = getBrand();
                if (lst.Count > 0)
                {
                    totalCount.Add(lst.Count());
                    foreach (var item in lst)
                    {
                        SellerKycList SellerDetails = new SellerKycList();
                        BrandLibrary brand = new BrandLibrary();
                        if (SellerDetailslst.Count > 0)
                        {
                            SellerDetails = SellerDetailslst.Where(s => s.UserID == item.SellerId).FirstOrDefault();
                        }
                        if (brandlst.Count > 0)
                        {
                            brand = brandlst.Where(s => s.ID == item.BrandId).FirstOrDefault();
                        }


                        Order_Data.Rows.Add();
                        Order_Data.Rows[rowCount]["OrderNo"] = item.OrderNo != null ? item.OrderNo : null;
                        Order_Data.Rows[rowCount]["OrderBy"] = item.OrderBy != null ? item.OrderBy : null;
                        Order_Data.Rows[rowCount]["OrderDate"] = item.OrderDate != null ? item.OrderDate : null;
                        Order_Data.Rows[rowCount]["SellerName"] = SellerDetails.DisplayName != null ? SellerDetails.DisplayName : null;
                        Order_Data.Rows[rowCount]["TradeName"] = SellerDetails.TradeName != null ? SellerDetails.TradeName : null;
                        Order_Data.Rows[rowCount]["LegalName"] = SellerDetails.LegalName;
                        Order_Data.Rows[rowCount]["BrandName"] = brand.Name;
                        Order_Data.Rows[rowCount]["ProductName"] = item.ProductName;
                        Order_Data.Rows[rowCount]["ProductSKUCode"] = item.ProductSKUCode;
                        Order_Data.Rows[rowCount]["MRP"] = item.MRP;
                        Order_Data.Rows[rowCount]["SellingPrice"] = item.SellingPrice;
                        Order_Data.Rows[rowCount]["Discount"] = item.Discount;
                        Order_Data.Rows[rowCount]["Quantity"] = item.Qty;
                        Order_Data.Rows[rowCount]["IsCouponApplied"] = item.IsCouponApplied;
                        Order_Data.Rows[rowCount]["Coupon"] = item.Coupon;
                        Order_Data.Rows[rowCount]["CoupontDiscount"] = item.CoupontDiscount;
                        Order_Data.Rows[rowCount]["ShippingChargePaidBy"] = item.ShippingChargePaidBy;
                        Order_Data.Rows[rowCount]["ShippingZone"] = item.ShippingZone;
                        Order_Data.Rows[rowCount]["UserName"] = item.UserName;
                        Order_Data.Rows[rowCount]["UserPhoneNo"] = item.UserPhoneNo;
                        Order_Data.Rows[rowCount]["UserEmail"] = item.UserEmail;
                        Order_Data.Rows[rowCount]["UserAddressLine1"] = item.UserAddressLine1;
                        Order_Data.Rows[rowCount]["UserAddressLine2"] = item.UserAddressLine2;
                        Order_Data.Rows[rowCount]["UserLandmark"] = item.UserLandmark;
                        Order_Data.Rows[rowCount]["UserCity"] = item.UserCity;
                        Order_Data.Rows[rowCount]["UserState"] = item.UserState;
                        Order_Data.Rows[rowCount]["UserPincode"] = item.UserPincode;
                        
                        if (ExbaseResponse.code == 200)
                        {
                            List<OrderWiseExtraCharges> extraChargesLibraries = new List<OrderWiseExtraCharges>();
                            extraChargesLibraries = (List<OrderWiseExtraCharges>)ExbaseResponse.Data;
                            var UniqueName = extraChargesLibraries.Where(e => e.OrderItemID == item.OrderItemId && e.OrderID == item.OrderId && !string.IsNullOrEmpty(e.ChargesType)).ToList();
                            if (UniqueName.Count > 0)
                            {
                                string Values = string.Empty;
                                foreach (var item1 in UniqueName)
                                {
                                    Values += "(" + item1.ChargesType + " : " + item1.TotalCharges + "," + item1.ChargesType + "_ChargesPaidBy" + " : " + item1.ChargesPaidBy + "),";
                                    //Order_Data.Rows[rowCount][item1.ChargesType] = item1.ChargesType;
                                    //Order_Data.Rows[rowCount][item1.ChargesType + "_" + item1.ChargesPaidBy] = item1.ChargesPaidBy;
                                    //Order_Data.Rows[rowCount][item1.ChargesType + "_" + item1.ChargesIn] = item1.ChargesIn;
                                    //Order_Data.Rows[rowCount][item1.ChargesType + "_" + item1.TotalCharges] = item1.TotalCharges;
                                }
                                Values = Values.Trim(',');
                                Order_Data.Rows[rowCount]["ExtraCharges"] = Values;
                            }
                        }


                        Order_Data.Rows[rowCount]["ItemTotalAmount"] = item.ItemTotalAmount;
                        Order_Data.Rows[rowCount]["ItemSubTotal"] = item.ItemSubTotal;
                        Order_Data.Rows[rowCount]["PaidAmount"] = item.PaidAmount;
                        Order_Data.Rows[rowCount]["TotalAmount"] = item.TotalAmount;
                        Order_Data.Rows[rowCount]["PaymentMode"] = item.PaymentMode;
                        Order_Data.Rows[rowCount]["CODCharge"] = item.CODCharge;
                        Order_Data.Rows[rowCount]["OrderStatus"] = item.orderStatus;
                        Order_Data.Rows[rowCount]["Status"] = item.Status;
                        Order_Data.Rows[rowCount]["CreatedAt"] = item.CreatedAt;
                        Order_Data.Rows[rowCount]["ModifiedAt"] = item.ModifiedAt;
                        rowCount++;
                    }
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(Order_Data, "Order_Data");

                var DeliveryRange = ws.RangeUsed();

                ws.Tables.FirstOrDefault().ShowAutoFilter = false;


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrderReport.xlsx");
                }
            }
        }


        [HttpGet("ProductReport")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> DownloadProductReport(string? sellerId = null, string? fromdate = null, string? todate = null)
        {
            int rowCount = 0;
            List<int> totalCount = new List<int>();

            DataTable Product_Data = new DataTable();

            Product_Data.Columns.Add("ProductName", typeof(string));
            Product_Data.Columns.Add("CompanySKUCode", typeof(string));
            Product_Data.Columns.Add("SellerDisplayName", typeof(string));
            Product_Data.Columns.Add("SellerLegalName", typeof(string));
            Product_Data.Columns.Add("SellerTradeName", typeof(string));
            Product_Data.Columns.Add("BrandName", typeof(string));
            Product_Data.Columns.Add("SellerSKU", typeof(string));
            Product_Data.Columns.Add("CustomeProductName", typeof(string));
            Product_Data.Columns.Add("PathNames", typeof(string));
            Product_Data.Columns.Add("TaxName", typeof(string));
            Product_Data.Columns.Add("HSNCode", typeof(string));
            Product_Data.Columns.Add("MRP", typeof(string));
            Product_Data.Columns.Add("SellingPrice", typeof(string));
            Product_Data.Columns.Add("Discount", typeof(string));
            Product_Data.Columns.Add("Color", typeof(string));
            Product_Data.Columns.Add("Quantity", typeof(string));
            Product_Data.Columns.Add("SizeType", typeof(string));
            Product_Data.Columns.Add("Size", typeof(string));
            Product_Data.Columns.Add("WarehouseName", typeof(string));
            Product_Data.Columns.Add("WarehouseQty", typeof(int));
            Product_Data.Columns.Add("PackingLength", typeof(string));
            Product_Data.Columns.Add("PackingBreadth", typeof(string));
            Product_Data.Columns.Add("PackingHeight", typeof(string));
            Product_Data.Columns.Add("PackingWeight", typeof(string));
            Product_Data.Columns.Add("SellerEmail", typeof(string));
            Product_Data.Columns.Add("SellerPhoneNo", typeof(string));
            Product_Data.Columns.Add("ShipmentChargesPaidBy", typeof(string));
            Product_Data.Columns.Add("SellerStatus", typeof(string));
            Product_Data.Columns.Add("ProductStatus", typeof(string));
            Product_Data.Columns.Add("ProductLive", typeof(string));
            rowCount = 0;

            string url = string.Empty;
            BaseResponse<ProductReports> baseResponseProduct = new BaseResponse<ProductReports>();

            if (!string.IsNullOrEmpty(fromdate))
            {
                url += string.IsNullOrEmpty(url) ? "?fromDate=" + fromdate : "&fromDate=" + fromdate;
            }
            if (!string.IsNullOrEmpty(todate))
            {
                url += string.IsNullOrEmpty(url) ? "?todate=" + todate : "&todate=" + todate;
            }
            if (!string.IsNullOrEmpty(sellerId))
            {
                url += string.IsNullOrEmpty(url) ? "?sellerId=" + sellerId : "&sellerId=" + sellerId;
            }

            var response = api.ApiCall(CatalogueUrl, EndPoints.Reports + "/getProductReport" + url, "GET", null);
            baseResponseProduct = baseResponseProduct.JsonParseList(response);

            if (baseResponseProduct.code == 200)
            {
                List<ProductReports> ProductLists = (List<ProductReports>)baseResponseProduct.Data;

                if (ProductLists.Count > 0)
                {
                    totalCount.Add(ProductLists.Count());
                    foreach (var item in ProductLists)
                    {

                        Product_Data.Rows.Add();

                        Product_Data.Rows[rowCount]["ProductName"] = item.ProductName;
                        Product_Data.Rows[rowCount]["CompanySKUCode"] = item.CompanySKUCode;
                        Product_Data.Rows[rowCount]["SellerDisplayName"] = item.SellerDisplayName;
                        Product_Data.Rows[rowCount]["SellerLegalName"] = item.SellerLegalName;
                        Product_Data.Rows[rowCount]["SellerTradeName"] = item.SellerTradeName;
                        Product_Data.Rows[rowCount]["BrandName"] = item.BrandName;
                        Product_Data.Rows[rowCount]["SellerSKU"] = item.SellerSKU;
                        Product_Data.Rows[rowCount]["CustomeProductName"] = item.CustomeProductName;
                        Product_Data.Rows[rowCount]["PathNames"] = item.PathNames;
                        Product_Data.Rows[rowCount]["TaxName"] = item.TaxName;
                        Product_Data.Rows[rowCount]["HSNCode"] = item.HSNCode;
                        Product_Data.Rows[rowCount]["MRP"] = item.MRP;
                        Product_Data.Rows[rowCount]["SellingPrice"] = item.SellingPrice;
                        Product_Data.Rows[rowCount]["Discount"] = item.Discount;
                        Product_Data.Rows[rowCount]["Color"] = item.Color;
                        Product_Data.Rows[rowCount]["Quantity"] = item.Quantity;
                        Product_Data.Rows[rowCount]["SizeType"] = item.SizeType;
                        Product_Data.Rows[rowCount]["Size"] = item.Size;
                        Product_Data.Rows[rowCount]["WarehouseName"] = item.WarehouseName;
                        Product_Data.Rows[rowCount]["WarehouseQty"] = item.WarehouseQty;
                        Product_Data.Rows[rowCount]["PackingLength"] = item.PackingLength;
                        Product_Data.Rows[rowCount]["PackingBreadth"] = item.PackingBreadth;
                        Product_Data.Rows[rowCount]["PackingHeight"] = item.PackingHeight;
                        Product_Data.Rows[rowCount]["PackingWeight"] = item.PackingWeight;
                        Product_Data.Rows[rowCount]["SellerEmail"] = item.SellerEmail;
                        Product_Data.Rows[rowCount]["SellerPhoneNo"] = item.SellerPhoneNo;
                        Product_Data.Rows[rowCount]["ShipmentChargesPaidBy"] = item.ShipmentChargesPaidBy;
                        Product_Data.Rows[rowCount]["SellerStatus"] = item.SellerStatus;
                        Product_Data.Rows[rowCount]["ProductStatus"] = item.Status;
                        Product_Data.Rows[rowCount]["ProductLive"] = item.Live;


                        rowCount++;
                    }
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(Product_Data, "Product_Data");

                var DeliveryRange = ws.RangeUsed();

                ws.Tables.FirstOrDefault().ShowAutoFilter = false;


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductReport.xlsx");
                }
            }
        }


        [HttpGet("CommissionReport")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> DownloadCommissionReport(string? sellerId = null, string? fromdate = null, string? todate = null)
        {
            int rowCount = 0;
            List<int> totalCount = new List<int>();

            DataTable Order_Data = new DataTable();
            Order_Data.Columns.Add("OrderNo", typeof(string));
            Order_Data.Columns.Add("OrderBy", typeof(string));
            Order_Data.Columns.Add("OrderDate", typeof(string));
            Order_Data.Columns.Add("SellerName", typeof(string));
            Order_Data.Columns.Add("TradeName", typeof(string));
            Order_Data.Columns.Add("LegalName", typeof(string));
            Order_Data.Columns.Add("BrandName", typeof(string));
            Order_Data.Columns.Add("ProductName", typeof(string));
            Order_Data.Columns.Add("ProductSKUCode", typeof(string));
            Order_Data.Columns.Add("MRP", typeof(string));
            Order_Data.Columns.Add("SellingPrice", typeof(string));
            Order_Data.Columns.Add("Discount", typeof(string));
            Order_Data.Columns.Add("Quantity", typeof(string));
            Order_Data.Columns.Add("ItemSubTotal", typeof(string));
            Order_Data.Columns.Add("ItemTotalAmount", typeof(string));
            Order_Data.Columns.Add("CommissionIn", typeof(string));
            Order_Data.Columns.Add("CommissionRate", typeof(string));
            Order_Data.Columns.Add("CommissionAmount", typeof(string));
            Order_Data.Columns.Add("OrderStatus", typeof(string));
            Order_Data.Columns.Add("Status", typeof(string));
            Order_Data.Columns.Add("CreatedAt", typeof(string));
            Order_Data.Columns.Add("ModifiedAt", typeof(string));

            rowCount = 0;

            string url = string.Empty;
            BaseResponse<CommissionReports> baseResponse3 = new BaseResponse<CommissionReports>();
            if (!string.IsNullOrEmpty(fromdate))
            {
                url += string.IsNullOrEmpty(url) ? "?fromDate=" + fromdate : "&fromDate=" + fromdate;
            }
            if (!string.IsNullOrEmpty(todate))
            {
                url += string.IsNullOrEmpty(url) ? "?todate=" + todate : "&todate=" + todate;
            }
            if (!string.IsNullOrEmpty(sellerId))
            {
                url += string.IsNullOrEmpty(url) ? "?sellerId=" + sellerId : "&sellerId=" + sellerId;
            }
            var response = api.ApiCall(OrderUrl, EndPoints.Reports + "/getCommissionReport" + url, "GET", null);
            baseResponse3 = baseResponse3.JsonParseList(response);

            if (baseResponse3.code == 200)
            {
                List<CommissionReports> lst = (List<CommissionReports>)baseResponse3.Data;
                GetOrders sellerr = new GetOrders(_configuration, _httpContext, null);
                List<SellerKycList> SellerDetailslst = sellerr.getsellerKyclst();
                List<BrandLibrary> brandlst = getBrand();
                if (lst.Count > 0)
                {
                    totalCount.Add(lst.Count());
                    foreach (var item in lst)
                    {
                        SellerKycList SellerDetails = new SellerKycList();
                        BrandLibrary brand = new BrandLibrary();
                        if (SellerDetailslst.Count > 0)
                        {
                             SellerDetails = SellerDetailslst.Where(s => s.UserID == item.SellerId).FirstOrDefault();
                        }
                        if (brandlst.Count > 0)
                        {
                            brand = brandlst.Where(s => s.ID == item.BrandId).FirstOrDefault();
                        }


                        Order_Data.Rows.Add();
                        Order_Data.Rows[rowCount]["OrderNo"] = item.OrderNo != null ? item.OrderNo : null;
                        Order_Data.Rows[rowCount]["OrderBy"] = item.OrderBy != null ? item.OrderBy : null;
                        Order_Data.Rows[rowCount]["OrderDate"] = item.OrderDate != null ? item.OrderDate : null;
                        Order_Data.Rows[rowCount]["SellerName"] = SellerDetails.DisplayName != null ? SellerDetails.DisplayName : null;
                        Order_Data.Rows[rowCount]["TradeName"] = SellerDetails.TradeName != null ? SellerDetails.TradeName : null;
                        Order_Data.Rows[rowCount]["LegalName"] = SellerDetails.LegalName;
                        Order_Data.Rows[rowCount]["BrandName"] = brand.Name;
                        Order_Data.Rows[rowCount]["ProductName"] = item.ProductName;
                        Order_Data.Rows[rowCount]["ProductSKUCode"] = item.ProductSKUCode;
                        Order_Data.Rows[rowCount]["MRP"] = item.MRP;
                        Order_Data.Rows[rowCount]["SellingPrice"] = item.SellingPrice;
                        Order_Data.Rows[rowCount]["Discount"] = item.Discount;
                        Order_Data.Rows[rowCount]["Quantity"] = item.Qty;
                        Order_Data.Rows[rowCount]["ItemSubTotal"] = item.ItemSubTotal;
                        Order_Data.Rows[rowCount]["ItemTotalAmount"] = item.ItemTotalAmount;
                        Order_Data.Rows[rowCount]["CommissionIn"] = item.CommissionIn;
                        Order_Data.Rows[rowCount]["CommissionRate"] = item.CommissionRate;
                        Order_Data.Rows[rowCount]["CommissionAmount"] = item.CommissionAmount;
                        Order_Data.Rows[rowCount]["OrderStatus"] = item.orderStatus;
                        Order_Data.Rows[rowCount]["Status"] = item.Status;
                        Order_Data.Rows[rowCount]["CreatedAt"] = item.CreatedAt;
                        Order_Data.Rows[rowCount]["ModifiedAt"] = item.ModifiedAt;
                        rowCount++;
                    }
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(Order_Data, "Order_Data");

                var DeliveryRange = ws.RangeUsed();

                ws.Tables.FirstOrDefault().ShowAutoFilter = false;


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CommissionReport.xlsx");
                }
            }
        }


        //[NonAction]
        //public List<SellerKycList> getsellerKyc()
        //{
        //    BaseResponse<SellerListModel> baseResponse = new BaseResponse<SellerListModel>();
        //    List<SellerKycList> sellerKycList = new List<SellerKycList>();
        //    var response2 = api.ApiCall(IDServerUrl, EndPoints.SellerList + "?pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
        //    baseResponse = baseResponse.JsonParseList(response2);

        //    if (baseResponse.code == 200)
        //    {
        //        List<SellerListModel> sellerLists = (List<SellerListModel>)baseResponse.Data;
        //        sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
        //        List<SellerKycList> lst = seller.bindSellerDetails(sellerLists);
        //        sellerKycList = lst.ToList();
        //    }

        //    return sellerKycList;
        //}
        [NonAction]
        public List<BrandLibrary> getBrand()
        {
            BaseResponse<BrandLibrary> brandResponse = new BaseResponse<BrandLibrary>();
            var response = api.ApiCall(UserApi, EndPoints.Brand + "?PageIndex=" + 0 + "&PageSize=" + 0, "GET", null);
            brandResponse = brandResponse.JsonParseList(response);

            List<BrandLibrary> brand = new List<BrandLibrary>();
            brand = (List<BrandLibrary>)brandResponse.Data;

            return brand;
        }

    }


}
