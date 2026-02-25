using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(CategoryLibrary category)
        {
            category.CreatedAt = DateTime.Now;
            var data = await _categoryService.Create(category);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(CategoryLibrary category)
        {
            category.ModifiedAt = DateTime.Now;
            var data = await _categoryService.Update(category);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            CategoryLibrary category = new CategoryLibrary();
            category.Id = Id;
            category.DeletedBy = DeletedBy;
            category.DeletedAt = DateTime.Now;
            var data = await _categoryService.Delete(category);
            return data;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<BaseResponse<List<CategoryLibrary>>> get(int Id = 0, int? ParentID = 0, string? ParentName = null, string? Name = null, string? PathIds = null, string? Status = null,string? Guid = null, bool Isdeleted = false , bool Getparent =false, bool Getchild = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            CategoryLibrary category = new CategoryLibrary();
            category.Id = Id;
            category.ParentId = ParentID;
            category.Name = Name;
            category.ParentName = ParentName;
            category.PathIds = PathIds;
            category.Status = Status;
            category.Guid = Guid;
            category.IsDeleted = Isdeleted;
            category.Searchtext = Searchtext;
            var data = await _categoryService.get(category, Getparent,Getchild, PageIndex, PageSize, Mode);
            return data;
        }

        [HttpGet("GetCategoryWithParent")]
        [AllowAnonymous]
        public async Task<BaseResponse<List<CategoryLibrary>>> GetCategoryWithParent(int Categoryid = 0)
        {
            
            var data = await _categoryService.GetCategoryWithParent(Categoryid);
            return data;
        }
    }
}
