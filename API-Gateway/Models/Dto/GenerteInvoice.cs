using DocumentFormat.OpenXml.Spreadsheet;
using Nancy.Json;
using Newtonsoft.Json.Linq;

namespace API_Gateway.Models.Dto
{
    public class GenerteInvoice
    {

        public InvoiceDetails InvoiceData { get; set; }
    }

    public class InvoiceDetails
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string? OrderItemIDs { get; set; }
        
        public string? SellerTradeName { get; set; }
        public string? SellerLegalName { get; set; }
        public string RegisteredAddressLine1 { get; set; }
        public string RegisteredAddressLine2 { get; set; }
        public string RegisteredLendmark { get; set; }
        public string RegisteredCity { get; set; }
        public string RegisteredState { get; set; }
        public string RegisteredCountry { get; set; }
        public string RegisteredPincode { get; set; }
        public string RegisteredGSTNo { get; set; }

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


        public string UserName { get; set; }
        public string BillToAddressLine1 { get; set; }
        public string BillToAddressLine2 { get; set; }
        public string BillToLendmark { get; set; }
        public string BillToCity { get; set; }
        public string BillToState { get; set; }
        public string BillToCountry { get; set; }
        public string BillToPincode { get; set; }
        public string BillToGSTNo { get; set; }
        public string BillToMobileNo { get; set; }
        public string BillToEmailID { get; set; }
        public string? BillToTaxNo { get; set; }

        public string ShipToAddressLine1 { get; set; }
        public string ShipToAddressLine2 { get; set; }
        public string ShipToLendmark { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToCountry { get; set; }
        public string ShipToPincode { get; set; }
        public string ShipToGSTNo { get; set; }
        public string ShipToMobileNo { get; set; }
        public string? ShipToTaxNo { get; set; }

        public InvoiceItemDetails? ItemDetails { get; set; }

        public bool? IsCouponApplied { get; set; }
        public string? Coupon { get; set; }
        public decimal? CoupontDiscount { get; set; }
        public string? CoupontDetails { get; set; }
        public string? PaymentMode { get; set; }
        public decimal? TotalCodCharges { get; set; }
        public string? CodCharges { get; set; }
        public string? ShippingCharges { get; set; }
        public decimal? TotalShippingCharges { get; set; }
        public string? ExtraCharges { get; set; }
        public decimal? TotalExtraCharges { get; set; }
        public decimal? InvoiceAmount { get; set; }
    }

    public class InvoiceItemDetails
    {
        public List<InvoiceItems> ProductItems { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class InvoiceItems
    {
        public int ProductId { get; set; }
        public int SellerProductId { get; set; }
        public string? SellerId { get; set; }
        public int BrandId { get; set; }
        public int SizeId { get; set; }
        public string? ProductName { get; set; }
        public string? SKUCode { get; set; }
        public string? Brand { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? HSNCode { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public int? OrderTaxRateId { get; set; }
        public string? OrderTaxRate { get; set; }
        public string? WarrantyTitle { get; set; }
        public decimal? ActualWarrantyPrice { get; set; }
        public int? WarrantyQty { get; set; }
        public int? WarrantyYear { get; set; }
        public decimal? TotalActualWarrantyPrice { get; set; }
        public string? Taxes { get; set; }
        public bool? IsCouponApplied { get; set; }
        public string? Coupon { get; set; }
        public decimal? CoupontDiscount { get; set; }
        public string? CoupontDetails { get; set; }
        public string? ProductSeriesNo { get; set; }
    }

}
