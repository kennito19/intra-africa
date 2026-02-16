namespace API_Gateway.Models.Entity.Catalogue
{
    public class Products
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public string? Guid { get; set; }
        public bool IsMasterProduct { get; set; } = false;
        public int? ParentId { get; set; }
        public int? CategoryId { get; set; }
        public int? AssiCategoryId { get; set; }
        public int? TaxValueId { get; set; }
        public int? HSNCodeId { get; set; }
        public string? ProductName { get; set; }
        public string? CustomeProductName { get; set; }
        public string? CompanySKUCode { get; set; }
        public string? Description { get; set; }
        public string? Highlights { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaTitle { get; set; }
        public string? Keywords { get; set; }
        public decimal? ProductLength { get; set; } = 0;
        public decimal? ProductBreadth { get; set; } = 0;
        public decimal? ProductWeight { get; set; } = 0;
        public decimal? ProductHeight { get; set; } = 0;
        public string? searchText { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string? CategoryName { get; set; }
        public string? CategoryPathIds { get; set; }
        public string? CategoryPathNames { get; set; }
        public string? HSNCode { get; set; }
        public string? TaxName { get; set; }
        public string? TaxValue { get; set; }
    }
}