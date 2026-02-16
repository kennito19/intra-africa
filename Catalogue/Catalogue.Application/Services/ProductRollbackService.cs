using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogue.Application.IServices;

namespace Catalogue.Application.Services
{
    public class ProductRollbackService:IProductRollbackService
    {
        private readonly IProductRollbackRepository _productRollback;

        public ProductRollbackService(IProductRollbackRepository productRollback)
        {
            _productRollback = productRollback;
        }

        public async Task<BaseResponse<long>> RemoveProduct(int ProductId)
        {
            var data =await _productRollback.RemoveProduct(ProductId);
            return data;
        }
    }
}
