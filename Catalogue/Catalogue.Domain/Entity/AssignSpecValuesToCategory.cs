using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class AssignSpecValuesToCategory
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int? AssignSpecID { get; set; }
        public int? SpecID { get; set; }
        public int? SpecTypeID { get; set; }
        public int? SpecTypeValueID { get; set; }
        public bool? IsAllowSpecInFilter { get; set; } = null;
        [JsonIgnore]
        public bool IsAllowSpecInVariant { get; set; } = false;
        public bool IsAllowSpecInComparision { get; set; } = false;
        public bool IsAllowSpecInTitle { get; set; } = false;
        public bool IsAllowMultipleSelection { get; set; } = false;
        public int? TitleSequenceOfSpecification { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? FieldType { get; set; }
        public string? SpecificationName { get; set; }
        public string? SpecificationTypeName { get; set; }
        public string? SpecificationTypeValueName { get; set; }
        public int? CategoryId { get; set; }
        public bool? AllowSpecifications { get; set; } = null;
        public bool? IsDeleted { get; set; } = null;
    }
}
