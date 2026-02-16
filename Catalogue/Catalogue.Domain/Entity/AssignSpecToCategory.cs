using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class AssignSpecToCategory
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int? CategoryID { get; set; }
        public string? Guid { get; set; }
        public bool IsAllowSize { get; set; } = false;
        [JsonInclude]
        public bool IsAllowPriceVariant { get; set; } = false;
        [JsonInclude]
        public bool IsAllowSpecifications { get; set; } = false;
        [JsonIgnore]
        public bool IsAllowExpiryDates { get; set; } = false;
        public bool IsAllowColors { get; set; } = false;
        public bool IsAllowColorsInFilter { get; set; } = false;
        [JsonIgnore]
        public bool IsAllowColorsInVariant { get; set; } = false;
        public bool IsAllowColorsInComparision { get; set; } = false;
        public bool IsAllowColorsInTitle { get; set; } = false;
        public int? TitleSequenceOfColor { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryPathNames { get; set; }
        public string? Searchtext { get; set; }
    }
}
