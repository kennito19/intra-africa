using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class ProductView
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public string ProductGUID { get; set; }
        public string SellerId { get; set; }
        public int SellerProductId { get; set; }
        public string? UserId { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
