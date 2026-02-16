using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IProductSpecificationMappingService
    {
        Task<BaseResponse<long>> Create(ProductSpecificationMapping productSpecificationMapping);

        Task<BaseResponse<long>> Update(ProductSpecificationMapping productSpecificationMapping);

        Task<BaseResponse<long>> Delete(ProductSpecificationMapping productSpecificationMapping);

        Task<BaseResponse<List<ProductSpecificationMapping>>> get(ProductSpecificationMapping productSpecificationMapping, int PageIndex, int PageSize, string Mode);
    }
}
