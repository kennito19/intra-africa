using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IProductRollbackRepository
    {
        Task<BaseResponse<long>> RemoveProduct(int ProductId);
    }
}
