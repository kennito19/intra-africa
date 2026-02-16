using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class ExistingProduct
    {
        public int ProductId { get; set; }

        public int BrandId { get; set; }
        public int? SellerProductId { get; set; }
        public string? SellerId { get; set; }
        public string? SellerSKU { get; set; }
        [JsonIgnore]
        public bool IsSizeWisePriceVariant { get; set; } = false;
        public bool IsExistingProduct { get; set; } = true;
        public bool? Live { get; set; }
        public string? Status { get; set; }
        [JsonIgnore]
        public DateTime? ManufacturedDate { get; set; }
        [JsonIgnore]
        public DateTime? ExpiryDate { get; set; }
        [JsonIgnore]
        public int? MOQ { get; set; } = null;
        public decimal? PackingLength { get; set; } = 0;
        public decimal? PackingBreadth { get; set; } = 0;
        public decimal? PackingHeight { get; set; } = 0;
        public decimal? PackingWeight { get; set; } = 0;

        public int? WeightSlabId { get; set; }

        public IEnumerable<ProductPriceDTO>? ProductPrices { get; set; }
    }
}
