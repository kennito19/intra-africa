namespace API_Gateway.Models.Dto
{
    public class CommissionReports
    {
        public string? OrderNo { get; set; }
        public string? OrderBy { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? OrderItemId { get; set; }
        public int? OrderId { get; set; }
        public string? SellerId { get; set; }
        public int? BrandId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductGUID { get; set; }
        public int? SellerProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductSKUCode { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Qty { get; set; }
        public decimal? ItemSubTotal { get; set; }
        public decimal? ItemTotalAmount { get; set; }
        public string? CommissionIn { get; set; }
        public decimal? CommissionRate { get; set; }
        public decimal? CommissionAmount { get; set; }
        public string? orderStatus { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
