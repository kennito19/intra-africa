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
    
    public class LendingPageSectionsController : ControllerBase
    {
        private readonly IManageLendingPageSectionsService _lendingPageSectionService;

        public LendingPageSectionsController(IManageLendingPageSectionsService lendingPageSection)
        {
            _lendingPageSectionService = lendingPageSection;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(LendingPageSectionsLibrary lendingPageSection)
        {
            lendingPageSection.CreatedAt = DateTime.Now;

            var data = await _lendingPageSectionService.Create(lendingPageSection);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(LendingPageSectionsLibrary lendingPageSection)
        {
            lendingPageSection.ModifiedAt = DateTime.Now;
            var data = await _lendingPageSectionService.Update(lendingPageSection);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            LendingPageSectionsLibrary lendingPageSection = new LendingPageSectionsLibrary();
            lendingPageSection.Id = Id;
            var data = await _lendingPageSectionService.Delete(lendingPageSection);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<LendingPageSectionsLibrary>>> get(int Id = 0, int LendingPageId = 0, int LayoutId = 0, int LayoutTypeId = 0, string? Name = null, string? LendingPageName = null, string? LayoutTypeName = null, string? LayoutName = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Status = null, string? Searchtext = null)
        {
            LendingPageSectionsLibrary lendingPageSection = new LendingPageSectionsLibrary();
            lendingPageSection.Id = Id;
            lendingPageSection.LendingPageId = LendingPageId;
            lendingPageSection.LayoutId = LayoutId;
            lendingPageSection.LayoutTypeId = LayoutTypeId;
            lendingPageSection.Name = Name;
            lendingPageSection.LendingPageName = LendingPageName;
            lendingPageSection.LayoutTypeName = LayoutTypeName;
            lendingPageSection.LayoutName = LayoutName;
            lendingPageSection.Status = Status;

            lendingPageSection.SearchText = Searchtext;

            var data = await _lendingPageSectionService.get(lendingPageSection, PageIndex, PageSize, Mode);
            return data;
        }


    }
}
