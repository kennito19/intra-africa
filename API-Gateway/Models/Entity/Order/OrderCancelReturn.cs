namespace API_Gateway.Models.Entity.Order
{
    public class OrderCancelReturn
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }


        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public string? NewOrderNo { get; set; }
        public int Qty { get; set; }
        public int ActionID { get; set; }
        public int? ExchangeProductID { get; set; }
        public int? ExchangeSizeId { get; set; }
        public string? ExchangeSize { get; set; }
        public decimal? ExchangePriceDiff { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhoneNo { get; set; }
        public string UserEmail { get; set; }
        public string? UserGSTNo { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Landmark { get; set; }
        public int? Pincode { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string Issue { get; set; }
        public string Reason { get; set; }
        public string? Comment { get; set; }
        public string PaymentMode { get; set; }
        public string? Attachment { get; set; }
        public decimal? RefundAmount { get; set; }
        public string? RefundType { get; set; }
        public string? BankName { get; set; }
        public string? BankBranch { get; set; }
        public string? BankIFSCCode { get; set; }
        public string? BankAccountNo { get; set; }
        public string? AccountType { get; set; }
        public string? AccountHolderName { get; set; }
        public string? ApprovedByID { get; set; }
        public string? ApprovedByName { get; set; }
        public string? Status { get; set; }
        public string? RefundStatus { get; set; }



        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string? ReturnAction { get; set; }

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

        public string? OrderNo { get; set; }
        public string? OrderBy { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public string? OrderPaymentMode { get; set; }
        public int? BrandID { get; set; }
        public decimal? ItemTotalAmount { get; set; }
        public decimal? ItemSubTotal { get; set; }
        public string? ShipmentTrackingNo { get; set; }
        public string? TrackingNo { get; set; }
        public string? ShipmentInfo { get; set; }
    }
}
