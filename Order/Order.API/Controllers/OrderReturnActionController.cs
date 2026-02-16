using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.IRepositories;
using Order.Domain;
using Order.Domain.Entity;
using Order.Infrastructure.Repository;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Accessable")]
    public class OrderReturnActionController : ControllerBase
    {
        private readonly IOrderReturnActionRepository _orderReturnActionRepository;

        public OrderReturnActionController(IOrderReturnActionRepository orderReturnActionRepository)
        {
            _orderReturnActionRepository = orderReturnActionRepository;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(OrderReturnAction orderReturnAction)
        {
            orderReturnAction.CreatedAt = DateTime.Now;
            var data = await _orderReturnActionRepository.Create(orderReturnAction);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(OrderReturnAction orderReturnAction)
        {
            orderReturnAction.ModifiedAt = DateTime.Now;
            var data = await _orderReturnActionRepository.Update(orderReturnAction);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            OrderReturnAction orderReturnAction = new OrderReturnAction();
            orderReturnAction.Id = Id;
            orderReturnAction.DeletedBy = DeletedBy;
            orderReturnAction.DeletedAt = DateTime.Now;
            var data = await _orderReturnActionRepository.Delete(orderReturnAction);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<OrderReturnAction>>> Get(int Id = 0,  string? ReturnAction = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            OrderReturnAction orderReturnAction = new OrderReturnAction();
            orderReturnAction.Id = Id;
            orderReturnAction.ReturnAction = ReturnAction;

            orderReturnAction.IsDeleted = Isdeleted;
            var data = await _orderReturnActionRepository.Get(orderReturnAction, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
