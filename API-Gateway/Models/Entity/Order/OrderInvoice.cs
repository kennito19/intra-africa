namespace API_Gateway.Models.Entity.Order
{
    public class OrderInvoice
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }

        public int PackageID { get; set; }
        public int OrderID { get; set; }
        public string OrderItemIDs { get; set; }
        public string InvoiceNo { get; set; }
        public string? SellerTradeName { get; set; }
        public string? SellerLegalName { get; set; }
        public string? SellerGSTNo { get; set; }
        public string? SellerRegisteredAddressLine1 { get; set; }
        public string? SellerRegisteredAddressLine2 { get; set; }
        public string? SellerRegisteredLandmark { get; set; }
        public int? SellerRegisteredPincode { get; set; }
        public string? SellerRegisteredCity { get; set; }
        public string? SellerRegisteredState { get; set; }
        public string? SellerRegisteredCountry { get; set; }
        public string? SellerPickupAddressLine1 { get; set; }
        public string? SellerPickupAddressLine2 { get; set; }
        public string? SellerPickupLandmark { get; set; }
        public int? SellerPickupPincode { get; set; }
        public string? SellerPickupCity { get; set; }
        public string? SellerPickupState { get; set; }
        public string? SellerPickupCountry { get; set; }
        public string? SellerPickupContactPersonName { get; set; }
        public string? SellerPickupContactPersonMobileNo { get; set; }
        public string? SellerPickupTaxNo { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal InvoiceCodCharges { get; set; }
        public string Status { get; set; }


        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? OrderNo { get; set; }
        public string? PackageNo { get; set; }
        public int? NoOfPackage { get; set; }
        public string? SellerId { get; set; }
    }
}
