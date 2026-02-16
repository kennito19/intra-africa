using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entity
{
    public class OrderWiseProductSeriesNo
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public string? SeriesNo { get; set; }
        public int? ProductID { get; set; }
        public int? SellerProductId { get; set; }
        public int? CategoryId { get; set; }
        public string? ProductName { get; set; }
        
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
