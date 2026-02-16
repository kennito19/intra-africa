namespace API_Gateway.Models.Dto
{
    public class TopSellingProductsDto
    {
        public int ProductID { get; set; }
        public string? ProductGUID { get; set; }
        public string? SellerID { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSell { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public string? ProductSKU { get; set; }

    }
    public class TopSellingSellersDto
    {
        public string SellerID { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSell { get; set; }
        public string? SellerName { get; set; }
        public string? SellerLogo { get; set; }
    }

    public class TopSellingBrandsDto
    {
        public int BrandID { get; set; }
        public string? SellerID { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSell { get; set; }
        public string? BrandName { get; set; }
        public string? BrandLogo { get; set; }
    }
    
    public class TopUsedCouponsDto
    {
        public string Coupon { get; set; }
        public string CoupontDetails { get; set; }
        public string? SellerID { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSell { get; set; }
    }
}
