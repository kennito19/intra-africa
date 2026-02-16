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
    public class OrderWiseExtraChargesController : ControllerBase
    {
        private readonly IOrderWiseExtraChargesService _orderWiseExtraChargesService;

        public OrderWiseExtraChargesController(IOrderWiseExtraChargesService orderWiseExtraChargesService)
        {
            _orderWiseExtraChargesService = orderWiseExtraChargesService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderWiseExtraCharges orderWiseExtraCharges)
        {
            orderWiseExtraCharges.CreatedAt = DateTime.Now;
            var data = await _orderWiseExtraChargesService.Create(orderWiseExtraCharges);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderWiseExtraCharges orderWiseExtraCharges)
        {
            orderWiseExtraCharges.ModifiedAt = DateTime.Now;
            var data = await _orderWiseExtraChargesService.Update(orderWiseExtraCharges);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderWiseExtraCharges orderWiseExtraCharges = new OrderWiseExtraCharges();
            orderWiseExtraCharges.Id = Id;
            orderWiseExtraCharges.DeletedBy = DeletedBy;
            orderWiseExtraCharges.DeletedAt = DateTime.Now;
            var data = await _orderWiseExtraChargesService.Delete(orderWiseExtraCharges);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<OrderWiseExtraCharges>>> Get(int Id = 0, int OrderID = 0, int OrderItemID = 0, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get",string? searchText=null)
        {
            OrderWiseExtraCharges orderWiseExtraCharges = new OrderWiseExtraCharges();

            orderWiseExtraCharges.Id = Id;

            orderWiseExtraCharges.OrderID = OrderID;
            orderWiseExtraCharges.OrderItemID = OrderItemID;
            orderWiseExtraCharges.searchText = searchText;
            
            orderWiseExtraCharges.IsDeleted = Isdeleted;
            var data = await _orderWiseExtraChargesService.Get(orderWiseExtraCharges, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
