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
    public class OrderTrackDetailsController : ControllerBase
    {
        private readonly IOrderTrackDetailsService _orderTrackDetailsService;

        public OrderTrackDetailsController(IOrderTrackDetailsService orderTrackDetailsService)
        {
            _orderTrackDetailsService = orderTrackDetailsService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderTrackDetails orderTrackDetails)
        {
            orderTrackDetails.CreatedAt = DateTime.Now;
            var data = await _orderTrackDetailsService.Create(orderTrackDetails);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderTrackDetails orderTrackDetails)
        {
            orderTrackDetails.ModifiedAt = DateTime.Now;
            var data = await _orderTrackDetailsService.Update(orderTrackDetails);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderTrackDetails orderTrackDetails = new OrderTrackDetails();
            orderTrackDetails.Id = Id;
            orderTrackDetails.DeletedBy = DeletedBy;
            orderTrackDetails.DeletedAt = DateTime.Now;
            var data = await _orderTrackDetailsService.Delete(orderTrackDetails);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<OrderTrackDetails>>> Get(int Id = 0, int OrderID = 0, int OrderItemID = 0, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            OrderTrackDetails orderTrackDetails = new OrderTrackDetails();
            orderTrackDetails.Id = Id;
            orderTrackDetails.OrderID = OrderID;
            orderTrackDetails.OrderItemID = OrderItemID;

            orderTrackDetails.IsDeleted = Isdeleted;
            var data = await _orderTrackDetailsService.Get(orderTrackDetails, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
