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
    public class ProductViewController : ControllerBase
    {
        private readonly IProductViewService _productViewService;

        public ProductViewController(IProductViewService productViewService)
        {
            _productViewService = productViewService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ProductView productView)
        {
            productView.CreatedAt = DateTime.Now;

            var data = await _productViewService.Create(productView);
            return data;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<List<ProductView>>> Get(int? ProductId = 0, string? SellerId = null, string? UserId = null, int? SellerProductId = 0, string? fromDate = null, string? toDate = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {

            ProductView productView = new ProductView();
            productView.ProductId = Convert.ToInt32(ProductId);
            productView.SellerId = SellerId;
            productView.UserId = UserId;
            productView.SellerProductId = Convert.ToInt32(SellerProductId);
            productView.fromDate = fromDate;
            productView.toDate = toDate;

            var data = await _productViewService.get(productView, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
