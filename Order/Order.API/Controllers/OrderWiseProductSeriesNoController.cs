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
    public class OrderWiseProductSeriesNoController : ControllerBase
    {
        private readonly IOrderWiseProductSeriesNoService _orderWiseProductSeriesNoService;

        public OrderWiseProductSeriesNoController(IOrderWiseProductSeriesNoService orderWiseProductSeriesNoService)
        {
            _orderWiseProductSeriesNoService = orderWiseProductSeriesNoService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderWiseProductSeriesNo orderWiseProductSeriesNo)
        {
            orderWiseProductSeriesNo.CreatedAt = DateTime.Now;
            var data = await _orderWiseProductSeriesNoService.Create(orderWiseProductSeriesNo);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderWiseProductSeriesNo orderWiseProductSeriesNo)
        {
            var data = await _orderWiseProductSeriesNoService.Update(orderWiseProductSeriesNo);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int OrderID, int OrderItemID)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderWiseProductSeriesNo orderWiseProductSeriesNo = new OrderWiseProductSeriesNo();
            orderWiseProductSeriesNo.OrderID = OrderID;
            orderWiseProductSeriesNo.OrderItemID = OrderItemID;
            var data = await _orderWiseProductSeriesNoService.Delete(orderWiseProductSeriesNo);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<OrderWiseProductSeriesNo>>> Get(int Id = 0, int OrderID = 0,int OrderItemID = 0, int? ProductId = 0, int? CategoryId = 0, string? seriesNo = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            OrderWiseProductSeriesNo orderWiseProductSeriesNo = new OrderWiseProductSeriesNo();
            orderWiseProductSeriesNo.Id = Id;
            orderWiseProductSeriesNo.OrderID = OrderID;
            orderWiseProductSeriesNo.OrderItemID = OrderItemID;
            orderWiseProductSeriesNo.SeriesNo = seriesNo;
            orderWiseProductSeriesNo.CategoryId = CategoryId != 0 ? Convert.ToInt32(CategoryId) : 0;
            orderWiseProductSeriesNo.ProductID = ProductId != 0 ? Convert.ToInt32(ProductId) : 0;
            var data = await _orderWiseProductSeriesNoService.Get(orderWiseProductSeriesNo, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
