using System.Text.Json.Serialization;

namespace API_Gateway.Models.Entity.Catalogue
{
    public class SellerProductDetails
    {
        public int? RowNumber { get; set; }
        public int? PageCount { get; set; }
        public int? RecordCount { get; set; }
        public int? ProductId { get; set; }
        public string? ProductGuid { get; set; }
        public int? ProductMasterId { get; set; }
        public int? CategoryId { get; set; }
        public int? SellerProductId { get; set; }
        public string? SellerId { get; set; }
        public int? BrandId { get; set; }
        public int? PricemasterId { get; set; }
        public string? ProductName { get; set; }
        public string? CustomeProductName { get; set; }
        public string? ProductSkuCode { get; set; }
        public string? SellerSkuCode { get; set; }
        public bool? IsSizeWisePriceVariant { get; set; }
        public decimal? PackingLength { get; set; }
        public decimal? PackingBreadth { get; set; }
        public decimal? PackingHeight { get; set; }
        public decimal? PackingWeight { get; set; }
        public int? MOQ { get; set; }
        public int? AssiCategoryId { get; set; }
        public int? WeightSlabId { get; set; }
        public string? WeightSlab { get; set; }
        [JsonIgnore]
        public decimal? LocalCharges { get; set; }
        [JsonIgnore]
        public decimal? ZonalCharges { get; set; }
        [JsonIgnore]
        public decimal? NationalCharges { get; set; }
        public int? HSNCodeId { get; set; }
        public string? HSNCode { get; set; }
        public int? TaxValueId { get; set; }
        public int? TaxTypeID { get; set; }
        public string? TaxValue { get; set; }
        public int? SizeId { get; set; }
        public string? SizeName { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Discount { get; set; }
        public int? Quantity { get; set; }
        [JsonInclude]
        public string? MarginIn { get; set; }
        [JsonInclude]
        public decimal? MarginCost { get; set; }
        [JsonInclude]
        public decimal? MarginPercentage { get; set; }
        public string? ProductImage { get; set; }
        public string? ExtraDetails { get; set; }
        public string? Status { get; set; }
        public bool? LiveStatus { get; set; }
    }
}
