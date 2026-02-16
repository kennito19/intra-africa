using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class checkAssignSpecToCategory
    {
        public bool? AllowSize { get; set; } = null;
        public bool? AllowColor { get; set; } = null;
        [JsonInclude]
        public bool? AllowSpecifications { get; set; } = null;
        [JsonIgnore]
        public bool? AllowExpiryDate { get; set; } = null;
        [JsonInclude]
        public bool? AllowPriceVariant { get; set; } = null;
        [JsonIgnore]
        public bool? AllowColorVariant { get; set; } = null;
    }
}
