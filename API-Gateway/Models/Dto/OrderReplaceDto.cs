namespace API_Gateway.Models.Dto
{
    public class OrderReplaceDto
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public int Qty { get; set; }
        public int ActionID { get; set; }
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
    }
}
