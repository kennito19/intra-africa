using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.DTO
{
    public class GSTReport
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public string? UserID { get; set; }
        public string? DisplayName { get; set; }
        public string? OwnerName { get; set; }
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
        public string? GSTNo { get; set; }
        public string? GSTType { get; set; }
        public string? RegisteredAddressLine1 { get; set; }
        public string? RegisteredAddressLine2 { get; set; }
        public string? RegisteredLandmark { get; set; }
        public string? RegisteredPincode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? GSTStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
