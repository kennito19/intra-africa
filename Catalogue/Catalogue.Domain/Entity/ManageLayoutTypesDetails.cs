using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class ManageLayoutTypesDetails
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int LayoutId { get; set; }
        public int LayoutTypeId { get; set; }
        public string Name { get; set; }
        public string? SectionType { get; set; }
        public string? InnerColumns { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? Searchtext { get; set; }
        public string? LayoutName { get; set; }
        public string? LayoutTypeName { get; set; }
    }
}
