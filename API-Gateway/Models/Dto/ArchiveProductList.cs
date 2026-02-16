namespace API_Gateway.Models.Dto
{
    public class ArchiveProductList
    {
        public int? RowNumber { get; set; }
        public int? PageCount { get; set; }
        public int? RecordCount { get; set; }
        public int? ProductId { get; set; }
        public string? Guid { get; set; }
        public int? ProductMasterId { get; set; }
        public int? CategoryId { get; set; }
        public int? AssiCategoryId { get; set; }
        public int? TaxValueId { get; set; }
        public int? HSNCodeId { get; set; }
        public string? ProductName { get; set; }
        public string? CustomeProductName { get; set; }
        public string? CompanySKUCode { get; set; }
        public string? SellerSKUCode { get; set; }
        public int? SellerProductId { get; set; }
        public int? BrandID { get; set; }
        public string? SellerID { get; set; }
        public string? Status { get; set; }
        public string? ExtraDetails { get; set; }
        public int? WeightSlabId { get; set; }
        public int? PriceMasterId { get; set; }
        public string? MRP { get; set; }
        public string? SellingPrice { get; set; }
        public string? Discount { get; set; }
        public string? Quantity { get; set; }
        public string? ProductImage { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryPathIds { get; set; }
        public string? CategoryPathNames { get; set; }
        public string? BrandName { get; set; }
        public string? SellerName { get; set; }
        public string? SearchText { get; set; }
    }
}
