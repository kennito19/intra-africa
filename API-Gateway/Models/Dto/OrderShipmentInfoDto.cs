namespace API_Gateway.Models.Dto
{
    public class OrderShipmentInfoDto
    {
        public int Id { get; set; }
        public string? SellerID { get; set; }
        public int OrderID { get; set; }
        public string OrderItemIDs { get; set; }
        public int PackageID { get; set; }
        public int WarehouseId { get; set; }
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

    }
}
