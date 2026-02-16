namespace API_Gateway.Models.Dto
{
    public class GetProductDTO
    {
        public int? productId { get; set; }
        public string? productGuid { get; set; }
        public bool IsMasterProduct { get; set; }
        public int? ParentId { get; set; }

        public string? CategoryName { get; set; }
        public string? CategoryPathName { get; set; }
        public int CategoryId { get; set; }
        public int AssiCategoryId { get; set; }
        public int TaxValueId { get; set; }
        public int HSNCodeId { get; set; }
        public string ProductName { get; set; }
        public string? CustomeProductName { get; set; }
        public string CompanySKUCode { get; set; }
        public string Description { get; set; }
        public string? Highlights { get; set; }
        public string? Keywords { get; set; }
        public int BrandID { get; set; }
        public string? BrandName { get; set; }
        public decimal? ProductLength { get; set; } = 0;
        public decimal? ProductBreadth { get; set; } = 0;
        public decimal ProductWeight { get; set; } = 0;
        public decimal? ProductHeight { get; set; } = 0;
        public string? WeightSlab { get; set; } = null;
        public string? TaxRate { get; set; } = null;
        public string? TaxValue { get; set; } = null;
        public string? HSNCode { get; set; } = null;
        public string? CategoryPathIds { get; set; } = null;
        public bool? IsWishlistProduct { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaTitle { get; set; }

        //public string?  ShipmentBy { get; set; } = null;
        //public string?  ShipmentPaidBy { get; set; } = null;




        public ReturnPolicyDTO? ReturnPolicy { get; set; }
        public IEnumerable<SellerProductDTO> SellerProducts { get; set; }
        public IEnumerable<ProductSpecificationMappingDto> ProductSpecificationsMapp { get; set; }
        public IEnumerable<ProductColorDTO> ProductColorMapping { get; set; }
        //public IEnumerable<ProductVideoLinkDTO> ProductVideoLinks { get; set; }
        public IEnumerable<ProductImageDTO> ProductImage { get; set; }
    }
}
