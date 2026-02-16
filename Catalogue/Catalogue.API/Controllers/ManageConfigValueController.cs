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
    
    public class ManageConfigValueController : ControllerBase
    {
        private readonly IManageConfigValueService _configValueService;

        public ManageConfigValueController(IManageConfigValueService configValueServices)
        {
            _configValueService = configValueServices;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageConfigValueLibrary configValue)
        {
            configValue.CreatedAt = DateTime.Now;

            var data = await _configValueService.Create(configValue);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageConfigValueLibrary configValue)
        {
            configValue.ModifiedAt = DateTime.Now;
            var data = await _configValueService.Update(configValue);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            ManageConfigValueLibrary configValue = new ManageConfigValueLibrary();
            configValue.Id = Id;
            var data = await _configValueService.Delete(configValue);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageConfigValueLibrary>>> get(int Id = 0, int KeyId = 0, string? keyName = null, string? Value = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ManageConfigValueLibrary configValue = new ManageConfigValueLibrary();
            configValue.Id = Id;
            configValue.KeyId = KeyId;
            configValue.KeyName = keyName;
            configValue.SearchText = Searchtext;
            configValue.Value = Value;

            var data = await _configValueService.get(configValue, PageIndex, PageSize, Mode);
            return data;
        }

    }
}
