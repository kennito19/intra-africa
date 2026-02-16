using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.DTO
{
    public class ProductBulkDownloadForStock
    {
        public string flag { get; set; }
        public int ProductID { get; set; }
        public int SellerProductId { get; set; }
        public string SellerSKUCode { get; set; }
        public string CustomeProductName { get; set; }
        public string CompanySKUCode { get; set; }
        public string MRP { get; set; }
        public string SellingPrice { get; set; }
        public string Discount { get; set; }
        public string Quantity { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseProductQty { get; set; }
        public string SizeId { get; set; }
        public string SizeName { get; set; }

    }
}
