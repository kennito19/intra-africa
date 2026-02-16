using Catalogue.Application.IServices;
using Catalogue.Application.Services;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductSpecificationMappingController : ControllerBase
    {
        private readonly IProductSpecificationMappingService _productSpecificationMappingService;

        public ProductSpecificationMappingController(IProductSpecificationMappingService productSpecificationMappingService)
        {
            _productSpecificationMappingService = productSpecificationMappingService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ProductSpecificationMapping productSpecificationMapping)
        {
            productSpecificationMapping.CreatedAt = DateTime.Now;
            var data = await _productSpecificationMappingService.Create(productSpecificationMapping);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ProductSpecificationMapping productSpecificationMapping)
        {
            productSpecificationMapping.ModifiedAt = DateTime.Now;
            var data = await _productSpecificationMappingService.Update(productSpecificationMapping);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int ProductId)
        {
            ProductSpecificationMapping productSpecificationMapping = new ProductSpecificationMapping();
            productSpecificationMapping.ProductID = ProductId;

            var data = await _productSpecificationMappingService.Delete(productSpecificationMapping);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductSpecificationMapping>>> get(int Id = 0, int CatId = 0, int ProductId = 0, int SpecId = 0, int SpecTypeId = 0, int SpecValueId = 0, string? Value = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            ProductSpecificationMapping productSpecificationMapping = new ProductSpecificationMapping();
            productSpecificationMapping.Id = Id;
            productSpecificationMapping.CatId = CatId;
            productSpecificationMapping.ProductID = ProductId;
            productSpecificationMapping.SpecId = SpecId;
            productSpecificationMapping.SpecTypeId = SpecTypeId;
            productSpecificationMapping.SpecValueId = SpecValueId;
            productSpecificationMapping.Value = Value;


            var data = await _productSpecificationMappingService.get(productSpecificationMapping, PageIndex, PageSize, Mode);
            return data;
        }

    }
}
