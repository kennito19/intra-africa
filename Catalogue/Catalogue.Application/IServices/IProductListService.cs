using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IProductListService
    {
        Task<BaseResponse<List<ProductListLibrary>>> get(ProductListLibrary productList, int PageIndex, int PageSize, string Mode);
    }
}
