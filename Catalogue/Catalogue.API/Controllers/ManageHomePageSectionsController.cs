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
    
    public class ManageHomePageSectionsController : ControllerBase
    {
        private readonly IManageHomePageSectionsService _homePageSectionService;

        public ManageHomePageSectionsController(IManageHomePageSectionsService homePageSectionServices)
        {
            _homePageSectionService = homePageSectionServices;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageHomePageSectionsLibrary homePageSection)
        {
            homePageSection.CreatedAt = DateTime.Now;

            var data = await _homePageSectionService.Create(homePageSection);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageHomePageSectionsLibrary homePageSection)
        {
            homePageSection.ModifiedAt = DateTime.Now;
            var data = await _homePageSectionService.Update(homePageSection);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            ManageHomePageSectionsLibrary homePageSection = new ManageHomePageSectionsLibrary();
            homePageSection.Id = Id;
            var data = await _homePageSectionService.Delete(homePageSection);
            return data;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<BaseResponse<List<ManageHomePageSectionsLibrary>>> get(int Id = 0, int? LayoutId = 0, int? LayoutTypeId = 0, string? Name = null, string? LayoutName = null, string? LayoutTypeName = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Status = null, string? Searchtext = null)
        {
            ManageHomePageSectionsLibrary layout = new ManageHomePageSectionsLibrary();
            layout.Id = Id;
            layout.LayoutId = (int)LayoutId;
            layout.LayoutTypeId = (int)LayoutTypeId;
            layout.Name = Name;
            layout.LayoutTypeName = LayoutTypeName;
            layout.LayoutName = LayoutName;
            layout.Status = Status;
            layout.SearchText = Searchtext;

            var data = await _homePageSectionService.get(layout, PageIndex, PageSize, Mode);
            return data;
        }


        [HttpGet("getProductHomePageSection")]
        [AllowAnonymous]
        public async Task<BaseResponse<List<ProductHomePageSectionLibrary>>> getProductHomePageSection(int categoryId = 0, int? topProduct = 0, string? productId = null, string? Mode = "get")
        {
            ProductHomePageSectionLibrary productSection = new ProductHomePageSectionLibrary();
            productSection.categoryId = categoryId;
            productSection.topProduct = topProduct;
            productSection.productIds = productId;

            var data = await _homePageSectionService.getProudctHomePageSection(productSection, Mode);
            return data;
        }


    }
}
