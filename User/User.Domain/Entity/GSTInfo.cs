using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Entity
{
    public class GSTInfo

    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public string? UserID { get; set; }
        public string? GSTNo { get; set; }
        public string? LegalName { get; set; }
        public string? TradeName { get; set; }
        public string? GSTType { get; set; }
        public string? GSTDoc { get; set; }
        public string? RegisteredAddressLine1 { get; set; }
        public string? RegisteredAddressLine2 { get; set; }
        public string? RegisteredLandmark { get; set; }
        public string? RegisteredPincode { get; set; }
        public int? RegisteredStateId { get; set; }
        public int? RegisteredCityId { get; set; }
        public int? RegisteredCountryId { get; set; }
        public string? TCSNo { get; set; }
        public string? Status { get; set; }
        public bool? IsHeadOffice { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public string? searchText { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? CountryName { get; set; }
        public string? StateName { get; set; }
        public string? CityName { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserStatus { get; set; }
        public string? ProfileImage { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public bool? IsPhoneConfirmed { get; set; }
    }
}
