using System.Text.Json.Serialization;

namespace API_Gateway.Models.Entity.Catalogue
{
    public class ProductSpecificationMapping
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int? CatId { get; set; }
        public int? ProductID { get; set; }
        public int? SpecId { get; set; }
        public int? SpecTypeId { get; set; }
        public int? SpecValueId { get; set; }
        public string? Value { get; set; }
        [JsonIgnore]
        public string? FileName { get; set; }
        public string? SpecificationName { get; set; }
        public string? SpecificationTypeName { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
