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
    
    public class ProductRollbackController : ControllerBase
    {
        private readonly IProductRollbackService _productRollbackService;
        public ProductRollbackController(IProductRollbackService productRollbackService)
        {
            _productRollbackService = productRollbackService;
        }

        [HttpGet]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> get(int ProductId)
        { 
            var data = await _productRollbackService.RemoveProduct(ProductId);
            return data;
        }
    }
}
