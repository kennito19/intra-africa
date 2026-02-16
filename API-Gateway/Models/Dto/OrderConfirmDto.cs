namespace API_Gateway.Models.Dto
{
    public class OrderConfirmDto
    {
        public int OrderId { get; set; }
        public int OrderItemId { get; set; }
        public int warehouseId { get; set; }
        public int? productWarehouseId { get; set; } = null;
    }
}
