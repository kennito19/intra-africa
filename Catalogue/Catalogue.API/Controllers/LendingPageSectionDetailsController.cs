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
    
    public class LendingPageSectionDetailsController : ControllerBase
    {
        private readonly IManageLendingPageSectionDetailsService _lendingPageSectionDetailService;

        public LendingPageSectionDetailsController(IManageLendingPageSectionDetailsService lendingPageSectionDetail)
        {
            _lendingPageSectionDetailService = lendingPageSectionDetail;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(LendingPageSectionDetailsLibrary lendingPageSectionDetail)
        {
            lendingPageSectionDetail.CreatedAt = DateTime.Now;

            var data = await _lendingPageSectionDetailService.Create(lendingPageSectionDetail);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(LendingPageSectionDetailsLibrary lendingPageSectionDetail)
        {
            lendingPageSectionDetail.ModifiedAt = DateTime.Now;
            var data = await _lendingPageSectionDetailService.Update(lendingPageSectionDetail);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int? Id = 0, int? SectionId = 0)
        {
            LendingPageSectionDetailsLibrary lendingPageSectionDetail = new LendingPageSectionDetailsLibrary();
            lendingPageSectionDetail.Id = Id;
            lendingPageSectionDetail.LendingPageSectionId = SectionId;
            var data = await _lendingPageSectionDetailService.Delete(lendingPageSectionDetail);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<LendingPageSectionDetailsLibrary>>> get(int Id = 0, int LendingPageSectionId = 0, int? LayoutTypeDetailsId = 0, int? OptionId = 0, string? LendingPageSectionName = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Status = null, string? Searchtext = null)
        {
            LendingPageSectionDetailsLibrary lendingPageSectionDetail = new LendingPageSectionDetailsLibrary();
            lendingPageSectionDetail.Id = Id;
            lendingPageSectionDetail.LendingPageSectionId = LendingPageSectionId;
            lendingPageSectionDetail.OptionId = OptionId;
            lendingPageSectionDetail.LendingPageSectionName = LendingPageSectionName;
            lendingPageSectionDetail.Status = Status;
            lendingPageSectionDetail.SearchText = Searchtext;

            var data = await _lendingPageSectionDetailService.get(lendingPageSectionDetail, PageIndex, PageSize, Mode);
            return data;
        }

    }
}
