using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class ProductPriceMaster
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int? ProductID { get; set; }
        public int? SellerProductID { get; set; }
        public decimal? MRP { get; set; } = 0;
        public decimal? SellingPrice { get; set; } = 0;
        public decimal? Discount { get; set; } = 0;
        public int? Quantity { get; set; }
        [JsonInclude]
        public string? MarginIn { get; set; }
        [JsonInclude]
        public decimal? MarginCost { get; set; } = 0;
        [JsonInclude]
        public decimal? MarginPercentage { get; set; } = 0;
        public int? SizeID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool? IsDeleted { get; set; } = null;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? SizeName { get; set; }
        public string? SizeTypeName { get; set; }
        
    }
}
