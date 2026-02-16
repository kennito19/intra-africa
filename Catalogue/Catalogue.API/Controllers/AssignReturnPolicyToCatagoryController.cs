using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AssignReturnPolicyToCatagoryController : ControllerBase
    {
        private readonly IAssignReturnPolicyToCatagoryService _assignReturnPolicyToCatagoryService;

        public AssignReturnPolicyToCatagoryController(IAssignReturnPolicyToCatagoryService assignReturnPolicyToCatagoryService)
        {
            _assignReturnPolicyToCatagoryService = assignReturnPolicyToCatagoryService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory)
        {
            assignReturnPolicyToCatagory.CreatedAt = DateTime.Now;
            var data = await _assignReturnPolicyToCatagoryService.Create(assignReturnPolicyToCatagory);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory)
        {
            assignReturnPolicyToCatagory.ModifiedAt = DateTime.Now;
            var data = await _assignReturnPolicyToCatagoryService.Update(assignReturnPolicyToCatagory);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

            AssignReturnPolicyToCatagory assignReturnPolicyToCatagory = new AssignReturnPolicyToCatagory();
            assignReturnPolicyToCatagory.Id = Id;
            assignReturnPolicyToCatagory.DeletedBy = DeletedBy;
            assignReturnPolicyToCatagory.DeletedAt = DateTime.Now;
            var data = await _assignReturnPolicyToCatagoryService.Delete(assignReturnPolicyToCatagory);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<AssignReturnPolicyToCatagory>>> get(int Id = 0, int? CategoryID = 0, int? ReturnPolicyDetailID = 0, int? ReturnPolicyID = 0, string? ReturnPolicy = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? searchtext = null)
        {
            AssignReturnPolicyToCatagory assignReturnPolicyToCatagory = new AssignReturnPolicyToCatagory();
            assignReturnPolicyToCatagory.Id = Id;
            assignReturnPolicyToCatagory.CategoryID = CategoryID;
            assignReturnPolicyToCatagory.ReturnPolicyDetailID = ReturnPolicyDetailID;
            assignReturnPolicyToCatagory.ReturnPolicyID = ReturnPolicyID;
            assignReturnPolicyToCatagory.ReturnPolicy = ReturnPolicy;
            assignReturnPolicyToCatagory.Searchtext = searchtext;
            assignReturnPolicyToCatagory.IsDeleted = Isdeleted;

            var data = await _assignReturnPolicyToCatagoryService.get(assignReturnPolicyToCatagory, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
