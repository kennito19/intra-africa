using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Domain.DTO
{
    public class ProductBulkDetails
    {
        public string? Category { get; set; }
        public string? AssignSpectoCat { get; set; }
        public string? AssignSizeValtoCat { get; set; }
        [JsonInclude]
        public string? AssignSpecValtoCat { get; set; }
        public string? AssignTaxRateToHSNCode { get; set; }
        public string? WeightSlab { get; set; }
        public string? Color { get; set; }
        public string? SellerProduct { get; set; }
    }
    public class ProductBulkDetailsParams
    {
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
    }
}
