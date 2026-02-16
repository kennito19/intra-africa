using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.IServices;
using Order.Application.Services;
using Order.Domain.Entity;
using Order.Domain;
using Microsoft.AspNetCore.Authorization;
using Order.Domain.DTO;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Accessable")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(Orders orders)
        {
            orders.CreatedAt = DateTime.Now;
            orders.OrderDate = DateTime.Now;
            var data = await _orderService.Create(orders);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(Orders orders)
        {
            orders.ModifiedAt = DateTime.Now;
            var data = await _orderService.Update(orders);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            Orders orders = new Orders();
            orders.Id = Id;
            orders.DeletedBy = DeletedBy;
            orders.DeletedAt = DateTime.Now;
            var data = await _orderService.Delete(orders);
            return data;
        }

        [HttpGet]

        public async Task<BaseResponse<List<Orders>>> Get(int? Id = 0, string? GUID = null, string? Coupon = null, string? OrderNo = null,string? OrderRefNo = null, string? UserId = null, string? SellerID = null, string? status = null, string? searchtext = null, bool? notInStatus = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? fromDate = null, string? toDate = null)
        {
            var Value = status;
            //string[] data1 = status != null ? status.Split(",") : null;
            //if (data1 != null && data1.Length > 0)
            //{
            //    foreach (var item in data1)
            //    {
            //        //Value = "'"+ item.ToString() + "','";
            //        Value = "'" + status + "',";
            //    }
            //    Value = Value.Trim(',');
            //}

            Orders orders = new Orders();
            if (Id != null && Id != 0)
            {
                orders.Id = Convert.ToInt32(Id);
            }

            orders.Guid = GUID;
            orders.Coupon = Coupon;
            orders.OrderNo = OrderNo;
            orders.UserId = UserId;
            orders.SellerID = SellerID;
            orders.Status = Value;
            orders.SearchText = searchtext;
            orders.NotInStatus = notInStatus;
            orders.OrderReferenceNo = OrderRefNo;
            orders.IsDeleted = Isdeleted;

            if (!string.IsNullOrEmpty(fromDate))
            {
                orders.FromDate = fromDate;
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                orders.ToDate = toDate;
            }

            var data = await _orderService.Get(orders, PageIndex, PageSize, Mode);
            return data;
        }

        [HttpGet("Invoice")]
        public async Task<BaseResponse<List<InvoiceDto>>> GetInvoice(string? Packageid, string? OrderNo)
        {
            var data = await _orderService.GetInvoice(Packageid, OrderNo);
            return data;
        }
    }
}
