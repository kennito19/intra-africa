namespace API_Gateway.Models.Dto
{
    public class WarehouseReport
    {
        public int RowNumber { get; set; }
        public int RecordCount { get; set; }
        public string? UserID { get; set; }
        public string? DisplayName { get; set; }
        public string? OwnerName { get; set; }
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
        public string? GSTNo { get; set; }
        public string? GSTType { get; set; }
        public string? WareHouseName { get; set; }
        public string? ContactPersonName { get; set; }
        public string? ContactPersonMobileNo { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Landmark { get; set; }
        public string? Pincode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? RegisteredAddressLine1 { get; set; }
        public string? RegisteredAddressLine2 { get; set; }
        public string? RegisteredLandmark { get; set; }
        public string? RegisteredPincode { get; set; }
        public string? RegisteredCity { get; set; }
        public string? RegisteredState { get; set; }
        public string? RegisteredCountry { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
