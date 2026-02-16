namespace API_Gateway.Models.Dto
{
    public class ProductBulkDownloadForStock
    {
        public string flag { get; set; }
        public int ProductID { get; set; }
        public int SellerProductId { get; set; }
        public string SellerSKUCode { get; set; }
        public string CustomeProductName { get; set; }
        public string CompanySKUCode { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Discount { get; set; }
        public int? Quantity { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseProductQty { get; set; }
        public int? SizeId { get; set; }
        public string? SizeName { get; set; }
    }
}
