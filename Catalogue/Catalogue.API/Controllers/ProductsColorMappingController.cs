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
    
    public class ProductsColorMappingController : ControllerBase
    {
        private readonly IProductsColorMappingService _colorMappingService;
        public ProductsColorMappingController(IProductsColorMappingService colorMappingService)
        {
            _colorMappingService = colorMappingService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ProductColorMapping productColor)
        {
            productColor.CreatedAt = DateTime.Now;
            var data = await _colorMappingService.Create(productColor);
            return data;
        }


        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int ProductID)
        {
            ProductColorMapping productColor = new ProductColorMapping();
            productColor.ProductID = ProductID;
            var data = await _colorMappingService.Delete(productColor);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductColorMapping>>> get(int ID = 0, int? ProductID = 0, int? ColorID = 0, string? ColorName = null, string? ColorCode = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? searchText = null)
        {
            ProductColorMapping productColor = new ProductColorMapping();
            productColor.Id = ID;
            productColor.ProductID = ProductID;
            productColor.ColorID = ColorID;
            productColor.ColorName = ColorName;
            productColor.ColorCode = ColorCode;
            productColor.searchText = searchText;

			var data = await _colorMappingService.get(productColor, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
