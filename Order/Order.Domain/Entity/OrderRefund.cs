using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entity
{
    public class OrderRefund
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }

        public int OrderCancelReturnID{ get; set; }
        public int OrderID { get; set; }
        public int OrderItemID { get; set; }
        public decimal RefundAmount{ get; set; }
        public string TransactionID{ get; set; }
        public string? Comment{ get; set; }
        public string Status{ get; set; }


        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? searchText { get; set; }
        public string? OrderNo { get; set; }
        public string? UserName { get; set; }
        public string? UserPhoneNo { get; set; }
        public string? UserEmail { get; set; }
        public int? ProductID { get; set; }
        public string? ProductGUID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductSKUCode { get; set; }
    }
}
