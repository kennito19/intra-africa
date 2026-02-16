using Catalogue.Application.IServices;
using Catalogue.Application.Services;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    
    public class ManageSubMenuController : ControllerBase
	{
		private readonly IManageSubMenuService _manageSubMenuService;

		public ManageSubMenuController(IManageSubMenuService manageSubMenuService)
        {
			_manageSubMenuService = manageSubMenuService;
		}

		[HttpPost]
		[Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageSubMenu model)
		{
			model.CreatedAt = DateTime.Now;

			var data = await _manageSubMenuService.Create(model);
			return data;
		}

		[HttpPut]
		[Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageSubMenu model)
		{
			model.ModifiedAt = DateTime.Now;
			var data = await _manageSubMenuService.Update(model);
			return data;
		}

		[HttpDelete]
		[Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int id)
		{
			ManageSubMenu model = new ManageSubMenu();
			model.Id = id;
			var data = await _manageSubMenuService.Delete(model);
			return data;
		}

		[HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageSubMenu>>> Get(int? id = null, string? name = null,string? menuType = null, string? Searchtext = null, int? parentId = null, int? headerId = null, int? CategoryId = null, string? HeaderName = null, string? ParentName = null, bool getParent = false, bool getChild = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
		{
			ManageSubMenu manageSub = new ManageSubMenu();
			if (id != null)
			{
				manageSub.Id = Convert.ToInt32(id);
			}
			manageSub.MenuType = menuType;
			manageSub.ParentId = parentId;
			manageSub.HeaderId = Convert.ToInt32(headerId);
			manageSub.CategoryId = Convert.ToInt32(CategoryId);
			manageSub.Name = name;
			manageSub.Searchtext = Searchtext;
			var data = await _manageSubMenuService.get(manageSub, PageIndex, PageSize, Mode,HeaderName,ParentName, getParent, getChild);
			return data;
		}
	}
}
