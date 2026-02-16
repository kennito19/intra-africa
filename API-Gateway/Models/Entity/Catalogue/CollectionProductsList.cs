namespace API_Gateway.Models.Entity.Catalogue
{
    public class CollectionProductsList
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }

        public int? CollectionId { get; set; }
        public int? Id { get; set; }
        public string? Guid { get; set; }
        public bool? IsMasterProduct { get; set; }
        public int? ParentId { get; set; }
        public int? CategoryId { get; set; }
        public string ProductName { get; set; }
        public string? CustomeProductName { get; set; }
        public string? CompanySKUCode { get; set; }
        public string? Image1 { get; set; }
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
        public string? Status { get; set; }
        public bool? Live { get; set; } = null;
        public string? SellerSKU { get; set; }
        public string? ManufacturedDate { get; set; }
        public string? ExpiryDate { get; set; }
        //public bool? IsSizeWisePriceVariant { get; set; } = null;
        public string? ExtraDetails { get; set; }
        public string? SearchText { get; set; }
    }
}
