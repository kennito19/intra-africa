using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IProductViewRepository
    {
        Task<BaseResponse<long>> Create(ProductView productView);
        Task<BaseResponse<List<ProductView>>> get(ProductView productView, int PageIndex, int PageSize, string Mode);
    }
}
