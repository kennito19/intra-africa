namespace API_Gateway.Models.Dto
{
    public class OrderReturnReuqestdto
    {
        public int ReturnRequestId { get; set; }
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public string? ApprovedByID { get; set; }
        public string? ApprovedByName { get; set; }
        public string? Status { get; set; }
        public string? RefundStatus { get; set; }

        // this details being use in to Return Shipment info. 
        public string? DropContactPersonName { get; set; }
        public string? DropContactPersonMobileNo { get; set; }
        public string? DropContactPersonEmailID { get; set; }
        public string? DropCompanyName { get; set; }
        public string? DropAddressLine1 { get; set; }
        public string? DropAddressLine2 { get; set; }
        public string? DropLandmark { get; set; }
        public int? DropPincode { get; set; } = 0;
        public string? DropCity { get; set; }
        public string? DropState { get; set; }
        public string? DropCountry { get; set; }
        public string? CustomeProductName { get; set; }
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
        public string? SellerID { get; set; }
        public int? WarehouseId { get; set; }
        public decimal? RefundAmount { get; set; }
        public string? ShipmentTrackingNo { get; set; }
        public string? TrackingNo { get; set; }
        public string? ShipmentInfo { get; set; }
    }

    public class CancelReturnReuqestdto
    {
        public int ReturnRequestId { get; set; }
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public string? ApprovedByID { get; set; }
        public string? ApprovedByName { get; set; }
    }
}
