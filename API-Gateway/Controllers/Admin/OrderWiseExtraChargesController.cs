using API_Gateway.Helper;
using API_Gateway.Models.Entity.Order;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderWiseExtraChargesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string URL = string.Empty;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        BaseResponse<OrderWiseExtraCharges> baseResponse = new BaseResponse<OrderWiseExtraCharges>();
        public OrderWiseExtraChargesController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
        }

        [HttpPost]
        public ActionResult<ApiHelper> Save(OrderWiseExtraCharges model)
        {
            var temp = helper.ApiCall(URL, EndPoints.OrderWiseExtraCharges + "?OrderID=" + model.OrderID + "&OrderItemID=" + model.OrderItemID, "GET");
            baseResponse = baseResponse.JsonParseList(temp);
            List<OrderWiseExtraCharges> tmp = baseResponse.Data as List<OrderWiseExtraCharges> ?? new List<OrderWiseExtraCharges>();
            if (tmp.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                OrderWiseExtraCharges owec = new OrderWiseExtraCharges();
                owec.OrderID = model.OrderID;
                owec.OrderItemID = model.OrderItemID;
                owec.ChargesType = model.ChargesType;
                owec.ChargesPaidBy = model.ChargesPaidBy;
                owec.ChargesIn = model.ChargesIn;
                owec.ChargesValueInPercentage = model.ChargesValueInPercentage;
                owec.ChargesValueInAmount = model.ChargesValueInAmount;
                owec.ChargesMaxAmount = model.ChargesMaxAmount;

                owec.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                owec.CreatedAt = DateTime.Now;

                var response = helper.ApiCall(URL, EndPoints.OrderWiseExtraCharges, "POST", owec);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        public ActionResult<ApiHelper> Update(OrderWiseExtraCharges model)
        {
            var temp = helper.ApiCall(URL, EndPoints.OrderWiseExtraCharges + "?OrderID=" + model.OrderID + "&OrderItemID=" + model.OrderItemID, "GET");
            baseResponse = baseResponse.JsonParseList(temp);
            List<OrderWiseExtraCharges> tmp = baseResponse.Data as List<OrderWiseExtraCharges> ?? new List<OrderWiseExtraCharges>();
            if (tmp.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(URL, EndPoints.OrderWiseExtraCharges + "?Id=" + model.Id, "GET");
                baseResponse = baseResponse.JsonParseRecord(response);
                OrderWiseExtraCharges owec = baseResponse.Data as OrderWiseExtraCharges;
                owec.OrderID = model.OrderID;
                owec.OrderItemID = model.OrderItemID;
                owec.ChargesType = model.ChargesType;
                owec.ChargesPaidBy = model.ChargesPaidBy;
                owec.ChargesIn = model.ChargesIn;
                owec.ChargesValueInPercentage = model.ChargesValueInPercentage;
                owec.ChargesValueInAmount = model.ChargesValueInAmount;
                owec.ChargesMaxAmount = model.ChargesMaxAmount;

                owec.ModifiedAt = DateTime.Now;
                owec.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                response = helper.ApiCall(URL, EndPoints.OrderWiseExtraCharges, "PUT", owec);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        public ActionResult<ApiHelper> Delete(int? id = 0)
        {
            var temp = helper.ApiCall(URL, EndPoints.OrderWiseExtraCharges + "?Id=" + id, "GET");
            baseResponse = baseResponse.JsonParseList(temp);
            List<OrderWiseExtraCharges> templist = baseResponse.Data as List<OrderWiseExtraCharges> ?? new List<OrderWiseExtraCharges>();
            if (templist.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.OrderWiseExtraCharges + "?Id=" + id, "DELETE");
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet("Search")]
        public ActionResult<ApiHelper> Search(string searchText)
        {
            var response = helper.ApiCall(URL, EndPoints.OrderWiseExtraCharges + "?searchText=" + HttpUtility.UrlEncode(searchText), "GET");
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
