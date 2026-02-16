using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ManageLayoutTypesController : ControllerBase
    {
        private readonly IManageLayoutTypesService _layoutTypeService;

        public ManageLayoutTypesController(IManageLayoutTypesService layouttypeServices)
        {
            _layoutTypeService = layouttypeServices;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageLayoutTypesLibrary layouttype)
        {
            layouttype.CreatedAt = DateTime.Now;

            var data = await _layoutTypeService.Create(layouttype);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageLayoutTypesLibrary layouttype)
        {
            layouttype.ModifiedAt = DateTime.Now;
            var data = await _layoutTypeService.Update(layouttype);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            ManageLayoutTypesLibrary layouttype = new ManageLayoutTypesLibrary();
            layouttype.Id = Id;
            var data = await _layoutTypeService.Delete(layouttype);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageLayoutTypesLibrary>>> get(int Id = 0, int? LayoutId = 0, string? Name = null, string? LayoutName = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ManageLayoutTypesLibrary layout = new ManageLayoutTypesLibrary();
            layout.Id = Id;
            layout.LayoutId = (int)LayoutId;
            layout.Name = Name;
            layout.LayoutName = LayoutName;
            layout.Searchtext = Searchtext;

            var data = await _layoutTypeService.get(layout, PageIndex, PageSize, Mode);
            return data;
        }


    }
}
