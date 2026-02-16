using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class ProductListService : IProductListService
    {
        private readonly IProductListRepository _productListRepository;

        public ProductListService(IProductListRepository productListRepository)
        {
            _productListRepository = productListRepository;
        }

        public async Task<BaseResponse<List<ProductListLibrary>>> get(ProductListLibrary productList, int PageIndex, int PageSize, string Mode)
        {
            var data =await _productListRepository.get(productList, PageIndex, PageSize, Mode);
            return data;
        }

    }
}
