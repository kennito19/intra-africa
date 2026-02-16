using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class AssignBrandToSellerDTO
    {
        public int? Id { get; set; }
        public string? BrandName { get; set; }
        public string? SellerName { get; set; }
        public string SellerID { get; set; }
        public int BrandId { get; set; }
        public string? Status { get; set; }
        public string? BrandCertificate { get; set; }
        //[JsonIgnore]
        public IFormFile? FileName { get; set; }
    }
}
