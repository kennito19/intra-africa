using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Gateway.Common.orders
{
    public class UpdateOrder
    {
        private readonly IConfiguration _configuration;
        public string OrderURL = string.Empty; 
        private readonly HttpContext _httpContext;
        public string UserId = string.Empty;
        private ApiHelper helper;

        BaseResponse<OrderDetails> orderDetailBaseRes = new BaseResponse<OrderDetails>();
        
        public UpdateOrder(IConfiguration configuration, HttpContext httpContext, string Userid)
        {
            UserId = Userid;
            _httpContext = httpContext;
            _configuration = configuration;
            OrderURL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            helper = new ApiHelper(_httpContext);
        }

        public Orders BindOrders(OrderDetails model)
        {
            BaseResponse<Orders> baseResponse = new BaseResponse<Orders>();
            var GetResponse = helper.ApiCall(OrderURL, EndPoints.Orders + "?Id=" + model.OrderId, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(GetResponse);


            Orders order = (Orders)baseResponse.Data;

            order.OrderNo = model.OrderNo;
            order.PaymentMode = model.PaymentMode;
            order.Status = model.Status;
            order.CreatedBy = UserId;
            order.CreatedAt = DateTime.Now;

            return order;
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public BaseResponse<OrderItems> UpdateOrderItemStatus(string orderStatus, int OrderId)
        {
            BaseResponse<OrderItems> orderItemBaseRes = new BaseResponse<OrderItems>();

            var temp = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + OrderId, "GET", null);
            orderItemBaseRes = orderItemBaseRes.JsonParseList(temp);
            List<OrderItems> tempList = (List<OrderItems>)orderItemBaseRes.Data;

            if (tempList.Where(x => x.Id != OrderId).Any())
            {
                orderItemBaseRes = orderItemBaseRes.AlreadyExists();
            }
            else
            {
                var OrderItem = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?OrderId=" + OrderId, "GET", null);
                orderItemBaseRes = orderItemBaseRes.JsonParseRecord(OrderItem);
                OrderItems orderItem = (OrderItems)orderItemBaseRes.Data;
                orderItem.OrderID = OrderId;
                orderItem.Status = orderStatus;
                orderItem.ModifiedAt = DateTime.Now;
                orderItem.ModifiedBy = UserId;
                var response = helper.ApiCall(OrderURL, EndPoints.OrderItems, "PUT", orderItem);
                orderItemBaseRes = orderItemBaseRes.JsonParseInputResponse(response);

                UpdateOrderStatus(orderStatus, OrderId);

                //OrderTrackEntries(OrderId);
            }
            return orderItemBaseRes;
        }

        [NonAction]
        public BaseResponse<Orders> UpdateOrderStatus(string orderStatus, int OrderId)
        {
            BaseResponse<Orders> orderBaseRes = new BaseResponse<Orders>();

            var temp = helper.ApiCall(OrderURL, EndPoints.Orders + "?Id=" + OrderId, "GET", null);
            orderBaseRes = orderBaseRes.JsonParseList(temp);
            List<Orders> tempList = (List<Orders>)orderBaseRes.Data;

            if (tempList.Where(x => x.Id != OrderId).Any())
            {
                orderBaseRes = orderBaseRes.AlreadyExists();
            }
            else
            {
                var OrderItem = helper.ApiCall(OrderURL, EndPoints.Orders + "?Id=" + OrderId, "GET", null);
                orderBaseRes = orderBaseRes.JsonParseRecord(OrderItem);
                Orders orders = (Orders)orderBaseRes.Data;
                orders.Id = OrderId;
                orders.Status = orderStatus;
                orders.ModifiedAt = DateTime.Now;
                orders.ModifiedBy = UserId;
                var response = helper.ApiCall(OrderURL, EndPoints.Orders, "PUT", orders);
                orderBaseRes = orderBaseRes.JsonParseInputResponse(response);
            }
            return orderBaseRes;
        }

        //[NonAction]
        //public BaseResponse<OrderTrackDetails> OrderTrackEntries(int orderId, int orderItemId)
        //{
        //    BaseResponse<OrderTrackDetails> bases = new BaseResponse<OrderTrackDetails>();
        //    OrderTrackDetails ois = BindOrderTrackDetails(orderId, orderItemId);

        //    BaseResponse<OrderTrackDetails> baseResponse = new BaseResponse<OrderTrackDetails>();

        //    OrderTrackDetails orderTrackDetails = new OrderTrackDetails();
        //    orderTrackDetails.OrderID = ois.OrderID;
        //    orderTrackDetails.OrderItemID = ois.OrderItemID;
        //    orderTrackDetails.OrderStage = "Placed";
        //    orderTrackDetails.OrderStatus = "Order has been placed";
        //    orderTrackDetails.OrderTrackDetail = "Payment approved";
        //    orderTrackDetails.TrackDate = DateTime.Now;
        //    orderTrackDetails.CreatedBy = UserId;
        //    orderTrackDetails.CreatedAt = DateTime.Now;

        //    var response = helper.ApiCall(OrderURL, EndPoints.OrderTrackDetails, "POST", orderTrackDetails);
        //    baseResponse = baseResponse.JsonParseInputResponse(response);
        //    return baseResponse;
        //}

    }
}
