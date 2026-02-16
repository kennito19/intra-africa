using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Order.Domain.Entity
{
    public class OrderTaxInfo
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public int ProductID { get; set; }
        public int SellerProductID { get; set; }
        [JsonInclude]
        public decimal? ShippingCharge { get; set; }
        [JsonInclude]
        public string? ShippingZone { get; set; }
        [JsonInclude]
        public decimal? TaxOnShipping { get; set; }
        [JsonInclude]
        public string? CommissionIn { get; set; }
        [JsonInclude]
        public decimal? CommissionRate { get; set; }
        [JsonInclude]
        public decimal? CommissionAmount { get; set; }
        [JsonInclude]
        public decimal? TaxOnCommission { get; set; }
        [JsonInclude]
        public decimal NetEarn { get; set; }
        public int OrderTaxRateId { get; set; }
        public string OrderTaxRate { get; set; }
        public string HSNCode { get; set; }
        [JsonIgnore]
        public string? ShipmentBy { get; set; }
        [JsonIgnore]
        public string? ShipmentPaidBy { get; set; }
        public bool IsSellerWithGSTAtOrderTime { get; set; }
        public string? WeightSlab { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
