using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Catalogue.Domain.DTO;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly IProductListService _productListService;
        private readonly IArchiveProductListService _archiveListService;

        public ProductController(IProductsService productsService, IProductListService productListService, IArchiveProductListService archiveListService)
        {
            _productsService = productsService;
            _productListService = productListService;
            _archiveListService = archiveListService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(Products products)
        {
            products.CreatedAt = DateTime.Now;
            var data = await _productsService.Create(products);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(Products products)
        {
            products.ModifiedAt = DateTime.Now;
            var data = await _productsService.Update(products);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            Products products = new Products();
            products.Id = Id;
            products.DeletedBy = DeletedBy;
            products.DeletedAt = DateTime.Now;
            var data = await _productsService.Delete(products);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<Products>>> get(int ID = 0, int? CategoryID = 0, int? AssiCategoryId = 0, int? ParentId = 0, string? PathIds = null, string? Guid = null, string? CompanySKUCode = null, string? HSNCode = null, bool IsMasterProduct = false, bool Isdeleted = false, bool Getparent = false, bool Getchild = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? searchText = null)
        {
            Products products = new Products();
            products.Id = ID;
            products.AssiCategoryId = AssiCategoryId;
            products.CategoryId = CategoryID;
            products.ParentId = ParentId;
            products.CategoryPathIds = PathIds;
            products.Guid = Guid;
            products.CompanySKUCode = CompanySKUCode;
            products.HSNCode = HSNCode;
            products.IsDeleted = Isdeleted;
            products.IsMasterProduct = IsMasterProduct;
            products.searchText = searchText;


            var data = await _productsService.get(products, Getparent, Getchild, PageIndex, PageSize, Mode);
            return data;
        }

        [HttpGet("getProductList")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductListLibrary>>> getProductList(int ID = 0, int CategoryID = 0, string? SellerId = null, int? BrandId = 0, string? Status = null, bool? Live = null, bool IsMasterProduct = false, bool Isdeleted = false, bool Getparent = false, bool Getchild = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? searchText = null)
        {
            ProductListLibrary products = new ProductListLibrary();
            products.Id = ID;
            products.SellerId = SellerId;
            products.BrandId = BrandId;
            products.CategoryId = CategoryID;
            products.Status = Status;
            if (Live != null)
            {
                products.Live = Live;
            }
            //products.IsMasterProduct = IsMasterProduct;
            products.IsDeleted = Isdeleted;
            products.SearchText = searchText;

            var data = await _productListService.get(products, PageIndex, PageSize, Mode);
            return data;
        }

        [HttpGet("getArchiveProductList")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ArchiveProductList>>> getArchiveProductList(int? ProductId = 0, int? ProductMasterid = 0, int? CategoryId = 0, int? AssiCategoryId = 0, string? CompanySkuCode = null, string? SellerSkuCode = null, string? SellerId = null, int? BrandId = 0, string? Guid = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? searchText = null)
        {
            ArchiveProductList products = new ArchiveProductList();
            if (ProductId != 0 && ProductId != null) { products.ProductId = ProductId; }
            if (ProductMasterid != 0 && ProductMasterid != null) { products.ProductMasterId = ProductMasterid; }
            if (CategoryId != 0 && CategoryId != null) { products.CategoryId = CategoryId; }
            if (AssiCategoryId != 0 && AssiCategoryId != null) { products.AssiCategoryId = AssiCategoryId; }
            if (BrandId != 0 && BrandId != null) { products.BrandID = BrandId; }
            products.CompanySKUCode = CompanySkuCode;
            products.SellerSKUCode = SellerSkuCode;
            products.SellerID = SellerId;
            products.Guid = Guid;
            products.SearchText = searchText;

            var data = await _archiveListService.get(products, PageIndex, PageSize, Mode);
            return data;
        }

        [HttpGet("getUserProductDetails")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<UserProductDetails>>> getUserProductDetails(string ProductGuid)
        {
            var data = await _productsService.getUserProductDetails(ProductGuid);
            return data;
        }

        [HttpGet("getExistingProductList")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<AddInExistingProductList>>> getAddInExistingProductList(string SellerId, int? CategoryId = 0, int? BrandId = 0, string? CompanySkuCode = null,string? searchText = null, int PageIndex = 1, int PageSize = 10)
        {
            AddInExistingProductList products = new AddInExistingProductList();
            if (CategoryId != 0 && CategoryId != null) { products.CategoryId = CategoryId; }
            if (BrandId != 0 && BrandId != null) { products.BrandID = BrandId; }
            products.CompanySKUCode = CompanySkuCode;
            products.SellerID = SellerId;
            products.SearchText = searchText;

            var data = await _productsService.getAddInExistingProductList(products, PageIndex, PageSize);
            return data;
        }

        [HttpGet("getProductCompare")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductCompare>>> getProductCompare(string SellerProductId)
        {
            var data = await _productsService.getProductCompare(SellerProductId);
            return data;
        }
        
        [HttpGet("getProductCompareBrand")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductCompareBrand>>> getProductCompareBrand(int CategoryId)
        {
            var data = await _productsService.getProductCompareBrand(CategoryId);
            return data;
        }
        
        [HttpGet("getProductCompareBrandProduct")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductCompareBrandProduct>>> getProductCompareBrandProduct(int CategoryId,int BrandId)
        {
            var data = await _productsService.getProductCompareBrandProduct(CategoryId, BrandId);
            return data;
        }

        [HttpGet("getProductBulkDetails")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductBulkDetails>>> getProductBulkDetails(int CategoryId, int BrandId)
        {
            ProductBulkDetailsParams detailsParams = new ProductBulkDetailsParams();
            detailsParams.CategoryId = CategoryId;
            detailsParams.BrandId = BrandId;
            var data = await _productsService.getProductBulkDetails(detailsParams);
            return data;
        }

        [HttpGet("getProductBulkDownload")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductBulkDownload>>> getProductBulkDownload(int CategoryId, int BrandId, string SellerId)
        {
           
            var data = await _productsService.getProductBulkDownload(CategoryId, BrandId, SellerId);
            return data;
        }
        
        [HttpGet("getProductBulkDownloadForStock")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductBulkDownloadForStock>>> getProductBulkDownloadForStock(string? SellerId)
        {
           
            var data = await _productsService.getProductBulkDownloadForStock(SellerId);
            return data;
        }

    }

}
