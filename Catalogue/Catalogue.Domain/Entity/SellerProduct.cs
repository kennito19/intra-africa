using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
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
        public bool? IsSizeWisePriceVariant { get; set; } = null;
        public bool? IsExistingProduct { get; set; } = null;
        public bool? Live { get; set; }
        public string? Status { get; set; }
        [JsonInclude]
        public DateTime? ManufacturedDate { get; set; }
        [JsonInclude]
        public DateTime? ExpiryDate { get; set; }
        public decimal? PackingLength { get; set; } = 0;
        public decimal? PackingBreadth { get; set; } = 0;
        public decimal? PackingHeight { get; set; } = 0;
        public decimal? PackingWeight { get; set; } = 0;
        public int? WeightSlabId { get; set; }

        public string? ExtraDetails { get; set; }
        [JsonInclude]
        public int? MOQ { get; set; } = null;
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; } = null;
        public string? ProductName { get; set; }
        public string? WeightSlabName { get; set; }
        public int? CategoryId { get; set; }
        public string? CompanySKUCode { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}
