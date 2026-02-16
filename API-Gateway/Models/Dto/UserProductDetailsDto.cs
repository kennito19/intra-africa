using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class UserProductDetailsDto
    {
        public int Id { get; set; }
        public string? Guid { get; set; }
        public int? MasterProductId { get; set; }
        public int? SellerProductId { get; set; }
        public int? PriceMasterId { get; set; }
        public string? SellerID { get; set; }
        public int? BrandID { get; set; }
        public int? CategoryId { get; set; }
        public int? AssiCategoryId { get; set; }
        public int? HSNCodeId { get; set; }
        public int? TaxValueId { get; set; }
        [JsonInclude]
        public bool? IsSizeWisePriceVariant { get; set; }
        public bool? IsExistingProduct { get; set; }
        public string? ProductName { get; set; }
        public string? CustomeProductName { get; set; }
        public string? CompanySKUCode { get; set; }
        public string? SKUCode { get; set; }
        public string? Description { get; set; }
        public string? Highlights { get; set; }
        public bool? Live { get; set; }
        public string? Status { get; set; }
        [JsonIgnore]
        public int? MOQ { get; set; }
        [JsonIgnore]
        public DateTime? ManufacturedDate { get; set; }
        [JsonIgnore]
        public DateTime? ExpiryDate { get; set; }
        public string? ExtraDetails { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Discount { get; set; }
        public string? MarginIn { get; set; }
        public decimal? MarginCost { get; set; }
        public decimal? MarginPercentage { get; set; }
        public int? Quantity { get; set; }
        public int? SizeId { get; set; }
        public string? SizeName { get; set; }
        public int? SizeTypeId { get; set; }
        public string? SizeTypeName { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryPathIds { get; set; }
        public string? CategoryPathNames { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaTitle { get; set; }
    }
}
