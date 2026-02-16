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
    
    public class LendingPageController : ControllerBase
    {
        private readonly IManageLendingPageService _lendingPageService;

        public LendingPageController(IManageLendingPageService lendingPageServices)
        {
            _lendingPageService = lendingPageServices;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(LendingPageLibrary lendingPage)
        {
            lendingPage.CreatedAt = DateTime.Now;

            var data = await _lendingPageService.Create(lendingPage);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(LendingPageLibrary lendingPage)
        {
            lendingPage.ModifiedAt = DateTime.Now;
            var data = await _lendingPageService.Update(lendingPage);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            LendingPageLibrary lendingPage = new LendingPageLibrary();
            lendingPage.Id = Id;
            var data = await _lendingPageService.Delete(lendingPage);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<LendingPageLibrary>>> get(int Id = 0, string? Name = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            LendingPageLibrary lendingPage = new LendingPageLibrary();
            lendingPage.Id = Id;
            lendingPage.Name = Name;
            lendingPage.SearchText = Searchtext;

            var data = await _lendingPageService.get(lendingPage, PageIndex, PageSize, Mode);
            return data;
        }

    }
}
