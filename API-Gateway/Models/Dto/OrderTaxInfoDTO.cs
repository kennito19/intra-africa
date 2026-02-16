using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class OrderTaxInfoDTO
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public int ProductID { get; set; }
        public int SellerProductID { get; set; }
        public decimal NetEarn { get; set; }
        public int OrderTaxRateId { get; set; }
        public string OrderTaxRate { get; set; }
        public string HSNCode { get; set; }
        [JsonInclude]
        public string? ShipmentBy { get; set; }
        [JsonInclude]
        public string? ShipmentPaidBy { get; set; }
        public bool IsSellerWithGSTAtOrderTime { get; set; }
        [JsonInclude]
        public string? WeightSlab { get; set; }
        [JsonInclude]
        public decimal ShippingCharge { get; set; }
        [JsonInclude]
        public string ShippingZone { get; set; }
        [JsonInclude]
        public decimal TaxOnShipping { get; set; }
        [JsonInclude]
        public string CommissionIn { get; set; }
        [JsonInclude]
        public decimal CommissionRate { get; set; }
        [JsonInclude]
        public decimal CommissionAmount { get; set; }
        [JsonIgnore]
        public decimal TaxOnCommission { get; set; }
    }
}
