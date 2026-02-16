using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface ICollectionProductsListRepository
    {
        Task<BaseResponse<List<CollectionProductsList>>> get(CollectionProductsList productList, int PageIndex, int PageSize);
    }
}
