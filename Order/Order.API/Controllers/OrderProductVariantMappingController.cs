using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.IRepositories;
using Order.Domain.Entity;
using Order.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Accessable")]
    public class OrderProductVariantMappingController : ControllerBase
    {
        private readonly IOrderProductVariantMappingRepository _orderProductVariantMappingRepository;

        public OrderProductVariantMappingController(IOrderProductVariantMappingRepository orderProductVariantMappingRepository)
        {
            _orderProductVariantMappingRepository = orderProductVariantMappingRepository;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderProductVariantMapping orderProductVariantMapping)
        {
            orderProductVariantMapping.CreatedAt = DateTime.Now;
            var data = await _orderProductVariantMappingRepository.Create(orderProductVariantMapping);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderProductVariantMapping orderProductVariantMapping)
        {
            orderProductVariantMapping.ModifiedAt = DateTime.Now;
            var data = await _orderProductVariantMappingRepository.Update(orderProductVariantMapping);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderProductVariantMapping orderProductVariantMapping = new OrderProductVariantMapping();
            orderProductVariantMapping.Id = Id;
            orderProductVariantMapping.DeletedBy = DeletedBy;
            orderProductVariantMapping.DeletedAt = DateTime.Now;
            var data = await _orderProductVariantMappingRepository.Delete(orderProductVariantMapping);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<OrderProductVariantMapping>>> Get(int Id = 0, int OrderID = 0, int OrderItemID = 0, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            OrderProductVariantMapping orderProductVariantMapping = new OrderProductVariantMapping();
            orderProductVariantMapping.Id = Id;

            orderProductVariantMapping.OrderID = OrderID;
            orderProductVariantMapping.OrderItemID = OrderItemID;
            orderProductVariantMapping.IsDeleted = Isdeleted;
 
             var data = await _orderProductVariantMappingRepository.Get(orderProductVariantMapping, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
