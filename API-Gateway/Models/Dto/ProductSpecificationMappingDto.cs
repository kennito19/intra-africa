using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class ProductSpecificationMappingDto
    {
        public int Id { get; set; }
        public int? SpecId { get; set; }
        public int? SpecTypeId { get; set; }
        public int? SpecValueId { get; set; }
        public string? Value { get; set; }
        [JsonIgnore]
        public string? FileName { get; set; }
        public string? SpecificationName { get; set; }
        public string? SpecificationTypeName { get; set; }

    }
}
