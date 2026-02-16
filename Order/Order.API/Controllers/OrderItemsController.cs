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
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemsService _orderItemsService;

        public OrderItemsController(IOrderItemsService orderItemsService)
        {
            _orderItemsService = orderItemsService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderItems orderItems)
        {
            orderItems.CreatedAt = DateTime.Now;
            var data = await _orderItemsService.Create(orderItems);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderItems orderItems)
        {
            orderItems.ModifiedAt = DateTime.Now;
            var data = await _orderItemsService.Update(orderItems);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderItems orderItems = new OrderItems();
            orderItems.Id = Id;
            orderItems.DeletedBy = DeletedBy;
            orderItems.DeletedAt = DateTime.Now;
            var data = await _orderItemsService.Delete(orderItems);
            return data;
        }

        [HttpGet]
        [Route("")]
        public async Task<BaseResponse<List<OrderItems>>> Get(int Id, int Orderid = 0, string? Guid = null, string? ProductGuid = null, string? SellerId = null, string? suborderno = null, bool Isdeleted = false, int? SellerProductID = 0, int? ProductId = 0, int? CategoryId = 0, string? Status = null, bool? notInStatus = null, string? fromDate = null, string? toDate = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            OrderItems orderItems = new OrderItems();
            orderItems.Id = Id;
            orderItems.OrderID = Orderid;
            orderItems.SellerID = SellerId;
            orderItems.Guid = Guid;
            orderItems.ProductGUID = ProductGuid;
            orderItems.SubOrderNo = suborderno;
            orderItems.IsDeleted = Isdeleted;
            orderItems.SellerProductID = SellerProductID != 0 ? Convert.ToInt32(SellerProductID) : 0;
            orderItems.CategoryId = CategoryId != 0 ? Convert.ToInt32(CategoryId) : 0;
            orderItems.ProductID = ProductId != 0 ? Convert.ToInt32(ProductId) : 0;
            orderItems.Status = Status;
            if (!string.IsNullOrEmpty(fromDate))
            {
                orderItems.FromDate = fromDate;
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                orderItems.ToDate = toDate;
            }
            if (notInStatus != null)
            {
                orderItems.NotInStatus = notInStatus;
            }

            var data = await _orderItemsService.Get(orderItems, PageIndex, PageSize, Mode);
            return data;
        }


        [HttpGet]
        [Route("GetOrderDetails")]
        public async Task<BaseResponse<List<OrderItemDetails>>> GetOrderDetails(int? OrderId = 0, int? OrderItemId = 0, string? SellerID = null, string? Guid = null, string? ProductGUID = null, string? OrderNo = null, string? SubOrderNo = null, int? ProductID = 0, string? ItemStatus = null, string? OrderStatus = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            OrderItemDetails orderItemDetails = new OrderItemDetails();

            orderItemDetails.OrderId = OrderId == 0 ? 0 : Convert.ToInt32(OrderId);
            orderItemDetails.OrderItemId = OrderItemId == 0 ? 0 : Convert.ToInt32(OrderItemId);
            orderItemDetails.SellerID = SellerID;
            orderItemDetails.Guid = Guid;
            orderItemDetails.ProductGUID = ProductGUID;
            orderItemDetails.OrderNo = OrderNo;
            orderItemDetails.SubOrderNo = SubOrderNo;
            orderItemDetails.ProductID = ProductID == 0 ? 0 : Convert.ToInt32(ProductID);
            orderItemDetails.ItemStatus = ItemStatus;
            orderItemDetails.OrderStatus = OrderStatus;

            var data = await _orderItemsService.GetOrderDetails(orderItemDetails, PageIndex, PageSize, Mode);
            return data;
        }

    }
}
