using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class TaxMapping
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int TaxId { get; set; }
        public int TaxTypeId { get; set; }
        public int? TaxValueId { get; set; }
        public string TaxMapBy { get; set; }
        public string? SpecificState { get; set; }
        public int? SpecificTaxTypeId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? Searchtext { get; set; }
        public string? Tax { get; set; }
        public string? TaxType { get; set; }
        public string? SpecificTaxType { get; set; }
    }
}
