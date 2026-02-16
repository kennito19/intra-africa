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
    
    public class ManageConfigKeyController : ControllerBase
    {
        private readonly IManageConfigKeyService _configKeyService;

        public ManageConfigKeyController(IManageConfigKeyService configKeyServices)
        {
            _configKeyService = configKeyServices;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageConfigKeyLibrary configKey)
        {
            configKey.CreatedAt = DateTime.Now;

            var data = await _configKeyService.Create(configKey);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageConfigKeyLibrary configKey)
        {
            configKey.ModifiedAt = DateTime.Now;
            var data = await _configKeyService.Update(configKey);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            ManageConfigKeyLibrary configKey = new ManageConfigKeyLibrary();
            configKey.Id = Id;
            var data = await _configKeyService.Delete(configKey);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageConfigKeyLibrary>>> get(int Id = 0, string? Name = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ManageConfigKeyLibrary configKey = new ManageConfigKeyLibrary();
            configKey.Id = Id;
            configKey.Name = Name;
            configKey.Searchtext = Searchtext;

            var data = await _configKeyService.get(configKey, PageIndex, PageSize, Mode);
            return data;
        }

    }
}
