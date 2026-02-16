using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class ProductHomePageSectionLibrary
    {
        public int id { get; set; }
        public string Guid { get; set; }
        public bool isMasterProduct { get; set; }
        public int parentId { get; set; }
        public int categoryId { get; set; }
        public int assiCategoryId { get; set; }
        public string productName { get; set; }
        public string customeProductName { get; set; }
        public string companySkuCode { get; set; }
        public string image1 { get; set; }
        public decimal mrp { get; set; }
        public decimal sellingPrice { get; set; }
        public decimal discount { get; set; }
        public int Quantity { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? modifiedAt { get; set; }
        public string categoryName { get; set; }
        public string categoryPathIds { get; set; }
        public string categoryPathNames { get; set; }
        public int sellerProductId { get; set; }
        public string sellerId { get; set; }
        public int brandId { get; set; }
        public string brandName { get; set; }
        public string status { get; set; }
        public bool live { get; set; }
        public string extraDetails { get; set; }
        public int totalVariant { get; set; }

        public int? topProduct { get; set; }
        public string productIds { get; set; }
    }
}
