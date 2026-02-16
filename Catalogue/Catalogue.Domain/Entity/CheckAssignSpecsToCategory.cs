using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class CheckAssignSpecsToCategory
    {
        public bool? AllowSize { get; set; } = null;
        public bool? AllowColor { get; set; } = null;
        public bool? IsAllowDeleteSizeType { get; set; } = null;
        [JsonInclude]
        public bool? IsAllowDeleteSpecType { get; set; } = null;
        
        [JsonInclude]
        public bool? AllowSpecifications { get; set; } = null;
        [JsonIgnore]
        public bool? AllowExpiryDate { get; set; } = null;
        [JsonInclude]
        public bool? AllowPriceVariant { get; set; } = null;
        [JsonIgnore]
        public bool? AllowColorVariant { get; set; } = null;
        [JsonIgnore]
        public bool? AllowSizeVariant { get; set; } = null;
        public string? SizeIds { get; set; } = null;
        [JsonIgnore]
        public bool? AllowSpecVariant { get; set; } = null;
        [JsonInclude]
        public string? SpecValueIds { get; set; } = null;
        
    }
}
