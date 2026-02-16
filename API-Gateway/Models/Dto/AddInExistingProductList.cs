namespace API_Gateway.Models.Dto
{
    public class AddInExistingProductList
    {
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int? Id { get; set; }
        public int? ProductID { get; set; }
        public int? ProductMasterID { get; set; }
        public string? ProductGuid { get; set; }
        public int? SellerProductId { get; set; }
        public int? CategoryId { get; set; }
        public string? SellerID { get; set; }
        public int? BrandID { get; set; }
        public int? AssiCategoryId { get; set; }
        public int? TaxValueId { get; set; }
        public int? HSNCodeId { get; set; }
        public string? ProductName { get; set; }
        public string? CustomeProductName { get; set; }
        public string? CompanySKUCode { get; set; }
        public string? SellerSKUCode { get; set; }
        public string? CategoryPathids { get; set; }
        public string? CategoryPathName { get; set; }
        public string? CategoryName { get; set; }
        public bool? IsExistingProduct { get; set; }
        public bool? IsSizeWisePriceVariant { get; set; }
        public string? ExtraDetails { get; set; }
        public string? ProductImage { get; set; }
        public string? Status { get; set; }
        public bool? Live { get; set; }
        public string? BrandName { get; set; }
        public string? SizeType { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? ProductVariants { get; set; }
    }
}
