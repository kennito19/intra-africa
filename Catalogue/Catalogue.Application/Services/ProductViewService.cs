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
    public class ProductViewService : IProductViewService
    {
        private readonly IProductViewRepository _productViewRepository;

        public ProductViewService(IProductViewRepository productViewRepository)
        {
            _productViewRepository = productViewRepository;
        }
        public Task<BaseResponse<long>> Create(ProductView productView)
        {
            var data = _productViewRepository.Create(productView);
            return data;
        }

        public Task<BaseResponse<List<ProductView>>> get(ProductView productView, int PageIndex, int PageSize, string Mode)
        {
            var data = _productViewRepository.get(productView, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
