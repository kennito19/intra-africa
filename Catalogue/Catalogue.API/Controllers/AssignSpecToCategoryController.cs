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
    
    public class AssignSpecToCategoryController : ControllerBase
    {
        private readonly IAssignSpecToCategoryService _assignSpecToCategoryService;

        public AssignSpecToCategoryController(IAssignSpecToCategoryService assignSpecToCategoryService)
        {
            _assignSpecToCategoryService = assignSpecToCategoryService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(AssignSpecToCategory assignSpecToCategory)
        {
            assignSpecToCategory.CreatedAt = DateTime.Now;
            assignSpecToCategory.IsAllowPriceVariant = assignSpecToCategory.IsAllowPriceVariant;
            assignSpecToCategory.IsAllowSpecifications = assignSpecToCategory.IsAllowSpecifications;
            assignSpecToCategory.IsAllowExpiryDates = false;
            assignSpecToCategory.IsAllowColorsInVariant = false;
            var data = await _assignSpecToCategoryService.Create(assignSpecToCategory);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(AssignSpecToCategory assignSpecToCategory)
        {
            assignSpecToCategory.ModifiedAt = DateTime.Now;
            assignSpecToCategory.IsAllowPriceVariant = assignSpecToCategory.IsAllowPriceVariant;
            assignSpecToCategory.IsAllowSpecifications = assignSpecToCategory.IsAllowSpecifications;
            assignSpecToCategory.IsAllowExpiryDates = false;
            assignSpecToCategory.IsAllowColorsInVariant = false;
            var data = await _assignSpecToCategoryService.Update(assignSpecToCategory);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            AssignSpecToCategory assignSpecToCategory = new AssignSpecToCategory();
            assignSpecToCategory.Id = Id;
            assignSpecToCategory.DeletedBy = DeletedBy;
            assignSpecToCategory.DeletedAt = DateTime.Now;
            var data = await _assignSpecToCategoryService.Delete(assignSpecToCategory);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<AssignSpecToCategory>>> get(int Id = 0, int? CategoryID = 0, string? Guid = null, string? categoryName = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            AssignSpecToCategory assignSpecToCategory = new AssignSpecToCategory();
            assignSpecToCategory.Id = Id;
            assignSpecToCategory.CategoryID = CategoryID;
            assignSpecToCategory.Guid = Guid;
            assignSpecToCategory.CategoryName = categoryName;
            assignSpecToCategory.IsDeleted = Isdeleted;
            assignSpecToCategory.Searchtext = Searchtext;

            var data = await _assignSpecToCategoryService.get(assignSpecToCategory, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
