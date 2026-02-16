using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class ProductWarehouse
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int SellerProductID { get; set; }
        public int SellerWiseProductPriceMasterID { get; set; }
        public int ProductID { get; set; }
        public int WarehouseID { get; set; }
        public string? WarehouseName { get; set; }
        public int ProductQuantity { get; set; }
       
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; } = null;
        public string? ProductName { get; set; }
    }
}
