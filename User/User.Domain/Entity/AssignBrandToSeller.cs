using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Entity
{
    public class AssignBrandToSeller
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public string? BrandName { get; set; }
        public string? SellerName { get; set; }
        public string SellerID { get; set; }
        public int BrandId { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsBrandDeleted { get; set; } = false;
        public string? BrandCertificate { get; set; }
        public string? Logo { get; set; }
        public string? Searchtext { get; set; }
        public string? BrandStatus { get; set; }
        public string? BrandGUID { get; set; }
        public string? BrandDescription { get; set; }
    }
}
