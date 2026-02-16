using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class ProductPriceDTO
    {
        public int Id { get; set; }
        public int SellerProductId { get; set; } // bind productId with it
        public decimal? MRP { get; set; } = 0;
        public decimal? SellingPrice { get; set; } = 0;
        public decimal? Discount { get; set; } = 0;
        public int? Quantity { get; set; }
        [JsonInclude]
        public string? MarginIn { get; set; }
        [JsonInclude]
        public decimal? MarginCost { get; set; } = 0;
        [JsonInclude]
        public decimal? MarginPercentage { get; set; } = 0;
        public int? SizeID { get; set; } = null;
        public string? SizeName { get; set; }
        public string? SizeTypeName { get; set; } = null;
        public IEnumerable<ProductWarehouseDTO>? ProductWarehouses { get; set; }
    }
}
