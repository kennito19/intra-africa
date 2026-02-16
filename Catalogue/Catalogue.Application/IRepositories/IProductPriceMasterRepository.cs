using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IProductPriceMasterRepository
    {
        Task<BaseResponse<long>> Create(ProductPriceMaster productPriceMaster);

        Task<BaseResponse<long>> Update(ProductPriceMaster productPriceMaster);

        Task<BaseResponse<long>> Delete(ProductPriceMaster productPriceMaster);

        Task<BaseResponse<List<ProductPriceMaster>>> Get(ProductPriceMaster productPriceMaster, int PageIndex, int PageSize, string Mode);
    }
}
