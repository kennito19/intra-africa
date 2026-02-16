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
    public class ProductsCountService : IProductsCountService
    {
        private readonly IProductsCountRepository _productCountRepository;

        public ProductsCountService(IProductsCountRepository productCountRepository)
        {
            _productCountRepository = productCountRepository;
        }
        public async Task<BaseResponse<List<ProductCounts>>> get(string? sellerId, string? days)
        {
            var data = await _productCountRepository.get(sellerId, days);
            return data;
        }
    }
}
