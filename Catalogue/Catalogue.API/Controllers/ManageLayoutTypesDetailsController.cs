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
    
    public class ManageLayoutTypesDetailsController : ControllerBase
    {
        private readonly IManageLayoutTypesDetailsService _layoutTypeDetailsService;

        public ManageLayoutTypesDetailsController(IManageLayoutTypesDetailsService layoutTypeDetailsService)
        {
            _layoutTypeDetailsService = layoutTypeDetailsService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageLayoutTypesDetails typesDetails)
        {
            typesDetails.CreatedAt = DateTime.Now;

            var data = await _layoutTypeDetailsService.Create(typesDetails);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageLayoutTypesDetails typesDetails)
        {
            typesDetails.ModifiedAt = DateTime.Now;
            var data = await _layoutTypeDetailsService.Update(typesDetails);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            ManageLayoutTypesDetails typesDetails = new ManageLayoutTypesDetails();
            typesDetails.Id = Id;
            var data = await _layoutTypeDetailsService.Delete(typesDetails);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageLayoutTypesDetails>>> get(int Id = 0, int? LayoutId = 0, int? LayoutTypeId = 0, string? Name = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ManageLayoutTypesDetails typesDetails = new ManageLayoutTypesDetails();
            typesDetails.Id = Id;
            typesDetails.LayoutId = (int)LayoutId;
            typesDetails.LayoutTypeId = (int)LayoutTypeId;
            typesDetails.Name = Name;
            typesDetails.Searchtext = Searchtext;

            var data = await _layoutTypeDetailsService.get(typesDetails, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
