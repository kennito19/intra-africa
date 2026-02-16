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
    
    public class ReturnPolicyController : ControllerBase
    {
        private readonly IReturnPolicyService _returnPolicyService;

        public ReturnPolicyController(IReturnPolicyService returnPolicyService)
        {
            _returnPolicyService = returnPolicyService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> AddReturnPolicy(ReturnPolicy returnPolicy)
        {
            returnPolicy.CreatedAt = DateTime.Now;
            var data = await _returnPolicyService.AddReturnPolicy(returnPolicy);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> UpdateReturnPolicy(ReturnPolicy returnPolicy)
        {
            returnPolicy.ModifiedAt = DateTime.Now;
            var data = await _returnPolicyService.UpdateReturnPolicy(returnPolicy);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> DeleteReturnPolicy(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            ReturnPolicy returnPolicy = new ReturnPolicy();
            returnPolicy.Id = Id;
            returnPolicy.DeletedBy = DeletedBy;
            returnPolicy.DeletedAt = DateTime.Now;
            var data = await _returnPolicyService.DeleteReturnPolicy(returnPolicy);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ReturnPolicy>>> getReturnPolicy(int? id = null, string? name = null, bool? isDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
         {
            ReturnPolicy returnPolicy = new ReturnPolicy();
            if (id != null)
            {
                returnPolicy.Id = Convert.ToInt32(id);
            }
            returnPolicy.Name = name;
            returnPolicy.IsDeleted = Convert.ToBoolean(isDeleted);
            returnPolicy.Searchtext = Searchtext;
            var data = await _returnPolicyService.GetReturnPolicy(returnPolicy, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
