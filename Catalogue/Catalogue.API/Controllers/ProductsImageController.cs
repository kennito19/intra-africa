using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductsImageController : ControllerBase
    {
        private readonly IProductsImagesService _imagesService;
        public ProductsImageController(IProductsImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ProductImages productImages)
        {
            productImages.CreatedAt = DateTime.Now;
            var data = await _imagesService.Create(productImages);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int ProductID)
        {
            ProductImages productImages = new ProductImages();
            productImages.ProductID = ProductID;
            var data = await _imagesService.Delete(productImages);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductImages>>> get(int ID = 0, int? ProductID = 0, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            ProductImages productImages = new ProductImages();
            productImages.Id = ID;
            productImages.ProductID = ProductID;

            var data = await _imagesService.get(productImages, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
