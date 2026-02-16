using Catalogue.Application.IServices;
using Catalogue.Application.Services;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCountsController : ControllerBase
    {
        private readonly IProductsCountService _productsCountService;

        public ProductCountsController(IProductsCountService productsCountService)
        {
            _productsCountService = productsCountService;
        }

        [HttpGet]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<List<ProductCounts>>> Get(string? sellerId=null, string? days = null)
        {
            var data = await _productsCountService.get(sellerId, days);
            return data;
        }
    }
}
