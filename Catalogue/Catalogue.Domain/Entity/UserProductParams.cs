

namespace Catalogue.Domain.Entity
{
    public class UserProductParams
    {
        public int? CategoryId { get; set; } = 0;
        public string? SellerIds { get; set; } = string.Empty;
        public string? BrandIds { get; set; } = string.Empty;
        public string? searchTexts { get; set; } = string.Empty;
        public string? SizeIds { get; set; } = string.Empty;
        public string? ColorIds { get; set; } = string.Empty;
        public string? productCollectionId { get; set; } = string.Empty;
        public string? guIds { get; set; } = string.Empty;
        public string? MinPrice { get; set; } = string.Empty;
        public string? MaxPrice { get; set; } = string.Empty;
        public string? MinDiscount { get; set; } = string.Empty;
        public bool? AvailableProductsOnly { get; set; } = false;
        public int? PriceSort { get; set; } = 0;
        public string? SpecTypeIds { get; set; } = string.Empty;

    }
}
