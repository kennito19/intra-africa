using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.DTO
{
    public class ProductExtraDetailsDto
    {
        public string? SellerId { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SellerStatus { get; set; }
        public string? DisplayName { get; set; }
        public string? DigitalSign { get; set; }
        public string? ShipmentBy { get; set; }
        public string? ShipmentChargesPaidBy { get; set; }
        public string? ShipmentChargesPaidByName { get; set; }
        public string? KycStatus { get; set; }
        public string? TradeName { get; set; }
        public string? LegalName { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? BrandStatus { get; set; }
        public string? BrandLogo { get; set; }
        public string? AssignBrandStatus { get; set; }
        public string? Mode { get; set; }
    }
}
