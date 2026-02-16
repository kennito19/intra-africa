using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class ProductCounts
    {
        public int Total { get; set; }
        public int Unique { get; set; }
        public int Active { get; set; }
        public int Inactivate { get; set; }
        public int InExisting { get; set; }
        public int InRequest { get; set; }
        public int InBulkUpload { get; set; }
        public int TotalStocks { get; set; }
        public int TotalActiveStocks { get; set; }
       // public int InOutOfStock { get; set; }
    }
}
