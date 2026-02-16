using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class AssignSizeValueToCategory
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int? AssignSpecID { get; set; }
        public int? SizeTypeID { get; set; }
        public int? SizeId { get; set; }
        public bool? IsAllowSizeInFilter { get; set; } = null;
        [JsonIgnore]
        public bool IsAllowSizeInVariant { get; set; } = false;
        public bool IsAllowSizeInComparision { get; set; } = false;
        public bool IsAllowSizeInTitle { get; set; } = false;
        public int? TitleSequenceOfSize { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? SizeName { get; set; }
        public string? SizeTypeName { get; set; }
        public string? Searchtext { get; set; }
        public int? CategoryId { get; set; }
        public bool? AllowSize { get; set; } = null;
        public bool? isPriceVariant { get; set; } = null;
        public bool? IsDeleted { get; set; } = null;
    }
}
