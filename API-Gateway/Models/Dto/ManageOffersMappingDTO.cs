namespace API_Gateway.Models.Dto
{
    public class ManageOffersMappingDTO
    {
        public int id { get; set; }
        public int offerId { get; set; }
        public int? categoryId { get; set; } = 0;
        public string? sellerId { get; set; } = null;
        public int? brandId { get; set; } = 0;
        public int? productId { get; set; } = 0;
        public int? getProductId { get; set; } = 0;
        public string? userId { get; set; } = null;
        public string? getDiscountType { get; set; } = null;
        public decimal? getDiscountValue { get; set; } = 0;
        public decimal? getProductPrice { get; set; } = 0;
        public bool? sellerOptIn { get; set; } = false;
        public string? optInSellerIds { get; set; } = null;
        public string? status { get; set; } = null;
        public string? CategoryIds { get; set; } = null;
        public string? SellerIds { get; set; } = null;
        public string? Brandids { get; set; } = null;
        public string? ProductIds { get; set; } = null;
    }
}
