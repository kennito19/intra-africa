namespace API_Gateway.Models.Dto
{
    public class ProductComparision
    {
        public string ProductGuid { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public string Description { get; set; }
        public string Highlights { get; set; }
        public string SellerProductID { get; set; }
        public string SellerID { get; set; }
        public string BrandID { get; set; }
        public string MRP { get; set; }
        public string SellingPrice { get; set; }
        public string Discount { get; set; }
        public string ProductImage { get; set; }
        public string ProductSpec { get; set; }
        public string Color { get; set; }
        public string ProductSize { get; set; }
        public string ProductVarint { get; set; }
    }
    public class ProductCompareBrand
    {
        public string BrandName { get; set; }
        public string BrandId { get; set; }

    }
    public class ProductCompareBrandProduct
    {
        public string ProductName { get; set; }
        public string SellerProductId { get; set; }
        public string ProductId { get; set; }

    }
}
