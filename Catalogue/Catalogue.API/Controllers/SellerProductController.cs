using Catalogue.Application.IServices;
using Catalogue.Application.Services;
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
    
    public class SellerProductController : ControllerBase
    {
        private readonly ISellerProductService _sellerProductService;

        public SellerProductController(ISellerProductService sellerProductService)
        {
            _sellerProductService = sellerProductService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> AddSellerProduct(SellerProduct sellerProduct)
        {
            sellerProduct.IsSizeWisePriceVariant = sellerProduct.IsSizeWisePriceVariant;
            sellerProduct.ManufacturedDate = null;
            sellerProduct.ExpiryDate = null;
            sellerProduct.CreatedAt = DateTime.Now;
            var data = await _sellerProductService.AddSellerProduct(sellerProduct);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> UpdateSellerProduct(SellerProduct sellerProduct)
        {
            sellerProduct.IsSizeWisePriceVariant = sellerProduct.IsSizeWisePriceVariant;
            sellerProduct.ManufacturedDate = null;
            sellerProduct.ExpiryDate = null;
            sellerProduct.ModifiedAt = DateTime.Now;
            var data = await _sellerProductService.UpdateSellerProduct(sellerProduct);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> DeleteSellerProduct(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SellerProduct sellerProduct = new SellerProduct();
            sellerProduct.Id = Id;
            sellerProduct.DeletedBy = DeletedBy;
            sellerProduct.DeletedAt = DateTime.Now;
            var data = await _sellerProductService.DeleteSellerProduct(sellerProduct);
            return data;
        }

        [HttpPut]
        [Route("Archived")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> ArchivedSellerProduct(string SellerId)
        {
            string ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SellerProduct sellerProduct = new SellerProduct();
            sellerProduct.SellerID = SellerId;
            sellerProduct.ModifiedBy = ModifiedBy;
            sellerProduct.ModifiedAt = DateTime.Now;
            var data = await _sellerProductService.ArchivedSellerProduct(sellerProduct);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<SellerProduct>>> getSellerProduct(int? id = null, int? productId = null,int? productMasterId = null, string? sellerId = null, int? brandId = null, int? categoryId = null , int? weightSlabId = null, string? skuCode = null, string? companySKUCode = null, bool? isSizeWisePriceVariant = null, bool? isProductExist = null, bool? isDeleted = null, bool? isArchive = null, bool? live = null, string? status = null, string? fromDate = null, string? toDate = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            SellerProduct sellerProduct = new SellerProduct();
            if (id != null)
            {
                sellerProduct.Id = Convert.ToInt32(id);
            }
            if (productId != null)
            {
                sellerProduct.ProductID = Convert.ToInt32(productId);
            }
            if (productMasterId != null)
            {
                sellerProduct.ProductMasterId = Convert.ToInt32(productMasterId);
            }
            if (sellerId != null)
            {
                sellerProduct.SellerID = Convert.ToString(sellerId);
            }
            if (brandId != null)
            {
                sellerProduct.BrandID = Convert.ToInt32(brandId);
            }
            if (categoryId != null)
            {
                sellerProduct.CategoryId = Convert.ToInt32(categoryId);
            }
            if (weightSlabId != null)
            {
                sellerProduct.WeightSlabId = Convert.ToInt32(weightSlabId);
            }
            if (live != null)
            {
                sellerProduct.Live = Convert.ToBoolean(live);
            }
            if (isProductExist != null)
            {
            sellerProduct.IsExistingProduct = Convert.ToBoolean(isProductExist);
            }
            if (isSizeWisePriceVariant != null)
            {
            sellerProduct.IsSizeWisePriceVariant = Convert.ToBoolean(isSizeWisePriceVariant);
            }
            if (isDeleted != null)
            {
                sellerProduct.IsDeleted = Convert.ToBoolean(isDeleted);
            }
            if (!string.IsNullOrEmpty(fromDate))
            {
                sellerProduct.FromDate = fromDate;
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                sellerProduct.ToDate = toDate;
            }
            sellerProduct.SKUCode = skuCode;
            sellerProduct.CompanySKUCode = companySKUCode;
            sellerProduct.Status = status;
            var data = await _sellerProductService.GetSellerProduct(sellerProduct, PageIndex, PageSize, Mode, isArchive);
            return data;
        }

        [HttpGet("getSellerProductDetails")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<SellerProductDetails>>> GetSellerProductDetails(int? productId = null, string? sellerProductId = null, string? productGuid = null, string? status = null, bool? isDeleted=null, bool? IsArchive = null, bool? liveStatus = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            SellerProductDetails sellerProduct = new SellerProductDetails();
            if (productId != null)
            {
                sellerProduct.ProductId = Convert.ToInt32(productId);
            }
            if (sellerProductId != null)
            {
                sellerProduct.SellerProductId = Convert.ToInt32(sellerProductId);
            }
            if (productGuid != null)
            {
                sellerProduct.ProductGuid = Convert.ToString(productGuid);
            }
            if (liveStatus != null)
            {
                sellerProduct.LiveStatus = Convert.ToBoolean(liveStatus);
            }
            sellerProduct.Status = status;
            
            var data = await _sellerProductService.GetSellerProductDetails(sellerProduct,PageIndex, PageSize, Mode, isDeleted, IsArchive);
            return data;
        }

        [HttpPut("UpdateExtraDetails")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> UpdateExtraDetails(ProductExtraDetailsDto ExtraDetails)
        {
            var data = await _sellerProductService.UpdateProductExtraDetails(ExtraDetails);
            return data;
        }
    }
}
