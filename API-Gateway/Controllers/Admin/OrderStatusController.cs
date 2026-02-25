using API_Gateway.Helper;
using API_Gateway.Models.Entity.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<OrderStatusLibrary> baseResponse = new BaseResponse<OrderStatusLibrary>();
        private ApiHelper helper;
        public OrderStatusController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Save(OrderStatusLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.OrderStatus + "?OrderStatus=" + model.OrderStatus, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<OrderStatusLibrary> tmp = baseResponse.Data as List<OrderStatusLibrary> ?? new List<OrderStatusLibrary>();
            if (tmp.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                OrderStatusLibrary Status = new OrderStatusLibrary();
                Status.OrderStatus = model.OrderStatus;

                Status.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                Status.CreatedAt = DateTime.Now;

                var response = helper.ApiCall(URL, EndPoints.OrderStatus, "POST", Status);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Update(OrderStatusLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.OrderStatus + "?OrderStatus=" + model.OrderStatus, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<OrderStatusLibrary> tmp = baseResponse.Data as List<OrderStatusLibrary> ?? new List<OrderStatusLibrary>();
            if (tmp.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(URL, EndPoints.OrderStatus + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(response);
                OrderStatusLibrary Status = baseResponse.Data as OrderStatusLibrary;

                Status.Id = model.Id;
                Status.OrderStatus = model.OrderStatus;

                Status.ModifiedAt = DateTime.Now;
                Status.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                response = helper.ApiCall(URL, EndPoints.OrderStatus, "PUT", Status);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Delete(int? id = 0)
        {
            var temp = helper.ApiCall(URL, EndPoints.OrderStatus + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<OrderStatusLibrary> templist = baseResponse.Data as List<OrderStatusLibrary> ?? new List<OrderStatusLibrary>();
            if (templist.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.OrderStatus + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.OrderStatus + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> ById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.OrderStatus + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Search(string searchtext, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(URL, EndPoints.OrderStatus + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
