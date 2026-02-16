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
	
	public class ManageHeaderMenuController : ControllerBase
	{
		private readonly IManageHeaderMenuService _manageHeaderMenuService;

		public ManageHeaderMenuController(IManageHeaderMenuService manageHeaderMenuService)
        {
			_manageHeaderMenuService = manageHeaderMenuService;
		}

		[HttpPost]
		[Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageHeaderMenu model)
		{
			model.CreatedAt = DateTime.Now;

			var data = await _manageHeaderMenuService.Create(model);
			return data;
		}

		[HttpPut]
		[Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageHeaderMenu model)
		{
			model.ModifiedAt = DateTime.Now;
			var data = await _manageHeaderMenuService.Update(model);
			return data;
		}

		[HttpDelete]
		[Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int id)
		{
			ManageHeaderMenu model = new ManageHeaderMenu();
			model.Id = id;
			var data = await _manageHeaderMenuService.Delete(model);
			return data;
		}

		[HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageHeaderMenu>>> Get(int? id = null, string? name = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
		{
			ManageHeaderMenu manageHeader = new ManageHeaderMenu();
			if (id != null)
			{
				manageHeader.Id = Convert.ToInt32(id);
			}
			manageHeader.Name = name;
			manageHeader.Searchtext = Searchtext;
            var data = await _manageHeaderMenuService.get(manageHeader, PageIndex, PageSize, Mode);
			return data;
		}
	}
}
