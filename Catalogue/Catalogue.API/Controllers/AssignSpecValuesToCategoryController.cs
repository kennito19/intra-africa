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
    [Authorize(Policy = "Accessable")]
    public class AssignSpecValuesToCategoryController : ControllerBase
    {
        private readonly IAssignSpecValuesToCategoryService _assignSpecValuesService;

        public AssignSpecValuesToCategoryController(IAssignSpecValuesToCategoryService assignSpecValuesService)
        {
            _assignSpecValuesService = assignSpecValuesService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(AssignSpecValuesToCategory assignSpec)
        {
            assignSpec.CreatedAt = DateTime.Now;
            assignSpec.IsAllowSpecInVariant = false;
            var data = await _assignSpecValuesService.Create(assignSpec);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(AssignSpecValuesToCategory assignSpec)
        {
            assignSpec.ModifiedAt = DateTime.Now;
            assignSpec.IsAllowSpecInVariant = false;
            var data = await _assignSpecValuesService.Update(assignSpec);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int AssignSpecID, int SpecID, int SpecTypeID)
        {
            AssignSpecValuesToCategory assignSpec = new AssignSpecValuesToCategory();
            assignSpec.AssignSpecID = AssignSpecID;
            assignSpec.SpecID = SpecID;
            assignSpec.SpecTypeID = SpecTypeID;
            var data = await _assignSpecValuesService.Delete(assignSpec);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<AssignSpecValuesToCategory>>> get(int Id = 0, int? AssignSpecID = 0, int? SpecID = 0, int? SpecTypeID = 0, int? SpecTypeValueID = 0, int? CategoryId = 0, bool? IsAllowSpecInFilter = null, bool? IsDeleted = null, string? SpecificationName = null, string? SpecificationTypeName = null, string? SpecificationTypeValueName = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            AssignSpecValuesToCategory assignSpec = new AssignSpecValuesToCategory();
            assignSpec.Id = Id;
            assignSpec.AssignSpecID = AssignSpecID;
            assignSpec.SpecID = SpecID;
            assignSpec.SpecTypeID = SpecTypeID;
            assignSpec.SpecTypeValueID = SpecTypeValueID;
            assignSpec.SpecificationName = SpecificationName;
            assignSpec.SpecificationTypeName = SpecificationTypeName;
            assignSpec.SpecificationTypeValueName = SpecificationTypeValueName;
            assignSpec.CategoryId = CategoryId;
            if (IsAllowSpecInFilter != null)
            {
                assignSpec.IsAllowSpecInFilter = IsAllowSpecInFilter;
            }
            if (IsDeleted != null)
            {
                assignSpec.IsDeleted = IsDeleted;
            }

            var data = await _assignSpecValuesService.get(assignSpec, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
