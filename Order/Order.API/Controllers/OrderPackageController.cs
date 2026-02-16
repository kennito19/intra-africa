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
    public class OrderPackageController : ControllerBase
    {
        private readonly IOrderPackageService _orderPackageService;

        public OrderPackageController(IOrderPackageService orderPackageService)
        {
            _orderPackageService = orderPackageService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderPackage orderPackage)
        {
            orderPackage.CreatedAt = DateTime.Now;
            var data = await _orderPackageService.Create(orderPackage);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderPackage orderPackage)
        {
            orderPackage.ModifiedAt = DateTime.Now;
            var data = await _orderPackageService.Update(orderPackage);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderPackage orderPackage = new OrderPackage();
            orderPackage.Id = Id;
            orderPackage.DeletedBy = DeletedBy;
            orderPackage.DeletedAt = DateTime.Now;
            var data = await _orderPackageService.Delete(orderPackage);
            return data;
        }

        [HttpGet]
        [Route("")]
        public async Task<BaseResponse<List<OrderPackage>>> Get(int Id, int Orderid = 0, string? OrderItemIDs = null,string? PackageNo = null,bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            OrderPackage orderPackage = new OrderPackage();
            orderPackage.Id = Id;
            orderPackage.OrderID = Orderid;
            orderPackage.OrderItemIDs = OrderItemIDs;
            orderPackage.PackageNo = PackageNo;
          
            orderPackage.IsDeleted = Isdeleted;
            var data = await _orderPackageService.Get(orderPackage, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
