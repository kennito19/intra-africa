using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Catalogue.Domain.DTO;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ManageHomePageDetailsController : ControllerBase
    {
        private readonly IManageHomePageDetailsService _homePageDetailsService;

        public ManageHomePageDetailsController(IManageHomePageDetailsService homePageDetailServices)
        {
            _homePageDetailsService = homePageDetailServices;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageHomePageDetailsLibrary homePageDetails)
        {
            homePageDetails.CreatedAt = DateTime.Now;

            var data = await _homePageDetailsService.Create(homePageDetails);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageHomePageDetailsLibrary homePageDetails)
        {
            homePageDetails.ModifiedAt = DateTime.Now;
            var data = await _homePageDetailsService.Update(homePageDetails);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int? Id = 0 , int? SectionId = 0)
        {
            ManageHomePageDetailsLibrary homePageSection = new ManageHomePageDetailsLibrary();
            homePageSection.Id = Id;
            homePageSection.SectionId = SectionId;
            var data = await _homePageDetailsService.Delete(homePageSection);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageHomePageDetailsLibrary>>> get(int Id = 0, int? SectionId = 0, int? LayoutTypeDetailsId = 0, int? OptionId = 0, string? SectionName = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Status = null, string? Searchtext = null)
        {
            ManageHomePageDetailsLibrary layout = new ManageHomePageDetailsLibrary();
            layout.Id = Id;
            layout.SectionId = (int)SectionId;
            layout.LayoutTypeDetailsId = (int)LayoutTypeDetailsId;
            layout.OptionId = (int)OptionId;
            layout.SectionName = SectionName;
            layout.Status = Status;
            layout.SearchText = Searchtext;

            var data = await _homePageDetailsService.get(layout, PageIndex, PageSize, Mode);
            return data;
        }

        [HttpGet("getFrontHomepageDetails")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<FrontHomepageDetailsDto>>> GetFrontHomepageDetails(string? Status=null)
        {
            var data = await _homePageDetailsService.GetFrontHomepageDetails("homePage", Status);
            return data;
        }

    }
}
