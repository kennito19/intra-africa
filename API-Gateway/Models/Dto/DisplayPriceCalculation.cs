using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class DisplayPriceCalculation
    {
        [JsonInclude]
        public int? categoryId { get; set; }
        [JsonInclude]
        public string? sellerId { get; set; }
        [JsonInclude]
        public int? BrandId { get; set; }
        [JsonInclude]
        public int? weightSlabId { get; set; }
        public decimal? mrp { get; set; }
        public decimal? sellingprice { get; set; }
        [JsonInclude]
        public string? CommissionChargesIn { get; set; }
        [JsonInclude]
        public string? CommissionCharges { get; set; }
        [JsonInclude]
        public string? CommissionRate { get; set; }
        [JsonIgnore]
        public string? CommissionGSTRate { get; set; }
        [JsonIgnore]
        public string? token { get; set; }
        [JsonIgnore]
        public string? shipmentBy { get; set; }
        [JsonInclude]
        public string? shippingPaidBy { get; set; }
    }
}
