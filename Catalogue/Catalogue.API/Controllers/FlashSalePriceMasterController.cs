using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class FlashSalePriceMasterController : ControllerBase
    {
        private readonly IManageFlashSalePriceMasterService _flashSalePriceService;

        public FlashSalePriceMasterController(IManageFlashSalePriceMasterService flashSalePrice)
        {
            _flashSalePriceService = flashSalePrice;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(FlashSalePriceMasterLibrary flashSalePrice)
        {
            flashSalePrice.CreatedAt = DateTime.Now;

            var data = await _flashSalePriceService.Create(flashSalePrice);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(FlashSalePriceMasterLibrary flashSalePrice)
        {
            flashSalePrice.ModifiedAt = DateTime.Now;
            var data = await _flashSalePriceService.Update(flashSalePrice);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            FlashSalePriceMasterLibrary flashSalePrice = new FlashSalePriceMasterLibrary();
            flashSalePrice.Id = Id;
            flashSalePrice.DeletedBy = DeletedBy;
            flashSalePrice.DeletedAt = DateTime.Now;
            var data = await _flashSalePriceService.Delete(flashSalePrice);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<FlashSalePriceMasterLibrary>>> get(int Id = 0, int sellerProductId = 0 , int SellerWiseProductPriceMasterId = 0, int CollectionId = 0, int CollectionMappingId = 0, string? CollectionName = null,string? Status = null, bool? IsSellerOptIn = null, bool? IsDeleted = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            FlashSalePriceMasterLibrary flashSalePrice = new FlashSalePriceMasterLibrary();
            flashSalePrice.Id = Id;
            flashSalePrice.SellerProductId = sellerProductId;
            flashSalePrice.SellerWiseProductPriceMasterId = SellerWiseProductPriceMasterId;
            flashSalePrice.CollectionId = CollectionId;
            flashSalePrice.CollectionMappingId = CollectionMappingId;
            flashSalePrice.CollectionName = CollectionName;
            if (IsSellerOptIn != null)
            {
                flashSalePrice.IsSellerOptIn = IsSellerOptIn;
            }
            if (IsDeleted != null)
            {
                flashSalePrice.IsDeleted = IsDeleted;
            }
            flashSalePrice.SearchText = Searchtext;
            flashSalePrice.Status = Status;

            var data = await _flashSalePriceService.get(flashSalePrice, PageIndex, PageSize, Mode);
            return data;
        }


    }
}
