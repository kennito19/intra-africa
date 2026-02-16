using API_Gateway.Common.orders;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.User
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        private readonly HttpContext _httpContext;
        BaseResponse<Orders> baseResponse = new BaseResponse<Orders>();
        private ApiHelper helper;
        public OrderController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
        }

        //[HttpGet("byUserId")]
        //[Authorize(Roles = "Admin, Seller, Customer")]
        //public ActionResult<ApiHelper> ByUserId(string UserId)
        //{
        //    if (UserId != null)
        //    {
        //        var response = helper.ApiCall(URL, EndPoints.Orders + "?UserId=" + UserId, "GET", null);
        //        baseResponse = baseResponse.JsonParseList(response);
        //    }
        //    return Ok(baseResponse);
        //}

        //[HttpGet("byId")]
        //[Authorize(Roles = "Admin, Seller, Customer")]
        //public ActionResult<ApiHelper> ById(string UserId, int id)
        //{
        //    if (UserId != null)
        //    {
        //        var response = helper.ApiCall(URL, EndPoints.Orders + "?UserId=" + UserId + "&Id=" + id, "GET", null);
        //        baseResponse = baseResponse.JsonParseRecord(response);
        //    }
        //    return Ok(baseResponse);
        //}

        //[HttpGet("byGUID")]
        //[Authorize(Roles = "Admin, Seller, Customer")]
        //public ActionResult<ApiHelper> ByGUID(string UserId, string GUID)
        //{
        //    if (UserId != null)
        //    {
        //        var response = helper.ApiCall(URL, EndPoints.Orders + "?UserId=" + UserId + "&GUID=" + GUID, "GET", null);
        //        baseResponse = baseResponse.JsonParseRecord(response);
        //    }
        //    return Ok(baseResponse);
        //}

        //[HttpGet("byOrderNo")]
        //[Authorize(Roles = "Admin, Seller, Customer")]
        //public ActionResult<ApiHelper> ByOrderNo(string UserId, string OrderNo)
        //{
        //    if (UserId != null)
        //    {
        //        var response = helper.ApiCall(URL, EndPoints.Orders + "?UserId=" + UserId + "&OrderNo=" + OrderNo, "GET", null);
        //        baseResponse = baseResponse.JsonParseRecord(response);
        //    }
        //    return Ok(baseResponse);
        //}

        [HttpGet]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(false, pageIndex, pageSize, 0, null, null, null, null, userID, null, null);
            return Ok(res);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> ById(int id)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(false, 0, 0, id, null, null, null, null, userID, null, null);
            return Ok(res);
        }

        [HttpGet("byOrderId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> ByGUID(string orderGuid)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(false, 0, 0, 0, orderGuid, null, null, null, userID, null, null);
            return Ok(res);
        }

        [HttpGet("byOrderNo")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> ByOrderNo(string orderNo)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(false, 0, 0, 0, null, orderNo, null, null, userID, null, null);
            return Ok(res);
        }
        
        [HttpGet("byOrderRefNo")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> byOrderRefNo(string orderRefNo)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(false, 0, 0, 0, null, null, orderRefNo, null, userID, null, null);
            return Ok(res);
        }

        [HttpGet("byUserId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> ByUserId(string userId, int? pageIndex = 1, int? pageSize = 10)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(false, pageIndex, pageSize, 0, null, null, null, null, userId, null, null);
            return Ok(res);
        }

        [HttpGet("byStatus")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> ByStatus(string userId, string status, int? pageIndex = 1, int? pageSize = 10)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(false, pageIndex, pageSize, 0, null, null, null, null, userId, status, null);
            return Ok(res);
        }

        [HttpGet("bysearchText")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> BysearchText(string userId, string? searchText = null, string? status = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GetOrders getorder = new GetOrders(_configuration, _httpContext, userID);
            var res = getorder.Get(false, pageIndex, pageSize, 0, null, null, null, null, userId, status, searchText);
            return Ok(res);
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

    }
}
