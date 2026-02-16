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
    public class ManageLayoutOptionController : ControllerBase
    {
        private readonly IManageLayoutOptionService _layoutOptionService;

        public ManageLayoutOptionController(IManageLayoutOptionService layoutOptionService)
        {
            _layoutOptionService = layoutOptionService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageLayoutOption layoutOption)
        {
            layoutOption.CreatedAt = DateTime.Now;

            var data = await _layoutOptionService.Create(layoutOption);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageLayoutOption layoutOption)
        {
            layoutOption.ModifiedAt = DateTime.Now;
            var data = await _layoutOptionService.Update(layoutOption);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            ManageLayoutOption layoutOption = new ManageLayoutOption();
            layoutOption.Id = Id;
            var data = await _layoutOptionService.Delete(layoutOption);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageLayoutOption>>> get(int Id = 0, string? Name = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ManageLayoutOption layoutOption = new ManageLayoutOption();
            layoutOption.Id = Id;
            layoutOption.Name = Name;
            layoutOption.Searchtext = Searchtext;

            var data = await _layoutOptionService.get(layoutOption, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
