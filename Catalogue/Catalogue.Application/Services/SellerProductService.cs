using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.DTO;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class SellerProductService : ISellerProductService
    {
        private readonly ISellerProductRepository _sellerProductRepository;

        public SellerProductService(ISellerProductRepository sellerProductRepository)
        {
            _sellerProductRepository = sellerProductRepository;
        }

        public Task<BaseResponse<long>> AddSellerProduct(SellerProduct sellerProduct)
        {
            var responce = _sellerProductRepository.AddSellerProduct(sellerProduct);
            return responce;
        }

        public Task<BaseResponse<long>> DeleteSellerProduct(SellerProduct sellerProduct)
        {
            var responce = _sellerProductRepository.DeleteSellerProduct(sellerProduct);
            return responce;
        }

        public Task<BaseResponse<long>> ArchivedSellerProduct(SellerProduct sellerProduct)
        {
            var responce = _sellerProductRepository.ArchivedSellerProduct(sellerProduct);
            return responce;
        }

        public Task<BaseResponse<List<SellerProduct>>> GetSellerProduct(SellerProduct sellerProduct, int PageIndex, int PageSize, string Mode, bool? isArchive = null)
        {
            var responce = _sellerProductRepository.GetSellerProduct(sellerProduct, PageIndex, PageSize, Mode, isArchive);
            return responce;
        }

        public Task<BaseResponse<long>> UpdateSellerProduct(SellerProduct sellerProduct)
        {
            var responce = _sellerProductRepository.UpdateSellerProduct(sellerProduct);
            return responce;
        }

        public Task<BaseResponse<List<SellerProductDetails>>> GetSellerProductDetails(SellerProductDetails sellerProduct, int PageIndex, int PageSize, string Mode, bool? isDeleted = null, bool? isArchive = null)
        {
            var responce = _sellerProductRepository.GetSellerProductDetails(sellerProduct, PageIndex, PageSize, Mode, isDeleted, isArchive);
            return responce;
        }

        public Task<BaseResponse<long>> UpdateProductExtraDetails(ProductExtraDetailsDto ExtraDetails)
        {
            var responce = _sellerProductRepository.UpdateProductExtraDetails(ExtraDetails);
            return responce;
        }
    }
}
