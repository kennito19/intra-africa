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
    
    public class ManageStaticPagesController : ControllerBase
    {
        private readonly IManageStaticPagesService _staticPagesService;

        public ManageStaticPagesController(IManageStaticPagesService staticPages)
        {
            _staticPagesService = staticPages;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageStaticPagesLibrary staticPages)
        {
            staticPages.CreatedAt = DateTime.Now;

            var data = await _staticPagesService.Create(staticPages);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageStaticPagesLibrary staticPages)
        {
            staticPages.ModifiedAt = DateTime.Now;
            var data = await _staticPagesService.Update(staticPages);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            ManageStaticPagesLibrary staticPages = new ManageStaticPagesLibrary();
            staticPages.Id = Id;
            var data = await _staticPagesService.Delete(staticPages);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageStaticPagesLibrary>>> get(int Id = 0, string? Name = null, string? Status = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ManageStaticPagesLibrary staticPages = new ManageStaticPagesLibrary();
            staticPages.Id = Id;
            staticPages.Name = Name;
            staticPages.Status = Status;
            staticPages.Searchtext = Searchtext;

            var data = await _staticPagesService.get(staticPages, PageIndex, PageSize, Mode);
            return data;
        }


    }
}
