using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.DTO
{
    public class TopSellingProductsDto
    {
        public int ProductID { get; set; }
        public string? ProductGUID { get; set; }
        public string? SellerID { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSell { get; set; }
    }
}
