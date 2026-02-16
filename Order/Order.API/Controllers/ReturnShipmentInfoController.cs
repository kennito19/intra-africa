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
    public class ReturnShipmentInfoController : ControllerBase
    {
        private readonly IReturnShipmentInfoService _returnShipmentInfoService;

        public ReturnShipmentInfoController(IReturnShipmentInfoService returnShipmentInfoService)
        {
            _returnShipmentInfoService = returnShipmentInfoService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(ReturnShipmentInfo returnShipmentInfo)
        {
            returnShipmentInfo.CreatedAt = DateTime.Now;
            var data = await _returnShipmentInfoService.Create(returnShipmentInfo);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(ReturnShipmentInfo returnShipmentInfo)
        {
            returnShipmentInfo.ModifiedAt = DateTime.Now;
            var data = await _returnShipmentInfoService.Update(returnShipmentInfo);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            ReturnShipmentInfo returnShipmentInfo = new ReturnShipmentInfo();
            returnShipmentInfo.Id = Id;
            returnShipmentInfo.DeletedBy = DeletedBy;
            returnShipmentInfo.DeletedAt = DateTime.Now;
            var data = await _returnShipmentInfoService.Delete(returnShipmentInfo);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<ReturnShipmentInfo>>> Get(int Id = 0, int OrderID = 0, int OrderItemID  = 0, int OrderCancelReturnID = 0, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            ReturnShipmentInfo returnShipmentInfo = new ReturnShipmentInfo();
            returnShipmentInfo.Id = Id;
            returnShipmentInfo.OrderID = OrderID;
            returnShipmentInfo.OrderItemID = OrderItemID;
            returnShipmentInfo.OrderCancelReturnID = OrderCancelReturnID;
            
            returnShipmentInfo.IsDeleted = Isdeleted;
            var data = await _returnShipmentInfoService.Get(returnShipmentInfo, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
