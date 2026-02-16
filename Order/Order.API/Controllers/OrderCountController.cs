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
    public class OrderCountController : ControllerBase
    {
        private readonly IOrderCountService _orderCountService;

        public OrderCountController(IOrderCountService orderCountService)
        {
            _orderCountService = orderCountService;
        }

        [HttpGet]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<List<OrdersCount>>> Get(string? sellerId, string? userId, string? days)
        {
            var data = await _orderCountService.get(sellerId, userId, days);
            return data;
        }
    }
}
