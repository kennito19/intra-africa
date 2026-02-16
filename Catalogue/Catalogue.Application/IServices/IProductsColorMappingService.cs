using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IProductsColorMappingService
    {
        Task<BaseResponse<long>> Create(ProductColorMapping productColorMapping);

        Task<BaseResponse<long>> Update(ProductColorMapping productColorMapping);

        Task<BaseResponse<long>> Delete(ProductColorMapping productColorMapping);

        Task<BaseResponse<List<ProductColorMapping>>> get(ProductColorMapping productColorMapping, int PageIndex, int PageSize, string Mode);
    }
}
