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
    public class ProductsColorMappingService : IProductsColorMappingService
    {
        private readonly IProductsColorMappingRepository _colorMappingRepository;
        public ProductsColorMappingService(IProductsColorMappingRepository colorMappingRepository)
        {
            _colorMappingRepository = colorMappingRepository;
        }
        public Task<BaseResponse<long>> Create(ProductColorMapping productColorMapping)
        {
            var response = _colorMappingRepository.Create(productColorMapping);
            return response;
        }
        public Task<BaseResponse<long>> Update(ProductColorMapping productColorMapping)
        {
            var response = _colorMappingRepository.Update(productColorMapping);
            return response;
        }
        public Task<BaseResponse<long>> Delete(ProductColorMapping productColorMapping)
        {
            var response = _colorMappingRepository.Delete(productColorMapping);
            return response;
        }

        public Task<BaseResponse<List<ProductColorMapping>>> get(ProductColorMapping productColorMapping, int PageIndex, int PageSize, string Mode)
        {
            var response = _colorMappingRepository.get(productColorMapping,PageIndex, PageSize, Mode);
            return response;
        }

        
    }
}
