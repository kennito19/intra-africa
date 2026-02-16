namespace API_Gateway.Models.Dto
{
    public class SizeWiseWarehouse
    {
        public int? ProductWarehouseId { get; set; }
        public int? ProductId { get; set; }
        public int? SellerProductId { get; set; }
        public int? WarehouseId { get; set; }
        public int? SizeId { get; set; }
        public int? Quantity { get; set; }
        public string? WarehouseName { get; set; }
    }
}
