using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class CheckAssignSizeValuestoCategory
    {
        [JsonIgnore]
        public bool? AllowSizeVariant { get; set; } = null;
        public string? SizeIds { get; set; } = null;
    }

    public class CheckSizeType
    {
        [JsonInclude]
        public bool? IsAllowDeleteSizeType { get; set; } = null;
        
    }
}
