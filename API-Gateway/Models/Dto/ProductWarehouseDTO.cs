namespace API_Gateway.Models.Dto
{
    public class ProductWarehouseDTO
    {
        public int? Id { get; set; }
        public int WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public int Quantity { get; set; }
    }
}
