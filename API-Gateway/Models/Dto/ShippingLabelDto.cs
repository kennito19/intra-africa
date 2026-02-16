namespace API_Gateway.Models.Dto
{
    public class ShippingLabelDto
    {
        public int? OrderID { get; set; }
        public string? OrderNo { get; set; }
        public string? SellerTradeName { get; set; }
        public string? SellerLegalName { get; set; }
        public string? SellerPickupAddressLine1 { get; set; }
        public string? SellerPickupAddressLine2 { get; set; }
        public string? SellerPickupLandmark { get; set; }
        public int? SellerPickupPincode { get; set; }
        public string? SellerPickupCity { get; set; }
        public string? SellerPickupState { get; set; }
        public string? SellerPickupCountry { get; set; }
        public string? SellerPickupTaxNo { get; set; }
        public string? SellerPickupContactPersonName { get; set; }
        public string? SellerPickupContactPersonMobileNo { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public decimal? Weight { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? DropContactPersonName { get; set; }
        public string? DropContactPersonMobileNo { get; set; }
        public string? DropContactPersonEmailID { get; set; }
        public string? DropCompanyName { get; set; }
        public string? DropAddressLine1 { get; set; }
        public string? DropAddressLine2 { get; set; }
        public string? DropLandmark { get; set; }
        public int? DropPincode { get; set; }
        public string? DropCity { get; set; }
        public string? DropState { get; set; }
        public string? DropCountry { get; set; }
        public string? DropTaxNo { get; set; }
        public string? PaymentMode { get; set; }
        public string? AwbNo { get; set; }
        public string? ShippingPartner { get; set; }
        public string? CourierName { get; set; }
        public int? NoOfPackage { get; set; }
    }
}
