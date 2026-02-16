using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.IServices;
using Order.Domain;
using Order.Domain.Entity;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Accessable")]
    public class OrderStatusLibraryController : ControllerBase
    {
        private readonly IOrderStatusLibraryService _orderStatusLibraryService;

        public OrderStatusLibraryController(IOrderStatusLibraryService orderStatusLibraryService)
        {
            _orderStatusLibraryService = orderStatusLibraryService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderStatusLibrary orderStatusLibrary)
        {
            orderStatusLibrary.CreatedAt = DateTime.Now;
            var data = await _orderStatusLibraryService.Create(orderStatusLibrary);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderStatusLibrary orderStatusLibrary)
        {
            orderStatusLibrary.ModifiedAt = DateTime.Now;
            var data = await _orderStatusLibraryService.Update(orderStatusLibrary);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderStatusLibrary orderStatusLibrary = new OrderStatusLibrary();
            orderStatusLibrary.Id = Id;
            orderStatusLibrary.DeletedBy = DeletedBy;
            orderStatusLibrary.DeletedAt = DateTime.Now;
            var data = await _orderStatusLibraryService.Delete(orderStatusLibrary);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<OrderStatusLibrary>>> Get(int Id = 0, string? OrderStatus = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            OrderStatusLibrary orderStatusLibrary = new OrderStatusLibrary();
            orderStatusLibrary.Id = Id;
            orderStatusLibrary.OrderStatus = OrderStatus;
            orderStatusLibrary.IsDeleted = Isdeleted;
            var data = await _orderStatusLibraryService.Get(orderStatusLibrary, PageIndex, PageSize, Mode);
            return data;
        }

    }
}
