using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entity
{
    public class OrderShipmentInfo
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }


        public string? SellerID { get; set; }
        public int OrderID { get; set; }
        public string OrderItemIDs { get; set; }
        public int PackageID { get; set; }
        public string PaymentMode { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal InvoiceCodCharges { get; set; }
        public string PackageDescription { get; set; }
        public bool IsShipmentInitiate { get; set; }
        public bool IsPaymentSuccess { get; set; }
        public string? CourierID { get; set; }
        public string? ServiceID { get; set; }
        public string? ServiceType { get; set; }
        public string PickupContactPersonName { get; set; }
        public string PickupContactPersonMobileNo { get; set; }
        public string PickupContactPersonEmailID { get; set; }
        public string? PickupCompanyName { get; set; }
        public string PickupAddressLine1 { get; set; }
        public string? PickupAddressLine2 { get; set; }
        public string? PickupLandmark { get; set; }
        public int PickupPincode { get; set; }
        public string PickupCity { get; set; }
        public string PickupState { get; set; }
        public string? PickupCountry { get; set; }
        public string? PickupTaxNo { get; set; }
        public string DropContactPersonName { get; set; }
        public string DropContactPersonMobileNo { get; set; }
        public string DropContactPersonEmailID { get; set; }
        public string? DropCompanyName { get; set; }
        public string DropAddressLine1 { get; set; }
        public string? DropAddressLine2 { get; set; }
        public string? DropLandmark { get; set; }
        public int DropPincode { get; set; }
        public string DropCity { get; set; }
        public string DropState { get; set; }
        public string? DropCountry { get; set; }
        public string? DropTaxNo { get; set; }
        public string? ShipmentID { get; set; }
        public string? ShipmentOrderID { get; set; }
        public string? ShippingPartner { get; set; }
        public string? CourierName { get; set; }
        public decimal? ShippingAmountFromPartner { get; set; }
        public string? AwbNo { get; set; }
        public bool IsShipmentSheduledByAdmin { get; set; }
        public string? PickupLocationID { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ForwardLable { get; set; }
        public string? ReturnLable { get; set; }

        public string? ShipmentTrackingNo { get; set; }
        public string? TrackingNo { get; set; }
        public string? ShipmentInfo { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
