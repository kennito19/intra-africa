using System.Text.Json.Serialization;

namespace API_Gateway.Models.Entity.Catalogue
{
    public class SellerProduct
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int ProductID { get; set; }
        public int? ProductMasterId { get; set; }
        public string SellerID { get; set; }
        public int BrandID { get; set; }
        public string? SKUCode { get; set; }
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
        public string? searchText { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? ProductName { get; set; }
        public string? WeightSlabName { get; set; }
        public int? CategoryId { get; set; }
        public string? CompanySKUCode { get; set; }

    }
}
