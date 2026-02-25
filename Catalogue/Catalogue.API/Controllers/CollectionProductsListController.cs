using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionProductsListController : ControllerBase
    {
        private readonly ICollectionProductsListService _collectionProductsListService;

        public CollectionProductsListController(ICollectionProductsListService collectionProductsListService)
        {
            _collectionProductsListService = collectionProductsListService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<BaseResponse<List<CollectionProductsList>>> Get(int collectionId = 0, int Id = 0, int categoryId = 0, string? sellerId = null, int? brandId = 0,  int pageIndex = 1, int pageSize = 10, string? searchText = null)
        {
            CollectionProductsList products = new CollectionProductsList();
            products.CollectionId = collectionId;
            products.Id = Id;
            products.SellerId = sellerId;
            products.BrandId = brandId;
            products.CategoryId = categoryId;
            products.SearchText = searchText;
            //products.IsMasterProduct = IsMasterProduct;

            var data = await _collectionProductsListService.get(products, pageIndex, pageSize);
            return data;
        }
    }
}
