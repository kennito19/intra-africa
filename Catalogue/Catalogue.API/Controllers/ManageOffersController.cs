using Catalogue.Application.IServices;
using Catalogue.Application.Services;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ManageOffersController : ControllerBase
	{
		private readonly IManageOffersService _manageOffers;

		public ManageOffersController(IManageOffersService manageOffers)
        {
			_manageOffers = manageOffers;
		}

		[HttpPost]
		[Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageOffersLibrary model)
		{
			model.CreatedAt = DateTime.Now;

			var data = await _manageOffers.Create(model);
			return data;
		}

		[HttpPut]
		[Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageOffersLibrary model)
		{
			model.ModifiedAt = DateTime.Now;
			var data = await _manageOffers.Update(model);
			return data;
		}

		[HttpDelete]
		[Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
		{
			string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
			ManageOffersLibrary model = new ManageOffersLibrary();
			model.id = Id;
			model.DeletedAt = DateTime.Now;
			model.DeletedBy = DeletedBy;
			var data = await _manageOffers.Delete(model);
			return data;
		}

		[HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageOffersLibrary>>> get(int Id = 0, string? Name = null, string? Status = null, bool? HasShippingFree = null, bool? showToCustomer = null, string? Code = null, string? OfferType = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null, string? offerIds = null, string? offerCreatedBy = null, string? createdBy = null)
		{
			ManageOffersLibrary model = new ManageOffersLibrary();
			model.id = Id;
			model.name = Name;
			model.offerType = OfferType;
			model.code = Code;
			model.status = Status;
			model.IsDeleted = Isdeleted;
			model.hasShippingFree = HasShippingFree;
			model.Searchtext = Searchtext;
            model.showToCustomer = showToCustomer;
            model.offerIds = offerIds;
			model.offerCreatedBy = offerCreatedBy;
			model.CreatedBy = createdBy;

			var data = await _manageOffers.get(model, PageIndex, PageSize, Mode);
			return data;
		}
	}
}
