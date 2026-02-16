using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.IRepositories;
using Order.Domain.Entity;
using Order.Domain;
using Order.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Accessable")]
    public class OrderRefundController : ControllerBase
    {
        private readonly IOrderRefundRepository _orderRefundRepository;

        public OrderRefundController(IOrderRefundRepository orderRefundRepository)
        {
            _orderRefundRepository = orderRefundRepository;
        }


        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderRefund orderRefund)
        {
            orderRefund.CreatedAt = DateTime.Now;
            var data = await _orderRefundRepository.Create(orderRefund);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderRefund orderRefund)
        {
            orderRefund.ModifiedAt = DateTime.Now;
            var data = await _orderRefundRepository.Update(orderRefund);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderRefund orderRefund = new OrderRefund();
            orderRefund.Id = Id;
            orderRefund.DeletedBy = DeletedBy;
            orderRefund.DeletedAt = DateTime.Now;
            var data = await _orderRefundRepository.Delete(orderRefund);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<OrderRefund>>> Get(int Id = 0, int OrderCancelReturnID = 0, int OrderID = 0, int OrderItemID = 0, string? TransactionID = null, string? Status = null,string? searchText = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            OrderRefund orderRefund = new OrderRefund();
            orderRefund.Id = Id;

            orderRefund.OrderCancelReturnID = OrderCancelReturnID;
            orderRefund.OrderID = OrderID;
            orderRefund.OrderItemID = OrderItemID;
            orderRefund.TransactionID = TransactionID;
            orderRefund.Status = Status;
            orderRefund.searchText = searchText;

            orderRefund.IsDeleted = Isdeleted;

            var data = await _orderRefundRepository.Get(orderRefund, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
