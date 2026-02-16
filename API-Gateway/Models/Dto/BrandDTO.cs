using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class BrandDTO
    {
        public int? ID { get; set; }
        public string? GUID { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Logo { get; set; }
        [JsonIgnore]
        public IFormFile? FileName { get; set; }
    }
}
