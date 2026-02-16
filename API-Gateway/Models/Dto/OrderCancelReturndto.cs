namespace API_Gateway.Models.Dto
{
    public class OrderCancelReturndto
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }


        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public string NewOrderNo { get; set; }
        public int Qty { get; set; }
        public int ActionID { get; set; }
        public int? ExchangeProductID { get; set; }
        public string? ExchangeSize { get; set; }
        public decimal? ExchangePriceDiff { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhoneNo { get; set; }
        public string UserEmail { get; set; }
        public string? UserGSTNo { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Landmark { get; set; }
        public int Pincode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
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
        public int? ApprovedByID { get; set; }
        public string? ApprovedByName { get; set; }
        public string Status { get; set; }
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
        public string? PickupContactPersonName { get; set; }
        public string? PickupContactPersonMobileNo { get; set; }
        public string? PickupContactPersonEmailID { get; set; }
        public string? PickupCompanyName { get; set; }
        public string? PickupAddressLine1 { get; set; }
        public string? PickupAddressLine2 { get; set; }
        public string? PickupLandmark { get; set; }
        public int? PickupPincode { get; set; } = 0;
        public string? PickupCity { get; set; }
        public string? PickupState { get; set; }
        public string? PickupCountry { get; set; }
        public string? CustomeProductName { get; set; }
        public int orderCancelReturnId { get; set; }
    }
}
