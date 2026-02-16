namespace API_Gateway.Models.Dto
{
    public class ApprovedStatusDto
    {
        public string approvedRequest { get; set; }
        public string name { get; set; }
        public int orderId { get; set; }
        public int orderItemId { get; set; }
        public int actionId { get; set; }
        public int? exchangeProductId { get; set; }
        public int? exchangeSizeid { get; set; }
        public string? exchangeSize { get; set; }
        public decimal? exchangePriceDiff { get; set; }
    }
}
