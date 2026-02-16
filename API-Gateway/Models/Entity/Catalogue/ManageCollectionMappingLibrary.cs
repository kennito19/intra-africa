namespace API_Gateway.Models.Entity.Catalogue
{
    public class ManageCollectionMappingLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public int ProductId { get; set; }
        public string Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public string? CollectionName { get; set; }
        public int? ParentId { get; set; }
        public int? CategoryId { get; set; }
        public int? AssiCategoryId { get; set; }

        public string? ProductName { get; set; }
        public string? CustomeProductName { get; set; }
        public string? CompanySKUCode { get; set; }
        public string? Image1 { get; set; }
        public string? ExtraDetails { get; set; }
        public decimal? MRP { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Discount { get; set; }
        public int? Quantity { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryPathIds { get; set; }
        public string? CategoryPathNames { get; set; }
        public int? SellerProductId { get; set; }
        public string? SellerId { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public int? TotalQty { get; set; }
        public string? ProductStatus { get; set; }
        public bool? ProductLive { get; set; }
        public int? TotalVariant { get; set; }
        public decimal? SaleMRP { get; set; }
        public decimal? SaleSellingPrice { get; set; }
        public decimal? SaleDiscount { get; set; }
        public string? SaleStatus { get; set; }
        public bool? IsSellerOptIn { get; set; }
    }
}
