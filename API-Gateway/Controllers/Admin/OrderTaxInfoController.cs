using API_Gateway.Helper;
using API_Gateway.Models.Entity.Order;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderTaxInfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private ApiHelper helper;
        public string URL = string.Empty;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        BaseResponse<OrderTaxInfo> baseResponse = new BaseResponse<OrderTaxInfo>();

        public OrderTaxInfoController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
        }

        [HttpPost]
        public ActionResult<ApiHelper> Save(OrderTaxInfo model)
        {
            var temp = helper.ApiCall(URL, EndPoints.OrderTaxInfo + "?OrderID=" + model.OrderID + "&OrderItemID=" + model.OrderItemID, "GET");
            baseResponse = baseResponse.JsonParseList(temp);
            List<OrderTaxInfo> tmp = (List<OrderTaxInfo>)baseResponse.Data;
            if (tmp.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                OrderTaxInfo orderTax = new OrderTaxInfo();
                orderTax.OrderID = model.OrderID;
                orderTax.OrderItemID = model.OrderItemID;
                orderTax.ProductID = model.ProductID;
                orderTax.SellerProductID = model.SellerProductID;
                orderTax.ShippingCharge = model.ShippingCharge;
                orderTax.ShippingZone = model.ShippingZone;
                orderTax.TaxOnShipping = model.TaxOnShipping;
                orderTax.CommissionIn = null;
                orderTax.CommissionRate = 0;
                orderTax.CommissionAmount = 0;
                orderTax.TaxOnCommission = 0;
                orderTax.NetEarn = model.NetEarn;
                orderTax.OrderTaxRateId = model.OrderTaxRateId;
                orderTax.OrderTaxRate = model.OrderTaxRate;
                orderTax.HSNCode = model.HSNCode;
                orderTax.ShipmentBy = model.ShipmentBy;
                orderTax.ShipmentPaidBy = model.ShipmentPaidBy;
                orderTax.IsSellerWithGSTAtOrderTime = model.IsSellerWithGSTAtOrderTime;
                orderTax.WeightSlab = model.WeightSlab;

                orderTax.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                orderTax.CreatedAt = DateTime.Now;

                var response = helper.ApiCall(URL, EndPoints.OrderTaxInfo, "POST", orderTax);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        public ActionResult<ApiHelper> Update(OrderTaxInfo model)
        {
            var temp = helper.ApiCall(URL, EndPoints.OrderTaxInfo + "?Id=" + model.Id, "GET");
            baseResponse = baseResponse.JsonParseList(temp);
            List<OrderTaxInfo> tmp = (List<OrderTaxInfo>)baseResponse.Data;
            if (tmp.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(URL, EndPoints.OrderTaxInfo + "?Id=" + model.Id, "GET");
                baseResponse = baseResponse.JsonParseRecord(response);
                OrderTaxInfo OrderTax = (OrderTaxInfo)baseResponse.Data;

                OrderTax.Id = model.Id;
                OrderTax.ShippingCharge = model.ShippingCharge;
                OrderTax.ShippingZone = model.ShippingZone;
                OrderTax.TaxOnShipping = model.TaxOnShipping;
                OrderTax.CommissionIn = null;
                OrderTax.CommissionRate = 0;
                OrderTax.CommissionAmount = 0;
                OrderTax.TaxOnCommission = 0;
                OrderTax.NetEarn = model.NetEarn;
                OrderTax.OrderTaxRateId = model.OrderTaxRateId;
                OrderTax.OrderTaxRate = model.OrderTaxRate;
                OrderTax.HSNCode = model.HSNCode;
                OrderTax.ShipmentBy = model.ShipmentBy;
                OrderTax.ShipmentPaidBy = model.ShipmentPaidBy;
                OrderTax.IsSellerWithGSTAtOrderTime = model.IsSellerWithGSTAtOrderTime;
                OrderTax.WeightSlab = model.WeightSlab;

                OrderTax.ModifiedAt = DateTime.Now;
                OrderTax.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                response = helper.ApiCall(URL, EndPoints.OrderTaxInfo, "PUT", OrderTax);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }


        [HttpDelete]
        public ActionResult<ApiHelper> Delete(int? id = 0)
        {
            var temp = helper.ApiCall(URL, EndPoints.OrderTaxInfo + "?Id=" + id, "GET");
            baseResponse = baseResponse.JsonParseList(temp);
            List<OrderTaxInfo> templist = (List<OrderTaxInfo>)baseResponse.Data;
            if (templist.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.OrderTaxInfo + "?Id=" + id, "DELETE");
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.OrderTaxInfo + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET");
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("ById")]
        public ActionResult<ApiHelper> ById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.OrderTaxInfo + "?Id=" + id, "GET");
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("Search")]
        public ActionResult<ApiHelper> Search(int? orderId = 0, int? orderItemId = 0, int? productId = 0, int? sellerProductId = 0)
        {
            var response = helper.ApiCall(URL, EndPoints.OrderTaxInfo + "?OrderID=" + orderId + "&OrderItemID=" + orderItemId + "&ProductID=" + productId + "&sellerProductID=" + sellerProductId, "GET");
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
