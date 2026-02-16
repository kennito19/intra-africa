using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IArchiveProductListRepository
    {
        Task<BaseResponse<List<ArchiveProductList>>> get(ArchiveProductList productList, int PageIndex, int PageSize, string Mode);
    }
}
