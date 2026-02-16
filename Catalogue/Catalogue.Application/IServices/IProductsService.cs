using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogue.Domain.DTO;

namespace Catalogue.Application.IServices
{
    public interface IProductsService
    {
        Task<BaseResponse<long>> Create(Products products);

        Task<BaseResponse<long>> Update(Products products);

        Task<BaseResponse<long>> Delete(Products products);

        Task<BaseResponse<List<Products>>> get(Products products, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode);
        Task<BaseResponse<List<UserProductDetails>>> getUserProductDetails(string ProductGuid);
        Task<BaseResponse<List<AddInExistingProductList>>> getAddInExistingProductList(AddInExistingProductList addInExisting, int PageIndex, int PageSize);
        Task<BaseResponse<List<ProductCompare>>> getProductCompare(string SellerProductId);
        Task<BaseResponse<List<ProductCompareBrand>>> getProductCompareBrand(int CategoryId);
        Task<BaseResponse<List<ProductCompareBrandProduct>>> getProductCompareBrandProduct(int CategoryId, int BrandId);
        Task<BaseResponse<List<ProductBulkDetails>>> getProductBulkDetails(ProductBulkDetailsParams detailsParams);
        Task<BaseResponse<List<ProductBulkDownload>>> getProductBulkDownload(int CategoryId, int BrandId, string SellerId);
        Task<BaseResponse<List<ProductBulkDownloadForStock>>> getProductBulkDownloadForStock(string? SellerId);
    }
}
