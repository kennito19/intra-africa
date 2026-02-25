using API_Gateway.Common.products;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.IDServer;
using API_Gateway.Models.Entity.Order;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string IDServerUrl = string.Empty;
        public string UserUrl = string.Empty;
        public string OrderUrl = string.Empty;
        private ApiHelper helper;


        public DashboardController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            UserUrl = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            OrderUrl = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpGet("getSellerCounts")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult GetSellerCounts(string? days = "All")
        {
            BaseResponse<SellerListModel> baseResponse = new BaseResponse<SellerListModel>();
            var response = helper.ApiCall(IDServerUrl, EndPoints.SellerList + "?pageIndex=0&pageSize=0", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<SellerListModel> lstSeller = baseResponse.Data as List<SellerListModel> ?? new List<SellerListModel>();
            lstSeller = lstSeller.Where(seller => seller.Status.ToLower() != "archived").ToList();

            //BaseResponse<KYCDetails> kycbaseResponse = new BaseResponse<KYCDetails>();
            //var Kycresponse = helper.ApiCall(UserUrl, EndPoints.KYCDetails + "?pageIndex=0&pageSize=0", "GET", null);
            //kycbaseResponse = kycbaseResponse.JsonParseList(Kycresponse);
            //List<KYCDetails> lstKyc = kycbaseResponse.Data as List<KYCDetails> ?? new List<KYCDetails>();

            int totalSellerCount = 0;
            int newSellerCount = 0;
            int ActiveSellerCount = 0;
            int inactiveSellerCount = 0;



            //New Seller
            if (lstSeller.Count > 0)
            {

                if (!string.IsNullOrEmpty(days))
                {
                    DateTime currentDate = new DateTime();
                    if (days.ToLower() == "all")
                    {
                        lstSeller = lstSeller;
                    }
                    else if (days.ToLower() == "today")
                    {
                        currentDate = DateTime.Now;
                        lstSeller = lstSeller.Where(x => x.CreatedAt == currentDate).ToList();
                    }
                    else
                    {
                        int _days = Convert.ToInt32(days);
                        currentDate = DateTime.Now.AddDays(-_days);
                        lstSeller = lstSeller.Where(x => x.CreatedAt >= currentDate).ToList();
                    }
                }

                //var query = from seller in lstSeller
                //            join kyc in lstKyc
                //            on seller.Id equals kyc.UserID into sellerKycGroup
                //            from sellerKyc in sellerKycGroup.DefaultIfEmpty()
                //            select new { Seller = seller, KYCDetails = sellerKyc };

                totalSellerCount = lstSeller.Count();

                //newSellerCount = query.Count(item => item.KYCDetails == null);
            }

            //Active Seller

            // Filter lstSeller by status = "Active" and lstKyc by status = "Approved"
            var filteredActiveSeller = lstSeller.Where(seller => seller.Status.ToLower() == "active");
            //var filteredActiveKyc = lstKyc.Where(kyc => kyc.Status.ToLower() == "approved");
            ActiveSellerCount = filteredActiveSeller.Count();

            //// Perform an outer join between the filtered lists based on the seller ID
            //var Activequery = from seller in filteredActiveSeller
            //                  join kyc in filteredActiveKyc
            //                  on seller.Id equals kyc.UserID into sellerKycGroup
            //                  from sellerKyc in sellerKycGroup.DefaultIfEmpty()
            //                  select new { Seller = seller, KYCDetails = sellerKyc };

            //// Count the number of items where KYC status is "Approved"
            //ActiveSellerCount = Activequery.Count(item => item.KYCDetails != null);

            //Inactive Seller

            // Filter lstSeller by status = "Active" and lstKyc by status = "Approved"
            var filteredSeller = lstSeller.Where(seller => seller.Status.ToLower() == "inactive");
            //var filteredKyc = lstKyc.Where(kyc => kyc.Status.ToLower() != "approved");
            inactiveSellerCount = filteredSeller.Count();

            //// Perform an outer join between the filtered lists based on the seller ID
            //var Inactivequery = from seller in filteredSeller
            //                    join kyc in filteredKyc
            //                    on seller.Id equals kyc.UserID into sellerKycGroup
            //                    from sellerKyc in sellerKycGroup.DefaultIfEmpty()
            //                    select new { Seller = seller, KYCDetails = sellerKyc };

            //// Count the number of items where KYC status is "Approved"
            //inactiveSellerCount = Inactivequery.Count(item => item.KYCDetails != null);

            var newSeller = lstSeller.Where(seller => seller.Status.ToLower() == "pending" || seller.Status.ToLower() == "in progress");
            newSellerCount = newSeller.Count();

            SellerCounts sellerCounts = new SellerCounts();
            sellerCounts.Total = totalSellerCount;
            sellerCounts.NewSellers = newSellerCount;
            sellerCounts.Active = ActiveSellerCount;
            sellerCounts.Inactive = inactiveSellerCount;

            BaseResponse<SellerCounts> sellerbaseResponse = new BaseResponse<SellerCounts>();
            sellerbaseResponse.Data = sellerCounts;
            return Ok(sellerbaseResponse);
        }

        [HttpGet("getSellerChart")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult GetSellerCountForChart(string? ReportDays = null)
        {
            int tmpReportDay = Convert.ToInt32(ReportDays);

            DateTime fromDate = DateTime.Now.AddDays(-tmpReportDay);
            DateTime toDate = DateTime.Now;

            BaseResponse<SellerListModel> baseResponse = new BaseResponse<SellerListModel>();
            var response = helper.ApiCall(IDServerUrl, EndPoints.SellerList + "?pageIndex=0&pageSize=0" + "&fromDate=" + fromDate + "&toDate=" + toDate, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<SellerListModel> lstSeller = baseResponse.Data as List<SellerListModel> ?? new List<SellerListModel>();

            int DayRange = 0;
            if (ReportDays == "7") { DayRange = 1; }
            else if (ReportDays == "1") { DayRange = 1; }
            else if (ReportDays == "15") { DayRange = 1; }
            else if (ReportDays == "30") { DayRange = 5; }
            else if (ReportDays == "90") { DayRange = 30; }
            else { DayRange = 30; }

            List<DashboardReport> lstclsAdminReport = new List<DashboardReport>();

            while (Convert.ToDateTime(fromDate) < Convert.ToDateTime(toDate))
            {
                DashboardReport objDashboardReport = new DashboardReport();
                if (Convert.ToUInt32(ReportDays) == 1 || Convert.ToUInt32(ReportDays) == 7 || Convert.ToUInt32(ReportDays) == 15)
                {
                    objDashboardReport.MonthDetails = Convert.ToDateTime(fromDate).ToString("dd'/'MMM'/'yyyy");
                    objDashboardReport.MonthStats = lstSeller.Where(p => Convert.ToDateTime(p.CreatedAt).Date == (Convert.ToDateTime(fromDate).Date)).ToList().Count();
                }
                else if (Convert.ToUInt32(ReportDays) == 30)
                {
                    DateTime TmpCompareEndDate = Convert.ToDateTime(fromDate).AddDays(DayRange - 1);

                    objDashboardReport.MonthDetails = Convert.ToDateTime(fromDate).ToString("dd'/'MMM'/'yyyy");
                    objDashboardReport.MonthStats = lstSeller.Where(p => (Convert.ToDateTime(p.CreatedAt).Date >= (Convert.ToDateTime(fromDate).Date)) &&
                        (Convert.ToDateTime(p.CreatedAt).Date <= (Convert.ToDateTime(TmpCompareEndDate).Date))).ToList().Count();
                }
                else if (Convert.ToUInt32(ReportDays) > 30)
                {
                    DateTime TmpCompareEndDate = Convert.ToDateTime(fromDate).AddDays(DayRange - 1);

                    objDashboardReport.MonthDetails = Convert.ToDateTime(fromDate).ToString("MMM");
                    objDashboardReport.MonthStats = lstSeller.Where(p => (Convert.ToDateTime(p.CreatedAt).Date >= (Convert.ToDateTime(fromDate).Date)) &&
                        (Convert.ToDateTime(p.CreatedAt).Date <= (Convert.ToDateTime(TmpCompareEndDate).Date))).ToList().Count();
                }

                fromDate = Convert.ToDateTime(fromDate).AddDays(DayRange);
                toDate = Convert.ToDateTime(toDate).AddSeconds(1);


                lstclsAdminReport.Add(objDashboardReport);
            }

            BaseResponse<DashboardReport> sellerbaseResponse = new BaseResponse<DashboardReport>();
            sellerbaseResponse.Data = lstclsAdminReport;
            return Ok(sellerbaseResponse);
        }

        [HttpGet("getProductChart")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> SellerProductChart(string? ReportDays = null)
        {
            int tmpReportDay = Convert.ToInt32(ReportDays);

            DateTime fromDate = DateTime.Now.AddDays(-tmpReportDay);
            DateTime toDate = DateTime.Now;

            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();

            var response = helper.ApiCall(URL, EndPoints.SellerProduct + "?fromDate=" + Convert.ToDateTime(fromDate).ToString("dd'-'MM'-'yyyy") + "&toDate=" + Convert.ToDateTime(toDate).ToString("dd'-'MM'-'yyyy") + "&pageIndex=" + 0 + "&pageSize=" + 0 + "&isDeleted=false", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<SellerProduct> sellerProducts = baseResponse.Data as List<SellerProduct> ?? new List<SellerProduct>();

            int DayRange = 0;
            if (ReportDays == "7") { DayRange = 1; }
            else if (ReportDays == "1") { DayRange = 1; }
            else if (ReportDays == "15") { DayRange = 1; }
            else if (ReportDays == "30") { DayRange = 5; }
            else if (ReportDays == "90") { DayRange = 30; }
            else { DayRange = 30; }

            List<DashboardReport> lstclsAdminReport = new List<DashboardReport>();

            while (Convert.ToDateTime(fromDate) < Convert.ToDateTime(toDate))
            {
                DashboardReport objDashboardReport = new DashboardReport();
                if (Convert.ToUInt32(ReportDays) == 1 || Convert.ToUInt32(ReportDays) == 7 || Convert.ToUInt32(ReportDays) == 15)
                {
                    objDashboardReport.MonthDetails = Convert.ToDateTime(fromDate).ToString("dd'/'MMM'/'yyyy");
                    objDashboardReport.MonthStats = sellerProducts.Where(p => Convert.ToDateTime(p.CreatedAt).Date == (Convert.ToDateTime(fromDate).Date)).ToList().Count();
                }
                else if (Convert.ToUInt32(ReportDays) == 30)
                {
                    DateTime TmpCompareEndDate = Convert.ToDateTime(fromDate).AddDays(DayRange - 1);

                    objDashboardReport.MonthDetails = Convert.ToDateTime(fromDate).ToString("dd'/'MMM'/'yyyy");
                    objDashboardReport.MonthStats = sellerProducts.Where(p => (Convert.ToDateTime(p.CreatedAt).Date >= (Convert.ToDateTime(fromDate).Date)) &&
                        (Convert.ToDateTime(p.CreatedAt).Date <= (Convert.ToDateTime(TmpCompareEndDate).Date))).ToList().Count();
                }
                else if (Convert.ToUInt32(ReportDays) > 30)
                {
                    DateTime TmpCompareEndDate = Convert.ToDateTime(fromDate).AddDays(DayRange - 1);

                    objDashboardReport.MonthDetails = Convert.ToDateTime(fromDate).ToString("MMM");
                    objDashboardReport.MonthStats = sellerProducts.Where(p => (Convert.ToDateTime(p.CreatedAt).Date >= (Convert.ToDateTime(fromDate).Date)) &&
                        (Convert.ToDateTime(p.CreatedAt).Date <= (Convert.ToDateTime(TmpCompareEndDate).Date))).ToList().Count();
                }

                fromDate = Convert.ToDateTime(fromDate).AddDays(DayRange);
                toDate = Convert.ToDateTime(toDate).AddSeconds(1);


                lstclsAdminReport.Add(objDashboardReport);
            }

            BaseResponse<DashboardReport> sellerbaseResponse = new BaseResponse<DashboardReport>();
            sellerbaseResponse.Data = lstclsAdminReport;
            return Ok(sellerbaseResponse);
        }

        [HttpGet("getKycCounts")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult GetKycCounts(string? days = "All")
        {
            BaseResponse<KycCounts> baseResponse = new BaseResponse<KycCounts>();
            var response = helper.ApiCall(UserUrl, EndPoints.GetKycCounts + "?days=" + days, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            KycCounts kycCounts = baseResponse.Data as KycCounts;
            return Ok(baseResponse);
        }

        [HttpGet("getBrandCounts")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult GetBrandCounts(string? days = "All")
        {
            BaseResponse<BrandCounts> baseResponse = new BaseResponse<BrandCounts>();
            var response = helper.ApiCall(UserUrl, EndPoints.GetBrandCounts + "?days=" + days, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            BrandCounts brandCounts = baseResponse.Data as BrandCounts;
            return Ok(baseResponse);
        }

        [HttpGet("getProductCounts")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult GetProductCounts(string? days = "All")
        {
            BaseResponse<ProductCounts> baseResponse = new BaseResponse<ProductCounts>();
            var response = helper.ApiCall(URL, EndPoints.GetProductCounts + "?days=" + days, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            ProductCounts productCounts = baseResponse.Data as ProductCounts;
            return Ok(baseResponse);
        }

        [HttpGet("getOrderCounts")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult getOrderCounts(string? days = "All")
        {
            BaseResponse<OrdersCount> baseResponse = new BaseResponse<OrdersCount>();
            var response = helper.ApiCall(OrderUrl, EndPoints.OrderCount + "?days=" + days, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            OrdersCount ordersCount = baseResponse.Data as OrdersCount;
            return Ok(baseResponse);
        }


        [HttpGet("getOrderChart")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> OrderChart(string? ReportDays = null)
        {
            int tmpReportDay = Convert.ToInt32(ReportDays);

            DateTime fromDate = DateTime.Now.AddDays(-tmpReportDay);
            DateTime toDate = DateTime.Now;

            //BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();

            //var response = helper.ApiCall(URL, EndPoints.SellerProduct + "?fromDate=" + Convert.ToDateTime(fromDate).ToString("dd'-'MM'-'yyyy") + "&toDate=" + Convert.ToDateTime(toDate).ToString("dd'-'MM'-'yyyy") + "&pageIndex=" + 0 + "&pageSize=" + 0 + "&isDeleted=false", "GET", null);
            //baseResponse = baseResponse.JsonParseList(response);
            //List<SellerProduct> sellerProducts = baseResponse.Data as List<SellerProduct>;


            BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
            var GetResponse = helper.ApiCall(OrderUrl, EndPoints.OrderItems + "?fromDate=" + Convert.ToDateTime(fromDate).ToString("dd'-'MM'-'yyyy") + "&toDate=" + Convert.ToDateTime(toDate).ToString("dd'-'MM'-'yyyy") + "&PageIndex=0&PageSize=0&notInStatus=" + false, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);


            List<OrderItems> orders = baseResponse.Data as List<OrderItems> ?? new List<OrderItems>();
            orders = orders.Where(p => p.Status != null && p.Status.ToLower() != "replaced" && p.Status.ToLower() != "returned" && p.Status.ToLower() != "exchanged" && p.Status.ToLower() != "initiate" && p.Status.ToLower() != "cancelled").ToList();
            int DayRange = 0;
            if (ReportDays == "7") { DayRange = 1; }
            else if (ReportDays == "1") { DayRange = 1; }
            else if (ReportDays == "15") { DayRange = 1; }
            else if (ReportDays == "30") { DayRange = 5; }
            else if (ReportDays == "90") { DayRange = 30; }
            else { DayRange = 30; }

            List<DashboardReport> lstclsAdminReport = new List<DashboardReport>();

            while (Convert.ToDateTime(fromDate) < Convert.ToDateTime(toDate))
            {
                DashboardReport objDashboardReport = new DashboardReport();
                if (Convert.ToUInt32(ReportDays) == 1 || Convert.ToUInt32(ReportDays) == 7 || Convert.ToUInt32(ReportDays) == 15)
                {
                    objDashboardReport.MonthDetails = Convert.ToDateTime(fromDate).ToString("dd'/'MMM'/'yyyy");
                    objDashboardReport.MonthStats = orders.Where(p => Convert.ToDateTime(p.OrderDate).Date == (Convert.ToDateTime(fromDate).Date)).Select(p => Convert.ToInt32(p.SubTotal)).Sum();
                }
                else if (Convert.ToUInt32(ReportDays) == 30)
                {
                    DateTime TmpCompareEndDate = Convert.ToDateTime(fromDate).AddDays(DayRange - 1);

                    objDashboardReport.MonthDetails = Convert.ToDateTime(fromDate).ToString("dd'/'MMM'/'yyyy");
                    objDashboardReport.MonthStats = orders.Where(p => (Convert.ToDateTime(p.OrderDate).Date >= (Convert.ToDateTime(fromDate).Date)) &&
                        (Convert.ToDateTime(p.OrderDate).Date <= (Convert.ToDateTime(TmpCompareEndDate).Date))).Select(p => Convert.ToInt32(p.SubTotal)).Sum();
                }
                else if (Convert.ToUInt32(ReportDays) > 30)
                {
                    DateTime TmpCompareEndDate = Convert.ToDateTime(fromDate).AddDays(DayRange - 1);

                    objDashboardReport.MonthDetails = Convert.ToDateTime(fromDate).ToString("MMM");
                    objDashboardReport.MonthStats = orders.Where(p => (Convert.ToDateTime(p.OrderDate).Date >= (Convert.ToDateTime(fromDate).Date)) &&
                        (Convert.ToDateTime(p.OrderDate).Date <= (Convert.ToDateTime(TmpCompareEndDate).Date))).Select(p => Convert.ToInt32(p.SubTotal)).Sum();
                }

                fromDate = Convert.ToDateTime(fromDate).AddDays(DayRange);
                toDate = Convert.ToDateTime(toDate).AddSeconds(1);


                lstclsAdminReport.Add(objDashboardReport);
            }

            BaseResponse<DashboardReport> sellerbaseResponse = new BaseResponse<DashboardReport>();
            sellerbaseResponse.Data = lstclsAdminReport;
            return Ok(sellerbaseResponse);
        }


        [HttpGet("getTopSellingProducts")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult getTopSellingProducts(int? top = 5, string? sellerId = null, string? days = "All")
        {
            BaseResponse<TopSellingProductsDto> baseResponse = new BaseResponse<TopSellingProductsDto>();

            string url = string.Empty;
            if (!string.IsNullOrEmpty(sellerId))
            {
                url += "&sellerId=" + sellerId;
            }

            var response = helper.ApiCall(OrderUrl, EndPoints.Reports+ "/getTopSellingProducts" + "?top=" + top + "&days=" + days + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);

            if (baseResponse.code == 200)
            {
                List<TopSellingProductsDto> TopSellingProductslst = baseResponse.Data as List<TopSellingProductsDto> ?? new List<TopSellingProductsDto>();
                if (TopSellingProductslst.Count > 0)
                {
                    string productIds = string.Join(",", TopSellingProductslst.Select(p => p.ProductID));

                    BaseResponse<ProductDetailsReport> productbaseResponse = new BaseResponse<ProductDetailsReport>();
                    var productresponse = helper.ApiCall(URL, EndPoints.Reports + "/getProductDetailsReport" + "?productIds=" + productIds, "GET", null);
                    productbaseResponse = productbaseResponse.JsonParseList(productresponse);
                    List<ProductDetailsReport> productlst = productbaseResponse.Data as List<ProductDetailsReport> ?? new List<ProductDetailsReport>();


                    var DTOList = TopSellingProductslst.Select(x => new TopSellingProductsDto
                    {
                        ProductID = x.ProductID,
                        ProductGUID = x.ProductGUID,
                        TotalOrders = x.TotalOrders,
                        TotalSell = x.TotalSell,
                        ProductName = productlst?.Where(p=>p.ProductId == x.ProductID).FirstOrDefault()?.ProductName,
                        ProductSKU = productlst?.Where(p => p.ProductId == x.ProductID).FirstOrDefault()?.ProductSKU,
                        ProductImage = productlst?.Where(p => p.ProductId == x.ProductID).FirstOrDefault()?.ProductImage,
                    }).ToList();

                    baseResponse.Data = DTOList;
                }
            }
            return Ok(baseResponse);
        }

        [HttpGet("getTopSellingSellers")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult getTopSellingSellers(int? top = 5, string? days = "All")
        {
            BaseResponse<TopSellingSellersDto> baseResponse = new BaseResponse<TopSellingSellersDto>();

            var response = helper.ApiCall(OrderUrl, EndPoints.Reports + "/getTopSellingSellers" + "?top=" + top + "&days=" + days, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);

            if (baseResponse.code == 200)
            {
                List<TopSellingSellersDto> TopSellingSellerslst = baseResponse.Data as List<TopSellingSellersDto> ?? new List<TopSellingSellersDto>();
                if (TopSellingSellerslst.Count > 0)
                {
                    string sellerIds = string.Join(",", TopSellingSellerslst.Select(p => p.SellerID));

                    BaseResponse<KYCDetails> productbaseResponse = new BaseResponse<KYCDetails>();
                    var productresponse = helper.ApiCall(UserUrl, EndPoints.KYCDetails + "?UserID=" + sellerIds + "&pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
                    productbaseResponse = productbaseResponse.JsonParseList(productresponse);
                    List<KYCDetails> kyclst = productbaseResponse.Data as List<KYCDetails> ?? new List<KYCDetails>();


                    var DTOList = TopSellingSellerslst.Select(x => new TopSellingSellersDto
                    {
                        SellerID = x.SellerID,
                        TotalOrders = x.TotalOrders,
                        TotalSell = x.TotalSell,
                        SellerName = kyclst?.Where(p => p.UserID == x.SellerID).FirstOrDefault()?.DisplayName,
                        SellerLogo = kyclst?.Where(p => p.UserID == x.SellerID).FirstOrDefault()?.Logo,
                    }).ToList();

                    baseResponse.Data = DTOList;
                }
            }
            return Ok(baseResponse);
        }

        [HttpGet("getTopSellingBrands")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult getTopSellingBrands(int? top = 5, string? sellerId = null, string? days = "All")
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(sellerId))
            {
                url += "&sellerId=" + sellerId;
            }

            BaseResponse<TopSellingBrandsDto> baseResponse = new BaseResponse<TopSellingBrandsDto>();

            var response = helper.ApiCall(OrderUrl, EndPoints.Reports + "/getTopSellingBrands" + "?top=" + top + "&days=" + days + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);

            if (baseResponse.code == 200)
            {
                List<TopSellingBrandsDto> TopSellingBrandslst = baseResponse.Data as List<TopSellingBrandsDto> ?? new List<TopSellingBrandsDto>();
                if (TopSellingBrandslst.Count > 0)
                {
                    string brandIds = string.Join(",", TopSellingBrandslst.Select(p => p.BrandID));

                    BaseResponse<BrandLibrary> productbaseResponse = new BaseResponse<BrandLibrary>();
                    var productresponse = helper.ApiCall(UserUrl, EndPoints.Brand + "?brandIds=" + brandIds + "&pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
                    productbaseResponse = productbaseResponse.JsonParseList(productresponse);
                    List<BrandLibrary> brandlst = productbaseResponse.Data as List<BrandLibrary> ?? new List<BrandLibrary>();


                    var DTOList = TopSellingBrandslst.Select(x => new TopSellingBrandsDto
                    {
                        SellerID = x.SellerID,
                        BrandID = x.BrandID,
                        TotalOrders = x.TotalOrders,
                        TotalSell = x.TotalSell,
                        BrandName = brandlst?.Where(p => p.ID == x.BrandID).FirstOrDefault()?.Name,
                        BrandLogo = brandlst?.Where(p => p.ID == x.BrandID).FirstOrDefault()?.Logo,
                    }).ToList();

                    baseResponse.Data = DTOList;
                }
            }
            return Ok(baseResponse);
        }

        [HttpGet("getTopUsedCoupons")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult getTopUsedCoupons(int? top = 5, string? sellerId = null, string? days = "All")
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(sellerId))
            {
                url += "&sellerId=" + sellerId;
            }
            BaseResponse<TopUsedCouponsDto> baseResponse = new BaseResponse<TopUsedCouponsDto>();
            var response = helper.ApiCall(OrderUrl, EndPoints.Reports + "/getTopUsedCoupons" + "?top=" + top + "&days=" + days + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            return Ok(baseResponse);
        }
    }
}
