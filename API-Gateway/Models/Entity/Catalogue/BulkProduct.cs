namespace API_Gateway.Models.Entity.Catalogue
{
    public class BulkProduct
    {
        public int CategoryId { get; set; }
        public string? SellerId { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }

        public IFormFile? File { get; set; }
    }
}
