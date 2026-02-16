using System.Text.Json.Serialization;

namespace API_Gateway.Models.Entity.Catalogue
{
    public class WeightSlabLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public string? WeightSlab { get; set; }
        [JsonInclude]
        public decimal? LocalCharges { get; set; } = 0;
        [JsonInclude]
        public decimal? ZonalCharges { get; set; } = 0;
        [JsonInclude]
        public decimal? NationalCharges { get; set; } = 0;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? Searchtext { get; set; }
    }
}
