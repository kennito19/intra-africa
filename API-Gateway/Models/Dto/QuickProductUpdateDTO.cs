using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class QuickProductUpdateDTO
    {
        public int SellerProductId { get; set; }
        public int ProductId { get; set; }
        public string ProductSku { get; set; }
        public string SellerSku { get; set; }
        public int? SizeTypeId { get; set; }
        public string? SizeTypeName { get; set; }
        public bool IsSizeWisePriceVariant { get; set; }
        public bool IsExistingProduct { get; set; }
        public string? ProductName { get; set; }
        public string? Status { get; set; }
        public bool? Live { get; set; }

        public decimal? PackingBreadth { get; set; }
        public decimal? PackingHeight { get; set; }
        public decimal? PackingLength { get; set; }
        public decimal? PackingWeight { get; set; }
        public int? WeightSlabId { get; set; }
        [JsonIgnore]
        public DateTime? ManufacturedDate { get; set; }
        [JsonIgnore]
        public DateTime? ExpiryDate { get; set; }
        [JsonIgnore]
        public int? MOQ { get; set; }
        public List<ProductPriceDTO> ProductPrice { get; set; }
    }
}
