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
    public class AppConfigController : ControllerBase
    {
        private readonly IManageAppConfigService _configService;

        public AppConfigController(IManageAppConfigService configService)
        {
            _configService = configService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageAppConfig config)
        {
            config.CreatedAt = DateTime.Now;

            var data = await _configService.Create(config);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageAppConfig config)
        {
            config.ModifiedAt = DateTime.Now;
            var data = await _configService.Update(config);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            ManageAppConfig config = new ManageAppConfig();
            config.Id = Id;
            var data = await _configService.Delete(config);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageAppConfig>>> get(int id = 0, string? name = null,string? status = null, int pageIndex = 1, int pageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ManageAppConfig config = new ManageAppConfig();
            config.Id = id;
            config.Name = name;
            config.Status = status;
            config.Searchtext = Searchtext;

            var data = await _configService.get(config, pageIndex, pageSize, Mode);
            return data;
        }
    }
}
