using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class ProductBulkDetails
    {
        public string? Category { get; set; }
        public string? AssignSpectoCat { get; set; }
        public string? AssignSizeValtoCat { get; set; }
        [JsonIgnore]
        public string? AssignSpecValtoCat { get; set; }
        public string? AssignTaxRateToHSNCode { get; set; }
        public string? WeightSlab { get; set; }
        public string? Color { get; set; }
        public string? SellerProduct { get; set; }
    }
}
