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
    public class OrderInvoiceController : ControllerBase
    {
        private readonly IOrderInvoiceService _orderInvoiceService;

        public OrderInvoiceController(IOrderInvoiceService orderInvoiceService)
        {
            _orderInvoiceService = orderInvoiceService;
        }


        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderInvoice orderInvoice)
        {
            orderInvoice.CreatedAt = DateTime.Now;
            var data = await _orderInvoiceService.Create(orderInvoice);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderInvoice orderInvoice)
        {
            orderInvoice.ModifiedAt = DateTime.Now;
            var data = await _orderInvoiceService.Update(orderInvoice);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderInvoice orderInvoice = new OrderInvoice();
            orderInvoice.Id = Id;
            orderInvoice.DeletedBy = DeletedBy;
            orderInvoice.DeletedAt = DateTime.Now;
            var data = await _orderInvoiceService.Delete(orderInvoice);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<OrderInvoice>>> Get(int Id = 0, int PackageID = 0, int OrderID = 0, string? OrderItemIDs = null, string? SellerId = null, string? InvoiceNo = null, string? Status = null, string? searchText = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            OrderInvoice orderInvoice = new OrderInvoice();
            orderInvoice.Id = Id;

            orderInvoice.PackageID = PackageID;
            orderInvoice.SellerId = SellerId;
            orderInvoice.OrderID = OrderID;
            orderInvoice.OrderItemIDs = OrderItemIDs;
            orderInvoice.InvoiceNo = InvoiceNo;
            orderInvoice.Status = Status;
            orderInvoice.IsDeleted = Isdeleted;
            orderInvoice.SearchText = searchText;
            var data = await _orderInvoiceService.Get(orderInvoice, PageIndex, PageSize, Mode);
            return data;
        }





    }
}
