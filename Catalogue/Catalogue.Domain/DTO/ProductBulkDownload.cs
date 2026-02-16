using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.DTO
{
    public class ProductBulkDownload
    {
        public string flag { get; set; }
        public int ProductID { get; set; }
        public int SellerProductId { get; set; }
        public string SellerSKUCode { get; set; }
        public string PackingLength { get; set; }
        public string PackingBreadth { get; set; }
        public string PackingHeight { get; set; }
        public string PackingWeight { get; set; }
        public string WeightSlabId { get; set; }
        public string HSNCode { get; set; }
        public string CustomeProductName { get; set; }
        public string CompanySKUCode { get; set; }
        public string Description { get; set; }
        public string Highlights { get; set; }
        public string Keywords { get; set; }
        public string ColorName { get; set; }
        public string MRP { get; set; }
        public string SellingPrice { get; set; }
        public string Discount { get; set; }
        public string Quantity { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseProductQty { get; set; }
        public string Image { get; set; }
        public string SizeName { get; set; }
        public int SpecID { get; set; }
        public string SpecName { get; set; }
        public string SpecTypeName { get; set; }
        public string SpecValueName { get; set; }
        public string Value { get; set; }
    }
}
