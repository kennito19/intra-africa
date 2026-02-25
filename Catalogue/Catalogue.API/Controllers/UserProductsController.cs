using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProductsController : ControllerBase
    {
        private readonly IUserProductService _userProductService;
        public UserProductsController(IUserProductService userProductService)
        {
            _userProductService = userProductService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int? CategoryId = 0, string? guIds = null, string? SellerIds = null, string? BrandIds = null, string? searchTexts = null, string? SizeIds = null, string? ColorIds = null, string? productCollectionId = null, string? MinPrice = null, string? MaxPrice = null, string? MinDiscount = null, bool? availableProduct = false, int? PriceSort = 0, string? SpecTypeIds = null, int? pageIndex = 1, int? pageSize = 30)
        {
            UserProductParams parameters = new UserProductParams()
            {
                CategoryId = CategoryId,
                SellerIds = SellerIds == null ? string.Empty : SellerIds,
                BrandIds = BrandIds == null ? string.Empty : BrandIds,
                ColorIds = ColorIds == null ? string.Empty : ColorIds,
                SizeIds = SizeIds == null ? string.Empty : SizeIds,
                searchTexts = searchTexts == null ? string.Empty : searchTexts,
                productCollectionId = productCollectionId == null ? string.Empty : productCollectionId,
                guIds = guIds == null ? string.Empty : guIds,
                MinPrice = MinPrice == null ? string.Empty : MinPrice,
                MaxPrice = MaxPrice == null ? string.Empty : MaxPrice,
                MinDiscount = MinDiscount == null ? string.Empty : MinDiscount,
                AvailableProductsOnly = availableProduct,
                PriceSort = PriceSort,
                SpecTypeIds = SpecTypeIds == null ? string.Empty : SpecTypeIds,
            };

            var response = await _userProductService.get(parameters, pageIndex, pageSize);
            return Ok(response);

        }
    }
}
