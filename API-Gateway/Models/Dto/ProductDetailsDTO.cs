namespace API_Gateway.Models.Dto
{
    public class ProductDetailsDTO
    {
        public int? ProductId { get; set; }
        public int? SellerProductId { get; set; }
        public string SellerId { get; set; }
        public string ProductGuid { get; set; }
        public string ProductName { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public string ProductImage { get; set; }
        public int? CategoryId { get; set; }
        public int? Quantity { get; set; }
    }
}
