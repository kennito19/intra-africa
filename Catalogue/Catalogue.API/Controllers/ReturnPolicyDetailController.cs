using Catalogue.Application.IServices;
using Catalogue.Application.Services;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ReturnPolicyDetailController : ControllerBase
    {
        private readonly IReturnPolicyDetailService _returnPolicyDetailService;

        public ReturnPolicyDetailController(IReturnPolicyDetailService returnPolicyDetailService)
        {
            _returnPolicyDetailService = returnPolicyDetailService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> AddReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail)
        {
            returnPolicyDetail.CreatedAt = DateTime.Now;
            var data = await _returnPolicyDetailService.AddReturnPolicyDetail(returnPolicyDetail);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> UpdateReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail)
        {
            returnPolicyDetail.ModifiedAt = DateTime.Now;
            var data = await _returnPolicyDetailService.UpdateReturnPolicyDetail(returnPolicyDetail);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> DeleteReturnPolicyDetail(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            ReturnPolicyDetail returnPolicyDetail = new ReturnPolicyDetail();
            returnPolicyDetail.Id = Id;
            returnPolicyDetail.DeletedBy = DeletedBy;
            returnPolicyDetail.DeletedAt = DateTime.Now;
            var data = await _returnPolicyDetailService.DeleteReturnPolicyDetail(returnPolicyDetail);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ReturnPolicyDetail>>> getReturnPolicyDetail(int? id = null, string? days = null, int? returnpolicyid = null, string? name = null, string? title = null, bool? isDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ReturnPolicyDetail returnPolicyDetail = new ReturnPolicyDetail();
            if (id != null)
            {
                returnPolicyDetail.Id = Convert.ToInt32(id);
            }
            if (returnpolicyid != null)
            {
                returnPolicyDetail.ReturnPolicyID = Convert.ToInt32(returnpolicyid);
            }
            returnPolicyDetail.ReturnPolicy = name;
            returnPolicyDetail.Title = title;
            if (days != null)
            {
                returnPolicyDetail.days = days;
            }
            returnPolicyDetail.IsDeleted = Convert.ToBoolean(isDeleted);
            returnPolicyDetail.Searchtext = Searchtext;
            var data = await _returnPolicyDetailService.GetReturnPolicyDetail(returnPolicyDetail, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
