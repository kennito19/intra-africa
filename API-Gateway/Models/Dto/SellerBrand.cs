namespace API_Gateway.Models.Dto
{
    public class SellerBrand
    {
        public int Id { get; set; }
        public int? AssignBrandToSeller { get; set; }
        public string Name { get; set; }
        public string SellerDisplayName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Logo { get; set; }
        public string? BrandCertificate { get; set; }
        public string? SellerId { get; set; }
        public IFormFile? LogoFile { get; set; }
        public IFormFile? BrandCertificateFile { get; set; }
    }
}
