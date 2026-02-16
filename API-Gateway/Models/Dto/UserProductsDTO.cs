namespace API_Gateway.Models.Dto
{
    public class UserProductsDTO
    {
        public int? Id { get; set; }
        public string? Guid { get; set; }
        public bool? IsMasterProduct { get; set; }
        public int? ParentId { get; set; }
        public int? CategoryId { get; set; }
        public int? AssiCategoryId { get; set; }
        public string? ProductName { get; set; }
        public string? CustomeProductName { get; set; }
        public string? CompanySKUCode { get; set; }
        public string? Image1 { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public int? Discount { get; set; }
        public int? Quantity { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryPathIds { get; set; }
        public string? CategoryPathNames { get; set; }
        public int? SellerProductId { get; set; }
        public string? SellerId { get; set; }
        public string? SellerName { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public int? TotalQty { get; set; }
        public string? Status { get; set; }
        public bool? Live { get; set; }
        public bool? IsWishlistProduct { get; set; }

        public string? ExtraDetails { get; set; }
        public int? TotalVariant { get; set; }
        public string? CreatedAt { get; set; }
        public string? ModifiedAt { get; set; }
        public string? Highlights { get; set; }
    }
}
