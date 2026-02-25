using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.IDServer;
using API_Gateway.Models.Entity.Order;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Seller
{
    [Route("api/Seller/[controller]")]
    [ApiController]
    public class SellerDashboardController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string IDServerUrl = string.Empty;
        public string OrderUrl = string.Empty;
        public string UserUrl = string.Empty;
        private ApiHelper helper;

        public SellerDashboardController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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

        [HttpGet("getBrandCounts")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult GetBrandCounts(string? days = "All")
        {
            string sellerId = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            BaseResponse<BrandCounts> baseResponse = new BaseResponse<BrandCounts>();
            var response = helper.ApiCall(UserUrl, EndPoints.GetBrandCounts + "?days=" + days + "&sellerId=" + sellerId, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            BrandCounts brandCounts = baseResponse.Data as BrandCounts;
            return Ok(baseResponse);
        }

        [HttpGet("getProductCounts")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult GetProductCounts(string? days = "All")
        {
            string sellerId = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            BaseResponse<ProductCounts> baseResponse = new BaseResponse<ProductCounts>();
            var response = helper.ApiCall(URL, EndPoints.GetProductCounts + "?days=" + days + "&sellerId=" + sellerId, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            ProductCounts productCounts = baseResponse.Data as ProductCounts;
            return Ok(baseResponse);
        }

        [HttpGet("getOrderCounts")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult getOrderCounts(string? days = "All")
        {
            string sellerId = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            BaseResponse<OrdersCount> baseResponse = new BaseResponse<OrdersCount>();
            var response = helper.ApiCall(OrderUrl, EndPoints.OrderCount + "?days=" + days + "&sellerId=" + sellerId, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            OrdersCount ordersCount = baseResponse.Data as OrdersCount;
            return Ok(baseResponse);
        }
        
        [HttpGet("getProductChart")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> SellerProductChart(string? ReportDays = null)
        {
            string sellerId = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            int tmpReportDay = Convert.ToInt32(ReportDays);

            DateTime fromDate = DateTime.Now.AddDays(-tmpReportDay);
            DateTime toDate = DateTime.Now;

            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();

            var response = helper.ApiCall(URL, EndPoints.SellerProduct + "?sellerId=" + sellerId + "&fromDate=" + Convert.ToDateTime(fromDate).ToString("dd'-'MM'-'yyyy") + "&toDate=" + Convert.ToDateTime(toDate).ToString("dd'-'MM'-'yyyy") + "&pageIndex=" + 0 + "&pageSize=" + 0 + "&isDeleted=false", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<SellerProduct> sellerProducts = baseResponse.Data as List<SellerProduct>;

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

        [HttpGet("getOrderChart")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> OrderChart(string? ReportDays = null)
        {
            string sellerId = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            int tmpReportDay = Convert.ToInt32(ReportDays);

            DateTime fromDate = DateTime.Now.AddDays(-tmpReportDay);
            DateTime toDate = DateTime.Now;

            //BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();

            //var response = helper.ApiCall(URL, EndPoints.SellerProduct + "?fromDate=" + Convert.ToDateTime(fromDate).ToString("dd'-'MM'-'yyyy") + "&toDate=" + Convert.ToDateTime(toDate).ToString("dd'-'MM'-'yyyy") + "&pageIndex=" + 0 + "&pageSize=" + 0 + "&isDeleted=false", "GET", null);
            //baseResponse = baseResponse.JsonParseList(response);
            //List<SellerProduct> sellerProducts = baseResponse.Data as List<SellerProduct>;


            BaseResponse<OrderItems> baseResponse = new BaseResponse<OrderItems>();
            var GetResponse = helper.ApiCall(OrderUrl, EndPoints.OrderItems + "?SellerId=" + sellerId + "&fromDate=" + Convert.ToDateTime(fromDate).ToString("dd'-'MM'-'yyyy") + "&toDate=" + Convert.ToDateTime(toDate).ToString("dd'-'MM'-'yyyy") + "&PageIndex=0&PageSize=0&notInStatus=" + false, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);


            List<OrderItems> orders = baseResponse.Data as List<OrderItems> ?? new List<OrderItems>();
            orders = orders.Where(p => p.Status.ToLower() != "replaced" && p.Status.ToLower() != "returned" && p.Status.ToLower() != "exchanged" && p.Status.ToLower() != "initiate" && p.Status.ToLower() != "cancelled").ToList();
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
    }
}
