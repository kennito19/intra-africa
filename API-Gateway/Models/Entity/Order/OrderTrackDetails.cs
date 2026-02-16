namespace API_Gateway.Models.Entity.Order
{
    public class OrderTrackDetails
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public string OrderStage { get; set; }
        public string OrderStatus { get; set; }
        public string OrderTrackDetail { get; set; }
        public DateTime TrackDate { get; set; }
        public string? RejectionType { get; set; }
        public string? RejectionBy { get; set; }
        public string? ReasonForRejection { get; set; }
        public string? Comment { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
