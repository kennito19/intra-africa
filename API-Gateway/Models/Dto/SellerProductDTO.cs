using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class SellerProductDTO
    {
        public int Id { get; set; }
        public int ProductID { get; set; }
        public string SellerID { get; set; }
        public int BrandID { get; set; }
        public string? SellerSKU { get; set; }
        [JsonInclude]
        public bool IsSizeWisePriceVariant { get; set; }
        public bool IsExistingProduct { get; set; }
        public bool? Live { get; set; }
        public string? Status { get; set; }
        [JsonIgnore]
        public DateTime? ManufacturedDate { get; set; }
        [JsonIgnore]
        public DateTime? ExpiryDate { get; set; }
        public string? ExtraDetails { get; set; }
        public decimal? PackingLength { get; set; } = 0;
        public decimal? PackingBreadth { get; set; } = 0;
        public decimal? PackingHeight { get; set; } = 0;
        public decimal? PackingWeight { get; set; } = 0;
        public int? WeightSlabId { get; set; }
        [JsonIgnore]
        public int? MOQ { get; set; } = null;
        public string? SellerName { get; set; }
        public string? BrandName { get; set; }
        public string? WeightSlabName { get; set; } = null;
        public string? ShipmentBy { get; set; } = null;
        public string? ShipmentPaidBy { get; set; } = null;

        public IEnumerable<ProductPriceDTO>? ProductPrices { get; set; }
    }
}
