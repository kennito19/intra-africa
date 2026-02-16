using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.DTO
{
    public class UserDetailsDTO
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? UserType { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserStatus { get; set; }
        public string? ProfileImage { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public bool? IsPhoneConfirmed { get; set; }
        public bool? IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        public int? KYCDetailsId { get; set; }
        public string? KYCFor { get; set; }
        public string? DisplayName { get; set; }
        public string? OwnerName { get; set; }
        public string? ContactPersonName { get; set; }
        public string? ContactPersonMobileNo { get; set; }
        public string? PanCardNo { get; set; }
        public string? NameOnPanCard { get; set; }
        public string? DateOfBirth { get; set; }
        public string? AadharCardNo { get; set; }
        public bool? IsUserWithGST { get; set; }
        public string? TypeOfCompany { get; set; }
        public string? CompanyRegistrationNo { get; set; }
        public string? BussinessType { get; set; }
        public string? MSMENo { get; set; }
        public string? AccountNo { get; set; }
        public string? AccountHolderName { get; set; }
        public string? BankName { get; set; }
        public string? AccountType { get; set; }
        public string? IFSCCode { get; set; }
        public string? Logo { get; set; }
        public string? DigitalSign { get; set; }
        public string? CancelCheque { get; set; }
        public string? PanCardDoc { get; set; }
        public string? MSMEDoc { get; set; }
        public string? AadharCardFrontDoc { get; set; }
        public string? AadharCardBackDoc { get; set; }
        public string? ShipmentBy { get; set; }
        public int? ShipmentChargesPaidBy { get; set; }
        public string? Note { get; set; }
        public string? KycStatus { get; set; }
        public string? GSTInfoDetails { get; set; }
        public string? WarehouseDetails { get; set; }
        public string? AddressDetails { get; set; }
        public string? Wishlist { get; set; }
        public string? SellerBrand { get; set; }
    }
}
