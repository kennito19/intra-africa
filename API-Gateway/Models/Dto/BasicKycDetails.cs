namespace API_Gateway.Models.Dto
{
    public class BasicKycDetails
    {
        public int? Id { get; set; }
        public string? UserID { get; set; }
        public string? KYCFor { get; set; }
        public string? DisplayName { get; set; }
        public string? OwnerName { get; set; }
        public string? ContactPersonName { get; set; }
        public string? ContactPersonMobileNo { get; set; }
        public string? PanCardNo { get; set; }
        public string? NameOnPanCard { get; set; }
        public string? DateOfBirth { get; set; }
        public string? AadharCardNo { get; set; }
        public bool IsUserWithGST { get; set; } = false;
        public string? TypeOfCompany { get; set; }
        public string? CompanyRegistrationNo { get; set; }
        public string? BussinessType { get; set; }
        public string? MSMENo { get; set; }
        public string? Logo { get; set; }
        public string? DigitalSign { get; set; }
        public string? ShipmentBy { get; set; }
        public int? ShipmentChargesPaidBy { get; set; } = null;
        public string? Note { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
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
