using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogue.Domain.DTO;

namespace Catalogue.Application.IRepositories
{
    public interface ISellerProductRepository
    {
        Task<BaseResponse<long>> AddSellerProduct(SellerProduct sellerProduct);

        Task<BaseResponse<long>> UpdateSellerProduct(SellerProduct sellerProduct);

        Task<BaseResponse<long>> DeleteSellerProduct(SellerProduct sellerProduct);

        Task<BaseResponse<long>> ArchivedSellerProduct(SellerProduct sellerProduct);

        Task<BaseResponse<List<SellerProduct>>> GetSellerProduct(SellerProduct sellerProduct, int PageIndex, int PageSize, string Mode, bool? isArchive = null);
        Task<BaseResponse<List<SellerProductDetails>>> GetSellerProductDetails(SellerProductDetails sellerProduct, int PageIndex, int PageSize, string Mode, bool? isDeleted = null, bool? isArchive = null);

        Task<BaseResponse<long>> UpdateProductExtraDetails(ProductExtraDetailsDto ExtraDetails);
    }
}
