using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.IServices;
using Order.Application.Services;
using Order.Domain;
using Order.Domain.Entity;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Accessable")]
    public class OrderShipmentInfoController : ControllerBase
    {
        private readonly IOrderShipmentInfoService _orderShipmentInfoService;

        public OrderShipmentInfoController(IOrderShipmentInfoService orderShipmentInfoService)
        {
            _orderShipmentInfoService = orderShipmentInfoService;
        }


        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderShipmentInfo orderShipmentInfo)
        {
            orderShipmentInfo.CreatedAt = DateTime.Now;
            var data = await _orderShipmentInfoService.Create(orderShipmentInfo);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderShipmentInfo orderShipmentInfo)
        {
            orderShipmentInfo.ModifiedAt = DateTime.Now;
            var data = await _orderShipmentInfoService.Update(orderShipmentInfo);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderShipmentInfo orderShipmentInfo = new OrderShipmentInfo ();
            orderShipmentInfo.Id = Id;
            orderShipmentInfo.DeletedBy = DeletedBy;
            orderShipmentInfo.DeletedAt = DateTime.Now;
            var data = await _orderShipmentInfoService.Delete(orderShipmentInfo);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<OrderShipmentInfo>>> Get(int Id = 0, string? SellerID = null, int OrderID = 0, string? OrderItemIDs = null, int PackageID = 0, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            OrderShipmentInfo orderShipmentInfo = new OrderShipmentInfo();
            orderShipmentInfo.Id = Id;

            orderShipmentInfo.OrderID = OrderID;
            orderShipmentInfo.SellerID = SellerID;
            orderShipmentInfo.OrderItemIDs = OrderItemIDs;
            orderShipmentInfo.PackageID = PackageID;

            orderShipmentInfo.IsDeleted = Isdeleted;
            var data = await _orderShipmentInfoService.Get(orderShipmentInfo, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
