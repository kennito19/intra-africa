using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catalogue.Domain.DTO
{
    public class ProductReport
    {
        public string PathNames { get; set; }
        public string TaxName { get; set; }
        public string HSNCode { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CustomeProductName { get; set; }
        public string CompanySKUCode { get; set; }
        public string SellerSKU { get; set; }
        public string PackingLength { get; set; }
        public string PackingBreadth { get; set; }
        public string PackingHeight { get; set; }
        public string PackingWeight { get; set; }
        public string SellerDisplayName { get; set; }
        public string SellerLegalName { get; set; }
        public string SellerTradeName { get; set; }
        public string SellerEmail { get; set; }
        public string SellerPhoneNo { get; set; }
        public string ShipmentChargesPaidBy { get; set; }
        public string SellerStatus { get; set; }
        public string BrandName { get; set; }
        public string Color { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public string SizeType { get; set; }
        public string Size { get; set; }
        public string WarehouseName { get; set; }
        public int WarehouseQty { get; set; }
         public string Status { get; set; }
        public bool Live { get; set; }

    }
    public class ProductDetailsReport
    {
        public int ProductId { get; set; }
        public string? ProductGuid { get; set; }
        public string? ProductName { get; set; }
        public string? ProductSKU { get; set; }
        public string? ProductImage { get; set; }
    }
}
