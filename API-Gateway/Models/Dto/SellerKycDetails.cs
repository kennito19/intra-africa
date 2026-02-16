using API_Gateway.Models.Entity.User;
using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class SellerKycDetails
    {
        #region Kyc Details
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
        public string? CompanyRegitrationNo { get; set; }
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
        [JsonInclude]
        public int? ShipmentChargesPaidBy { get; set; } = null;
        [JsonInclude]
        public string? ShipmentChargesPaidByName { get; set; } = null;
        public string? Note { get; set; }
        public string? Status { get; set; }
        public string? SellerStatus { get; set; }
        #endregion

        #region SellerDetails
        public string SellerId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        #endregion

        #region GStInfo
        public IEnumerable<GSTInfo>? gSTInfos { get; set; }
        #endregion

        #region WareHouse
        public IEnumerable<Warehouse>? wareHouses { get; set; }
        #endregion

        #region Brands
        public IEnumerable<AssignBrandToSeller>? Brands { get; set; }
        #endregion
    }
}
