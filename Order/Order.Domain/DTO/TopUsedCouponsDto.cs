using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.DTO
{
    public class TopUsedCouponsDto
    {
        public string Coupon { get; set; }
        public string CoupontDetails { get; set; }
        public string? SellerID { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSell { get; set; }
    }
}
