using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entity
{
    public class OrdersCount
    {
        public int TotalOrders { get; set; }
        public int Pending { get; set; }
        public int Confirmed { get; set; }
        public int PartialConfirmed { get; set; }
        public int Packed { get; set; }
        public int PartialPacked { get; set; }
        public int Shipped { get; set; }
        public int PartialShipped { get; set; }
        public int Delivered { get; set; }
        public int PartialDelivered { get; set; }
        public int Cancelled { get; set; }
        public int Replaced { get; set; }
        public int Returned { get; set; }
        public int Exchanged { get; set; }
    }
}
