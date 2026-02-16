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
    public class ProductsImagesService : IProductsImagesService
    {
        private readonly IProductsImagesRepository _imagesRepository;
        public ProductsImagesService(IProductsImagesRepository imagesRepository)
        {
            _imagesRepository = imagesRepository;
        }
        public Task<BaseResponse<long>> Create(ProductImages productImages)
        {
            var response = _imagesRepository.Create(productImages);
            return response;
        }
        public Task<BaseResponse<long>> Update(ProductImages productImages)
        {
            var response = _imagesRepository.Update(productImages);
            return response;
        }
        public Task<BaseResponse<long>> Delete(ProductImages productImages)
        {
            var response = _imagesRepository.Delete(productImages);
            return response;
        }

        public Task<BaseResponse<List<ProductImages>>> get(ProductImages productImages, int PageIndex, int PageSize, string Mode)
        {
            var response = _imagesRepository.get(productImages, PageIndex, PageSize, Mode);
            return response;
        }

        
    }
}
