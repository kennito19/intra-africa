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
    public class OrderCancelReturnController : ControllerBase
    {
        private readonly IOrderCancelReturnService _orderCancelReturnService;

        public OrderCancelReturnController(IOrderCancelReturnService orderCancelReturnService)
        {
            _orderCancelReturnService = orderCancelReturnService;
        }



        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderCancelReturn orderCancelReturn)
        {
            orderCancelReturn.CreatedAt = DateTime.Now;
            var data = await _orderCancelReturnService.Create(orderCancelReturn);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderCancelReturn orderCancelReturn)
        {
            orderCancelReturn.ModifiedAt = DateTime.Now;
            var data = await _orderCancelReturnService.Update(orderCancelReturn);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderCancelReturn orderCancelReturn = new OrderCancelReturn();
            orderCancelReturn.Id = Id;
            orderCancelReturn.DeletedBy = DeletedBy;
            orderCancelReturn.DeletedAt = DateTime.Now;
            var data = await _orderCancelReturnService.Delete(orderCancelReturn);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<OrderCancelReturn>>> Get(int Id = 0, int OrderID = 0, int BrandId = 0, int OrderItemID = 0, string? OrderNo = null, string? SellerId = null,string? NewOrderNo = null,int ActionID = 0,string? UserId = null,string? Status = null,string? RefundStatus = null, bool Isdeleted = false,bool withCancel = false, int PageIndex = 1, int PageSize = 10, string? searchText = null, string ? Mode = "get")
        {
            OrderCancelReturn orderCancelReturn = new OrderCancelReturn();
            orderCancelReturn.Id = Id;
            orderCancelReturn.OrderID=OrderID;
            orderCancelReturn.OrderItemID=OrderItemID;
            orderCancelReturn.NewOrderNo=NewOrderNo;
            orderCancelReturn.ActionID=ActionID;
            orderCancelReturn.UserId=UserId;
            orderCancelReturn.Status=Status;
            orderCancelReturn.RefundStatus = RefundStatus;
            orderCancelReturn.BrandID = BrandId;
            orderCancelReturn.OrderNo = OrderNo;
            orderCancelReturn.SellerID = SellerId;
            orderCancelReturn.searchText = searchText;
            orderCancelReturn.WithCancel = withCancel;

            orderCancelReturn.IsDeleted = Isdeleted;
            var data = await _orderCancelReturnService.Get(orderCancelReturn, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
