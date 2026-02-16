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
    
    public class AssignSizeValueToCategoryController : ControllerBase
    {
        private readonly IAssignSizeValueToCategoryService _assignSizeValueService;

        public AssignSizeValueToCategoryController(IAssignSizeValueToCategoryService assignSizeValueService)
        {
            _assignSizeValueService = assignSizeValueService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(AssignSizeValueToCategory assignSize)
        {
            assignSize.CreatedAt = DateTime.Now;
            assignSize.IsAllowSizeInVariant = false;
            var data = await _assignSizeValueService.Create(assignSize);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(AssignSizeValueToCategory assignSize)
        {
            assignSize.ModifiedAt = DateTime.Now;
            assignSize.IsAllowSizeInVariant = false;
            var data = await _assignSizeValueService.Update(assignSize);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int AssignSpecID, int SizeTypeID)
        {
            AssignSizeValueToCategory assignSize = new AssignSizeValueToCategory();
            assignSize.AssignSpecID = AssignSpecID;
            assignSize.SizeTypeID = SizeTypeID;
            var data = await _assignSizeValueService.Delete(assignSize);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<AssignSizeValueToCategory>>> get(int Id = 0, int? AssignSpecID = 0, int? CategoryId = 0, bool? IsAllowSizeInFilter= null, bool? IsDeleted = null, int? SizeTypeID = 0, int? SizeId = 0, string? SizeTypeName = null, string? SizeName = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            AssignSizeValueToCategory assignSize = new AssignSizeValueToCategory();
            assignSize.Id = Id;
            assignSize.AssignSpecID = AssignSpecID;
            assignSize.SizeId = SizeId;
            assignSize.SizeTypeID = SizeTypeID;
            assignSize.SizeName = SizeName;
            assignSize.SizeTypeName = SizeTypeName;
            assignSize.CategoryId = CategoryId;
            if (IsAllowSizeInFilter != null) {
                assignSize.IsAllowSizeInFilter = IsAllowSizeInFilter;
            }
            if (IsDeleted != null)
            {
                assignSize.IsDeleted = IsDeleted;
            }
            assignSize.Searchtext = Searchtext;

            var data = await _assignSizeValueService.get(assignSize, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
