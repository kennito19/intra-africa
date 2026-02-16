namespace API_Gateway.Models.Dto
{
    public class OrderCancelDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderItemIds { get; set; }
        public string? NewOrderNo { get; set; }
        public int Qty { get; set; }
        public int ActionID { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhoneNo { get; set; }
        public string UserEmail { get; set; }
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
    }
}
