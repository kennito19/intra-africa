using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Entity
{
    public class Cart
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string SessionId { get; set; }
        public int SellerProductMasterId { get; set; }
        public int? SizeId { get; set; }
        public int Quantity { get; set; }
        public decimal TempMRP { get; set; }
        public decimal TempSellingPrice { get; set; }
        public decimal TempDiscount { get; set; }
        public decimal SubTotal { get; set; }
        public int? WarrantyId { get; set; }
        public string? Fullname { get; set; }
        public string? Username { get; set; }
        public string? Phone { get; set; }
        public string? UserStatus { get; set; }
        public string? searchText { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
