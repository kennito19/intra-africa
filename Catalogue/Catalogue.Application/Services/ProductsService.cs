using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.DTO;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public Task<BaseResponse<long>> Create(Products products)
        {
            var data = _productsRepository.Create(products);
            return data;
        }
        public Task<BaseResponse<long>> Update(Products products)
        {
            var data = _productsRepository.Update(products);
            return data;
        }
        public Task<BaseResponse<long>> Delete(Products products)
        {
            var data = _productsRepository.Delete(products);
            return data;
        }

        public Task<BaseResponse<List<Products>>> get(Products products, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode)
        {
            var data = _productsRepository.get(products,Getparent,Getchild, PageIndex, PageSize, Mode);
            return data;
        }
        public Task<BaseResponse<List<UserProductDetails>>> getUserProductDetails(string ProductGuid)
        {
            var data = _productsRepository.getUserProductDetails(ProductGuid);
            return data;
        }

        public Task<BaseResponse<List<AddInExistingProductList>>> getAddInExistingProductList(AddInExistingProductList addInExisting, int PageIndex, int PageSize)
        {
            var data = _productsRepository.getAddInExistingProductList(addInExisting, PageIndex, PageSize);
            return data;
        }

        public Task<BaseResponse<List<ProductCompare>>> getProductCompare(string SellerProductId)
        {
            var data = _productsRepository.getProductCompare(SellerProductId);
            return data;
        }

        public Task<BaseResponse<List<ProductCompareBrand>>> getProductCompareBrand(int CategoryId)
        {
            var data = _productsRepository.getProductCompareBrand(CategoryId);
            return data;
        }

        public Task<BaseResponse<List<ProductCompareBrandProduct>>> getProductCompareBrandProduct(int CategoryId,int BrandId)
        {
            var data = _productsRepository.getProductCompareBrandProduct(CategoryId, BrandId);
            return data;
        }

        public Task<BaseResponse<List<ProductBulkDetails>>> getProductBulkDetails(ProductBulkDetailsParams detailsParams)
        {
            var data = _productsRepository.getProductBulkDetails(detailsParams);
            return data;
        }
        public Task<BaseResponse<List<ProductBulkDownload>>> getProductBulkDownload(int CategoryId, int BrandId, string SellerId)
        {
            var data = _productsRepository.getProductBulkDownload(CategoryId, BrandId, SellerId);
            return data;
        }

        public Task<BaseResponse<List<ProductBulkDownloadForStock>>> getProductBulkDownloadForStock(string? SellerId)
        {
            var data = _productsRepository.getProductBulkDownloadForStock( SellerId);
            return data;
        }
    }
}
