

namespace Catalogue.Domain.Entity
{
    public class UserProductList
    {
        public int? RowNumber { get; set; }
        public int? RecordCount { get; set; }
        public int? PageCount { get; set; }
        public char? flag { get; set; }
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
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public int? TotalQty { get; set; }
        public string? Status { get; set; }
        public bool? Live { get; set; }
        public string? ExtraDetails { get; set; }
        public int? TotalVariant { get; set; }
        public string? CreatedAt { get; set; }
        public string? ModifiedAt { get; set; }
        public int? F_CategoryId { get; set; }
        public string? F_CategoryName { get; set; }
        public int? F_ProductCount { get; set; }
        public int? F_BrandId { get; set; }
        public string? F_BrandName { get; set; }
        public int? F_SizeID { get; set; }
        public string? F_Size { get; set; }
        public int? F_Quantity { get; set; }
        public int? F_ColorID { get; set; }
        public string? F_ColorName { get; set; }
        public string? F_ColorCode { get; set; }
        public decimal? MinSellingPrice { get; set; }
        public decimal? MaxSellingPrice { get; set; }
        public int? FilterTypeId { get; set; }
        public string? FilterTypeName { get; set; } = string.Empty;
        public int? FilterValueId { get; set; }
        public string? FilterValueName { get; set; }
        public string? Highlights { get; set; }

    }
}
