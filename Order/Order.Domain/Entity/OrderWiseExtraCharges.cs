using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entity
{
    public class OrderWiseExtraCharges
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int OrderItemID { get; set; }

        public string? searchText { get; set; }
        public string ChargesType { get; set; }
        public string ChargesPaidBy { get; set; }
        public string ChargesIn { get; set; }
        public decimal? ChargesValueInPercentage { get; set; }
        public decimal ChargesValueInAmount { get; set; }
        public decimal? ChargesMaxAmount { get; set; }
        public decimal TaxOnChargesAmount { get; set; }
        public decimal ChargesAmountWithoutTax { get; set; }
        public decimal TotalCharges { get; set; }




        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
