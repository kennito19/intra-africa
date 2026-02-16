namespace API_Gateway.Models.Entity.Catalogue
{
    public class ProductChildListLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int? Id { get; set; }
        public string? Guid { get; set; }
        public bool? IsMasterProduct { get; set; } = false;
        public int? ParentId { get; set; }
        public int CategoryId { get; set; }
        public int AssiCategoryId { get; set; }
        public string ProductName { get; set; }
        public string CustomeProductName { get; set; }
        public string? CompanySKUCode { get; set; }
        public string? Image1 { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Discount { get; set; }
        public int Quantity { get; set; }
        public string? CategoryName { get; set; }
        public int? CategoryPathIds { get; set; }
        public string? CategoryPathNames { get; set; }
        public int? SellerProductId { get; set; }
        public string? SellerId { get; set; }
        public bool? IsExistingProduct { get; set; } = false;
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public int TotalQTY { get; set; }
        public string? Status { get; set; }
        public bool? Live { get; set; } = false;
        public int? TotalVariant { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string SearchText { get; set; }
        public int totalSellerCount { get; set; }
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; }
    }
}
