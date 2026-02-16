namespace API_Gateway.Models.Dto
{
    public class CheckCompanySKUCodeDto
    {
        public int? CategoryId { get; set; } = 0;
        public int? BrandID { get; set; } = 0;
        public string? CompanySKUCode { get; set; } = null;
        public int? ProductId { get; set; } = null;
    }

    public class CheckSellerSKUCodeDto
    {
        public int? CategoryId { get; set; } = 0;
        public string? SellerID { get; set; } = null;
        public int? BrandID { get; set; } = 0;
        public string? SellerSKUCode { get; set; } = null; 
        public int? SellerProductId { get; set; } = null;
    }
}
