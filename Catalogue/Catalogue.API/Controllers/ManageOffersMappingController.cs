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
	
	public class ManageOffersMappingController : ControllerBase
	{
		private readonly IManageOffersMappingService _manageOffersMapping;

		public ManageOffersMappingController(IManageOffersMappingService manageOffersMapping)
        {
			_manageOffersMapping = manageOffersMapping;
		}

		[HttpPost]
		[Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageOffersMappingLibrary model)
		{
			model.CreatedAt = DateTime.Now;

			var data = await _manageOffersMapping.Create(model);
			return data;
		}

		[HttpPut]
		[Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageOffersMappingLibrary model)
		{
			model.ModifiedAt = DateTime.Now;
			var data = await _manageOffersMapping.Update(model);
			return data;
		}

		[HttpDelete]
		[Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
		{
			string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
			ManageOffersMappingLibrary model = new ManageOffersMappingLibrary();
			model.id = Id;
			model.DeletedAt = DateTime.Now;
			model.DeletedBy = DeletedBy;
			var data = await _manageOffersMapping.Delete(model);
			return data;
		}

		[HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageOffersMappingLibrary>>> get(int Id = 0, string? offerId = null, int categoryId = 0, int brandId = 0, int productId = 0, int getProductId = 0, string? sellerId = null, string? userId = null, string? offerName = null, string? productName = null, string? categoryName = null, string? status = null, bool IsDeleted = false, bool? sellerOptIn = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null, string? ApplyOn = null)
        {
			ManageOffersMappingLibrary model = new ManageOffersMappingLibrary();
			model.id  = Id;
			model.offerIds = offerId;
			model.categoryId = categoryId;
			model.brandId = brandId;
			model.productId = productId;
			model.getProductId = getProductId;
			model.sellerId = sellerId;
			model.userId = userId;
			model.offerName = offerName;
			model.productName = productName;
			model.categoryName = categoryName;
			model.status = (string)status;
			model.IsDeleted = IsDeleted;
			model.sellerOptIn = sellerOptIn;
			model.Searchtext = Searchtext;
			model.ApplyOn = ApplyOn;

			var data = await _manageOffersMapping.get(model, PageIndex, PageSize, Mode);
			return data;
		}
	}
}
