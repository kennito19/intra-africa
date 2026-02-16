using Catalogue.Application.IServices;
using Catalogue.Application.Services;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ManageLayoutsController : ControllerBase
    {
        private readonly IManageLayoutsService _layoutService;

        public ManageLayoutsController(IManageLayoutsService layoutServices)
        {
            _layoutService = layoutServices;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageLayoutsLibrary layout)
        {
            layout.CreatedAt = DateTime.Now;

            var data = await _layoutService.Create(layout);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageLayoutsLibrary layout)
        {
            layout.ModifiedAt = DateTime.Now;
            var data = await _layoutService.Update(layout);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            ManageLayoutsLibrary layout = new ManageLayoutsLibrary();
            layout.Id = Id;
            var data = await _layoutService.Delete(layout);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageLayoutsLibrary>>> get(int Id = 0, string? Name = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ManageLayoutsLibrary layout = new ManageLayoutsLibrary();
            layout.Id = Id;
            layout.Name = Name;
            layout.Searchtext = Searchtext;

            var data = await _layoutService.get(layout, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
