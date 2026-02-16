using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.IServices;
using Order.Application.Services;
using Order.Domain.Entity;
using Order.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Accessable")]
    public class RejectionTypeController : ControllerBase
    {
        private readonly IRejectionTypeService _rejectionTypeService;

        public RejectionTypeController(IRejectionTypeService rejectionTypeService)
        {
            _rejectionTypeService = rejectionTypeService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(RejectionType rejectionType)
        {
            rejectionType.CreatedAt = DateTime.Now;
            var data = await _rejectionTypeService.Create(rejectionType);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(RejectionType rejectionType)
        {
            rejectionType.ModifiedAt = DateTime.Now;
            var data = await _rejectionTypeService.Update(rejectionType);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            RejectionType rejectionType = new RejectionType();
            rejectionType.Id = Id;
            rejectionType.DeletedBy = DeletedBy;
            rejectionType.DeletedAt = DateTime.Now;
            var data = await _rejectionTypeService.Delete(rejectionType);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<RejectionType>>> Get(int Id = 0, string? Type = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            RejectionType rejectionType = new RejectionType();
            rejectionType.Id = Id;
            rejectionType.Type = Type;
            rejectionType.IsDeleted = Isdeleted;
            var data = await _rejectionTypeService.Get(rejectionType, PageIndex, PageSize, Mode);
            return data;
        }

    }
}
