using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Application.Services;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Catalogue.Infrastructure.Repository;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageHomePageController : ControllerBase
    {
        private readonly IManageHomePageService _manageHomePageService;

        public ManageHomePageController(IManageHomePageService manageHomePageService)
        {
            _manageHomePageService = manageHomePageService;
        }

        [HttpPost]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageHomePage manageHomePage)
        {
            var data = await _manageHomePageService.Create(manageHomePage);
            return data;
        }

        [HttpPut]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageHomePage manageHomePage)
        {
            var data = await _manageHomePageService.Update(manageHomePage);
            return data;
        }

        [HttpDelete]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
           
            ManageHomePage manageHomePage = new ManageHomePage();
            manageHomePage.Id = Id;
            var data = await _manageHomePageService.Delete(manageHomePage);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<List<ManageHomePage>>> get(int PageIndex, int PageSize, int? Id = null, string? Name = null, string? Status = null,  string Mode="get")
        {
            ManageHomePage mhp = new ManageHomePage();
            mhp.Id = Id;
            mhp.Name = Name;    
            mhp.Status = Status;

            var data =await _manageHomePageService.get(mhp, PageIndex, PageSize, Mode);
            return data;
        }

    }
}
