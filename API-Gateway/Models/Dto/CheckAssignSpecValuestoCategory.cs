using System.Text.Json.Serialization;

namespace API_Gateway.Models.Dto
{
    public class CheckAssignSpecValuestoCategory
    {
        [JsonIgnore]
        public bool? AllowSpecVariant { get; set; } = null;
        public string? SpecValueIds { get; set; } = null;
    }

    public class CheckSpecType
    {
        [JsonInclude]
        public bool? IsAllowDeleteSpecType { get; set; } = null;
    }
}
