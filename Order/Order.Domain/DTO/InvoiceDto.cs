using Order.Domain.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.DTO
{
    public class InvoiceDto
    {
        public int? Id { get; set; }
        public int? OrderID { get; set; }
        public string? OrderNo { get; set; }
        public string? OrderItemIDs { get; set; }
        public string? InvoiceNo { get; set; }
        public string? SellerTradeName { get; set; }
        public string? SellerLegalName { get; set; }
        public string? SellerGSTNo { get; set; }
        public string? SellerRegisteredAddressLine1 { get; set; }
        public string? SellerRegisteredAddressLine2 { get; set; }
        public string? SellerRegisteredLandmark { get; set; }
        public string? SellerRegisteredPincode { get; set; }
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
        public decimal? InvoiceAmount { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? SellerID { get; set; }
        public int? BrandID { get; set; }
        public int? ProductID { get; set; }
        public int? SellerProductID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductSKUCode { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Discount { get; set; }
        public int? Qty { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? PriceTypeID { get; set; }
        public string? PriceType { get; set; }
        public int? SizeID { get; set; }
        public string? SizeValue { get; set; }
        public bool? IsCouponApplied { get; set; }
        public string? Coupon { get; set; }
        public decimal? CoupontDiscount { get; set; }
        public string? CoupontDetails { get; set; }
        public string? ShippingZone { get; set; }
        public string? ShippingCharge { get; set; }
        public string? ShippingChargePaidBy { get; set; }
        public decimal? SubTotal { get; set; }
        public string? Status { get; set; }
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
        public decimal? TaxOnShipping { get; set; }
        public string? OrderTaxRate { get; set; }
        public int? OrderTaxRateId { get; set; }
        public string? HSNCode { get; set; }
        public string? PaymentMode { get; set; }
        public string? AwbNo { get; set; }
        public string? ShippingPartner { get; set; }
        public string? CourierName { get; set; }
        public int? NoOfPackage { get; set; }
        public decimal? Weight { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string? WarrantyTitle { get; set; }
        public decimal? ActualWarrantyPrice { get; set; }
        public int? WarrantyQty { get; set; }
        public int? WarrantyYear { get; set; }
        public decimal? TotalActualWarrantyPrice { get; set; }
        public string? ExtrachargesName { get; set; }
        public decimal? TotalExtracharges { get; set; }
        public decimal? CODCharge { get; set; }
        public string? ProductSeriesNos { get; set; }
    }
}
