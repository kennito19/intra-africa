namespace API_Gateway.Models.Dto
{
    public class OrderTrackDetailDTO
    {
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
    }
}
