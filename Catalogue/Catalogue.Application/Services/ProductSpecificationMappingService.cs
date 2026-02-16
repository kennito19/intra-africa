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
    public class ProductSpecificationMappingService : IProductSpecificationMappingService
    {
        private readonly IProductSpecificationMappingRepository _productSpecificationMappingRepository;

        public ProductSpecificationMappingService(IProductSpecificationMappingRepository productSpecificationMappingRepository)
        {
            _productSpecificationMappingRepository = productSpecificationMappingRepository;
        }

        public Task<BaseResponse<long>> Create(ProductSpecificationMapping productSpecificationMapping)
        {
            var response = _productSpecificationMappingRepository.Create(productSpecificationMapping);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ProductSpecificationMapping productSpecificationMapping)
        {
            var response = _productSpecificationMappingRepository.Delete(productSpecificationMapping);
            return response;
        }

        public Task<BaseResponse<List<ProductSpecificationMapping>>> get(ProductSpecificationMapping productSpecificationMapping, int PageIndex, int PageSize, string Mode)
        {
            var response = _productSpecificationMappingRepository.get(productSpecificationMapping, PageIndex, PageSize, Mode);
            return response;
        }

        public Task<BaseResponse<long>> Update(ProductSpecificationMapping productSpecificationMapping)
        {
            var response = _productSpecificationMappingRepository.Update(productSpecificationMapping);
            return response;
        }
    }
}
