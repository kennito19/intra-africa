using Catalogue.Application.IServices;
using Catalogue.Application.Services;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckAssignSpecsToCategoryController : ControllerBase
    {
        private readonly ICheckAssignSpecsToCategoryService _checkAssignSpecsToCategoryService;

        public CheckAssignSpecsToCategoryController(ICheckAssignSpecsToCategoryService checkAssignSpecsToCategoryService)
        {
            _checkAssignSpecsToCategoryService = checkAssignSpecsToCategoryService;
        }

        [HttpGet("checkAssignSpecToCat")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSpecToCat(int assignSpecId, bool? multiSeller = false)
        {
            var data = await _checkAssignSpecsToCategoryService.checkAssignSpecToCat(assignSpecId, multiSeller);
            return data;
        }

        [HttpGet("checkAssignSizeValuesToCat")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSizeValuesToCat(int assignSpecId, int sizeTypeId, bool? multiSeller = false)
        {
            var data = await _checkAssignSpecsToCategoryService.checkAssignSizeValuesToCat(assignSpecId, sizeTypeId, multiSeller);
            return data;
        }

        [HttpGet("checkAssignSpecvaluesToCat")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSpecvaluesToCat(int assignSpecId, int specTypeId, bool? multiSeller = false)
        {
            var data = await _checkAssignSpecsToCategoryService.checkAssignSpecvaluesToCat(assignSpecId, specTypeId, multiSeller);
            return data;
        }
        [HttpGet("checkSizeType")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkSizeType(int assignSpecId, int sizeTypeId)
        {
            var data = await _checkAssignSpecsToCategoryService.checkSizeType(assignSpecId, sizeTypeId);
            return data;
        }

        [HttpGet("checkSpecType")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkSpecType(int assignSpecId, int specTypeId)
        {
            var data = await _checkAssignSpecsToCategoryService.checkSpecType(assignSpecId, specTypeId);
            return data;
        }

    }
}
