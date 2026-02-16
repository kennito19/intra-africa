using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.DTO
{
    public class SizeWiseWarehouse
    {
        public int? ProductWarehouseId { get; set; }
        public int? ProductId { get; set; }
        public int? SellerProductId { get; set; }
        public int? WarehouseId { get; set; }
        public int? SizeId { get; set; }
        public int? Quantity { get; set; }
    }
}
