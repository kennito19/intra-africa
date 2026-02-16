using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.IServices;
using Order.Application.Services;
using Order.Domain.Entity;
using Order.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Accessable")]
    public class OrderTaxInfoController : ControllerBase
    {
        private readonly IOrderTaxInfoService _orderTaxInfoService;

        public OrderTaxInfoController(IOrderTaxInfoService orderTaxInfoService)
        {
            _orderTaxInfoService = orderTaxInfoService;
        }


        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderTaxInfo orderTaxInfo)
        {
            orderTaxInfo.CreatedAt = DateTime.Now;
            orderTaxInfo.ShippingZone = null;
            orderTaxInfo.TaxOnShipping = 0;
            orderTaxInfo.CommissionIn = orderTaxInfo.CommissionIn;
            orderTaxInfo.CommissionRate = orderTaxInfo.CommissionRate;
            orderTaxInfo.CommissionAmount = orderTaxInfo.CommissionAmount;
            orderTaxInfo.TaxOnCommission = orderTaxInfo.TaxOnCommission;
            orderTaxInfo.ShippingCharge = orderTaxInfo.ShippingCharge;
            orderTaxInfo.ShippingZone = orderTaxInfo.ShippingZone;
            orderTaxInfo.TaxOnShipping = orderTaxInfo.TaxOnShipping;
            var data = await _orderTaxInfoService.Create(orderTaxInfo);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderTaxInfo orderTaxInfo)
        {
            orderTaxInfo.ModifiedAt = DateTime.Now;
            orderTaxInfo.ShippingCharge = 0;
            orderTaxInfo.ShippingZone = null;
            orderTaxInfo.TaxOnShipping = 0;
            orderTaxInfo.CommissionIn = orderTaxInfo.CommissionIn;
            orderTaxInfo.CommissionRate = orderTaxInfo.CommissionRate;
            orderTaxInfo.CommissionAmount = orderTaxInfo.CommissionAmount;
            orderTaxInfo.TaxOnCommission = orderTaxInfo.TaxOnCommission;
            orderTaxInfo.ShippingCharge = orderTaxInfo.ShippingCharge;
            orderTaxInfo.ShippingZone = orderTaxInfo.ShippingZone;
            orderTaxInfo.TaxOnShipping = orderTaxInfo.TaxOnShipping;
            var data = await _orderTaxInfoService.Update(orderTaxInfo);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderTaxInfo orderTaxInfo = new OrderTaxInfo();
            orderTaxInfo.Id = Id;
            orderTaxInfo.DeletedBy = DeletedBy;
            orderTaxInfo.DeletedAt = DateTime.Now;
            var data = await _orderTaxInfoService.Delete(orderTaxInfo);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<OrderTaxInfo>>> Get(int Id = 0, int OrderID = 0,int OrderItemID = 0,int ProductID = 0,int SellerProductID = 0, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            OrderTaxInfo orderTaxInfo = new OrderTaxInfo();
            orderTaxInfo.Id = Id;

            orderTaxInfo.OrderID = OrderID;
            orderTaxInfo.OrderItemID = OrderItemID;
            orderTaxInfo.ProductID = ProductID;
            orderTaxInfo.SellerProductID = SellerProductID;



            orderTaxInfo.IsDeleted = Isdeleted;
            var data = await _orderTaxInfoService.Get(orderTaxInfo, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
