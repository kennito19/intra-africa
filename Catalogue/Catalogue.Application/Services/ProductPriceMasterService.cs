using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class ProductPriceMasterService : IProductPriceMasterService
    {
        private readonly IProductPriceMasterRepository _productPriceMasterRepository;

        public ProductPriceMasterService(IProductPriceMasterRepository productPriceMasterRepository)
        {
            _productPriceMasterRepository = productPriceMasterRepository;
        }
        public Task<BaseResponse<long>> Create(ProductPriceMaster productPriceMaster)
        {
            var data = _productPriceMasterRepository.Create(productPriceMaster);
            return data;
        }

        public Task<BaseResponse<long>> Update(ProductPriceMaster productPriceMaster)
        {
            var data = _productPriceMasterRepository.Update(productPriceMaster);
            return data;
        }

        public Task<BaseResponse<long>> Delete(ProductPriceMaster productPriceMaster)
        {
            var data = _productPriceMasterRepository.Delete(productPriceMaster);
            return data;
        }

        public Task<BaseResponse<List<ProductPriceMaster>>> Get(ProductPriceMaster productPriceMaster, int PageIndex, int PageSize, string Mode)
        {
            var data = _productPriceMasterRepository.Get(productPriceMaster, PageIndex, PageSize, Mode);
            return data;
        }

        
    }
}
