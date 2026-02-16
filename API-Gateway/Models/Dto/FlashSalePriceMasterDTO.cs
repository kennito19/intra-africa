namespace API_Gateway.Models.Dto
{
    public class FlashSalePriceMasterDTO
    {
        public int Id { get; set; }
        public int SellerProductId { get; set; }
        public int SellerWiseProductPriceMasterId { get; set; }
        public int CollectionId { get; set; }
        public int CollectionMappingId { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Discount { get; set; }
        public string? Status { get; set; }
        public bool? IsSellerOptIn { get; set; }
    }
}
